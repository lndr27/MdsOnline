using Lndr.MdsOnline.Web.Models.DTO;
using Lndr.MdsOnline.Services;
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Lndr.MdsOnline.Web.Controllers
{
    public class FileApiController : BaseController
    {
        private readonly IMdsOnlineService _service;

        public FileApiController(IMdsOnlineService service)
        {
            this._service = service;
        }

        [HttpGet]
        public ActionResult GetArquivo(string guid)
        {
            if (string.IsNullOrWhiteSpace(guid) || !Guid.TryParse(guid, out Guid result))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            var arquivo = this._service.ObterArquivo(guid);
            if (arquivo != null)
            {
                return File(arquivo.Arquivo, arquivo.ContentType, arquivo.Nome);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        public ActionResult UploadRascunho()
        {
            return this.Upload(true);
        }

        [HttpPost]
        public ActionResult UploadArquivo()
        {
            return this.Upload(false);
        }

        private ActionResult Upload(bool isRascunho)
        {
            try
            {
                if (Request.Files.Count == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var file = Request.Files[0];
                var guid = Guid.NewGuid();
                var arquivo = new ArquivoDTO
                {
                    Arquivo = this.ObterBytesArquivo(file),
                    ContentType = file.ContentType,
                    TamanhoKb = file.ContentLength / 1000f,
                    Guid = guid,
                    Nome = file.FileName,
                    IsRascunho = isRascunho,
                    Extensao = Path.GetExtension(file.FileName),
                    DataUpload = DateTime.Now

                };
                this._service.UploadArquivo(arquivo);
                return Json(new { guid = guid.ToString("D") });
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        public ActionResult Delete(string guid)
        {
            try
            {
                this._service.ApagarArquivo(guid);
                return new HttpStatusCodeResult((int)HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
            }            
        }

        private byte[] ObterBytesArquivo(HttpPostedFileBase file)
        {
            using (var inputsStream = file.InputStream)
            using (var memoryStream = new MemoryStream())
            {
                inputsStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}