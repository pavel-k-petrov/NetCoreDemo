using Microsoft.AspNetCore.Http;

namespace HttpRequestProcessing
{
    public class RequestModel
    {
        public string Body { get; set; }
        public string Path { get; set; }

        public string Method { get; set; }
        public string QueryString { get; set; }
    }
}