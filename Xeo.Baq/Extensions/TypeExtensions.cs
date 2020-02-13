using System;

namespace Xeo.Baq.Extensions
{
    public static class TypeExtensions
    {
        public static T Instantiate<T>(this Type @this)
            => (T) Activator.CreateInstance(@this);
    }
}