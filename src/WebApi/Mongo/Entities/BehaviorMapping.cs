using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Mongo.Entities
{
    public class BehaviorMapping : Entity
    {
        [BsonElement("method")]
        public string Method { get; set; }

        [BsonElement("operator")]
        public string Operator { get; set; }

        [BsonElement("value")]
        public string Value { get; set; }
    }
}
