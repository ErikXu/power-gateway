using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Mongo.Entities
{
    public class Qps : Entity
    {
        [BsonElement("time")]
        public DateTime Time { get; set; }

        [BsonElement("text")]
        public string Text { get; set; }

        [BsonElement("total")]
        public int Total { get; set; }

        [BsonElement("value")]
        public double Value { get; set; }

        [BsonElement("latencyBenchmark")]
        public int LatencyBenchmark { get; set; }

        [BsonElement("lowPerformanceCount")]
        public int LowPerformanceCount { get; set; }

        [BsonElement("latencyAvg")]
        public double LatencyAvg { get; set; }

        [BsonElement("latencyMin")]
        public double LatencyMin { get; set; }

        [BsonElement("latencyMax")]
        public double LatencyMax { get; set; }
    }

    public class Qps24Hour: Qps
    {

    }

    public class Qps1Hour : Qps
    {

    }

    public class Qps1Munite : Qps
    {

    }
}

