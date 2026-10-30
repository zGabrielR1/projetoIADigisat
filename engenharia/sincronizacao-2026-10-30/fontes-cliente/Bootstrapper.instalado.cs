using System;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Xpf.Core;
using DigisatServer.Domain.Main.Aggregates.AuxiliaresAgg.Configuracoes;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.Cardapios;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.Grupos;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.MenusRapido;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.Pessoas;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.ProdutosServicos;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.ProdutosServicos.Estoques;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.ProdutosServicos.Estoques.Quantidades;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.TabelasPreco;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.TributacoesFederal;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.Usuarios;
using DigisatServer.Domain.Main.Aggregates.FinanceirosAgg.Contas;
using DigisatServer.Domain.Main.Aggregates.IndustriasAgg.EstoquesFisico;
using DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg;
using DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.RegistrosPafEcf;
using DigisatServer.Domain.Main.Interfaces;
using DigisatServer.Domain.Main.Services;
using DigisatServer.Infrastructure.Crosscutting.Event;
using DigisatServer.Infrastructure.Crosscutting.Interfaces;
using DigisatServer.Infrastructure.Crosscutting.Logging;
using DigisatServer.Infrastructure.Crosscutting.Mvvm;
using DigisatServer.Infrastructure.Crosscutting.Mvvm.Interfaces;
using DigisatServer.Infrastructure.Crosscutting.NetFramework.Event;
using DigisatServer.Infrastructure.Crosscutting.NetFramework.Logging;
using DigisatServer.Infrastructure.Crosscutting.NetFramework.Service;
using DigisatServer.Infrastructure.Crosscutting.NetFramework.Wrapper;
using DigisatServer.Infrastructure.Crosscutting.RegistroSistema.Service;
using DigisatServer.Infrastructure.Crosscutting.Service;
using DigisatServer.Infrastructure.Crosscutting.Service.CEST;
using DigisatServer.Infrastructure.Crosscutting.Service.IdentificacaoChamada;
using DigisatServer.Infrastructure.Crosscutting.Wrapper;
using DigisatServer.Infrastructure.Data.Core;
using DigisatServer.Infrastructure.Data.Core.Interfaces;
using DigisatServer.Infrastructure.Data.Core.Mappings;
using DigisatServer.Infrastructure.Data.Main.Repositories;
using Microsoft.Practices.Unity;
using Otus.Infraestrutura.Events;
using Otus.Infraestrutura.Interfaces;
using Otus.Infraestrutura.Models;
using Otus.Infraestrutura.Services;
using Otus.Infraestrutura.ViewModels.Options;
using Otus.Infraestrutura.Views;
using Otus.Service;
using Otus.Shell.Views;
using Prism.Modularity;
using Prism.Unity;

