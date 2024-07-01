using Track.Order.Application.Interfaces;
using Track.Order.Domain.Entities;

namespace Track.Order.Infrastructure.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario, int>, IUsuarioRepository
    {
        public UsuarioRepository(TrackOrderDbContext context)
         : base(context)
        { }
    }
}
