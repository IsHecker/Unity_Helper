using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityHelper.Extensions
{
    /// <summary>
 /// Provides Functions and Extensions for Arrays.
 /// </summary>
    public static class ArrayExtensions
    {
        public static void Add<T>(this T[] array, T item)
        {
            Array.Resize(ref array, array.Length + 1);
            array[^1] = item;
        }
        public static bool ArrayEquals<T>(T[] lhs, T[] rhs)
        {
            if (lhs == null || rhs == null)
                return lhs == rhs;

            if (lhs.Length != rhs.Length)
                return false;

            ReadOnlySpan<T> lSpan = lhs;
            ReadOnlySpan<T> rSpan = rhs;
            for (int i = 0; i < lhs.Length; i++)
            {
                if (!lSpan[i].Equals(rSpan[i]))
                    return false;
            }
            return true;
        }
        public static bool ArrayReferenceEquals<T>(T[] lhs, T[] rhs)
        {
            if (lhs == null || rhs == null)
                return lhs == rhs;

            if (lhs.Length != rhs.Length)
                return false;

            ReadOnlySpan<T> lSpan = lhs;
            ReadOnlySpan<T> rSpan = rhs;
            for (int i = 0; i < lSpan.Length; i++)
            {
                if ((object)lSpan[i] != (object)rSpan[i])
                    return false;
            }

            return true;
        }
        public static void AddRange<T>(ref T[] array, T[] items)
        {
            int arrayLen = array.Length;
            Array.Resize(ref array, array.Length + items.Length);
            for (int i = 0; i < items.Length; i++)
            {
                array[arrayLen + i] = items[i];
            }
        }
        public static void Insert<T>(ref T[] array, int index, T item)
        {
            ArrayList arrayList = new ArrayList();
            arrayList.AddRange(array);
            arrayList.Insert(index, item);
            array = arrayList.ToArray(typeof(T)) as T[];
        }
        public static void Remove<T>(ref T[] array, T item)
        {
            List<T> list = new List<T>(array);
            list.Remove(item);
            array = list.ToArray();
        }
        public static List<T> FindAll<T>(T[] array, Predicate<T> match)
        {
            List<T> list = new List<T>(array);
            return list.FindAll(match);
        }
        public static T Find<T>(T[] array, Predicate<T> match)
        {
            List<T> list = new List<T>(array);
            return list.Find(match);
        }
        public static int FindIndex<T>(T[] array, Predicate<T> match)
        {
            List<T> list = new List<T>(array);
            return list.FindIndex(match);
        }
        public static int IndexOf<T>(T[] array, T value)
        {
            List<T> list = new List<T>(array);
            return list.IndexOf(value);
        }
        public static int LastIndexOf<T>(T[] array, T value)
        {
            List<T> list = new List<T>(array);
            return list.LastIndexOf(value);
        }
        public static void RemoveAt<T>(ref T[] array, int index)
        {
            List<T> list = new List<T>(array);
            list.RemoveAt(index);
            array = list.ToArray();
        }
        public static void Clear<T>(this T[] array)
        {
            Array.Clear(array, 0, array.Length);
            Array.Resize(ref array, 0);
        }
    }
}
