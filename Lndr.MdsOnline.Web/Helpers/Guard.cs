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

        public static void ArgumentoForaDaFaixa(string nomeArgumento, int arg, int min = int.MinValue, int max = int.MaxValue)
        {
            if (arg < min || arg > max)
                throw new ArgumentOutOfRangeException(string.Format("Argumento {0} fora da faixa de {1} até {2}", nomeArgumento, min, max));

        }
    }
}