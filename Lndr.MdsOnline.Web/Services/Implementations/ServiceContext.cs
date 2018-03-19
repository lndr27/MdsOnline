using System.Security.Principal;
using System.Web;

namespace Lndr.MdsOnline.Services
{
    public class ServiceContext : IServiceContext
    {
        private WindowsIdentity _identity;

        public WindowsIdentity Identity
        {
            get
            {
                if (HttpContext.Current.User == null) return null;

                return this._identity = this._identity ?? HttpContext.Current.User.Identity as WindowsIdentity;
            }
        }

        public int UsuarioID
        {
            get
            {
                return this.Identity != null ? 1 : -1;
            }
        }

        public string CodigoUsuario
        {
            get
            {
                return this.Identity != null ? this.Identity.Name : null;
            }
        }

        public string NomeUsuario
        {
            get
            {
                return this.Identity != null ? this.Identity.Name : null;
            }
        }
    }
}