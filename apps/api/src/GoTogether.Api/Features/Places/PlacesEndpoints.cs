using GoTogether.Data;
using GoTogether.Entities;
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
        group.MapPost("/", async (CreatePlaceRequest req, HttpContext http, AppDbContext db) =>
        {
            if (string.IsNullOrWhiteSpace(req.Name))
                return Results.BadRequest(new { error = "Name is required." });

            var userId = http.User.FindFirst("sub")?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return Results.Unauthorized();

            var place = new Place
            {
                Id = Guid.NewGuid(),
                OwnerUserId = userId,
                Name = req.Name.Trim(),
                Description = req.Description,
                CreatedAt = DateTimeOffset.UtcNow
            };

            db.Places.Add(place);
            await db.SaveChangesAsync();

            var response = new CreatePlaceResponse(place.Id, place.Name);

            return Results.Created($"/api/v1/places/{place.Id}", response);
        })
        .RequireAuthorization();


        return group;
    }
}