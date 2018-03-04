using System.Configuration;

namespace Lndr.MdsOnline.Helpers
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