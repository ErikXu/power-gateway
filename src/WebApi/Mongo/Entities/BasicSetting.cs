using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Mongo.Entities
{
    public class BasicSetting : Entity
    {
        [BsonElement("latency")]
        public int Latency { get; set; }

        [BsonElement("userIdField")]
        public string UserIdField { get; set; }

        [BsonElement("endpoint")]
        public string Endpoint { get; set; }

        [BsonElement("dataKeepDays")]
        public int DataKeepDays { get; set; }

        [BsonElement("updateAt")]
        public DateTime UpdateAt { get; set; }
    }
}

