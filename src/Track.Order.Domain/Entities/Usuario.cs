using System.ComponentModel.DataAnnotations;
namespace Track.Order.Domain.Entities;

public class Usuario
{
    [Key]
    public int IDUsuario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string Contraseña { get; set; } = string.Empty ;
}