using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using PokeLikeAPI.Data;
using PokeLikeAPI.Models;
using PokeLikeAPI.Dtos;
using BCrypt.Net;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
                "http://pokelike.s3-website.eu-west-2.amazonaws.com",
                "http://localhost:5137"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContextPool<PokeLikeDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PokeLikeDbContext") ??
        Environment.GetEnvironmentVariable("DB_CONNECTION_STRING"))
       .UseSnakeCaseNamingConvention());

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PokeLike",
        Description = "Keep track of what pokemon you think are cute 🥰",
        Version = "v1"
    });
});
}

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

async Task MigrateDatabase(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<PokeLikeDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    var retryCount = 0;
    const int maxRetries = 5;

    while (retryCount < maxRetries)
    {
        try
        {
            await context.Database.MigrateAsync();
            // If seeding is needed
            await PokemonSeeder.SeedPokemonsAsync(context);
            await ThemesSeeder.SeedThemesAsync(context);
            logger.LogInformation("Database migration completed");
            break;
        }
        catch (Exception ex)
        {
            retryCount++;
            logger.LogError(ex, $"Migration attempt {retryCount} of {maxRetries} failed");
            if (retryCount < maxRetries)
            {
                logger.LogInformation($"Waiting 5 seconds before retry...");
                await Task.Delay(5000); // Wait 5 seconds before retry
            }
            else
            {
                throw; // If we've exhausted all retries, rethrow the exception
            }
        }
    }
}

try
{
    await MigrateDatabase(app.Services);
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while migrating the database.");
    throw;
}

app.UseCors("AllowSpecificOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PokeLike V1");
    });
}

app.MapGet("/", () => "Welcome to the PokeLike API!");

app.MapGet("/themes", async (PokeLikeDbContext db) =>
    await db.Themes
    .OrderBy(theme => theme.Id)
    .ToListAsync());

app.MapGet("/pokemon", async (HttpContext context, PokeLikeDbContext db) =>
{
    await db.Pokemon
    .OrderBy(poke => poke.Id)
    .ToListAsync();
});

app.MapPost("/pokemon", async (PokeLikeDbContext db, Pokemon pokemon) =>
{
    await db.Pokemon.AddAsync(pokemon);
    await db.SaveChangesAsync();
    return Results.Created($"/pokemon/{pokemon.Id}", pokemon);
});

app.MapPatch("/pokemon/{id}", async (PokeLikeDbContext db, int id, HttpRequest request) =>
{
    var pokemon = await db.Pokemon.FindAsync(id);
    if (pokemon == null)
    {
        return Results.NotFound();
    }

    var updateData = await request.ReadFromJsonAsync<UpdatePokemonDto>();
    if (updateData == null || updateData.Likes == null)
    {
        return Results.BadRequest(new { message = "Invalid request payload." });
    }

    pokemon.Likes = updateData.Likes.Value;
    await db.SaveChangesAsync();

    return Results.Ok(pokemon);
});

app.MapGet("/collections", async (PokeLikeDbContext db) =>
    await db.Collections
        .Include(c => c.PokemonCollections)
            .ThenInclude(pc => pc.Pokemon)
        .OrderBy(collection => collection.Id)
        .ToListAsync());

app.MapPost("/collections", async (PokeLikeDbContext db, CreateCollectionDto dto) =>
{
    var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

    var collection = new Collection
    {
        Name = dto.Name,
        ThemeId = dto.ThemeId.ToString(),
        Likes = dto.Likes,
        PasswordHash = passwordHash
    };

    await db.Collections.AddAsync(collection);
    await db.SaveChangesAsync();

    foreach (var pokemonId in dto.PokemonIds)
    {
        var pokemonCollection = new PokemonCollections
        {
            CollectionId = collection.Id,
            Collection = collection,
            PokemonId = pokemonId,
            Pokemon = await db.Pokemon.FindAsync(pokemonId) ?? throw new InvalidOperationException($"Pokemon with ID {pokemonId} not found.")
        };
        await db.PokemonCollections.AddAsync(pokemonCollection);
    }
    await db.SaveChangesAsync();
    return Results.Created($"/collections/{collection.Id}", collection);

});

app.MapGet("/health", () => Results.Ok("Healthy"));

app.Run();


// builder.Services.AddCors(options => { });