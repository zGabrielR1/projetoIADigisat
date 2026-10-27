using System;
using System.Linq.Expressions;
using DevExpress.Mvvm;
using DigisatServer.Application.Main.Dto.Auxiliares.Turnos;
using DigisatServer.Infrastructure.Crosscutting.Module;
using DigisatServer.Infrastructure.Crosscutting.Mvvm;
using DigisatServer.Infrastructure.Crosscutting.Mvvm.Interfaces;
using DigisatServer.Infrastructure.Crosscutting.Right;
using Otus.Infraestrutura.Models;
using Otus.Infraestrutura.ViewModels;

namespace Otus.Modulo.Turnos.ViewModels.Turnos;

public class TurnoFechamentoViewModel : ViewModelDtoBase<TurnoDto>, IViewModelPrincipal, IViewModelTitulo, IViewModelDtoBase, IViewModelBase
{
	public ContagemDeCedulasMoedas ContagemDeCedulasMoedas
	{
		get
		{
			return ((BindableBase)this).GetProperty<ContagemDeCedulasMoedas>((Expression<Func<ContagemDeCedulasMoedas>>)(() => ContagemDeCedulasMoedas));
		}
		set
		{
			((BindableBase)this).SetProperty<ContagemDeCedulasMoedas>((Expression<Func<ContagemDeCedulasMoedas>>)(() => ContagemDeCedulasMoedas), value);
		}
	}

	public decimal TotalEspecie
	{
		get
		{
			return ((BindableBase)this).GetProperty<decimal>((Expression<Func<decimal>>)(() => TotalEspecie));
		}
		set
		{
			((BindableBase)this).SetProperty<decimal>((Expression<Func<decimal>>)(() => TotalEspecie), value, (Action)CalcularTotal);
		}
	}

	public decimal TotalCheque
	{
		get
		{
			return ((BindableBase)this).GetProperty<decimal>((Expression<Func<decimal>>)(() => TotalCheque));
		}
		set
		{
			((BindableBase)this).SetProperty<decimal>((Expression<Func<decimal>>)(() => TotalCheque), value, (Action)CalcularTotal);
		}
	}

	public decimal TotalCartao
	{
		get
		{
			return ((BindableBase)this).GetProperty<decimal>((Expression<Func<decimal>>)(() => TotalCartao));
		}
		set
		{
			((BindableBase)this).SetProperty<decimal>((Expression<Func<decimal>>)(() => TotalCartao), value, (Action)CalcularTotal);
		}
	}

	public decimal TotalVale
	{
		get
		{
			return ((BindableBase)this).GetProperty<decimal>((Expression<Func<decimal>>)(() => TotalVale));
		}
		set
		{
			((BindableBase)this).SetProperty<decimal>((Expression<Func<decimal>>)(() => TotalVale), value, (Action)CalcularTotal);
		}
	}

	public decimal TotalValePresente
	{
		get
		{
			return ((BindableBase)this).GetProperty<decimal>((Expression<Func<decimal>>)(() => TotalValePresente));
		}
		set
		{
			((BindableBase)this).SetProperty<decimal>((Expression<Func<decimal>>)(() => TotalValePresente), value, (Action)CalcularTotal);
		}
	}

	public decimal TotalCreditoLoja
	{
		get
		{
			return ((BindableBase)this).GetProperty<decimal>((Expression<Func<decimal>>)(() => TotalCreditoLoja));
		}
		set
		{
			((BindableBase)this).SetProperty<decimal>((Expression<Func<decimal>>)(() => TotalCreditoLoja), value, (Action)CalcularTotal);
		}
	}

	public decimal TotalCarteirasDigitais
	{
		get
		{
			return ((BindableBase)this).GetProperty<decimal>((Expression<Func<decimal>>)(() => TotalCarteirasDigitais));
		}
		set
		{
			((BindableBase)this).SetProperty<decimal>((Expression<Func<decimal>>)(() => TotalCarteirasDigitais), value, (Action)CalcularTotal);
		}
	}

	public decimal TotalOutrasEspecies
	{
		get
		{
			return ((BindableBase)this).GetProperty<decimal>((Expression<Func<decimal>>)(() => TotalOutrasEspecies));
		}
		set
		{
			((BindableBase)this).SetProperty<decimal>((Expression<Func<decimal>>)(() => TotalOutrasEspecies), value, (Action)CalcularTotal);
		}
	}

	public decimal TotalDepositoBancario
	{
		get
		{
			return ((BindableBase)this).GetProperty<decimal>((Expression<Func<decimal>>)(() => TotalDepositoBancario));
		}
		set
		{
			((BindableBase)this).SetProperty<decimal>((Expression<Func<decimal>>)(() => TotalDepositoBancario), value, (Action)CalcularTotal);
		}
	}

