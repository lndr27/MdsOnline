using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Lndr.MdsOnline.Models.ViewData
{
    public class DocumentoViewData
    {
        public int DocumentoID { get; set; }

        public string Nome { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Tipo Documento")]
        public int TipoDocumentoID { get; set; }

        public List<SelectListItem> TipoDocumentoLista { get; set; }

        public IEnumerable<DocumentoGrupoItemCheckListViewData> GruposItemChecklist { get; set; }

        public IEnumerable<DocumentoTopicoViewData> Topicos { get; set; }

    }
}