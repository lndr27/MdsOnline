using AutoMapper;
using Lndr.MdsOnline.Services;
using Lndr.MdsOnline.Web.Models.DTO.RTF;
using Lndr.MdsOnline.Web.Models.ViewData.RTF;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using System.Linq;

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
                return Json(new { camposComErros = base.ParseModelState() });
            }

            var rtf = Mapper.Map<RtfDTO>(model);
            rtf.UsuarioID = base.UsuarioID;
            rtf.UsuarioVerificacaoID = base.UsuarioID;
            rtf.UsuarioAtualizacaoID = base.UsuarioID;

            this._service.SalvarRTF(rtf);
            return new HttpStatusCodeResult(HttpStatusCode.Created);
        }

        public Dictionary<string, string> GetValidationDefinition(object container, Type type)
        {
            var modelMetaData = System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(container, type);
            var mvcContext = new System.Web.Mvc.ControllerContext();
            var validationAttributes = new Dictionary<string, string>();
            foreach (var metaDataForProperty in modelMetaData)
            {
                var validationRulesForProperty = metaDataForProperty.GetValidators(mvcContext).SelectMany(v => v.GetClientValidationRules());
                foreach (System.Web.Mvc.ModelClientValidationRule rule in validationRulesForProperty)
                {
                    string key = metaDataForProperty.PropertyName + "-" + rule.ValidationType;
                    validationAttributes.Add(key, System.Web.HttpUtility.HtmlEncode(rule.ErrorMessage ?? string.Empty));
                    key = key + "-";
                    foreach (KeyValuePair<string, object> pair in rule.ValidationParameters)
                    {
                        validationAttributes.Add(key + pair.Key, System.Web.HttpUtility.HtmlAttributeEncode(pair.Value != null ? Convert.ToString(pair.Value, System.Globalization.CultureInfo.InvariantCulture) : string.Empty));
                    }
                }

            }
            return validationAttributes;
        }
    }
}