using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;
using DigisatServer.Application.Main.Dto;
using DigisatServer.Application.Main.Dto.Auxiliares.Turnos;
using DigisatServer.Distributed.Wcf.Main.Contracts.AuxiliaresModulo;
using DigisatServer.Infrastructure.Crosscutting.Dto.Relatorios;
using DigisatServer.Infrastructure.Crosscutting.Helper;
using DigisatServer.Infrastructure.Crosscutting.Interfaces;
using DigisatServer.Infrastructure.Crosscutting.Model.Sistemas;
using DigisatServer.Infrastructure.Crosscutting.Module;
using DigisatServer.Infrastructure.Crosscutting.Mvvm;
using DigisatServer.Infrastructure.Crosscutting.Report.Models;
using DigisatServer.Infrastructure.Crosscutting.Right;
using Otus.Infraestrutura.Interfaces;
using Otus.Infraestrutura.Models;
using Otus.Infraestrutura.ViewModels;
using Otus.Infraestrutura.ViewModels.Options;
using Otus.Modulo.Turnos.Reports;
using Otus.Modulo.Turnos.ViewModels.Jornadas;
using Otus.Modulo.Turnos.ViewModels.Turnos;
using Otus.Modulo.Turnos.Views;
using Prism.Regions;

namespace Otus.Modulo.Turnos.ViewModels;

public class TurnoListaViewModel : ListaManutencaoBase<TurnoDto, TurnoViewModel, Turno>
{
	private readonly IWService _service;

	private JornadaDto _jornada;

