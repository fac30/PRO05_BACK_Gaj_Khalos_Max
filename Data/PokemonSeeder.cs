using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using PokeLikeAPI.Data;
using PokeLikeAPI.Models;
using System.Collections.Generic;
using System;

public static class PokemonSeeder
{
    private static readonly HttpClient httpClient = new HttpClient();

    public static async Task SeedPokemonsAsync(PokeLikeDbContext context)
    {
        if (await context.Pokemon.AnyAsync()) return;

        var response = await httpClient.GetAsync("https://pokeapi.co/api/v2/pokemon?limit=649");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var jsonData = JsonDocument.Parse(json);
            var root = jsonData.RootElement.GetProperty("results");

            var pokemons = new List<Pokemon>();
            int pokemonNumber = 1;

            foreach (var element in root.EnumerateArray())
            {
                var name = element.GetProperty("name").GetString() ?? "unknown";
                var apiUrl = element.GetProperty("url").GetString() ?? string.Empty;
                var imageUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/dream-world/{pokemonNumber}.svg";

                var pokemon = new Pokemon
                {
                    Name = name,
                    ImageUrl = imageUrl,
                    ApiUrl = apiUrl,
                    CreatedAt = DateTime.UtcNow
                };

                pokemons.Add(pokemon);
                pokemonNumber++;
            }

            await context.Pokemon.AddRangeAsync(pokemons);
            await context.SaveChangesAsync();
        }
        else
        {
            Console.WriteLine($"Failed to fetch Pokémon data. Status Code: {response.StatusCode}");
        }
    }
}