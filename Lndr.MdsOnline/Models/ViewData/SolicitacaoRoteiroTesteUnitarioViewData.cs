using System.ComponentModel.DataAnnotations;

namespace Lndr.MdsOnline.Models.ViewData
{
    public class SolicitacaoRoteiroTesteUnitarioViewData
    {
        public int SolicitacaoRoteiroTesteUnitarioID { get; set; }

        [Required]
        public int SolicitacaoID { get; set; }

        public int Sequencia { get; set; }

        public string Condicao { get; set; }

        public string DadosEntrada { get; set; }

        public string ResultadoEsperado { get; set; }

        public int Verificacao { get; set; }

        [Required]
        public string ComoTestar { get; set; }

        public string Observacoes { get; set; }

        [Required]
        public int Ordem { get; set; }
    }
}