// Local .NET Framework bridge. Uses the installed vendor client and normal login.
// Credentials arrive over stdin, never command-line arguments or persistent files.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class NativeReportHost {
    private static string suite;
    private static string mapPath;
    private static string secret = "";
    private static string activeToken = "";
    private static readonly List<ICommunicationObject> channels = new List<ICommunicationObject>();
    private static Assembly wcf;
    private static Assembly cross;
    private static Assembly core;

    [STAThread]
    public static int Main(string[] args) {
        try {
            Console.InputEncoding = new System.Text.UTF8Encoding(false);
            Console.OutputEncoding = new System.Text.UTF8Encoding(false);
            if (args.Length != 2) throw new ArgumentException("Expected installed SuiteG6 path and fixed report map path.");
            suite = Path.GetFullPath(args[0]); mapPath = Path.GetFullPath(args[1]);
            AppDomain.CurrentDomain.AssemblyResolve += ResolveInstalled;
            return Run();
        } catch (Exception error) {
            while ((error is TargetInvocationException || error is AggregateException) && error.InnerException != null) error = error.InnerException;
            string message = error.Message;
            if (secret.Length > 0) message = message.Replace(secret, "[redacted]");
            if (activeToken.Length > 0) message = message.Replace(activeToken, "[redacted]");
            // No stack, request echo, headers, credentials, or auth token in diagnostics.
            Console.WriteLine("{\"ok\":false,\"error\":" + Quote(message) + ",\"error_type\":" + Quote(error.GetType().Name) + "}");
            return 1;
        } finally {
            foreach (ICommunicationObject channel in channels) {
                try { channel.Close(TimeSpan.FromSeconds(2)); } catch { channel.Abort(); }
            }
            secret = ""; activeToken = "";
        }
    }

    private static Assembly ResolveInstalled(object sender, ResolveEventArgs args) {
        string name = new AssemblyName(args.Name).Name + ".dll";
        if (Path.GetFileName(name) != name) return null;
        foreach (string directory in new string[] {suite, Path.Combine(suite,"Modulos")}) {
            string candidate = Path.Combine(directory, name);
            if (File.Exists(candidate)) return Assembly.LoadFrom(candidate);
        }
        return null;
    }

    private static string Quote(string value) {
        return "\"" + value.Replace("\\","\\\\").Replace("\"","\\\"").Replace("\r","\\r").Replace("\n","\\n").Replace("\t","\\t") + "\"";
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int Run() {
        string input = Console.In.ReadToEnd();
        if (input.Length > 1024*1024) throw new ArgumentException("Request exceeds 1 MiB.");
        JObject request = JObject.Parse(input);
        secret = (string)request["password"] ?? "";
        wcf = Assembly.LoadFrom(Path.Combine(suite,"DigisatServer.Distributed.Wcf.Main.dll"));
        cross = Assembly.LoadFrom(Path.Combine(suite,"DigisatServer.Infrastructure.Crosscutting.dll"));
        core = Assembly.LoadFrom(Path.Combine(suite,"DigisatServer.Distributed.Wcf.Core.dll"));
        string operation = (string)request["operation"];
        if (operation == "describe") return Describe(request);
        int port = (int?)request["port"] ?? 12221;
        if (port < 1024 || port > 65535) throw new ArgumentException("Invalid local service port.");
        object endpoint = Activator.CreateInstance(cross.GetType("DigisatServer.Infrastructure.Crosscutting.Model.EndPointWcf",true),new object[] {new IPEndPoint(IPAddress.Loopback,port)});
        if (operation == "probe") {
            object client = CreateChannel("IInformacaoModuleService", endpoint, null);
            object info = Invoke(client,"IInformacaoModuleService","ObterInformacoes",new object[0]);
            object release = info.GetType().GetProperty("Release").GetValue(info,null);
            Write(new {ok=true, release=Convert.ToString(release,CultureInfo.InvariantCulture)}); return 0;
        }
        string company=(string)request["company_id"], username=(string)request["username"];
        if (String.IsNullOrWhiteSpace(company) || String.IsNullOrWhiteSpace(username) || secret.Length==0) throw new ArgumentException("A SuiteG6 company and normal user login are required.");
        object credentials=CreateCredentials(company,username,secret,"");
        object auth=CreateChannel("IAutenticacaoModuleService",endpoint,credentials);
        activeToken=Convert.ToString(Invoke(auth,"IAutenticacaoModuleService","ExecutarLogOnAsync",new object[0]),CultureInfo.InvariantCulture);
        if (String.IsNullOrWhiteSpace(activeToken)) throw new InvalidOperationException("Native authentication returned no session token.");
        credentials=CreateCredentials(company,username,secret,activeToken);
        try {
            if (operation=="login") { Write(new {ok=true,authenticated=true}); return 0; }
            if (operation!="invoke") throw new ArgumentException("Unsupported operation.");
            JObject map=JObject.Parse(File.ReadAllText(mapPath));
            string reportId=(string)request["report_id"];
            JArray calls=(JArray)map["reports"][reportId]["calls"];
            int callIndex=(int?)request["call_index"] ?? 0;
            if (calls==null || callIndex<0 || callIndex>=calls.Count) throw new ArgumentException("Report call is not in the fixed allowlist.");
            JObject call=(JObject)calls[callIndex];
            string contract=(string)call["contract"], methodName=(string)call["method"];
            Type contractType=Contract(contract);
            MethodInfo method=contractType.GetMethod(methodName);
            if (method==null) throw new MissingMethodException("Report operation is absent in the installed service contract.");
            JObject arguments=request["arguments"] as JObject ?? new JObject();
            ParameterInfo[] parameters=method.GetParameters();
            HashSet<string> valid=new HashSet<string>(parameters.Select(p=>p.Name));
            if (arguments.Properties().Any(p=>!valid.Contains(p.Name))) throw new ArgumentException("Unknown native report parameter.");
            JsonSerializer serializer=JsonSerializer.Create(new JsonSerializerSettings {TypeNameHandling=TypeNameHandling.None,MissingMemberHandling=MissingMemberHandling.Error,Culture=CultureInfo.InvariantCulture,DateTimeZoneHandling=DateTimeZoneHandling.Unspecified});
            object[] values=new object[parameters.Length];
            for(int i=0;i<parameters.Length;i++) {
                JToken supplied=arguments[parameters[i].Name];
                if (supplied==null) throw new ArgumentException("Missing native report parameter: "+parameters[i].Name);
                values[i]=supplied.ToObject(parameters[i].ParameterType,serializer);
            }
            object client=CreateChannel(contract,endpoint,credentials);
            object result=Await(method.Invoke(client,values));
            bool empty=result==null || result.GetType().Name.EndsWith("DtoNull",StringComparison.Ordinal) || result.GetType().Name.EndsWith("NullDto",StringComparison.Ordinal);
            Write(new {ok=true,report_id=reportId,call_index=callIndex,is_null=empty,data=result}); return 0;
        } finally {
            // Only remove this helper's own token; never force another user's logout.
            try {
                object logout=CreateChannel("IAutenticacaoModuleService",endpoint,credentials);
                Invoke(logout,"IAutenticacaoModuleService","ExecutarLogOffAsync",new object[0]);
            } catch { /* The server owns expiration if the service connection failed. */ }
        }
    }

    private static Type Contract(string name) {
        Type[] matches=wcf.GetTypes().Where(t=>t.IsInterface && t.Name==name && t.Namespace!=null && t.Namespace.StartsWith("DigisatServer.Distributed.Wcf.Main.Contracts.",StringComparison.Ordinal)).ToArray();
        if (matches.Length!=1) throw new ArgumentException("Native report contract is missing or ambiguous.");
        return matches[0];
    }

    private static object CreateCredentials(string company,string user,string password,string token) {
        Type factory=wcf.GetType("DigisatServer.Distributed.Wcf.Main.Clients.ChannelFactoryProtegido",true);
        Type credentials=factory.GetConstructors().Single(c=>c.GetParameters().Length==3).GetParameters()[1].ParameterType;
        return Activator.CreateInstance(credentials,new object[] {company,user,password,token});
    }

    private static object CreateChannel(string contract,object endpoint,object credentials) {
        string typeName="DigisatServer.Distributed.Wcf.Main.Clients.ChannelFactory"+(credentials==null?"Basico":"Protegido");
        object factory;
        if (credentials==null) factory=Activator.CreateInstance(wcf.GetType(typeName,true),new object[] {endpoint});
        else {
            Type encryption=core.GetType("DigisatServer.Distributed.Wcf.Core.Encodings.ContentEncryptionType",true);
            factory=Activator.CreateInstance(wcf.GetType(typeName,true),new object[] {endpoint,credentials,Enum.Parse(encryption,"All")});
        }
        object client=factory.GetType().GetMethod("CriarCanal").MakeGenericMethod(Contract(contract)).Invoke(factory,new object[0]);
        ICommunicationObject channel=client as ICommunicationObject;
        if(channel!=null)channels.Add(channel);
        IContextChannel context=client as IContextChannel;
        if(context!=null)context.OperationTimeout=TimeSpan.FromSeconds(20);
        return client;
    }

    private static object Invoke(object client,string contract,string method,object[] values) {
        return Await(Contract(contract).GetMethod(method).Invoke(client,values));
    }

    private static object Await(object value) {
        Task task=value as Task;
        if(task==null)return value;
        if(!task.Wait(TimeSpan.FromSeconds(25)))throw new TimeoutException("Native report service timed out.");
        PropertyInfo result=task.GetType().GetProperty("Result");
        return result==null?null:result.GetValue(task,null);
    }

    private static int Describe(JObject request) {
        JObject map=JObject.Parse(File.ReadAllText(mapPath));
        JObject reports=(JObject)map["reports"];
        Assembly reportAssembly=Assembly.LoadFrom(Path.Combine(suite,"Modulos/Modulo.Relatorios.dll"));
        Type[] reportTypes;bool completeTypes=true;
        try { reportTypes=reportAssembly.GetTypes(); }
        catch(ReflectionTypeLoadException error) { reportTypes=error.Types.Where(t=>t!=null).ToArray();completeTypes=false; }
        foreach(JProperty report in reports.Properties()) {
            string className=(string)report.Value["class_name"];
            string[] classes=reportTypes.Where(t=>t.Name==className).Select(t=>t.FullName).ToArray();
            report.Value["installed_report_types"]=JArray.FromObject(classes);
            report.Value["class_presence"]=classes.Length>0?"present":completeTypes?"absent":"unknown";
            foreach(JObject call in (JArray)report.Value["calls"]) {
                try {
                    Type contract=Contract((string)call["contract"]);
                    MethodInfo method=contract.GetMethod((string)call["method"]);
                    if(method==null)throw new MissingMethodException();
                    call["parameters"]=JArray.FromObject(method.GetParameters().Select(p=>new {name=p.Name,type=p.ParameterType.FullName,kind=Kind(p.ParameterType),properties=Properties(p.ParameterType),choices=EnumChoices(p.ParameterType)}));
                    call["available"]=true;
                } catch(Exception error) { call["available"]=false;call["error"]=error.GetType().Name; }
            }
        }
        map["installed_assemblies"]=JArray.FromObject(new Assembly[] {wcf,cross,core,reportAssembly}.Select(a=>new {name=a.GetName().Name,mvid=a.ManifestModule.ModuleVersionId.ToString(),sha256=FileHash(a.Location),file_version=System.Diagnostics.FileVersionInfo.GetVersionInfo(a.Location).FileVersion}));
        map["report_type_inventory_complete"]=completeTypes;
        map["value_choices"]=ValueChoices();
        map["verified_at"]=DateTime.UtcNow.ToString("o");
        Write(new {ok=true,map=map});return 0;
    }

    private static object Properties(Type type) {
        if(Kind(type)!="object")return null;
        return type.GetProperties().Where(p=>p.CanWrite && p.GetIndexParameters().Length==0).Select(p=>new {name=p.Name,type=p.PropertyType.FullName,kind=Kind(p.PropertyType),choices=EnumChoices(p.PropertyType)}).Take(80).ToArray();
    }
    private static string Kind(Type type) {
        if(type==typeof(string))return "text";
        if(type==typeof(bool))return "boolean";
        if(type==typeof(DateTime))return "datetime";
        if(type.IsEnum)return "enum";
        if(typeof(IEnumerable).IsAssignableFrom(type))return "array";
        if(type.IsPrimitive || type==typeof(decimal))return "number";
        return "object";
    }
    private static void Write(object data) {
        string json=JsonConvert.SerializeObject(data,new JsonSerializerSettings {ReferenceLoopHandling=ReferenceLoopHandling.Ignore,TypeNameHandling=TypeNameHandling.None,DateTimeZoneHandling=DateTimeZoneHandling.Unspecified,MaxDepth=64});
        if(json.Length>16*1024*1024)throw new InvalidOperationException("Report exceeds the 16 MiB response limit; reduce the selected period.");
        Console.WriteLine(json);
    }
    private static object EnumChoices(Type type) {
        if(!type.IsEnum)return null;
        return Enum.GetValues(type).Cast<object>().Select(v=>new {value=Convert.ToInt64(v),label=Enum.GetName(type,v)}).ToArray();
    }
    private static string FileHash(string path) {
        using(var hash=System.Security.Cryptography.SHA256.Create())using(var stream=File.OpenRead(path)) {
            return BitConverter.ToString(hash.ComputeHash(stream)).Replace("-","").ToLowerInvariant();
        }
    }
    private static JObject ValueChoices() {
        JObject result=new JObject();
        foreach(string name in new string[] {"OrigensPesquisaMovimentacao","SituacoesMovimentacao"}) {
            try {
                Type type=cross.GetType("DigisatServer.Infrastructure.Crosscutting.Model."+name+"."+name,true);
                PropertyInfo property=type.GetProperty("Dados",BindingFlags.Public|BindingFlags.Static);
                object value=property!=null?property.GetValue(null,null):type.GetField("Dados",BindingFlags.Public|BindingFlags.Static).GetValue(null);
                var list=new List<object>();
                foreach(object entry in (IEnumerable)value) {
                    Type item=entry.GetType();
                    int code=Convert.ToInt32(item.GetProperty("Codigo").GetValue(entry,null));
                    string description=Convert.ToString(item.GetProperty("Descricao").GetValue(entry,null));
                    list.Add(new {label=description,value=new {Codigo=code,Descricao=description},interfaces=item.GetInterfaces().Select(i=>i.Name).ToArray()});
                }
                result[name]=JArray.FromObject(list);
            }catch(Exception){result[name]=new JArray();}
        }
        return result;
    }
}
