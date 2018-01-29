using Lndr.MdsOnline.Models.ViewData;
using System.Web.Mvc;

namespace Lndr.MdsOnline.Controllers
{
    public class AdminDocumentacaoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NovoDocumento()
        {
            return View(new DocumentoViewData());
        }
    }
}