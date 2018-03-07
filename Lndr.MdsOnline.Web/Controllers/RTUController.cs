using AutoMapper;
using Lndr.MdsOnline.Web.Models.Domain;
using Lndr.MdsOnline.Web.Models.ViewData;
using Lndr.MdsOnline.Services;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace Lndr.MdsOnline.Web.Controllers
{
    public class RTUController : BaseController
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
            var testesDomain = this._service.ObterRTU(chamado);
            var testesViewData = Mapper.Map<List<SolicitacaoRTUViewData>>(testesDomain);
            var model = new RTUViewData
            {
                Testes = testesViewData
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SalvarRTU(RTUViewData model)
        {
            if (!ModelState.IsValid)
            {
                base.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { camposComErros = base.ParseModelState() });
            }

            var rtu = Mapper.Map<List<SolicitacaoRTUDomain>>(model.Testes);
            this._service.SalvarRTU(rtu, model.Chamado);
            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }
    }
}