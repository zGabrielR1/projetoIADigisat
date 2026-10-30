using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Threading;
using Akka.Actor;
using DigisatServer.Infrastructure.Crosscutting.Dto;
using DigisatServer.Infrastructure.Crosscutting.Helper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace DigisatServer.Infrastructure.Data.Core
{
	// Token: 0x02000003 RID: 3
	public class CollectionReplicator
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002188 File Offset: 0x00000388
		private CollectionReplicator(string sourceId, IObservable<MongoOperation> source, IMongoCollection<BsonDocument> collectionLastTs, ObjectId globalTsId)
		{
			this._sourceId = sourceId;
			this._source = source;
			this._collectionLastTs = collectionLastTs;
			this._globalTsId = globalTsId;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021B0 File Offset: 0x000003B0
		public static CollectionReplicator From(string sourceId, OpLogTail tail, string sourceDatabaseName, string sourceCollectionName)
		{
			if (string.IsNullOrEmpty(sourceId))
			{
				throw new SourceIdIsNullOrEmptyException();
			}
			if (tail == null)
			{
				throw new OpLogTailIsNullException();
			}
			return new CollectionReplicator(sourceId, tail.Where<MongoOperation>((MongoOperation o) => o.DatabaseAffected == sourceDatabaseName && o.CollectionAffected == sourceCollectionName), tail.CollectionLastTs, tail.GlobalTsId);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000220C File Offset: 0x0000040C
		public CollectionReplicator To(IMongoCollection<BsonDocument> target, string targetId = null)
		{
			if (target == null)
			{
				return this;
			}
			FilterDefinitionBuilder<BsonDocument> builder = Builders<BsonDocument>.Filter;
			this._source.Subscribe<MongoOperation>(delegate(MongoOperation o)
			{
				if (o.OperationType == Operations.Insert || o.OperationType == Operations.Update)
				{
					BsonValue bsonValue = ((o.OperationType == Operations.Insert) ? o.Data["_id"] : o.Source["o2"]["_id"]);
					BsonDocument asBsonDocument = o.Data.DeepClone().AsBsonDocument;
					if (targetId != null && asBsonDocument.Contains("__sourceid__") && asBsonDocument["__sourceid__"].ToString() == targetId)
					{
						return;
					}
					asBsonDocument["__sourceid__"] = this._sourceId;
					target.ReplaceOne(builder.Eq<BsonValue>("_id", bsonValue), asBsonDocument, new UpdateOptions
					{
						IsUpsert = true
					}, default(CancellationToken));
				}
				else if (o.OperationType == Operations.Delete)
				{
					target.DeleteOne(builder.Eq<BsonValue>("_id", o.Source["o2"]["_id"]), default(CancellationToken));
				}
				this.UpdateLastTs(o.Source);
			});
			return this;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002264 File Offset: 0x00000464
		public CollectionReplicator To(IEnumerable<ICanTell> target, IActorRef actor, string targetId = null, bool updateLastTs = true)
		{
			if (target == null)
			{
				return this;
			}
			this._source.Subscribe<MongoOperation>(delegate(MongoOperation o)
			{
				if (o.OperationType == Operations.Insert || o.OperationType == Operations.Update)
				{
					BsonValue bsonValue = ((o.OperationType == Operations.Insert) ? o.Data["_id"] : o.Source["o2"]["_id"]);
					BsonDocument asBsonDocument = o.Data.DeepClone().AsBsonDocument;
					if (targetId != null && asBsonDocument.Contains("__sourceid__") && asBsonDocument["__sourceid__"].ToString() == targetId)
					{
						return;
					}
					if (!asBsonDocument.Contains("$set"))
					{
						asBsonDocument["__sourceid__"] = this._sourceId;
					}
					foreach (ICanTell canTell in target)
					{
						canTell.Tell(new OpLogInsertRequest(bsonValue.ToString(), o.CollectionAffected, asBsonDocument.ToJson(null, null, null, default(BsonSerializationArgs))), actor);
						if (o.CollectionAffected.Equals("ProdutosServicos"))
						{
							CollectionReplicator <>4__this = this;
							string text = "Enviou produto para o client {0}";
							IActorRef actorRef = canTell.To<IActorRef>();
							<>4__this.GravarLog(string.Format(text, (actorRef != null) ? actorRef.Path.Name : null));
						}
					}
				}
				if (updateLastTs)
				{
					this.UpdateLastTs(o.Source);
				}
			});
			return this;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000022C0 File Offset: 0x000004C0
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

		// Token: 0x0600000C RID: 12 RVA: 0x0000231C File Offset: 0x0000051C
		private void UpdateLastTs(BsonDocument source)
		{
			this._collectionLastTs.ReplaceOne(Builders<BsonDocument>.Filter.Eq<ObjectId>("_id", this._globalTsId), new BsonDocument
			{
				{ "_id", this._globalTsId },
				{
					"ts",
					source["ts"]
				}
			}, new UpdateOptions
			{
				IsUpsert = true
			}, default(CancellationToken));
		}

		// Token: 0x04000003 RID: 3
		private readonly string _sourceId;

		// Token: 0x04000004 RID: 4
		private readonly IObservable<MongoOperation> _source;

		// Token: 0x04000005 RID: 5
		private readonly IMongoCollection<BsonDocument> _collectionLastTs;

		// Token: 0x04000006 RID: 6
		private readonly ObjectId _globalTsId;
	}
}
