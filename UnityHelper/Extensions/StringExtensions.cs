using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityHelper.Extensions
{
    /// <summary>
    /// Provides Extension Methods for string.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Modify the allocated <paramref name="source"/> string without re-allocating an entirely new string.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="replace"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public unsafe static string Modify(this string source, char replace, int start = 0, int end = 0)
        {

            if (start - 1 < 0 || end - 1 >= source.Length)
                throw new InvalidOperationException("Index Out Of Range!");

            ReadOnlySpan<char> span = source;
            for (int i = start - 1; i < end; i++)
                fixed (char* ptr = &span[i]) *ptr = replace;
            return source;
        }

        public static string Reverse(this string str)
        {
            char[] charArray = str.ToCharArray();
            System.Array.Reverse(charArray);
            return new string(charArray);
        }

        public static string Shuffle(this string str)
        {
            char[] charArray = str.ToCharArray();
            for (int i = 0; i < charArray.Length; i++)
            {
                int randomIndex = UnityEngine.Random.Range(i, charArray.Length);
                (charArray[randomIndex], charArray[i]) = (charArray[i], charArray[randomIndex]);
            }
            return new string(charArray);
        }

    }
}
