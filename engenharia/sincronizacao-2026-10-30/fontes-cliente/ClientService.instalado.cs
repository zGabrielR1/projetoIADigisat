using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ServiceModel;
using DigisatServer.Application.Main.Dto;
using DigisatServer.Application.Main.Dto.Exceptions;
using DigisatServer.Distributed.Wcf.Core;
using DigisatServer.Distributed.Wcf.Core.Attributes;
using DigisatServer.Distributed.Wcf.Core.Encodings;
using DigisatServer.Distributed.Wcf.Main.Contracts.ServidoresModulo;
using DigisatServer.Infrastructure.Crosscutting.Helper;
using DigisatServer.Infrastructure.Crosscutting.Model;

namespace DigisatServer.Distributed.Wcf.Main.Clients
{
	// Token: 0x02000036 RID: 54
	public sealed class ClientService : IClientService, IDisposable
	{
		// Token: 0x060000F5 RID: 245 RVA: 0x00005CBC File Offset: 0x00003EBC
		public ClientService(EndPointWcf endPoint)
		{
			this._endPoint = endPoint;
			this._channels = new ConcurrentDictionary<string, ICommunicationObject>();
			this.CriarCredentials("", "", "", "");
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00005CFB File Offset: 0x00003EFB
		public void SetarXmlKey(string xmlKey)
		{
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00005D00 File Offset: 0x00003F00
		private string Inicializar()
		{
			InformacaoServidorDto informacaoServidorDto = this.Canal<IInformacaoModuleService, InformacaoServidorDto>((IInformacaoModuleService service) => service.ObterInformacoes());
			this.SetarXmlKey(informacaoServidorDto.XmlKey);
			return informacaoServidorDto.Release.ToStringEmptyWhenNull();
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00005D4A File Offset: 0x00003F4A
		public ClientService SetarNomeUsuario(string nomeUsuario)
		{
			this.CriarCredentials(ClientService._credentials.IdEmpresa, nomeUsuario, ClientService._credentials.Senha, ClientService._credentials.IdToken);
			return this;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00005D72 File Offset: 0x00003F72
		private void CriarCredentials(string idEmpresa = "", string nomeUsuario = "", string senha = "", string token = "")
		{
			ClientService._credentials = new Credentials(idEmpresa, nomeUsuario, senha, token);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00005D83 File Offset: 0x00003F83
		public ClientService SetarSenha(string senha)
		{
			this.CriarCredentials(ClientService._credentials.IdEmpresa, ClientService._credentials.NomeUsuario, senha, ClientService._credentials.IdToken);
			return this;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00005DAB File Offset: 0x00003FAB
		public ClientService SetarIdEmpresa(string idEmpresa)
		{
			this.CriarCredentials(idEmpresa, ClientService._credentials.NomeUsuario, ClientService._credentials.Senha, ClientService._credentials.IdToken);
			return this;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005DD3 File Offset: 0x00003FD3
		public ClientService SetarIdToken(string token)
		{
			this.CriarCredentials(ClientService._credentials.IdEmpresa, ClientService._credentials.NomeUsuario, ClientService._credentials.Senha, token);
			return this;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005DFC File Offset: 0x00003FFC
		public void Canal<TContract>(Action<TContract> action)
		{
			ICommunicationObject communicationObject = this.CriarCanal<TContract>();
			try
			{
				if (action != null)
				{
					action((TContract)((object)communicationObject));
				}
			}
			catch (FaultException<ApplicationServiceError> faultException)
			{
				throw new Exception(faultException.Message);
			}
			catch (FaultException ex)
			{
				throw new Exception(ex.Message);
			}
			catch (EndpointNotFoundException)
			{
				throw new Exception("Falha ao acessar endereço do serviço. Tente novamente!");
			}
			catch (CommunicationException)
			{
				this.Reiniciar();
				throw new Exception("Falha de comunicação");
			}
			catch (TimeoutException)
			{
				throw new Exception("Timeout");
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005EA4 File Offset: 0x000040A4
		public string Reiniciar()
		{
			object sync = this._sync;
			string text;
			lock (sync)
			{
				this.FecharCanais();
				this._channels.Clear();
				text = this.Inicializar();
			}
			return text;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005EF8 File Offset: 0x000040F8
		private ICommunicationObject CriarCanal<TContract>()
		{
			ICommunicationObject channel;
			this._channels.TryGetValue(typeof(TContract).Name, out channel);
			if (channel != null)
			{
				return channel;
			}
			if (typeof(TContract).GetCustomAttributes(typeof(ProtectedAttribute), true).Length != 0)
			{
				channel = new ChannelFactoryProtegido(this._endPoint, ClientService._credentials, ContentEncryptionType.All).CriarCanal<TContract>() as ICommunicationObject;
			}
			else if (typeof(TContract).GetCustomAttributes(typeof(NoProtectedAttribute), true).Length != 0)
			{
				channel = new ChannelFactoryProtegido(this._endPoint, ClientService._credentials, ContentEncryptionType.None).CriarCanal<TContract>() as ICommunicationObject;
			}
			else
			{
				channel = new ChannelFactoryBasico(this._endPoint).CriarCanal<TContract>() as ICommunicationObject;
			}
			this._channels.AddOrUpdate(typeof(TContract).Name, (string s) => channel, (string s, ICommunicationObject o) => channel);
			return channel;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00006020 File Offset: 0x00004220
		public TResult Canal<TContract, TResult>(Func<TContract, TResult> func)
		{
			ICommunicationObject communicationObject = this.CriarCanal<TContract>();
			TResult tresult;
			try
			{
				tresult = func((TContract)((object)communicationObject));
			}
			catch (FaultException<ApplicationServiceError> faultException)
			{
				throw new Exception(faultException.Message);
			}
			catch (FaultException ex)
			{
				throw new Exception(ex.Message);
			}
			catch (EndpointNotFoundException)
			{
				throw new Exception("Falha ao acessar endereço do serviço. Tente novamente!");
			}
			catch (CommunicationException)
			{
				this.Reiniciar();
				throw new Exception("Falha de comunicação");
			}
			catch (TimeoutException)
			{
				throw new Exception("Timeout");
			}
			return tresult;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x000060C8 File Offset: 0x000042C8
		private void FecharCanais()
		{
			foreach (KeyValuePair<string, ICommunicationObject> keyValuePair in this._channels)
			{
				keyValuePair.Value.Close();
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000611C File Offset: 0x0000431C
		~ClientService()
		{
			this.Dispose(false);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000614C File Offset: 0x0000434C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000615C File Offset: 0x0000435C
		private void Dispose(bool disposeManaged)
		{
			if (!disposeManaged)
			{
				return;
			}
			object sync = this._sync;
			lock (sync)
			{
				this.FecharCanais();
				this._channels.Clear();
				this._channels = null;
				ClientService._credentials = null;
			}
		}

		// Token: 0x04000055 RID: 85
		private readonly EndPointWcf _endPoint;

		// Token: 0x04000056 RID: 86
		private static Credentials _credentials;

		// Token: 0x04000057 RID: 87
		private ConcurrentDictionary<string, ICommunicationObject> _channels;

		// Token: 0x04000058 RID: 88
		private readonly object _sync = new object();
	}
}
