using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Track.Order.Domain.Entities;

namespace Track.Order.Infrastructure.EntityConfigurations
{
    public class CuentaEntityConfiguration : IEntityTypeConfiguration<Cuenta>
    {
        public void Configure(EntityTypeBuilder<Cuenta> builder)
        {
            builder.HasKey(estado => estado.IdCuenta);

            builder.Property(estado => estado.Nombre);
            
        }
    }
}


