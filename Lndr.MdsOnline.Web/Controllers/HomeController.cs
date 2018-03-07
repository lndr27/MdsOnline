using Lndr.MdsOnline.Web.Helpers;
using Lndr.MdsOnline.Web.Models.ViewData;
using Lndr.MdsOnline.Services;
using System.Web.Mvc;

namespace Lndr.MdsOnline.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BarraMenu()
        {
            ViewBag.NomeUsuario = HttpContext.User.Identity.Name;
            return View();
        }
    }
}