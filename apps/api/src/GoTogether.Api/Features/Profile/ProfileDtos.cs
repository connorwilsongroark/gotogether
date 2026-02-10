namespace GoTogether.Features.Profile;

public sealed record MeResponse(
    Guid Id,
    string Auth0Sub,
    string DisplayName,
    string? Email,
    string? AvatarUrl,
    string? TimeZoneId,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
);

public sealed record UpdateMeRequest(
    string? DisplayName,
    string? TimeZoneId
);