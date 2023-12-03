using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tarefas.API.Domain.Entities;

namespace Tarefas.API.Infra.Data.EntityConfiguration
{
    public class HistoricoConfigurationMap : IEntityTypeConfiguration<Historico>
    {
        public void Configure(EntityTypeBuilder<Historico> builder)
        {
            builder.HasKey(h => h.Id);

            builder.Property(h => h.ValorAnterior)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(h => h.ValorAtual)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(h => h.DataModificacao)
                   .IsRequired();

            builder.HasOne(h => h.Tarefa)
                   .WithMany(h => h.Historicos)
                   .HasForeignKey(h => h.TarefaId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.HasOne(h => h.Usuario)
                   .WithMany()
                   .HasForeignKey(h => h.UsuarioId)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired();
        }
    }
}
