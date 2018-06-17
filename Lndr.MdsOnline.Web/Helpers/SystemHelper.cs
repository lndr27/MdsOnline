using System;
using System.Configuration;
using System.Web;
using System.Web.Security;

namespace Lndr.MdsOnline.Web.Helpers
{
    public static class SystemHelper
    {
        private static readonly string[] _purpose = new string[] { "XY@#$xfd__sfd- 6986df#$)<^&_)C" };

        public static string BDMdsConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["BDMdsOnline"].ConnectionString;
            }
        }

        public static string Encode(int input)
        {
            return HttpServerUtility.UrlTokenEncode(MachineKey.Protect(BitConverter.GetBytes(input), _purpose));
        }

        public static int DecodeInt(string input)
        {
            return BitConverter.ToInt32(MachineKey.Unprotect(HttpServerUtility.UrlTokenDecode(input), _purpose), 0);
        }
    }
}