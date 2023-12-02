using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tarefas.API.Domain.Entities;

namespace Tarefas.API.Infra.Data.EntityConfiguration
{
    public class ComentarioConfigurationMap : IEntityTypeConfiguration<Comentario>
    {
        public void Configure(EntityTypeBuilder<Comentario> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Descricao)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.HasOne(t => t.Tarefa)
                   .WithMany(p => p.Comentarios)
                   .HasForeignKey(t => t.TarefaId)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired();
        }
    }
}
