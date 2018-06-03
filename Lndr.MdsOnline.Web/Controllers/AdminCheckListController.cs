using AutoMapper;
using Lndr.MdsOnline.Services;
using Lndr.MdsOnline.Web.Models.DTO.CheckList;
using Lndr.MdsOnline.Web.Models.ViewData.CheckList;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace Lndr.MdsOnline.Web.Controllers
{
    public class AdminCheckListController : BaseController
    {
        private readonly IMdsOnlineService _service;

        public AdminCheckListController(IMdsOnlineService service)
        {
            this._service = service;
        }

        [HttpGet]
        public ActionResult CheckList(int checklistId)
        {
            ViewData["checklistId"] = checklistId;
            return View("CheckList");
        }

        [HttpGet]
        public ActionResult ListaCheckLists()
        {
            return View("ListaCheckList");
        }

        [HttpPost]
        public ActionResult ObterListaCheckLists()
        {
            var model = Mapper.Map<List<CheckListViewData>>(this._service.ObterListaCheckLists());
        }

        [HttpPost]
        public ActionResult ObterCheckList(int checklistId)
        {
            var checklist = this._service.ObterCheckList(checklistId);
            var model = Mapper.Map<CheckListViewData>(checklist);
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult SalvarCheckList(CheckListViewData model)
        {
            if (!ModelState.IsValid)
            {
                base.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { camposComErros = base.ObterCamposComErros() });
            }

            var checklist = Mapper.Map<CheckListDTO>(model);

            //TODO Preencher usuarios corretamente
            checklist.UsuarioCriacaoID = base.UsuarioID;
            checklist.UsuarioAtualizacaoID = base.UsuarioID;

            this._service.SalvarCheckList(checklist);
            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }
    }
}