namespace Otus.Boot
{
	// Token: 0x02000002 RID: 2
	public class Bootstrapper : UnityBootstrapper
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		protected override DependencyObject CreateShell()
		{
			return base.Container.Resolve<ShellView>(Array.Empty<ResolverOverride>());
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002062 File Offset: 0x00000262
		protected override void InitializeShell()
		{
			Application.Current.MainWindow.Show();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002073 File Offset: 0x00000273
		protected override IModuleCatalog CreateModuleCatalog()
		{
			return new DirectoryModuleCatalog
			{
				ModulePath = ".\\Modulos"
			};
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002088 File Offset: 0x00000288
		protected override void ConfigureContainer()
		{
			Bootstrapper.ConfigureFactories();
			MongoMap.Convention();
			base.ConfigureContainer();
			base.Container.RegisterTypeForNavigation<DialogoNotificacaoView>(null);
			base.Container.RegisterTypeForNavigation<InclusaoChequeFichaView>(null);
			base.Container.RegisterTypeForNavigation<InclusaoCartaoFichaView>(null);
			base.Container.RegisterTypeForNavigation<InclusaoCarteiraDigitalFichaView>(null);
			base.Container.RegisterTypeForNavigation<InclusaoCarteiraDigitalShipayFichaView>(null);
			base.Container.RegisterTypeForNavigation<BoletoPagamentoFichaView>(null);
			base.Container.RegisterType<ITefService, TefService>(Array.Empty<InjectionMember>());
			base.Container.RegisterType<IShipayService, ShipayService>(Array.Empty<InjectionMember>());
			base.Container.RegisterType<INotificacao, Notificacao>(Array.Empty<InjectionMember>());
			base.Container.RegisterType<IEventPublisher, EventPublisher>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IExecutarProcessoService, ExecutarProcessoService>(Array.Empty<InjectionMember>());
			base.Container.RegisterType<ICestService, CestService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IRegionManagerService, RegionManagerService>(Array.Empty<InjectionMember>());
			base.Container.RegisterType<INavegacaoPreAutenticacaoService, TrabalhoNavegacaoService>(string.Format("p{0}", typeof(TrabalhoNavegacaoService).Name), Array.Empty<InjectionMember>());
			base.Container.RegisterType<INavegacaoBloqueioService, TrabalhoNavegacaoService>(typeof(TrabalhoNavegacaoService).Name, Array.Empty<InjectionMember>());
			base.Container.RegisterType<INavegacaoAlterarSenhaService, TrabalhoNavegacaoService>(typeof(TrabalhoNavegacaoService).Name, Array.Empty<InjectionMember>());
			base.Container.RegisterType<IConfiguracaoClientService, ConfiguracaoClientService>(Array.Empty<InjectionMember>());
			base.Container.RegisterType<IBalancaService, BalancaService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IMobileService, MobileService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<ILeitorSerialService, LeitorSerialService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IImpressoraNaoFiscalService, ImpressoraNaoFiscalService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IImpressaoGraficoService, ImpressaoGraficoService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IImpressaoImagemItensMovimentacaoService, ImpressaoImagemItensMovimentacaoService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IImpressaoImagemAnexaDavOsService, ImpressaoImagemAnexaDavOsService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IImpressoraChequeService, ImpressoraChequeService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IWService, WService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IGeradorImpressoTurnoService, GeradorImpressoTurnoService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IIdentificadorDeChamadaService, IdentificadorDeChamadaService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IInutilizacaoClientService, InutilizacaoClientService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IMercadoLivreApiService, MercadoLivreApiService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IShopeeApiService, ShopeeApiService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IParametrizaSoftClientService, ParametrizaSoftClientService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IIMendesClientService, IMendesClientService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityAtivos<, >), typeof(EntityAtivos<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityAtivos<, >), typeof(EntityAtivosTextoNumerico<, >), "EntityAtivosTextoNumerico", Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityAtivos<, >), typeof(EntityAtivosTextoReferencia<, >), "EntityAtivosTextoReferencia", Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityInativos<, >), typeof(EntityInativos<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityInativos<, >), typeof(EntityInativosTextoReferencia<, >), "EntityInativosTextoReferencia", Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityInativos<, >), typeof(EntityInativosTextoNumerico<, >), "EntityInativosTextoNumerico", Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IValueObjectTodos<>), typeof(ValueObjectTodos<>), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityFiltroAvancadoPagarReceber<, >), typeof(EntityFiltroAvancadoPagarReceber<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityFiltroAvancadoGestaoContrato<, >), typeof(EntityFiltroAvancadoGestaoContrato<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityFiltroAvancadoMovimentacaoCtrc<, >), typeof(EntityFiltroAvancadoMovimentacaoCtrc<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityFiltroAvancadoManifestacaoDestinatario<, >), typeof(EntityFiltroAvancadoManifestacaoDestinatario<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityFiltroAvancadoManifestacaoDestinatarioCte<, >), typeof(EntityFiltroAvancadoManifestacaoDestinatarioCte<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityFiltroAvancadoOrdemProducao<, >), typeof(EntityFiltroAvancadoOrdemProducao<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityFiltroAvancadoOrdemDesmontagem<, >), typeof(EntityFiltroAvancadoOrdemDesmontagem<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityFiltroAvancadoMovimentacao<, , >), typeof(EntityFiltroAvancadoMovimentacao<, , >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityFiltroAvancadoConhecimentoTransporteEletronico<, >), typeof(EntityFiltroAvancadoConhecimentoTransporteEletronico<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityFiltroMovimentacao<, , >), typeof(EntityFiltroMovimentacao<, , >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityFiltroConhecimentoTransporteEletronico<, >), typeof(EntityFiltroConhecimentoTransporteEletronico<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityAtivosFiltroDevolucao<, >), typeof(EntityAtivoFiltroDevolucao<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityInativosFiltroDevolucao<, >), typeof(EntityInativoFiltroDevolucao<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityFiltroManifestoEletronicoDocumentoFiscal<, >), typeof(EntityFiltroManifestoEletronicoDocumentoFiscal<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityFiltroManifestacaoDestinatario<, >), typeof(EntityFiltroManifestacaoDestinatario<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityFiltroManifestacaoDestinatarioCte<, >), typeof(EntityFiltroManifestacaoDestinatarioCte<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityAtivosBoleto<, >), typeof(EntityAtivoFiltroBoleto<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityInativoBoleto<, >), typeof(EntityInativoFiltroBoleto<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityFiltroControleEntrega<, >), typeof(EntityFiltroControleEntrega<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityAtivoFiltroTurno<, >), typeof(EntityAtivoFiltroTurno<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityInativoFiltroTurno<, >), typeof(EntityInativoFiltroTurno<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityAtivosSemPaginacao<, >), typeof(EntityAtivosSemPaginacao<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityInativosSemPaginacao<, >), typeof(EntityInativosSemPaginacao<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityInativosFiltroPagarReceber<, >), typeof(EntityInativoFiltroPagarReceber<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityAtivosFiltroPagarReceber<, >), typeof(EntityAtivoFiltroPagarReceber<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityInativosFiltroProdutoServico<, >), typeof(EntityInativoFiltroProdutoServico<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityAtivosFiltroProdutoServico<, >), typeof(EntityAtivoFiltroProdutoServico<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityInativosFiltroPessoa<, >), typeof(EntityInativoFiltroPessoa<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityAtivosFiltroPessoa<, >), typeof(EntityAtivoFiltroPessoa<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityInativosFiltroTicket<, >), typeof(EntityInativoFiltroTicket<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityAtivosFiltroTicket<, >), typeof(EntityAtivoFiltroTicket<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityAtivosGestaoContrato<, >), typeof(EntityAtivoFiltroGestaoContrato<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityInativoGestaoContrato<, >), typeof(EntityInativoFiltroGestaoContrato<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityAtivosFiltroRomaneioCarga<, >), typeof(EntityAtivosFiltroRomaneioCarga<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityAtivosFiltroRomaneioConhecimentoTransporteEletronico<, >), typeof(EntityAtivosFiltroRomaneioConhecimentoTransporteEletronico<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityFiltroOrcamentoIndustria<, >), typeof(EntityFiltroOrcamentoIndustria), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityFiltroOrdemProducao<, >), typeof(EntityFiltroOrdemProducao), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityFiltroOrdemDesmontagem<, >), typeof(EntityFiltroOrdemDesmontagem), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityFiltroProgramacaoIndustria<, >), typeof(EntityFiltroProgramacaoIndustria), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IArquivoAuxiliarService, ArquivoAuxiliarService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<ITransmissaoClienteReducaoZService, TransmissaoClienteReducaoZService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IEnvioPafEcfService, EnvioPafEcfService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IImpressaoRelatorioService, ImpressaoRelatorioService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IArquivoFiscalCatalogoService, ArquivoFiscalCatalogoService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IGeracaoAutomaticaArquivosFiscaisService, GeracaoAutomaticaArquivosFiscaisService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IRegeneracaoAutomaticaTimerService, RegeneracaoAutomaticaTimerService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IBoletoProxyService, BoletoProxyService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IInteracaoBarraTarrefaService, InteracaoBarraTarrefaService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IImpressaoDigitalService, ImpressaoDigitalService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityAtivosMovimentacaoCtrc<, >), typeof(EntityAtivoFiltroMovimentacaoCtrc<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityInativosMovimentacaoCtrc<, >), typeof(EntityInativoFiltroMovimentacaoCtrc<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityInativosFiltroGrupoMenuRapido<, >), typeof(EntityInativoFiltroGrupoMenuRapido<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IEntityAtivosFiltroGrupoMenuRapido<, >), typeof(EntityAtivoFiltroGrupoMenuRapido<, >), Array.Empty<InjectionMember>());
			base.Container.RegisterTypeForNavigation<InclusaoChequeFichaView>(null);
			base.Container.RegisterType<Seletor<IPainel>>(Array.Empty<InjectionMember>());
			base.Container.RegisterType<IPainelTitulo, PainelTitulo>(Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IRepositoryBaseWrite<>), typeof(RepositoryBaseWrite<>), new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IRepositoryEmpresaRead<>), typeof(RepositoryEmpresaRead<>), new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IRepositoryPafWrite<>), typeof(RepositoryPafWrite<>), new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IRepositoryPafBaseWrite<>), typeof(RepositoryPafBaseWrite<>), new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IRepositoryRead<>), typeof(RepositoryRead<>), new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IRepositoryTreeRead<>), typeof(RepositoryTreeRead<>), new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IRepositoryTreeWrite<>), typeof(RepositoryTreeWrite<>), new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType(typeof(IRepositoryWrite<>), typeof(RepositoryWrite<>), new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IEstoqueRepositoryWrite, EstoqueRepositoryWrite>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IEstoqueFisicoQuantidadeRepositoryWrite, EstoqueFisicoQuantidadeRepositoryWrite>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<ProdutoServicoEmpresaRepositoryReadFactory>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			IUnityContainer container4 = base.Container;
			LifetimeManager lifetimeManager = new ContainerControlledLifetimeManager();
			InjectionMember[] array = new InjectionMember[1];
			array[0] = new InjectionFactory((IUnityContainer container) => container.Resolve<ProdutoServicoEmpresaRepositoryReadFactory>(Array.Empty<ResolverOverride>()).SetarTributacaoFederalRepositoryRead(container.Resolve<TributacaoFederalRepositoryRead>(Array.Empty<ResolverOverride>())).Criar());
			container4.RegisterType<IProdutoServicoEmpresaRepositoryRead>(lifetimeManager, array);
			base.Container.RegisterType<IMovimentacaoRepositoryRead, MovimentacaoRepositoryRead>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IPrecoRepositoryRead, PrecoRepositoryRead>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<ITributacaoFederalRepositoryRead, TributacaoFederalRepositoryRead>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IGrupoRepositoryRead, GrupoRepositoryRead>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<ITabelaPrecoRepositoryRead, TabelaPrecoRepositoryRead>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IPessoaRepositoryRead, PessoaRepositoryRead>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<KitRepositoryReadFactory>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			IUnityContainer container2 = base.Container;
			LifetimeManager lifetimeManager2 = new ContainerControlledLifetimeManager();
			InjectionMember[] array2 = new InjectionMember[1];
			array2[0] = new InjectionFactory((IUnityContainer container) => container.Resolve<KitRepositoryReadFactory>(Array.Empty<ResolverOverride>()).Criar());
			container2.RegisterType<IKitRepositoryRead>(lifetimeManager2, array2);
			base.Container.RegisterType<IConfiguracaoRepositoryRead, ConfiguracaoRepositoryRead>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			IUnityContainer container3 = base.Container;
			LifetimeManager lifetimeManager3 = new ContainerControlledLifetimeManager();
			InjectionMember[] array3 = new InjectionMember[1];
			array3[0] = new InjectionFactory((IUnityContainer container) => ProdutoServicoRepositoryReadFactory.Criar());
			container3.RegisterType<IProdutoServicoRepositoryRead>(lifetimeManager3, array3);
			base.Container.RegisterType<IRegistroPafEcfRepositoryRead, RegistroPafEcfRepositoryRead>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IUsuarioRepositoryRead, UsuarioRepositoryRead>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IRegistroWindowsService, RegistroWindowsService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IContaRepositoryRead, ContaRepositoryRead>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IAcessoRapidoService, AcessoRapidoService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<ICardapioRepositoryRead, CardapioRepositoryRead>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IMenuRapidoRepositoryRead, MenuRapidoRepositoryRead>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IMenuRapidoRepositoryWrite, MenuRapidoRepositoryWrite>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IGrupoMenuRepositoryWrite, GrupoMenuRepositoryWrite>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<NancyHostService>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IEstoqueQuantidadeRepositoryRead, EstoqueQuantidadeRepositoryRead>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IFileIoWrapper, FileIoWrapper>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			base.Container.RegisterType<IAssinaturaWrapper, AssinaturaWrapper>(new ContainerControlledLifetimeManager(), Array.Empty<InjectionMember>());
			this.ConfigureLoader();
			this.InitializeInstances();
			DadosSistema.SetarContainer(base.Container);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002FD9 File Offset: 0x000011D9
		public override void Run(bool runWithDefaultConfiguration)
		{
			base.Run(runWithDefaultConfiguration);
			LoaderProcessoService.MostrarLoader("Carregando...");
			Task.Run(delegate
			{
				this.ConfigureMaps();
			}).ContinueWith(delegate(Task x)
			{
				LoaderProcessoService.EsconderLoader();
				if (x.IsFaulted)
				{
					x.Exception.ShowLastException("", false, false);
					return;
				}
				base.Container.Resolve<IEventPublisher>(Array.Empty<ResolverOverride>()).Publish<MongoDbConfiguradoEvent>(new MongoDbConfiguradoEvent());
			});
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000300F File Offset: 0x0000120F
		protected override void InitializeModules()
		{
			DispatcherHelper.UpdateLayoutAndDoEvents();
			base.InitializeModules();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000301C File Offset: 0x0000121C
		private static void ConfigureFactories()
		{
			LoggerFactory.SetCurrent(new TraceSourceLogFactory());
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00003028 File Offset: 0x00001228
		private void ConfigureLoader()
		{
			LoaderProcessoService.SetarEventPublisher(base.Container.Resolve<IEventPublisher>(Array.Empty<ResolverOverride>()));
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000303F File Offset: 0x0000123F
		private void ConfigureMaps()
		{
			MongoMap.InitProducaoClient();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00003046 File Offset: 0x00001246
		private void InitializeInstances()
		{
			base.Container.Resolve<NancyHostService>(Array.Empty<ResolverOverride>());
		}
	}
}
