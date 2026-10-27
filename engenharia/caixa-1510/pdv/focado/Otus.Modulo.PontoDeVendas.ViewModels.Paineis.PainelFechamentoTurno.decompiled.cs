using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DigisatServer.Application.Main.Dto.Auxiliares.Turnos;
using DigisatServer.Application.Main.Dto.Movimentacoes;
using DigisatServer.Distributed.Wcf.Main.Contracts.AuxiliaresModulo;
using DigisatServer.Distributed.Wcf.Main.Contracts.SaidasModulo;
using DigisatServer.Domain.Main.Aggregates.AuxiliaresAgg.Pbms;
using DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg;
using DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.RegistrosPafEcf;
using DigisatServer.Infrastructure.Crosscutting.Concentrador.Interfaces;
using DigisatServer.Infrastructure.Crosscutting.Enum;
using DigisatServer.Infrastructure.Crosscutting.Helper;
using DigisatServer.Infrastructure.Crosscutting.Interfaces;
using DigisatServer.Infrastructure.Crosscutting.Module;
using DigisatServer.Infrastructure.Crosscutting.Mvvm;
using DigisatServer.Infrastructure.Crosscutting.Mvvm.Interfaces;
using DigisatServer.Infrastructure.Crosscutting.Right;
using DigisatServer.Infrastructure.Crosscutting.ViewModel;
using DigisatServer.Infrastructure.Data.Core.Interfaces;
using Otus.Infraestrutura.Interfaces;
using Otus.Infraestrutura.Models;
using Otus.Modulo.PontoDeVendas.Interfaces;
using Otus.Modulo.PontoDeVendas.Services;
using Otus.Modulo.PontoDeVendas.ViewModels.ItensInteracao;

namespace Otus.Modulo.PontoDeVendas.ViewModels.Paineis;

public class PainelFechamentoTurno : PainelInteracao<PainelAberturaTurno>
{
	private readonly IEmissorService _emissorService;

	private readonly IWService _service;

	private readonly Seletor<IPainel> _seletorEsquerdo;

	private readonly ISessaoService _sessaoService;

	private ItemInteracaoDecimalFocusable _totalEspecie;

	private ItemInteracaoDecimal _totalCheque;

	private ItemInteracaoDecimal _totalCartao;

	private ItemInteracaoDecimal _totalVale;

	private ItemInteracaoDecimal _totalDepositoBancario;

	private ItemInteracaoDecimal _totalTed;

	private ItemInteracaoDecimal _totalDoc;

	private ItemInteracaoDecimal _totalValePresente;

	private ItemInteracaoDecimal _totalOutrasEspecies;

	private ItemInteracaoDecimal _totalCarteirasDigitais;

	private ItemInteracaoDecimal _totalPrazo;

	private ItemInteracaoDecimal _totalCreditoLoja;

	private ItemInteracaoTexto _observacao;

	private TurnoDto _turnoDto;

	private bool _navegouPainelPendenciasNfce;

	private readonly SincronizacaoService _sincronizacaoService;

	public bool Processando
	{
		get
		{
			return GetProperty(() => Processando);
		}
		set
		{
			SetProperty(() => Processando, value);
		}
	}

	public override string Titulo => "Fechar turno";

	public PainelFechamentoTurno()
	{
	}

