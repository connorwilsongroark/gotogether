using GoTogether.Data;
using GoTogether.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoTogether.Features.Places;

public static class PlacesEndpoints
{
    public static RouteGroupBuilder MapPlacesEndpoints(this RouteGroupBuilder group)
    {
        // GET /api/v1/places
        group.MapGet("/", async (
            ClaimsPrincipal user, 
            AppDbContext db, 
            CancellationToken ct
        ) =>
        {
            var userId = user.GetUserId();
            if (userId is null) return Results.Unauthorized();

            var places = await db.Places
                .AsNoTracking()
                .Where(p => p.OwnerUserId == userId)
                .OrderBy(p => p.Name)
                .ToListAsync(ct);
            
            return Results.Ok(places);
        })
            .RequireAuthorization();

        // GET /api/v1/places/{id}
        group.MapGet("/{id:guid}", async (
            Guid id,
            ClaimsPrincipal user,
            AppDbContext db,
            CancellationToken ct
        ) =>
        {
            var userId = user.GetUserId();
            if (userId is null) return Results.Unauthorized();
            var place = await db.Places
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && p.OwnerUserId == userId, ct);

            if (place is null) return Results.NotFound();

            return Results.Ok(place);
        })
             .RequireAuthorization();

        //PUT /api/v1/places/{id}
        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdatePlaceRequest req,
            ClaimsPrincipal user,
            AppDbContext db,
            CancellationToken ct) =>
        {
            var userId = user.GetUserId();
            if (userId is null) return Results.Unauthorized();

            // Validation
            if (string.IsNullOrWhiteSpace(req.Name))
                return Results.BadRequest(new { error = "Name is required."});

            // Get the place from the DB, if it exists
            var place = await db.Places.FirstOrDefaultAsync(p => p.Id == id, ct);
            if (place is null) return Results.NotFound();

            // Only allow users to edit their own places for now
            if (place.OwnerUserId != userId) return Results.Forbid();

            // Update properties
            place.Name = req.Name.Trim();
            place.Description = req.Description;

            await db.SaveChangesAsync();
            return Results.NoContent();
        });


        // POST /api/v1/places
        group.MapPost("/", async (
            CreatePlaceRequest req, 
            ClaimsPrincipal user, 
            AppDbContext db,
            CancellationToken ct) =>
        {
            var userId = user.GetUserId();
            if (userId is null) return Results.Unauthorized();

            if (string.IsNullOrWhiteSpace(req.Name.Trim()))
                return Results.BadRequest(new { error = "Name is required." });

            var place = new Place
            {
                Id = Guid.NewGuid(),
                OwnerUserId = userId,
                Name = req.Name.Trim(),
                Description = string.IsNullOrWhiteSpace(req.Description) ? null : req.Description.Trim(),
                CreatedAt = DateTimeOffset.UtcNow
            };

            db.Places.Add(place);
            await db.SaveChangesAsync(ct);

            var response = new CreatePlaceResponse(place.Id, place.Name);

            return Results.Created($"/api/v1/places/{place.Id}", response);
        })
        .RequireAuthorization();


        return group;
    }
}