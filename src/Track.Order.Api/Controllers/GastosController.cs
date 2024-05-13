using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

using Track.Order.Api.Contracts.Gasto;
using Track.Order.Api.Contracts.Order.SearchOrders;
using Track.Order.Application.Interfaces;
using Track.Order.Common;

using Track.Order.Infrastructure;
using Track.Order.Api.Contracts.Ingreso;
using Track.Order.Domain.Entities;
using Track.Order.Api.Contracts.Cuenta;

namespace Track.Order.Api.Controllers;

[ApiController]
[Route("/gastos")]
public class GastosController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public GastosController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    [HttpGet()]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllGastosAsync()
    {
        var serviceResult = await _orderService.GetAllGastosAsync();

        if (serviceResult.IsFailure)
            return serviceResult.BuildErrorResult();

        return Ok(serviceResult.Data);
    }


    [HttpGet("search")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> SearchOrdersAsync([FromQuery] Filters filters, [FromQuery] Sort sort, [FromQuery] Pagination pagination, bool search)
    {
        var serviceResult = await _orderService.SearchOrdersAsync(filters, sort, pagination, search);

        if (serviceResult.IsFailure)
            return serviceResult.BuildErrorResult();

        return Ok(serviceResult.Data);
    }

    [HttpPost("agregarGasto")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AgregarGasto([FromBody] AgregarGastoRequest detalle)
    {
        try
        {
            var serviceResult = await _orderService.AgregarGasto(detalle);
            return Ok(serviceResult);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al agregar el gasto.");
        }
    }

    [HttpPost("agregarIngreso")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AgregarIngreso([FromBody] AgregarIngresoRequest detalle)
    {
        try
        {
            var serviceResult = await _orderService.AgregarIngreso(detalle);
            return Ok(serviceResult);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al agregar el ingreso.");
        }
    }

    [HttpPost("agregarCategoria")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AgregarCategoria([FromBody] AgregarCategoriaRequest categoria)
    {
        try
        {
            var serviceResult = await _orderService.AgregarCategoria(categoria);
            return Ok(serviceResult);
        }
        catch (Exception ex)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al agregar el categoria.");
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
            var serviceResult = await _orderService.AgregarCuenta(cuenta);
            return Ok(serviceResult);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al agregar el cuenta.");
        }
    }
   

    [HttpGet("orderCount")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> CountOrdersAsync([FromQuery] Filters filters)
    {
        try
        {
            var serviceResult = await _orderService.CountOrdersAsync(filters);
            return Ok(serviceResult);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al realizar el OrderCount.");
        }
    }

    [HttpGet("getCategory")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> GetCategoriesAsync()
    {
        try
        {
            var serviceResult = await _orderService.GetCategoriesAsync();
            return Ok(serviceResult);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al realizar el GetCategory.");
        }

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
            var serviceResult = await _orderService.GetCuentaAsync();
            return Ok(serviceResult);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al realizar el GetCuenta.");
        }

    }

    [HttpDelete("delete/{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> eliminarGasto(int id)
    {
        try
        {
            var serviceResult = await _orderService.eliminarGasto(id);
            return Ok(serviceResult);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al eliminar gasto.");
        }
    }

}