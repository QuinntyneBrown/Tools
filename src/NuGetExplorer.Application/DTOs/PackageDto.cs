namespace NuGetExplorer.Application.DTOs;

public record PackageDto(
    string Id,
    string Version,
    string Title,
    string Description,
    string Authors,
    string IconUrl,
    string ProjectUrl,
    string RepositoryUrl,
    string Tags,
    long TotalDownloads,
    bool Verified,
    List<string> Versions);

public record PackageSearchResultDto(
    int TotalHits,
    List<PackageDto> Packages);

public record PackageSummaryDto(
    string Id,
    string Version,
    string Title,
    string Authors,
    long TotalDownloads);
