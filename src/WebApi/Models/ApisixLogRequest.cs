namespace WebApi.Models
{
    public class ApisixLogRequest
    {
        public ApisixResponse response { get; set; }
        public ApisixRequest request { get; set; }
        public int apisix_latency { get; set; }
        public string client_ip { get; set; }
        public string service_id { get; set; }
        public Server server { get; set; }
        public int upstream_latency { get; set; }
        public float latency { get; set; }
        public long start_time { get; set; }
        public string route_id { get; set; }
        public string upstream { get; set; }
    }

    public class ApisixResponse
    {
        public int status { get; set; }
        public Headers headers { get; set; }
        public int size { get; set; }
        public string body { get; set; }
    }

    public class Headers
    {
        public string server { get; set; }
        public string contenttype { get; set; }
        public string connection { get; set; }
        public string date { get; set; }
    }

    public class ApisixRequest
    {
        public Querystring querystring { get; set; }
        public Headers1 headers { get; set; }
        public string url { get; set; }
        public string uri { get; set; }
        public int size { get; set; }
        public string method { get; set; }
    }

    public class Querystring
    {
    }

    public class Headers1
    {
        public string secfetchsite { get; set; }
        public string acceptencoding { get; set; }
        public string secfetchmode { get; set; }
        public string host { get; set; }
        public string referer { get; set; }
        public string secchua { get; set; }
        public string acceptlanguage { get; set; }
        public string secchuamobile { get; set; }
        public string secfetchdest { get; set; }
        public string accept { get; set; }
        public string secchuaplatform { get; set; }
        public string useragent { get; set; }
    }

    public class Server
    {
        public string version { get; set; }
        public string hostname { get; set; }
    }

}
