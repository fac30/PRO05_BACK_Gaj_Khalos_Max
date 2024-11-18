using PokeLikeAPI.Models;

namespace PokeLikeAPI.Data
{
    using Microsoft.EntityFrameworkCore;

    public class PokeLikeDbContext : DbContext
    {
        public PokeLikeDbContext(DbContextOptions<PokeLikeDbContext> options) : base(options) { }

        public DbSet<Pokemon> Pokemon { get; set; } = null!;
        public DbSet<Collection> Collections { get; set; } = null!;
        public DbSet<PokemonCollections> PokemonCollections { get; set; } = null!;
        public DbSet<Theme> Themes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PokemonCollections>()
                .HasKey(pc => new { pc.PokemonId, pc.CollectionId });

            modelBuilder.Entity<PokemonCollections>()
                .HasOne(pc => pc.Pokemon)
                .WithMany()
                .HasForeignKey(pc => pc.PokemonId);

            modelBuilder.Entity<PokemonCollections>()
                .HasOne(pc => pc.Collection)
                .WithMany(c => c.PokemonCollections)
                .HasForeignKey(pc => pc.CollectionId);
        }

    }
}
