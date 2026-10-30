using System;
using DigisatServer.Infrastructure.Crosscutting.Interfaces;
using NLog;
using Topshelf;

namespace Sincronizador
{
	// Token: 0x02000006 RID: 6
	public class SincronizacaoService : ServiceControl
	{
		// Token: 0x06000022 RID: 34 RVA: 0x00002A6E File Offset: 0x00000C6E
		public SincronizacaoService(IClientActorSystem actorSystem, Logger logger)
		{
			this._actorSystem = actorSystem as SincronizacaoClientActorSystem;
			this._logger = logger;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002A8C File Offset: 0x00000C8C
		public bool Start(HostControl hostControl)
		{
			try
			{
				this._actorSystem.Iniciar();
			}
			catch (Exception ex)
			{
				this._logger.Error<Exception>(ex);
			}
			return true;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002AC8 File Offset: 0x00000CC8
		public bool Stop(HostControl hostControl)
		{
			this._actorSystem.Finalizar();
			return true;
		}

		// Token: 0x04000009 RID: 9
		private readonly SincronizacaoClientActorSystem _actorSystem;

		// Token: 0x0400000A RID: 10
		private readonly Logger _logger;
	}
}
