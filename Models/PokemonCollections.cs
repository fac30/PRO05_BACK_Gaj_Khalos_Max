namespace PokeLikeAPI.Models
{
    public class PokemonCollections
    {
        public int PokemonId { get; set; }
        public required Pokemon Pokemon { get; set; }
        public int CollectionId { get; set; }
        public required Collection Collection { get; set; }
    }
}
