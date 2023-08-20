using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PangyaAPI.SQL.Tools
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
    }
}
