using Lndr.MdsOnline.Web.Models.ViewData;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Lndr.MdsOnline.Web.Controllers
{
    public class BaseController : Controller
    {
        private int _usuarioID;

        public int UsuarioID
        {
            get
            {
                // TODO: Carregar usuario
                return this._usuarioID = HttpContext.User.Identity.IsAuthenticated ? 1 : -1;
            }
        }

        public List<CampoViewData> ObterCamposComErros()
        {
            var result = new List<CampoViewData>();

            foreach (var val in ModelState)
            {
                if (val.Value.Errors.Count == 0) continue;

                result.Add(new CampoViewData
                {
                    Erros = val.Value.Errors.Select(e => e.ErrorMessage),
                    Campo = val.Key
                });                
            }
            return result;
        }
    }
}