namespace GoTogether.Features.Places;

public sealed record CreatePlaceRequest(string Name, string? Description);
public sealed record CreatePlaceResponse(Guid Id, string Name);