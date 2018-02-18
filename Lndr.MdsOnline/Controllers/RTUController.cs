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

        public ActionResult NovoRTU()
        {
            return View("RTU");
        }

        [HttpGet]
        public JsonResult ObterRTU(int chamado)
        {
            var rtu = this._service.ObterRtu(chamado);
            var rtuViewData = Mapper.Map<SolicitacaoRoteiroTesteUnitarioViewData>(rtu);
            return Json(rtuViewData);
        }

        [HttpPost]
        public JsonResult SalvarRTU(IEnumerable<SolicitacaoRoteiroTesteUnitarioViewData> rtu)
        {
            return null;
        }
    }
}