using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml.Linq;
using DigisatServer.Distributed.Wcf.Core;
using DigisatServer.Distributed.Wcf.Core.Bindings;
using DigisatServer.Distributed.Wcf.Core.Encodings;
using DigisatServer.Distributed.Wcf.Core.Inspectors;
using DigisatServer.Infrastructure.Crosscutting.Model;

namespace DigisatServer.Distributed.Wcf.Main.Clients
{
	// Token: 0x02000035 RID: 53
	public class ChannelFactoryProtegido
	{
		// Token: 0x060000EF RID: 239 RVA: 0x00005990 File Offset: 0x00003B90
		public ChannelFactoryProtegido(EndPointWcf endPoint, Credentials credenciais, ContentEncryptionType contentEncryption)
		{
			this._endPoint = endPoint;
			this._credenciais = credenciais;
			this._contentEncryption = contentEncryption;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000059B8 File Offset: 0x00003BB8
		public TContract CriarCanal<TContract>()
		{
			object sync = this._sync;
			TContract tcontract;
			lock (sync)
			{
				ClientMessageInspector clientMessageInspector = new ClientMessageInspector();
				clientMessageInspector.BeforeSendRequested += this.MessageInspectorOnBeforeSendRequested;
				clientMessageInspector.AfterReceiveReplied += this.MessageInspectorOnAfterReceiveReplied;
				clientMessageInspector.DeveInspecionarResposta = new Func<Message, bool>(ChannelFactoryProtegido.EhRespostaComToken);
				string name = typeof(TContract).Name;
				Binding binding;
				if (!name.ToLower().Contains("arquivospedfiscal") && !name.ToLower().Contains("arquivoinventario") && !name.ToLower().Contains("demonstrativoresultado") && !name.ToLower().Contains("imendes") && !name.ToLower().Contains("arquivoefdcontribuicao") && !name.ToLower().Contains("geradorarquivocontabilidade") && !name.ToLower().Contains("arquivosintegra") && !name.ToLower().Contains("relacionamentocliente") && !name.ToLower().Contains("importacaoguiadafarmacia") && !name.ToLower().Contains("importacaoabcfarma"))
				{
					binding = new ClientCustomBinding(this._contentEncryption);
				}
				else
				{
					ClientCustomBinding clientCustomBinding = new ClientCustomBinding(this._contentEncryption);
					clientCustomBinding.OpenTimeout = TimeSpan.FromHours(12.0);
					clientCustomBinding.ReceiveTimeout = TimeSpan.FromHours(12.0);
					clientCustomBinding.CloseTimeout = TimeSpan.FromHours(12.0);
					binding = clientCustomBinding;
					clientCustomBinding.SendTimeout = TimeSpan.FromHours(12.0);
				}
				tcontract = new ChannelFactory<TContract>(binding)
				{
					Endpoint = 
					{
						Behaviors = { clientMessageInspector }
					}
				}.CreateChannel(new EndpointAddress(new Uri(string.Format("http://{0}/{1}", this._endPoint, name)), Array.Empty<AddressHeader>()));
			}
			return tcontract;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00005BB4 File Offset: 0x00003DB4
		private void MessageInspectorOnBeforeSendRequested(object sender, MessageInspectorArgs messageInspectorArgs)
		{
			messageInspectorArgs.Message.Headers.Add(this._credenciais.ToMessageHeader());
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005BD4 File Offset: 0x00003DD4
		private static bool EhRespostaComToken(Message resposta)
		{
			string action = resposta.Headers.Action;
			return string.IsNullOrEmpty(action) || action.EndsWith("LogOnResponse", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00005C04 File Offset: 0x00003E04
		private void MessageInspectorOnAfterReceiveReplied(object sender, MessageInspectorArgs messageInspectorArgs)
		{
			XNode xnode = XNode.ReadFrom(messageInspectorArgs.Message.GetReaderAtBodyContents());
			if (((XElement)xnode).Name.LocalName == "LogOnResponse")
			{
				this._credenciais = new Credentials(this._credenciais.IdEmpresa, this._credenciais.NomeUsuario, this._credenciais.Senha, ((XElement)xnode).Value);
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00005C78 File Offset: 0x00003E78
		public void Atualizar(Credentials credenciais)
		{
			object sync = this._sync;
			lock (sync)
			{
				this._credenciais = credenciais;
			}
		}

		// Token: 0x04000051 RID: 81
		private readonly EndPointWcf _endPoint;

		// Token: 0x04000052 RID: 82
		private readonly object _sync = new object();

		// Token: 0x04000053 RID: 83
		private Credentials _credenciais;

		// Token: 0x04000054 RID: 84
		private readonly ContentEncryptionType _contentEncryption;
	}
}
