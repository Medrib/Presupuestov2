using Track.Order.Api.Contracts.Usuario;
using Track.Order.Domain.Entities;

namespace Track.Order.Application.Interfaces;

public interface IUsuarioservice
{
    Task<List<Usuario>> GetUsuarioAsync();
    Task<Usuario> loginUser(UsuarioRequest loginRequest);
    Task<string> CreateUsuario(CreateUsuarioRequest detalle);
    Task<string> eliminarUsuario(int id);
}
