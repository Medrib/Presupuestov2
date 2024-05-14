using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Track.Order.Domain.Entities;

namespace Track.Order.Infrastructure.EntityConfigurations;

public class OrdersEntityConfiguration : IEntityTypeConfiguration<Gastos>
{
    public void Configure(EntityTypeBuilder<Gastos> builder)
    {
        builder.HasKey(order => order.IDGasto);

        builder.Property(order => order.Monto);
        builder.Property(order => order.Fecha);
        builder.Property(order => order.Descripcion);

        builder.HasOne(order => order.Presupuesto)
               .WithMany()
               .HasForeignKey(order => order.IDPresupuesto); // Aquí utilizamos la propiedad de clave externa PresupuestoId

        builder.HasOne(order => order.CategoriaGasto)
               .WithMany()
               .HasForeignKey(order => order.IDCategoriaGasto);

        builder.HasOne(order => order.Cuenta)
              .WithMany()
              .HasForeignKey(order => order.IDCuenta);

    }
}