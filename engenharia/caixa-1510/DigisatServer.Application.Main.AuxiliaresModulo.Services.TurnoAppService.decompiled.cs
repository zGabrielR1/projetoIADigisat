using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigisatServer.Application.Core.Contracts.AuxiliaresModulo;
using DigisatServer.Application.Core.Contracts.CadastrosModulo;
using DigisatServer.Application.Core.Projections;
using DigisatServer.Application.Core.Projections.AuxiliaresProjection.Turnos;
using DigisatServer.Application.Main.Dto;
using DigisatServer.Application.Main.Dto.Auxiliares.Turnos;
using DigisatServer.Application.Main.Resources;
using DigisatServer.Domain.Core;
using DigisatServer.Domain.Main.Aggregates.AuxiliaresAgg;
using DigisatServer.Domain.Main.Aggregates.AuxiliaresAgg.Turnos;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.ProdutosServicos;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.ProdutosServicos.Produtos;
using DigisatServer.Domain.Main.Aggregates.CadastrosAgg.Usuarios;
using DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg;
using DigisatServer.Infrastructure.Crosscutting;
using DigisatServer.Infrastructure.Crosscutting.Dto.Relatorios;
using DigisatServer.Infrastructure.Crosscutting.Enum;
using DigisatServer.Infrastructure.Crosscutting.Function;
using DigisatServer.Infrastructure.Crosscutting.Helper;
using DigisatServer.Infrastructure.Crosscutting.Interfaces;
using DigisatServer.Infrastructure.Crosscutting.Model.OrigensLancamentosTurno;
using DigisatServer.Infrastructure.Crosscutting.Validation;
using DigisatServer.Infrastructure.Crosscutting.ValueObject.FinanceirosVO.Especies.Recebimentos;
using DigisatServer.Infrastructure.Data.Core.Interfaces;
using MongoDB.Bson;

namespace DigisatServer.Application.Main.AuxiliaresModulo.Services;

public class TurnoAppService : ITurnoAppService, IAdicionarEmpresaAppService<TurnoDto>, IAtualizarEmpresaAppService<TurnoDto>
{
	private readonly ITurnoRepositoryRead _turnoRepositoryRead;

	private readonly IRepositoryRead<TurnoLancamento> _turnoLancamentoRepositoryRead;

	private readonly IRepositoryWrite<Turno> _turnoRepositoryWrite;

	private readonly IRepositoryWrite<TurnoLancamento> _turnoLancamentoRepositoryWrite;

	private readonly IUsuarioRepositoryRead _usuarioRepositoryRead;

	private readonly IMovimentacaoRepositoryRead _movimentacaoRepositoryRead;

	private readonly Habilitacao _habilitacao;

	private readonly IProdutoServicoRepositoryRead _produtoServicoRepositoryRead;

	private readonly IProdutoServicoEmpresaRepositoryRead _produtoServicoEmpresaRepositoryRead;

	public TurnoAppService(ITurnoRepositoryRead turnoRepositoryRead, IRepositoryWrite<Turno> turnoRepositoryWrite, IRepositoryRead<TurnoLancamento> turnoLancamentoRepositoryRead, IRepositoryWrite<TurnoLancamento> turnoLancamentoRepositoryWrite, IUsuarioRepositoryRead usuarioRepositoryRead, IMovimentacaoRepositoryRead movimentacaoRepositoryRead, Habilitacao habilitacao, IProdutoServicoRepositoryRead produtoServicoRepositoryRead, IProdutoServicoEmpresaRepositoryRead produtoServicoEmpresaRepositoryRead)
	{
		_turnoRepositoryRead = turnoRepositoryRead;
		_turnoRepositoryWrite = turnoRepositoryWrite;
		_turnoLancamentoRepositoryRead = turnoLancamentoRepositoryRead;
		_turnoLancamentoRepositoryWrite = turnoLancamentoRepositoryWrite;
		_usuarioRepositoryRead = usuarioRepositoryRead;
		_movimentacaoRepositoryRead = movimentacaoRepositoryRead;
		_habilitacao = habilitacao;
		_produtoServicoRepositoryRead = produtoServicoRepositoryRead;
		_produtoServicoEmpresaRepositoryRead = produtoServicoEmpresaRepositoryRead;
	}

