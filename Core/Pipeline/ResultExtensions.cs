using System;
using System.Linq;

namespace Core.Pipeline
{
    internal static class ResultExtensions
    {
        public static TResponse ConvertTo<TResponse>(this Result result)
        {
            var responseType = typeof(TResponse);

            // construct Result<TItem>
            if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var itemType = typeof(TResponse).GetGenericArguments().First();
                var genericType = typeof(Result<>).MakeGenericType(itemType);
                var conversionMethod = genericType.GetMethod("op_Implicit", new[] { typeof(Result) });
                var @implicit = conversionMethod.Invoke(null, new object[] { result });

                return (TResponse)@implicit;
            }

            //construct (TItem, Result) (aka ValueType<TItem, Result>)
            if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(ValueTuple<,>))
            {
                var itemType = responseType.GetGenericArguments().First();
                var genericType = typeof(ValueTuple<,>).MakeGenericType(itemType, typeof(Result));
                return (TResponse)Activator.CreateInstance(genericType, null, result);
            }

            return default;
        }
    }
}
