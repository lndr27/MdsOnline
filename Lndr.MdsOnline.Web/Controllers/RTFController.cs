using AutoMapper;
using Lndr.MdsOnline.Services;
using Lndr.MdsOnline.Web.Models.DTO.RTF;
using Lndr.MdsOnline.Web.Models.ViewData.RTF;
using System.Net;
using System.Web.Mvc;

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

        [HttpGet]
        public ActionResult ObterRTF(int chamado)
        {
            var rtf = this._service.ObterRTF(chamado);
            var model = Mapper.Map<RtfViewData>(rtf);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SalvarRTF(RtfViewData model)
        {
            if (!ModelState.IsValid)
            {
                base.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { camposComErros = base.ObterCamposComErros() });
            }

            var rtf = Mapper.Map<RtfDTO>(model);

            //TODO Preencher usuarios corretamente
            rtf.UsuarioID = base.UsuarioID;
            rtf.UsuarioVerificacaoID = base.UsuarioID;
            rtf.UsuarioAtualizacaoID = base.UsuarioID;

            this._service.SalvarRTF(rtf);
            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }
    }
}