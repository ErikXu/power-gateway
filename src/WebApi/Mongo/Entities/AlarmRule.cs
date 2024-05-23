using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Mongo.Entities
{
    public class AlarmRule : Entity
    {
        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("field")]
        public string Field { get; set; }

        [BsonElement("operator")]
        public string Operator { get; set; }

        [BsonElement("value")]
        public string Value { get; set; }

        //[BsonElement("alarmConfigId")]
        //public ObjectId AlarmConfigId { get; set; }
    }
}

