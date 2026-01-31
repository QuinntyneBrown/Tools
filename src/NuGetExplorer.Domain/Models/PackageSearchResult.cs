namespace NuGetExplorer.Domain.Models;

public class PackageSearchResult
{
    public int TotalHits { get; set; }
    public List<NuGetPackage> Packages { get; set; } = new();
}