	public PainelFechamentoTurno(IRepositoryRead<Movimentacao> movimentacaoRepositoryRead, IRepositoryRead<RegistroPafEcf> registroPafEcfRepositoryRead, IRepositoryRead<DigisatServer.Domain.Main.Aggregates.AuxiliaresAgg.Pbms.TransacaoPbm> transacaoPbmRepositoryRead, IEmissorService emissorService, IWService service, Seletor<IPainel> seletorEsquerdo, Seletor<IPainel> seletorDireito, ISessaoService sessaoService)
		: base(seletorDireito)
	{
		_emissorService = emissorService;
		_service = service;
		_seletorEsquerdo = seletorEsquerdo;
		_sessaoService = sessaoService;
		_totalEspecie = ItemInteracaoBase<ItemInteracaoDecimalFocusable, decimal>.Criar().ComDescricao("Total em espécie").Build();
		_totalCheque = ItemInteracaoBase<ItemInteracaoDecimal, decimal>.Criar().ComDescricao("Total em cheque").Build();
		_totalCartao = ItemInteracaoBase<ItemInteracaoDecimal, decimal>.Criar().ComDescricao("Total em cartão").Build();
		_totalVale = ItemInteracaoBase<ItemInteracaoDecimal, decimal>.Criar().ComDescricao("Total em vale").Build();
		_totalDepositoBancario = ItemInteracaoBase<ItemInteracaoDecimal, decimal>.Criar().ComDescricao("Total em depósito bancário").Build();
		_totalTed = ItemInteracaoBase<ItemInteracaoDecimal, decimal>.Criar().ComDescricao("Total em TED").Build();
		_totalDoc = ItemInteracaoBase<ItemInteracaoDecimal, decimal>.Criar().ComDescricao("Total em DOC").Build();
		_totalValePresente = ItemInteracaoBase<ItemInteracaoDecimal, decimal>.Criar().ComDescricao("Total em vale presente").Build();
		_totalCarteirasDigitais = ItemInteracaoBase<ItemInteracaoDecimal, decimal>.Criar().ComDescricao("Total em carteiras digitais").Build();
		_totalCreditoLoja = ItemInteracaoBase<ItemInteracaoDecimal, decimal>.Criar().ComDescricao("Total em crédito loja").Build();
		_totalOutrasEspecies = ItemInteracaoBase<ItemInteracaoDecimal, decimal>.Criar().ComDescricao("Total em outras espécies").Build();
		_observacao = ItemInteracaoBase<ItemInteracaoTexto, string>.Criar().ComDescricao("Observação").Build();
		_totalPrazo = ItemInteracaoBase<ItemInteracaoDecimal, decimal>.Criar().ComDescricao("Total em prazo/pgto posterior").Build();
		Campos.Add(_totalEspecie);
		Campos.Add(_totalCheque);
		Campos.Add(_totalCartao);
		Campos.Add(_totalVale);
		Campos.Add(_totalValePresente);
		Campos.Add(_totalCreditoLoja);
		Campos.Add(_totalCarteirasDigitais);
		Campos.Add(_totalOutrasEspecies);
		if (emissorService.IsNot<IEcfService>() && emissorService.IsNot<INotaManualService>())
		{
			Campos.Add(_totalDepositoBancario);
			Campos.Add(_totalTed);
			Campos.Add(_totalDoc);
		}
		Campos.Add(_totalPrazo);
		Campos.Add(_observacao);
		_sincronizacaoService = new SincronizacaoService(movimentacaoRepositoryRead, registroPafEcfRepositoryRead, transacaoPbmRepositoryRead, service);
	}

	public override void Load()
	{
		new Thread(async () =>
		{
			Processando = true;
			LoaderProcessoService.MostrarLoader("Verificando turno aberto...");
			try
			{
				_turnoDto = _service.Canal((ITurnoModuleService service) => service.ObterTurnoAbertoDoUsuarioLogadoAsync().Result);
				if (_turnoDto.Is<TurnoDtoNull>())
				{
					throw new Exception("Nenhum turno aberto");
				}
				await Sincronizar();
				VerificarFechamentoTurnoPostoCombustivel();
				AbrirGaveta();
			}
			catch (Exception ex)
			{
				Exception ex2 = ex;
				Exception e = ex2;
				ThreadPrincipal.Executar(() =>
				{
					e.ShowLastException();
					Seletor.Voltar();
				});
			}
			finally
			{
				LoaderProcessoService.EsconderLoader();
				Processando = false;
				RaisePropertyChanged(() => Processando);
				RecuperarFoco();
			}
		}).Start();
	}

	private void AbrirGaveta()
	{
		if (_sessaoService.VerificarPermissao<PontoDeVenda, PermitirAbrirGaveta>() != null)
		{
			_emissorService.AbrirGaveta();
		}
	}

	public async Task Sincronizar()
	{
		_sincronizacaoService.DataInicial = DateTime.Today;
		_sincronizacaoService.DataFinal = DateTime.Today;
		await _sincronizacaoService.Processar().ContinueWith((Task task) =>
		{
			if (_sincronizacaoService.Erros.Any())
			{
				DialogoFactory.MostrarDialogoConfirma("Falha ao sincronizar: \n" + string.Join(Environment.NewLine, _sincronizacaoService.Erros.Select((string x) => x)));
			}
			LoaderProcessoService.EsconderLoader();
		}, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.Default);
	}

	private void VerificarFechamentoTurnoPostoCombustivel()
	{
		if (_navegouPainelPendenciasNfce)
		{
			return;
		}
		if (DadosSistema.IsPostoCombustivel && (_emissorService.Tipo == TipoMovimentacao.CupomFiscal || _emissorService.Tipo == TipoMovimentacao.NotaFiscalConsumidorEletronica))
		{
			List<IAbastecimento> abastecimentosPendentes = _seletorEsquerdo.Get<PainelEsquerdoPosto>().Lista.Itens.Select((AbastecimentoViewModel x) => x.Abastecimento).ToListEmptyWhenNull();
			if (!abastecimentosPendentes.Any())
			{
				VerificarNotasFicaisConsumidorEletronicasComPendencias();
				return;
			}
			ThreadPrincipal.Executar(() =>
			{
				_emissorService.EmitirDocumentoFiscalAbastecimentosFechamentoTurno(Seletor.VoltarTudo, VerificarNotasFicaisConsumidorEletronicasComPendencias, abastecimentosPendentes.ToArray());
			});
		}
		else
		{
			VerificarNotasFicaisConsumidorEletronicasComPendencias();
		}
	}

