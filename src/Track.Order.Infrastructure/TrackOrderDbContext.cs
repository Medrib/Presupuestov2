namespace Track.Order.Infrastructure;

using Track.Order.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class TrackOrderDbContext : DbContext
{


    public TrackOrderDbContext(DbContextOptions<TrackOrderDbContext> options)
        : base(options)
    {
    }

    public DbSet<Orders> Orders => Set<Orders>();
    public DbSet<Usuarios> Usuarios => Set<Usuarios>();
    public DbSet<Estados> Estados => Set<Estados>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackOrderDbContext).Assembly);
    }

}
