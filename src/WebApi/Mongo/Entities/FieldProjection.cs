using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Mongo.Entities
{
    public class FieldProjection : Entity
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("key")]
        public string Key { get; set; }

        [BsonElement("fromKey")]
        public string FromKey { get; set; }

        [BsonElement("mappings")]
        public Dictionary<string, string> Mappings { get; set; }
    }
}

