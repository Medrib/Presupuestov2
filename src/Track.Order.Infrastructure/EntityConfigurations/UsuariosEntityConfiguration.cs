using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Track.Order.Domain.Entities;

namespace Track.Order.Infrastructure.EntityConfigurations;

public class UsuariosEntityConfiguration : IEntityTypeConfiguration<Usuarios>
{
    public void Configure(EntityTypeBuilder<Usuarios> builder)
    {
        builder.HasKey(usuario => usuario.IdUsuario);

        builder.Property(usuario => usuario.Nombre);
        builder.Property(usuario => usuario.CorreoElectronico);
        builder.Property(usuario => usuario.IdCliente);
    }
}
