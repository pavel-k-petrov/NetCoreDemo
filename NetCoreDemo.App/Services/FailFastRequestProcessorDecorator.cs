using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HttpRequestProcessing
{
    public class FailFastRequestProcessorDecorator : IRequestProcessor
    {
        private readonly IRequestProcessor _requestProcessor;
        private readonly IMessageBusHealthChecker _messageBusHealthChecker;

        public FailFastRequestProcessorDecorator(IRequestProcessor requestProcessor, IMessageBusHealthChecker messageBusHealthChecker)
        {
            _requestProcessor = requestProcessor;
            _messageBusHealthChecker = messageBusHealthChecker;
        }

        public async Task<ResponseModel> Process(RequestModel request, CancellationToken cancellationToken)
        {
            if (_messageBusHealthChecker.GetStatus() != MessageBusHealthStatus.Online)
                return GetFailResponse();

            var result = await _requestProcessor.Process(request, cancellationToken);
            return result;
        }

        private ResponseModel GetFailResponse()
        {
            return new ResponseModel
            {
                StatusCode = StatusCodes.Status503ServiceUnavailable,
                Body = "Message bus is offline"
            };
        }
    }
}