using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Pipeline
{
    public interface IAuthorize<in TRequest> where TRequest : Request
    {
        Task<bool> Authorize(TRequest request);
    }

    public class Authorization<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : Request
    {
        private readonly ILogger<Authorization<TRequest, TResponse>> _logger;
        private readonly IAuthorize<TRequest> _authorize;

        public Authorization(IAuthorize<TRequest> authorize, ILogger<Authorization<TRequest, TResponse>> logger)
        {
            _authorize = authorize;
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (!await _authorize.Authorize(request))
            {
                _logger.LogWarning("User is not authorized {user}", request.User?.GetIdentity());
                return Result.Forbidden.ConvertTo<TResponse>();
            }

            var response = await next();
            return response;
        }
    }
}
