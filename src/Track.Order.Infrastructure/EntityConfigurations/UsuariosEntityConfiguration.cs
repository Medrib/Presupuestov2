using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Track.Order.Domain.Entities;

namespace Track.Order.Infrastructure.EntityConfigurations;

public class UsuariosEntityConfiguration : IEntityTypeConfiguration<CategoriaGasto>
{
    public void Configure(EntityTypeBuilder<CategoriaGasto> builder)
    {
        builder.HasKey(usuario => usuario.IDCategoriaGasto);

        builder.Property(usuario => usuario.Nombre);
        builder.Property(usuario => usuario.Descripcion);
    }
}