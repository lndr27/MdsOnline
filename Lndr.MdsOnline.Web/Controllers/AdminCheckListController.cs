using AutoMapper;
using Lndr.MdsOnline.Services;
using Lndr.MdsOnline.Web.Helpers;
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
        public ActionResult CheckList(string checklistId)
        {
            ViewData["checklistId"] = checklistId;
            return View("CheckList");
        }

        [HttpGet]
        public ActionResult ListaCheckLists()
        {
            return View("ListaCheckLists");
        }

        [HttpPost]
        public ActionResult ObterListaCheckLists(int pagina, int tamanhoPagina)
        {
            var pagination = this._service.ObterListaCheckLists(pagina, tamanhoPagina);
            var model = new ListaCheckListsViewData
            {
                CheckLists = Mapper.Map<List<CheckListViewData>>(pagination.Lista),
                Pagina = pagina,
                TamanhoPagina = tamanhoPagina,
                PaginaAnterior = pagination.PaginaAnterior,
                ProximaPagina = pagination.ProximaPagina,
                PossuiPaginaAnterior = pagination.PossuiPaginaAnterior,
                PossuiProximaPagina = pagination.PossuiProximaPagina,
                TotalPaginas = pagination.TotalPaginas
            };
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult ObterCheckList(string checklistId)
        {
            var checklist = this._service.ObterCheckList(SystemHelper.DecodeInt(checklistId));
            var model = Mapper.Map<CheckListViewData>(checklist);
            return Json(model, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult NovoCheckList()
        {
            return Json(new CheckListViewData(), JsonRequestBehavior.DenyGet);
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

            var checklistId = this._service.SalvarCheckList(checklist);
            base.Response.StatusCode = (int)HttpStatusCode.Created;
            return Json(new { checklistId = SystemHelper.Encode(checklistId) }, JsonRequestBehavior.DenyGet);
        }
    }
}