namespace PokeLikeAPI.Models
{
    public class Pokemon : BaseEntity
    {
        public required string Name { get; set; }
        public required string ImageUrl { get; set; }
        public required string ApiUrl { get; set; }
        public int Likes { get; set; }
    }

    public class UpdatePokemonDto
    {
        public int? Likes { get; set; }
    }
}
