using Lndr.MdsOnline.Web.Models.ViewData;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Lndr.MdsOnline.Web.Controllers
{
    public class BaseController : Controller
    {
        public List<CampoViewData> ParseModelState()
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