namespace Track.Order.Api.Contracts.Order.SearchOrders
{
    public class Filters
    {
        public int? Id { get; set; }
        public int? ContratoSAP { get; set; }
        public string? Estado { get; set; }
        public int? IdUsuario { get; set; }
        public string? PedidoCliente { get; set; }
        public DateTime? CreadoEl { get; set; }
        public DateTime? FechaPedidoCliente { get; set; }
        public string? ClaseDocVentas { get; set; }
        public int? Pedido { get; set; }
        public int? PosPed { get; set; }
        public int? Solicitante { get; set; }
        public string? SolDesc { get; set; }
        public int? Pagador { get; set; }
        public string? DescPagador { get; set; }
        public string? DMDesc { get; set; }
        public int? RefEmpleado { get; set; }
        public int? Almacen { get; set; }
        public string? Material { get; set; }
        public string? TextoBreveMaterial { get; set; }
        public string? NumeroMaterialCliente { get; set; }
        public DateTime? FechaPrimerReparto { get; set; }
        public int? CantidadPedido { get; set; }
        public int? CtdPend { get; set; }
        public int? CtdEntreg { get; set; }
        public string? UnMedidaVenta { get; set; }
        public int? Entrega { get; set; }
        public int? Factura { get; set; }
        public int? Referencia { get; set; }
        public int? NroTransporte { get; set; }
        public DateTime?  FechaConfirmacion { get; set; }
        public int? NumIDExt { get; set; }
        public string? Oportunidad { get; set; }
        public string? ClasePedido { get; set; }
        public string? MotivoPedido { get; set; }
        public int? NExpCliente { get; set; }
        public string? NumContCliente { get; set; }
        public string? DireccionEmail { get; set; }
        public string? Lote { get; set; }
        public string? UrlTransportista { get; set; }
        public string? DocumentoAnexo { get; set; }
        public int? DestinatarioMercancias { get; set; }
    }
}

