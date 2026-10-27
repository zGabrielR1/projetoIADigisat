using System;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;
using DigisatServer.Application.Main.Dto;
using DigisatServer.Application.Main.Dto.Auxiliares.Turnos;
using DigisatServer.Distributed.Wcf.Main.Contracts.AuxiliaresModulo;
using DigisatServer.Infrastructure.Crosscutting.Dto.Relatorios;
using DigisatServer.Infrastructure.Crosscutting.Interfaces;
using DigisatServer.Infrastructure.Crosscutting.Module;
using DigisatServer.Infrastructure.Crosscutting.Mvvm;
using DigisatServer.Infrastructure.Crosscutting.Mvvm.Interfaces;
using DigisatServer.Infrastructure.Crosscutting.Report.Models;
using DigisatServer.Infrastructure.Crosscutting.Right;
using Otus.Infraestrutura.Models;
using Otus.Infraestrutura.ViewModels;
using Otus.Modulo.Turnos.Reports;
using Otus.Modulo.Turnos.ViewModels.Turnos;

namespace Otus.Modulo.Turnos.ViewModels;

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
		TurnoDto dto = await _service.Canal<ITurnoModuleService, Task<TurnoDto>>((Func<ITurnoModuleService, Task<TurnoDto>>)((ITurnoModuleService service) => service.FecharTurnoAsync(formulario.ToDto<TurnoDto>())));
		DadosSistema.SetarTurnoAbertoDoUsuario(false);
		await ImprimirResumoTurno(dto);
		base.SelecaoNavigateParameterCallBack?.CallBack(dto);
		return dto;
	}

	private async Task ImprimirResumoTurno(TurnoDto turnoDto)
	{
		if (DialogoFactory.MostrarDialogoSimNao("Turno fechado! Deseja imprimir o resumo do turno?", false, 0, HorizontalAlignment.Center) == MessageBoxResult.Yes)
		{
			XtraReportExtensions.ShowPreviewDialog((IReport)(object)((Relatorio)new ResumoTurno(await _service.Canal<ITurnoRelatorioModuleService, Task<ResumoTurnoDto>>((Func<ITurnoRelatorioModuleService, Task<ResumoTurnoDto>>)((ITurnoRelatorioModuleService service) => service.ObterResumoTurno(((EntityBaseDto)turnoDto).Id))))).Processar());
		}
	}
}
