using System.Security.Principal;
using System.Web;

namespace Lndr.MdsOnline.Services
{
    public class ServiceContext : IServiceContext
    {
        private GenericIdentity _identity;

        public ServiceContext()
        {
            this._identity = this._identity ?? HttpContext.Current.User.Identity as GenericIdentity;
        }

        public int UsuarioID
        {
            get
            {
                return this._identity != null ? 1 : -1;
            }
        }

        public string CodigoUsuario
        {
            get
            {
                return this._identity != null ? this._identity.Name : null;
            }
        }

        public string NomeUsuario
        {
            get
            {
                return this._identity != null ? this._identity.Name : null;
            }
        }
    }
}