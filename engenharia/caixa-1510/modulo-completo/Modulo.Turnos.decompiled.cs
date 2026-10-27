using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using DevExpress.Mvvm;
using DevExpress.Utils;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DigisatServer.Application.Main.Dto.Auxiliares.Turnos;
using DigisatServer.Application.Main.Dto.Cadastros.Usuarios;
using DigisatServer.Distributed.Wcf.Main.Contracts.AuxiliaresModulo;
using DigisatServer.Infrastructure.Crosscutting.Constant;
using DigisatServer.Infrastructure.Crosscutting.Dto.Auxiliares.OrigensLancamentosTurno;
using DigisatServer.Infrastructure.Crosscutting.Dto.Financeiros.EspeciesPagamento;
using DigisatServer.Infrastructure.Crosscutting.Dto.Relatorios;
using DigisatServer.Infrastructure.Crosscutting.Enum;
using DigisatServer.Infrastructure.Crosscutting.Helper;
using DigisatServer.Infrastructure.Crosscutting.Interfaces;
using DigisatServer.Infrastructure.Crosscutting.Model.OrigensLancamentosTurno;
using DigisatServer.Infrastructure.Crosscutting.Model.Sistemas;
using DigisatServer.Infrastructure.Crosscutting.Module;
using DigisatServer.Infrastructure.Crosscutting.Mvvm;
using DigisatServer.Infrastructure.Crosscutting.Mvvm.Interfaces;
using DigisatServer.Infrastructure.Crosscutting.Projection;
using DigisatServer.Infrastructure.Crosscutting.Projection.Financeiros;
using DigisatServer.Infrastructure.Crosscutting.Report.Models;
using DigisatServer.Infrastructure.Crosscutting.Right;
using DigisatServer.Infrastructure.Crosscutting.Style.ListViews;
using DigisatServer.Infrastructure.Crosscutting.ValueObject.FinanceirosVO.Especies.Recebimentos;
using Microsoft.Practices.Unity;
using Otus.Infraestrutura.Interfaces;
using Otus.Infraestrutura.Models;
using Otus.Infraestrutura.UserControls;
using Otus.Infraestrutura.ViewModels;
using Otus.Infraestrutura.ViewModels.Options;
using Otus.Infraestrutura.Views;
using Otus.Modulo.Turnos.Reports;
using Otus.Modulo.Turnos.ViewModels.Jornadas;
using Otus.Modulo.Turnos.ViewModels.Turnos;
using Otus.Modulo.Turnos.Views;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

[assembly: CompilationRelaxations(8)]
[assembly: RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: Debuggable(DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: AssemblyTitle("Otus.Modulo.Turnos")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Digisat Tecnologia Ltda")]
[assembly: AssemblyProduct("Otus.Modulo.Turnos")]
[assembly: AssemblyCopyright("Copyright ©  2016")]
[assembly: AssemblyTrademark("")]
[assembly: ComVisible(false)]
[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]
[assembly: AssemblyFileVersion("2.0.26293.5")]
[assembly: TargetFramework(".NETFramework,Version=v4.6", FrameworkDisplayName = ".NET Framework 4.6")]
[assembly: AssemblyVersion("1.0.0.0")]
namespace Otus.Modulo.Turnos
{
	[Module(ModuleName = "TurnoModulo", OnDemand = true)]
	[ModuleDependency("RelatorioModulo")]
	[ModuleDependency("UsuarioModulo")]
	public class TurnoModulo : IModule
	{
		private readonly IUnityContainer _container;

		public TurnoModulo(IUnityContainer container)
		{
			_container = container;
		}

		public void Initialize()
		{
			_container.RegisterTypeForNavigation<TurnosView>();
			_container.RegisterTypeForNavigation<TurnoListaView>();
			_container.RegisterTypeForNavigation<TurnoFichaAdicionarView>();
			_container.RegisterTypeForNavigation<TurnoFichaAtualizarView>();
			_container.RegisterTypeForNavigation<TurnoFechamentoFichaView>();
			_container.RegisterTypeForNavigation<TurnoConferenciaFechamentoFichaView>();
			_container.RegisterTypeForNavigation<TurnoMovimentacoesFichaView>();
			_container.RegisterTypeForNavigation<JornadaListaView>();
			_container.RegisterTypeForNavigation<JornadaAberturaFichaView>();
			_container.RegisterTypeForNavigation<JornadaFechamentoFichaView>();
		}
	}
}
namespace Otus.Modulo.Turnos.Views
{
	public class JornadaAberturaFichaView : System.Windows.Controls.UserControl, IComponentConnector
	{
		internal FichaBasePersistir FichaBase;

		private bool _contentLoaded;

		public JornadaAberturaFichaView()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Modulo.Turnos;component/views/jornadaaberturafichaview.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				FichaBase = (FichaBasePersistir)target;
			}
			else
			{
				_contentLoaded = true;
			}
		}
	}
	public class JornadaFechamentoFichaView : System.Windows.Controls.UserControl, IComponentConnector
	{
		internal FichaBasePersistir FichaBase;

		private bool _contentLoaded;

		public JornadaFechamentoFichaView()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Modulo.Turnos;component/views/jornadafechamentofichaview.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				FichaBase = (FichaBasePersistir)target;
			}
			else
			{
				_contentLoaded = true;
			}
		}
	}
	public class JornadaListaView : System.Windows.Controls.UserControl, IComponentConnector
	{
		internal ListViewFocusable lstResultadosPesquisa;

		private bool _contentLoaded;

		public JornadaListaView()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Modulo.Turnos;component/views/jornadalistaview.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				lstResultadosPesquisa = (ListViewFocusable)target;
			}
			else
			{
				_contentLoaded = true;
			}
		}
	}
	public class TurnoConferenciaFechamentoFichaView : System.Windows.Controls.UserControl, IComponentConnector
	{
		internal FichaBasePersistir FichaBase;

		private bool _contentLoaded;

		public TurnoConferenciaFechamentoFichaView()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Modulo.Turnos;component/views/turnoconferenciafechamentofichaview.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				FichaBase = (FichaBasePersistir)target;
			}
			else
			{
				_contentLoaded = true;
			}
		}
	}
	public class TurnoConferenciaFechamentoView : System.Windows.Controls.UserControl, IComponentConnector
	{
		private bool _contentLoaded;

		public TurnoConferenciaFechamentoView()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Modulo.Turnos;component/views/turnoconferenciafechamentoview.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			_contentLoaded = true;
		}
	}
	public class TurnoFichaAdicionarView : System.Windows.Controls.UserControl, IComponentConnector
	{
		internal FichaBasePersistir FichaBase;

		private bool _contentLoaded;

		public TurnoFichaAdicionarView()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Modulo.Turnos;component/views/turnofichaadicionarview.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				FichaBase = (FichaBasePersistir)target;
			}
			else
			{
				_contentLoaded = true;
			}
		}
	}
	public class TurnoFichaAtualizarView : System.Windows.Controls.UserControl, IComponentConnector
	{
		internal FichaBasePersistir FichaBase;

		private bool _contentLoaded;

		public TurnoFichaAtualizarView()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Modulo.Turnos;component/views/turnofichaatualizarview.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				FichaBase = (FichaBasePersistir)target;
			}
			else
			{
				_contentLoaded = true;
			}
		}
	}
	public class TurnoFechamentoFichaView : System.Windows.Controls.UserControl, IComponentConnector
	{
		internal FichaBasePersistir FichaBase;

		private bool _contentLoaded;

		public TurnoFechamentoFichaView()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Modulo.Turnos;component/views/turnofechamentofichaview.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				FichaBase = (FichaBasePersistir)target;
			}
			else
			{
				_contentLoaded = true;
			}
		}
	}
	public class TurnoListaView : System.Windows.Controls.UserControl, IComponentConnector
	{
		internal ListViewFocusable lstResultadosPesquisa;

		internal Grid DetalheLateral;

		internal System.Windows.Controls.Button btnDetalhes;

		internal KeyBinding kbdExibir;

		private bool _contentLoaded;

		public ICommand ExibirCommand { get; set; }

		public TurnoListaView()
		{
			InitializeComponent();
			ExibirCommand = new DelegateCommand(Exibir);
			kbdExibir.Command = ExibirCommand;
			btnDetalhes.DataContext = this;
		}

		public void Exibir()
		{
			if (btnDetalhes.RenderTransform is RotateTransform rotateTransform)
			{
				if (Math.Abs(rotateTransform.Angle) <= 0.0)
				{
					DetalheLateral.Width = 500.0;
					rotateTransform.Angle += 180.0;
				}
				else
				{
					DetalheLateral.Width = 35.0;
					rotateTransform.Angle = 0.0;
				}
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Modulo.Turnos;component/views/turnolistaview.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				lstResultadosPesquisa = (ListViewFocusable)target;
				break;
			case 2:
				DetalheLateral = (Grid)target;
				break;
			case 3:
				btnDetalhes = (System.Windows.Controls.Button)target;
				break;
			case 4:
				kbdExibir = (KeyBinding)target;
				break;
			default:
				_contentLoaded = true;
				break;
			}
		}
	}
	public class TurnoMovimentacoesFichaView : System.Windows.Controls.UserControl, IComponentConnector
	{
		internal FichaBasePersistir FichaBase;

		private bool _contentLoaded;

		public TurnoMovimentacoesFichaView()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Modulo.Turnos;component/views/turnomovimentacoesfichaview.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				FichaBase = (FichaBasePersistir)target;
			}
			else
			{
				_contentLoaded = true;
			}
		}
	}
	public class TurnosView : System.Windows.Controls.UserControl, IComponentConnector
	{
		private bool _contentLoaded;

		public TurnosView()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Modulo.Turnos;component/views/turnosview.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			_contentLoaded = true;
		}
	}
}
namespace Otus.Modulo.Turnos.Views.TurnoFichaViewDetalhes
{
	public class TurnoAberturaView : System.Windows.Controls.UserControl, IComponentConnector
	{
		private bool _contentLoaded;

		public TurnoAberturaView()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Modulo.Turnos;component/views/turnofichaviewdetalhes/turnoaberturaview.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			_contentLoaded = true;
		}
	}
	public class TurnoFechamentoView : System.Windows.Controls.UserControl, IComponentConnector
	{
		private bool _contentLoaded;

		public TurnoFechamentoView()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Modulo.Turnos;component/views/turnofichaviewdetalhes/turnofechamentoview.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			_contentLoaded = true;
		}
	}
	public class TurnoMovimentacaoFichaView : CustomCrudUserControl, IComponentConnector
	{
		private bool _contentLoaded;

		public TurnoMovimentacaoFichaView()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Modulo.Turnos;component/views/turnofichaviewdetalhes/turnomovimentacaofichaview.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			_contentLoaded = true;
		}
	}
	public class TurnoMovimentacaoListaView : CustomCrudUserControl, IComponentConnector
	{
		internal TurnoMovimentacaoListaView ListaDetalhe;

		private bool _contentLoaded;

		public TurnoMovimentacaoListaView()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Modulo.Turnos;component/views/turnofichaviewdetalhes/turnomovimentacaolistaview.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 1)
			{
				ListaDetalhe = (TurnoMovimentacaoListaView)target;
			}
			else
			{
				_contentLoaded = true;
			}
		}
	}
}
namespace Otus.Modulo.Turnos.Views.Links
{
	public class LinkFecharTurno : System.Windows.Controls.UserControl, IComponentConnector
	{
		private bool _contentLoaded;

		public LinkFecharTurno()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Modulo.Turnos;component/views/links/linkfecharturno.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			_contentLoaded = true;
		}
	}
	public class LinkImprimirResumoConferencia : System.Windows.Controls.UserControl, IComponentConnector
	{
		private bool _contentLoaded;

		public LinkImprimirResumoConferencia()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Modulo.Turnos;component/views/links/linkimprimirresumoconferencia.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			_contentLoaded = true;
		}
	}
	public class LinkImprimir : System.Windows.Controls.UserControl, IComponentConnector
	{
		private bool _contentLoaded;

		public LinkImprimir()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Modulo.Turnos;component/views/links/linkimprimir.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			_contentLoaded = true;
		}
	}
	public class LinkMovimentacoesTurno : System.Windows.Controls.UserControl, IComponentConnector
	{
		private bool _contentLoaded;

		public LinkMovimentacoesTurno()
		{
			InitializeComponent();
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			if (!_contentLoaded)
			{
				_contentLoaded = true;
				Uri resourceLocator = new Uri("/Modulo.Turnos;component/views/links/linkmovimentacoesturno.xaml", UriKind.Relative);
				System.Windows.Application.LoadComponent(this, resourceLocator);
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			_contentLoaded = true;
		}
	}
}
namespace Otus.Modulo.Turnos.ViewModels
{
	public class JornadaFechamentoFichaViewModel : FichaBasePersistir<JornadaDto, JornadaFechamentoViewModel>
	{
		private readonly IWService _service;

		public JornadaFechamentoFichaViewModel(IWService service)
			: base("")
		{
			_service = service;
		}

		protected override async Task<JornadaDto> Persistir(IFormularioEntity formulario)
		{
			JornadaDto jornadaFechamento = formulario.ToDto<JornadaDto>();
			await _service.Canal((IJornadaModuleService s) => s.Fechar(jornadaFechamento));
			return jornadaFechamento;
		}
	}
	public class JornadaListaViewModel : ListaSelecaoBase<JornadaDto, JornadaViewModel, Jornada>
	{
		private readonly IWService _service;

		public JornadaListaViewModel(IWService service, ISessao sessao, IEntityAtivos<JornadaDto, IJornadaModuleService> ativos)
			: base(sessao)
		{
			Pesquisa.AdicionarOpcao(ativos as OpcaoBase);
			_service = service;
		}
	}
	public class JornadaAberturaFichaViewModel : FichaBasePersistir<JornadaDto, JornadaAberturaViewModel>
	{
		private readonly IWService _service;

		public JornadaAberturaFichaViewModel(IWService service)
			: base("")
		{
			_service = service;
		}

		protected override async Task<JornadaDto> Persistir(IFormularioEntity formulario)
		{
			JornadaDto jornadaAbertura = formulario.ToDto<JornadaDto>();
			await _service.Canal((IJornadaModuleService s) => s.Abrir(jornadaAbertura));
			return jornadaAbertura;
		}
	}
	public class JornadaViewModel : ViewModelDtoBase<JornadaDto>, IViewModelTitulo
	{
		public JornadaViewModel()
		{
		}

		public JornadaViewModel(JornadaDto jornadaDto)
			: base(jornadaDto)
		{
		}

		public override void ProjectedAsViewModel()
		{
		}

		public string ToTituloLista()
		{
			return "Jornadas";
		}
	}
	public class TurnoConferenciaFechamentoFichaViewModel : FichaBasePersistir<TurnoDto, TurnoConferenciaFechamentoViewModel>
	{
		private readonly IWService _service;

		public TurnoConferenciaFechamentoFichaViewModel(IWService service)
			: base("Conferência do fechamento de Turno")
		{
			_service = service;
		}

