using Core.Pipeline;
using System.Threading.Tasks;

namespace Core
{
    public class DefaultAuthorize<TRequest> : IAuthorize<TRequest>
        where TRequest : Request
    {
        public Task<bool> Authorize(TRequest request)
            => Task.FromResult(request.User != null);
    }
}
