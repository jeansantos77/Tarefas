using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tarefas.API.Domain.Entities;

namespace Tarefas.API.Infra.Data.EntityConfiguration
{
    public class ProjetoConfigurationMap : IEntityTypeConfiguration<Projeto>
    {
        public void Configure(EntityTypeBuilder<Projeto> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(150);

            builder.HasOne(p => p.Usuario)
               .WithMany()
               .HasForeignKey(p => p.UsuarioId)
               .OnDelete(DeleteBehavior.NoAction)
               .IsRequired();

            builder.HasMany(p => p.Tarefas)
               .WithOne()
               .HasForeignKey(t => t.ProjetoId)
               .OnDelete(DeleteBehavior.NoAction)
               .IsRequired();


        }
    }
}
