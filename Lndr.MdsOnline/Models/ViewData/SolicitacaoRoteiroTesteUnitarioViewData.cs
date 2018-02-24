using Lndr.MdsOnline.Helpers.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Lndr.MdsOnline.Models.ViewData
{
    public class SolicitacaoRoteiroTesteUnitarioViewData
    {
        public int SolicitacaoRoteiroTesteUnitarioID { get; set; }

        public int SolicitacaoID { get; set; }

        public string Sequencia { get; set; }

        public string Condicao { get; set; }

        public string DadosEntrada { get; set; }

        public string ResultadoEsperado { get; set; }

        public string Verificacao { get; set; }

        [Required]
        [RequiredConteudoHtml(ErrorMessage = "Campo \"Como Testar\" deve ser preenchido")]
        public string ComoTestar { get; set; }

        public string Observacoes { get; set; }

        [Required]
        public int Ordem { get; set; }
    }
}