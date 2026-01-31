using NuGetExplorer.Application.DTOs;

namespace NuGetExplorer.Application.Services;

public interface INuGetService
{
    Task<PackageSearchResultDto> SearchPackagesAsync(string query, int skip = 0, int take = 20);
    Task<PackageSearchResultDto> GetOwnerPackagesAsync(string owner, int skip = 0, int take = 20);
    Task<PackageDto?> GetPackageAsync(string packageId);
    Task<IEnumerable<string>> GetPackageVersionsAsync(string packageId);
}