	public async Task<TurnoDto> Adicionar(TurnoDto dto, IUsuarioIdentity usuarioIdentity)
	{
		UsuarioBase usuarioBase = _usuarioRepositoryRead.PorNome(usuarioIdentity.NomeUsuario);
		VerificarTurnoAberto(usuarioBase);
		dto.DataHoraAbertura = DateTime.Now;
		Turno turno = Turno.Criar(usuarioBase, ObjectId.Parse(usuarioIdentity.IdEmpresa), dto.DataHoraAbertura, dto.SaldoInicial, dto.Observacao);
		_turnoRepositoryWrite.Add(turno);
		if (dto.SaldoInicial <= 0m)
		{
			return _turnoRepositoryRead.GetById(turno.Id).ProjectedAsDto(_habilitacao.ObterSistemaPorCnpjEmpresa());
		}
		TurnoLancamento entity = FluentFactory<TurnoLancamentoBuilder>.Init().SetarEmpresaReferencia(ObjectId.Parse(usuarioIdentity.IdEmpresa)).SetarUsuarioReferencia(usuarioBase.Id)
			.SetarDataHora(dto.DataHoraAbertura)
			.SetarValor(dto.SaldoInicial)
			.SetarObservacao(string.IsNullOrEmpty(dto.Observacao) ? "Saldo inicial do turno" : $"Saldo inicial do turno: {dto.Observacao}")
			.SetarOrigemLancamento(OrigensLancamentosTurno.ObterOrigemLancamentoTurnoPorCodigo(5))
			.SetarEspecieRecebimento(new Dinheiro())
			.Criar();
		_turnoLancamentoRepositoryWrite.Add(entity);
		return await Task.Factory.StartNew(() => _turnoRepositoryRead.GetById(turno.Id).ProjectedAsDto(_habilitacao.ObterSistemaPorCnpjEmpresa()));
	}

	private void VerificarTurnoAberto(UsuarioBase usuario)
	{
		IEnumerable<Turno> enumerable = _turnoRepositoryRead.Find(TurnoSpecification.AtivoPorSituacaoEPorUsuario(SituacaoTurno.Aberto, usuario.Id));
		if (enumerable == null || !enumerable.Any())
		{
			return;
		}
		throw new InvalidOperationException("Usuário já possui um turno aberto.");
	}

	public async Task<TurnoDto> Atualizar(TurnoDto dto, IUsuarioIdentity usuarioIdentity)
	{
		AssertionConcern.AssertArgumentNotNull(dto, typeof(TurnoDto).Name);
		AssertionConcern.AssertArgumentFalse(dto.IsTransient(), string.Format(Messages.exception_Invalido, typeof(TurnoDto).Name));
		Turno turno = _turnoRepositoryRead.GetById(ObjectId.Parse(dto.Id));
		if (dto.Ativo)
		{
			turno.Ativar();
		}
		else
		{
			turno.Desativar();
		}
		turno.AlterarDataHoraAbertura(dto.DataHoraAbertura, usuarioIdentity.NomeUsuario);
		_turnoRepositoryWrite.Update(turno);
		return await Task.Factory.StartNew(() => turno.ProjectedAsDto(_habilitacao.ObterSistemaPorCnpjEmpresa()));
	}

