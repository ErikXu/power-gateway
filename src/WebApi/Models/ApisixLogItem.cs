namespace WebApi.Models
{
    public class ApisixLogItem
    {
        public string Id { get; set; }

        public string Method { get; set; }

        public string Url { get; set; }

        public string Upstream { get; set; }

        public double Latency { get; set; }

        public int Status { get; set; }

        public DateTime StartTime { get; set; }
    }
}
