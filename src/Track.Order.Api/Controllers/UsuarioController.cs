using Microsoft.AspNetCore.Mvc;
using Track.Order.Application.Interfaces;
using Track.Order.Api.Contracts.Usuario;
using AutoMapper;
using Track.Order.Application.Services;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Track.Order.Api.Controllers;
[ApiController]
[Route("/gastos")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioservice _usuarioService;
    private readonly string _cadenaSQL;

    public UsuarioController(IUsuarioservice usuarioService, IConfiguration config)
    {
        _usuarioService = usuarioService;
        _cadenaSQL = config.GetConnectionString("SqlServerConnection");
    }



    [HttpGet("getUsuario")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> GetUsuarioAsync()
    {
        try
        {
            var serviceResult = await _usuarioService.GetUsuarioAsync();
            return Ok(serviceResult);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al realizar el GetUsuario.");
        }

    }

    [HttpPost("agregarUsuario")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUsuario([FromBody] CreateUsuarioRequest detalle)
    { 
        try
        {
            var serviceResult = await _usuarioService.CreateUsuario(detalle);
            return Ok(serviceResult);
        }
        catch (Exception)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al agregar el usuario.");
        }
    }

    [HttpPost("login")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> loginUser([FromBody] UsuarioRequest loginRequest)
    {
        try
        {
            var usuario = await _usuarioService.loginUser(loginRequest);

            if (usuario != null)
            {
                return Ok(usuario);
            }
            else
            {
                return Unauthorized("Usuario y contraseñas incorrectas.");
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error durante el login.");
        }
    }

    [HttpDelete("deleteUser/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> eliminarUsuario(int id)
    {
        try
        {
            var serviceResult = await _usuarioService.eliminarUsuario(id);
            return Ok(serviceResult);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al eliminar el usuario.");
        }
    }
}

