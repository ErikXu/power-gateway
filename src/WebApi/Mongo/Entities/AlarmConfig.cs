using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Mongo.Entities
{
    public class AlarmConfig : Entity
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("botUrl")]
        public string BotUrl { get; set; }

        [BsonElement("createAt")]
        public DateTime CreateAt { get; set; }
    }
}

