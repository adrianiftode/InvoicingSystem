
namespace Core.Services
{
    public class Result<TItem>
    {
        private Result()
        {
        }

        public ResultStatus Status { get; private set; } = ResultStatus.Success;
        public TItem Item { get; private set; }

        public static readonly Result<TItem> Forbidden = new Result<TItem> { Status = ResultStatus.Forbidden };
        public static readonly Result<TItem> NotPresent = new Result<TItem> { Status = ResultStatus.NotPresent };

        public static Result<TItem> Success(TItem item)
            => new Result<TItem>
            {
                Item = item
            };
    }
}
