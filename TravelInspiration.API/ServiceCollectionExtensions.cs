using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TravelInspiration.API.Shared.Behaviors;
using TravelInspiration.API.Shared.Metrics;
using TravelInspiration.API.Shared.Networking;
using TravelInspiration.API.Shared.Persistence;
using TravelInspiration.API.Shared.Slices;

namespace TravelInspiration.API;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDestinationSearchApiClient, DestinationSearchApiClient>();
        services.RegisterSlices();
        var currentAssembly = Assembly.GetExecutingAssembly();
        services.AddAutoMapper(currentAssembly);
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(currentAssembly)
                .AddOpenRequestPreProcessor(typeof(LoggingBehavior<>))
                .AddOpenBehavior(typeof(ModelValidationBehavior<,>))  
                .AddOpenBehavior(typeof(HandlerPerformanceMetricBehavior<,>));
        });
        services.AddValidatorsFromAssembly(currentAssembly);
        // We are counting milliseconds elapsed across requests so it needs a singleton lifetime
        services.AddSingleton<HandlerPerformanceMetric>();
        return services;
    }

    public static IServiceCollection RegisterPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TravelInspirationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("TravelInspirationDbConnection")));

        return services;
    }
}
