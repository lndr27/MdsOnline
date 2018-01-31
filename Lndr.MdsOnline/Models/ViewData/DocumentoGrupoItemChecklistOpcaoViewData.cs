using System.ComponentModel.DataAnnotations;

namespace Lndr.MdsOnline.Models.ViewData
{
    public class DocumentoGrupoItemChecklistOpcaoViewData
    {
        [Required]
        public int DocumentoGrupoItemChecklistOpcaoID { get; set; }

        [Required]
        public int DocumentoGrupoItemChecklistID { get; set; }

        [Required]
        [MaxLength(200)]
        public string Nome { get; set; }

        [MaxLength(8000)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required]
        public int Ordem { get; set; }

        [Required]
        [Display(Name = "Obrigatório")]
        public bool Obrigatorio { get; set; }

        [Required]
        [Display(Name = "Tipo do Campo")]
        public string DataType { get; set; }

        [Display(Name = "Valores Válidos")]
        public string[] ValoresValidos { get; set; }

        [Required]
        [Display(Name = "Função")]
        public string Funcao { get; set; }
    }
}