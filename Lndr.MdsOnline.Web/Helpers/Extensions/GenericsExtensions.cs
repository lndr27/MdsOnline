using System.Collections.Generic;
using System.Linq;

namespace Lndr.MdsOnline.Web.Helpers.Extensions
{
    public static class GenericsExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }
    }
}