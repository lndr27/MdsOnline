using System.ComponentModel.DataAnnotations;

namespace Lndr.MdsOnline.Models.ViewData
{
    public class DocumentoTopicoViewData
    {
        [Required]
        public int DocumentoTopicoID { get; set; }

        [Required]
        public int DocumentoID { get; set; }

        [Required]
        [MaxLength(200)]
        public string Nome { get; set; }

        [Display(Name = "Descrição")]
        [MaxLength(8000)]
        public string Descricao { get; set; }

        [Required]
        [Display(Name = "Obrigatório")]
        public bool Obrigatorio { get; set; }
    }
}