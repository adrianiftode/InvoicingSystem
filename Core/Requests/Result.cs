using System.Collections.Generic;

namespace Core
{
    public sealed class Result
    {
        public ResultStatus Status { get; private set; } = ResultStatus.Success;
        public string StatusCode => Status.ToString();
        public IReadOnlyCollection<string> Errors { get; private set; } = new List<string>();
        public static readonly Result Forbidden = new Result { Status = ResultStatus.Forbidden };
        public static readonly Result NotPresent = new Result { Status = ResultStatus.NotPresent };
        public static readonly Result Success = new Result { Status = ResultStatus.Success };

        public static Result Error(string message) => new Result
        {
            Status = ResultStatus.InvalidOperation,
            Errors = new List<string> { message }
        };

        public static Result Error(params string[] message) => new Result
        {
            Status = ResultStatus.InvalidOperation,
            Errors = message
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
