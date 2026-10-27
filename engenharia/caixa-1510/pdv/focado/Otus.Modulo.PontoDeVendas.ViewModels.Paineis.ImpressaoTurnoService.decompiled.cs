using System.Linq;
using DigisatServer.Application.Main.Dto.Auxiliares.Turnos;
using DigisatServer.Distributed.Wcf.Main.Contracts.AuxiliaresModulo;
using DigisatServer.Domain.Main.Interfaces;
using DigisatServer.Domain.Main.Services;
using DigisatServer.Infrastructure.Crosscutting.Dto.Relatorios;
using DigisatServer.Infrastructure.Crosscutting.Enum;
using DigisatServer.Infrastructure.Crosscutting.Helper;
using DigisatServer.Infrastructure.Crosscutting.Interfaces;
using Otus.Infraestrutura.Models;
using Otus.Modulo.PontoDeVendas.Interfaces;

namespace Otus.Modulo.PontoDeVendas.ViewModels.Paineis;

public class ImpressaoTurnoService
{
	private readonly IEmissorService _emissorService;

	private readonly IWService _service;

	private readonly IGeradorImpressoTurnoService _geradorImpressoTurnoService;

	private TurnoDto _turnoDto;

	public ImpressaoTurnoService(IEmissorService emissorService, IWService service)
	{
		_emissorService = emissorService;
		_service = service;
		_geradorImpressoTurnoService = new GeradorImpressoTurnoService(DadosSistema.TamanhoBobina == TamanhoBobina.CinquentaESeisCinquentaESeteCinquentaEOitoMilimetros && _emissorService.IsNot<IEcfService>());
	}

	public void ExecutarFechamentoTurno(TurnoDto turnoDto, string idTurnoFechado)
	{
		_turnoDto = turnoDto;
		ConferenciaTurnoDto result = _service.Canal((ITurnoRelatorioModuleService x) => x.ObterConferenciaTurno(idTurnoFechado)).Result;
		TotalConvenioTurnoDto totalConvenioTurno = _service.Canal((ITurnoModuleService service) => service.ObterTotalDescontoConvenioAplicadoNoTurno(_turnoDto.Id));
		TotalCanceladasTurnoDto totalCanceladasTurno = _service.Canal((ITurnoModuleService service) => service.ObterTotalCancelamentosNoTurno(_turnoDto.Id));
		_emissorService.ImprimirRelatorioGerencialFechamentoTurno(_geradorImpressoTurnoService.ObterRelatorioFechamentoDeTurno(_turnoDto).Concat(_geradorImpressoTurnoService.ObterRelatorioConferenciaFechamentoDeTurno(result, totalConvenioTurno, totalCanceladasTurno, DadosSistema.ConfiguracaoDto.Saida.ImprimirItensVendidosFechamentoTurno.Valor)));
	}

	public void ObterRelatorioResumoFechamentoDeTurnoSintetico(TurnoDto turnoDto, string idTurnoFechado, bool imprimirItens, bool imprimirMovimentacoesPrazo, bool imprimirLancamentosManuais)
	{
		_turnoDto = turnoDto;
		ConferenciaTurnoSinteticoDto result = _service.Canal((ITurnoRelatorioModuleService x) => x.ObterConferenciaTurnoSintetico(idTurnoFechado)).Result;
		_emissorService.ImprimirRelatorioGerencialFechamentoTurno(_geradorImpressoTurnoService.ObterRelatorioResumoFechamentoDeTurno(_turnoDto, result, imprimirItens, imprimirMovimentacoesPrazo, imprimirLancamentosManuais));
	}
}
