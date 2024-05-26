using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Mongo.Entities
{
    public class IpRequest : Entity
    {
        [BsonElement("time")]
        public DateTime Time { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("ip")]
        public string Ip { get; set; }

        [BsonElement("count")]
        public int Count { get; set; }
    }

    public class IpRequest1Day : IpRequest
    {

    }

    public class IpRequest1Hour : IpRequest
    {

    }

    public class IpRequest1Munite : IpRequest
    {

    }
}
