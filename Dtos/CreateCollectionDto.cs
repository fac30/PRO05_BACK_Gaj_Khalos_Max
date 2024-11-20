namespace PokeLikeAPI.Dtos
{
    public class CreateCollectionDto
    {
        public string Name { get; set; } = "";
        public int ThemeId { get; set; }
        public int Likes { get; set; }
        public string Password { get; set; } = "";
        public List<int> PokemonIds { get; set; } = new List<int>();
    }
}