using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HttpRequestProcessing
{
    public class HttpRequestProcessingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRequestProcessor _requestProcessor;

        public HttpRequestProcessingMiddleware(RequestDelegate next, IRequestProcessor requestProcessor)
        {
            _next = next;
            _requestProcessor = requestProcessor;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var cancellationToken = context.RequestAborted;

            string body = null;
            using (var reader = new StreamReader(context.Request.Body))
                body = await reader.ReadToEndAsync();

            var request = new RequestModel
            {
                Method = context.Request.Method.ToString(),
                Path = context.Request.Path.Value,
                Body = body
            };

            var response = await _requestProcessor.Process(request, cancellationToken);

            var writer = context.Response.BodyWriter;
            context.Response.StatusCode = response.StatusCode;
            if (!string.IsNullOrWhiteSpace(response.Body))
                await context.Response.WriteAsync(response.Body, cancellationToken);
            await context.Response.CompleteAsync();
        }
    }
}