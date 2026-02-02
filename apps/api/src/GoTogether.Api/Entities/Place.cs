using System.ComponentModel.DataAnnotations;

namespace GoTogether.Entities;

public sealed class Place
{
    public Guid Id {get; set;} = Guid.NewGuid();

    [Required, MaxLength(120)]
    public string Name {get; set; } = string.Empty;

    public DateTimeOffset CreatedAt {get; set;} = DateTimeOffset.UtcNow;
}