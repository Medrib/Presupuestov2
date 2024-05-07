
using System.ComponentModel.DataAnnotations;

namespace Track.Order.Domain.Entities
{
    public class Orders
    {
        public int Id { get; set; }
        public int? ContratoSAP { get; set; }
        public int IdEstado { get; set; }
        public int IdUsuario { get; set; }
        public string PedidoCliente { get; set; } = string.Empty;
        public DateTime CreadoEl { get; set; }
        public DateTime FechaPedidoCliente { get; set; }
        public string ClaseDocVentas { get; set; } = string.Empty;
        public int Pedido { get; set; }
        public int PosPed { get; set; }
        public int Solicitante { get; set; }
        public string SolDesc { get; set; } = string.Empty;
        public int Pagador { get; set; }
        public string DescPagador { get; set; } = string.Empty;
        public string DMDesc { get; set; } = string.Empty;
        public int RefEmpleado { get; set; }
        public int Almacen { get; set; }
        public string Material { get; set; } = string.Empty;
        public string TextoBreveMaterial { get; set; } = string.Empty;
        public string NumeroMaterialCliente { get; set; } = string.Empty;
        public DateTime FechaPrimerReparto { get; set; }
        public int CantidadPedido { get; set; }
        public int CtdPend { get; set; }
        public int CtdEntreg { get; set; }
        public string UnMedidaVenta { get; set; } = string.Empty;
        public int Entrega { get; set; }
        public int Factura { get; set; }
        public int Referencia { get; set; }
        public int NroTransporte { get; set; }
        public DateTime FechaConfirmacion { get; set; }
        public int NumIDExt { get; set; }
        public string Oportunidad { get; set; } = string.Empty;
        public string ClasePedido { get; set; } = string.Empty;
        public string MotivoPedido { get; set; } = string.Empty;
        public int NExpCliente { get; set; }
        public string NumContCliente { get; set; } = string.Empty;
        public string DireccionEmail { get; set; } = string.Empty;
        public string Lote { get; set; } = string.Empty;
        public string UrlTransportista { get; set; } = string.Empty;
        public string DocumentoAnexo { get; set; } = string.Empty;
        public int DestinatarioMercancias { get; set; }
        public Estados? Estados { get; set; }
        public Usuarios? Usuarios { get; set; }
    }
}
