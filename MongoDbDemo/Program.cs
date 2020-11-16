using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace MongoDbDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoCrud db = new MongoCrud("AddressBook");

            //db.InsertRecord("Users",
            //    new PersonModel
            //    {
            //        FirstName = "Fabio", LastName = "Galante",
            //        PrimaryAddress = new AddressModel
            //            {City = "São Paulo", State = "SP", StreetAddress = "Rua Jacofer, 161", ZipCode = "02712070"}
            //    });


            var recs = db.LoadRecords<PersonModel>("Users");

            //foreach (var personModel in recs)
            //{
            //    Console.WriteLine($"{personModel.Id} : {personModel.FirstName} {personModel.LastName} ");

            //    if (personModel.PrimaryAddress != null)
            //    {
            //        Console.WriteLine(personModel.PrimaryAddress.City);
            //    }
            //    Console.WriteLine();
            //}

            var record = db.LoadRecordById<PersonModel>("Users", new Guid("810b3aa5-d99b-4879-a249-6d11054f2094"));

            Console.ReadLine();
        }
    }


    public class PersonModel
    {
        [BsonId]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public AddressModel PrimaryAddress { get; set; }

    }

    public class AddressModel
    {
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    class MongoCrud
    {
        private IMongoDatabase _db;

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
    }
}
