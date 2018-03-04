namespace Lndr.MdsOnline.DataModel.Model
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RtfHistorico")]
    public partial class RtfHistorico
    {
        public int RtfHistoricoID { get; set; }

        public int RtfID { get; set; }

        public int SolicitacaoID { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public int UsuarioID { get; set; }

        public int UsuarioVerificacaoID { get; set; }

        public virtual RTF RTF { get; set; }
    }
}
