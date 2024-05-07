
namespace Track.Order.Domain.Entities;

public class Presupuesto
{
    public int IDPresupuesto { get; set; }
    public string FechaCreacion { get; set; } = string.Empty;
    public int Nombre { get; set; }
    public int Descripcion { get; set; }

    public Usuario? Usuario { get; set; }



}