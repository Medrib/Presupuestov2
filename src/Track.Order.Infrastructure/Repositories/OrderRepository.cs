namespace Track.Order.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using Track.Order.Application.Interfaces;
using Track.Order.Domain.Entities;
using Track.Order.Api.Contracts.Order;
using Track.Order.Api.Contracts.Enums;

public class OrderRepository : BaseRepository<Orders, int>, IOrderRepository
{
    public OrderRepository(TrackOrderDbContext context)
        : base(context)
    { }
    public async Task<UserRoleResponse> GetRolByIdAsync(string nombre, string correoElectronico)
    {
        try
        {
            var user = await DbContext.Set<Usuarios>()
                .FirstOrDefaultAsync(u => u.Nombre == nombre && u.CorreoElectronico == correoElectronico);

            if (user == null)
            {
                return new UserRoleResponse { Rol = -1, IdUsuario = -1 };
            }

            var userRole = await (from ur in DbContext.Set<UsuarioRol>()
                                  join r in DbContext.Set<Domain.Entities.Roles>() on ur.IdRol equals r.IdRol
                                  where ur.IdUsuario == user.IdUsuario
                                  select new { Rol = r.nombreRol, IdUsuario = user.IdUsuario })
                                  .FirstOrDefaultAsync();

            if (userRole == null)
            {
                return new UserRoleResponse { Rol = -1, IdUsuario = user.IdUsuario };
            }
            var userRoleString = userRole.Rol;
            int roleId = ConvertRoleToNumber(userRoleString);

            var response = new UserRoleResponse
            {
                Rol = roleId,
                IdUsuario = userRole.IdUsuario
            };

            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en GetRolByIdAsync: {ex}");
            return new UserRoleResponse { Rol = -1, IdUsuario = -1 };
        }

    }
    private int ConvertRoleToNumber(string roleString)
    {
        Api.Contracts.Enums.Roles.Rol roleEnum;
        if (Enum.TryParse(roleString, out roleEnum))
        {
            switch (roleEnum)
            {
                case Api.Contracts.Enums.Roles.Rol.gestorContrato:
                    return (int)Api.Contracts.Enums.Roles.Rol.gestorContrato;
                case Api.Contracts.Enums.Roles.Rol.coordinador:
                    return (int)Api.Contracts.Enums.Roles.Rol.coordinador;
                case Api.Contracts.Enums.Roles.Rol.empleado:
                    return (int)Api.Contracts.Enums.Roles.Rol.empleado;
                default:
                    return -1;
            }
        }
        else
        {
            return -1;
        }
    }

    public async Task<IQueryable<Orders>> GetOrdersByIdAsync(int idUsuario)
    {
        var idCliente = await (from u in DbContext.Set<Usuarios>()
                               where u.IdUsuario == idUsuario
                               select u.IdCliente).FirstOrDefaultAsync();

        var usuariosConElMismoCliente =
                        await (from u in DbContext.Set<Usuarios>()
                               where u.IdCliente == idCliente
                               select u.IdUsuario).ToListAsync();

        var ordersQuery = (from o in DbContext.Set<Orders>()
                           join u in DbContext.Set<Usuarios>() on o.IdUsuario equals u.IdUsuario
                           join e in DbContext.Set<Estados>() on o.IdEstado equals e.IdEstado
                           where usuariosConElMismoCliente.Contains(o.IdUsuario)
                           select new Orders
                           {
                               Id = o.Id,
                               ContratoSAP = o.ContratoSAP,
                               IdEstado = o.IdEstado,
                               IdUsuario = o.IdUsuario,
                               PedidoCliente = o.PedidoCliente,
                               CreadoEl = o.CreadoEl,
                               FechaPedidoCliente = o.FechaPedidoCliente,
                               ClaseDocVentas = o.ClaseDocVentas,
                               Pedido = o.Pedido,
                               PosPed = o.PosPed,
                               Solicitante = o.Solicitante,
                               SolDesc = o.SolDesc,
                               Pagador = o.Pagador,
                               DescPagador = o.DescPagador,
                               DMDesc = o.DMDesc,
                               RefEmpleado = o.RefEmpleado,
                               Almacen = o.Almacen,
                               Material = o.Material,
                               TextoBreveMaterial = o.TextoBreveMaterial,
                               NumeroMaterialCliente = o.NumeroMaterialCliente,
                               FechaPrimerReparto = o.FechaPrimerReparto,
                               CantidadPedido = o.CantidadPedido,
                               CtdPend = o.CtdPend,
                               CtdEntreg = o.CtdEntreg,
                               UnMedidaVenta = o.UnMedidaVenta,
                               Entrega = o.Entrega,
                               Factura = o.Factura,
                               Referencia = o.Referencia,
                               NroTransporte = o.NroTransporte,
                               FechaConfirmacion = o.FechaConfirmacion,
                               NumIDExt = o.NumIDExt,
                               Oportunidad = o.Oportunidad,
                               ClasePedido = o.ClasePedido,
                               MotivoPedido = o.MotivoPedido,
                               NExpCliente = o.NExpCliente,
                               NumContCliente = o.NumContCliente,
                               DireccionEmail = o.DireccionEmail,
                               Lote = o.Lote,
                               UrlTransportista = o.UrlTransportista,
                               DocumentoAnexo = o.DocumentoAnexo,
                               DestinatarioMercancias = o.DestinatarioMercancias,
                               Estados = e,
                               Usuarios = u
                           });

        return ordersQuery;
    }

    public async Task<IEnumerable<Orders>> OrdersFiltered(
     IQueryable<Orders> query,
     Expression<Func<Orders, bool>>? filter = null,
     Func<IQueryable<Orders>, IOrderedQueryable<Orders>>? orderBy = null,
     string includeProperties = "")
    {
        if (filter != null)
            query = query.Where(filter);

        if (orderBy != null)
            return await orderBy(query).ToListAsync();
        else
            return await query.ToListAsync();
    }

}
