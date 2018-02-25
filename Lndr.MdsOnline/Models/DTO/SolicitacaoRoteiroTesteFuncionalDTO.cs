using System.Collections.Generic;

namespace Lndr.MdsOnline.Models.DTO
{
    public class SolicitacaoRoteiroTesteFuncionalDTO
    {
        public int SolicitacaoRoteiroTesteFuncionalID { get; set; }

        public string Sequencia { get; set; }

        public string Funcionalidade { get; set; }

        public string CondicaoCenario { get; set; }

        public string PreCondicao { get; set; }

        public string DadosEntrada { get; set; }

        public string ResultadoEsperado { get; set; }

        public string Observacoes { get; set; }

        public int StatusExecucaoHomologacaoID { get; set; }

        public int Ordem { get; set; }

        public IEnumerable<SolicitacaoRoteiroTesteFuncionalEvidenciaDTO> Evidencias { get; set; }
    }
}