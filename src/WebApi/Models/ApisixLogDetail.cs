namespace WebApi.Models
{
    public class ApisixLogDetail
    {
        public string Id { get; set; }

        public string Method { get; set; }

        public string Url { get; set; }

        public string Upstream { get; set; }

        public double Latency { get; set; }

        public int Status { get; set; }

        public string ClientIp { get; set; }

        public DateTime StartTime { get; set; }

        public List<KeyValueItem<string, string>> RequestHeaders { get; set; }

        public List<KeyValueItem<string, string>> Jwt { get; set; }

        public List<KeyValueItem<string, string>> Projection { get; set; }

        public List<KeyValueItem<string, string>> QueryStrings { get; set; }

        public string RequestBody { get; set; }

        public List<KeyValueItem<string, string>> ResponseHeaders { get; set; }

        public string ResponseBody { get; set; }
    }
}