	public async Task<TurnoDto> FecharTurno(TurnoDto turnoDto, IUsuarioIdentity usuarioIdentity)
	{
		GerarLog log = new GerarLog();
		log.GravarLog($"Iniciou FecharTurno. Usuário: {usuarioIdentity.NomeUsuario}");
		AssertionConcern.AssertArgumentNotNull(turnoDto, typeof(TurnoDto).Name);
		AssertionConcern.AssertArgumentFalse(turnoDto.IsTransient(), string.Format(Messages.exception_Invalido, typeof(TurnoDto).Name));
		ObjectId id = _usuarioRepositoryRead.PorNome(usuarioIdentity.NomeUsuario).Id;
		try
		{
			Turno turno;
			IEnumerable<EstoqueFechamento> estoquesFechamento;
			if (!turnoDto.TurnoFechado)
			{
				turno = TurnoAbertoDoUsuario(usuarioIdentity);
				if (turno == null)
				{
					throw new InvalidOperationException("Usuário não possui turno aberto.");
				}
				if (turno.UsuarioReferencia != id)
				{
					throw new InvalidOperationException("Turno não pode ser fechado pois pertence a outro usuário.");
				}
				turnoDto.Fechamento.DataHoraFechamento = DateTime.Now;
				IEnumerable<ProdutoServico> source = _produtoServicoRepositoryRead.Find(ProdutoServicoSpecifications.PorTipo(TipoProdutoServico.Combustivel));
				estoquesFechamento = from x in _produtoServicoEmpresaRepositoryRead.GetIn((ProdutoServicoEmpresa x) => x.ProdutoServicoReferencia, source.Select((ProdutoServico x) => x.Id), usuarioIdentity)
					select new EstoqueFechamento(x.Id, x.ProdutoServico.Descricao, ((ProdutoEmpresa)x).Estoque?.QuantidadeSaldo ?? 0m);
			}
			else
			{
				turno = _turnoRepositoryRead.GetById(turnoDto.Id.TryParseReferencia());
				turnoDto.Fechamento.DataHoraFechamento = turnoDto.Fechamento.DataHoraFechamento;
				estoquesFechamento = turno.TurnoFechamento.EstoquesFechamento;
			}
			turno.FecharTurno(FluentFactory<TurnoFechamentoBuilder>.Init().SetarDataHora(turnoDto.Fechamento.DataHoraFechamento).SetarDinheiro(turnoDto.Fechamento.TotalEspecie)
				.SetarCartao(turnoDto.Fechamento.TotalCartao)
				.SetarCheque(turnoDto.Fechamento.TotalCheque)
				.SetarVale(turnoDto.Fechamento.TotalVale)
				.SetarDepositoBancario(turnoDto.Fechamento.TotalDepositoBancario)
				.SetarTotalTed(turnoDto.Fechamento.TotalTed)
				.SetarTotalDoc(turnoDto.Fechamento.TotalDoc)
				.SetarTotalValePresente(turnoDto.Fechamento.TotalValePresente)
				.SetarTotalOutrasEspecies(turnoDto.Fechamento.TotalOutrasEspecies)
				.SetarObservacao(turnoDto.Fechamento.Observacao)
				.SetarTotalCarteirasDigitais(turnoDto.Fechamento.TotalCarteirasDigitais)
				.SetarTotalPrazo(turnoDto.Fechamento.TotalPrazo)
				.SetarTotalCreditoLoja(turnoDto.Fechamento.TotalCreditoLoja)
				.SetarEstoquesFechamento(estoquesFechamento)
				.Criar(), usuarioIdentity.NomeUsuario);
			_turnoRepositoryWrite.Update(turno);
			return await Task.Factory.StartNew(() => turno.ProjectedAsDto(_habilitacao.ObterSistemaPorCnpjEmpresa()));
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			throw;
		}
		finally
		{
			log.GravarLog("Finalizou FecharTurno");
		}
	}

	public TotalConvenioTurnoDto ObterTotalDescontoConvenioAplicadoNoTurno(string idTurno)
	{
		Turno byId = _turnoRepositoryRead.GetById(ObjectId.Parse(idTurno));
		DateTime dataFinal = byId.TurnoFechamento?.DataHora ?? DateTime.Now;
		IEnumerable<ObjectId> fieldData = from x in _turnoLancamentoRepositoryRead.Find(TurnoLancamentoSpecification.PorUsuario(byId.UsuarioReferencia).PorDataInicialDataFinal(byId.TurnoAbertura.DataHora, dataFinal))
			where x.Is<TurnoMovimentacao>()
			where !x.Cancelado
			select x.OrigemReferencia;
		List<Movimentacao> source = (from x in _movimentacaoRepositoryRead.GetIn((Movimentacao x) => x.Id, fieldData).ToListEmptyWhenNull()
			where x is IMovimentacaoConvenio
			where x.To<IMovimentacaoConvenio>().Convenio != null && x.Situacao.Codigo != 4
			select x).ToListEmptyWhenNull();
		decimal num = source.Sum((Movimentacao x) => x.TotalProdutos);
		decimal num2 = source.Sum((Movimentacao x) => x.TotalServicos);
		decimal totalDescontoConvenio = source.Sum((Movimentacao x) => x.TotalDescontoAplicado);
		decimal totalLiquido = source.Sum((Movimentacao x) => x.Total);
		return new TotalConvenioTurnoDto
		{
			TotalBruto = num + num2,
			TotalDescontoConvenio = totalDescontoConvenio,
			TotalLiquido = totalLiquido
		};
	}

