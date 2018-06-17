using System;

namespace Lndr.MdsOnline.Web.Helpers
{
    public static class Guard
    {
        public static void ForNullOrEmpty(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        public static void ForNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}