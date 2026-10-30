using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Util;
using DigisatServer.Application.Core.Contracts.AuxiliaresModulo;
using DigisatServer.Infrastructure.Crosscutting.Dto;
using DigisatServer.Infrastructure.Crosscutting.Event;
using DigisatServer.Infrastructure.Crosscutting.Helper;
using DigisatServer.Infrastructure.Data.Core;
using MongoDB.Bson;

namespace DigisatServer.Distributed.Akka.Main.Actors
{
	// Token: 0x02000010 RID: 16
	public class SincronizacaoServerActor : ReceiveActor
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000050 RID: 80 RVA: 0x000044BC File Offset: 0x000026BC
		private IEnumerable<string> ColecoesASincronizar
		{
			get
			{
				return new List<string>
				{
					"Usuarios", "Perfis", "Configuracoes", "Pessoas", "UnidadesMedida", "ProdutosServicos", "ProdutosServicosEmpresa", "Precos", "Promocoes", "TabelasPreco",
					"Estoques", "Kits", "Grades", "LinhasColunas", "TributacoesEstadual", "TributacoesMunicipal", "TributacoesFederal", "TributacoesIPI", "TributacoesPISCOFINS", "Grupos",
					"SubGrupos", "Contas", "PlanosConta", "CentrosCusto", "OperacoesFiscais", "FormasPagamento", "Cardapios", "Impressoras", "OrdensCardapio", "ObservacoesRestaurante",
					"SaldosIcmsStRetido", "RelacoesEspecieConta", "PromocoesScanntech", "Moedas", "MenusRapido", "GruposMenuRapido", "OrdensMenuRapido", "PredefinicoesMenuRapido", "TributacoesIbsCbs", "TributacoesIs"
				};
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00004688 File Offset: 0x00002888
		public SincronizacaoServerActor(RepositorySincronizacao repositorySincronizacao, ISincronizacaoAppService sincronizacaoAppService, IEventPublisher eventPublisher)
		{
			this._repositorySincronizacao = repositorySincronizacao;
			this._sincronizacaoAppService = sincronizacaoAppService;
			eventPublisher.GetEvent<SincronizarClientes>().Subscribe<SincronizarClientes>(delegate(SincronizarClientes clientes)
			{
				if (!clientes.DarCarga)
				{
					return;
				}
				foreach (SincronizacaoClient sincronizacaoClient in ((IEnumerable<SincronizacaoClient>)this._clients))
				{
					sincronizacaoClient.ActorRef.Tell(new RestaurouBackupRequest());
				}
				this._clients.Clear();
			});
			eventPublisher.GetEvent<AntesRestaurarBaseDadosEvent>().Subscribe<AntesRestaurarBaseDadosEvent>(delegate(AntesRestaurarBaseDadosEvent x)
			{
				OpLogTail tail = this._tail;
				if (tail != null)
				{
					tail.Dispose();
				}
				foreach (SincronizacaoClient sincronizacaoClient2 in ((IEnumerable<SincronizacaoClient>)this._clients))
				{
					sincronizacaoClient2.ActorRef.Tell(x);
				}
			});
			this.Running();
			(from x in Observable.Interval(TimeSpan.FromMinutes(30.0))
				select this._clients.Where<SincronizacaoClient>((SincronizacaoClient z) => z.OfflineAMaisDeUmDia).ToListEmptyWhenNull<SincronizacaoClient>()).Subscribe<List<SincronizacaoClient>>(delegate(List<SincronizacaoClient> clients)
			{
				foreach (SincronizacaoClient sincronizacaoClient3 in clients)
				{
					this._clients.TryRemove(sincronizacaoClient3);
				}
			});
			(from x in Observable.Interval(TimeSpan.FromMinutes(1.0))
				select this._clients.ToListEmptyWhenNull<SincronizacaoClient>()).Subscribe<List<SincronizacaoClient>>(delegate(List<SincronizacaoClient> clients)
			{
				foreach (SincronizacaoClient sincronizacaoClient4 in clients)
				{
					this.GravarLog(string.Format("Cliente conectado de dentro do observable {0} - {1}", sincronizacaoClient4.ActorRef.Path.Name, DateTime.Now));
				}
			});
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004758 File Offset: 0x00002958
		private void InserirClient(IActorRef actorRef)
		{
			SincronizacaoClient sincronizacaoClient = this._clients.FirstOrDefault<SincronizacaoClient>((SincronizacaoClient x) => x.ActorRef.Path.Name.Equals(actorRef.Path.Name));
			if (sincronizacaoClient.IsNotNull())
			{
				this.GravarLog(string.Format("Removeu cliente {0} - {1}", actorRef.Path.Name, DateTime.Now));
				this._clients.TryRemove(sincronizacaoClient);
			}
			this._clients.TryAdd(new SincronizacaoClient(actorRef));
			this.GravarLog(string.Format("Conectou cliente {0} - {1}", actorRef.Path.Name, DateTime.Now));
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000480C File Offset: 0x00002A0C
		public void GravarLog(string informacao)
		{
			string text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogSincronizador.txt");
			if (!File.Exists(text))
			{
				return;
			}
			using (StreamWriter streamWriter = new StreamWriter(text, true))
			{
				streamWriter.WriteLine(informacao);
				streamWriter.Close();
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00004868 File Offset: 0x00002A68
		private void Running()
		{
			if (Environment.GetCommandLineArgs().Any<string>((string x) => x.Contains("Conversor")))
			{
				return;
			}
			this._tail = OpLog.LoadFrom(this._repositorySincronizacao.Database).Tail(Operations.InsertUpdateAndDelete);
			foreach (string text in this.ColecoesASincronizar)
			{
				CollectionReplicator.From(string.Format("from_{0}_server", text), this._tail, this._repositorySincronizacao.Database.DatabaseNamespace.DatabaseName, text).To(this._clients.Select<SincronizacaoClient, IActorRef>((SincronizacaoClient x) => x.ActorRef), base.Self, string.Format("from_{0}_client", text), true);
			}
			this._tail.Run();
			base.Receive<SincronizacaoConnectRequest>(delegate(SincronizacaoConnectRequest message)
			{
				this.InserirClient(base.Sender);
				UntypedActor.Context.Watch(base.Sender);
				base.Sender.Tell(new SincronizacaoConnectResponse(), base.Self);
			}, null);
			base.Receive<Terminated>(delegate(Terminated terminated)
			{
				SincronizacaoClient sincronizacaoClient = this._clients.FirstOrDefault<SincronizacaoClient>((SincronizacaoClient x) => x.ActorRef.Path.Name.Equals(terminated.ActorRef.Path.Name));
				if (sincronizacaoClient != null)
				{
					sincronizacaoClient.SetarOffline();
				}
				this.GravarLog(string.Format("Setou client offline {0}", (sincronizacaoClient != null) ? sincronizacaoClient.ActorRef.Path.Name : null));
			}, null);
			base.Receive<SincronizacaoReConnectRequest>(delegate(SincronizacaoReConnectRequest message)
			{
				SincronizacaoClient sincronizacaoClient2 = this._clients.FirstOrDefault<SincronizacaoClient>((SincronizacaoClient x) => x.ActorRef.Path.Name.Equals(base.Sender.Path.Name));
				DateTime dateTime = ((sincronizacaoClient2 != null) ? sincronizacaoClient2.DataHoraOffline : DateTime.MinValue);
				this.InserirClient(base.Sender);
				UntypedActor.Context.Watch(base.Sender);
				base.Sender.Tell(new SincronizacaoConnectResponse(), base.Self);
				if (dateTime == DateTime.MinValue)
				{
					this.EnviarCarga(base.Sender);
					return;
				}
				OpLogTail tailReconnect = OpLog.LoadFrom(this._repositorySincronizacao.Database).Tail(Operations.InsertUpdateAndDelete);
				foreach (string text2 in this.ColecoesASincronizar)
				{
					CollectionReplicator.From(string.Format("from_{0}_server", text2), tailReconnect, this._repositorySincronizacao.Database.DatabaseNamespace.DatabaseName, text2).To(new List<ICanTell> { base.Sender }, base.Self, string.Format("from_{0}_client", text2), false);
				}
				tailReconnect.RunBetween(dateTime, DateTime.Now).ContinueWith(delegate(Task x)
				{
					tailReconnect.Dispose();
				});
			}, null);
			base.Receive<SincronizacaoDataRequest>(delegate(SincronizacaoDataRequest message)
			{
				this.EnviarCarga(base.Sender);
			}, null);
			base.ReceiveAsync<OpLogInsertRequest>((OpLogInsertRequest request) => this._sincronizacaoAppService.ReplaceOne(request.CollectionAffected, request.Id, request.Data), null);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000049D0 File Offset: 0x00002BD0
		private async void EnviarCarga(IActorRef sender)
		{
			int numeroRegistrosEnviarPorPagina = this.GetNumeroRegistros();
			this.GravarLog(string.Format("Carga completa: enviando {0} registros por página. {1}", numeroRegistrosEnviarPorPagina, DateTime.Now));
			try
			{
				using (IEnumerator<string> enumerator = this.ColecoesASincronizar.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string colecao = enumerator.Current;
						await Task.Run(delegate
						{
							ObjectId? objectId = null;
							bool flag = true;
							for (;;)
							{
								List<string> list = this._repositorySincronizacao.Get(colecao, objectId, numeroRegistrosEnviarPorPagina).ToList<string>();
								if (!list.Any<string>())
								{
									break;
								}
								sender.Tell(new SincronizacaoDataResponse(colecao, flag, list));
								flag = false;
								objectId = new ObjectId?(BsonDocument.Parse(list.Last<string>())["_id"].AsObjectId);
							}
						}).ContinueWith(delegate(Task task)
						{
							if (task.IsFaulted)
							{
								SincronizacaoServerActor <>4__this = this;
								string text = "{0}: {1}\n{2}\n ---";
								object colecao2 = colecao;
								AggregateException exception = task.Exception;
								object obj = ((exception != null) ? exception.GetBaseException().Message : null);
								AggregateException exception2 = task.Exception;
								<>4__this.GravarLog(string.Format(text, colecao2, obj, (exception2 != null) ? exception2.StackTrace : null));
							}
						});
					}
				}
				IEnumerator<string> enumerator = null;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				throw;
			}
			finally
			{
				this.GravarLog(string.Format("Terminou em {0}---", DateTime.Now));
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004A14 File Offset: 0x00002C14
		private int GetNumeroRegistros()
		{
			int num2;
			try
			{
				int num = Convert.ToInt32(Environment.GetEnvironmentVariable("SINCRONIZADOR_NUMERO_REGISTROS", EnvironmentVariableTarget.Machine));
				num2 = ((num <= 0) ? 100 : num);
			}
			catch (Exception)
			{
				num2 = 100;
			}
			return num2;
		}

		// Token: 0x04000026 RID: 38
		private readonly ConcurrentSet<SincronizacaoClient> _clients = new ConcurrentSet<SincronizacaoClient>();

		// Token: 0x04000027 RID: 39
		private readonly RepositorySincronizacao _repositorySincronizacao;

		// Token: 0x04000028 RID: 40
		private readonly ISincronizacaoAppService _sincronizacaoAppService;

		// Token: 0x04000029 RID: 41
		private OpLogTail _tail;
	}
}
