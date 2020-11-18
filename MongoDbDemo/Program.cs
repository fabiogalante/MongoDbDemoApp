using System;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace MongoDbDemo
{
    internal class Program
    {
        //https://mongodb-entities.com/wiki/Code-Samples.html


        private static void Main(string[] args)
        {
            /*
                docker exec -it mongodb bash
                mongo
                show dbs
                use AddressBook
                db.Users.find().pretty()
                db.myCollection.find().pretty()
             */


            var db = new MongoCrud("Picking");

            //db.DeleteRecord<PersonModel>("Users", new Guid("2c6befbe-bb73-46d4-9200-464f11cb578b"));

            //db.InsertRecord("Users",
            //    new PersonModel
            //    {
            //        FirstName = "Fabio", LastName = "Galante",
            //        PrimaryAddress = new AddressModel
            //            {City = "São Paulo", State = "SP", StreetAddress = "Rua Jacofer, 161", ZipCode = "02712070"}
            //    });

            //var personModels = db.LoadRecords<PersonModel>("Users");

            //var nameModels = db.LoadRecords<NameModel>("Users");

            //var recs = db.LoadRecords<PersonModel>("Users");


            //foreach (var personModel in recs)
            //{
            //    Console.WriteLine($"{personModel.Id} : {personModel.FirstName} {personModel.LastName} ");

            //    if (personModel.PrimaryAddress != null)
            //    {
            //        Console.WriteLine(personModel.PrimaryAddress.City);
            //    }
            //    Console.WriteLine();
            //}


            //foreach (var personModel in personModels)
            //{
            //    Console.WriteLine($"{personModel.Id} : {personModel.FirstName} {personModel.LastName} ");

            //    Console.WriteLine();
            //}


            //var record = db.LoadRecordById<PersonModel>("Users", new Guid("810b3aa5-d99b-4879-a249-6d11054f2094"));

            //record.DateOfBirth = new DateTime(1971, 6, 24,0,0,0, DateTimeKind.Utc);

            //db.UpsertRecord<PersonModel>("Users", record.Id, record);

            //var newrecord = db.LoadRecordById<PersonModel>("Users", new Guid("810b3aa5-d99b-4879-a249-6d11054f2094"));

            // db.DeleteRecord<PersonModel>("Users", new Guid("2c6befbe-bb73-46d4-9200-464f11cb578b"));


            // OutrosExempplos();

            //GravarPedidos();

            GravarCestos();


            // Console.ReadLine();
        }

        private static void GravarCestos()
        {
            var db = new MongoCrud("Picking");

            var rnd = new Random();

            for (var i = 0; i < 1500; i++)
            {
                long baskNum = rnd.Next(100000, 999999);
                long orderNum = rnd.Next(1000000, 9999999);
                var basket = new Basket
                {
                    BasketNum = baskNum,
                    DatabaseName = "SBOBBelezaNaWebProd",
                    OrderNum = orderNum,
                    PickEntry = 0
                };
                db.InsertRecord("Basket", basket);
            }
        }

        private static void GravarPedidos()
        {
            var db = new MongoCrud("Picking");

            var rnd = new Random();


            for (var i = 0; i < 2000; i++)
            {
                long docEntry = rnd.Next(100000, 999999);
                long baskNum = rnd.Next(1000, 9999);
                long docNum = rnd.Next(1000000, 9999999);
                var fetch = new FechNextPick
                {
                    DocEntry = docEntry,
                    Fork = "Simples",
                    Location = 0,
                    BasketNum = baskNum,
                    BplId = 5,
                    DocNum = docNum
                };
                db.InsertRecord("Ordr", fetch);
            }
        }

        private static async void OutrosExempplos()
        {
            //https://dev.to/djnitehawk/tutorial-mongodb-with-c-the-easy-way-1g68


            await DB.InitAsync("MyDatabase", "localhost");

            var lisa = new Person
            {
                Name = "Lisa Malfrey",
                DateOfBirth = new DateTime(1983, 10, 11),
                SiblingCount = 1
            };

            await lisa.SaveAsync();

            Console.WriteLine($"Lisa's ID: {lisa.ID}");
            Console.Read();

            var person = await DB.Find<Person>().OneAsync(lisa.ID);

            Console.WriteLine($"Found Person: {person.Name}");
            Console.Read();

            var result = (await DB.Find<Person>()
                    .ManyAsync(p => p.SiblingCount >= 1))
                .First();

            Console.WriteLine($"Count: {result.SiblingCount}");
            Console.Read();


            var newPerson = await DB.Find<Person>().OneAsync(lisa.ID);

            newPerson.Name = "Lisa Kudrow";
            newPerson.SiblingCount = 2;

            await newPerson.SaveAsync();

            await DB.Update<Person>()
                .Match(p => p.ID == lisa.ID)
                .Modify(p => p.Name, "Lisa Kudrow")
                .Modify(p => p.SiblingCount, 2)
                .ExecuteAsync();
        }

        public class FechNextPick
        {
            [BsonId] public Guid Id { get; set; }

            public long DocEntry { get; set; }
            public long DocNum { get; set; }
            public int? BplId { get; set; }
            public string Fork { get; set; }
            public long BasketNum { get; set; }
            public int Location { get; set; }
        }


        public class Basket
        {
            [BsonId] public Guid Id { get; set; }

            public long OrderNum { get; set; }

            public long PickEntry { get; set; }

            public long BasketNum { get; set; }
            public string DatabaseName { get; set; }
        }

        public class Person : Entity
        {
            public string Name { get; set; }
            public DateTime DateOfBirth { get; set; }
            public int SiblingCount { get; set; }
        }
    }
}