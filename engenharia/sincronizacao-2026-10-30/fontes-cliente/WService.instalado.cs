using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Xpf.Core;
using DigisatServer.Application.Main.Dto;
using DigisatServer.Distributed.Wcf.Main.Clients;
using DigisatServer.Distributed.Wcf.Main.Contracts.AuxiliaresModulo;
using DigisatServer.Distributed.Wcf.Main.Contracts.ServidoresModulo;
using DigisatServer.Infrastructure.Crosscutting.Config;
using DigisatServer.Infrastructure.Crosscutting.Event;
using DigisatServer.Infrastructure.Crosscutting.File;
using DigisatServer.Infrastructure.Crosscutting.Function;
using DigisatServer.Infrastructure.Crosscutting.Helper;
using DigisatServer.Infrastructure.Crosscutting.Interfaces;
using DigisatServer.Infrastructure.Crosscutting.Model;
using DigisatServer.Infrastructure.Crosscutting.Mvvm;
using DigisatServer.Infrastructure.Crosscutting.NetFramework.File;
using DigisatServer.Infrastructure.Crosscutting.NetFramework.Session;
using Otus.Infraestrutura.Events;
using Otus.Infraestrutura.Interfaces;
using Otus.Infraestrutura.Models;
using Otus.Infraestrutura.Models.Sessoes;

