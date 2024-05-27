using Newtonsoft.Json;
using RestSharp;
using WebApi.Mongo.Entities;

namespace WebApi.Services
{
    public interface ILarkService
    {
        public Task SendLarkTextMsg(string url, LarkTextMsgRequest larkTextMsgRequest);

        public Task SendLarkPostMsg(string url, LarkPostMsgRequest larkPostMsgRequest);

        public Task SendTestMsg(string url, string title, string message);

        public Task SendRequestAlarmMsg(string url, string title, ApisixLogRequest apisixLogRequest, BasicSetting setting);
    }

    public class LarkService : ILarkService
    {
        private readonly ILogger<LarkService> _logger;

        public LarkService(ILogger<LarkService> logger)
        {
            _logger = logger;
        }

        public async Task SendLarkTextMsg(string url, LarkTextMsgRequest larkTextMsgRequest)
        {
            try
            {
                var request = new RestRequest(url);

                var body = JsonConvert.SerializeObject(larkTextMsgRequest);
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                using var restClient = new RestClient();
                await restClient.ExecuteAsync(request, Method.Post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送飞书通知失败");
            }
        }

        public async Task SendLarkPostMsg(string url, LarkPostMsgRequest larkPostMsgRequest)
        {
            try
            {
                var request = new RestRequest(url);

                var body = JsonConvert.SerializeObject(larkPostMsgRequest);
                request.AddParameter("application/json", body, ParameterType.RequestBody);

                using var restClient = new RestClient();
                await restClient.ExecuteAsync(request, Method.Post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送飞书通知失败");
            }
        }

        public async Task SendTestMsg(string url, string title, string message)
        {
            var larkPostMsgRequest = new LarkPostMsgRequest
            {
                Content = new LarkPostMsgContent
                {
                    Post = new LarkPostMsgContentPost
                    {
                        EnUs = new LarkPostMsgContentEnUs
                        {
                            Title = title,
                            Content = new List<List<LarkPostMsgContentItem>>
                            {
                                new List<LarkPostMsgContentItem>
                                {
                                    new LarkPostMsgContentItem
                                    {
                                        Tag = "text",
                                        Text = message
                                    }
                                }
                            }
                        }
                    }
                }
            };

            await SendLarkPostMsg(url, larkPostMsgRequest);
        }

        public async Task SendRequestAlarmMsg(string url, string title, ApisixLogRequest apisixLogRequest, BasicSetting setting)
        {
            var larkPostMsgRequest = new LarkPostMsgRequest
            {
                Content = new LarkPostMsgContent
                {
                    Post = new LarkPostMsgContentPost
                    {
                        EnUs = new LarkPostMsgContentEnUs
                        {
                            Title = title,
                            Content = new List<List<LarkPostMsgContentItem>>()
                        }
                    }
                }
            };

            larkPostMsgRequest.Content.Post.EnUs.Content.Add(new List<LarkPostMsgContentItem>() { new LarkPostMsgContentItem { Tag = "text", Text = $"Start Time：{DateTimeOffset.FromUnixTimeMilliseconds(apisixLogRequest.StartTime).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")}" } });
            larkPostMsgRequest.Content.Post.EnUs.Content.Add(new List<LarkPostMsgContentItem>() { new LarkPostMsgContentItem { Tag = "text", Text = $"Client Ip：{apisixLogRequest.ClientIp}" } });
            larkPostMsgRequest.Content.Post.EnUs.Content.Add(new List<LarkPostMsgContentItem>() { new LarkPostMsgContentItem { Tag = "text", Text = $"Url：{apisixLogRequest.Request.Url}" } });
            larkPostMsgRequest.Content.Post.EnUs.Content.Add(new List<LarkPostMsgContentItem>() { new LarkPostMsgContentItem { Tag = "text", Text = $"Latency：{apisixLogRequest.Latency}" } });
            larkPostMsgRequest.Content.Post.EnUs.Content.Add(new List<LarkPostMsgContentItem>() { new LarkPostMsgContentItem { Tag = "text", Text = $"Status：{apisixLogRequest.Response.Status}" } });

            if (apisixLogRequest.Request.Jwt != null && apisixLogRequest.Request.Jwt.Count > 0)
            {
                foreach (var jwt in apisixLogRequest.Request.Jwt)
                {
                    larkPostMsgRequest.Content.Post.EnUs.Content.Add(new List<LarkPostMsgContentItem>() { new LarkPostMsgContentItem { Tag = "text", Text = $"{jwt.Key}: {jwt.Value}" } });
                }
            }

            if (apisixLogRequest.Request.Projection != null && apisixLogRequest.Request.Projection.Count > 0)
            {
                foreach (var projection in apisixLogRequest.Request.Projection)
                {
                    larkPostMsgRequest.Content.Post.EnUs.Content.Add(new List<LarkPostMsgContentItem>() { new LarkPostMsgContentItem { Tag = "text", Text = $"{projection.Key}: {projection.Value}" } });
                }
            }

            if (setting != null && !string.IsNullOrWhiteSpace(setting.Endpoint))
            {
                larkPostMsgRequest.Content.Post.EnUs.Content.Add(new List<LarkPostMsgContentItem>() { new LarkPostMsgContentItem { Tag = "text", Text = $"Details：{setting.Endpoint}/zh#/apisix-request-log/{apisixLogRequest.Id}" } });
            }

            await SendLarkPostMsg(url, larkPostMsgRequest);
        }
    }

    public class LarkTextMsgRequest
    {
        [JsonProperty("msg_type")]
        public string MsgType { get; set; } = "text";


        [JsonProperty("content")]
        public LarkTextMsgContent Content { get; set; }
    }

    public class LarkTextMsgContent
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class LarkPostMsgRequest
    {
        [JsonProperty("msg_type")]
        public string MsgType { get; set; } = "post";

        [JsonProperty("content")]
        public LarkPostMsgContent Content { get; set; }
    }

    public class LarkPostMsgContent
    {
        [JsonProperty("post")]
        public LarkPostMsgContentPost Post { get; set; }
    }

    public class LarkPostMsgContentPost
    {
        [JsonProperty("en_us")]
        public LarkPostMsgContentEnUs EnUs { get; set; }
    }

    public class LarkPostMsgContentEnUs
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("content")]
        public List<List<LarkPostMsgContentItem>> Content { get; set; }
    }

    public class LarkPostMsgContentItem
    {
        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("href")]
        public string Href { get; set; }
    }
}

