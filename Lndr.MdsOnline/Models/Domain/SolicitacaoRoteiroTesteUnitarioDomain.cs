namespace Lndr.MdsOnline.Models.Domain
{
    public class SolicitacaoRoteiroTesteUnitarioDomain
    {
        public int SolicitacaoRoteiroTesteUnitarioID { get; set; }

        public int SolicitacaoID { get; set; }

        public int Sequencia { get; set; }

        public string Condicao { get; set; }

        public string DadosEntrada { get; set; }

        public string ResultadoEsperado { get; set; }

        public int Verificacao { get; set; }

        public string ComoTestar { get; set; }

        public string Observacoes { get; set; }

        public int Ordem { get; set; }
    }
}