
using System.Collections.Generic;

namespace Core
{
    public sealed class Result
    {
        public ResultStatus Status { get; private set; } = ResultStatus.Success;
        public IReadOnlyCollection<string> Errors { get; private set; } = new List<string>();
        public static readonly Result Forbidden = new Result { Status = ResultStatus.Forbidden };
        public static readonly Result NotPresent = new Result { Status = ResultStatus.NotPresent };

        public static Result Error(string message) => new Result
        {
            Status = ResultStatus.InvalidOperation,
            Errors = new List<string> { message }
        };
    }

    public sealed class Result<TItem>
    {
        private Result()
        {
        }

        public ResultStatus Status { get; private set; } = ResultStatus.Success;
        public TItem Item { get; private set; }
        public IReadOnlyCollection<string> Errors { get; private set; } = new List<string>();
        
        public static implicit operator Result<TItem>(Result result) => new Result<TItem>
        {
            Errors = result.Errors,
            Status = result.Status
        };

        public static implicit operator Result<TItem>(TItem item) => new Result<TItem>
        {
            Item = item,
            Status = ResultStatus.Success
        };
    }
}