		protected override async Task<TurnoDto> Persistir(IFormularioEntity formulario)
		{
			if (DadosSistema.AutenticacaoService.Instancia.VerificarPermissao<Turno, PermitirConferirTurno>() == null)
			{
				throw new Exception("O turno não será conferido");
			}
			TurnoDto dto = await _service.Canal((ITurnoModuleService service) => service.ConferirTurnoAsync(formulario.ToDto<TurnoDto>()));
			await ImprimirResumoTurno(dto);
			SelecaoNavigateParameterCallBack?.CallBack(dto);
			return dto;
		}

		private async Task ImprimirResumoTurno(TurnoDto turnoDto)
		{
			if (DialogoFactory.MostrarDialogoSimNao("Turno conferido! Deseja imprimir o resumo do turno?") == MessageBoxResult.Yes)
			{
				new ResumoConferenciaTurno(await _service.Canal((ITurnoRelatorioModuleService service) => service.ObterResumoConferenciaTurno(turnoDto.Id))).Processar().ShowPreviewDialog();
			}
		}
	}
	public class TurnoConferenciaFechamentoFormulario : FormularioEntity<TurnoDto>
	{
		public TurnoConferenciaFechamentoFormulario(TurnoDto dto)
			: base(dto)
		{
			CriarAbas();
		}

		private void CriarAbas()
		{
			AdicionarProjetarAbas(new TurnoConferenciaFechamentoAbaViewModel(new TurnoConferenciaFechamentoViewModel(Dto)));
		}
	}
	public class TurnoConferenciaFechamentoAbaViewModel : AbaPrincipalViewModelBase
	{
		public TurnoConferenciaFechamentoAbaViewModel(IViewModelPrincipal viewModel)
			: base("CONFERÊNCIA DE FECHAMENTO", "Conferência do fechamento de turno", viewModel)
		{
		}
	}
	public class TurnoFechamentoFormulario : FormularioEntity<TurnoDto>
	{
		public TurnoFechamentoFormulario(TurnoDto dto)
			: base(dto)
		{
			CriarAbas();
		}

		private void CriarAbas()
		{
			AdicionarProjetarAbas(new TurnoFechamentoAbaViewModel(new TurnoFechamentoViewModel(Dto)));
		}
	}
	public class TurnoConferenciaFechamentoViewModel : ViewModelDtoBase<TurnoDto>, IViewModelPrincipal, IViewModelTitulo, IViewModelDtoBase, IViewModelBase
	{
		public decimal TotalEspecie
		{
			get
			{
				return GetProperty(() => TotalEspecie);
			}
			set
			{
				SetProperty(() => TotalEspecie, value, CalcularTotal);
			}
		}

		public decimal DiferencaEspecie => TotalEspecie - Dto.Fechamento.TotalEspecie;

		public decimal TotalCheque
		{
			get
			{
				return GetProperty(() => TotalCheque);
			}
			set
			{
				SetProperty(() => TotalCheque, value, CalcularTotal);
			}
		}

		public decimal DiferencaCheque => TotalCheque - Dto.Fechamento.TotalCheque;

		public decimal TotalCartao
		{
			get
			{
				return GetProperty(() => TotalCartao);
			}
			set
			{
				SetProperty(() => TotalCartao, value, CalcularTotal);
			}
		}

		public decimal DiferencaCartao => TotalCartao - Dto.Fechamento.TotalCartao;

		public decimal TotalVale
		{
			get
			{
				return GetProperty(() => TotalVale);
			}
			set
			{
				SetProperty(() => TotalVale, value, CalcularTotal);
			}
		}

		public decimal DiferencaVale => TotalVale - Dto.Fechamento.TotalVale;

		public decimal TotalValePresente
		{
			get
			{
				return GetProperty(() => TotalValePresente);
			}
			set
			{
				SetProperty(() => TotalValePresente, value, CalcularTotal);
			}
		}

		public decimal DiferencaValePresente => TotalValePresente - Dto.Fechamento.TotalValePresente;

		public decimal TotalCreditoLoja
		{
			get
			{
				return GetProperty(() => TotalCreditoLoja);
			}
			set
			{
				SetProperty(() => TotalCreditoLoja, value, CalcularTotal);
			}
		}

		public decimal DiferencaCreditoLoja => TotalCreditoLoja - Dto.Fechamento.TotalCreditoLoja;

		public decimal TotalCarteirasDigitais
		{
			get
			{
				return GetProperty(() => TotalCarteirasDigitais);
			}
			set
			{
				SetProperty(() => TotalCarteirasDigitais, value, CalcularTotal);
			}
		}

		public decimal DiferencaCarteirasDigitais => TotalCarteirasDigitais - Dto.Fechamento.TotalCarteirasDigitais;

		public decimal TotalOutrasEspecies
		{
			get
			{
				return GetProperty(() => TotalOutrasEspecies);
			}
			set
			{
				SetProperty(() => TotalOutrasEspecies, value, CalcularTotal);
			}
		}

		public decimal DiferencaOutrasEspecies => TotalOutrasEspecies - Dto.Fechamento.TotalOutrasEspecies;

		public decimal TotalDepositoBancario
		{
			get
			{
				return GetProperty(() => TotalDepositoBancario);
			}
			set
			{
				SetProperty(() => TotalDepositoBancario, value, CalcularTotal);
			}
		}

		public decimal DiferencaDepositoBancario => TotalDepositoBancario - Dto.Fechamento.TotalDepositoBancario;

		public decimal TotalTed
		{
			get
			{
				return GetProperty(() => TotalTed);
			}
			set
			{
				SetProperty(() => TotalTed, value, CalcularTotal);
			}
		}

		public decimal DiferencaTed => TotalTed - Dto.Fechamento.TotalTed;

		public decimal TotalDoc
		{
			get
			{
				return GetProperty(() => TotalDoc);
			}
			set
			{
				SetProperty(() => TotalDoc, value, CalcularTotal);
			}
		}

		public decimal DiferencaDoc => TotalDoc - Dto.Fechamento.TotalDoc;

		public decimal TotalPrazo
		{
			get
			{
				return GetProperty(() => TotalPrazo);
			}
			set
			{
				SetProperty(() => TotalPrazo, value, CalcularTotal);
			}
		}

		public decimal DiferencaPrazo => TotalPrazo - Dto.Fechamento.TotalPrazo;

		public bool PermitirAlterarDataHoraAberturaFechamentoTurno => DadosSistema.VerificarPermissao<Turno, PermitirAlterarDataHoraAberturaFechamentoTurno>();

		public DateTime DataHoraFechamento
		{
			get
			{
				return GetProperty(() => DataHoraFechamento);
			}
			set
			{
				SetProperty(() => DataHoraFechamento, value, CalcularTotal);
			}
		}

		public string Observacao
		{
			get
			{
				return GetProperty(() => Observacao);
			}
			set
			{
				SetProperty(() => Observacao, value, CalcularTotal);
			}
		}

		public decimal TotalGeral
		{
			get
			{
				return GetProperty(() => TotalGeral);
			}
			set
			{
				SetProperty(() => TotalGeral, value);
			}
		}

		public decimal TotalDiferenca
		{
			get
			{
				return GetProperty(() => TotalDiferenca);
			}
			set
			{
				SetProperty(() => TotalDiferenca, value);
			}
		}

		public TurnoConferenciaFechamentoViewModel()
		{
		}

		public TurnoConferenciaFechamentoViewModel(TurnoDto dto)
			: base(dto)
		{
		}

		public override void ProjectedAsViewModel()
		{
			DataHoraFechamento = (Dto.TurnoConferido ? Dto.Conferencia.DataHoraFechamento : DateTime.Now);
			TotalEspecie = Dto.Conferencia.TotalEspecie;
			TotalCheque = Dto.Conferencia.TotalCheque;
			TotalCartao = Dto.Conferencia.TotalCartao;
			TotalVale = Dto.Conferencia.TotalVale;
			TotalValePresente = Dto.Conferencia.TotalValePresente;
			TotalCreditoLoja = Dto.Conferencia.TotalCreditoLoja;
			TotalCarteirasDigitais = Dto.Conferencia.TotalCarteirasDigitais;
			TotalOutrasEspecies = Dto.Conferencia.TotalOutrasEspecies;
			TotalDepositoBancario = Dto.Conferencia.TotalDepositoBancario;
			TotalTed = Dto.Conferencia.TotalTed;
			TotalDoc = Dto.Conferencia.TotalDoc;
			TotalPrazo = Dto.Conferencia.TotalPrazo;
			Observacao = Dto.Conferencia.Observacao;
		}

		public override void AtualizarDto<T>(ref T dto)
		{
			base.AtualizarDto(ref dto);
			(dto as TurnoDto).Conferencia = new FechamentoDto
			{
				DataHoraFechamento = DataHoraFechamento,
				TotalEspecie = TotalEspecie,
				TotalCheque = TotalCheque,
				TotalCartao = TotalCartao,
				TotalVale = TotalVale,
				TotalValePresente = TotalValePresente,
				TotalCreditoLoja = TotalCreditoLoja,
				TotalCarteirasDigitais = TotalCarteirasDigitais,
				TotalOutrasEspecies = TotalOutrasEspecies,
				TotalDepositoBancario = TotalDepositoBancario,
				TotalTed = TotalTed,
				TotalDoc = TotalDoc,
				TotalPrazo = TotalPrazo,
				Observacao = Observacao
			};
		}

		private void CalcularTotal()
		{
			TotalGeral = TotalEspecie + TotalCheque + TotalCartao + TotalVale + TotalValePresente + TotalCreditoLoja + TotalCarteirasDigitais + TotalOutrasEspecies + TotalDepositoBancario + TotalTed + TotalDoc + TotalPrazo;
			TotalDiferenca = DiferencaEspecie + DiferencaCheque + DiferencaCartao + DiferencaVale + DiferencaValePresente + DiferencaCreditoLoja + DiferencaCarteirasDigitais + DiferencaOutrasEspecies + DiferencaDepositoBancario + DiferencaTed + DiferencaDoc + DiferencaPrazo;
			RaisePropertyChanged(() => DiferencaEspecie);
			RaisePropertyChanged(() => DiferencaCheque);
			RaisePropertyChanged(() => DiferencaCartao);
			RaisePropertyChanged(() => DiferencaVale);
			RaisePropertyChanged(() => DiferencaValePresente);
			RaisePropertyChanged(() => DiferencaCreditoLoja);
			RaisePropertyChanged(() => DiferencaCarteirasDigitais);
			RaisePropertyChanged(() => DiferencaOutrasEspecies);
			RaisePropertyChanged(() => DiferencaDepositoBancario);
			RaisePropertyChanged(() => DiferencaTed);
			RaisePropertyChanged(() => DiferencaDoc);
			RaisePropertyChanged(() => DiferencaPrazo);
		}

		public TurnoDto ObterDto()
		{
			return Dto;
		}

		public string ToTituloLista()
		{
			return string.Empty;
		}

		public IFormulario CriarFormulario()
		{
			return new TurnoConferenciaFechamentoFormulario(Dto);
		}

		public string ToTituloFicha()
		{
			return string.Empty;
		}
	}
	public class TurnoFichaAdicionarViewModel : FichaBaseAdicionar<TurnoDto, TurnoViewModel, ITurnoModuleService>
	{
		public TurnoFichaAdicionarViewModel(IWService service)
			: base(service)
		{
		}

		protected override Task<TurnoDto> Persistir(IFormularioEntity formulario)
		{
			Task<TurnoDto> result = base.Persistir(formulario);
			DadosSistema.SetarTurnoAbertoDoUsuario(turnoAberto: true);
			return result;
		}
	}
	public class TurnoFichaAtualizarViewModel : FichaBaseAtualizar<TurnoDto, TurnoViewModel, ITurnoModuleService>
	{
		public TurnoFichaAtualizarViewModel(IWService service)
			: base(service)
		{
		}
	}
	public class TurnoFormulario : FormularioEntity<TurnoDto>
	{
		public TurnoFormulario(TurnoDto dto)
			: base(dto)
		{
			AdicionarProjetarAbas(new TurnoAberturaAbaViewModel(new TurnoAberturaViewModel(Dto)));
		}
	}
	public class TurnoMovimentacoesFichaViewModel : FichaBasePersistir<TurnoDto, TurnoMovimentacaoViewModel>
	{
		private readonly IWService _service;

		public TurnoMovimentacoesFichaViewModel(IWService service)
			: base("Movimentações do Turno")
		{
			_service = service;
		}

		protected override async Task Gravar()
		{
			try
			{
				LoaderProcessoService.MostrarLoader("Gravando...");
				await Persistir(Formulario);
				ParametroRetornado += OnParametroRetornado;
				DisableConfirmOnClose();
				FecharView();
			}
			catch (Exception ex)
			{
				DialogoFactory.MostrarDialogoConfirma(ex.Message);
			}
			finally
			{
				LoaderProcessoService.EsconderLoader();
			}
		}

		protected override Task<TurnoDto> Persistir(IFormularioEntity formulario)
		{
			throw new NotImplementedException();
		}

		private Task Persistir(IFormulario formulario)
		{
			List<TurnoLancamentoDto> turnoMovimentacoes = (((TurnoMovimentacoesFormulario)formulario).Abas.FirstOrDefault((IAbaViewModelBase x) => x is TurnoMovimentacaoAbaViewModel).ViewModel as TurnoMovimentacaoViewModel)?.ObterDtos().ToList();
			_service.Canal((ITurnoModuleService service) => service.AtualizarMovimentacoesAsync(turnoMovimentacoes));
			SelecaoNavigateParameterCallBack?.CallBack(null);
			TurnoDto turno = new TurnoDto();
			return Task.Factory.StartNew(() => turno);
		}
	}
	internal class TurnoMovimentacoesFormulario : Formulario
	{
		public TurnoMovimentacoesFormulario(IEnumerable<TurnoLancamentoDto> dto)
		{
			AdicionarProjetarAbas(new TurnoMovimentacaoAbaViewModel(new TurnoMovimentacaoViewModel(dto)));
		}
	}
	public class TurnoFechamentoFichaViewModel : FichaBasePersistir<TurnoDto, TurnoFechamentoViewModel>
	{
		private readonly IWService _service;

		public TurnoFechamentoFichaViewModel(IWService service)
			: base("Fechamento de Turno")
		{
			_service = service;
		}

		protected override async Task<TurnoDto> Persistir(IFormularioEntity formulario)
		{
			if (DadosSistema.AutenticacaoService.Instancia.VerificarPermissao<Turno, PermitirFecharTurno>() == null)
			{
				throw new Exception("O turno não será fechado");
			}
			TurnoDto dto = await _service.Canal((ITurnoModuleService service) => service.FecharTurnoAsync(formulario.ToDto<TurnoDto>()));
			DadosSistema.SetarTurnoAbertoDoUsuario(turnoAberto: false);
			await ImprimirResumoTurno(dto);
			SelecaoNavigateParameterCallBack?.CallBack(dto);
			return dto;
		}

		private async Task ImprimirResumoTurno(TurnoDto turnoDto)
		{
			if (DialogoFactory.MostrarDialogoSimNao("Turno fechado! Deseja imprimir o resumo do turno?") == MessageBoxResult.Yes)
			{
				new ResumoTurno(await _service.Canal((ITurnoRelatorioModuleService service) => service.ObterResumoTurno(turnoDto.Id))).Processar().ShowPreviewDialog();
			}
		}
	}
	public class TurnoListaViewModel : ListaManutencaoBase<TurnoDto, TurnoViewModel, Turno>
	{
		private readonly IWService _service;

