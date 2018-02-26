using System.Web.Mvc;

namespace Lndr.MdsOnline.Controllers
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