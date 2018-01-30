using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lndr.MdsOnline.Models.ViewData
{
    public class DocumentoViewData
    {
        [Required]
        public int DocumentoID { get; set; }

        [Required]
        [MaxLength(200)]
        public string Nome { get; set; }

        [MaxLength(8000)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required]
        [Display(Name = "Tipo Documento")]
        public int TipoDocumentoID { get; set; }

        public List<DocumentoGrupoItemChecklistViewData> GruposItemChecklist { get; set; }

        public List<DocumentoTopicoViewData> Topicos { get; set; }

    }
}