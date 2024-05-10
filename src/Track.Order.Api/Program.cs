using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Track.Order.Application;
using Track.Order.Infrastructure;

namespace Track.Order.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                Log.Information("Initiating API");

                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog(); // Integrar Serilog

                Log.Logger.Information("Builder created, adding services");

                builder.Services.RegisterServices(builder.Configuration);
                builder.Services.RegisterInfrastructure(builder.Configuration);
                builder.Services.AddControllers();
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
                builder.Services.ConfigureOptions(builder.Configuration);
                builder.Services.ConfigureAutoMapper();

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseCors(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            
                app.UseHttpsRedirection();
                app.UseAuthorization();
                app.MapControllers();
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}