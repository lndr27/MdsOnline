namespace Lndr.MdsOnline.DataModel.Model
{
    using System.Data.Entity;

    public partial class MdsOnlineDbContext : DbContext
    {
        public MdsOnlineDbContext(string connString)
            : base(connString)
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

            modelBuilder.Entity<Arquivo>()
                .HasMany(e => e.RtfTesteEvidencia)
                .WithRequired(e => e.Arquivo)
                .WillCascadeOnDelete(false);

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
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<RtfTesteEvidencia>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

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

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.Arquivos)
                .WithRequired(e => e.Usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.RTFUsuario)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.UsuarioID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.RTFUsuarioVerificacao)
                .WithRequired(e => e.UsuarioVerificacao)
                .HasForeignKey(e => e.UsuarioVerificacaoID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.RtfTestes)
                .WithRequired(e => e.Usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.RtfTesteEvidencia)
                .WithRequired(e => e.Usuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.RTUUsuario)
                .WithRequired(e => e.Usuario)
                .HasForeignKey(e => e.UsuarioID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.RTUUsuarioVerificacao)
                .WithRequired(e => e.UsuarioVerificacao)
                .HasForeignKey(e => e.UsuarioVerificacaoID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.RtuTestes)
                .WithRequired(e => e.Usuario)
                .WillCascadeOnDelete(false);
        }

    }
}
