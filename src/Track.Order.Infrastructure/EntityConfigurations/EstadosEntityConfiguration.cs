using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Track.Order.Domain.Entities;

namespace Track.Order.Infrastructure.EntityConfigurations;

public class EstadosEntityConfiguration : IEntityTypeConfiguration<Estados>
{
    public void Configure(EntityTypeBuilder<Estados> builder)
    {
        builder.HasKey(estado => estado.IdEstado);

        builder.Property(estado => estado.Descripcion);
    }
}
