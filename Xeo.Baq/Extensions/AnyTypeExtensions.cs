using System;

namespace Xeo.Baq.Extensions
{
    public static class AnyTypeExtensions
    {
        public static T ReturnThis<T>(this T @this, Action action)
        {
            action();

            return @this;
        }
    }
}