using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Pipeline
{
    public class Validation<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public Validation(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            
            if (failures.Count != 0)
            {
                Result failResult;
                
                var codes = failures.Select(c => c.ErrorCode).ToList();
                if (codes.Any(c => c == Result.Forbidden.StatusCode))
                {
                    failResult = Result.Forbidden;
                }
                else if (codes.Any(c => c == Result.NotPresent.StatusCode))
                {
                    failResult = Result.NotPresent;                    
                }
                else
                {
                    failResult = Result.Error(failures.Select(c=>c.ErrorMessage).ToArray());
                }

                return Task.FromResult(failResult.ConvertTo<TResponse>());
            }

            return next();
        }
    }
}
