namespace PokeLikeAPI.Models
{
    public class Theme : BaseEntity
    {
        public required string Name { get; set; }
        public required string ApiUrl { get; set; }
        public int Likes { get; set; }
    }
}