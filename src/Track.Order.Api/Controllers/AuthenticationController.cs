using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Track.Order.Application.Interfaces;
using Track.Order.Api.Contracts.Order;

namespace Track.Order.Api.Controllers;

[ApiController]
[Route("/rol")]
public class AuthenticacionController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public AuthenticacionController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    [HttpGet()]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<UserRoleResponse>GetRolByIdAsync(string nombre, string correoElectronico)
    {
        var userRole = await _orderService.GetRolByIdAsync(nombre, correoElectronico);
        return userRole;
    }
}