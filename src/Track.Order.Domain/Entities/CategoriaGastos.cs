using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Track.Order.Domain.Entities;

public class CategoriaGasto
{
    [Key]
    public int IDCategoriaGasto { get; set; }
    public string Nombre { get; set; } = string.Empty;
    //public string? Descripcion { get; set; } = null!;
    public string? Descripcion { get; set; } = string.Empty;
}