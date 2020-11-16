using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbDemo
{
    public class NameModel
    {
        [BsonId]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}