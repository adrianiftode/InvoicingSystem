using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Pipeline
{
    public class AttachUser<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : Request
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AttachUser(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            request.User = _httpContextAccessor.HttpContext.User;

            var response = await next();
            return response;
        }
    }
}
