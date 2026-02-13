using System.Security.Claims;
using GoTogether.Data;
using GoTogether.Storage;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GoTogether.Features.Profile;

public static class MeAvatarEndpoints
{
        public static RouteGroupBuilder MapMeAvatarEndpoints(this RouteGroupBuilder group){
        group.MapPost("/avatar", UploadAvatar)
            .RequireAuthorization()
            .DisableAntiforgery();

        return group;
    }

    private static async Task<Results<Ok<AvatarUploadResponse>, BadRequest<string>, NotFound>> UploadAvatar(
        AppDbContext db,
        IFileStorage storage,
        ClaimsPrincipal user,
        HttpRequest request,
        CancellationToken ct
    )
    {
        var sub = user.GetRequiredSub();

        var appUser = await db.Users.SingleOrDefaultAsync(x => x.Auth0Sub == sub, ct);
        if (appUser is null)
            return TypedResults.NotFound();
        
        // Read multipart
        if (!request.HasFormContentType)
            return TypedResults.BadRequest("Expected multipart/form-data.");
        
        var form = await request.ReadFormAsync(ct);
        var file = form.Files.GetFile("file");
        if (file is null)
            return TypedResults.BadRequest("Missing file. Provide it as form field named 'file'.");

        // Basic validation
        const long maxBytes = 2 * 1024 * 1024; // 2 MB
        if (file.Length <= 0)
            return TypedResults.BadRequest("File is empty.");
        if (file.Length > maxBytes)
            return TypedResults.BadRequest("File is too large (max 2MB)");

        // Establish which image filetypes are allowed
        var allowedFileTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg",
            "image/png",
            "image/webp"
        };

        // If uploaded file is not an accepted type, kick it back
        if (string.IsNullOrWhiteSpace(file.ContentType) || !allowedFileTypes.Contains(file.ContentType))
            return TypedResults.BadRequest("Unsupported file type. Please use jpeg, png, or webp.");

        // Map filetype to extension
        var extension = file.ContentType.ToLowerInvariant() switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            "image/webp" => ".webp",
            _ => ".bin"
        };
        
        // Version the filename to be unique
        var fileName = $"avatar-{Guid.NewGuid():N}{extension}";
        var relativePath = $"avatars/{appUser.Id}/{fileName}";

        await using var stream = file.OpenReadStream();
        var urlPath = await storage.SaveAsync(stream, file.ContentType, relativePath, ct);

        // Update the user profile
        appUser.AvatarUrl = urlPath;
        appUser.UpdatedAt = DateTimeOffset.UtcNow;

        await db.SaveChangesAsync(ct);

        return TypedResults.Ok(new AvatarUploadResponse(urlPath));
    }
}
public sealed record AvatarUploadResponse(
    string AvatarUrl
);