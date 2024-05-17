using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityHelper.Extensions
{
    /// <summary>
    /// Provides Generic Extension Methods.
    /// </summary>
    public static class GenericExtensions
    {
        public static void Log(this object source) => Console.WriteLine(source);
        public static void Log(this IEnumerable source)
        {
            Console.Write("{ ");

            foreach (var value in source)
                Console.Write($"{value} ");

            Console.Write("} \n");
        }
        public static void Loop<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
                return;

            IEnumerator<T> enumerator = source.GetEnumerator();
            IDisposable disposable;
            try
            {
                while (enumerator.MoveNext())
                    action(enumerator.Current);
            }
            finally
            {
                disposable = enumerator;
                disposable.Dispose();
            }
        }
        public static bool CheckLoop<T>(this IEnumerable<T> source, Func<T, bool> condition)
        {
            IEnumerator<T> enumerator = source.GetEnumerator();
            IDisposable disposable;
            try
            {
                while (enumerator.MoveNext())
                    if (condition(enumerator.Current)) return true;
            }
            finally
            {
                disposable = enumerator;
                disposable.Dispose();
            }
            return false;
        }
        public static int FindTo<T>(this T[] value, Predicate<T> predicate)
        {
            for (int i = 0; i < value.Length; i++)
                if (predicate(value[i])) return i;
            return -1;
        }
        public static RangeEnumerator GetEnumerator(this Range range) => new(range);
    }
}
