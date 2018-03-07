using System.Configuration;

namespace Lndr.MdsOnline.Web.Helpers
{
    public static class SystemHelper
    {
        public static string BDMdsConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["BDMdsOnline"].ConnectionString;
            }
        }
    }
}