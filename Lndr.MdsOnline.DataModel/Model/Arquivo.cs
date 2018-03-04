namespace Lndr.MdsOnline.DataModel.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Arquivo")]
    public partial class Arquivo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Arquivo()
        {
        }

        public int ArquivoID { get; set; }

        public Guid Guid { get; set; }

        [StringLength(1000)]
        public string Nome { get; set; }

        [StringLength(100)]
        public string Extensao { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }

        public double? TamanhoKB { get; set; }

        [Column("Arquivo")]
        public byte[] Arquivo1 { get; set; }

        public bool IsRascunho { get; set; }

        public DateTime DataUpload { get; set; }

        public int UsuarioID { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
