using System;
using System.Collections.Generic;
using System.Linq;

namespace DigisatServer.Application.Main.Dto.Auxiliares.Turnos;

public class FechamentoDtoNull : FechamentoDto
{
	public override decimal TotalCarteirasDigitais => 0m;

	public override decimal TotalValePresente => 0m;

	public override decimal TotalEspecie => 0m;

	public override decimal TotalCheque => 0m;

	public override decimal TotalCartao => 0m;

	public override decimal TotalVale => 0m;

	public override decimal TotalDepositoBancario => 0m;

	public override decimal TotalTed => 0m;

	public override decimal TotalDoc => 0m;

	public override decimal TotalOutrasEspecies => 0m;

	public override decimal Total => 0m;

	public override decimal TotalCreditoLoja => 0m;

	public override decimal TotalPrazo => 0m;

	public override DateTime DataHoraFechamento => DateTime.MinValue;

	public override string Observacao => string.Empty ?? throw new InvalidOperationException("[NullGuard] Return value of property 'System.String DigisatServer.Application.Main.Dto.Auxiliares.Turnos.FechamentoDtoNull::Observacao()' is null.");

	public override IEnumerable<EstoqueFechamentoDto> EstoquesFechamento => Enumerable.Empty<EstoqueFechamentoDtoNull>() ?? throw new InvalidOperationException("[NullGuard] Return value of property 'System.Collections.Generic.IEnumerable`1<DigisatServer.Application.Main.Dto.Auxiliares.Turnos.EstoqueFechamentoDto> DigisatServer.Application.Main.Dto.Auxiliares.Turnos.FechamentoDtoNull::EstoquesFechamento()' is null.");
}
