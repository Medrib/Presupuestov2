namespace Track.Order.Api.Contracts.Order.SearchOrders
{
    public class Filters
    {
        public int? IDGasto { get; set; }
        public int? Monto { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Descripcion { get; set; }
        public int? IDPresupuesto { get; set; }
        public int? IDCategoriaGasto { get; set; }

    }
}