using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;

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

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext(request);

            var results = new List<ValidationResult>();
            foreach (var validator in _validators)
            {
                results.Add(await validator.ValidateAsync(context, cancellationToken));
            }

            var failures = results
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

                return failResult.ConvertTo<TResponse>();
            }

            return await next();
        }
    }
}
