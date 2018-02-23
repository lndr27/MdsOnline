namespace Lndr.MdsOnline.Models.Domain
{
    public class SolicitacaoRoteiroTesteFuncionalDomain
    {
        public int SolicitacaoRoteiroTesteFuncionalID { get; set; }

        public int SolicitacaoID { get; set; }

        public string Sequencia { get; set; }

        public string Funcionalidade { get; set; }

        public string Condicao { get; set; }

        public string PreCondicao { get; set; }

        public string DadosEntrada { get; set; }

        public string ResultadoEsperado { get; set; }

        public string Observacao { get; set; }

        public int ExecucaoHomologacaoID { get; set; }
    }
}