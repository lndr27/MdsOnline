using System;

namespace Lndr.MdsOnline.Models.Domain
{
    public class SolicitacaoRTFDomain
    {
        public int SolicitacaoRTFID { get; set; }

        public int SolicitacaoID { get; set; }

        public string Sequencia { get; set; }

        public string Funcionalidade { get; set; }

        public string CondicaoCenario { get; set; }

        public string PreCondicao { get; set; }

        public string DadosEntrada { get; set; }

        public string ResultadoEsperado { get; set; }

        public string Observacoes { get; set; }

        public int StatusExecucaoHomologacaoID { get; set; }

        public int Ordem { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public int UsuarioID { get; set; }
    }
}