	public bool Ativo
	{
		get
		{
			if (((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens != null && ((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.SelecionadoValido)
			{
				return ((ViewModelAtivoBase<TurnoDto>)((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado).Ativo;
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
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected Obj, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected Obj, but got Unknown
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected Obj, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected Obj, but got Unknown
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected Obj, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected Obj, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected Obj, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Expected Obj, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected Obj, but got Unknown
		_service = service;
		ImprimirCommand = new DelegateCommand((Action)Imprimir, (Func<bool>)PodeImprimir, (bool?)null);
		ImprimirResumoConferenciaCommand = new DelegateCommand((Action)ImprimirResumoConferencia, (Func<bool>)PodeImprimirResumoConferencia, (bool?)null);
		MovimentacoesTurnoCommand = new DelegateCommand((Action)MovimentacoesTurno, (Func<bool>)PodeMovimentacoesTurno, (bool?)null);
		FecharTurnoCommand = new DelegateCommand((Action)FecharTurno, (Func<bool>)PodeFecharTurno, (bool?)null);
		CorrigirFechamentoTurnoCommand = new DelegateCommand((Action)CorrigirFechamentoTurno, (Func<bool>)PodeCorrigirFechamentoTurno, (bool?)null);
		AbrirJornadaCommand = new DelegateCommand((Action)AbrirJornada, (Func<bool>)PodeAbrirJornada, (bool?)null);
		FecharJornadaCommand = new DelegateCommand((Action)FecharJornada, (Func<bool>)PodeFecharJornada, (bool?)null);
		VisualizarRelatorioCommand = (ICommand)new DelegateCommand((Action)Visualizar, (Func<bool>)PodeVisualizar, (bool?)null);
		ConferirFechamentoTurnoCommand = (ICommand)new DelegateCommand((Action)ConferirFechamentoTurno, (Func<bool>)PodeConferirFechamentoTurno, (bool?)null);
		((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Pesquisa.AdicionarOpcao((OpcaoBase)(object)((ativos is OpcaoBase) ? ativos : null));
		((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Pesquisa.AdicionarOpcao((OpcaoBase)(object)((inativos is OpcaoBase) ? inativos : null));
		((NavigationBase)this).ParametroInformado += OnInicializar;
	}

	private async void Imprimir()
	{
		try
		{
			XtraReportExtensions.ShowPreviewDialog((IReport)(object)((Relatorio)new ResumoTurno(await _service.Canal<ITurnoRelatorioModuleService, Task<ResumoTurnoDto>>((Func<ITurnoRelatorioModuleService, Task<ResumoTurnoDto>>)((ITurnoRelatorioModuleService service) => service.ObterResumoTurno(((EntityBaseDto)((ViewModelDtoBase<TurnoDto>)(object)((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado).Dto).Id))))).Processar());
		}
		catch (Exception ex)
		{
			DialogoFactory.MostrarDialogoConfirma(ex.Message, false, HorizontalAlignment.Center);
		}
	}

	private bool PodeImprimir()
	{
		if (((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens != null && ((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.SelecionadoValido && ((ViewModelAtivoBase<TurnoDto>)((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado).Ativo)
		{
			return ((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado.TurnoFechado;
		}
		return false;
	}

	private async void ImprimirResumoConferencia()
	{
		try
		{
			XtraReportExtensions.ShowPreviewDialog((IReport)(object)((Relatorio)new ResumoConferenciaTurno(await _service.Canal<ITurnoRelatorioModuleService, Task<ResumoConferenciaTurnoDto>>((Func<ITurnoRelatorioModuleService, Task<ResumoConferenciaTurnoDto>>)((ITurnoRelatorioModuleService service) => service.ObterResumoConferenciaTurno(((EntityBaseDto)((ViewModelDtoBase<TurnoDto>)(object)((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado).Dto).Id))))).Processar());
		}
		catch (Exception ex)
		{
			DialogoFactory.MostrarDialogoConfirma(ex.Message, false, HorizontalAlignment.Center);
		}
	}

	private bool PodeImprimirResumoConferencia()
	{
		if (((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens != null && ((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.SelecionadoValido && ((ViewModelAtivoBase<TurnoDto>)((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado).Ativo)
		{
			return ((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado.TurnoConferido;
		}
		return false;
	}

	private async void MovimentacoesTurno()
	{
		try
		{
			TurnoDto dto = ((ViewModelDtoBase<TurnoDto>)(object)((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado).Dto;
			IEnumerable<TurnoLancamentoDto> dto2 = await _service.Canal<ITurnoModuleService, Task<IEnumerable<TurnoLancamentoDto>>>((Func<ITurnoModuleService, Task<IEnumerable<TurnoLancamentoDto>>>)((ITurnoModuleService service) => service.ObterMovimentacoesDoTurnoAsync(dto)));
			TurnoListaViewModel turnoListaViewModel = this;
			string name = typeof(TurnoMovimentacoesFichaView).Name;
			Action<NavigationResult> action = (NavigationResult result) =>
			{
			};
			NavigationParameters val = new NavigationParameters();
			val.Add(typeof(TurnoMovimentacaoViewModel).Name, (object)new TurnoMovimentacaoViewModel(dto2));
			((NavigationBase)turnoListaViewModel).Navegar(name, action, val);
		}
		catch (Exception ex)
		{
			DialogoFactory.MostrarDialogoConfirma(ex.Message, false, HorizontalAlignment.Center);
		}
	}

	private bool PodeMovimentacoesTurno()
	{
		if (((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens != null && ((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.SelecionadoValido)
		{
			return ((ViewModelAtivoBase<TurnoDto>)((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado).Ativo;
		}
		return false;
	}

	private void FecharTurno()
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected Obj, but got Unknown
		TurnoDto dto = ((ViewModelDtoBase<TurnoDto>)(object)((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado).Dto;
		string name = typeof(TurnoFechamentoFichaView).Name;
		Action<NavigationResult> action = (NavigationResult result) =>
		{
		};
		NavigationParameters val = new NavigationParameters();
		val.Add(typeof(TurnoFechamentoViewModel).Name, (object)new TurnoFechamentoViewModel(dto));
		((NavigationBase)this).Navegar(name, action, val);
	}

	private bool PodeFecharTurno()
	{
		if (HelperObject.IsNotNull((object)((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado) && ((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado.PodeFecharTurno && ((ViewModelAtivoBase<TurnoDto>)((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado).Ativo && !((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado.TurnoFechado)
		{
			return !((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado.TurnoConferido;
		}
		return false;
	}

	private bool PodeCorrigirFechamentoTurno()
	{
		if (((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens != null && ((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.SelecionadoValido && ((ViewModelAtivoBase<TurnoDto>)((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado).Ativo && ((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado.TurnoFechado)
		{
			return !((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado.TurnoConferido;
		}
		return false;
	}

	private void CorrigirFechamentoTurno()
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected Obj, but got Unknown
		if (DadosSistema.AutenticacaoService.Instancia.VerificarPermissao<Turno, PermitirCorrigirFechamentoTurno>() != null)
		{
			TurnoDto dto = ((ViewModelDtoBase<TurnoDto>)(object)((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado).Dto;
			string name = typeof(TurnoFechamentoFichaView).Name;
			Action<NavigationResult> action = (NavigationResult result) =>
			{
			};
			NavigationParameters val = new NavigationParameters();
			val.Add(typeof(TurnoFechamentoViewModel).Name, (object)new TurnoFechamentoViewModel(dto));
			((NavigationBase)this).Navegar(name, action, val);
		}
	}

	private void AbrirJornada()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected Obj, but got Unknown
		JornadaDto dto = ObterDadosAberturaJornada();
		Action<NavigationResult> action = (NavigationResult result) =>
		{
		};
		NavigationParameters val = new NavigationParameters();
		val.Add(typeof(JornadaAberturaViewModel).Name, (object)new JornadaAberturaViewModel(dto));
		((NavigationBase)this).Navegar("JornadaAberturaFichaView", action, val);
	}

	private JornadaDto ObterDadosAberturaJornada()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected Obj, but got Unknown
		if ((int)((ListaSelecaoBase<TurnoDto, TurnoViewModel, Turno>)(object)this).Sessao.Perfis.ItemSelecionado.Empresa.RamoAtividade.Tipo != 2)
		{
			return new JornadaDto();
		}
		return _service.Canal<IJornadaModuleService, JornadaDto>((Func<IJornadaModuleService, JornadaDto>)((IJornadaModuleService service) => service.ObterDadosAberturaJornada()));
	}

	private bool PodeAbrirJornada()
	{
		if (((ListaSelecaoBase<TurnoDto, TurnoViewModel, Turno>)(object)this).Sessao.VerificarPermissao<Jornada, Adicionar>())
		{
			return HelperObject.Is<JornadaDtoNull>((object)_jornada);
		}
		return false;
	}

	private void FecharJornada()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected Obj, but got Unknown
		Action<NavigationResult> action = (NavigationResult result) =>
		{
		};
		NavigationParameters val = new NavigationParameters();
		val.Add(typeof(JornadaFechamentoViewModel).Name, (object)new JornadaFechamentoViewModel(_jornada));
		((NavigationBase)this).Navegar("JornadaFechamentoFichaView", action, val);
	}

	private bool PodeFecharJornada()
	{
		if (((ListaSelecaoBase<TurnoDto, TurnoViewModel, Turno>)(object)this).Sessao.VerificarPermissao<Jornada, Atualizar>())
		{
			return HelperObject.IsNot<JornadaDtoNull>((object)_jornada);
		}
		return false;
	}

	private bool PodeVisualizar()
	{
		if (DadosSistema.VerificarPermissao<Turno, VisualizarRelatorio>() && DadosSistema.PodeExecutarFuncao(new Type[2]
		{
			typeof(Administrador),
			typeof(AdministradorIndustria)
		}))
		{
			return !((ListaSelecaoBase<TurnoDto, TurnoViewModel>)(object)this).PodeSelecionar();
		}
		return false;
	}

	private void Visualizar()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected Obj, but got Unknown
		HelperObject.To<ResolucaoBase>((object)((NavigationBase)this).Resolucao).Maximizar();
		NavigationParameters val = new NavigationParameters();
		val.Add("ItemModuloCreator", (object)67);
		val.Add("VoltarAction", (object)(Action)(() =>
		{
			HelperObject.To<ResolucaoBase>((object)((NavigationBase)this).Resolucao).Restaurar();
		}));
		NavigationParameters val2 = val;
		((NavigationBase)this).Navegar("RelatorioFichaView", (Action<NavigationResult>)((NavigationResult navigationResult) =>
		{
		}), val2);
	}

	private void OnInicializar(object sender, NavigationParameters navigationParameters)
	{
		IniciarAberturaJornada();
	}

	private void IniciarAberturaJornada()
	{
		_jornada = _service.Canal<IJornadaModuleService, Task<JornadaDto>>((Func<IJornadaModuleService, Task<JornadaDto>>)((IJornadaModuleService service) => service.ObterJornadaAberta())).Result;
	}

	private bool PodeConferirFechamentoTurno()
	{
		if (((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens != null && ((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.SelecionadoValido && ((ViewModelAtivoBase<TurnoDto>)((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado).Ativo && ((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado.TurnoFechado)
		{
			return !((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado.TurnoConferido;
		}
		return false;
	}

	private void ConferirFechamentoTurno()
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected Obj, but got Unknown
		TurnoDto dto = ((ViewModelDtoBase<TurnoDto>)(object)((ListaBase<TurnoDto, TurnoViewModel>)(object)this).Itens.Selecionado).Dto;
		List<DadosRelatorioConferenciaTurnoItemDto> especiesRecebimento = _service.Canal<ITurnoRelatorioModuleService, Task<ConferenciaTurnoDto>>((Func<ITurnoRelatorioModuleService, Task<ConferenciaTurnoDto>>)((ITurnoRelatorioModuleService x) => x.ObterConferenciaTurno(((EntityBaseDto)dto).Id))).Result.EspeciesRecebimento;
		dto.Fechamento.EspeciesRecebimento = especiesRecebimento;
		string name = typeof(TurnoConferenciaFechamentoFichaView).Name;
		Action<NavigationResult> action = (NavigationResult result) =>
		{
		};
		NavigationParameters val = new NavigationParameters();
		val.Add(typeof(TurnoConferenciaFechamentoViewModel).Name, (object)new TurnoConferenciaFechamentoViewModel(dto));
		((NavigationBase)this).Navegar(name, action, val);
	}

	protected override bool PodeAdicionar()
	{
		return true;
	}

	public override void Dispose()
	{
		((NavigationBase)this).ParametroInformado -= OnInicializar;
		base.Dispose();
	}
}
