using NuGetExplorer.Application.DTOs;
using NuGetExplorer.Domain.Interfaces;
using NuGetExplorer.Domain.Models;

namespace NuGetExplorer.Application.Services;

public class NuGetService : INuGetService
{
    private readonly INuGetRepository _repository;

    public NuGetService(INuGetRepository repository)
    {
        _repository = repository;
    }

    public async Task<PackageSearchResultDto> SearchPackagesAsync(string query, int skip = 0, int take = 20)
    {
        var result = await _repository.SearchPackagesAsync(query, skip, take);
        return MapToSearchResultDto(result);
    }

    public async Task<PackageSearchResultDto> GetOwnerPackagesAsync(string owner, int skip = 0, int take = 20)
    {
        var result = await _repository.GetPackagesByOwnerAsync(owner, skip, take);
        return MapToSearchResultDto(result);
    }

    public async Task<PackageDto?> GetPackageAsync(string packageId)
    {
        var package = await _repository.GetPackageByIdAsync(packageId);
        return package is null ? null : MapToDto(package);
    }

    public async Task<IEnumerable<string>> GetPackageVersionsAsync(string packageId)
    {
        return await _repository.GetPackageVersionsAsync(packageId);
    }

    private static PackageSearchResultDto MapToSearchResultDto(PackageSearchResult result)
    {
        return new PackageSearchResultDto(
            result.TotalHits,
            result.Packages.Select(MapToDto).ToList());
    }

    private static PackageDto MapToDto(NuGetPackage package)
    {
        return new PackageDto(
            package.Id,
            package.Version,
            package.Title,
            package.Description,
            package.Authors,
            package.IconUrl,
            package.ProjectUrl,
            package.RepositoryUrl,
            package.Tags,
            package.TotalDownloads,
            package.Verified,
            package.Versions);
    }
}
