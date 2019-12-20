using System.Threading;
using System.Threading.Tasks;

namespace HttpRequestProcessing
{
    public interface IRequestProcessor
    {
        Task<ResponseModel> Process(RequestModel request, CancellationToken cancellationToken);
    }
}