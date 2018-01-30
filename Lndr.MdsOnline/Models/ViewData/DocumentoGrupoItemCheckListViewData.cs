using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lndr.MdsOnline.Models.ViewData
{
    public class DocumentoGrupoItemChecklistViewData
    {
        [Required]
        public int DocumentoGrupoItemCheckListID { get; set; }

        [Required]
        public int DocumentoID { get; set; }

        [Required]
        [MaxLength(200)]
        public string Nome { get; set; }

        [MaxLength(8000)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        public List<DocumentoGrupoItemChecklistOpcaoViewData> Opcoes { get; set; }

        public List<DocumentoItemChecklistViewData> Itens { get; set; }
    }
}