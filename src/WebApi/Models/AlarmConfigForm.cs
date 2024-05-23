using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models
{
    public class AlarmConfigForm
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string BotUrl { get; set; }
    }
}