		private JornadaDto _jornada;

		public bool Ativo
		{
			get
			{
				if (Itens != null && Itens.SelecionadoValido)
				{
					return Itens.Selecionado.Ativo;
				}
				return false;
			}
		}

		public DelegateCommand ImprimirCommand { get; set; }

		public DelegateCommand ImprimirResumoConferenciaCommand { get; set; }

		public DelegateCommand MovimentacoesTurnoCommand { get; set; }

		public DelegateCommand FecharTurnoCommand { get; set; }

		public DelegateCommand CorrigirFechamentoTurnoCommand { get; set; }

		public DelegateCommand AbrirJornadaCommand { get; set; }

		public DelegateCommand FecharJornadaCommand { get; set; }

		public ICommand VisualizarRelatorioCommand { get; set; }

		public ICommand ConferirFechamentoTurnoCommand { get; set; }

		public TurnoListaViewModel(IWService service, ISessao sessao, IEntityAtivoFiltroTurno<TurnoDto, ITurnoModuleService> ativos, IEntityInativoFiltroTurno<TurnoDto, ITurnoModuleService> inativos)
			: base(sessao, typeof(TurnoFichaAdicionarView).Name, typeof(TurnoFichaAtualizarView).Name)
		{
			_service = service;
			ImprimirCommand = new DelegateCommand(Imprimir, PodeImprimir);
			ImprimirResumoConferenciaCommand = new DelegateCommand(ImprimirResumoConferencia, PodeImprimirResumoConferencia);
			MovimentacoesTurnoCommand = new DelegateCommand(MovimentacoesTurno, PodeMovimentacoesTurno);
			FecharTurnoCommand = new DelegateCommand(FecharTurno, PodeFecharTurno);
			CorrigirFechamentoTurnoCommand = new DelegateCommand(CorrigirFechamentoTurno, PodeCorrigirFechamentoTurno);
			AbrirJornadaCommand = new DelegateCommand(AbrirJornada, PodeAbrirJornada);
			FecharJornadaCommand = new DelegateCommand(FecharJornada, PodeFecharJornada);
			VisualizarRelatorioCommand = new DelegateCommand(Visualizar, PodeVisualizar);
			ConferirFechamentoTurnoCommand = new DelegateCommand(ConferirFechamentoTurno, PodeConferirFechamentoTurno);
			Pesquisa.AdicionarOpcao(ativos as OpcaoBase);
			Pesquisa.AdicionarOpcao(inativos as OpcaoBase);
			ParametroInformado += OnInicializar;
		}

		private async void Imprimir()
		{
			try
			{
				new ResumoTurno(await _service.Canal((ITurnoRelatorioModuleService service) => service.ObterResumoTurno(Itens.Selecionado.Dto.Id))).Processar().ShowPreviewDialog();
			}
			catch (Exception ex)
			{
				DialogoFactory.MostrarDialogoConfirma(ex.Message);
			}
		}

		private bool PodeImprimir()
		{
			if (Itens != null && Itens.SelecionadoValido && Itens.Selecionado.Ativo)
			{
				return Itens.Selecionado.TurnoFechado;
			}
			return false;
		}

		private async void ImprimirResumoConferencia()
		{
			try
			{
				new ResumoConferenciaTurno(await _service.Canal((ITurnoRelatorioModuleService service) => service.ObterResumoConferenciaTurno(Itens.Selecionado.Dto.Id))).Processar().ShowPreviewDialog();
			}
			catch (Exception ex)
			{
				DialogoFactory.MostrarDialogoConfirma(ex.Message);
			}
		}

		private bool PodeImprimirResumoConferencia()
		{
			if (Itens != null && Itens.SelecionadoValido && Itens.Selecionado.Ativo)
			{
				return Itens.Selecionado.TurnoConferido;
			}
			return false;
		}

		private async void MovimentacoesTurno()
		{
			try
			{
				TurnoDto dto = Itens.Selecionado.Dto;
				IEnumerable<TurnoLancamentoDto> dto2 = await _service.Canal((ITurnoModuleService service) => service.ObterMovimentacoesDoTurnoAsync(dto));
				Navegar(typeof(TurnoMovimentacoesFichaView).Name, (NavigationResult result) =>
				{
				}, new NavigationParameters { 
				{
					typeof(TurnoMovimentacaoViewModel).Name,
					new TurnoMovimentacaoViewModel(dto2)
				} });
			}
			catch (Exception ex)
			{
				DialogoFactory.MostrarDialogoConfirma(ex.Message);
			}
		}

		private bool PodeMovimentacoesTurno()
		{
			if (Itens != null && Itens.SelecionadoValido)
			{
				return Itens.Selecionado.Ativo;
			}
			return false;
		}

		private void FecharTurno()
		{
			TurnoDto dto = Itens.Selecionado.Dto;
			Navegar(typeof(TurnoFechamentoFichaView).Name, (NavigationResult result) =>
			{
			}, new NavigationParameters { 
			{
				typeof(TurnoFechamentoViewModel).Name,
				new TurnoFechamentoViewModel(dto)
			} });
		}

		private bool PodeFecharTurno()
		{
			if (Itens.Selecionado.IsNotNull() && Itens.Selecionado.PodeFecharTurno && Itens.Selecionado.Ativo && !Itens.Selecionado.TurnoFechado)
			{
				return !Itens.Selecionado.TurnoConferido;
			}
			return false;
		}

		private bool PodeCorrigirFechamentoTurno()
		{
			if (Itens != null && Itens.SelecionadoValido && Itens.Selecionado.Ativo && Itens.Selecionado.TurnoFechado)
			{
				return !Itens.Selecionado.TurnoConferido;
			}
			return false;
		}

		private void CorrigirFechamentoTurno()
		{
			if (DadosSistema.AutenticacaoService.Instancia.VerificarPermissao<Turno, PermitirCorrigirFechamentoTurno>() != null)
			{
				TurnoDto dto = Itens.Selecionado.Dto;
				Navegar(typeof(TurnoFechamentoFichaView).Name, (NavigationResult result) =>
				{
				}, new NavigationParameters { 
				{
					typeof(TurnoFechamentoViewModel).Name,
					new TurnoFechamentoViewModel(dto)
				} });
			}
		}

		private void AbrirJornada()
		{
			JornadaDto dto = ObterDadosAberturaJornada();
			Navegar("JornadaAberturaFichaView", (NavigationResult result) =>
			{
			}, new NavigationParameters { 
			{
				typeof(JornadaAberturaViewModel).Name,
				new JornadaAberturaViewModel(dto)
			} });
		}

		private JornadaDto ObterDadosAberturaJornada()
		{
			if (Sessao.Perfis.ItemSelecionado.Empresa.RamoAtividade.Tipo != RamoDeAtividade.PostoCombustivel)
			{
				return new JornadaDto();
			}
			return _service.Canal((IJornadaModuleService service) => service.ObterDadosAberturaJornada());
		}

		private bool PodeAbrirJornada()
		{
			if (Sessao.VerificarPermissao<Jornada, DigisatServer.Infrastructure.Crosscutting.Right.Adicionar>())
			{
				return _jornada.Is<JornadaDtoNull>();
			}
			return false;
		}

		private void FecharJornada()
		{
			Navegar("JornadaFechamentoFichaView", (NavigationResult result) =>
			{
			}, new NavigationParameters { 
			{
				typeof(JornadaFechamentoViewModel).Name,
				new JornadaFechamentoViewModel(_jornada)
			} });
		}

		private bool PodeFecharJornada()
		{
			if (Sessao.VerificarPermissao<Jornada, DigisatServer.Infrastructure.Crosscutting.Right.Atualizar>())
			{
				return _jornada.IsNot<JornadaDtoNull>();
			}
			return false;
		}

		private bool PodeVisualizar()
		{
			if (DadosSistema.VerificarPermissao<Turno, DigisatServer.Infrastructure.Crosscutting.Right.VisualizarRelatorio>() && DadosSistema.PodeExecutarFuncao(typeof(Administrador), typeof(AdministradorIndustria)))
			{
				return !PodeSelecionar();
			}
			return false;
		}

		private void Visualizar()
		{
			Resolucao.To<ResolucaoBase>().Maximizar();
			NavigationParameters navigationParameters = new NavigationParameters
			{
				{ "ItemModuloCreator", 67 },
				{
					"VoltarAction",
					(Action)(() =>
					{
						Resolucao.To<ResolucaoBase>().Restaurar();
					})
				}
			};
			Navegar("RelatorioFichaView", (NavigationResult navigationResult) =>
			{
			}, navigationParameters);
		}

		private void OnInicializar(object sender, NavigationParameters navigationParameters)
		{
			IniciarAberturaJornada();
		}

		private void IniciarAberturaJornada()
		{
			_jornada = _service.Canal((IJornadaModuleService service) => service.ObterJornadaAberta()).Result;
		}

		private bool PodeConferirFechamentoTurno()
		{
			if (Itens != null && Itens.SelecionadoValido && Itens.Selecionado.Ativo && Itens.Selecionado.TurnoFechado)
			{
				return !Itens.Selecionado.TurnoConferido;
			}
			return false;
		}

		private void ConferirFechamentoTurno()
		{
			TurnoDto dto = Itens.Selecionado.Dto;
			List<DadosRelatorioConferenciaTurnoItemDto> especiesRecebimento = _service.Canal((ITurnoRelatorioModuleService x) => x.ObterConferenciaTurno(dto.Id)).Result.EspeciesRecebimento;
			dto.Fechamento.EspeciesRecebimento = especiesRecebimento;
			Navegar(typeof(TurnoConferenciaFechamentoFichaView).Name, (NavigationResult result) =>
			{
			}, new NavigationParameters { 
			{
				typeof(TurnoConferenciaFechamentoViewModel).Name,
				new TurnoConferenciaFechamentoViewModel(dto)
			} });
		}

		protected override bool PodeAdicionar()
		{
			return true;
		}

		public override void Dispose()
		{
			ParametroInformado -= OnInicializar;
			base.Dispose();
		}
	}
	public class TurnosViewModel : ModuloBase
	{
		public TurnosViewModel(IRegionManager regionManager)
			: base(regionManager, Regioes.ModuloTurnosRegion, typeof(TurnoListaView).Name, new ResolucaoBase())
		{
		}
	}
	public class TurnoViewModel : ViewModelAtivoBase<TurnoDto>, IViewModelPrincipal, IViewModelTitulo, IViewModelDtoBase, IViewModelBase
	{
		public bool IsTransiente
		{
			get
			{
				return GetProperty(() => IsTransiente);
			}
			set
			{
				SetProperty(() => IsTransiente, value);
			}
		}

		public bool PodeFecharTurno
		{
			get
			{
				return GetProperty(() => PodeFecharTurno);
			}
			set
			{
				SetProperty(() => PodeFecharTurno, value);
			}
		}

		public bool TurnoFechado
		{
			get
			{
				return GetProperty(() => TurnoFechado);
			}
			set
			{
				SetProperty(() => TurnoFechado, value);
			}
		}

		public bool TurnoConferido
		{
			get
			{
				return GetProperty(() => TurnoConferido);
			}
			set
			{
				SetProperty(() => TurnoConferido, value);
			}
		}

		public bool PermitirAlterarDataHoraAberturaFechamentoTurno => DadosSistema.VerificarPermissao<Turno, PermitirAlterarDataHoraAberturaFechamentoTurno>();

		public DateTime DataHoraAbertura
		{
			get
			{
				return GetProperty(() => DataHoraAbertura);
			}
			set
			{
				SetProperty(() => DataHoraAbertura, value);
			}
		}

		public UsuarioDto Usuario
		{
			get
			{
				return GetProperty(() => Usuario);
			}
			set
			{
				SetProperty(() => Usuario, value);
			}
		}

		public decimal SaldoInicial
		{
			get
			{
				return GetProperty(() => SaldoInicial);
			}
			set
			{
				SetProperty(() => SaldoInicial, value);
			}
		}

		public string Observacao
		{
			get
			{
				return GetProperty(() => Observacao);
			}
			set
			{
				SetProperty(() => Observacao, value);
			}
		}

		public TurnoViewModel()
		{
		}

		public TurnoViewModel(TurnoDto dto)
			: base(dto)
		{
		}

		public override void ProjectedAsViewModel()
		{
			Ativo = Dto.Ativo;
			DataHoraAbertura = Dto.DataHoraAbertura;
			SaldoInicial = Dto.SaldoInicial;
			Observacao = Dto.Observacao.ToStringEmptyWhenNull();
			Usuario = Dto.Usuario;
			TurnoFechado = Dto.TurnoFechado;
			TurnoConferido = Dto.TurnoConferido;
			PodeFecharTurno = DadosSistema.VerificarPermissao<Turno, PermitirFecharTurno>() && !Dto.TurnoFechado;
			IsTransiente = Dto.IsTransient();
		}

		public override void AtualizarDto<T>(ref T dto)
		{
			TurnoDto turnoDto = dto as TurnoDto;
			if (!turnoDto.IsNull())
			{
				turnoDto.DataHoraAbertura = DataHoraAbertura;
				turnoDto.SaldoInicial = SaldoInicial;
				turnoDto.Observacao = Observacao;
				turnoDto.Ativo = Ativo;
				base.AtualizarDto(ref dto);
			}
		}

		public string ToTituloLista()
		{
			return "Controle de turnos";
		}

		public IFormulario CriarFormulario()
		{
			return new TurnoFormulario(Dto);
		}

		public string ToTituloFicha()
		{
			return "Abertura do turno";
		}
	}
}
namespace Otus.Modulo.Turnos.ViewModels.Turnos
{
	public class ContagemDeCedulasMoedas : BindableBase
	{
		public decimal TotalEspecie
		{
			get
			{
				return GetProperty(() => TotalEspecie);
			}
			set
			{
				SetProperty(() => TotalEspecie, value);
			}
		}

		public int Moeda01
		{
			get
			{
				return GetProperty(() => Moeda01);
			}
			set
			{
				SetProperty(() => Moeda01, value, () =>
				{
					ValorMoeda01 = (decimal)Moeda01 * 0.01m;
					ObterSomatoriaReal();
				});
			}
		}

		public decimal ValorMoeda01
		{
			get
			{
				return GetProperty(() => ValorMoeda01);
			}
			set
			{
				SetProperty(() => ValorMoeda01, value);
			}
		}

		public int Moeda05
		{
			get
			{
				return GetProperty(() => Moeda05);
			}
			set
			{
				SetProperty(() => Moeda05, value, () =>
				{
					ValorMoeda05 = (decimal)Moeda05 * 0.05m;
					ObterSomatoriaReal();
				});
			}
		}

		public decimal ValorMoeda05
		{
			get
			{
				return GetProperty(() => ValorMoeda05);
			}
			set
			{
				SetProperty(() => ValorMoeda05, value);
			}
		}

		public int Moeda10
		{
			get
			{
				return GetProperty(() => Moeda10);
			}
			set
			{
				SetProperty(() => Moeda10, value, () =>
				{
					ValorMoeda10 = (decimal)Moeda10 * 0.10m;
					ObterSomatoriaReal();
				});
			}
		}

		public decimal ValorMoeda10
		{
			get
			{
				return GetProperty(() => ValorMoeda10);
			}
			set
			{
				SetProperty(() => ValorMoeda10, value);
			}
		}

