using System.ComponentModel.DataAnnotations;

namespace GoTogether.Entities;

public sealed class Place
{
    public Guid Id {get; set;}
    [Required, MaxLength(150)]
    public string OwnerUserId {get; set;} = string.Empty;

    [Required, MaxLength(120)]
    public string Name {get; set; } = string.Empty;
    [MaxLength(2000)]
    public string? Description {get; set;}

    public DateTimeOffset CreatedAt {get; set;}
}