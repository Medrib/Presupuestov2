using Track.Order.Application.Interfaces;
using Track.Order.Domain.Entities;

namespace Track.Order.Infrastructure.Repositories
{
    public class CuentaRepository : BaseRepository<Cuenta, int>, ICuentaRepository
    {
        public CuentaRepository(TrackOrderDbContext context)
         : base(context)
        { }
    }
}


