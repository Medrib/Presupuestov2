using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Track.Order.Domain.Entities;

namespace Track.Order.Infrastructure.EntityConfigurations
{
    public class UsuariosEntityConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(estado => estado.IDUsuario);
            builder.Property(estado => estado.Nombre);
            builder.Property(estado => estado.CorreoElectronico);
        }
    }
}