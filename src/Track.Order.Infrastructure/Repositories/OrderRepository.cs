namespace Track.Order.Infrastructure.Repositories;

using System.Threading.Tasks;
using Track.Order.Api.Contracts.Gasto;
using Track.Order.Application.Interfaces;
using Track.Order.Domain.Entities;

public class OrderRepository : BaseRepository<Gastos, int>, IOrderRepository
{
    public OrderRepository(TrackOrderDbContext context)
        : base(context)
    { 
    }
}