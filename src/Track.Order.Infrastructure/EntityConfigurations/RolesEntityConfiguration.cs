using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Track.Order.Domain.Entities;

namespace Track.Order.Infrastructure.EntityConfigurations
{
    public class RolesEntityConfiguration : IEntityTypeConfiguration<Roles>
    {
        public void Configure(EntityTypeBuilder<Roles> builder)
        {
            builder.HasKey(usuario => usuario.IdRol);

            builder.Property(usuario => usuario.nombreRol);
            
        }
    }
}
