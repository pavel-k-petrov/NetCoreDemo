using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HttpRequestProcessing
{
    public class SimpleRequestProcessor : IRequestProcessor
    {
        public Task<ResponseModel> Process(RequestModel request, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                new ResponseModel
                {
                    StatusCode = StatusCodes.Status200OK,
                    Body = $"requested path: {request.Path}{request.QueryString}"
                }
            );
        }
    }
}