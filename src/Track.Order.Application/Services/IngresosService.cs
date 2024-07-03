using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Track.Order.Api.Contracts.Ingreso;
using Track.Order.Application.Interfaces;
using Track.Order.Domain.Entities;

namespace Track.Order.Application.Services;



public class IngresosService : IIngresosService
{
    private readonly IIngresoRepository _ingresoRepository;
    public IngresosService(IIngresoRepository ingresoRepository)
    {
        _ingresoRepository = ingresoRepository;
    }
    public async Task<string> AgregarIngreso(AgregarIngresoRequest detalle)
    {
        var nuevoIngreso = new Ingresos
        {
            IDPresupuesto = detalle.IDPresupuesto,
            Monto = detalle.Monto,
            Fecha = detalle.Fecha,
            Descripcion = detalle.Descripcion
        };
        await _ingresoRepository.AddAsync(nuevoIngreso);

        return "Ingreso agregado exitosamente";
    }
}
