using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PangyaAPI.SQL
{
    public static class EngineTools
    {
        public static bool IsNullOrEmpty<T>(T array)
        {
            return array == null;
        }

        public static bool IsNullOrEmpty<T>(T[] array)
        {
            return array == null || array.Length == 0;
        }
        public static string ToStrings(this sbyte s)
        {
            return $"'{s}'";
        }
        
        public static string ToStrings(this byte s)
        {
            return $"'{s}'";
        }

        public static string ToStrings(this short s)
        {
            return $"'{s}'";
        }

        public static string ToStrings(this ushort s)
        {
            return $"'{s}'";
        }

        public static string ToStrings(this int s)
        {
            return $"'{s}'";
        }

        public static string ToStrings(this bool s)
        {
            return $"'{s}'";
        }

        public static string ToStrings(this string s)
        {
            return $"'{s}'";
        }

        public static string ToStrings(this uint s)
        {
            return $"'{s}'";
        }

        public static string ToStrings(this long s)
        {
            return $"'{s}'";
        }

        public static string ToStrings(this ulong s)
        {
            return $"'{s}'";
        }

        public static string ToStrings(this float s)
        {
            return $"'{s}'";
        }

        public static string ToStrings(this double s)
        {
            return $"'{s}'";
        }

        public static string ToStrings(this decimal s)
        {
            return $"'{s}'";
        }

        public static string ToStrings(this char s)
        {
            return $"'{s}'";
        }

    }
}
