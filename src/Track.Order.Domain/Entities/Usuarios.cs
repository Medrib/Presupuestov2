using System.Globalization;

namespace Track.Order.Domain.Entities;

public class Usuarios
{
    public int IdUsuario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = null!;
    public int IdCliente { get; set; }
}
