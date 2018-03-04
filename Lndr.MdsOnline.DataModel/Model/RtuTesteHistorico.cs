namespace Lndr.MdsOnline.DataModel.Model
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RtuTesteHistorico")]
    public partial class RtuTesteHistorico
    {
        public int RtuTesteHistoricoID { get; set; }

        public int RtuTesteID { get; set; }

        public int RtuID { get; set; }

        public string Sequencia { get; set; }

        public string Condicao { get; set; }

        public string DadosEntrada { get; set; }

        public string ResultadoEsperado { get; set; }

        public int StatusVerificacaoTesteUnitarioID { get; set; }

        public string ComoTestar { get; set; }

        public string Observacoes { get; set; }

        public int Ordem { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public int UsuarioID { get; set; }

        public virtual RtuTeste RtuTeste { get; set; }
    }
}
