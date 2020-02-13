using System;
using System.Collections.Generic;
using System.Linq;

namespace Xeo.Baq.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            using (IEnumerator<T> enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return YieldBatchElements(enumerator, batchSize - 1);
                }
            }
        }

        public static IEnumerable<TElement> ConditionalWhere<TElement, TParam>(this IEnumerable<TElement> source,
            Func<IEnumerable<TElement>, bool> condition,
            Func<TParam> paramFactory,
            Func<TElement, TParam, bool> wherePredicate)
        {
            if (condition(source))
            {
                TParam param = paramFactory();

                return source.Where(x => wherePredicate(x, param));
            }

            return source;
        }

        public static IEnumerable<TElement> ConditionalWhere<TElement>(this IEnumerable<TElement> source,
            Func<IEnumerable<TElement>, bool> condition,
            Func<TElement, bool> wherePredicate)
        {
            if (condition(source))
            {
                return source.Where(wherePredicate);
            }

            return source;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
            => source.ReturnThis(() =>
            {
                foreach (T x in source)
                {
                    action(x);
                }
            });

        private static IEnumerable<T> YieldBatchElements<T>(IEnumerator<T> source, int batchSize)
        {
            yield return source.Current;

            for (var i = 0; i < batchSize && source.MoveNext(); i++)
            {
                yield return source.Current;
            }
        }
    }
}