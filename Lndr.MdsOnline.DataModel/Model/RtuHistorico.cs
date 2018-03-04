namespace Lndr.MdsOnline.DataModel.Model
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RtuHistorico")]
    public partial class RtuHistorico
    {
        public int RtuHistoricoID { get; set; }

        public int RtuID { get; set; }

        public int SolicitacaoID { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public int UsuarioID { get; set; }

        public int UsuarioVerificacaoID { get; set; }

        public virtual RTU RTU { get; set; }
    }
}
