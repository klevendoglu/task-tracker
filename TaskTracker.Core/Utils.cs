using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker
{
    public static class Utils
    {
        public static IEnumerable<T> ToSafeList<T>(this List<T> source)
        {
            if (source != null)
                return source;
            return new T[0];
        }
    }
}
