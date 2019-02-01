using MediatR;
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
        private readonly IAuthorize<TRequest> _authorize;

        public Authorization(IAuthorize<TRequest> authorize)
        {
            _authorize = authorize;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (!await _authorize.Authorize(request))
            {
                return Result.Forbidden.ConvertTo<TResponse>();
            }

            var response = await next();
            return response;
        }
    }
}
