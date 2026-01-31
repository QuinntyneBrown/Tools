using Microsoft.Extensions.DependencyInjection;
using NuGetExplorer.Domain.Interfaces;
using NuGetExplorer.Infrastructure.Repositories;

namespace NuGetExplorer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient<INuGetRepository, NuGetRepository>(client =>
        {
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "NuGetExplorer/1.0");
        });

        return services;
    }
}
