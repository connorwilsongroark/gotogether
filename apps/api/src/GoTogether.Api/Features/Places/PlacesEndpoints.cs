using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace GoTogether.Features.Places;

public static class PlacesEndpoints
{
    public static RouteGroupBuilder MapPlacesEndpoints(this RouteGroupBuilder group)
    {
        // GET /api/v1/places
        group.MapGet("/", () => Results.Ok(new[] {"todo"}))
            .RequireAuthorization();

        // GET /api/v1/places/{id}
        group.MapGet("/{id:guid}", (Guid id) => Results.Ok(new { id }))
             .RequireAuthorization();


        // POST /api/v1/places
        group.MapPost("/", (CreatePlaceRequest req) =>
        {
            if (string.IsNullOrWhiteSpace(req.Name))
                return Results.BadRequest(new { error = "Name is required." });

            return Results.Ok(new { id = Guid.NewGuid(), name = req.Name.Trim() });
        })
        .RequireAuthorization();


        return group;
    }
}