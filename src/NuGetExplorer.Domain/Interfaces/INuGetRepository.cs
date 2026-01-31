using NuGetExplorer.Domain.Models;

namespace NuGetExplorer.Domain.Interfaces;

public interface INuGetRepository
{
    Task<PackageSearchResult> SearchPackagesAsync(string query, int skip = 0, int take = 20);
    Task<PackageSearchResult> GetPackagesByOwnerAsync(string owner, int skip = 0, int take = 20);
    Task<NuGetPackage?> GetPackageByIdAsync(string packageId);
    Task<IEnumerable<string>> GetPackageVersionsAsync(string packageId);
}
