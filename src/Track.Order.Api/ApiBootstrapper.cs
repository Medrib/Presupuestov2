using AutoMapper;

namespace Track.Order.Api;

public static class ApiBootstrapper
{
    public static void ConfigureAutoMapper(this IServiceCollection services)
    {
        var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddMaps(typeof(ApiProfile)));
        mapperConfiguration.AssertConfigurationIsValid();
        mapperConfiguration.CompileMappings();

        services.AddSingleton(mapperConfiguration.CreateMapper());
    }

    public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        // configure options
    }
}
