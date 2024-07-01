namespace Track.Order.Infrastructure;

using Track.Order.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class TrackOrderDbContext : DbContext
{


    public TrackOrderDbContext(DbContextOptions<TrackOrderDbContext> options)
        : base(options)
    {
    }

    public DbSet<Gastos> Gasto=> Set<Gastos>();
    public DbSet<Usuario> Usuario => Set<Usuario>();
    public DbSet<CategoriaGasto> CategoriaGasto => Set<CategoriaGasto>();

    public DbSet<Cuenta> cuenta => Set<Cuenta>();

    public DbSet<Ingresos> Ingreso => Set<Ingresos>();



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackOrderDbContext).Assembly);
    }

}
