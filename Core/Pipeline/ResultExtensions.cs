using System.Linq;

namespace Core.Pipeline
{
    internal static class ResultExtensions
    {
        public static TResponse ConvertTo<TResponse>(this Result result)
        {
            var itemType = typeof(TResponse).GetGenericArguments().First();
            var genericType = typeof(Result<>).MakeGenericType(itemType);
            var conversionMethod = genericType.GetMethod("op_Implicit", new[] { typeof(Result) });
            var @implicit = conversionMethod.Invoke(null, new object[] { result });

            return (TResponse)@implicit;
        }
    }
}
