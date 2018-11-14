
using System.Collections.Generic;

namespace Core.Services
{
    public class Result<TItem>
    {
        private Result()
        {
        }

        public ResultStatus Status { get; private set; } = ResultStatus.Success;
        public TItem Item { get; private set; }
        public IReadOnlyCollection<string> Errors { get; private set; } = new List<string>();

        public static readonly Result<TItem> Forbidden = new Result<TItem> { Status = ResultStatus.Forbidden };
        public static readonly Result<TItem> NotPresent = new Result<TItem> { Status = ResultStatus.NotPresent };

        public static Result<TItem> Error(string message) => new Result<TItem>
        {
            Status = ResultStatus.InvalidOperation,
            Errors = new List<string> { message }
        };

        public static Result<TItem> Error(params string[] messages) => new Result<TItem>
        {
            Status = ResultStatus.InvalidOperation,
            Errors = new List<string>(messages)
        };

        public static Result<TItem> Success(TItem item)
            => new Result<TItem>
            {
                Item = item
            };
    }
}
