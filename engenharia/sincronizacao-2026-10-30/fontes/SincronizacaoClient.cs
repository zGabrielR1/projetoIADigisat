using System;
using Akka.Actor;

namespace DigisatServer.Distributed.Akka.Main.Actors
{
	// Token: 0x0200000F RID: 15
	public class SincronizacaoClient
	{
		// Token: 0x0600004A RID: 74 RVA: 0x00004421 File Offset: 0x00002621
		public SincronizacaoClient(IActorRef actorRef)
		{
			this.ActorRef = actorRef;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00004430 File Offset: 0x00002630
		public IActorRef ActorRef { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00004438 File Offset: 0x00002638
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00004440 File Offset: 0x00002640
		public DateTime DataHoraOffline { get; protected set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600004E RID: 78 RVA: 0x0000444C File Offset: 0x0000264C
		public bool OfflineAMaisDeUmDia
		{
			get
			{
				return this.DataHoraOffline > DateTime.MinValue && this.DataHoraOffline < DateTime.Now.AddDays(-1.0);
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004490 File Offset: 0x00002690
		public void SetarOffline()
		{
			this.DataHoraOffline = DateTime.Now.AddMinutes(-5.0);
		}
	}
}
