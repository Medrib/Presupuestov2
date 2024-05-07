using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Track.Order.Api.Contracts.Order.SearchOrders;
using Track.Order.Application.Interfaces;
using Track.Order.Common;
using Track.Order.Domain.Entities;
using Track.Order.Infrastructure;

namespace Track.Order.Api.Controllers;



[ApiController]
[Route("/order")]
public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrderController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    [HttpGet()]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllOrderAsync()
    {
        var serviceResult = await _orderService.GetAllOrderAsync();

        if (serviceResult.IsFailure)
            return serviceResult.BuildErrorResult();

        return Ok(serviceResult.Data);
    }

    [HttpGet("search")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> SearchOrdersAsync([FromQuery] Filters filters, [FromQuery] Sort sort, [FromQuery] Pagination pagination,bool search, string nombre,string correoElectronico)
    {
        var serviceResult = await _orderService.SearchOrdersAsync(filters, sort, pagination,search, nombre, correoElectronico);

        if (serviceResult.IsFailure)
            return serviceResult.BuildErrorResult();

        return Ok(serviceResult.Data);
    }
}
