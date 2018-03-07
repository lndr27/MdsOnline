using Lndr.MdsOnline.Web.Helpers.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Lndr.MdsOnline.Web.Models.ViewData
{
    public class SolicitacaoRTUViewData
    {
        public int SolicitacaoRTUID { get; set; }

        public string Sequencia { get; set; }

        public string Condicao { get; set; }

        public string DadosEntrada { get; set; }

        public string ResultadoEsperado { get; set; }

        public string StatusVerificacaoTesteUnitarioID { get; set; }

        [Required]
        [RequiredConteudoHtml(ErrorMessage = "Campo \"Como Testar\" deve ser preenchido")]
        public string ComoTestar { get; set; }

        public string Observacoes { get; set; }

        [Required]
        public int Ordem { get; set; }

        public DateTime DataAtualizacao { get; set; }
    }
}