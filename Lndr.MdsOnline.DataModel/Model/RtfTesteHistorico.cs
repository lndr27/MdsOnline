namespace Lndr.MdsOnline.DataModel.Model
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RtfTesteHistorico")]
    public partial class RtfTesteHistorico
    {
        public int RtfTesteHistoricoID { get; set; }

        public int RtfTesteID { get; set; }

        public int RtfID { get; set; }

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

        public virtual RtfTeste RtfTeste { get; set; }
    }
}
