using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using PokeLikeAPI.Data;
using PokeLikeAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

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

app.UseCors("AllowSpecificOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PokeLike V1");
    });
}

app.MapGet("/", () => "Welcome to the PokeLike API!");

app.MapGet("/pokemon", async (PokeLikeDbContext db) => await db.Pokemon.OrderBy(poke => poke.Id).ToListAsync());

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

app.Run();


// builder.Services.AddCors(options => { });