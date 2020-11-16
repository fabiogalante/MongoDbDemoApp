using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDbDemo
{
    class MongoCrud
    {
        private readonly IMongoDatabase _db;

        public MongoCrud(string database)
        {
            var client = new MongoClient();
            _db = client.GetDatabase(database);
        }

        public void InsertRecord<T>(string table, T record)
        {
            var collection = _db.GetCollection<T>(table);
            collection.InsertOne(record);
        }


        //https://github.com/jonaswirth/MongoHelper/blob/master/MongoHelper/src/MongoHelper/MongoHelper.cs
        public bool Insert(string collectionName, BsonDocument doc)
        {
            try
            {
                var collection = _db.GetCollection<BsonDocument>(collectionName);
                collection.InsertOne(doc);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool InsertMany<T>(string collectionName, IEnumerable<T> documents)
        {
            try
            {
                List<BsonDocument> docs = new List<BsonDocument>();
                for (int i = 0; i < documents.Count(); i++)
                {
                    docs[i] = documents.ElementAt(i).ToBsonDocument();
                }
                var collection = _db.GetCollection<BsonDocument>(collectionName);
                collection.InsertMany(docs);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<T> LoadRecords<T>(string table)
        {
            var collection = _db.GetCollection<T>(table);

            return collection.Find(new BsonDocument()).ToList();
        }

        public T LoadRecordById<T>(string table, Guid id)
        {
            var collection = _db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);

            return collection.Find(filter).First();
        }

        //public void UpsertRecord<T>(string table, BsonBinaryData id, T record)
        //{
        //    var collection = _db.GetCollection<T>(table);

        //    var result =
        //        collection.ReplaceOne(new BsonDocument("_id", id), record, new ReplaceOptions {IsUpsert = true});
        //}

        public void UpsertRecord<T>(string table, Guid id, T record)
        {
            // Create the collection
            var collection = _db.GetCollection<T>(table);
            // Create the filter
            var filter = Builders<T>.Filter.Eq("Id", id);
            // Replace the record via an upsert
            // Upsert is an update if the filter finds something or
            // an insert if there is nothing matching
            collection.ReplaceOne(
                filter,
                record,
                new ReplaceOptions { IsUpsert = true });
        }

        public void DeleteRecord<T>(string table, Guid id)
        {
            var collection = _db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);

            collection.DeleteOne(filter);
        }
    }
}