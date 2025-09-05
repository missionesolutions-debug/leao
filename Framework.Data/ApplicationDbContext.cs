using Data.Models;
using Data.Models.Catalogo;
using Data.Models.Conteudo;
using Data.Models.Core;
using Data.Models.PainelConfig;
using Data.Models.Pessoa;
using Data.Models.Propriedade;
using Framework.Data.Models.Conteudo;
using Framework.Data.Models.Core;
using Framework.Data.Models.Factory;
using Framework.Data.Models.xCode;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Data
{
	public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            // Definir o esquema padrão como dbo
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<ChatIA>().ToTable("ChatIA");
            modelBuilder.Entity<ChatIAItem>().ToTable("ChatIAItem");

            modelBuilder.Entity<Acabamento>().ToTable("Acabamento");
            modelBuilder.Entity<Embalagem>().ToTable("Embalagem");
            modelBuilder.Entity<EmbalagemType>().ToTable("EmbalagemType");
            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<Supplier>().ToTable("Supplier");
            modelBuilder.Entity<ProjectStatus>().ToTable("ProjectStatus");
            modelBuilder.Entity<Project>().ToTable("Project");
            modelBuilder.Entity<ProjectUsuario>().ToTable("ProjectUsuario");
            modelBuilder.Entity<ProjectItem>().ToTable("ProjectItem");
            modelBuilder.Entity<ProjectPhase>().ToTable("ProjectPhase");
            modelBuilder.Entity<ProjectGroup>().ToTable("ProjectGroup");
            modelBuilder.Entity<ProjectBlock>().ToTable("ProjectBlock");
            modelBuilder.Entity<ProjectMeasureItem>().ToTable("ProjectMeasureItem");
            modelBuilder.Entity<ProjectImage>().ToTable("ProjectImage");
            modelBuilder.Entity<ProjectTemplate>().ToTable("ProjectTemplate");

            // Configuração de Metadata
            modelBuilder.Entity<Metadata>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Section>()
                  .HasMany(s => s.SectionTranslations)
                  .WithOne(t => t.Section)
                  .HasForeignKey(t => t.SectionId)
                  .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ChatIA> ChatIAs { get; set; }
        public DbSet<ChatIAItem> ChatIAItems { get; set; }
        public DbSet<ProjectTemplate> ProjectTemplates { get; set; }
        public DbSet<Acabamento> Acabamentos { get; set; }
        public DbSet<Embalagem> Embalagems { get; set; }
        public DbSet<EmbalagemType> EmbalagemTypes { get; set; }
        public DbSet<Client> Clients { get; set; }
		public DbSet<Supplier> Suppliers { get; set; }
		public DbSet<ProjectStatus> ProjectStatuses { get; set; }
		public DbSet<Project> Projects { get; set; }
		public DbSet<ProjectUsuario> ProjectUsuarios { get; set; }
		public DbSet<ProjectItem> ProjectItems { get; set; }
		public DbSet<ProjectPhase> ProjectPhases { get; set; }
		public DbSet<ProjectGroup> ProjectGroups { get; set; }
		public DbSet<ProjectBlock> ProjectBlocks { get; set; }
		public DbSet<ProjectMeasureItem> ProjectMeasureItems { get; set; }
		public DbSet<ProjectImage> ProjectImages { get; set; }


		public DbSet<PageSection> PageSection { get; set; }
        public DbSet<PageSectionItem> PageSectionItem { get; set; }
        public DbSet<Page> Page { get; set; }
        public DbSet<Section> Section { get; set; }
        public DbSet<SectionTranslation> SectionTranslation { get; set; }
        public DbSet<Metadata> Metadata { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<Arquivo> Arquivo { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Imagem> Imagem { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Equipe> Equipe { get; set; }
        public DbSet<Pagina> Pagina { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<TipoCategoria> TipoCategoria { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<SubCategoria> SubCategoria { get; set; }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + "/../WebApplication/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DatabaseConnection");
            builder.UseSqlServer(connectionString);
            return new ApplicationDbContext(builder.Options);
        }
    }
}