		public int Moeda25
		{
			get
			{
				return GetProperty(() => Moeda25);
			}
			set
			{
				SetProperty(() => Moeda25, value, () =>
				{
					ValorMoeda25 = (decimal)Moeda25 * 0.25m;
					ObterSomatoriaReal();
				});
			}
		}

		public decimal ValorMoeda25
		{
			get
			{
				return GetProperty(() => ValorMoeda25);
			}
			set
			{
				SetProperty(() => ValorMoeda25, value);
			}
		}

		public int Moeda50
		{
			get
			{
				return GetProperty(() => Moeda50);
			}
			set
			{
				SetProperty(() => Moeda50, value, () =>
				{
					ValorMoeda50 = (decimal)Moeda50 * 0.50m;
					ObterSomatoriaReal();
				});
			}
		}

		public decimal ValorMoeda50
		{
			get
			{
				return GetProperty(() => ValorMoeda50);
			}
			set
			{
				SetProperty(() => ValorMoeda50, value);
			}
		}

		public int Moeda1
		{
			get
			{
				return GetProperty(() => Moeda1);
			}
			set
			{
				SetProperty(() => Moeda1, value, () =>
				{
					ValorMoeda1 = (decimal)Moeda1 * 1.00m;
					ObterSomatoriaReal();
				});
			}
		}

		public decimal ValorMoeda1
		{
			get
			{
				return GetProperty(() => ValorMoeda1);
			}
			set
			{
				SetProperty(() => ValorMoeda1, value);
			}
		}

		public int Cedula2
		{
			get
			{
				return GetProperty(() => Cedula2);
			}
			set
			{
				SetProperty(() => Cedula2, value, () =>
				{
					ValorCedula2 = (decimal)Cedula2 * 2.00m;
					ObterSomatoriaReal();
				});
			}
		}

		public decimal ValorCedula2
		{
			get
			{
				return GetProperty(() => ValorCedula2);
			}
			set
			{
				SetProperty(() => ValorCedula2, value);
			}
		}

		public int Cedula5
		{
			get
			{
				return GetProperty(() => Cedula5);
			}
			set
			{
				SetProperty(() => Cedula5, value, () =>
				{
					ValorCedula5 = (decimal)Cedula5 * 5.00m;
					ObterSomatoriaReal();
				});
			}
		}

		public decimal ValorCedula5
		{
			get
			{
				return GetProperty(() => ValorCedula5);
			}
			set
			{
				SetProperty(() => ValorCedula5, value);
			}
		}

		public int Cedula10
		{
			get
			{
				return GetProperty(() => Cedula10);
			}
			set
			{
				SetProperty(() => Cedula10, value, () =>
				{
					ValorCedula10 = (decimal)Cedula10 * 10.00m;
					ObterSomatoriaReal();
				});
			}
		}

		public decimal ValorCedula10
		{
			get
			{
				return GetProperty(() => ValorCedula10);
			}
			set
			{
				SetProperty(() => ValorCedula10, value);
			}
		}

		public int Cedula20
		{
			get
			{
				return GetProperty(() => Cedula20);
			}
			set
			{
				SetProperty(() => Cedula20, value, () =>
				{
					ValorCedula20 = (decimal)Cedula20 * 20.00m;
					ObterSomatoriaReal();
				});
			}
		}

		public decimal ValorCedula20
		{
			get
			{
				return GetProperty(() => ValorCedula20);
			}
			set
			{
				SetProperty(() => ValorCedula20, value);
			}
		}

		public int Cedula50
		{
			get
			{
				return GetProperty(() => Cedula50);
			}
			set
			{
				SetProperty(() => Cedula50, value, () =>
				{
					ValorCedula50 = (decimal)Cedula50 * 50.00m;
					ObterSomatoriaReal();
				});
			}
		}

		public decimal ValorCedula50
		{
			get
			{
				return GetProperty(() => ValorCedula50);
			}
			set
			{
				SetProperty(() => ValorCedula50, value);
			}
		}

		public int Cedula100
		{
			get
			{
				return GetProperty(() => Cedula100);
			}
			set
			{
				SetProperty(() => Cedula100, value, () =>
				{
					ValorCedula100 = (decimal)Cedula100 * 100.00m;
					ObterSomatoriaReal();
				});
			}
		}

		public decimal ValorCedula100
		{
			get
			{
				return GetProperty(() => ValorCedula100);
			}
			set
			{
				SetProperty(() => ValorCedula100, value);
			}
		}

		public int Cedula200
		{
			get
			{
				return GetProperty(() => Cedula200);
			}
			set
			{
				SetProperty(() => Cedula200, value, () =>
				{
					ValorCedula200 = (decimal)Cedula200 * 200.00m;
					ObterSomatoriaReal();
				});
			}
		}

		public decimal ValorCedula200
		{
			get
			{
				return GetProperty(() => ValorCedula200);
			}
			set
			{
				SetProperty(() => ValorCedula200, value);
			}
		}

		public event EventHandler<decimal> AtualizouTotalEspecieEvent;

		public void ObterSomatoriaReal()
		{
			TotalEspecie = ValorMoeda01 + ValorMoeda05 + ValorMoeda10 + ValorMoeda25 + ValorMoeda50 + ValorMoeda1 + ValorCedula2 + ValorCedula5 + ValorCedula10 + ValorCedula20 + ValorCedula50 + ValorCedula100 + ValorCedula200;
			OnAtualizouTotalEspecieEvent(TotalEspecie);
		}

		protected virtual void OnAtualizouTotalEspecieEvent(decimal e)
		{
			AtualizouTotalEspecieEvent?.Invoke(this, e);
		}
	}
	public class TurnoAberturaAbaViewModel : AbaPrincipalViewModelBase
	{
		public TurnoAberturaAbaViewModel(IViewModelPrincipal viewModel)
			: base("ABERTURA DE TURNO", "Dados para abertura de turnos", viewModel)
		{
		}
	}
	public class TurnoAberturaViewModel : TurnoViewModel
	{
		public TurnoAberturaViewModel(TurnoDto dto)
			: base(dto)
		{
		}
	}
	public class TurnoFechamentoAbaViewModel : AbaPrincipalViewModelBase
	{
		public TurnoFechamentoAbaViewModel(IViewModelPrincipal viewModel)
			: base("FECHAMENTO DE TURNO", "Dados para fechamento de turno", viewModel)
		{
		}
	}
	public class TurnoFechamentoViewModel : ViewModelDtoBase<TurnoDto>, IViewModelPrincipal, IViewModelTitulo, IViewModelDtoBase, IViewModelBase
	{
		public ContagemDeCedulasMoedas ContagemDeCedulasMoedas
		{
			get
			{
				return GetProperty(() => ContagemDeCedulasMoedas);
			}
			set
			{
				SetProperty(() => ContagemDeCedulasMoedas, value);
			}
		}

		public decimal TotalEspecie
		{
			get
			{
				return GetProperty(() => TotalEspecie);
			}
			set
			{
				SetProperty(() => TotalEspecie, value, CalcularTotal);
			}
		}

		public decimal TotalCheque
		{
			get
			{
				return GetProperty(() => TotalCheque);
			}
			set
			{
				SetProperty(() => TotalCheque, value, CalcularTotal);
			}
		}

		public decimal TotalCartao
		{
			get
			{
				return GetProperty(() => TotalCartao);
			}
			set
			{
				SetProperty(() => TotalCartao, value, CalcularTotal);
			}
		}

		public decimal TotalVale
		{
			get
			{
				return GetProperty(() => TotalVale);
			}
			set
			{
				SetProperty(() => TotalVale, value, CalcularTotal);
			}
		}

		public decimal TotalValePresente
		{
			get
			{
				return GetProperty(() => TotalValePresente);
			}
			set
			{
				SetProperty(() => TotalValePresente, value, CalcularTotal);
			}
		}

		public decimal TotalCreditoLoja
		{
			get
			{
				return GetProperty(() => TotalCreditoLoja);
			}
			set
			{
				SetProperty(() => TotalCreditoLoja, value, CalcularTotal);
			}
		}

		public decimal TotalCarteirasDigitais
		{
			get
			{
				return GetProperty(() => TotalCarteirasDigitais);
			}
			set
			{
				SetProperty(() => TotalCarteirasDigitais, value, CalcularTotal);
			}
		}

		public decimal TotalOutrasEspecies
		{
			get
			{
				return GetProperty(() => TotalOutrasEspecies);
			}
			set
			{
				SetProperty(() => TotalOutrasEspecies, value, CalcularTotal);
			}
		}

		public decimal TotalDepositoBancario
		{
			get
			{
				return GetProperty(() => TotalDepositoBancario);
			}
			set
			{
				SetProperty(() => TotalDepositoBancario, value, CalcularTotal);
			}
		}

		public decimal TotalTed
		{
			get
			{
				return GetProperty(() => TotalTed);
			}
			set
			{
				SetProperty(() => TotalTed, value, CalcularTotal);
			}
		}

		public decimal TotalDoc
		{
			get
			{
				return GetProperty(() => TotalDoc);
			}
			set
			{
				SetProperty(() => TotalDoc, value, CalcularTotal);
			}
		}

		public decimal TotalPrazo
		{
			get
			{
				return GetProperty(() => TotalPrazo);
			}
			set
			{
				SetProperty(() => TotalPrazo, value, CalcularTotal);
			}
		}

		public bool PermitirAlterarDataHoraAberturaFechamentoTurno => DadosSistema.VerificarPermissao<Turno, PermitirAlterarDataHoraAberturaFechamentoTurno>();

		public DateTime DataHoraFechamento
		{
			get
			{
				return GetProperty(() => DataHoraFechamento);
			}
			set
			{
				SetProperty(() => DataHoraFechamento, value, CalcularTotal);
			}
		}

		public string Observacao
		{
			get
			{
				return GetProperty(() => Observacao);
			}
			set
			{
				SetProperty(() => Observacao, value, CalcularTotal);
			}
		}

		public decimal TotalGeral
		{
			get
			{
				return GetProperty(() => TotalGeral);
			}
			set
			{
				SetProperty(() => TotalGeral, value);
			}
		}

		public TurnoFechamentoViewModel()
		{
		}

		public TurnoFechamentoViewModel(TurnoDto dto)
			: base(dto)
		{
		}

		public override void ProjectedAsViewModel()
		{
			DataHoraFechamento = (Dto.TurnoFechado ? Dto.Fechamento.DataHoraFechamento : DateTime.Now);
			ContagemDeCedulasMoedas = new ContagemDeCedulasMoedas
			{
				TotalEspecie = Dto.Fechamento.TotalEspecie
			};
			ContagemDeCedulasMoedas.AtualizouTotalEspecieEvent += ContagemDeCedulasMoedasOnAtualizouTotalEspecieEvent;
			TotalEspecie = Dto.Fechamento.TotalEspecie;
			TotalCheque = Dto.Fechamento.TotalCheque;
			TotalCartao = Dto.Fechamento.TotalCartao;
			TotalVale = Dto.Fechamento.TotalVale;
			TotalValePresente = Dto.Fechamento.TotalValePresente;
			TotalCreditoLoja = Dto.Fechamento.TotalCreditoLoja;
			TotalCarteirasDigitais = Dto.Fechamento.TotalCarteirasDigitais;
			TotalOutrasEspecies = Dto.Fechamento.TotalOutrasEspecies;
			TotalDepositoBancario = Dto.Fechamento.TotalDepositoBancario;
			TotalTed = Dto.Fechamento.TotalTed;
			TotalDoc = Dto.Fechamento.TotalDoc;
			TotalPrazo = Dto.Fechamento.TotalPrazo;
			Observacao = Dto.Fechamento.Observacao;
		}

		public override void AtualizarDto<T>(ref T dto)
		{
			base.AtualizarDto(ref dto);
			(dto as TurnoDto).Fechamento = new FechamentoDto
			{
				DataHoraFechamento = DataHoraFechamento,
				TotalEspecie = TotalEspecie,
				TotalCheque = TotalCheque,
				TotalCartao = TotalCartao,
				TotalVale = TotalVale,
				TotalValePresente = TotalValePresente,
				TotalCreditoLoja = TotalCreditoLoja,
				TotalCarteirasDigitais = TotalCarteirasDigitais,
				TotalOutrasEspecies = TotalOutrasEspecies,
				TotalDepositoBancario = TotalDepositoBancario,
				TotalTed = TotalTed,
				TotalDoc = TotalDoc,
				TotalPrazo = TotalPrazo,
				Observacao = Observacao
			};
		}

		private void ContagemDeCedulasMoedasOnAtualizouTotalEspecieEvent(object sender, decimal valor)
		{
			TotalEspecie = valor;
			CalcularTotal();
		}

		private void CalcularTotal()
		{
			TotalGeral = TotalEspecie + TotalCheque + TotalCartao + TotalVale + TotalValePresente + TotalCreditoLoja + TotalCarteirasDigitais + TotalOutrasEspecies + TotalDepositoBancario + TotalTed + TotalDoc + TotalPrazo;
		}

		public override void Dispose()
		{
			ContagemDeCedulasMoedas.AtualizouTotalEspecieEvent -= ContagemDeCedulasMoedasOnAtualizouTotalEspecieEvent;
			base.Dispose();
		}

		public TurnoDto ObterDto()
		{
			return Dto;
		}

		public string ToTituloLista()
		{
			return string.Empty;
		}

		public IFormulario CriarFormulario()
		{
			return new TurnoFechamentoFormulario(Dto);
		}

		public string ToTituloFicha()
		{
			return string.Empty;
		}
	}
	public class TurnoMovimentacaoAbaViewModel : AbaPrincipalViewModelBase
	{
		public TurnoMovimentacaoAbaViewModel(IViewModelPrincipal viewModel)
			: base("TURNO MOVIMENTAÇÕES", "Movimentações do turno", viewModel)
		{
		}
	}
	public class TurnoMovimentacaoFichaViewModel : FichaManutencaoDetalheBase<TurnoLancamentoDto>
	{
		public decimal Valor
		{
			get
			{
				return GetProperty(() => Valor);
			}
			set
			{
				SetProperty(() => Valor, value);
			}
		}

		public OrigemLancamentoTurnoDto OrigemLancamento
		{
			get
			{
				return GetProperty(() => OrigemLancamento);
			}
			set
			{
				SetProperty(() => OrigemLancamento, value);
			}
		}

		public Lista<OrigemLancamentoTurnoDto> OrigensLancamento
		{
			get
			{
				return GetProperty(() => OrigensLancamento);
			}
			set
			{
				SetProperty(() => OrigensLancamento, value);
			}
		}

		public string Observacao
		{
			get
			{
				return GetProperty(() => Observacao);
			}
			set
			{
				SetProperty(() => Observacao, value);
			}
		}

		public Lista<EspecieRecebimentoDto> Especie
		{
			get
			{
				return GetProperty(() => Especie);
			}
			set
			{
				SetProperty(() => Especie, value);
			}
		}

		public DateTime DataHora
		{
			get
			{
				return GetProperty(() => DataHora);
			}
			set
			{
				SetProperty(() => DataHora, value);
			}
		}

		public bool PodeAtualizar
		{
			get
			{
				if (OrigemLancamento.Codigo != 4)
				{
					return OrigemLancamento.Codigo == 5;
				}
				return true;
			}
		}

