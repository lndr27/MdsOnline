using AutoMapper;
using Lndr.MdsOnline.Web.Models.Domain;
using Lndr.MdsOnline.Web.Models.ViewData;
using Lndr.MdsOnline.Services;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Lndr.MdsOnline.Web.Models.ViewData.Rtu;
using Lndr.MdsOnline.Web.Models.DTO.Rtu;

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
            var rtuDTO = this._service.ObterRtu(chamado);
            var rtuViewData = Mapper.Map<RtuViewData>(rtuDTO);
            return Json(rtuViewData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SalvarRTU(RtuViewData model)
        {
            if (!ModelState.IsValid)
            {
                base.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { camposComErros = base.ParseModelState() });
            }

            var rtu = Mapper.Map<RtuDTO>(model);

            //TODO REMOVER
            rtu.UsuarioID = 1;
            rtu.UsuarioAtualizacaoID = 1;
            rtu.UsuarioVerificacaoID = 1;

            this._service.SalvarRtu(rtu);
            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }
    }
}