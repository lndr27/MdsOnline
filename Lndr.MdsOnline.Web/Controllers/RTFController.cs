using AutoMapper;
using Lndr.MdsOnline.Web.Models.DTO;
using Lndr.MdsOnline.Services;
using Lndr.MdsOnline.Web.Models.ViewData.RTF;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Lndr.MdsOnline.Web.Models.DTO.RTF;

namespace Lndr.MdsOnline.Web.Controllers
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


        public ActionResult ObterRTF(int chamado)
        {
            var rtf = this._service.GetRTF(chamado);
            var model = Mapper.Map<RTFViewData>(rtf);
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

            var rtf = Mapper.Map<List<SolicitacaoRTFDTO>>(model.Testes);
            this._service.SalvarRTF(rtf, model.Chamado);
            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }

        [HttpPost]
        public ActionResult SaveRTF(RTFViewData rtf)
        {
            var x = Mapper.Map<RtfDTO>(rtf);
            this._service.SaveRTF(x);
            return null;
        }
    }
}