namespace Otus.Service
{
	// Token: 0x02000002 RID: 2
	public class WService : IWService
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public WService(IEventPublisher eventPublisher, IConfiguracaoClientService configuracaoClientService, IAutenticacaoService autenticacaoService)
		{
			this._eventPublisher = eventPublisher;
			this._configuracaoClientService = configuracaoClientService;
			this._autenticacaoService = autenticacaoService;
			this._eventPublisher.GetEvent<SessaoAtualizadaEvent>().Subscribe<SessaoAtualizadaEvent>(async delegate(SessaoAtualizadaEvent x)
			{
				this._sessao = x.Sessao;
				if (!this._offLine)
				{
					await this.ExecutarLogOn();
				}
			});
			(from x in Observable.Interval(TimeSpan.FromSeconds(35.0))
				where this._offLine
				select x).Subscribe<long>(async delegate(long x)
			{
				try
				{
					if (this.Inicializado)
					{
						ClientService clientService = this._clientService;
						if (clientService != null)
						{
							clientService.Reiniciar();
						}
					}
					else
					{
						this.Criar();
						if (!this.Inicializado)
						{
							return;
						}
						this.SetarNomeUsuario(this._nomeUsuario);
						this.SetarSenha(this._senha);
						this.SetarIdEmpresa(this._idEmpresa);
					}
					await this.ExecutarLogOn();
				}
				catch (Exception)
				{
				}
			});
			XmlFileFactory.SetCurrent(new XmlTokenFileFactory());
			this._xmlTokenFile = XmlFileFactory.Create();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020E0 File Offset: 0x000002E0
		private async Task ExecutarLogOn()
		{
			int num = 0;
			try
			{
				if (this._sessao == null)
				{
					return;
				}
				string text = await this.Canal<IAutenticacaoModuleService, Task<string>>((IAutenticacaoModuleService service) => service.ExecutarLogOnAsync());
				this._offLine = false;
				ClientService clientService = this._clientService;
				if (clientService != null)
				{
					clientService.SetarIdToken(text).Reiniciar();
				}
				this._xmlTokenFile.Save<Token>(this._sessao.GerarToken(text));
				this._eventPublisher.Publish<ExecutouLogonEvent>(new ExecutouLogonEvent());
				this._eventPublisher.Publish<AbrirPontoDeVendaAoLogarEvent>(new AbrirPontoDeVendaAoLogarEvent());
				this._eventPublisher.Publish<AbrirAutoatendimentoBalancaAoLogarEvent>(new AbrirAutoatendimentoBalancaAoLogarEvent());
			}
			catch (Exception obj)
			{
				num = 1;
			}
			object obj;
			if (num == 1)
			{
				Exception ex = (Exception)obj;
				if (ex.Message.Contains("Usuário efetuou login em outro local! Reinicie o sistema ou faça a troca de usuário."))
				{
					await this.Canal<IAutenticacaoUsuarioJaLogadoModuleService, Task>((IAutenticacaoUsuarioJaLogadoModuleService service) => service.RemoverUsuarioAsync());
					await this.ExecutarLogOn();
					return;
				}
				DispatcherHelper.UpdateLayoutAndDoEvents();
				this._autenticacaoService.Instancia.LimparSessao();
				if (!this._autenticacaoService.Instancia.Inicializar())
				{
					DialogoFactory.MostrarDialogoConfirma(ex.Message, false, HorizontalAlignment.Center);
				}
				ex = null;
			}
			obj = null;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002128 File Offset: 0x00000328
		public IWService Criar()
		{
			if (this.Inicializado)
			{
				return this;
			}
			try
			{
				ConfiguracaoClient configuracaoClient = this._configuracaoClientService.ObterConfiguracao();
				if (!configuracaoClient.IsNull() && configuracaoClient.IpValido)
				{
					this._clientService = new ClientService(configuracaoClient.EndPoint);
					ConfiguracaoSomAlerta somAlerta = configuracaoClient.SomAlerta;
					DialogoFactory.SetarTocarSomAlerta(somAlerta != null && somAlerta.TocarSomAlerta);
				}
			}
			catch (Exception)
			{
			}
			return this;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000219C File Offset: 0x0000039C
		public void TestarConfiguracao(string ip, int porta = 12221)
		{
			if (this.Inicializado)
			{
				return;
			}
			new ClientService(new EndPointWcfFactory().SetarHost(ip).SetarPorta(porta).Criar());
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021C3 File Offset: 0x000003C3
		public IWService SetarNomeUsuario(string nomeUsuario)
		{
			this._nomeUsuario = nomeUsuario;
			ClientService clientService = this._clientService;
			if (clientService != null)
			{
				clientService.SetarNomeUsuario(nomeUsuario);
			}
			return this;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021E0 File Offset: 0x000003E0
		public IWService SetarSenha(string senha)
		{
			this._senha = senha;
			ClientService clientService = this._clientService;
			if (clientService != null)
			{
				clientService.SetarSenha(senha);
			}
			return this;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021FD File Offset: 0x000003FD
		public IWService SetarIdEmpresa(string idEmpresa)
		{
			this._idEmpresa = idEmpresa;
			ClientService clientService = this._clientService;
			if (clientService != null)
			{
				clientService.SetarIdEmpresa(idEmpresa);
			}
			return this;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000221A File Offset: 0x0000041A
		public IWService SetarToken(string token)
		{
			ClientService clientService = this._clientService;
			if (clientService != null)
			{
				clientService.SetarIdToken(token);
			}
			return this;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002230 File Offset: 0x00000430
		public void Canal<TContract>(Action<TContract> action)
		{
			this.OnlineMsg(true);
			this._clientService.Canal<TContract>(action);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002246 File Offset: 0x00000446
		public TResult Canal<TContract, TResult>(Func<TContract, TResult> func)
		{
			if (typeof(TContract).Name.Contains("PdvOfflineModuleService"))
			{
				this.OnlinePdvOfflineMsg();
			}
			else
			{
				this.OnlineMsg(true);
			}
			return this._clientService.Canal<TContract, TResult>(func);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002280 File Offset: 0x00000480
		public bool Inicializado
		{
			get
			{
				return this._clientService != null;
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000228C File Offset: 0x0000048C
		public bool OnlineMsg(bool propagarException = false)
		{
			InformacaoServidorDto informacaoServidorDto = null;
			for (int i = 0; i < 3; i++)
			{
				try
				{
					informacaoServidorDto = this._clientService.Canal<IInformacaoModuleService, InformacaoServidorDto>((IInformacaoModuleService service) => service.ObterInformacoes());
					break;
				}
				catch (Exception)
				{
					ConfiguracaoOffline configuracaoOffline = ConfiguracaoOffline.Carregar();
					bool flag = false;
					if (configuracaoOffline.PdvOfflineValido)
					{
						flag = DadosSistema.HabilitacaoLocalContemModuloPdvOffline(true);
					}
					if (configuracaoOffline.PdvOfflineValido && flag)
					{
						this._offLine = true;
						return true;
					}
					Thread.Sleep(500);
				}
			}
			if (informacaoServidorDto.IsNotNull())
			{
				this._clientService.SetarXmlKey(informacaoServidorDto.XmlKey);
				this._offLine = false;
				return true;
			}
			this._offLine = true;
			if (propagarException)
			{
				throw new Exception("Servidor Off-line! Verifique.");
			}
			DialogoFactory.MostrarDialogoConfirma("Servidor Off-line! Verifique.", false, HorizontalAlignment.Center);
			return false;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002364 File Offset: 0x00000564
		public bool OnlinePdvOfflineMsg()
		{
			InformacaoServidorDto informacaoServidorDto = null;
			GerarLog gerarLog = new GerarLog();
			try
			{
				gerarLog.GravarLog("Chegou no OnlinePdvOfflineMsg");
				informacaoServidorDto = this._clientService.Canal<IInformacaoModuleService, InformacaoServidorDto>((IInformacaoModuleService service) => service.ObterInformacoes());
			}
			catch (Exception)
			{
			}
			if (informacaoServidorDto.IsNotNull())
			{
				gerarLog.GravarLog("Retornou true no OnlinePdvOfflineMsg");
				return true;
			}
			gerarLog.GravarLog("Retornou 'Servidor Off - line!Verifique.' no OnlinePdvOfflineMsg");
			throw new Exception("Servidor Off-line! Verifique.");
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000023F0 File Offset: 0x000005F0
		public string Reiniciar()
		{
			ClientService clientService = this._clientService;
			if (clientService == null)
			{
				return null;
			}
			return clientService.Reiniciar();
		}

		// Token: 0x04000001 RID: 1
		private readonly IEventPublisher _eventPublisher;

		// Token: 0x04000002 RID: 2
		private readonly IConfiguracaoClientService _configuracaoClientService;

		// Token: 0x04000003 RID: 3
		private readonly IAutenticacaoService _autenticacaoService;

		// Token: 0x04000004 RID: 4
		private ClientService _clientService;

		// Token: 0x04000005 RID: 5
		private readonly IXmlFile _xmlTokenFile;

		// Token: 0x04000006 RID: 6
		private bool _offLine;

		// Token: 0x04000007 RID: 7
		private string _nomeUsuario;

		// Token: 0x04000008 RID: 8
		private string _senha;

		// Token: 0x04000009 RID: 9
		private string _idEmpresa;

		// Token: 0x0400000A RID: 10
		private ISessao _sessao;
	}
}