		public TurnoMovimentacaoFichaViewModel(TurnoLancamentoDto baseDto)
			: base(baseDto)
		{
		}

		public override void ProjectedAsViewModel()
		{
			Ativo = Dto.Ativo;
			DataHora = ((Dto.DataHora == DateTime.MinValue) ? DateTime.Now : Dto.DataHora);
			Observacao = Dto.Observacao;
			Valor = Dto.Valor;
			OrigemLancamento = Dto.OrigemLancamento;
			OrigensLancamento = new Lista<OrigemLancamentoTurnoDto>();
			OrigensLancamento.Adicionar(OrigensLancamentosTurno.ObterOrigemLancamentoTurnoParaLancamentoManual().ProjectedAsCollectionDto());
			OrigensLancamento.Posicionar((OrigemLancamentoTurnoDto x) => x.Codigo == Dto.OrigemLancamento.Codigo);
			OrigensLancamento.AlterouItemSelecionadoEvent -= OrigensLancamentoAlterouItemSelecionadoEvent;
			OrigensLancamento.AlterouItemSelecionadoEvent += OrigensLancamentoAlterouItemSelecionadoEvent;
			IEnumerable<EspecieRecebimentoDto> itens = EspecieRecebimentoValueObjectService.ObterEspeciesRecebimentoParaSangria().ProjectedAsCollectionDto();
			Especie = new Lista<EspecieRecebimentoDto>();
			Especie.Adicionar(itens);
			Especie.Posicionar((EspecieRecebimentoDto x) => x.Codigo == Dto.Especie.Codigo);
		}

		private void OrigensLancamentoAlterouItemSelecionadoEvent(object sender, OrigemLancamentoTurnoDto e)
		{
			if (!e.IsNull())
			{
				OrigemLancamento = e;
			}
		}

		public override void AtualizarDto<T>(ref T dto)
		{
			TurnoLancamentoDto dto2 = dto as TurnoLancamentoDto;
			if (!dto2.IsNull())
			{
				if (OrigemLancamento.Codigo == 0)
				{
					OrigemLancamento = OrigensLancamento.ItemSelecionado;
				}
				dto2.Ativo = Ativo;
				dto2.DataHora = DataHora;
				dto2.Especie = Especie.ItemSelecionado;
				dto2.Observacao = Observacao;
				dto2.OrigemLancamento = OrigemLancamento;
				dto2.Valor = Valor;
				base.AtualizarDto(ref dto2);
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			OrigensLancamento.AlterouItemSelecionadoEvent -= OrigensLancamentoAlterouItemSelecionadoEvent;
		}
	}
	public class TurnoMovimentacaoListaViewModel : ListaDetalheManutencaoBase<TurnoLancamentoDto, TurnoMovimentacaoFichaViewModel>
	{
		public TurnoMovimentacaoListaViewModel(IEnumerable<TurnoLancamentoDto> dtos)
			: base(dtos)
		{
		}
	}
	public class TurnoMovimentacaoViewModel : ViewModelDetalheBase<TurnoLancamentoDto, TurnoMovimentacaoListaViewModel, TurnoMovimentacaoFichaViewModel>, IViewModelPrincipal, IViewModelTitulo, IViewModelDtoBase, IViewModelBase
	{
		public override bool Valido => true;

		public TurnoMovimentacaoViewModel(IEnumerable<TurnoLancamentoDto> dto)
			: base(dto.ToList())
		{
		}

		public IEnumerable<TurnoLancamentoDto> ObterDtos()
		{
			return Dto;
		}

		public string ToTituloLista()
		{
			return string.Empty;
		}

		public string ToTituloFicha()
		{
			return string.Empty;
		}

		public IFormulario CriarFormulario()
		{
			return new TurnoMovimentacoesFormulario(Dto);
		}
	}
}
namespace Otus.Modulo.Turnos.ViewModels.Jornadas
{
	public class BicoJornadaViewModel : BindableBase
	{
		public BicoJornadaDto BicoJornadaDto { get; protected set; }

		public decimal EncerranteInicial
		{
			get
			{
				return GetProperty(() => EncerranteInicial);
			}
			set
			{
				SetProperty(() => EncerranteInicial, value, () =>
				{
					BicoJornadaDto.EncerranteInicial = value;
				});
			}
		}

		public decimal EncerranteFinal
		{
			get
			{
				return GetProperty(() => EncerranteFinal);
			}
			set
			{
				SetProperty(() => EncerranteFinal, value, () =>
				{
					BicoJornadaDto.EncerranteFinal = value;
				});
			}
		}

		public BicoJornadaViewModel(BicoJornadaDto bicoJornadaDto)
		{
			BicoJornadaDto = bicoJornadaDto;
			EncerranteInicial = bicoJornadaDto.EncerranteInicial;
			EncerranteFinal = bicoJornadaDto.EncerranteFinal;
		}
	}
	public class JornadaAberturaAbaViewModel : AbaPrincipalViewModelBase
	{
		public JornadaAberturaAbaViewModel(IViewModelPrincipal viewModel)
			: base("Abrir jornada", "Dados para abertura da jornada", viewModel)
		{
		}
	}
	public class JornadaAberturaFormulario : FormularioEntity<JornadaDto>
	{
		public JornadaAberturaFormulario(JornadaDto dto)
			: base(dto)
		{
			AdicionarProjetarAbas(new JornadaAberturaAbaViewModel(new JornadaAberturaViewModel(Dto)));
		}
	}
	public class JornadaAberturaViewModel : ViewModelDtoBase<JornadaDto>, IViewModelPrincipal, IViewModelTitulo, IViewModelDtoBase, IViewModelBase
	{
		public DateTime DataHoraAbertura
		{
			get
			{
				return GetProperty(() => DataHoraAbertura);
			}
			set
			{
				SetProperty(() => DataHoraAbertura, value, () =>
				{
					Dto.DataHoraAbertura = value;
				});
			}
		}

		public Visibility MostrarBicos { get; set; }

		public Lista<BicoJornadaViewModel> BicosJornada { get; set; }

		public JornadaAberturaViewModel()
		{
		}

		public JornadaAberturaViewModel(JornadaDto dto)
			: base(dto)
		{
		}

		public override void ProjectedAsViewModel()
		{
			DataHoraAbertura = Dto.DataHoraAbertura;
			MostrarBicos = ((!Dto.Bicos.Any()) ? Visibility.Collapsed : Visibility.Visible);
			if (Dto.Bicos.Any())
			{
				BicosJornada = new Lista<BicoJornadaViewModel>();
				BicosJornada.Adicionar(from x in Dto.Bicos
					orderby x.Numero
					select new BicoJornadaViewModel(x));
			}
		}

		public IFormulario CriarFormulario()
		{
			return new JornadaAberturaFormulario(Dto);
		}

		public string ToTituloLista()
		{
			return "Abertura de jornadas";
		}

		public string ToTituloFicha()
		{
			return "Abertura de jornada";
		}
	}
	public class JornadaFechamentoAbaViewModel : AbaPrincipalViewModelBase
	{
		public JornadaFechamentoAbaViewModel(IViewModelPrincipal viewModel)
			: base("Fechar jornada", "Dados para fechamento da jornada", viewModel)
		{
		}
	}
	public class JornadaFechamentoViewModel : ViewModelDtoBase<JornadaDto>, IViewModelPrincipal, IViewModelTitulo, IViewModelDtoBase, IViewModelBase
	{
		public DateTime DataHoraAbertura
		{
			get
			{
				return GetProperty(() => DataHoraAbertura);
			}
			set
			{
				SetProperty(() => DataHoraAbertura, value, () =>
				{
					Dto.DataHoraAbertura = value;
				});
			}
		}

		public DateTime DataHoraFechamento
		{
			get
			{
				return GetProperty(() => DataHoraFechamento);
			}
			set
			{
				SetProperty(() => DataHoraFechamento, value, () =>
				{
					Dto.DataHoraFechamento = value;
				});
			}
		}

		public Visibility MostrarBicos { get; set; }

		public Lista<BicoJornadaViewModel> BicosJornada { get; set; }

		public JornadaFechamentoViewModel()
		{
		}

		public JornadaFechamentoViewModel(JornadaDto dto)
			: base(dto)
		{
		}

		public override void ProjectedAsViewModel()
		{
			DataHoraAbertura = Dto.DataHoraAbertura;
			DataHoraFechamento = DateTime.Now;
			MostrarBicos = ((!Dto.Bicos.Any()) ? Visibility.Collapsed : Visibility.Visible);
			if (Dto.Bicos.Any())
			{
				BicosJornada = new Lista<BicoJornadaViewModel>();
				BicosJornada.Adicionar(from x in Dto.Bicos
					orderby x.Numero
					select new BicoJornadaViewModel(x));
			}
		}

		public IFormulario CriarFormulario()
		{
			return new JornadaFechamentoFormulario(Dto);
		}

		public string ToTituloLista()
		{
			return "Fechamento de jornadas";
		}

		public string ToTituloFicha()
		{
			return "Fechamento de jornada";
		}
	}
	public class JornadaFechamentoFormulario : FormularioEntity<JornadaDto>
	{
		public JornadaFechamentoFormulario(JornadaDto dto)
			: base(dto)
		{
			AdicionarProjetarAbas(new JornadaFechamentoAbaViewModel(new JornadaFechamentoViewModel(Dto)));
		}
	}
}
namespace Otus.Modulo.Turnos.Reports
{
	public class ResumoConferenciaTurno : DigisatServer.Infrastructure.Crosscutting.Report.Models.Relatorio
	{
		private readonly ResumoConferenciaTurnoDto _resumoConferenciaTurno;

		private IContainer components;

		private DetailBand Detail;

		private XRControlStyle Title;

		private XRControlStyle FieldCaption;

		private XRControlStyle PageInfo;

		private XRControlStyle DataField;

		private TopMarginBand TopMargin;

		private BottomMarginBand bottomMarginBand1;

		private BindingSource bindingSource1;

		private XRLabel xrLabel3;

		private XRLabel xrLabel2;

		private XRLabel xrLabel4;

		private XRLabel xrLabel5;

		private XRLabel lblFechamentoTurno;

		private XRLabel xrlFechamentoTurno;

		private PageHeaderBand PageHeader;

		private XRLabel xrLabel7;

		private XRLabel xrLabel6;

		private DetailReportBand DetailReport;

		private DetailBand Detail1;

		private XRLabel xrLabel8;

		private XRLabel xrLabel9;

		private FormattingRule Negrito;

		private XRControlStyle xrControlStyle1;

		private XRLabel Empresa;

		private XRLabel Titulo;

		private XRLabel CNPJ;

		private PageFooterBand PageFooter;

		private XRLabel DataEmissao;

		private XRLabel xrLabel1;

		private XRPageInfo xrPageInfo1;

		private XRLabel xrLabel10;

		private XRLabel xrLabel12;

		private GroupFooterBand GroupFooter1;

		private XRLabel xrLabel26;

		private XRLabel xrLabel25;

		private XRLabel xrLabel24;

		private XRLabel xrLabel23;

		private XRLabel xrLabel22;

		private XRLabel xrLabel21;

		private XRLabel xrLabel28;

		private XRLabel xrLabel27;

		private XRLabel xrLabel29;

		private XRLabel xrLabel30;

		private XRLabel xrLabel13;

		private XRLabel xrLabel11;

		public override string Descricao => "Resumo da conferência de turno";

		public override bool ExibirNoModuloRelatorios => false;

		public ResumoConferenciaTurno(ResumoConferenciaTurnoDto resumoConferenciaTurno)
		{
			_resumoConferenciaTurno = resumoConferenciaTurno;
			InitializeComponent();
		}

