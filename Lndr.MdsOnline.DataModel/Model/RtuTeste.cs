namespace Lndr.MdsOnline.DataModel.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RtuTeste")]
    public partial class RtuTeste
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RtuTeste()
        {
            RtuTesteHistorico = new HashSet<RtuTesteHistorico>();
        }

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

        public virtual RTU RTU { get; set; }

        public virtual Usuario Usuario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RtuTesteHistorico> RtuTesteHistorico { get; set; }
    }
}
