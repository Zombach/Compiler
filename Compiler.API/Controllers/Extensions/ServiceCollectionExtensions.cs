using Compiler.Business.Services;
using Compiler.Business.Services.Interfaces;
using Compiler.Core.Configs;

namespace Compiler.API.Controllers.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddAppConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<AppConfig>()
        .Bind(configuration.GetSection(nameof(AppConfig)))
        .ValidateDataAnnotations();
    }

    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<ITestService, TestService>();
        return services;
    }

    public static IServiceCollection AddConnectors(this IServiceCollection services)
    {
        //services.AddTransient<ITestRepository, TestRepository>();
        return services;
    }
}