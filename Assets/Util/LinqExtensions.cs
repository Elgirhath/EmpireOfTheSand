using System;
using System.Collections.Generic;
using System.Linq;

namespace Util
{
    public static class LinqExtensions
    {
        public static TS ArgMax<T, TS>(this IEnumerable<TS> enumerable, Func<TS, T> predicate) where T : IComparable
        {
            if (enumerable == null || !enumerable.Any()) return default;

            return enumerable.Aggregate((curMax, x) => curMax == null || Comparer<T>.Default.Compare(predicate(x), predicate(curMax)) > 0 ? x : curMax);
        }

        public static TS ArgMin<T, TS>(this IEnumerable<TS> enumerable, Func<TS, T> predicate) where T : IComparable
        {
            if (enumerable == null || !enumerable.Any()) return default;

            return enumerable.Aggregate((curMin, x) => curMin == null || Comparer<T>.Default.Compare(predicate(x), predicate(curMin)) < 0 ? x : curMin);
        }
    }
}