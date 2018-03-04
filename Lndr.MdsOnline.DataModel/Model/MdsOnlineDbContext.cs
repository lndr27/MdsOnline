namespace Lndr.MdsOnline.DataModel.Model
{
    using System.Data.Entity;

    public partial class MdsOnlineDbContext : DbContext
    {
        public MdsOnlineDbContext()
            : base("name=MdsOnlineDbContext")
        {
        }

        public virtual DbSet<Arquivo> Arquivo { get; set; }
        public virtual DbSet<RTF> RTF { get; set; }
        public virtual DbSet<RtfHistorico> RtfHistorico { get; set; }
        public virtual DbSet<RtfTeste> RtfTeste { get; set; }
        public virtual DbSet<RtfTesteEvidencia> RtfTesteEvidencia { get; set; }
        public virtual DbSet<RtfTesteEvidenciaHistorico> RtfTesteEvidenciaHistorico { get; set; }
        public virtual DbSet<RtfTesteHistorico> RtfTesteHistorico { get; set; }
        public virtual DbSet<RTU> RTU { get; set; }
        public virtual DbSet<RtuHistorico> RtuHistorico { get; set; }
        public virtual DbSet<RtuTeste> RtuTeste { get; set; }
        public virtual DbSet<RtuTesteHistorico> RtuTesteHistorico { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Arquivo>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Arquivo>()
                .Property(e => e.Extensao)
                .IsUnicode(false);

            modelBuilder.Entity<Arquivo>()
                .Property(e => e.ContentType)
                .IsUnicode(false);

            modelBuilder.Entity<RTF>()
                .HasMany(e => e.Historico)
                .WithRequired(e => e.RTF)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RTF>()
                .HasMany(e => e.Testes)
                .WithRequired(e => e.RTF)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RtfTeste>()
                .Property(e => e.Sequencia)
                .IsUnicode(false);

            modelBuilder.Entity<RtfTeste>()
                .Property(e => e.Funcionalidade)
                .IsUnicode(false);

            modelBuilder.Entity<RtfTeste>()
                .Property(e => e.CondicaoCenario)
                .IsUnicode(false);

            modelBuilder.Entity<RtfTeste>()
                .Property(e => e.PreCondicao)
                .IsUnicode(false);

            modelBuilder.Entity<RtfTeste>()
                .Property(e => e.DadosEntrada)
                .IsUnicode(false);

            modelBuilder.Entity<RtfTeste>()
                .Property(e => e.ResultadoEsperado)
                .IsUnicode(false);

            modelBuilder.Entity<RtfTeste>()
                .Property(e => e.Observacoes)
                .IsUnicode(false);

            modelBuilder.Entity<RtfTeste>()
                .HasMany(e => e.Evidencias)
                .WithRequired(e => e.RtfTeste)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RtfTeste>()
                .HasMany(e => e.Historico)
                .WithRequired(e => e.RtfTeste)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RtfTesteEvidencia>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<RtfTesteEvidencia>()
                .HasMany(e => e.Historico)
                .WithRequired(e => e.RtfTesteEvidencia)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RtfTesteEvidenciaHistorico>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<RtfTesteHistorico>()
                .Property(e => e.Sequencia)
                .IsUnicode(false);

            modelBuilder.Entity<RtfTesteHistorico>()
                .Property(e => e.Funcionalidade)
                .IsUnicode(false);

            modelBuilder.Entity<RtfTesteHistorico>()
                .Property(e => e.CondicaoCenario)
                .IsUnicode(false);

            modelBuilder.Entity<RtfTesteHistorico>()
                .Property(e => e.PreCondicao)
                .IsUnicode(false);

            modelBuilder.Entity<RtfTesteHistorico>()
                .Property(e => e.DadosEntrada)
                .IsUnicode(false);

            modelBuilder.Entity<RtfTesteHistorico>()
                .Property(e => e.ResultadoEsperado)
                .IsUnicode(false);

            modelBuilder.Entity<RtfTesteHistorico>()
                .Property(e => e.Observacoes)
                .IsUnicode(false);

            modelBuilder.Entity<RTU>()
                .HasMany(e => e.Historico)
                .WithRequired(e => e.RTU)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RTU>()
                .HasMany(e => e.Testes)
                .WithRequired(e => e.RTU)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RtuTeste>()
                .Property(e => e.Sequencia)
                .IsUnicode(false);

            modelBuilder.Entity<RtuTeste>()
                .Property(e => e.Condicao)
                .IsUnicode(false);

            modelBuilder.Entity<RtuTeste>()
                .Property(e => e.DadosEntrada)
                .IsUnicode(false);

            modelBuilder.Entity<RtuTeste>()
                .Property(e => e.ResultadoEsperado)
                .IsUnicode(false);

            modelBuilder.Entity<RtuTeste>()
                .Property(e => e.ComoTestar)
                .IsUnicode(false);

            modelBuilder.Entity<RtuTeste>()
                .Property(e => e.Observacoes)
                .IsUnicode(false);

            modelBuilder.Entity<RtuTeste>()
                .HasMany(e => e.RtuTesteHistorico)
                .WithRequired(e => e.RtuTeste)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RtuTesteHistorico>()
                .Property(e => e.Sequencia)
                .IsUnicode(false);

            modelBuilder.Entity<RtuTesteHistorico>()
                .Property(e => e.Condicao)
                .IsUnicode(false);

            modelBuilder.Entity<RtuTesteHistorico>()
                .Property(e => e.DadosEntrada)
                .IsUnicode(false);

            modelBuilder.Entity<RtuTesteHistorico>()
                .Property(e => e.ResultadoEsperado)
                .IsUnicode(false);

            modelBuilder.Entity<RtuTesteHistorico>()
                .Property(e => e.ComoTestar)
                .IsUnicode(false);

            modelBuilder.Entity<RtuTesteHistorico>()
                .Property(e => e.Observacoes)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Codigo)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Email)
                .IsUnicode(false);
        }
    }
}
