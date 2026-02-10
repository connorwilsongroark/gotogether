using System.Security.Claims;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? user.FindFirst("sub")?.Value;
    }
    public static string GetRequiredSub(this ClaimsPrincipal user)
    {
        // Auth0 usually exposes sub through ClaimTypes.NameIdentifier, but if not, look for "sub" directly
        var sub = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(sub))
            throw new InvalidOperationException("Missing required 'sub' (NameIdentifier) claim.");

        return sub;
    }
    public static string? GetEmail(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.Email) ?? user.FindFirstValue("email");
    }
    public static string? GetDisplayNameHint(this ClaimsPrincipal user)
    {
        return user.FindFirstValue("name")
           ?? user.FindFirstValue("nickname")
           ?? user.FindFirstValue(ClaimTypes.Name);
    }
    public static string? GetPictureUrl(this ClaimsPrincipal user)
    {
        return  user.FindFirstValue("picture");
    }
}