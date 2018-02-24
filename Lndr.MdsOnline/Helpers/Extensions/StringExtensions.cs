using System;
using System.Text.RegularExpressions;

namespace Lndr.MdsOnline.Helpers.Extensions
{
    public static class StringExtensions
    {
        public static string RemoverTagsHtml(this string input)
        {
            return Regex.Replace(input, @"<.*?>", String.Empty);
        }

        public static string RemoverEspacosEmBranco(this string input)
        {
            return Regex.Replace(input, @"\s+", String.Empty);
        }
    }
}