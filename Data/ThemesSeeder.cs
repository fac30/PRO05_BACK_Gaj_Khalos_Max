using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using PokeLikeAPI.Data;
using PokeLikeAPI.Models;
using System.Collections.Generic;
using System;

public static class ThemesSeeder
{
    private static readonly List<string> themes = new()
    {
        "Cute",
        "Strong",
        "Ugly",
        "Sleepy",
        "Intelligent"
    };

    public static async Task SeedThemesAsync(PokeLikeDbContext context)
    {
        var themeEntities = new List<Theme>();
        foreach (var themeName in themes)
        {
            themeEntities.Add(new Theme
            {
                Name = themeName,
                CreatedAt = DateTime.UtcNow,
            });
        }

        await context.Themes.AddRangeAsync(themeEntities);
        await context.SaveChangesAsync();
    }
}