	public decimal TotalTed
	{
		get
		{
			return ((BindableBase)this).GetProperty<decimal>((Expression<Func<decimal>>)(() => TotalTed));
		}
		set
		{
			((BindableBase)this).SetProperty<decimal>((Expression<Func<decimal>>)(() => TotalTed), value, (Action)CalcularTotal);
		}
	}

	public decimal TotalDoc
	{
		get
		{
			return ((BindableBase)this).GetProperty<decimal>((Expression<Func<decimal>>)(() => TotalDoc));
		}
		set
		{
			((BindableBase)this).SetProperty<decimal>((Expression<Func<decimal>>)(() => TotalDoc), value, (Action)CalcularTotal);
		}
	}

	public decimal TotalPrazo
	{
		get
		{
			return ((BindableBase)this).GetProperty<decimal>((Expression<Func<decimal>>)(() => TotalPrazo));
		}
		set
		{
			((BindableBase)this).SetProperty<decimal>((Expression<Func<decimal>>)(() => TotalPrazo), value, (Action)CalcularTotal);
		}
	}

	public bool PermitirAlterarDataHoraAberturaFechamentoTurno => DadosSistema.VerificarPermissao<Turno, PermitirAlterarDataHoraAberturaFechamentoTurno>();

	public DateTime DataHoraFechamento
	{
		get
		{
			return ((BindableBase)this).GetProperty<DateTime>((Expression<Func<DateTime>>)(() => DataHoraFechamento));
		}
		set
		{
			((BindableBase)this).SetProperty<DateTime>((Expression<Func<DateTime>>)(() => DataHoraFechamento), value, (Action)CalcularTotal);
		}
	}

	public string Observacao
	{
		get
		{
			return ((BindableBase)this).GetProperty<string>((Expression<Func<string>>)(() => Observacao));
		}
		set
		{
			((BindableBase)this).SetProperty<string>((Expression<Func<string>>)(() => Observacao), value, (Action)CalcularTotal);
		}
	}

	public decimal TotalGeral
	{
		get
		{
			return ((BindableBase)this).GetProperty<decimal>((Expression<Func<decimal>>)(() => TotalGeral));
		}
		set
		{
			((BindableBase)this).SetProperty<decimal>((Expression<Func<decimal>>)(() => TotalGeral), value);
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
		DataHoraFechamento = (base.Dto.TurnoFechado ? base.Dto.Fechamento.DataHoraFechamento : DateTime.Now);
		ContagemDeCedulasMoedas = new ContagemDeCedulasMoedas
		{
			TotalEspecie = base.Dto.Fechamento.TotalEspecie
		};
		ContagemDeCedulasMoedas.AtualizouTotalEspecieEvent += ContagemDeCedulasMoedasOnAtualizouTotalEspecieEvent;
		TotalEspecie = base.Dto.Fechamento.TotalEspecie;
		TotalCheque = base.Dto.Fechamento.TotalCheque;
		TotalCartao = base.Dto.Fechamento.TotalCartao;
		TotalVale = base.Dto.Fechamento.TotalVale;
		TotalValePresente = base.Dto.Fechamento.TotalValePresente;
		TotalCreditoLoja = base.Dto.Fechamento.TotalCreditoLoja;
		TotalCarteirasDigitais = base.Dto.Fechamento.TotalCarteirasDigitais;
		TotalOutrasEspecies = base.Dto.Fechamento.TotalOutrasEspecies;
		TotalDepositoBancario = base.Dto.Fechamento.TotalDepositoBancario;
		TotalTed = base.Dto.Fechamento.TotalTed;
		TotalDoc = base.Dto.Fechamento.TotalDoc;
		TotalPrazo = base.Dto.Fechamento.TotalPrazo;
		Observacao = base.Dto.Fechamento.Observacao;
	}

	public override void AtualizarDto<T>(ref T dto)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected Obj, but got Unknown
		base.AtualizarDto<T>(ref dto);
		object obj = dto;
		((TurnoDto)((obj is TurnoDto) ? obj : null)).Fechamento = new FechamentoDto
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
		((ViewModelValidationBase)this).Dispose();
	}

	public TurnoDto ObterDto()
	{
		return base.Dto;
	}

	public string ToTituloLista()
	{
		return string.Empty;
	}

	public IFormulario CriarFormulario()
	{
		return (IFormulario)(object)new TurnoFechamentoFormulario(base.Dto);
	}

	public string ToTituloFicha()
	{
		return string.Empty;
	}
}
