using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using PokeLikeAPI.Data;
using PokeLikeAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContextPool<PokeLikeDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PokeLikeDbContext"))
       .UseSnakeCaseNamingConvention());

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PokeLike",
        Description = "Keep track of what pokemon you think is cute 🥰",
        Version = "v1"
    });
});
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PokeLikeDbContext>();
    await PokemonSeeder.SeedPokemonsAsync(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PokeLike V1");
    });
}

app.MapGet("/", () => "Hello World!");

app.MapGet("/pokemon", async (PokeLikeDbContext db) => await db.Pokemon.ToListAsync());

app.MapPost("/pokemon", async (PokeLikeDbContext db, Pokemon pokemon) =>
{
    await db.Pokemon.AddAsync(pokemon);
    await db.SaveChangesAsync();
    return Results.Created($"/pokemon/{pokemon.Id}", pokemon);
});

app.Run();


// builder.Services.AddCors(options => { });