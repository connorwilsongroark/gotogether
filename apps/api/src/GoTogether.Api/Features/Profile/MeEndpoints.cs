using System.Security.Claims;
using GoTogether.Data;
using GoTogether.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoTogether.Features.Profile;

public static class MeEndpoints
{
    public static RouteGroupBuilder MapMeEndpoints(this RouteGroupBuilder group){
        group.MapGet("/", GetMe).RequireAuthorization();
        group.MapPatch("/", UpdateMe).RequireAuthorization();

        return group;
    }

    private static async Task<IResult> GetMe(
        AppDbContext db, ClaimsPrincipal user, CancellationToken ct)
    {
        var sub = user.GetRequiredSub();

        var appUser = await db.Users
            .SingleOrDefaultAsync(x => x.Auth0Sub == sub, ct);
        
        // Provisioning for first login
        if (appUser is null)
        {
            appUser = new AppUser
            {
                Id = Guid.NewGuid(),
                Auth0Sub = sub,
                Email = user.GetEmail(),
                DisplayName = user.GetDisplayNameHint() ?? "New User",
                AvatarUrl = user.GetPictureUrl(),
                CreatedAt = DateTimeOffset.UtcNow
            };

            db.Users.Add(appUser);
            await db.SaveChangesAsync(ct);
        }

        return Results.Ok(ToResponse(appUser));
    }

    private static async Task<IResult> UpdateMe(
        AppDbContext db, ClaimsPrincipal user, [FromBody] UpdateMeRequest request, CancellationToken ct)
    {
        var sub = user.GetRequiredSub();

        var appUser = await db.Users.SingleOrDefaultAsync(x => x.Auth0Sub == sub, ct);

        if (appUser is null) return Results.NotFound();
        
        var errors = Validate(request);
        if (errors.Count > 0) return Results.ValidationProblem(errors);

        if (request.DisplayName is not null) 
            appUser.DisplayName = request.DisplayName.Trim();

        if (request.TimeZoneId is not null)
            appUser.TimeZoneId =
                string.IsNullOrWhiteSpace(request.TimeZoneId)
                    ? null
                    : request.TimeZoneId.Trim();

        appUser.UpdatedAt = DateTimeOffset.UtcNow;

        await db.SaveChangesAsync(ct);

        return Results.Ok(ToResponse(appUser));
    }
    private static Dictionary<string, string[]> Validate(UpdateMeRequest r)
    {
        var errors = new Dictionary<string, string[]>();

        if (r.DisplayName is not null)
        {
            var dn = r.DisplayName.Trim();
            if (dn.Length < 2 || dn.Length > 80)
            {
                errors["displayName"] =
                    ["DisplayName must be between 2 and 80 characters."];
            }
        }

        if (r.TimeZoneId is not null && r.TimeZoneId.Trim().Length > 64)
        {
            errors["timeZoneId"] = ["TimeZoneId is too long."];
        }

        return errors;
    }
    private static MeResponse ToResponse(AppUser u) =>
    new(
        u.Id,
        u.Auth0Sub,
        u.DisplayName,
        u.Email,
        u.AvatarUrl,
        u.TimeZoneId,
        u.CreatedAt,
        u.UpdatedAt
    );
}