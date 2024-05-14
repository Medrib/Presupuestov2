using Track.Order.Application.Interfaces;
using Track.Order.Domain.Entities;

namespace Track.Order.Infrastructure.Repositories
{
    public class CategoriaRepository : BaseRepository<CategoriaGasto, int>, ICategoriaRepository
    {
        public CategoriaRepository(TrackOrderDbContext context)
         : base(context)
        { }
    }
}