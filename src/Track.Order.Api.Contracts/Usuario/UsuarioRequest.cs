using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Track.Order.Api.Contracts.Usuario
{
    public class UsuarioRequest
    {
        public string CorreoElectronico { get; set; } = string.Empty;
        public string Contraseña { get; set; } = string.Empty;

    }
}
