using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace WebApi.Mongo.Entities
{
    public class ApisixLogRequest : Entity
    {
        [BsonElement("response")]
        [JsonProperty("response")]
        public ApisixResponse Response { get; set; }

        [BsonElement("request")]
        [JsonProperty("request")]
        public ApisixRequest Request { get; set; }

        [BsonElement("apisixLatency")]
        [JsonProperty("apisix_latency")]
        public double ApisixLatency { get; set; }

        [BsonElement("clientIp")]
        [JsonProperty("client_ip")]
        public string ClientIp { get; set; }

        [BsonElement("serviceId")]
        [JsonProperty("service_id")]
        public string ServiceId { get; set; }

        [BsonElement("server")]
        [JsonProperty("server")]
        public Dictionary<string, string> Server { get; set; }

        [BsonElement("upstreamLatency")]
        [JsonProperty("upstream_latency")]
        public int UpstreamLatency { get; set; }

        [BsonElement("latency")]
        [JsonProperty("latency")]
        public double Latency { get; set; }

        [BsonElement("startTime")]
        [JsonProperty("start_time")]
        public long StartTime { get; set; }

        [BsonElement("routeId")]
        [JsonProperty("route_id")]
        public string RouteId { get; set; }

        [BsonElement("upstream")]
        [JsonProperty("upstream")]
        public string Upstream { get; set; }
    }

    public class ApisixResponse
    {
        [BsonElement("status")]
        [JsonProperty("status")]
        public int Status { get; set; }

        [BsonElement("headers")]
        [JsonProperty("headers")]
        public Dictionary<string, string> Headers { get; set; }

        [BsonElement("size")]
        [JsonProperty("size")]
        public int Size { get; set; }

        [BsonElement("body")]
        [JsonProperty("body")]
        public string Body { get; set; }
    }

    public class ApisixRequest
    {
        [BsonElement("queryString")]
        [JsonProperty("querystring")]
        public Dictionary<string, string> QueryString { get; set; }

        [BsonElement("headers")]
        [JsonProperty("headers")]
        public Dictionary<string, string> Headers { get; set; }

        [BsonElement("jwt")]
        public Dictionary<string, string> Jwt { get; set; }

        [BsonElement("url")]
        [JsonProperty("url")]
        public string Url { get; set; }

        [BsonElement("uri")]
        [JsonProperty("uri")]
        public string Uri { get; set; }

        [BsonElement("size")]
        [JsonProperty("size")]
        public int Size { get; set; }

        [BsonElement("method")]
        [JsonProperty("method")]
        public string Method { get; set; }

        [BsonElement("body")]
        [JsonProperty("body")]
        public string Body { get; set; }
    }
}
