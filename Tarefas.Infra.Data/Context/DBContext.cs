using Microsoft.EntityFrameworkCore;
using Tarefas.API.Domain.Entities;
using Tarefas.API.Infra.Data.EntityConfiguration;

namespace Tarefas.API.Infra.Data.Context
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base (options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=DESKTOP-AIP5M5I;database=DbTarefas;trusted_connection=true;");
                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ProjetoConfigurationMap());
            builder.ApplyConfiguration(new TarefaConfigurationMap());
            builder.ApplyConfiguration(new UsuarioConfigurationMap());

            base.OnModelCreating(builder);
        }
    }
}
