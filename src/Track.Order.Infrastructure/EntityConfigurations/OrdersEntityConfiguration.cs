using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Track.Order.Domain.Entities;

namespace Track.Order.Infrastructure.EntityConfigurations
{
    public class OrdersEntityConfiguration : IEntityTypeConfiguration<Orders>
    {
        public void Configure(EntityTypeBuilder<Orders> builder)
        {
            builder.HasKey(order => order.Id);

            builder.Property(order => order.ContratoSAP);
            builder.Property(order => order.PedidoCliente);
            builder.Property(order => order.CreadoEl);
            builder.Property(order => order.FechaPedidoCliente);
            builder.Property(order => order.ClaseDocVentas);
            builder.Property(order => order.Pedido);
            builder.Property(order => order.PosPed);
            builder.Property(order => order.Solicitante);
            builder.Property(order => order.SolDesc);
            builder.Property(order => order.Pagador);
            builder.Property(order => order.DescPagador);
            builder.Property(order => order.DMDesc);
            builder.Property(order => order.RefEmpleado);
            builder.Property(order => order.Almacen);
            builder.Property(order => order.Material);
            builder.Property(order => order.TextoBreveMaterial);
            builder.Property(order => order.NumeroMaterialCliente);
            builder.Property(order => order.FechaPrimerReparto);
            builder.Property(order => order.CantidadPedido);
            builder.Property(order => order.CtdPend);
            builder.Property(order => order.CtdEntreg);
            builder.Property(order => order.UnMedidaVenta);
            builder.Property(order => order.Entrega);
            builder.Property(order => order.Factura);
            builder.Property(order => order.Referencia);
            builder.Property(order => order.NroTransporte);
            builder.Property(order => order.FechaConfirmacion);
            builder.Property(order => order.NumIDExt);
            builder.Property(order => order.Oportunidad);
            builder.Property(order => order.ClasePedido);
            builder.Property(order => order.MotivoPedido);
            builder.Property(order => order.NExpCliente);
            builder.Property(order => order.NumContCliente);
            builder.Property(order => order.DireccionEmail);
            builder.Property(order => order.Lote);
            builder.Property(order => order.UrlTransportista);
            builder.Property(order => order.DocumentoAnexo);
            builder.Property(order => order.DestinatarioMercancias);

            builder.HasOne(order => order.Estados)
                .WithMany()
                .HasForeignKey(order => order.IdEstado);

            builder.HasOne(order => order.Usuarios)
                .WithMany()
                .HasForeignKey(order => order.IdUsuario);
        }
    }
}
