using System;
using System.Reflection;
using Xunit;

namespace Tests.Extensions.Database
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal sealed class ContextsAttribute : MemberDataAttributeBase
    {
        public ContextsAttribute()
            : base(nameof(ContextProvider.Contexts), null)
        {
            MemberType = typeof(ContextProvider);
        }

        protected override object[] ConvertDataItem(MethodInfo testMethod, object item)
        {
            if (item == null)
            {
                return null;
            }

            if (!(item is object[] array))
            {
                throw new ArgumentException(
                    $"Property {MemberName} on {MemberType ?? testMethod.ReflectedType} yielded an item that is not an object[]");
            }
            return array;
        }
    }
}