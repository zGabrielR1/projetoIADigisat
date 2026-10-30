using System;
using System.Threading.Tasks;
using DigisatServer.Application.Core.Contracts.AuxiliaresModulo;
using DigisatServer.Domain.Main.Aggregates.AuxiliaresAgg.Pbms;
using DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg;
using DigisatServer.Domain.Main.Aggregates.MovimentacoesAgg.RegistrosPafEcf;
using DigisatServer.Infrastructure.Crosscutting.Helper;
using DigisatServer.Infrastructure.Crosscutting.Logging;
using DigisatServer.Infrastructure.Crosscutting.Model.SituacoesMovimentacao;
using DigisatServer.Infrastructure.Crosscutting.NetFramework.Logging;
using DigisatServer.Infrastructure.Data.Core.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace DigisatServer.Application.Main.AuxiliaresModulo.Services
{
	// Token: 0x020001E8 RID: 488
	public class SincronizacaoAppService : ISincronizacaoAppService
	{
		// Token: 0x0600136C RID: 4972 RVA: 0x000922D2 File Offset: 0x000904D2
		public SincronizacaoAppService(IMovimentacaoRepositoryWrite movimentacaoRepositoryWrite, IMovimentacaoRepositoryRead movimentacaoRepositoryRead, IRegistroPafEcfRepositoryWrite registroPafEcfRepositoryWrite, IRepositoryWrite<TransacaoPbm> transacaoPbmRepositoryWrite, IRepositoryRead<TransacaoPbm> transacaoPbmRepositoryRead)
		{
			this._movimentacaoRepositoryWrite = movimentacaoRepositoryWrite;
			this._movimentacaoRepositoryRead = movimentacaoRepositoryRead;
			this._registroPafEcfRepositoryWrite = registroPafEcfRepositoryWrite;
			this._transacaoPbmRepositoryWrite = transacaoPbmRepositoryWrite;
			this._transacaoPbmRepositoryRead = transacaoPbmRepositoryRead;
			LoggerFactory.SetCurrent(new TraceSourceLogFactory());
		}

		// Token: 0x0600136D RID: 4973 RVA: 0x0009230C File Offset: 0x0009050C
		public Task ReplaceOne(string collectionName, string id, string data)
		{
			return Task.Factory.StartNew(delegate
			{
				try
				{
					this.ReplaceOneSync(collectionName, data);
				}
				catch (Exception ex)
				{
					LoggerFactory.CreateLog().LogError(string.Format("Falha ao Sincronizar Collection: {0} Id: {1}", collectionName, id), ex, Array.Empty<object>());
				}
			});
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x00092354 File Offset: 0x00090554
		private void ReplaceOneSync(string collectionName, string data)
		{
			if (collectionName == "RegistrosPafEcf")
			{
				this.SincronizarRegistroPafEcf(BsonSerializer.Deserialize<RegistroPafEcf>(data, null));
				return;
			}
			if (collectionName == "TransacoesPbm")
			{
				this.SincronizarTransacaoPbm(BsonSerializer.Deserialize<TransacaoPbm>(data, null));
				return;
			}
			if (collectionName == "Movimentacoes")
			{
				this.SincronizarMovimentacao(BsonSerializer.Deserialize<Movimentacao>(data, null));
				return;
			}
			if (!(collectionName == "LastTs"))
			{
				throw new Exception(string.Format("Collection '{0}' não implementada para sincronizar com o servidor.", collectionName));
			}
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x000923D2 File Offset: 0x000905D2
		private void SincronizarRegistroPafEcf(RegistroPafEcf registroPafEcf)
		{
			this._registroPafEcfRepositoryWrite.Salvar(registroPafEcf);
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x000923E0 File Offset: 0x000905E0
		private void SincronizarTransacaoPbm(TransacaoPbm transacaoPbm)
		{
			if (this._transacaoPbmRepositoryRead.GetById(transacaoPbm.Id).IsNull())
			{
				this._transacaoPbmRepositoryWrite.Update(transacaoPbm);
			}
		}

		// Token: 0x06001371 RID: 4977 RVA: 0x00092408 File Offset: 0x00090608
		private void SincronizarMovimentacao(Movimentacao movimentacaoCliente)
		{
			Movimentacao oneBy = this._movimentacaoRepositoryRead.GetOneBy<ObjectId>((Movimentacao x) => x.Id, movimentacaoCliente.Id);
			if (oneBy.IsNotNull())
			{
				if (oneBy.Situacao is Concluido && movimentacaoCliente.Situacao is Cancelado)
				{
					oneBy.Cancelar(movimentacaoCliente);
					this._movimentacaoRepositoryWrite.Update(oneBy);
					return;
				}
			}
			else
			{
				if (movimentacaoCliente.Situacao is Cancelado)
				{
					movimentacaoCliente.Cancelar(null);
				}
				else
				{
					this._movimentacaoRepositoryRead.PreencherPagamentosRecebimentos(true, new Movimentacao[] { movimentacaoCliente });
					movimentacaoCliente.Efetivar(string.Empty);
				}
				this._movimentacaoRepositoryWrite.Add(movimentacaoCliente);
			}
		}

		// Token: 0x06001372 RID: 4978 RVA: 0x000924DC File Offset: 0x000906DC
		public Task Sincronizar(string colecao, string id, string dados)
		{
			return Task.Factory.StartNew(delegate
			{
				try
				{
					this.ReplaceOneSync(colecao, dados);
				}
				catch (Exception ex)
				{
					throw new Exception(string.Format("Falha ao sincronizar coleção: {0} Id: {1}", colecao, id), ex);
				}
			});
		}

		// Token: 0x0400090B RID: 2315
		private readonly IMovimentacaoRepositoryWrite _movimentacaoRepositoryWrite;

		// Token: 0x0400090C RID: 2316
		private readonly IMovimentacaoRepositoryRead _movimentacaoRepositoryRead;

		// Token: 0x0400090D RID: 2317
		private readonly IRegistroPafEcfRepositoryWrite _registroPafEcfRepositoryWrite;

		// Token: 0x0400090E RID: 2318
		private readonly IRepositoryWrite<TransacaoPbm> _transacaoPbmRepositoryWrite;

		// Token: 0x0400090F RID: 2319
		private readonly IRepositoryRead<TransacaoPbm> _transacaoPbmRepositoryRead;
	}
}
