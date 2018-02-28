using System.Web.Mvc.Filters;

namespace Lndr.MdsOnline.Helpers.Filters
{
    public class AuthenticationFilter : IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }
}