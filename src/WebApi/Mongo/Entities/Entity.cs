using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Mongo.Entities
{
    public abstract class EntityWithTypedId<TId>
    {
        public TId Id { get; set; }
    }

    public abstract class Entity : EntityWithTypedId<ObjectId>
    {
        [BsonElement("createAt")]
        public DateTime CreateAt { get; set; }

        public Entity()
        {
            CreateAt = DateTime.UtcNow;
        }
    }
}
