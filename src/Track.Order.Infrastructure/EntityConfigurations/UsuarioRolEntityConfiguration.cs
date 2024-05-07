using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Track.Order.Domain.Entities;

namespace Track.Order.Infrastructure.EntityConfigurations;

public class IngresosEntityConfiguration : IEntityTypeConfiguration<Ingresos>
{
    public void Configure(EntityTypeBuilder<Ingresos> builder)
    {
        builder.HasKey(order => order.IDIngreso);

        builder.Property(order => order.Monto);
        builder.Property(order => order.Fecha);
        builder.Property(order => order.Descripcion);

        builder.HasOne(order => order.Presupuesto)
               .WithMany()
               .HasForeignKey(order => order.IDPresupuesto);
    }
}