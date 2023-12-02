using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tarefas.API.Domain.Entities;

namespace Tarefas.API.Infra.Data.EntityConfiguration
{
    public class TarefaConfigurationMap : IEntityTypeConfiguration<Tarefa>
    {
        public void Configure(EntityTypeBuilder<Tarefa> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Titulo)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(t => t.Descricao)
                   .HasMaxLength(1000);

            builder.Property(t => t.Vencimento)
                   .IsRequired();

            builder.Property(t => t.Status)
                   .IsRequired();

            builder.Property(t => t.Prioridade)
                   .IsRequired();

            builder.HasOne(t => t.Projeto)
                   .WithMany(p => p.Tarefas)
                   .HasForeignKey(t => t.ProjetoId)
                   .IsRequired();

            builder.HasOne(t => t.Usuario)
                   .WithMany()
                   .HasForeignKey(t => t.UsuarioId)
                   .IsRequired();
        }
    }
}
