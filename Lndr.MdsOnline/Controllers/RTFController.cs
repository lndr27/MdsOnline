using AutoMapper;
using Lndr.MdsOnline.Models.Domain;
using Lndr.MdsOnline.Models.DTO;
using Lndr.MdsOnline.Models.ViewData;
using Lndr.MdsOnline.Services;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace Lndr.MdsOnline.Controllers
{
    public class RTFController : BaseController
    {
        private readonly IMdsOnlineService _service;

        public RTFController(IMdsOnlineService service)
        {
            this._service = service;
        }

        [HttpGet]
        public ActionResult RTF(int chamado)
        {
            base.ViewData["chamado"] = chamado;
            return View("RTF");
        }

        [HttpGet]
        public ActionResult ObterRTF(int chamado)
        {
            var testesDomain = this._service.ObterRTF(chamado);
            var testesViewData = Mapper.Map<List<SolicitacaoRoteiroTesteFuncionalViewData>>(testesDomain);
            var model = new RTFViewData
            {
                Testes = testesViewData
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SalvarRTF(RTFViewData model)
        {
            if (!ModelState.IsValid)
            {
                base.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { camposComErros = base.ParseModelState() });
            }

            var rtf = Mapper.Map<List<SolicitacaoRoteiroTesteFuncionalDTO>>(model.Testes);
            this._service.SalvarRTF(rtf, model.Chamado);
            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }
    }
}