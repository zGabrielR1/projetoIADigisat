using DigisatServer.Application.Main.Dto.Auxiliares.Turnos;
using DigisatServer.Infrastructure.Crosscutting.Mvvm;
using DigisatServer.Infrastructure.Crosscutting.Mvvm.Interfaces;
using Otus.Modulo.Turnos.ViewModels.Turnos;

namespace Otus.Modulo.Turnos.ViewModels;

public class TurnoFechamentoFormulario : FormularioEntity<TurnoDto>
{
	public TurnoFechamentoFormulario(TurnoDto dto)
		: base(dto)
	{
		CriarAbas();
	}

	private void CriarAbas()
	{
		((Formulario)this).AdicionarProjetarAbas(new IAbaViewModelBase[1] { (IAbaViewModelBase)new TurnoFechamentoAbaViewModel((IViewModelPrincipal)(object)new TurnoFechamentoViewModel(base.Dto)) });
	}
}
