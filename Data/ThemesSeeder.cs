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
    private static readonly string[] themesList = [
        "Cute",
        "Strong",
        "Ugly",
        "Sleepy",
        "Intelligent"
    ];


    public static async Task SeedThemesAsync(PokeLikeDbContext context)
    {
        var themes = new List<Theme>();
        foreach (var theme in themesList)
        {
            themes.Add(new Theme
            {
                Name = theme,
                CreatedAt = DateTime.UtcNow,
            });
        }

        await context.Themes.AddRangeAsync(themes);
        await context.SaveChangesAsync();
    }
}