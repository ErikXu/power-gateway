using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Mongo.Entities
{
    public class BasicSetting : Entity
    {
        [BsonElement("latency")]
        public int Latency { get; set; }

        [BsonElement("userIdField")]
        public string UserIdField { get; set; }

        [BsonElement("updateAt")]
        public DateTime UpdateAt { get; set; }
    }
}

