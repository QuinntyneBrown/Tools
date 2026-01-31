using Microsoft.Extensions.DependencyInjection;
using NuGetExplorer.Application.Services;

namespace NuGetExplorer.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<INuGetService, NuGetService>();
        return services;
    }
}
