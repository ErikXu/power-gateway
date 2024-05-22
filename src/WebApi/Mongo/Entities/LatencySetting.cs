using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Mongo.Entities
{
    public class LatencySetting : Entity
    {
        [BsonElement("latency")]
        public int Latency { get; set; }
    }
}

