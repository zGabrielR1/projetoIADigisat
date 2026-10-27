using System.Collections.Generic;
using System.Linq;
using DigisatServer.Application.Core.Projections.CadastrosProjection.Usuarios;
using DigisatServer.Application.Main.Dto.Auxiliares.Turnos;
using DigisatServer.Application.Main.Dto.Mobiles;
using DigisatServer.Domain.Core;
using DigisatServer.Domain.Main.Aggregates.AuxiliaresAgg.Turnos;
using DigisatServer.Infrastructure.Crosscutting.Helper;
using DigisatServer.Infrastructure.Crosscutting.Model.Sistemas;

namespace DigisatServer.Application.Core.Projections.AuxiliaresProjection.Turnos;

public static class TurnoProjection
{
	public static TurnoDto ProjectedAsDto(this Turno turno, Sistema sistema)
	{
		if (turno.IsNull())
		{
			return new TurnoDtoNull();
		}
		return (TurnoDto)new TurnoDto
		{
			SaldoInicial = turno.TurnoAbertura.SaldoInicial,
			Observacao = turno.TurnoAbertura.Observacao.ToStringEmptyWhenNull(),
			DataHoraAbertura = turno.TurnoAbertura.DataHora,
			TurnoFechado = turno.TurnoFechado,
			TurnoConferido = turno.TurnoConferido,
			Usuario = turno.Usuario.ProjectedAsDto(sistema),
			Fechamento = turno.TurnoFechamento.ProjectedAsDto(),
			Conferencia = turno.TurnoConferencia.ProjectedAsDto(),
			IsMobile = turno.TurnoAbertura.IsMobile,
			Historicos = turno.ObterHistoricos()
		}.ProjectedAsDto(turno);
	}

	public static IEnumerable<TurnoDto> ProjetedAsCollectionDto(this IEnumerable<Turno> turnos, Sistema sistema)
	{
		List<Turno> source = turnos.ToListEmptyWhenNull();
		if (!source.Any())
		{
			return Enumerable.Empty<TurnoDtoNull>();
		}
		return source.Select((Turno x) => x.ProjectedAsDto(sistema));
	}

	public static FechamentoDto ProjectedAsDto(this TurnoFechamento fechamento)
	{
		if (fechamento.IsNull())
		{
			return new FechamentoDtoNull();
		}
		return new FechamentoDto
		{
			DataHoraFechamento = fechamento.DataHora,
			Observacao = fechamento.Observacao.ToStringEmptyWhenNull(),
			TotalCartao = fechamento.Cartao,
			TotalCheque = fechamento.Cheque,
			TotalEspecie = fechamento.Dinheiro,
			TotalVale = fechamento.Vale,
			TotalDepositoBancario = fechamento.DepositoBancario,
			TotalTed = fechamento.Ted,
			TotalDoc = fechamento.Doc,
			TotalValePresente = fechamento.ValePresente,
			TotalOutrasEspecies = fechamento.OutrasEspecies,
			TotalCarteirasDigitais = fechamento.CarteirasDigitais,
			TotalPrazo = fechamento.Prazo,
			TotalCreditoLoja = fechamento.CreditoLoja,
			EstoquesFechamento = (fechamento.EstoquesFechamento?.ProjetedAsCollectionDto() ?? Enumerable.Empty<EstoqueFechamentoDtoNull>())
		};
	}

	public static EstoqueFechamentoDto ProjectedAsDto(this EstoqueFechamento estoqueFechamento)
	{
		if (estoqueFechamento.IsNull())
		{
			return new EstoqueFechamentoDtoNull();
		}
		return new EstoqueFechamentoDto
		{
			ProdutoServicoReferencia = estoqueFechamento.ProdutoServicoReferencia.ToStringEmptyWhenDefault(),
			Descricao = estoqueFechamento.Descricao.ToStringEmptyWhenNull(),
			QuantidadeSaldo = estoqueFechamento.QuantidadeSaldo
		};
	}

	public static IEnumerable<EstoqueFechamentoDto> ProjetedAsCollectionDto(this IEnumerable<EstoqueFechamento> estoquesFechamento)
	{
		List<EstoqueFechamento> source = estoquesFechamento.ToListEmptyWhenNull();
		if (!source.Any())
		{
			return Enumerable.Empty<EstoqueFechamentoDtoNull>();
		}
		return source.Select(ProjectedAsDto);
	}

	public static TurnoMobileDto ProjectedAsMobileDto(this Turno turno)
	{
		if (turno.IsNull())
		{
			return new TurnoMobileDtoNull();
		}
		return (TurnoMobileDto)new TurnoMobileDto
		{
			SaldoInicial = turno.TurnoAbertura.SaldoInicial,
			Observacao = turno.TurnoAbertura.Observacao.ToStringEmptyWhenNull(),
			DataHoraAbertura = turno.TurnoAbertura.DataHora.ToString("dd/MM/yyyy HH:mm"),
			FechamentoMobile = turno.TurnoFechamento.ProjectedAsMobileDto()
		}.ProjectedAsDto(turno);
	}

	public static FechamentoMobileDto ProjectedAsMobileDto(this TurnoFechamento turnoFechamento)
	{
		if (turnoFechamento.IsNull())
		{
			return new FechamentoMobileDtoNull();
		}
		return new FechamentoMobileDto
		{
			DataHoraFechamento = turnoFechamento.DataHora.ToString("dd/MM/yyyy HH:mm"),
			Observacao = turnoFechamento.Observacao.ToStringEmptyWhenNull(),
			TotalCartao = turnoFechamento.Cartao,
			TotalCheque = turnoFechamento.Cheque,
			TotalEspecie = turnoFechamento.Dinheiro,
			TotalVale = turnoFechamento.Vale,
			TotalDepositoBancario = turnoFechamento.DepositoBancario,
			TotalTed = turnoFechamento.Ted,
			TotalDoc = turnoFechamento.Doc,
			TotalValePresente = turnoFechamento.ValePresente,
			TotalOutrasEspecies = turnoFechamento.OutrasEspecies,
			TotalCarteirasDigitais = turnoFechamento.CarteirasDigitais,
			TotalPrazo = turnoFechamento.Prazo,
			TotalCreditoLoja = turnoFechamento.CreditoLoja
		};
	}
}
