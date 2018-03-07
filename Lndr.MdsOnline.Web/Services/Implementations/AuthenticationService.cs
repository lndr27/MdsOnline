using Lndr.MdsOnline.Web.Repositories;

namespace Lndr.MdsOnline.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private IServiceContext _context;

        private IMdsOnlineRepository _repository;

        public AuthenticationService(IServiceContext context, IMdsOnlineRepository repository)
        {
            this._context = context;
            this._repository = repository;
        }
    }
}