namespace PokeLikeAPI.Data
{
    using Microsoft.EntityFrameworkCore;

    public class PokeLikeDbContext : DbContext
    {
        public PokeLikeDbContext(DbContextOptions<PokeLikeDbContext> options) : base(options) { }

        public DbSet<Pokemon> Pokemon { get; set; } = null!;
        public DbSet<Collection> Collections { get; set; } = null!;

    }
}
