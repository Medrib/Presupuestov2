

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Track.Order.Domain.Entities;

namespace Track.Order.Infrastructure.EntityConfigurations;

public class UsuarioRolEntityConfiguration : IEntityTypeConfiguration<UsuarioRol>
{

    public void Configure(EntityTypeBuilder<UsuarioRol> builder)
    {
        builder.HasKey(usuario => usuario.IdUsuario);

        builder.Property(usuario => usuario.IdRol);
       
    }
}
