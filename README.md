# NuGet Explorer API

A .NET 8 N-Tier Web API for querying NuGet packages, with a focus on packages owned by Quinntyne Brown.

## Author

**Quinntyne Brown**
Email: quinntyne@hotmail.com

## Architecture

This project follows a clean N-Tier architecture with the following layers:

```
NuGetExplorer/
├── src/
│   ├── NuGetExplorer.Domain/        # Core domain models and interfaces
│   ├── NuGetExplorer.Infrastructure/ # External service implementations (NuGet API)
│   ├── NuGetExplorer.Application/   # Business logic and DTOs
│   └── NuGetExplorer.Api/           # Web API controllers and configuration
└── NuGetExplorer.sln
```

### Layer Responsibilities

- **Domain Layer**: Contains core business entities (`NuGetPackage`, `PackageSearchResult`) and repository interfaces
- **Infrastructure Layer**: Implements the NuGet API client using `HttpClient` to communicate with nuget.org
- **Application Layer**: Business logic services and Data Transfer Objects (DTOs)
- **API Layer**: ASP.NET Core Web API controllers with Swagger documentation

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

## Getting Started

### Build the Solution

```bash
dotnet build NuGetExplorer.sln
```

### Run the API

```bash
dotnet run --project src/NuGetExplorer.Api
```

The API will start and be available at:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`

### Access Swagger UI

Navigate to the root URL (e.g., `http://localhost:5000`) to access the Swagger UI documentation.

## API Endpoints

### Get Owner Packages (Default: quinntynebrown)

```http
GET /api/packages?skip=0&take=20
```

Returns all NuGet packages owned by Quinntyne Brown.

### Search Packages

```http
GET /api/packages/search?q={query}&skip=0&take=20
```

Search for packages by query string.

### Get Package by ID

```http
GET /api/packages/{packageId}
```

Get details for a specific package.

### Get Package Versions

```http
GET /api/packages/{packageId}/versions
```

Get all available versions for a specific package.

### Get Packages by Owner

```http
GET /api/packages/owner/{owner}?skip=0&take=20
```

Get packages owned by a specific owner.

## Example Usage

### Get all packages by Quinntyne Brown

```bash
curl http://localhost:5000/api/packages
```

### Search for a specific package

```bash
curl "http://localhost:5000/api/packages/search?q=Buildingblocks"
```

### Get package details

```bash
curl http://localhost:5000/api/packages/BuildingBlocks.Core
```

## Response Models

### PackageSearchResultDto

```json
{
  "totalHits": 10,
  "packages": [
    {
      "id": "PackageName",
      "version": "1.0.0",
      "title": "Package Title",
      "description": "Package description",
      "authors": "Quinntyne Brown",
      "iconUrl": "https://...",
      "projectUrl": "https://...",
      "repositoryUrl": "https://...",
      "tags": "tag1, tag2",
      "totalDownloads": 1000,
      "verified": true,
      "versions": ["1.0.0", "0.9.0"]
    }
  ]
}
```

## Technology Stack

- .NET 8
- ASP.NET Core Web API
- Swagger/OpenAPI (Swashbuckle)
- HttpClient for NuGet API integration

## License

MIT License
