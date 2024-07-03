using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Track.Order.Api.Contracts.Ingreso;

namespace Track.Order.Application.Interfaces;

public interface IIngresosService
{
    Task<string> AgregarIngreso(AgregarIngresoRequest detalle);
}
