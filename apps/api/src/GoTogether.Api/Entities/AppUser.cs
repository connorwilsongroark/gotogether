namespace GoTogether.Entities;

public sealed class AppUser
{
    public Guid Id {get; set;}
    public string Auth0Sub {get; set;} = null!;
    public string? Email {get; set;}
    public string DisplayName {get; set;}
    public string? AvatarUrl {get; set;}
    public string? TimeZoneId {get; set;}
    public DateTimeOffset CreatedAt {get; set;}
    public DateTimeOffset? UpdatedAt {get; set;}

}