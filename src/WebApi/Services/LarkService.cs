using Newtonsoft.Json;
using RestSharp;

namespace WebApi.Services
{
    public interface ILarkService
    {
        public Task SendLarkTextMsg(string url, LarkTextMsgRequest larkTextMsgRequest);

        public Task SendLarkPostMsg(string url, LarkPostMsgRequest larkPostMsgRequest);

        public Task SendTestMsg(string url, string title, string message);
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

