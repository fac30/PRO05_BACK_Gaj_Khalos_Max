public class Collection : BaseEntity
{
    public required string Name { get; set; }
    public required string ThemeId { get; set; }
    public int Likes { get; set; }
    public required string PasswordHash { get; set; }
}
