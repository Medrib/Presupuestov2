using Track.Order.Api.Contracts.Usuario;
using Track.Order.Application.Interfaces;
using Track.Order.Domain.Entities;

namespace Track.Order.Application.Services;
public class UsuarioService : IUsuarioservice
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }
    public async Task<List<Usuario>> GetUsuarioAsync()
    {
        var usuario = await _usuarioRepository.GetAllAsync();
        var usuariosObtenidos = usuario.Where(cat => !string.IsNullOrEmpty(cat.Nombre)).ToList();
        return usuario.ToList();
    }

    public async Task<Usuario> loginUser(UsuarioRequest loginRequest)
    {
        var usuario = await _usuarioRepository.SearchAsync(u => u.CorreoElectronico == loginRequest.CorreoElectronico);
        var loginUsuario = usuario.FirstOrDefault();

        if (loginUsuario != null && loginUsuario.Contraseña == loginRequest.Contraseña)
        {

            return loginUsuario;
        }
        else
        {
            return null;
        }
    }

    public async Task<string> CreateUsuario(CreateUsuarioRequest detalle)
    {

        var nuevoUsuario = new Usuario
        {
            Nombre = detalle.Nombre,
            CorreoElectronico = detalle.CorreoElectronico,
            Contraseña = detalle.Contraseña,
        };

        await _usuarioRepository.AddAsync(nuevoUsuario);

        return "Usuario agregado exitosamente";
    }
    public async Task<string> eliminarUsuario(int id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);

        try
        {

            if (usuario != null)
            {
                await _usuarioRepository.DeleteAsync(usuario);
                return "usuario eliminado exitosamente";
            }
            else
            {
                return "No se encontró ningún usuario con el ID proporcionado";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