	public TotalCanceladasTurnoDto ObterTotalCancelamentosNoTurno(string idTurno)
	{
		Turno byId = _turnoRepositoryRead.GetById(ObjectId.Parse(idTurno));
		DateTime dataFinal = byId.TurnoFechamento?.DataHora ?? DateTime.Now;
		IEnumerable<ObjectId> fieldData = from x in _turnoLancamentoRepositoryRead.Find(TurnoLancamentoSpecification.PorUsuario(byId.UsuarioReferencia).PorDataInicialDataFinal(byId.TurnoAbertura.DataHora, dataFinal))
			where x.Is<TurnoMovimentacao>()
			where x.Cancelado
			select x.OrigemReferencia;
		List<Movimentacao> list = (from x in _movimentacaoRepositoryRead.GetIn((Movimentacao x) => x.Id, fieldData).ToListEmptyWhenNull()
			where x.Situacao.Codigo == 4
			select x).ToListEmptyWhenNull();
		decimal num = list.Sum((Movimentacao x) => x.TotalProdutos);
		decimal num2 = list.Sum((Movimentacao x) => x.TotalServicos);
		decimal totalLiquido = list.Sum((Movimentacao x) => x.Total);
		return new TotalCanceladasTurnoDto
		{
			Quantidade = list.Count,
			TotalBruto = num + num2,
			TotalLiquido = totalLiquido
		};
	}

	public async Task<IEnumerable<TurnoDto>> ObterTurnosPorPeriodoAsync(DateTime dataInicialFechamento, DateTime dataFinalFechamento, IUsuarioIdentity usuarioIdentity)
	{
		return await Task.Factory.StartNew(() => _turnoRepositoryRead.Find(TurnoSpecification.PorEmpresa(usuarioIdentity.IdEmpresa.TryParseReferencia()).PorPeriodoFechamento(dataInicialFechamento, dataFinalFechamento.ToFinalDia()).AtivoPorSituacao(SituacaoTurno.Fechado)).ToListEmptyWhenNull().ProjetedAsCollectionDto(_habilitacao.ObterSistemaPorCnpjEmpresa()));
	}

	private async Task<PaginaEntityDto> Pesquisar(ISpecification<Turno> specification, string informacao, short pagina = 1, short tamanho = 30)
	{
		IPage<Turno> page = await _turnoRepositoryRead.Find(specification, informacao, pagina, tamanho, SortType.Descending, (Turno turno) => turno.TurnoAbertura.DataHora);
		return page.ProjectedAsDto(page.Lista.ProjetedAsCollectionDto(_habilitacao.ObterSistemaPorCnpjEmpresa()));
	}

	public async Task<PaginaEntityDto> PesquisarAtivos(SituacaoTurno situacao, IUsuarioIdentity usuarioIdentity, string informacao, short pagina = 1, short tamanho = 30)
	{
		return await Pesquisar(TurnoSpecification.AtivoPorSituacao(situacao), informacao, pagina, tamanho);
	}

	public async Task<PaginaEntityDto> PesquisarInativos(SituacaoTurno situacao, IUsuarioIdentity usuarioIdentity, string informacao, short pagina = 1, short tamanho = 30)
	{
		return await Pesquisar(TurnoSpecification.InativoPorSituacao(situacao), informacao, pagina, tamanho);
	}

	public void AdicionarLancamento(TurnoLancamentoDto turnoLancamentoDto, IUsuarioIdentity usuarioIdentity)
	{
		UsuarioBase usuarioBase = _usuarioRepositoryRead.PorNome(usuarioIdentity.NomeUsuario);
		TurnoLancamento entity = turnoLancamentoDto.Morfar(ObjectId.Parse(usuarioIdentity.IdEmpresa), usuarioBase.Id);
		_turnoLancamentoRepositoryWrite.Add(entity);
	}

	public void AtualizarLancamentos(IEnumerable<TurnoLancamentoDto> turnoMovimentacoesDto, IUsuarioIdentity usuarioIdentity)
	{
		if (!turnoMovimentacoesDto.Any())
		{
			return;
		}
		UsuarioBase usuarioBase = _usuarioRepositoryRead.PorNome(usuarioIdentity.NomeUsuario);
		foreach (TurnoLancamento item in turnoMovimentacoesDto.Where((TurnoLancamentoDto x) => x.OrigemLancamento.Codigo.In(4, 5)).MorfarCollection(ObjectId.Parse(usuarioIdentity.IdEmpresa), usuarioBase.Id).ToList())
		{
			if (string.IsNullOrWhiteSpace(item.Id.ToString()))
			{
				_turnoLancamentoRepositoryWrite.Add(item);
			}
			else
			{
				_turnoLancamentoRepositoryWrite.Update(item);
			}
		}
	}

