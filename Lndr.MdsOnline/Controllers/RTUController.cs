using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lndr.MdsOnline.Controllers
{
    public class RTUController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NovoRTU()
        {
            return View("RTU");
        }
    }
}