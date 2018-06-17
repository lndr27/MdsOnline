using System;
using System.Web;
using System.Web.Security;

namespace Lndr.MdsOnline.Web.Helpers.Extensions
{
    public static class EncryptionExtensions
    {
        private static readonly string _purpose = @"kZ'TUhcXJ\,-89\#?]NZkYD.5Ze@KxG(5=#&%{Lk(GnBW'X&";

        public static string Enrypt(this int num)
        {
            return BitConverter.GetBytes(num).Encrypt();
        }

        public static string Encrypt(this byte[] bytes)
        {
            Guard.ForNull(bytes, "bytes");

            return HttpServerUtility.UrlTokenEncode(MachineKey.Protect(bytes, _purpose));
        }

        public static int Decrypt(this string valorEncriptado)
        {
            if (string.IsNullOrEmpty(valorEncriptado)) return 0;

            return BitConverter.ToInt32(DecryptToBytes(valorEncriptado), 0);
        }

        private static byte[] DecryptToBytes(string valorEncriptado)
        {
            Guard.ForNullOrEmpty(valorEncriptado, "valorEncriptado");

            return MachineKey.Unprotect(HttpServerUtility.UrlTokenDecode(valorEncriptado));
        }
    }
}