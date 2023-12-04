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
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Historico> Historicos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=localhost, 1450;database=DbTarefas;User Id=sa;Password=Senha@2023;");
                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProjetoConfigurationMap());
            modelBuilder.ApplyConfiguration(new TarefaConfigurationMap());
            modelBuilder.ApplyConfiguration(new UsuarioConfigurationMap());
            modelBuilder.ApplyConfiguration(new ComentarioConfigurationMap());
            modelBuilder.ApplyConfiguration(new HistoricoConfigurationMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
