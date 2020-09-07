using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.Moa
{
    class StringFunction
    {
        public static string before(string origin, string keyword)
        {
            var fIndex = origin.IndexOf(keyword);
            return origin.Substring(0, fIndex);
        }

        public static string beforeLast(string origin, string keyword)
        {
            var lIndex = origin.LastIndexOf(keyword);
            return origin.Substring(0, lIndex);
        }

        public static string after(string origin, string keyword)
        {
            var fIndex = origin.IndexOf(keyword)+1;
            return origin.Substring(fIndex, origin.Length - fIndex);
        }

        public static string afterLast(string origin, string keyword)
        {
            var lIndex = origin.LastIndexOf(keyword)+1;
            return origin.Substring(lIndex, origin.Length - lIndex);
        }

        internal static string[] splitWithFirst(string origin, string keyword)
        {
            return new string[] { before(origin, keyword), after(origin, keyword) };
        }

        internal static string[] splitWithLast(string origin, string keyword)
        {
            return new string[] { beforeLast(origin, keyword), afterLast(origin, keyword) };
        }
    }
}
