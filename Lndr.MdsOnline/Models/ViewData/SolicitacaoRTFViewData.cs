using System.Collections.Generic;

namespace Lndr.MdsOnline.Models.ViewData
{
    public class SolicitacaoRTFViewData
    {
        public int SolicitacaoRTFID { get; set; }

        public string Sequencia { get; set; }

        public string Funcionalidade { get; set; }

        public string CondicaoCenario { get; set; }

        public string PreCondicao { get; set; }

        public string DadosEntrada { get; set; }

        public string ResultadoEsperado { get; set; }

        public string Observacoes { get; set; }

        public string StatusExecucaoHomologacaoID { get; set; }

        public int Ordem { get; set; }

        public IEnumerable<SolicitacaoRTFEvidenciaViewData> Evidencias { get; set; }

        public IEnumerable<SolicitacaoRTFEvidenciaViewData> Erros { get; set; }
    }
}