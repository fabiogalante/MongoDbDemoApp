using System;
using System.Linq;
using MongoDB.Entities;

namespace MongoDbDemo
{
    class Program
    {

        //https://mongodb-entities.com/wiki/Code-Samples.html


        static void Main(string[] args)
        {

            /*
                docker exec -it mongodb bash
                mongo
                show dbs
                use AddressBook
                db.Users.find().pretty()
                db.myCollection.find().pretty()
             */


             MongoCrud db = new MongoCrud("AddressBook");

            //db.DeleteRecord<PersonModel>("Users", new Guid("2c6befbe-bb73-46d4-9200-464f11cb578b"));

            //db.InsertRecord("Users",
            //    new PersonModel
            //    {
            //        FirstName = "Fabio", LastName = "Galante",
            //        PrimaryAddress = new AddressModel
            //            {City = "São Paulo", State = "SP", StreetAddress = "Rua Jacofer, 161", ZipCode = "02712070"}
            //    });

            var personModels = db.LoadRecords<PersonModel>("Users");

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




            OutrosExempplos();


            Console.ReadLine();
        }

        public class Person : Entity
        {
            public string Name { get; set; }
            public DateTime DateOfBirth { get; set; }
            public int SiblingCount { get; set; }
        }

        private async static void OutrosExempplos()
        {
            //https://dev.to/djnitehawk/tutorial-mongodb-with-c-the-easy-way-1g68


            await DB.InitAsync("MyDatabase", "localhost", 27017);

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
    }
}