	private void VerificarNotasFicaisConsumidorEletronicasComPendencias()
	{
		if (!_emissorService.Is<INfceService>())
		{
			return;
		}
		IEnumerable<MovimentacaoBaseDto> movimentacoes = _service.Canal((INotaFiscalConsumidorEletronicaModuleService service) => service.ObterNfceComPendenciasTurno(_turnoDto.Id, _turnoDto.DataHoraAbertura, DateTime.Now)).Result;
		if (movimentacoes.Any())
		{
			ThreadPrincipal.Executar(() =>
			{
				Seletor.Get<PainelResumoNfceTurno>().SetarMovimentacoesComPendencias(movimentacoes);
				Seletor.Get<PainelResumoNfceTurno>().SetarTurno(_turnoDto);
				Seletor.Selecionar<PainelResumoNfceTurno>();
				_navegouPainelPendenciasNfce = true;
			});
		}
	}

	public override void UnLoad()
	{
		_emissorService.VerificarEstado();
		Limpar();
	}

	public override void FecharMethod()
	{
		base.FecharMethod();
		_navegouPainelPendenciasNfce = false;
	}

	public override bool PodeVoltar()
	{
		return true;
	}

	public override void Dispose()
	{
		_totalEspecie = null;
		_totalCartao = null;
		_totalCheque = null;
		_totalVale = null;
		_totalDepositoBancario = null;
		_totalTed = null;
		_totalDoc = null;
		_totalOutrasEspecies = null;
		_totalCreditoLoja = null;
		_totalValePresente = null;
		_totalCarteirasDigitais = null;
		_observacao = null;
		_totalPrazo = null;
	}

	protected override void Limpar()
	{
		_totalEspecie.Limpar();
		_totalCartao.Limpar();
		_totalCheque.Limpar();
		_totalVale.Limpar();
		_totalDepositoBancario.Limpar();
		_totalTed.Limpar();
		_totalDoc.Limpar();
		_totalOutrasEspecies.Limpar();
		_totalValePresente.Limpar();
		_totalCreditoLoja.Limpar();
		_totalCarteirasDigitais.Limpar();
		_observacao.Limpar();
		_totalPrazo.Limpar();
	}

	protected override void F2Method()
	{
		try
		{
			if (DadosSistema.AutenticacaoService.Instancia.VerificarPermissao<Turno, PermitirFecharTurno>() != null)
			{
				_turnoDto.Fechamento = new FechamentoDto
				{
					Observacao = _observacao.Valor,
					TotalEspecie = _totalEspecie.Valor,
					TotalCartao = _totalCartao.Valor,
					TotalCheque = _totalCheque.Valor,
					TotalVale = _totalVale.Valor,
					TotalDepositoBancario = _totalDepositoBancario.Valor,
					TotalTed = _totalTed.Valor,
					TotalDoc = _totalDoc.Valor,
					TotalValePresente = _totalValePresente.Valor,
					TotalOutrasEspecies = _totalOutrasEspecies.Valor,
					TotalCarteirasDigitais = _totalCarteirasDigitais.Valor,
					TotalPrazo = _totalPrazo.Valor,
					TotalCreditoLoja = _totalCreditoLoja.Valor
				};
				ImpressaoTurnoService impressaoTurnoService = new ImpressaoTurnoService(_emissorService, _service);
				TurnoDto turnoDto = _service.Canal((ITurnoModuleService service) => service.FecharTurnoAsync(_turnoDto).Result);
				_turnoDto.Fechamento.DataHoraFechamento = turnoDto.Fechamento.DataHoraFechamento;
				impressaoTurnoService.ExecutarFechamentoTurno(_turnoDto, turnoDto.Id);
				DialogoFactory.MostrarDialogoConfirma("O turno foi fechado");
				DadosSistema.SetarTurnoAbertoDoUsuario(turnoAberto: false);
				Seletor.Voltar();
			}
		}
		catch (Exception ex)
		{
			ex.ShowLastException();
		}
	}

	protected override bool CanF2Method()
	{
		if (base.CanF2Method() && _emissorService.Carregado && _emissorService.Pronto)
		{
			return !Processando;
		}
		return false;
	}
}
