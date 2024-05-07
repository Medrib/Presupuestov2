using Track.Order.Application.Interfaces;
using Track.Order.Domain.Entities;

namespace Track.Order.Infrastructure.Repositories
{
    public class IngresoRepository : BaseRepository<Ingresos, int>, IIngresoRepository
    {
        public IngresoRepository(TrackOrderDbContext context)
         : base(context)
        { }
    }
}