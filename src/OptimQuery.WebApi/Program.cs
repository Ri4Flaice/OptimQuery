using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OptimQuery.Business.Feature.Repository;
using OptimQuery.Core.Interfaces.Repository;
using OptimQuery.Data.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Optimquery API",
        Version = "v1",
    });
    options.EnableAnnotations();
});

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("OptimQueryDB") ??
                      throw new InvalidOperationException(
                          "The connection string 'OptimQueryDB' is missing or empty.")));

builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var cancellationToken = new CancellationTokenSource();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();

    if (!await dbContext.UserEntities.AnyAsync())
    {
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        await userRepository.AddUsersAsync(cancellationToken.Token);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/start", () => "Hello World!");
app.MapGet("/users", async (
    string? cursor = null,
    int limit = 10,
    CancellationToken cancellationToken = default) =>
{
    switch (limit)
    {
        case < 1:
            return Results.BadRequest("Limit can't be less than 1");
        case > 100:
            return Results.BadRequest("Limit can't be more than 100");
    }

    using var scope = app.Services.CreateScope();

    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

    var result = await userRepository
        .GetUsersAsync(cursor, limit, cancellationToken);
    
    return Results.Ok(result);
});

await app.RunAsync();