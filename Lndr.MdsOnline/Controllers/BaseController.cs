using Lndr.MdsOnline.Models.ViewData;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace Lndr.MdsOnline.Controllers
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