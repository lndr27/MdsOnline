using System.Collections.Generic;
using System.Linq;

namespace Lndr.MdsOnline.Helpers.Extensions
{
    public static class GenericsExtensions
    {
        public static bool IsNullOrEmpty(this IEnumerable<object> list)
        {
            return list == null || !list.Any();
        }
    }
}