using System;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.DI.SimpleInjector;
using DigisatServer.Infrastructure.Crosscutting.Dto;
using DigisatServer.Infrastructure.Crosscutting.Helper;
using DigisatServer.Infrastructure.Crosscutting.Interfaces;
using DigisatServer.Infrastructure.Crosscutting.NetFramework.Actor;
using SimpleInjector;

namespace Sincronizador
{
	// Token: 0x02000002 RID: 2
	public class SincronizacaoClientActorSystem : DClientActorSystemBase<IServiceProvider, SimpleInjectorDependencyResolver>
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public SincronizacaoClientActorSystem(Container container)
			: base(container)
		{
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002059 File Offset: 0x00000259
		public override IActorSystem Criar(string sistema)
		{
			this.System = ActorSystem.Create(sistema, base.ObterConfiguracao());
			this.System.WhenTerminated.ContinueWith(new Action<Task>(this.Reiniciar));
			return this;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000208B File Offset: 0x0000028B
		private void Reiniciar(Task task)
		{
			this.Finalizar();
			base.Reload();
			if (task.IsFaulted)
			{
				this.CriarAtor().Tell(new SincronizacaoConnectRequest());
				return;
			}
			this.CriarAtor().Tell(new SincronizacaoReConnectRequest());
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020C4 File Offset: 0x000002C4
		public void Iniciar()
		{
			this._nome = Guid.NewGuid().ToString().RemoverPontoBarraHifen();
			this.CriarAtor().Tell(new SincronizacaoConnectRequest());
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020FF File Offset: 0x000002FF
		private IActorRef CriarAtor()
		{
			this._actorRef = base.Criar<SincronizacaoClientActor>(this._nome);
			return this._actorRef;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002119 File Offset: 0x00000319
		public void Finalizar()
		{
			base.Finalizar(this._actorRef);
		}

		// Token: 0x04000001 RID: 1
		private IActorRef _actorRef;

		// Token: 0x04000002 RID: 2
		private string _nome;
	}
}
