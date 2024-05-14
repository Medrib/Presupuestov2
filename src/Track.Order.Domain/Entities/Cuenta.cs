
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Track.Order.Domain.Entities;

public class Cuenta
{
    [Key]
    public int IdCuenta { get; set; }
    public string Nombre { get; set; } = string.Empty;

}


