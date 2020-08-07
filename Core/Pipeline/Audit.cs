using Audit.Core;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Pipeline
{
    public class Audit<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : Request
    {
        private readonly IAuditScopeFactory _auditScopeFactory;

        public Audit(IAuditScopeFactory auditScopeFactory) => _auditScopeFactory = auditScopeFactory;

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            await using var _ = await _auditScopeFactory.CreateAsync(new AuditScopeOptions(typeof(TRequest).Name, null, new
            {
                Request = request
            }));

            return await next();
        }
    }
}
