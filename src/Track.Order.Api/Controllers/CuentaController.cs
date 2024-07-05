using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Track.Order.Api.Contracts.Gasto;
using Track.Order.Api.Contracts.Order.SearchOrders;
using Track.Order.Application.Interfaces;
using Track.Order.Common;
using Track.Order.Api.Contracts.Ingreso;
using Track.Order.Api.Contracts.Cuenta;

namespace Track.Order.Api.Controllers;
[ApiController]
[Route("/cuenta")]
public class CuentaController : Controller
{
    private readonly ICuentaService _cuentaService;

    public CuentaController(ICuentaService cuentaService)
    {
        _cuentaService = cuentaService;

    }

    [HttpGet("getCuenta")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCuentaAsync()
    {
        try
        {
            var serviceResult = await _cuentaService.GetCuentaAsync();
            return Ok(serviceResult);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al realizar el GetCuenta.");
        }

    }
    [HttpPost("agregarCuenta")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AgregarCuenta([FromBody] AgregarCuentaRequest cuenta)
    {
        try
        {
            var serviceResult = await _cuentaService.AgregarCuenta(cuenta);
            return Ok(serviceResult);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al agregar el cuenta.");
        }
    }
}
