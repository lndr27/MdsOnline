namespace Lndr.MdsOnline.DataModel.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RtfTesteEvidencia")]
    public partial class RtfTesteEvidencia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RtfTesteEvidencia()
        {
            Historico = new HashSet<RtfTesteEvidenciaHistorico>();
        }

        public int RtfTesteEvidenciaID { get; set; }

        public int RtfTesteID { get; set; }

        public int TipoEvidenciaID { get; set; }

        public int ArquivoID { get; set; }

        public string Descricao { get; set; }

        public int Ordem { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public int UsuarioID { get; set; }

        public virtual Arquivo Arquivo { get; set; }

        public virtual RtfTeste RtfTeste { get; set; }

        public virtual Usuario Usuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RtfTesteEvidenciaHistorico> Historico { get; set; }
    }
}
