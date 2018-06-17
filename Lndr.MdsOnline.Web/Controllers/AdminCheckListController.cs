using AutoMapper;
using Lndr.MdsOnline.Services;
using Lndr.MdsOnline.Web.Models.ViewData.CheckList;
using System.Web.Mvc;

namespace Lndr.MdsOnline.Web.Controllers
{
    public class AdminCheckListController : Controller
    {
        private readonly IMdsOnlineService _service;

        public AdminCheckListController(IMdsOnlineService service)
        {
            this._service = service;
        }

        [HttpGet]
        public ActionResult ListaCheckLists()
        {
            return View("ListaCheckLists");
        }

        [HttpGet]
        public ActionResult CheckList(int checklistId)
        {
            ViewData["checklistId"] = checklistId;
            return View("CheckList");
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
            return null;
        }
    }
}