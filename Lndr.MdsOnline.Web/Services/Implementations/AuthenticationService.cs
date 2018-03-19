using Lndr.MdsOnline.Web.Repositories;

namespace Lndr.MdsOnline.Services
{
    public class AuthenticationService : BaseRepository, IAuthenticationService
    {
        private IServiceContext _context;

        public AuthenticationService(IServiceContext context)
        {
            this._context = context;
        }
    }
}