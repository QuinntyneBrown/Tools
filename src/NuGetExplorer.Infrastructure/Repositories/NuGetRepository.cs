using System.Net.Http.Json;
using System.Text.Json;
using NuGetExplorer.Domain.Interfaces;
using NuGetExplorer.Domain.Models;

namespace NuGetExplorer.Infrastructure.Repositories;

public class NuGetRepository : INuGetRepository
{
    private readonly HttpClient _httpClient;
    private const string SearchBaseUrl = "https://azuresearch-usnc.nuget.org/query";
    private const string RegistrationBaseUrl = "https://api.nuget.org/v3/registration5-semver1";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public NuGetRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PackageSearchResult> SearchPackagesAsync(string query, int skip = 0, int take = 20)
    {
        var url = $"{SearchBaseUrl}?q={Uri.EscapeDataString(query)}&skip={skip}&take={take}&prerelease=false";
        return await ExecuteSearchAsync(url);
    }

    public async Task<PackageSearchResult> GetPackagesByOwnerAsync(string owner, int skip = 0, int take = 20)
    {
        var query = $"owner:{owner}";
        var url = $"{SearchBaseUrl}?q={Uri.EscapeDataString(query)}&skip={skip}&take={take}&prerelease=false";
        return await ExecuteSearchAsync(url);
    }

    public async Task<NuGetPackage?> GetPackageByIdAsync(string packageId)
    {
        var url = $"{SearchBaseUrl}?q=packageid:{Uri.EscapeDataString(packageId)}&take=1";
        var result = await ExecuteSearchAsync(url);
        return result.Packages.FirstOrDefault();
    }

    public async Task<IEnumerable<string>> GetPackageVersionsAsync(string packageId)
    {
        var url = $"{RegistrationBaseUrl}/{packageId.ToLowerInvariant()}/index.json";

        try
        {
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<string>();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
            var versions = new List<string>();

            if (json.TryGetProperty("items", out var items))
            {
                foreach (var item in items.EnumerateArray())
                {
                    if (item.TryGetProperty("items", out var leafItems))
                    {
                        foreach (var leaf in leafItems.EnumerateArray())
                        {
                            if (leaf.TryGetProperty("catalogEntry", out var entry) &&
                                entry.TryGetProperty("version", out var version))
                            {
                                versions.Add(version.GetString() ?? string.Empty);
                            }
                        }
                    }
                }
            }

            return versions;
        }
        catch
        {
            return Enumerable.Empty<string>();
        }
    }

    private async Task<PackageSearchResult> ExecuteSearchAsync(string url)
    {
        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>(JsonOptions);
            var result = new PackageSearchResult();

            if (json.TryGetProperty("totalHits", out var totalHits))
            {
                result.TotalHits = totalHits.GetInt32();
            }

            if (json.TryGetProperty("data", out var data))
            {
                foreach (var item in data.EnumerateArray())
                {
                    var package = ParsePackage(item);
                    result.Packages.Add(package);
                }
            }

            return result;
        }
        catch (HttpRequestException)
        {
            return new PackageSearchResult();
        }
    }

    private static NuGetPackage ParsePackage(JsonElement item)
    {
        var package = new NuGetPackage
        {
            Id = GetStringProperty(item, "id"),
            Version = GetStringProperty(item, "version"),
            Title = GetStringProperty(item, "title"),
            Description = GetStringProperty(item, "description"),
            Authors = GetAuthorsString(item),
            IconUrl = GetStringProperty(item, "iconUrl"),
            LicenseUrl = GetStringProperty(item, "licenseUrl"),
            ProjectUrl = GetStringProperty(item, "projectUrl"),
            Tags = GetTagsString(item),
            TotalDownloads = GetLongProperty(item, "totalDownloads"),
            Verified = GetBoolProperty(item, "verified")
        };

        if (item.TryGetProperty("versions", out var versions))
        {
            foreach (var v in versions.EnumerateArray())
            {
                if (v.TryGetProperty("version", out var versionProp))
                {
                    package.Versions.Add(versionProp.GetString() ?? string.Empty);
                }
            }
        }

        return package;
    }

    private static string GetStringProperty(JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.String
            ? prop.GetString() ?? string.Empty
            : string.Empty;
    }

    private static long GetLongProperty(JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.Number
            ? prop.GetInt64()
            : 0;
    }

    private static bool GetBoolProperty(JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out var prop) &&
               (prop.ValueKind == JsonValueKind.True || prop.ValueKind == JsonValueKind.False) &&
               prop.GetBoolean();
    }

    private static string GetAuthorsString(JsonElement item)
    {
        if (!item.TryGetProperty("authors", out var authors))
            return string.Empty;

        if (authors.ValueKind == JsonValueKind.Array)
        {
            var authorList = new List<string>();
            foreach (var author in authors.EnumerateArray())
            {
                if (author.ValueKind == JsonValueKind.String)
                    authorList.Add(author.GetString() ?? string.Empty);
            }
            return string.Join(", ", authorList);
        }

        if (authors.ValueKind == JsonValueKind.String)
            return authors.GetString() ?? string.Empty;

        return string.Empty;
    }

    private static string GetTagsString(JsonElement item)
    {
        if (!item.TryGetProperty("tags", out var tags) || tags.ValueKind != JsonValueKind.Array)
            return string.Empty;

        var tagList = new List<string>();
        foreach (var tag in tags.EnumerateArray())
        {
            if (tag.ValueKind == JsonValueKind.String)
                tagList.Add(tag.GetString() ?? string.Empty);
        }
        return string.Join(", ", tagList);
    }
}
