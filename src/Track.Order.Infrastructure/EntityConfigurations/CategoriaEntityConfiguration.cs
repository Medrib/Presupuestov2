using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Track.Order.Domain.Entities;

namespace Track.Order.Infrastructure.EntityConfigurations
{
    public class CategoriaEntityConfiguration : IEntityTypeConfiguration<CategoriaGasto>
    {
        public void Configure(EntityTypeBuilder<CategoriaGasto> builder)
        {
            builder.HasKey(estado => estado.IDCategoriaGasto);

            builder.Property(estado => estado.Nombre);
            builder.Property(estado => estado.Descripcion);
        }
    }
}