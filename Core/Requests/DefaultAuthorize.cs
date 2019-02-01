using System.Threading.Tasks;
using Core.Pipeline;

namespace Core
{
    public class DefaultAuthorize<TRequest> : IAuthorize<TRequest>
        where TRequest : Request
    {
        public Task<bool> Authorize(TRequest request) => Task.FromResult(request.User != null);
    }
}