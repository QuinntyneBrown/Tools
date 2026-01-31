using NuGetExplorer.Application;
using NuGetExplorer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "NuGet Explorer API",
        Version = "v1",
        Description = "API for querying NuGet packages owned by Quinntyne Brown (quinntyne@hotmail.com)",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Quinntyne Brown",
            Email = "quinntyne@hotmail.com"
        }
    });
});

// Add application layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "NuGet Explorer API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
