using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tarefas.API.Domain.Entities;

namespace Tarefas.API.Infra.Data.EntityConfiguration
{
    public class UsuarioConfigurationMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Nome)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.HasMany(u => u.Projetos)
                   .WithOne(p => p.Usuario)
                   .HasForeignKey(p => p.UsuarioId)
                   .IsRequired();
        }
    }
}
