using System.ComponentModel.DataAnnotations;

namespace Lndr.MdsOnline.Models.ViewData
{
    public class DocumentoItemChecklistViewData
    {
        [Required]
        public int DocumentoItemCheckListID { get; set; }

        [Required]
        public int DocumentoGrupoItemCheckListID { get; set; }

        [Required]
        public string Texto { get; set; }

    }
}