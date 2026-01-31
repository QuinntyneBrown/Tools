namespace NuGetExplorer.Domain.Models;

public class NuGetPackage
{
    public string Id { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Authors { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public string LicenseUrl { get; set; } = string.Empty;
    public string ProjectUrl { get; set; } = string.Empty;
    public string RepositoryUrl { get; set; } = string.Empty;
    public string Tags { get; set; } = string.Empty;
    public long TotalDownloads { get; set; }
    public bool Verified { get; set; }
    public DateTimeOffset? Published { get; set; }
    public List<string> Versions { get; set; } = new();
}
