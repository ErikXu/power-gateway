using Newtonsoft.Json;

namespace WebApi.Models
{
    public class ApisixLogRequest
    {
        [JsonProperty("response")]
        public ApisixResponse Response { get; set; }

        [JsonProperty("request")]
        public ApisixRequest Request { get; set; }

        [JsonProperty("apisix_latency")]
        public double ApisixLatency { get; set; }

        [JsonProperty("client_ip")]
        public string ClientIp { get; set; }

        [JsonProperty("service_id")]
        public string ServiceId { get; set; }

        [JsonProperty("server")]
        public Dictionary<string, string> Server { get; set; }

        [JsonProperty("upstream_latency")]
        public int UpstreamLatency { get; set; }

        [JsonProperty("latency")]
        public double Latency { get; set; }

        [JsonProperty("start_time")]
        public long StartTime { get; set; }

        [JsonProperty("route_id")]
        public string RouteId { get; set; }

        [JsonProperty("upstream")]
        public string Upstream { get; set; }
    }

    public class ApisixResponse
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("headers")]
        public Dictionary<string, string> Headers { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }

    public class ApisixRequest
    {
        [JsonProperty("querystring")]
        public Dictionary<string, string> QueryString { get; set; }

        [JsonProperty("headers")]
        public Dictionary<string, string> Headers { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }
    }

    public class Server
    {
        public string version { get; set; }
        public string hostname { get; set; }
    }

}
