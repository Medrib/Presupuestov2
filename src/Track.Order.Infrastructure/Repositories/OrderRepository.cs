namespace Track.Order.Infrastructure.Repositories;

using Track.Order.Application.Interfaces;
using Track.Order.Domain.Entities;

public class OrderRepository : BaseRepository<Gastos, int>, IOrderRepository
{
    public OrderRepository(TrackOrderDbContext context)
        : base(context)
    { }


}