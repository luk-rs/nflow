namespace Flow.Reactive.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class EnumerableExtensions
    {
        public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            var items = source.ToList();
            items.ForEach(item => action(item));
            return items;
        }

        public static bool None<T>(this IEnumerable<T> source, Predicate<T> predicate) => source.All(item => !predicate(item));

        public static IEnumerable<T> SelectMany<T>(this IEnumerable<IEnumerable<T>> source) => source.SelectMany(x => x);
    }
}
