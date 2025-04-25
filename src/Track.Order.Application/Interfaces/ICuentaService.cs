using Track.Order.Api.Contracts.Cuenta;
using Track.Order.Domain.Entities;

namespace Track.Order.Application.Interfaces
{
    public interface ICuentaService
    {

        Task<string> AgregarCuenta(AgregarCuentaRequest cuenta);
        Task<List<Cuenta>> GetCuentaAsync();
    }
}