		public override XtraReport Processar()
		{
			Empresa.Text = DadosSistema.EmpresaLogada.Nome;
			CNPJ.Text = DadosSistema.EmpresaLogada.CpfCnpjFormatado;
			DataEmissao.Text = $"Gerado em {DateTime.Now:d' de 'MMMM' de 'yyyy}";
			DataSource = new List<ResumoConferenciaTurnoDto> { _resumoConferenciaTurno };
			ShowPrintMarginsWarning = false;
			CreateDocument();
			return this;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new Container();
			Detail = new DetailBand();
			xrLabel13 = new XRLabel();
			xrLabel11 = new XRLabel();
			xrLabel26 = new XRLabel();
			xrLabel25 = new XRLabel();
			xrLabel24 = new XRLabel();
			xrLabel23 = new XRLabel();
			xrLabel22 = new XRLabel();
			xrLabel21 = new XRLabel();
			xrLabel7 = new XRLabel();
			xrLabel6 = new XRLabel();
			xrLabel3 = new XRLabel();
			xrLabel2 = new XRLabel();
			xrLabel4 = new XRLabel();
			xrLabel5 = new XRLabel();
			lblFechamentoTurno = new XRLabel();
			xrlFechamentoTurno = new XRLabel();
			Title = new XRControlStyle();
			FieldCaption = new XRControlStyle();
			PageInfo = new XRControlStyle();
			DataField = new XRControlStyle();
			TopMargin = new TopMarginBand();
			bottomMarginBand1 = new BottomMarginBand();
			bindingSource1 = new BindingSource(components);
			PageHeader = new PageHeaderBand();
			Empresa = new XRLabel();
			Titulo = new XRLabel();
			CNPJ = new XRLabel();
			DetailReport = new DetailReportBand();
			Detail1 = new DetailBand();
			xrLabel28 = new XRLabel();
			xrLabel27 = new XRLabel();
			xrLabel8 = new XRLabel();
			xrLabel9 = new XRLabel();
			GroupFooter1 = new GroupFooterBand();
			xrLabel30 = new XRLabel();
			xrLabel29 = new XRLabel();
			xrLabel10 = new XRLabel();
			xrLabel12 = new XRLabel();
			Negrito = new FormattingRule();
			xrControlStyle1 = new XRControlStyle();
			PageFooter = new PageFooterBand();
			DataEmissao = new XRLabel();
			xrLabel1 = new XRLabel();
			xrPageInfo1 = new XRPageInfo();
			((ISupportInitialize)bindingSource1).BeginInit();
			((ISupportInitialize)this).BeginInit();
			Detail.Controls.AddRange(new XRControl[16]
			{
				xrLabel13, xrLabel11, xrLabel26, xrLabel25, xrLabel24, xrLabel23, xrLabel22, xrLabel21, xrLabel7, xrLabel6,
				xrLabel3, xrLabel2, xrLabel4, xrLabel5, lblFechamentoTurno, xrlFechamentoTurno
			});
			Detail.Dpi = 254f;
			Detail.HeightF = 451.2486f;
			Detail.Name = "Detail";
			Detail.Padding = new PaddingInfo(0, 0, 0, 0, 254f);
			Detail.StyleName = "DataField";
			Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			xrLabel13.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "Observacao")
			});
			xrLabel13.Dpi = 254f;
			xrLabel13.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, GraphicsUnit.Point, 0);
			xrLabel13.LocationFloat = new PointFloat(245.4792f, 292.1f);
			xrLabel13.Multiline = true;
			xrLabel13.Name = "xrLabel13";
			xrLabel13.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel13.SizeF = new SizeF(1636.894f, 58.42001f);
			xrLabel13.StylePriority.UseFont = false;
			xrLabel11.Dpi = 254f;
			xrLabel11.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel11.LocationFloat = new PointFloat(0f, 292.1f);
			xrLabel11.Name = "xrLabel11";
			xrLabel11.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel11.SizeF = new SizeF(245.4792f, 58.42001f);
			xrLabel11.StylePriority.UseFont = false;
			xrLabel11.Text = "Observação:";
			xrLabel26.Borders = BorderSide.Bottom;
			xrLabel26.Dpi = 254f;
			xrLabel26.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel26.LocationFloat = new PointFloat(1552.968f, 401.2485f);
			xrLabel26.Name = "xrLabel26";
			xrLabel26.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel26.SizeF = new SizeF(329.4054f, 49.99994f);
			xrLabel26.StylePriority.UseBorders = false;
			xrLabel26.StylePriority.UseFont = false;
			xrLabel26.StylePriority.UseTextAlignment = false;
			xrLabel26.Text = "Diferença";
			xrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
			xrLabel25.Borders = BorderSide.Bottom;
			xrLabel25.Dpi = 254f;
			xrLabel25.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel25.LocationFloat = new PointFloat(1223.562f, 401.2485f);
			xrLabel25.Name = "xrLabel25";
			xrLabel25.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel25.SizeF = new SizeF(329.4054f, 49.99994f);
			xrLabel25.StylePriority.UseBorders = false;
			xrLabel25.StylePriority.UseFont = false;
			xrLabel25.StylePriority.UseTextAlignment = false;
			xrLabel25.Text = "Conferido";
			xrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
			xrLabel24.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "UsuarioConferiu")
			});
			xrLabel24.Dpi = 254f;
			xrLabel24.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, GraphicsUnit.Point, 0);
			xrLabel24.LocationFloat = new PointFloat(398.9375f, 233.68f);
			xrLabel24.Name = "xrLabel24";
			xrLabel24.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel24.SizeF = new SizeF(624.4166f, 58.42001f);
			xrLabel24.StylePriority.UseFont = false;
			xrLabel23.Dpi = 254f;
			xrLabel23.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel23.LocationFloat = new PointFloat(0f, 233.68f);
			xrLabel23.Name = "xrLabel23";
			xrLabel23.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel23.SizeF = new SizeF(398.9375f, 58.42001f);
			xrLabel23.StylePriority.UseFont = false;
			xrLabel23.Text = "Usuário que conferiu:";
			xrLabel22.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "DataHoraConferencia", "{0:dd/MM/yyyy HH:mm}")
			});
			xrLabel22.Dpi = 254f;
			xrLabel22.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, GraphicsUnit.Point, 0);
			xrLabel22.LocationFloat = new PointFloat(398.9375f, 175.26f);
			xrLabel22.Name = "xrLabel22";
			xrLabel22.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel22.SizeF = new SizeF(620.3925f, 58.42001f);
			xrLabel22.StylePriority.UseFont = false;
			xrLabel21.Dpi = 254f;
			xrLabel21.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel21.LocationFloat = new PointFloat(0f, 175.26f);
			xrLabel21.Name = "xrLabel21";
			xrLabel21.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel21.SizeF = new SizeF(398.9375f, 58.42001f);
			xrLabel21.StylePriority.UseFont = false;
			xrLabel21.Text = "Conferência do turno:";
			xrLabel7.Borders = BorderSide.Bottom;
			xrLabel7.Dpi = 254f;
			xrLabel7.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel7.LocationFloat = new PointFloat(894.1567f, 401.2487f);
			xrLabel7.Name = "xrLabel7";
			xrLabel7.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel7.SizeF = new SizeF(329.4054f, 49.99994f);
			xrLabel7.StylePriority.UseBorders = false;
			xrLabel7.StylePriority.UseFont = false;
			xrLabel7.StylePriority.UseTextAlignment = false;
			xrLabel7.Text = "Entregue";
			xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
			xrLabel6.Borders = BorderSide.Bottom;
			xrLabel6.Dpi = 254f;
			xrLabel6.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel6.LocationFloat = new PointFloat(25f, 401.2485f);
			xrLabel6.Name = "xrLabel6";
			xrLabel6.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel6.SizeF = new SizeF(869.1567f, 49.99994f);
			xrLabel6.StylePriority.UseBorders = false;
			xrLabel6.StylePriority.UseFont = false;
			xrLabel6.StylePriority.UseTextAlignment = false;
			xrLabel6.Text = "Espécie";
			xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
			xrLabel3.Dpi = 254f;
			xrLabel3.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel3.LocationFloat = new PointFloat(0f, 0f);
			xrLabel3.Name = "xrLabel3";
			xrLabel3.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel3.SizeF = new SizeF(164.0416f, 58.42001f);
			xrLabel3.StylePriority.UseFont = false;
			xrLabel3.Text = "Usuário:";
			xrLabel2.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "Usuario")
			});
			xrLabel2.Dpi = 254f;
			xrLabel2.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, GraphicsUnit.Point, 0);
			xrLabel2.LocationFloat = new PointFloat(164.0416f, 0f);
			xrLabel2.Name = "xrLabel2";
			xrLabel2.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel2.SizeF = new SizeF(624.4166f, 58.42001f);
			xrLabel2.StylePriority.UseFont = false;
			xrLabel4.Dpi = 254f;
			xrLabel4.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel4.LocationFloat = new PointFloat(0f, 58.42001f);
			xrLabel4.Name = "xrLabel4";
			xrLabel4.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel4.SizeF = new SizeF(340.7292f, 58.42001f);
			xrLabel4.StylePriority.UseFont = false;
			xrLabel4.Text = "Abertura do turno:";
			xrLabel5.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "DataHoraAbertura", "{0:dd/MM/yyyy HH:mm}")
			});
			xrLabel5.Dpi = 254f;
			xrLabel5.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, GraphicsUnit.Point, 0);
			xrLabel5.LocationFloat = new PointFloat(340.7293f, 58.42001f);
			xrLabel5.Name = "xrLabel5";
			xrLabel5.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel5.SizeF = new SizeF(624.4166f, 58.42001f);
			xrLabel5.StylePriority.UseFont = false;
			lblFechamentoTurno.Dpi = 254f;
			lblFechamentoTurno.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			lblFechamentoTurno.LocationFloat = new PointFloat(0f, 116.84f);
			lblFechamentoTurno.Name = "lblFechamentoTurno";
			lblFechamentoTurno.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			lblFechamentoTurno.SizeF = new SizeF(398.9375f, 58.42001f);
			lblFechamentoTurno.StylePriority.UseFont = false;
			lblFechamentoTurno.Text = "Fechamento do turno:";
			xrlFechamentoTurno.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "DataHoraFechamento", "{0:dd/MM/yyyy HH:mm}")
			});
			xrlFechamentoTurno.Dpi = 254f;
			xrlFechamentoTurno.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, GraphicsUnit.Point, 0);
			xrlFechamentoTurno.LocationFloat = new PointFloat(398.9375f, 116.84f);
			xrlFechamentoTurno.Name = "xrlFechamentoTurno";
			xrlFechamentoTurno.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrlFechamentoTurno.SizeF = new SizeF(620.3925f, 58.42001f);
			xrlFechamentoTurno.StylePriority.UseFont = false;
			Title.BackColor = System.Drawing.Color.Transparent;
			Title.BorderColor = System.Drawing.Color.Black;
			Title.Borders = BorderSide.None;
			Title.BorderWidth = 1f;
			Title.Font = new Font("Times New Roman", 20f, System.Drawing.FontStyle.Bold);
			Title.ForeColor = System.Drawing.Color.Maroon;
			Title.Name = "Title";
			FieldCaption.BackColor = System.Drawing.Color.Transparent;
			FieldCaption.BorderColor = System.Drawing.Color.Black;
			FieldCaption.Borders = BorderSide.None;
			FieldCaption.BorderWidth = 1f;
			FieldCaption.Font = new Font("Arial", 10f, System.Drawing.FontStyle.Bold);
			FieldCaption.ForeColor = System.Drawing.Color.Maroon;
			FieldCaption.Name = "FieldCaption";
			PageInfo.BackColor = System.Drawing.Color.Transparent;
			PageInfo.BorderColor = System.Drawing.Color.Black;
			PageInfo.Borders = BorderSide.None;
			PageInfo.BorderWidth = 1f;
			PageInfo.Font = new Font("Times New Roman", 10f, System.Drawing.FontStyle.Bold);
			PageInfo.ForeColor = System.Drawing.Color.Black;
			PageInfo.Name = "PageInfo";
			DataField.BackColor = System.Drawing.Color.Transparent;
			DataField.BorderColor = System.Drawing.Color.Black;
			DataField.Borders = BorderSide.None;
			DataField.BorderWidth = 1f;
			DataField.Font = new Font("Times New Roman", 10f);
			DataField.ForeColor = System.Drawing.Color.Black;
			DataField.Name = "DataField";
			DataField.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			TopMargin.Dpi = 254f;
			TopMargin.HeightF = 50f;
			TopMargin.Name = "TopMargin";
			bottomMarginBand1.Dpi = 254f;
			bottomMarginBand1.HeightF = 120f;
			bottomMarginBand1.Name = "bottomMarginBand1";
			bindingSource1.DataSource = typeof(ResumoConferenciaTurnoDto);
			PageHeader.Controls.AddRange(new XRControl[3] { Empresa, Titulo, CNPJ });
			PageHeader.Dpi = 254f;
			PageHeader.HeightF = 300f;
			PageHeader.Name = "PageHeader";
			Empresa.Dpi = 254f;
			Empresa.Font = new Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			Empresa.LocationFloat = new PointFloat(0f, 0f);
			Empresa.Name = "Empresa";
			Empresa.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			Empresa.SizeF = new SizeF(1900f, 58.42f);
			Empresa.StylePriority.UseFont = false;
			Empresa.Text = "Empresa";
			Titulo.Dpi = 254f;
			Titulo.Font = new Font("Segoe UI", 19f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			Titulo.LocationFloat = new PointFloat(0f, 116.84f);
			Titulo.Name = "Titulo";
			Titulo.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			Titulo.SizeF = new SizeF(1900f, 99.99998f);
			Titulo.StylePriority.UseFont = false;
			Titulo.StylePriority.UseTextAlignment = false;
			Titulo.Text = "Resumo da conferência de Turno";
			Titulo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
			CNPJ.Dpi = 254f;
			CNPJ.Font = new Font("Segoe UI", 10f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			CNPJ.LocationFloat = new PointFloat(0f, 58.42001f);
			CNPJ.Name = "CNPJ";
			CNPJ.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			CNPJ.SizeF = new SizeF(1900f, 58.41998f);
			CNPJ.StylePriority.UseFont = false;
			CNPJ.Text = "CNPJ";
			DetailReport.Bands.AddRange(new Band[2] { Detail1, GroupFooter1 });
			DetailReport.DataMember = "ResumoTurnoLancamentos";
			DetailReport.DataSource = bindingSource1;
			DetailReport.Dpi = 254f;
			DetailReport.Level = 0;
			DetailReport.Name = "DetailReport";
			Detail1.Controls.AddRange(new XRControl[4] { xrLabel28, xrLabel27, xrLabel8, xrLabel9 });
			Detail1.Dpi = 254f;
			Detail1.HeightF = 58.42f;
			Detail1.Name = "Detail1";
			xrLabel28.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "ResumoTurnoLancamentos.Diferenca", "{0:n2}")
			});
			xrLabel28.Dpi = 254f;
			xrLabel28.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel28.LocationFloat = new PointFloat(1552.968f, 0f);
			xrLabel28.Name = "xrLabel28";
			xrLabel28.OddStyleName = "xrControlStyle1";
			xrLabel28.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel28.SizeF = new SizeF(329.4054f, 58.42f);
			xrLabel28.StylePriority.UseFont = false;
			xrLabel28.StylePriority.UseTextAlignment = false;
			xrLabel28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
			xrLabel27.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "ResumoTurnoLancamentos.ValorEntregue", "{0:n2}")
			});
			xrLabel27.Dpi = 254f;
			xrLabel27.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel27.LocationFloat = new PointFloat(894.1567f, 0f);
			xrLabel27.Name = "xrLabel27";
			xrLabel27.OddStyleName = "xrControlStyle1";
			xrLabel27.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel27.SizeF = new SizeF(329.4054f, 58.42f);
			xrLabel27.StylePriority.UseFont = false;
			xrLabel27.StylePriority.UseTextAlignment = false;
			xrLabel27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
			xrLabel8.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "ResumoTurnoLancamentos.Especie")
			});
			xrLabel8.Dpi = 254f;
			xrLabel8.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel8.LocationFloat = new PointFloat(25f, 0f);
			xrLabel8.Name = "xrLabel8";
			xrLabel8.OddStyleName = "xrControlStyle1";
			xrLabel8.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel8.SizeF = new SizeF(869.1567f, 58.42f);
			xrLabel8.StylePriority.UseFont = false;
			xrLabel8.StylePriority.UseTextAlignment = false;
			xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
			xrLabel9.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "ResumoTurnoLancamentos.ValorConferido", "{0:n2}")
			});
			xrLabel9.Dpi = 254f;
			xrLabel9.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel9.LocationFloat = new PointFloat(1223.562f, 0f);
			xrLabel9.Name = "xrLabel9";
			xrLabel9.OddStyleName = "xrControlStyle1";
			xrLabel9.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel9.SizeF = new SizeF(329.4054f, 58.42f);
			xrLabel9.StylePriority.UseFont = false;
			xrLabel9.StylePriority.UseTextAlignment = false;
			xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
			GroupFooter1.Controls.AddRange(new XRControl[4] { xrLabel30, xrLabel29, xrLabel10, xrLabel12 });
			GroupFooter1.Dpi = 254f;
			GroupFooter1.HeightF = 58.42f;
			GroupFooter1.Name = "GroupFooter1";
			xrLabel30.Borders = BorderSide.Top;
			xrLabel30.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "Diferenca", "{0:n2}")
			});
			xrLabel30.Dpi = 254f;
			xrLabel30.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold);
			xrLabel30.LocationFloat = new PointFloat(1552.968f, 0f);
			xrLabel30.Name = "xrLabel30";
			xrLabel30.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel30.SizeF = new SizeF(329.4054f, 58.42f);
			xrLabel30.StylePriority.UseBorders = false;
			xrLabel30.StylePriority.UseFont = false;
			xrLabel30.StylePriority.UseTextAlignment = false;
			xrLabel30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
			xrLabel29.Borders = BorderSide.Top;
			xrLabel29.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "TotalFechamento", "{0:n2}")
			});
			xrLabel29.Dpi = 254f;
			xrLabel29.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold);
			xrLabel29.LocationFloat = new PointFloat(894.1567f, 0f);
			xrLabel29.Name = "xrLabel29";
			xrLabel29.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel29.SizeF = new SizeF(329.4054f, 58.42f);
			xrLabel29.StylePriority.UseBorders = false;
			xrLabel29.StylePriority.UseFont = false;
			xrLabel29.StylePriority.UseTextAlignment = false;
			xrLabel29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
			xrLabel10.Borders = BorderSide.Top;
			xrLabel10.Dpi = 254f;
			xrLabel10.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, GraphicsUnit.Point, 0);
			xrLabel10.LocationFloat = new PointFloat(25f, 0f);
			xrLabel10.Name = "xrLabel10";
			xrLabel10.OddStyleName = "xrControlStyle1";
			xrLabel10.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel10.SizeF = new SizeF(869.1567f, 58.42f);
			xrLabel10.StylePriority.UseBorders = false;
			xrLabel10.StylePriority.UseFont = false;
			xrLabel10.StylePriority.UseTextAlignment = false;
			xrLabel10.Text = "Total";
			xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
			xrLabel12.Borders = BorderSide.Top;
			xrLabel12.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "TotalConferencia", "{0:n2}")
			});
			xrLabel12.Dpi = 254f;
			xrLabel12.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold);
			xrLabel12.LocationFloat = new PointFloat(1223.562f, 0f);
			xrLabel12.Name = "xrLabel12";
			xrLabel12.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel12.SizeF = new SizeF(329.4054f, 58.42f);
			xrLabel12.StylePriority.UseBorders = false;
			xrLabel12.StylePriority.UseFont = false;
			xrLabel12.StylePriority.UseTextAlignment = false;
			xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
			Negrito.Name = "Negrito";
			xrControlStyle1.BackColor = System.Drawing.Color.Gainsboro;
			xrControlStyle1.Name = "xrControlStyle1";
			xrControlStyle1.Padding = new PaddingInfo(0, 0, 0, 0, 254f);
			PageFooter.Controls.AddRange(new XRControl[3] { DataEmissao, xrLabel1, xrPageInfo1 });
			PageFooter.Dpi = 254f;
			PageFooter.HeightF = 185.42f;
			PageFooter.Name = "PageFooter";
			DataEmissao.Dpi = 254f;
			DataEmissao.Font = new Font("Segoe UI", 10f);
			DataEmissao.LocationFloat = new PointFloat(0f, 68.58002f);
			DataEmissao.Name = "DataEmissao";
			DataEmissao.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			DataEmissao.SizeF = new SizeF(1900f, 58.42f);
			DataEmissao.StylePriority.UseFont = false;
			DataEmissao.StylePriority.UseTextAlignment = false;
			DataEmissao.Text = "DataEmissao";
			DataEmissao.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			xrLabel1.Dpi = 254f;
			xrLabel1.Font = new Font("Segoe UI", 10f);
			xrLabel1.LocationFloat = new PointFloat(380.0004f, 127f);
			xrLabel1.Name = "xrLabel1";
			xrLabel1.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel1.SizeF = new SizeF(1140f, 58.42f);
			xrLabel1.StylePriority.UseFont = false;
			xrLabel1.StylePriority.UseTextAlignment = false;
			xrLabel1.Text = "Desenvolvido por DigiSat Tecnologia LTDA®  www.digisat.com.br";
			xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			xrPageInfo1.Dpi = 254f;
			xrPageInfo1.Font = new Font("Segoe UI", 10f);
			xrPageInfo1.Format = " Página {0} de {1}";
			xrPageInfo1.LocationFloat = new PointFloat(1520f, 127f);
			xrPageInfo1.Name = "xrPageInfo1";
			xrPageInfo1.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrPageInfo1.SizeF = new SizeF(380f, 58.42f);
			xrPageInfo1.StylePriority.UseFont = false;
			xrPageInfo1.StylePriority.UseTextAlignment = false;
			xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			Bands.AddRange(new Band[6] { Detail, TopMargin, bottomMarginBand1, PageHeader, DetailReport, PageFooter });
			DataSource = bindingSource1;
			Dpi = 254f;
			FormattingRuleSheet.AddRange(new FormattingRule[1] { Negrito });
			Margins = new Margins(100, 100, 50, 120);
			PageHeight = 2970;
			PageWidth = 2100;
			PaperKind = PaperKind.A4;
			ReportUnit = ReportUnit.TenthsOfAMillimeter;
			StyleSheet.AddRange(new XRControlStyle[5] { Title, FieldCaption, PageInfo, DataField, xrControlStyle1 });
			Version = "15.2";
			((ISupportInitialize)bindingSource1).EndInit();
			((ISupportInitialize)this).EndInit();
		}
	}
	public class ResumoTurno : DigisatServer.Infrastructure.Crosscutting.Report.Models.Relatorio
	{
		private readonly ResumoTurnoDto _resumoTurnoDto;

		private IContainer components;

		private DetailBand Detail;

		private XRControlStyle Title;

		private XRControlStyle FieldCaption;

		private XRControlStyle PageInfo;

		private XRControlStyle DataField;

		private TopMarginBand TopMargin;

		private BottomMarginBand bottomMarginBand1;

		private BindingSource bindingSource1;

		private XRLabel xrLabel3;

		private XRLabel xrLabel2;

		private XRLabel xrLabel4;

		private XRLabel xrLabel5;

		private XRLabel lblFechamentoTurno;

		private XRLabel xrlFechamentoTurno;

		private PageHeaderBand PageHeader;

		private XRLabel xrLabel7;

		private XRLabel xrLabel6;

		private DetailReportBand DetailReport;

		private DetailBand Detail1;

		private XRLabel xrLabel8;

		private XRLabel xrLabel9;

		private FormattingRule Negrito;

		private XRControlStyle xrControlStyle1;

		private XRLabel Empresa;

		private XRLabel Titulo;

		private XRLabel CNPJ;

		private PageFooterBand PageFooter;

		private XRLabel DataEmissao;

		private XRLabel xrLabel1;

		private XRPageInfo xrPageInfo1;

		private XRLabel xrLabel10;

		private XRLabel xrLabel12;

		private GroupFooterBand GroupFooter1;

		private DetailReportBand DetailReport1;

		private DetailBand Detail2;

		private XRLabel xrLabel20;

		private XRLabel xrLabel19;

		private XRLabel xrLabel18;

		private XRLabel xrLabel17;

		private GroupHeaderBand GroupHeader1;

		private XRLabel xrLabel16;

		private XRLabel xrLabel15;

		private XRLabel xrLabel14;

		private XRLabel xrLabel13;

		private XRLabel xrLabel11;

		private XRLabel xrLabel21;

		private XRLabel xrLabel22;

		public override string Descricao => "Resumo do turno";

		public override bool ExibirNoModuloRelatorios => false;

		public ResumoTurno(ResumoTurnoDto resumoTurnoDto)
		{
			_resumoTurnoDto = resumoTurnoDto;
			InitializeComponent();
		}

		public override XtraReport Processar()
		{
			Empresa.Text = DadosSistema.EmpresaLogada.Nome;
			CNPJ.Text = DadosSistema.EmpresaLogada.CpfCnpjFormatado;
			DataEmissao.Text = $"Gerado em {DateTime.Now:d' de 'MMMM' de 'yyyy}";
			DataSource = new List<ResumoTurnoDto> { _resumoTurnoDto };
			ShowPrintMarginsWarning = false;
			CreateDocument();
			return this;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new Container();
			Detail = new DetailBand();
			xrLabel21 = new XRLabel();
			xrLabel22 = new XRLabel();
			xrLabel7 = new XRLabel();
			xrLabel6 = new XRLabel();
			xrLabel3 = new XRLabel();
			xrLabel2 = new XRLabel();
			xrLabel4 = new XRLabel();
			xrLabel5 = new XRLabel();
			lblFechamentoTurno = new XRLabel();
			xrlFechamentoTurno = new XRLabel();
			Title = new XRControlStyle();
			FieldCaption = new XRControlStyle();
			PageInfo = new XRControlStyle();
			DataField = new XRControlStyle();
			TopMargin = new TopMarginBand();
			bottomMarginBand1 = new BottomMarginBand();
			bindingSource1 = new BindingSource(components);
			PageHeader = new PageHeaderBand();
			Empresa = new XRLabel();
			Titulo = new XRLabel();
			CNPJ = new XRLabel();
			DetailReport = new DetailReportBand();
			Detail1 = new DetailBand();
			xrLabel8 = new XRLabel();
			xrLabel9 = new XRLabel();
			GroupFooter1 = new GroupFooterBand();
			xrLabel10 = new XRLabel();
			xrLabel12 = new XRLabel();
			Negrito = new FormattingRule();
			xrControlStyle1 = new XRControlStyle();
			PageFooter = new PageFooterBand();
			DataEmissao = new XRLabel();
			xrLabel1 = new XRLabel();
			xrPageInfo1 = new XRPageInfo();
			DetailReport1 = new DetailReportBand();
			Detail2 = new DetailBand();
			xrLabel20 = new XRLabel();
			xrLabel19 = new XRLabel();
			xrLabel18 = new XRLabel();
			xrLabel17 = new XRLabel();
			GroupHeader1 = new GroupHeaderBand();
			xrLabel16 = new XRLabel();
			xrLabel15 = new XRLabel();
			xrLabel14 = new XRLabel();
			xrLabel13 = new XRLabel();
			xrLabel11 = new XRLabel();
			((ISupportInitialize)bindingSource1).BeginInit();
			((ISupportInitialize)this).BeginInit();
			Detail.Controls.AddRange(new XRControl[10] { xrLabel21, xrLabel22, xrLabel7, xrLabel6, xrLabel3, xrLabel2, xrLabel4, xrLabel5, lblFechamentoTurno, xrlFechamentoTurno });
			Detail.Dpi = 254f;
			Detail.HeightF = 337.2068f;
			Detail.Name = "Detail";
			Detail.Padding = new PaddingInfo(0, 0, 0, 0, 254f);
			Detail.StyleName = "DataField";
			Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			xrLabel21.Dpi = 254f;
			xrLabel21.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel21.LocationFloat = new PointFloat(1.425906f, 175.26f);
			xrLabel21.Name = "xrLabel21";
			xrLabel21.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel21.SizeF = new SizeF(245.4792f, 58.42001f);
			xrLabel21.StylePriority.UseFont = false;
			xrLabel21.Text = "Observação:";
			xrLabel22.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "Observacao")
			});
			xrLabel22.Dpi = 254f;
			xrLabel22.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, GraphicsUnit.Point, 0);
			xrLabel22.LocationFloat = new PointFloat(246.9051f, 175.26f);
			xrLabel22.Multiline = true;
			xrLabel22.Name = "xrLabel22";
			xrLabel22.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel22.SizeF = new SizeF(1636.894f, 58.42001f);
			xrLabel22.StylePriority.UseFont = false;
			xrLabel7.Borders = BorderSide.Bottom;
			xrLabel7.Dpi = 254f;
			xrLabel7.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel7.LocationFloat = new PointFloat(1328.948f, 287.2069f);
			xrLabel7.Name = "xrLabel7";
			xrLabel7.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel7.SizeF = new SizeF(329.4054f, 49.99994f);
			xrLabel7.StylePriority.UseBorders = false;
			xrLabel7.StylePriority.UseFont = false;
			xrLabel7.StylePriority.UseTextAlignment = false;
			xrLabel7.Text = "Valor";
			xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
			xrLabel6.Borders = BorderSide.Bottom;
			xrLabel6.Dpi = 254f;
			xrLabel6.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel6.LocationFloat = new PointFloat(131.7084f, 287.2069f);
			xrLabel6.Name = "xrLabel6";
			xrLabel6.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel6.SizeF = new SizeF(1197.24f, 49.99994f);
			xrLabel6.StylePriority.UseBorders = false;
			xrLabel6.StylePriority.UseFont = false;
			xrLabel6.StylePriority.UseTextAlignment = false;
			xrLabel6.Text = "Espécie";
			xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
			xrLabel3.Dpi = 254f;
			xrLabel3.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel3.LocationFloat = new PointFloat(0f, 0f);
			xrLabel3.Name = "xrLabel3";
			xrLabel3.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel3.SizeF = new SizeF(164.0416f, 58.42001f);
			xrLabel3.StylePriority.UseFont = false;
			xrLabel3.Text = "Usuário:";
			xrLabel2.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "Usuario")
			});
			xrLabel2.Dpi = 254f;
			xrLabel2.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, GraphicsUnit.Point, 0);
			xrLabel2.LocationFloat = new PointFloat(164.0416f, 0f);
			xrLabel2.Name = "xrLabel2";
			xrLabel2.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel2.SizeF = new SizeF(624.4166f, 58.42001f);
			xrLabel2.StylePriority.UseFont = false;
			xrLabel4.Dpi = 254f;
			xrLabel4.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel4.LocationFloat = new PointFloat(0f, 58.42001f);
			xrLabel4.Name = "xrLabel4";
			xrLabel4.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel4.SizeF = new SizeF(340.7292f, 58.42001f);
			xrLabel4.StylePriority.UseFont = false;
			xrLabel4.Text = "Abertura do turno:";
			xrLabel5.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "DataHoraAbertura", "{0:dd/MM/yyyy HH:mm}")
			});
			xrLabel5.Dpi = 254f;
			xrLabel5.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, GraphicsUnit.Point, 0);
			xrLabel5.LocationFloat = new PointFloat(340.7293f, 58.42001f);
			xrLabel5.Name = "xrLabel5";
			xrLabel5.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel5.SizeF = new SizeF(624.4166f, 58.42001f);
			xrLabel5.StylePriority.UseFont = false;
			lblFechamentoTurno.Dpi = 254f;
			lblFechamentoTurno.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			lblFechamentoTurno.LocationFloat = new PointFloat(1.425898f, 116.84f);
			lblFechamentoTurno.Name = "lblFechamentoTurno";
			lblFechamentoTurno.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			lblFechamentoTurno.SizeF = new SizeF(398.9375f, 58.42001f);
			lblFechamentoTurno.StylePriority.UseFont = false;
			lblFechamentoTurno.Text = "Fechamento do turno:";
			xrlFechamentoTurno.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "DataHoraFechamento", "{0:dd/MM/yyyy HH:mm}")
			});
			xrlFechamentoTurno.Dpi = 254f;
			xrlFechamentoTurno.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, GraphicsUnit.Point, 0);
			xrlFechamentoTurno.LocationFloat = new PointFloat(400.3634f, 116.84f);
			xrlFechamentoTurno.Name = "xrlFechamentoTurno";
			xrlFechamentoTurno.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrlFechamentoTurno.SizeF = new SizeF(620.3925f, 58.42001f);
			xrlFechamentoTurno.StylePriority.UseFont = false;
			Title.BackColor = System.Drawing.Color.Transparent;
			Title.BorderColor = System.Drawing.Color.Black;
			Title.Borders = BorderSide.None;
			Title.BorderWidth = 1f;
			Title.Font = new Font("Times New Roman", 20f, System.Drawing.FontStyle.Bold);
			Title.ForeColor = System.Drawing.Color.Maroon;
			Title.Name = "Title";
			FieldCaption.BackColor = System.Drawing.Color.Transparent;
			FieldCaption.BorderColor = System.Drawing.Color.Black;
			FieldCaption.Borders = BorderSide.None;
			FieldCaption.BorderWidth = 1f;
			FieldCaption.Font = new Font("Arial", 10f, System.Drawing.FontStyle.Bold);
			FieldCaption.ForeColor = System.Drawing.Color.Maroon;
			FieldCaption.Name = "FieldCaption";
			PageInfo.BackColor = System.Drawing.Color.Transparent;
			PageInfo.BorderColor = System.Drawing.Color.Black;
			PageInfo.Borders = BorderSide.None;
			PageInfo.BorderWidth = 1f;
			PageInfo.Font = new Font("Times New Roman", 10f, System.Drawing.FontStyle.Bold);
			PageInfo.ForeColor = System.Drawing.Color.Black;
			PageInfo.Name = "PageInfo";
			DataField.BackColor = System.Drawing.Color.Transparent;
			DataField.BorderColor = System.Drawing.Color.Black;
			DataField.Borders = BorderSide.None;
			DataField.BorderWidth = 1f;
			DataField.Font = new Font("Times New Roman", 10f);
			DataField.ForeColor = System.Drawing.Color.Black;
			DataField.Name = "DataField";
			DataField.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			TopMargin.Dpi = 254f;
			TopMargin.HeightF = 50f;
			TopMargin.Name = "TopMargin";
			bottomMarginBand1.Dpi = 254f;
			bottomMarginBand1.HeightF = 120f;
			bottomMarginBand1.Name = "bottomMarginBand1";
			bindingSource1.DataSource = typeof(ResumoTurnoDto);
			PageHeader.Controls.AddRange(new XRControl[3] { Empresa, Titulo, CNPJ });
			PageHeader.Dpi = 254f;
			PageHeader.HeightF = 300f;
			PageHeader.Name = "PageHeader";
			Empresa.Dpi = 254f;
			Empresa.Font = new Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			Empresa.LocationFloat = new PointFloat(0f, 0f);
			Empresa.Name = "Empresa";
			Empresa.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			Empresa.SizeF = new SizeF(1900f, 58.42f);
			Empresa.StylePriority.UseFont = false;
			Empresa.Text = "Empresa";
			Titulo.Dpi = 254f;
			Titulo.Font = new Font("Segoe UI", 19f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			Titulo.LocationFloat = new PointFloat(0f, 116.84f);
			Titulo.Name = "Titulo";
			Titulo.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			Titulo.SizeF = new SizeF(1900f, 99.99998f);
			Titulo.StylePriority.UseFont = false;
			Titulo.StylePriority.UseTextAlignment = false;
			Titulo.Text = "Resumo do Turno";
			Titulo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
			CNPJ.Dpi = 254f;
			CNPJ.Font = new Font("Segoe UI", 10f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			CNPJ.LocationFloat = new PointFloat(0f, 58.42001f);
			CNPJ.Name = "CNPJ";
			CNPJ.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			CNPJ.SizeF = new SizeF(1900f, 58.41998f);
			CNPJ.StylePriority.UseFont = false;
			CNPJ.Text = "CNPJ";
			DetailReport.Bands.AddRange(new Band[2] { Detail1, GroupFooter1 });
			DetailReport.DataMember = "ResumoTurnoLancamentos";
			DetailReport.DataSource = bindingSource1;
			DetailReport.Dpi = 254f;
			DetailReport.Level = 0;
			DetailReport.Name = "DetailReport";
			Detail1.Controls.AddRange(new XRControl[2] { xrLabel8, xrLabel9 });
			Detail1.Dpi = 254f;
			Detail1.HeightF = 58.42f;
			Detail1.Name = "Detail1";
			xrLabel8.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "ResumoTurnoLancamentos.Descricao")
			});
			xrLabel8.Dpi = 254f;
			xrLabel8.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel8.LocationFloat = new PointFloat(135.6772f, 0f);
			xrLabel8.Name = "xrLabel8";
			xrLabel8.OddStyleName = "xrControlStyle1";
			xrLabel8.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel8.SizeF = new SizeF(1193.271f, 58.42f);
			xrLabel8.StylePriority.UseFont = false;
			xrLabel8.StylePriority.UseTextAlignment = false;
			xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
			xrLabel9.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "ResumoTurnoLancamentos.Valor", "{0:n2}")
			});
			xrLabel9.Dpi = 254f;
			xrLabel9.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel9.LocationFloat = new PointFloat(1328.948f, 0f);
			xrLabel9.Name = "xrLabel9";
			xrLabel9.OddStyleName = "xrControlStyle1";
			xrLabel9.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel9.SizeF = new SizeF(333.375f, 58.42f);
			xrLabel9.StylePriority.UseFont = false;
			xrLabel9.StylePriority.UseTextAlignment = false;
			xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
			GroupFooter1.Controls.AddRange(new XRControl[2] { xrLabel10, xrLabel12 });
			GroupFooter1.Dpi = 254f;
			GroupFooter1.HeightF = 58.42f;
			GroupFooter1.Name = "GroupFooter1";
			xrLabel10.Borders = BorderSide.Top;
			xrLabel10.Dpi = 254f;
			xrLabel10.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, GraphicsUnit.Point, 0);
			xrLabel10.LocationFloat = new PointFloat(135.6772f, 0f);
			xrLabel10.Name = "xrLabel10";
			xrLabel10.OddStyleName = "xrControlStyle1";
			xrLabel10.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel10.SizeF = new SizeF(1193.271f, 58.42f);
			xrLabel10.StylePriority.UseBorders = false;
			xrLabel10.StylePriority.UseFont = false;
			xrLabel10.StylePriority.UseTextAlignment = false;
			xrLabel10.Text = "Total";
			xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
			xrLabel12.Borders = BorderSide.Top;
			xrLabel12.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "Total", "{0:n2}")
			});
			xrLabel12.Dpi = 254f;
			xrLabel12.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold);
			xrLabel12.LocationFloat = new PointFloat(1328.949f, 0f);
			xrLabel12.Name = "xrLabel12";
			xrLabel12.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel12.SizeF = new SizeF(333.38f, 58.42f);
			xrLabel12.StylePriority.UseBorders = false;
			xrLabel12.StylePriority.UseFont = false;
			xrLabel12.StylePriority.UseTextAlignment = false;
			xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
			Negrito.Name = "Negrito";
			xrControlStyle1.BackColor = System.Drawing.Color.Gainsboro;
			xrControlStyle1.Name = "xrControlStyle1";
			xrControlStyle1.Padding = new PaddingInfo(0, 0, 0, 0, 254f);
			PageFooter.Controls.AddRange(new XRControl[3] { DataEmissao, xrLabel1, xrPageInfo1 });
			PageFooter.Dpi = 254f;
			PageFooter.HeightF = 185.42f;
			PageFooter.Name = "PageFooter";
			DataEmissao.Dpi = 254f;
			DataEmissao.Font = new Font("Segoe UI", 10f);
			DataEmissao.LocationFloat = new PointFloat(0f, 68.58002f);
			DataEmissao.Name = "DataEmissao";
			DataEmissao.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			DataEmissao.SizeF = new SizeF(1900f, 58.42f);
			DataEmissao.StylePriority.UseFont = false;
			DataEmissao.StylePriority.UseTextAlignment = false;
			DataEmissao.Text = "DataEmissao";
			DataEmissao.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			xrLabel1.Dpi = 254f;
			xrLabel1.Font = new Font("Segoe UI", 10f);
			xrLabel1.LocationFloat = new PointFloat(380.0004f, 127f);
			xrLabel1.Name = "xrLabel1";
			xrLabel1.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel1.SizeF = new SizeF(1140f, 58.42f);
			xrLabel1.StylePriority.UseFont = false;
			xrLabel1.StylePriority.UseTextAlignment = false;
			xrLabel1.Text = "Desenvolvido por DigiSat Tecnologia LTDA®  www.digisat.com.br";
			xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			xrPageInfo1.Dpi = 254f;
			xrPageInfo1.Font = new Font("Segoe UI", 10f);
			xrPageInfo1.Format = " Página {0} de {1}";
			xrPageInfo1.LocationFloat = new PointFloat(1520f, 127f);
			xrPageInfo1.Name = "xrPageInfo1";
			xrPageInfo1.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrPageInfo1.SizeF = new SizeF(380f, 58.42f);
			xrPageInfo1.StylePriority.UseFont = false;
			xrPageInfo1.StylePriority.UseTextAlignment = false;
			xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			DetailReport1.Bands.AddRange(new Band[2] { Detail2, GroupHeader1 });
			DetailReport1.DataMember = "DetalhamentosSangriaSuprimento";
			DetailReport1.DataSource = bindingSource1;
			DetailReport1.Dpi = 254f;
			DetailReport1.Level = 1;
			DetailReport1.Name = "DetailReport1";
			Detail2.Controls.AddRange(new XRControl[4] { xrLabel20, xrLabel19, xrLabel18, xrLabel17 });
			Detail2.Dpi = 254f;
			Detail2.HeightF = 58.42f;
			Detail2.Name = "Detail2";
			xrLabel20.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "DetalhamentosSangriaSuprimento.Valor", "{0:n2}")
			});
			xrLabel20.Dpi = 254f;
			xrLabel20.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel20.LocationFloat = new PointFloat(1606.753f, 0f);
			xrLabel20.Name = "xrLabel20";
			xrLabel20.OddStyleName = "xrControlStyle1";
			xrLabel20.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel20.SizeF = new SizeF(239.4481f, 58.42f);
			xrLabel20.StylePriority.UseFont = false;
			xrLabel20.StylePriority.UseTextAlignment = false;
			xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
			xrLabel19.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "DetalhamentosSangriaSuprimento.Observacao", "{0:dd/MM/yyyy HH:mm}")
			});
			xrLabel19.Dpi = 254f;
			xrLabel19.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel19.LocationFloat = new PointFloat(695.2689f, 0f);
			xrLabel19.Name = "xrLabel19";
			xrLabel19.OddStyleName = "xrControlStyle1";
			xrLabel19.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel19.SizeF = new SizeF(911.4838f, 58.42f);
			xrLabel19.StylePriority.UseFont = false;
			xrLabel19.StylePriority.UseTextAlignment = false;
			xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
			xrLabel18.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "DetalhamentosSangriaSuprimento.DataHora", "{0:dd/MM/yyyy HH:mm}")
			});
			xrLabel18.Dpi = 254f;
			xrLabel18.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel18.LocationFloat = new PointFloat(357.9261f, 0f);
			xrLabel18.Name = "xrLabel18";
			xrLabel18.OddStyleName = "xrControlStyle1";
			xrLabel18.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel18.SizeF = new SizeF(337.3427f, 58.42f);
			xrLabel18.StylePriority.UseFont = false;
			xrLabel18.StylePriority.UseTextAlignment = false;
			xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
			xrLabel17.DataBindings.AddRange(new XRBinding[1]
			{
				new XRBinding("Text", null, "DetalhamentosSangriaSuprimento.Descricao")
			});
			xrLabel17.Dpi = 254f;
			xrLabel17.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel17.LocationFloat = new PointFloat(57.62508f, 0f);
			xrLabel17.Name = "xrLabel17";
			xrLabel17.OddStyleName = "xrControlStyle1";
			xrLabel17.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel17.SizeF = new SizeF(300.301f, 58.42f);
			xrLabel17.StylePriority.UseFont = false;
			xrLabel17.StylePriority.UseTextAlignment = false;
			xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
			GroupHeader1.Controls.AddRange(new XRControl[5] { xrLabel16, xrLabel15, xrLabel14, xrLabel13, xrLabel11 });
			GroupHeader1.Dpi = 254f;
			GroupHeader1.HeightF = 168.7917f;
			GroupHeader1.Name = "GroupHeader1";
			xrLabel16.Borders = BorderSide.Bottom;
			xrLabel16.Dpi = 254f;
			xrLabel16.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel16.LocationFloat = new PointFloat(1606.753f, 118.7917f);
			xrLabel16.Name = "xrLabel16";
			xrLabel16.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel16.SizeF = new SizeF(239.4482f, 49.99998f);
			xrLabel16.StylePriority.UseBorders = false;
			xrLabel16.StylePriority.UseFont = false;
			xrLabel16.StylePriority.UseTextAlignment = false;
			xrLabel16.Text = "Valor";
			xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
			xrLabel15.Borders = BorderSide.Bottom;
			xrLabel15.Dpi = 254f;
			xrLabel15.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel15.LocationFloat = new PointFloat(695.2687f, 118.7917f);
			xrLabel15.Name = "xrLabel15";
			xrLabel15.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel15.SizeF = new SizeF(911.484f, 49.99998f);
			xrLabel15.StylePriority.UseBorders = false;
			xrLabel15.StylePriority.UseFont = false;
			xrLabel15.StylePriority.UseTextAlignment = false;
			xrLabel15.Text = "Observação";
			xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
			xrLabel14.Borders = BorderSide.Bottom;
			xrLabel14.Dpi = 254f;
			xrLabel14.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel14.LocationFloat = new PointFloat(357.926f, 118.7917f);
			xrLabel14.Name = "xrLabel14";
			xrLabel14.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel14.SizeF = new SizeF(337.3427f, 49.99998f);
			xrLabel14.StylePriority.UseBorders = false;
			xrLabel14.StylePriority.UseFont = false;
			xrLabel14.StylePriority.UseTextAlignment = false;
			xrLabel14.Text = "Data/Hora";
			xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
			xrLabel13.Borders = BorderSide.Bottom;
			xrLabel13.Dpi = 254f;
			xrLabel13.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel13.LocationFloat = new PointFloat(57.62508f, 118.7917f);
			xrLabel13.Name = "xrLabel13";
			xrLabel13.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel13.SizeF = new SizeF(300.301f, 49.99998f);
			xrLabel13.StylePriority.UseBorders = false;
			xrLabel13.StylePriority.UseFont = false;
			xrLabel13.StylePriority.UseTextAlignment = false;
			xrLabel13.Text = "Descrição";
			xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
			xrLabel11.Borders = BorderSide.None;
			xrLabel11.Dpi = 254f;
			xrLabel11.Font = new Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, 0);
			xrLabel11.LocationFloat = new PointFloat(1.425906f, 50.27083f);
			xrLabel11.Name = "xrLabel11";
			xrLabel11.Padding = new PaddingInfo(5, 5, 0, 0, 254f);
			xrLabel11.SizeF = new SizeF(787.0324f, 49.99998f);
			xrLabel11.StylePriority.UseBorders = false;
			xrLabel11.StylePriority.UseFont = false;
			xrLabel11.StylePriority.UseTextAlignment = false;
			xrLabel11.Text = "Detalhamento dos Suprimentos e Sangrias";
			xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
			Bands.AddRange(new Band[7] { Detail, TopMargin, bottomMarginBand1, PageHeader, DetailReport, PageFooter, DetailReport1 });
			DataSource = bindingSource1;
			Dpi = 254f;
			FormattingRuleSheet.AddRange(new FormattingRule[1] { Negrito });
			Margins = new Margins(100, 100, 50, 120);
			PageHeight = 2970;
			PageWidth = 2100;
			PaperKind = PaperKind.A4;
			ReportUnit = ReportUnit.TenthsOfAMillimeter;
			StyleSheet.AddRange(new XRControlStyle[5] { Title, FieldCaption, PageInfo, DataField, xrControlStyle1 });
			Version = "15.2";
			((ISupportInitialize)bindingSource1).EndInit();
			((ISupportInitialize)this).EndInit();
		}
	}
}
namespace Otus.Modulo.Turnos.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (resourceMan == null)
				{
					resourceMan = new ResourceManager("Otus.Modulo.Turnos.Properties.Resources", typeof(Resources).Assembly);
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal Resources()
		{
		}
	}
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
	internal sealed class Settings : ApplicationSettingsBase
	{
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

		public static Settings Default => defaultInstance;
	}
}
