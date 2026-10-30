using System;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using DigisatServer.Infrastructure.Crosscutting.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DigisatServer.Infrastructure.Data.Core
{
	// Token: 0x0200000F RID: 15
	public sealed class OpLogTail : IObservable<MongoOperation>, IDisposable
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00002C9C File Offset: 0x00000E9C
		internal OpLogTail(IMongoCollection<BsonDocument> oplog, IMongoCollection<BsonDocument> collectionLastTs, Operations monitoredOperations)
		{
			this._oplog = oplog;
			this._collectionLastTs = collectionLastTs;
			this._monitoredOperations = monitoredOperations;
			this._lastTs = BsonMinKey.Value;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002CF0 File Offset: 0x00000EF0
		internal OpLogTail(IMongoCollection<BsonDocument> oplog, IMongoCollection<BsonDocument> collectionLastTs, Operations monitoredOperations, DateTime checkpointInUniversalTime)
		{
			this._oplog = oplog;
			this._collectionLastTs = collectionLastTs;
			this._monitoredOperations = monitoredOperations;
			int num = (int)checkpointInUniversalTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
			this._lastTs = new BsonTimestamp(num, 0);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002D63 File Offset: 0x00000F63
		public IDisposable Subscribe(IObserver<MongoOperation> observer)
		{
			return this._subject.Subscribe(observer);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002D71 File Offset: 0x00000F71
		public IDisposable Subscribe(Action<MongoOperation> onNext)
		{
			return this._subject.Subscribe<MongoOperation>(onNext);
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002D7F File Offset: 0x00000F7F
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002D87 File Offset: 0x00000F87
		public bool IsRunning { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002D90 File Offset: 0x00000F90
		public IMongoCollection<BsonDocument> CollectionLastTs
		{
			get
			{
				return this._collectionLastTs;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002D98 File Offset: 0x00000F98
		public ObjectId GlobalTsId
		{
			get
			{
				return ObjectId.Parse("5a17fdffc6d05a51fe7eda79");
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002DA4 File Offset: 0x00000FA4
		public void Run()
		{
			object lockObject = this._lockObject;
			lock (lockObject)
			{
				if (this.IsRunning)
				{
					return;
				}
				this.IsRunning = true;
			}
			BsonDocument bsonDocument = this._collectionLastTs.FindSync<BsonDocument>(Builders<BsonDocument>.Filter.Eq<ObjectId>("_id", this.GlobalTsId), null, default(CancellationToken)).FirstOrDefault<BsonDocument>(default(CancellationToken));
			if (bsonDocument != null)
			{
				this._lastTs = bsonDocument["ts"];
			}
			CancellationToken token = this._tokenSource.Token;
			Task.Run(async delegate
			{
				FindOptions<BsonDocument> options = new FindOptions<BsonDocument>
				{
					CursorType = CursorType.TailableAwait,
					OplogReplay = new bool?(true)
				};
				FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
				FilterDefinition<BsonDocument> filter = builder.Gte<BsonValue>("ts", this._lastTs);
				while (!token.IsCancellationRequested)
				{
					if (this._subject.HasObservers)
					{
						try
						{
							IAsyncCursor<BsonDocument> asyncCursor = await this._oplog.FindAsync<BsonDocument>(filter, options, token);
							using (IAsyncCursor<BsonDocument> cursor = asyncCursor)
							{
								await cursor.ForEachAsync<BsonDocument>(delegate(BsonDocument document)
								{
									this._lastTs = document["ts"];
									MongoOperation mongoOperation = new MongoOperation(document);
									if ((mongoOperation.OperationType & this._monitoredOperations) != Operations.None)
									{
										this._subject.OnNext(mongoOperation);
									}
								}, token);
							}
							IAsyncCursor<BsonDocument> cursor = null;
							filter = builder.Gt<BsonValue>("ts", this._lastTs);
							continue;
						}
						catch (Exception ex)
						{
							LoggerFactory.CreateLog().LogError("Erro OpLogTail > Run.", ex, Array.Empty<object>());
							continue;
						}
						break;
					}
				}
			}, token);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002E84 File Offset: 0x00001084
		public Task RunBetween(DateTime inicio, DateTime fim)
		{
			CancellationToken token = this._tokenSource.Token;
			BsonTimestamp bsonTimestamp = new BsonTimestamp((int)inicio.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds, 1);
			BsonTimestamp bsonTimestamp2 = new BsonTimestamp((int)fim.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds, 1);
			FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.And(new FilterDefinition<BsonDocument>[]
			{
				Builders<BsonDocument>.Filter.Gte<BsonTimestamp>("ts", bsonTimestamp),
				Builders<BsonDocument>.Filter.Lte<BsonTimestamp>("ts", bsonTimestamp2)
			});
			FindOptions<BsonDocument> options = new FindOptions<BsonDocument>
			{
				OplogReplay = new bool?(true)
			};
			return Task.Run(async delegate
			{
				try
				{
					IAsyncCursor<BsonDocument> asyncCursor = await this._oplog.FindAsync<BsonDocument>(filter, options, token);
					using (IAsyncCursor<BsonDocument> cursor = asyncCursor)
					{
						await cursor.ForEachAsync<BsonDocument>(delegate(BsonDocument document)
						{
							MongoOperation mongoOperation = new MongoOperation(document);
							if ((mongoOperation.OperationType & this._monitoredOperations) != Operations.None)
							{
								this._subject.OnNext(mongoOperation);
							}
						}, token);
					}
					IAsyncCursor<BsonDocument> cursor = null;
				}
				catch (Exception ex)
				{
					LoggerFactory.CreateLog().LogError("Erro OpLogTail > RunBetween.", ex, Array.Empty<object>());
				}
			}, token);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002F7B File Offset: 0x0000117B
		public void Dispose()
		{
			this._tokenSource.Cancel();
			Subject<MongoOperation> subject = this._subject;
			if (subject != null)
			{
				subject.OnCompleted();
			}
			Subject<MongoOperation> subject2 = this._subject;
			if (subject2 != null)
			{
				subject2.Dispose();
			}
			this._tokenSource.Dispose();
		}

		// Token: 0x0400001B RID: 27
		private readonly IMongoCollection<BsonDocument> _oplog;

		// Token: 0x0400001C RID: 28
		private readonly IMongoCollection<BsonDocument> _collectionLastTs;

		// Token: 0x0400001D RID: 29
		private readonly Operations _monitoredOperations;

		// Token: 0x0400001E RID: 30
		private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();

		// Token: 0x0400001F RID: 31
		private readonly object _lockObject = new object();

		// Token: 0x04000020 RID: 32
		private BsonValue _lastTs;

		// Token: 0x04000021 RID: 33
		private readonly Subject<MongoOperation> _subject = new Subject<MongoOperation>();
	}
}
