using Microsoft.AspNetCore.Mvc;
using NuGetExplorer.Application.DTOs;
using NuGetExplorer.Application.Services;

namespace NuGetExplorer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PackagesController : ControllerBase
{
    private readonly INuGetService _nuGetService;
    private const string DefaultOwner = "quinntynebrown";

    public PackagesController(INuGetService nuGetService)
    {
        _nuGetService = nuGetService;
    }

    /// <summary>
    /// Get all packages owned by Quinntyne Brown
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PackageSearchResultDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PackageSearchResultDto>> GetOwnerPackages(
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20)
    {
        var result = await _nuGetService.GetOwnerPackagesAsync(DefaultOwner, skip, take);
        return Ok(result);
    }

    /// <summary>
    /// Search for packages by query
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PackageSearchResultDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PackageSearchResultDto>> SearchPackages(
        [FromQuery] string q,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest("Search query is required");

        var result = await _nuGetService.SearchPackagesAsync(q, skip, take);
        return Ok(result);
    }

    /// <summary>
    /// Get a specific package by ID
    /// </summary>
    [HttpGet("{packageId}")]
    [ProducesResponseType(typeof(PackageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PackageDto>> GetPackage(string packageId)
    {
        var package = await _nuGetService.GetPackageAsync(packageId);

        if (package is null)
            return NotFound($"Package '{packageId}' not found");

        return Ok(package);
    }

    /// <summary>
    /// Get all versions of a specific package
    /// </summary>
    [HttpGet("{packageId}/versions")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<string>>> GetPackageVersions(string packageId)
    {
        var versions = await _nuGetService.GetPackageVersionsAsync(packageId);
        return Ok(versions);
    }

    /// <summary>
    /// Get packages by a specific owner
    /// </summary>
    [HttpGet("owner/{owner}")]
    [ProducesResponseType(typeof(PackageSearchResultDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PackageSearchResultDto>> GetPackagesByOwner(
        string owner,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 20)
    {
        var result = await _nuGetService.GetOwnerPackagesAsync(owner, skip, take);
        return Ok(result);
    }
}
