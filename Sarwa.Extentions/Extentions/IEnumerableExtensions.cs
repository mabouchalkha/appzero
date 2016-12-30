namespace System.Collections.Generic
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T obj in source)
                action?.Invoke(obj);
        }

        public static IEnumerable<T> LazyDefaultIfEmpty<T>(this IEnumerable<T> source, Func<T> defaultFactory)
        {
            var isEmpty = true;

            foreach (T value in source)
            {
                yield return value;
                isEmpty = false;
            }

            if (isEmpty)
                yield return defaultFactory();

        }
    }
}
