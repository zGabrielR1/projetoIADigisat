using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using Akka.Actor;
using DigisatServer.Infrastructure.Crosscutting;
using DigisatServer.Infrastructure.Crosscutting.Config;
using DigisatServer.Infrastructure.Crosscutting.Constant;
using DigisatServer.Infrastructure.Crosscutting.Dto;
using DigisatServer.Infrastructure.Crosscutting.Event;
using DigisatServer.Infrastructure.Crosscutting.File;
using DigisatServer.Infrastructure.Crosscutting.Helper;
using DigisatServer.Infrastructure.Crosscutting.RegistroSistema.File;
using DigisatServer.Infrastructure.Data.Core;
using DigisatServer.Infrastructure.Data.Core.Mappings;
using MongoDB.Driver;
using NLog;

namespace Sincronizador
{
	// Token: 0x02000005 RID: 5
	public class SincronizacaoClientActor : ReceiveActor
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002219 File Offset: 0x00000419
		private IEnumerable<string> ColecoesASincronizar
		{
			get
			{
				return new List<string> { "Movimentacoes", "RegistrosPafEcf", "TransacoesPbm", "LastTs" };
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000B RID: 11 RVA: 0x0000224C File Offset: 0x0000044C
		private string[] ColecoesLimparAntesInserir
		{
			get
			{
				return new string[] { "Contas", "UnidadesMedida", "Usuarios", "Perfis", "OperacoesFiscais", "RelacoesEspecieConta", "PromocoesScanntech" };
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000228C File Offset: 0x0000048C
		public SincronizacaoClientActor(RepositorySincronizacao repository, Logger logger)
		{
			XmlFileFactory.SetCurrent(new XmlConfiguracaoSistemaG6FileFactory());
			this._repository = repository;
			this._logger = logger;
			this.Initializing();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022B4 File Offset: 0x000004B4
		private void Initializing()
		{
			base.SetReceiveTimeout(new TimeSpan?(TimeSpan.FromSeconds(15.0)));
			base.Receive<SincronizacaoConnectRequest>(delegate(SincronizacaoConnectRequest x)
			{
				base.Become(new Action(this.Connecting));
			}, null);
			base.Receive<SincronizacaoReConnectRequest>(delegate(SincronizacaoReConnectRequest x)
			{
				base.Become(new Action(this.ReConnect));
			}, null);
			this._contador = 0;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002308 File Offset: 0x00000508
		private void Connecting()
		{
			try
			{
				EntityMongoMapServidor.Create().IniciarMongoDb().Initialize();
			}
			catch (Exception ex)
			{
				this.GravarLog(ex.Message);
				this.RegistrarLog(ex.Message, true);
			}
			base.Receive<ReceiveTimeout>(delegate(ReceiveTimeout timeout)
			{
				this.RegistrarLog("(Connect) - TimeOut Sincronização", false);
				this.Create(new SincronizacaoConnectRequest());
			}, null);
			base.Receive<SincronizacaoConnectResponse>(delegate(SincronizacaoConnectResponse ack)
			{
				base.Become(new Action(this.Connected));
				this._server.Tell(new SincronizacaoDataRequest(), null);
			}, null);
			this.Create(new SincronizacaoConnectRequest());
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002384 File Offset: 0x00000584
		private void Create(IConnectRequest connectRequest)
		{
			try
			{
				ConfiguracaoClient configuracaoClient = XmlFileFactory.Create().Read<ConfiguracaoClient>();
				if (configuracaoClient == null || !configuracaoClient.IpValido)
				{
					throw new Exception("Configurações inválidas. Verifique!");
				}
				this._server = UntypedActor.Context.ActorSelection(new AkkaServerAddress("DigisatServerAkka", configuracaoClient.Ip, 12222, "SincronizacaoServerActor").ToString());
				this.Connect(connectRequest);
			}
			catch (Exception ex)
			{
				this.GravarLog(ex.Message);
				this.RegistrarLog(ex.Message, true);
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002420 File Offset: 0x00000620
		private void Connect(IConnectRequest connectRequest)
		{
			this._server.Tell(connectRequest, null);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002430 File Offset: 0x00000630
		private void Connected()
		{
			this.RegistrarLog("(Connected) - Sincronização", false);
			this.GravarLog("(Connected) - Sincronização");
			base.SetReceiveTimeout(null);
			UntypedActor.Context.Watch(base.Sender);
			this._tail = OpLog.LoadFrom(this._repository.Database).Tail(Operations.InsertUpdateAndDelete);
			foreach (string text in this.ColecoesASincronizar)
			{
				CollectionReplicator.From(string.Format("from_{0}_client", text), this._tail, this._repository.Database.DatabaseNamespace.DatabaseName, text).To(new ICanTell[] { this._server }, base.Sender, string.Format("from_{0}_server", text), true);
			}
			this._tail.Run();
			this._repository.IniciarLastTs();
			base.ReceiveAsync<SincronizacaoDataResponse>(delegate(SincronizacaoDataResponse ack)
			{
				this.RegistrarLog("SincronizacaoDataResponse", false);
				this.GravarLog("SincronizacaoDataResponse");
				if (!ack.Data.Any<string>())
				{
					return Task.CompletedTask;
				}
				this.RegistrarLog("Possui dados", false);
				this.GravarLog("Possui dados");
				if (ack.DropCollection && this.ColecoesLimparAntesInserir.Any<string>((string x) => x.Equals(ack.CollectionName)))
				{
					this._repository.Drop(ack.CollectionName);
				}
				this.RegistrarLog(string.Format("Coleção: {0}", ack.CollectionName), false);
				this.GravarLog(string.Format("Coleção: {0}", ack.CollectionName));
				return this._repository.BulkUpdateAsync(ack.CollectionName, ack.Data).ContinueWith(delegate(Task x)
				{
					this.RegistrarLog("-----", false);
					this.GravarLog("-----");
					if (!x.IsFaulted)
					{
						return;
					}
					if (x.Exception.InnerException is MongoWaitQueueFullException)
					{
						this.RegistrarLog(x.Exception.Message, true);
						this.GravarLog(x.Exception.Message);
						throw x.Exception.InnerException;
					}
					SincronizacaoClientActor <>4__this = this;
					string text2 = "Falha ao persistir parte da carga completa collection: {0} \r\n Exception: {1}";
					object collectionName = ack.CollectionName;
					AggregateException exception = x.Exception;
					object obj;
					if (exception == null)
					{
						obj = null;
					}
					else
					{
						Exception lastException = exception.GetLastException();
						obj = ((lastException != null) ? lastException.Message : null);
					}
					<>4__this.RegistrarLog(string.Format(text2, collectionName, obj), true);
					SincronizacaoClientActor <>4__this2 = this;
					string text3 = "Falha ao persistir parte da carga completa collection: {0} \r\n Exception: {1}";
					object collectionName2 = ack.CollectionName;
					AggregateException exception2 = x.Exception;
					object obj2;
					if (exception2 == null)
					{
						obj2 = null;
					}
					else
					{
						Exception lastException2 = exception2.GetLastException();
						obj2 = ((lastException2 != null) ? lastException2.Message : null);
					}
					<>4__this2.GravarLog(string.Format(text3, collectionName2, obj2));
					SincronizacaoClientActor.RestartService("SincronizadorDigisat", 10000);
				});
			}, null);
			base.ReceiveAsync<OpLogInsertRequest>(new Func<OpLogInsertRequest, Task>(this.HandleOpLogRequest), null);
			base.Receive<Terminated>(delegate(Terminated ack)
			{
				OpLogTail tail = this._tail;
				if (tail != null)
				{
					tail.Dispose();
				}
				this.RegistrarLog("(Disconnected) - Sincronização", false);
				this.GravarLog("(Disconnected) - Sincronização");
				UntypedActor.Context.System.Terminate();
			}, null);
			base.Receive<AntesRestaurarBaseDadosEvent>(delegate(AntesRestaurarBaseDadosEvent request)
			{
				base.Become(new Action(this.Stoped));
			}, null);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002580 File Offset: 0x00000780
		private void Stoped()
		{
			this.RegistrarLog("Antes de restaurar base de dados", false);
			OpLogTail tail = this._tail;
			if (tail != null)
			{
				tail.Dispose();
			}
			base.Receive<RestaurouBackupRequest>(delegate(RestaurouBackupRequest request)
			{
				this.RegistrarLog("Restaurou base de dados", false);
				try
				{
					this._repository.DropAll(ColecoesSincronizarServidorCliente.Colecoes());
				}
				catch (MongoWaitQueueFullException ex)
				{
					this.RegistrarLog(ex.Message, true);
					throw;
				}
				catch (Exception ex2)
				{
					this.RegistrarLog(ex2.Message, true);
				}
				this.RegistrarLog("(Disconnected) - Sincronização", false);
				UntypedActor.Context.System.Terminate();
			}, null);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000025B2 File Offset: 0x000007B2
		private void ReConnect()
		{
			base.Receive<ReceiveTimeout>(delegate(ReceiveTimeout timeout)
			{
				this.RegistrarLog("(Reconnect) - TimeOut Sincronização", false);
				this._contador++;
				if (this._contador == 10)
				{
					this._contador = 0;
					this.GravarLog("Reiniciando o serviço de sincronização");
					SincronizacaoClientActor.RestartService("SincronizadorDigisat", 10000);
					return;
				}
				this.Create(new SincronizacaoReConnectRequest());
			}, null);
			base.Receive<SincronizacaoConnectResponse>(delegate(SincronizacaoConnectResponse ack)
			{
				base.Become(new Action(this.Connected));
			}, null);
			this.Create(new SincronizacaoReConnectRequest());
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000025E8 File Offset: 0x000007E8
		public void GravarLog(string informacao)
		{
			string text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogSincronizador.txt");
			if (!File.Exists(text))
			{
				return;
			}
			using (StreamWriter streamWriter = new StreamWriter(text, true))
			{
				streamWriter.WriteLine(string.Format("{0} - {1}", DateTime.Now, informacao));
				streamWriter.Close();
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002658 File Offset: 0x00000858
		public static void RestartService(string serviceName, int timeoutMilliseconds = 10000)
		{
			ServiceController serviceController = new ServiceController(serviceName);
			int tickCount = Environment.TickCount;
			TimeSpan timeSpan = TimeSpan.FromMilliseconds((double)timeoutMilliseconds);
			serviceController.Refresh();
			if (serviceController.Status != ServiceControllerStatus.Stopped)
			{
				serviceController.Stop();
				serviceController.WaitForStatus(ServiceControllerStatus.Stopped, timeSpan);
				int tickCount2 = Environment.TickCount;
				timeSpan = TimeSpan.FromMilliseconds((double)(timeoutMilliseconds - (tickCount2 - tickCount)));
				serviceController.Start();
				serviceController.WaitForStatus(ServiceControllerStatus.Running, timeSpan);
				return;
			}
			try
			{
				serviceController.Start();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000026D4 File Offset: 0x000008D4
		private Task HandleOpLogRequest(OpLogInsertRequest message)
		{
			return ((message.Data.Contains("$set") || message.Data.Contains("$unset")) ? this._repository.UpdateOneAsync(message.CollectionAffected, message.Id, message.Data) : this._repository.ReplaceOneAsync(message.CollectionAffected, message.Id, message.Data)).ContinueWith(delegate(Task x)
			{
				if (!x.IsFaulted)
				{
					return;
				}
				SincronizacaoClientActor <>4__this = this;
				string text = "Falha ao persistir item collection: {0} id: {1} \r\n Exception: {2}";
				object collectionAffected = message.CollectionAffected;
				object id = message.Id;
				AggregateException exception = x.Exception;
				object obj;
				if (exception == null)
				{
					obj = null;
				}
				else
				{
					Exception innerException = exception.InnerException;
					obj = ((innerException != null) ? innerException.Message : null);
				}
				<>4__this.RegistrarLog(string.Format(text, collectionAffected, id, obj), true);
			});
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000278E File Offset: 0x0000098E
		private void RegistrarLog(string message, bool error = false)
		{
			if (error)
			{
				this._logger.Error(message);
			}
			else
			{
				this._logger.Info(message);
			}
			Console.WriteLine(message);
		}

		// Token: 0x04000004 RID: 4
		private ActorSelection _server;

		// Token: 0x04000005 RID: 5
		private readonly RepositorySincronizacao _repository;

		// Token: 0x04000006 RID: 6
		private OpLogTail _tail;

		// Token: 0x04000007 RID: 7
		private readonly Logger _logger;

		// Token: 0x04000008 RID: 8
		private int _contador;
	}
}
