namespace Lndr.MdsOnline.DataModel.Model
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RtfTesteEvidenciaHistorico")]
    public partial class RtfTesteEvidenciaHistorico
    {
        public int RtfTesteEvidenciaHistoricoID { get; set; }

        public int RtfTesteEvidenciaID { get; set; }

        public int RtfTesteID { get; set; }

        public int TipoEvidenciaID { get; set; }

        public int ArquivoID { get; set; }

        public string Descricao { get; set; }

        public int Ordem { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public int UsuarioID { get; set; }

        public virtual RtfTesteEvidencia Evidencia { get; set; }
    }
}