	public async Task<IEnumerable<TurnoLancamentoDto>> ObterLancamentosDoTurno(TurnoDto dto, IUsuarioIdentity usuarioIdentity)
	{
		AssertionConcern.AssertArgumentNotNull(dto, typeof(TurnoDto).Name);
		AssertionConcern.AssertArgumentFalse(dto.IsTransient(), string.Format(Messages.exception_Invalido, typeof(TurnoDto).Name));
		ObjectId id = _usuarioRepositoryRead.PorNome(usuarioIdentity.NomeUsuario).Id;
		Turno byId = _turnoRepositoryRead.GetById(ObjectId.Parse(dto.Id));
		if (byId.UsuarioReferencia != id)
		{
			throw new InvalidOperationException("Informações disponíveis apenas para o usuário do turno logado.");
		}
		DateTime dataFinal = byId.TurnoFechamento?.DataHora ?? DateTime.Now;
		return (await _turnoLancamentoRepositoryRead.Find(TurnoLancamentoSpecification.PorUsuario(id).PorDataInicialDataFinal(dto.DataHoraAbertura, dataFinal).PorTipoSangriaOuSuprimento(), string.Empty, 1, int.MaxValue, SortType.Ascending, (TurnoLancamento x) => x.DataHora)).Lista.ProjetedAsCollectionDto();
	}

	private Turno TurnoAbertoDoUsuario(IUsuarioIdentity usuarioIdentity)
	{
		ObjectId id = _usuarioRepositoryRead.PorNome(usuarioIdentity.NomeUsuario).Id;
		return _turnoRepositoryRead.FindOne(TurnoSpecification.PorEmpresa(ObjectId.Parse(usuarioIdentity.IdEmpresa)).AtivoPorSituacao(SituacaoTurno.Aberto).PorUsuario(id));
	}

	public async Task<TurnoDto> ObterTurnoAbertoDoUsuario(IUsuarioIdentity usuarioIdentity)
	{
		return await Task.Factory.StartNew(() => TurnoAbertoDoUsuario(usuarioIdentity).ProjectedAsDto(_habilitacao.ObterSistemaPorCnpjEmpresa()));
	}

	public async Task<TurnoDto> ConferirTurno(TurnoDto turnoDto, IUsuarioIdentity usuarioIdentity)
	{
		AssertionConcern.AssertArgumentNotNull(turnoDto, typeof(TurnoDto).Name);
		AssertionConcern.AssertArgumentFalse(turnoDto.IsTransient(), string.Format(Messages.exception_Invalido, typeof(TurnoDto).Name));
		Turno turno = _turnoRepositoryRead.GetById(turnoDto.Id.TryParseReferencia());
		turnoDto.Conferencia.DataHoraFechamento = turnoDto.Conferencia.DataHoraFechamento;
		turno.ConferirTurno(FluentFactory<TurnoFechamentoBuilder>.Init().SetarDataHora(turnoDto.Conferencia.DataHoraFechamento).SetarDinheiro(turnoDto.Conferencia.TotalEspecie)
			.SetarCartao(turnoDto.Conferencia.TotalCartao)
			.SetarCheque(turnoDto.Conferencia.TotalCheque)
			.SetarVale(turnoDto.Conferencia.TotalVale)
			.SetarDepositoBancario(turnoDto.Conferencia.TotalDepositoBancario)
			.SetarTotalTed(turnoDto.Conferencia.TotalTed)
			.SetarTotalDoc(turnoDto.Conferencia.TotalDoc)
			.SetarTotalValePresente(turnoDto.Conferencia.TotalValePresente)
			.SetarTotalOutrasEspecies(turnoDto.Conferencia.TotalOutrasEspecies)
			.SetarObservacao(turnoDto.Conferencia.Observacao)
			.SetarTotalCarteirasDigitais(turnoDto.Conferencia.TotalCarteirasDigitais)
			.SetarTotalPrazo(turnoDto.Conferencia.TotalPrazo)
			.SetarTotalCreditoLoja(turnoDto.Conferencia.TotalCreditoLoja)
			.Criar(), usuarioIdentity.NomeUsuario);
		_turnoRepositoryWrite.Update(turno);
		return await Task.Factory.StartNew(() => turno.ProjectedAsDto(_habilitacao.ObterSistemaPorCnpjEmpresa()));
	}
}
