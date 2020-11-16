using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;

namespace MongoDbDemo
{
    public class PersonModel : IEntity
    {
        [BsonId]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AddressModel PrimaryAddress { get; set; }
        [BsonElement("dob")]
        public DateTime DateOfBirth { get; set; } //new property

        public void SetNewID() =>
            ID = ObjectId.GenerateNewId().ToString();

        public string ID { get; set; }
    }
}