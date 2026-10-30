using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace DigisatServer.Infrastructure.Data.Core
{
	// Token: 0x0200000B RID: 11
	public class RepositorySincronizacao
	{
		// Token: 0x0600001C RID: 28 RVA: 0x0000255E File Offset: 0x0000075E
		public RepositorySincronizacao(string dataBaseName, int porta = 12220)
		{
			this._dataBaseName = dataBaseName;
			this._porta = porta;
			this.PreencherDataBase();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002590 File Offset: 0x00000790
		private void PreencherDataBase()
		{
			this.Database = Util<BsonDocument>.GetDatabaseFromAddress(Util<BsonDocument>.GetDefaultMongoServerAddress(this._porta), this._dataBaseName);
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000025AE File Offset: 0x000007AE
		// (set) Token: 0x0600001F RID: 31 RVA: 0x000025B6 File Offset: 0x000007B6
		public IMongoDatabase Database { get; protected set; }

		// Token: 0x06000020 RID: 32 RVA: 0x000025C0 File Offset: 0x000007C0
		public Task ReplaceOneAsync(string collectionName, string id, string data)
		{
			IMongoCollection<BsonDocument> collection = this.Database.GetCollection<BsonDocument>(collectionName, null);
			BsonDocument bsonDocument = BsonSerializer.Deserialize<BsonDocument>(data, null);
			return collection.ReplaceOneAsync(this._builder.Eq<ObjectId>("_id", ObjectId.Parse(id)), bsonDocument, new UpdateOptions
			{
				IsUpsert = true
			}, default(CancellationToken));
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002618 File Offset: 0x00000818
		public Task UpdateOneAsync(string collectionName, string id, string data)
		{
			return this.Database.GetCollection<BsonDocument>(collectionName, null).FindOneAndUpdateAsync<BsonDocument>(this._builder.Eq<ObjectId>("_id", ObjectId.Parse(id)), data, null, default(CancellationToken));
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002664 File Offset: 0x00000864
		public async Task GetOneByOne(string collectionName, Action<KeyValuePair<string, string>> action)
		{
			CancellationToken token = this._tokenSource.Token;
			IAsyncCursor<BsonDocument> asyncCursor = await this.Database.GetCollection<BsonDocument>(collectionName, null).FindAsync<BsonDocument>(FilterDefinition<BsonDocument>.Empty, null, token);
			using (IAsyncCursor<BsonDocument> cursor = asyncCursor)
			{
				await cursor.ForEachAsync<BsonDocument>(delegate(BsonDocument x)
				{
					action(new KeyValuePair<string, string>(collectionName, x.ToJson(null, null, null, default(BsonSerializationArgs))));
				}, token);
			}
			IAsyncCursor<BsonDocument> cursor = null;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000026BC File Offset: 0x000008BC
		public Task ReplaceOneAsync(string collectionName, string data)
		{
			IMongoCollection<BsonDocument> collection = this.Database.GetCollection<BsonDocument>(collectionName, null);
			BsonDocument bsonDocument = BsonSerializer.Deserialize<BsonDocument>(data, null);
			FilterDefinitionBuilder<BsonDocument> filter = Builders<BsonDocument>.Filter;
			return collection.ReplaceOneAsync(filter.Eq<BsonValue>("_id", bsonDocument["_id"]), bsonDocument, new UpdateOptions
			{
				IsUpsert = true
			}, default(CancellationToken));
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000271C File Offset: 0x0000091C
		public Task BulkWriteAsync(string collectionName, IEnumerable<string> data)
		{
			IMongoCollection<BsonDocument> collection = this.Database.GetCollection<BsonDocument>(collectionName, null);
			IEnumerable<InsertOneModel<BsonDocument>> enumerable = from x in data
				select BsonSerializer.Deserialize<BsonDocument>(x, null) into z
				select new InsertOneModel<BsonDocument>(z);
			return collection.BulkWriteAsync(new List<WriteModel<BsonDocument>>(enumerable), null, default(CancellationToken));
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002798 File Offset: 0x00000998
		public void DropAll(IEnumerable<string> collectionsName)
		{
			this.PreencherDataBase();
			foreach (string text in collectionsName)
			{
				this.Database.DropCollection(text, default(CancellationToken));
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000027F4 File Offset: 0x000009F4
		public void InsertMany(IDictionary<string, string> data)
		{
			foreach (KeyValuePair<string, string> keyValuePair in data)
			{
				if (keyValuePair.Value.Any<char>())
				{
					IMongoCollection<BsonDocument> collection = this.Database.GetCollection<BsonDocument>(keyValuePair.Key, null);
					IEnumerable<BsonDocument> enumerable = BsonSerializer.Deserialize<IEnumerable<BsonDocument>>(keyValuePair.Value, null);
					collection.InsertMany(enumerable, null, default(CancellationToken));
				}
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002874 File Offset: 0x00000A74
		public IEnumerable<string> GetAll(string collectionName)
		{
			IMongoCollection<BsonDocument> collection = this.Database.GetCollection<BsonDocument>(collectionName, null);
			BsonValue bsonValue = BsonMinKey.Value;
			FilterDefinitionBuilder<BsonDocument> filter = Builders<BsonDocument>.Filter;
			List<BsonDocument> list = new List<BsonDocument>();
			for (;;)
			{
				FilterDefinition<BsonDocument> filterDefinition = filter.Empty;
				if (bsonValue != BsonMinKey.Value)
				{
					filterDefinition = filter.Gt<BsonValue>("_id", bsonValue);
				}
				List<BsonDocument> list2 = collection.Find<BsonDocument>(filterDefinition, null).Sort(Builders<BsonDocument>.Sort.Ascending("_id")).Limit(new int?(50))
					.ToList<BsonDocument>(default(CancellationToken));
				if (!list2.Any<BsonDocument>())
				{
					break;
				}
				list.AddRange(list2);
				BsonDocument bsonDocument = list2.LastOrDefault<BsonDocument>();
				bsonValue = ((bsonDocument != null) ? bsonDocument["_id"] : null);
			}
			return list.Select<BsonDocument, string>((BsonDocument x) => x.ToJson(null, null, null, default(BsonSerializationArgs)));
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000295C File Offset: 0x00000B5C
		public void IniciarLastTs()
		{
			try
			{
				IMongoCollection<BsonDocument> collection = this.Database.GetCollection<BsonDocument>("LastTs", null);
				BsonDocument bsonDocument = collection.Find<BsonDocument>(FilterDefinition<BsonDocument>.Empty, null).Limit(new int?(1)).FirstOrDefault<BsonDocument, BsonDocument>(default(CancellationToken));
				BsonDocument bsonDocument2 = new BsonDocument { 
				{
					"data",
					DateTime.Now.ToString("s")
				} };
				if (bsonDocument == null)
				{
					collection.InsertOne(bsonDocument2, null, default(CancellationToken));
				}
				else
				{
					collection.ReplaceOne(Builders<BsonDocument>.Filter.Eq<BsonValue>("_id", bsonDocument["_id"]), bsonDocument2, null, default(CancellationToken));
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002A30 File Offset: 0x00000C30
		public long Count(string collectionName)
		{
			return this.Database.GetCollection<BsonDocument>(collectionName, null).Count(FilterDefinition<BsonDocument>.Empty, null, default(CancellationToken));
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002A60 File Offset: 0x00000C60
		public IEnumerable<string> Get(string collectionName, ObjectId? lastId, int take)
		{
			IMongoCollection<BsonDocument> collection = this.Database.GetCollection<BsonDocument>(collectionName, null);
			FilterDefinition<BsonDocument> filterDefinition = ((lastId == null) ? FilterDefinition<BsonDocument>.Empty : Builders<BsonDocument>.Filter.Gt<ObjectId>("_id", lastId.Value));
			return from x in collection.Find<BsonDocument>(filterDefinition, null).Sort(Builders<BsonDocument>.Sort.Ascending("_id")).Limit(new int?(take))
					.ToList<BsonDocument>(default(CancellationToken))
				select x.ToJson(null, null, null, default(BsonSerializationArgs));
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002B04 File Offset: 0x00000D04
		public Task BulkUpdateAsync(string collectionName, IEnumerable<string> data)
		{
			IMongoCollection<BsonDocument> collection = this.Database.GetCollection<BsonDocument>(collectionName, null);
			IEnumerable<ReplaceOneModel<BsonDocument>> enumerable = from x in data
				select BsonSerializer.Deserialize<BsonDocument>(x, null) into z
				select new ReplaceOneModel<BsonDocument>(Builders<BsonDocument>.Filter.Eq<BsonValue>("_id", z["_id"]), z)
				{
					IsUpsert = true
				};
			return collection.BulkWriteAsync(new List<WriteModel<BsonDocument>>(enumerable), null, default(CancellationToken));
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002B80 File Offset: 0x00000D80
		public void Drop(string collectionName)
		{
			this.Database.DropCollection(collectionName, default(CancellationToken));
		}

		// Token: 0x0400000D RID: 13
		private readonly string _dataBaseName;

		// Token: 0x0400000E RID: 14
		private readonly int _porta;

		// Token: 0x0400000F RID: 15
		private readonly FilterDefinitionBuilder<BsonDocument> _builder = Builders<BsonDocument>.Filter;

		// Token: 0x04000010 RID: 16
		private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
	}
}
