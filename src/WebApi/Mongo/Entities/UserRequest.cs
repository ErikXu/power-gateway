using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Mongo.Entities
{
    public class UserRequest : Entity
    {
        [BsonElement("time")]
        public DateTime Time { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("count")]
        public int Count { get; set; }
    }

    public class UserRequest1Day : UserRequest
    {

    }

    public class UserRequest1Hour : UserRequest
    {

    }

    public class UserRequest1Munite : UserRequest
    {

    }
}
