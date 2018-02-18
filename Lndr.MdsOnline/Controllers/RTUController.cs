using AutoMapper;
using Lndr.MdsOnline.Models.ViewData;
using Lndr.MdsOnline.Services;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Lndr.MdsOnline.Controllers
{
    public class RTUController : Controller
    {
        private readonly IMdsOnlineService _service;

        public RTUController(IMdsOnlineService service)
        {
            this._service = service;
        }

        public ActionResult RTU(int chamado)
        {
            base.ViewData["chamado"] = chamado;
            return View("RTU");
        }

        [HttpGet]
        public JsonResult ObterRTU(int chamado)
        {
            var testesDomain = this._service.ObterRtu(chamado);
            var testesViewData = Mapper.Map<List<SolicitacaoRoteiroTesteUnitarioViewData>>(testesDomain);

            var model = new RTUViewData
            {
                Testes = testesViewData
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvarRTU(RTUViewData model)
        {
            return null;
        }
    }
}