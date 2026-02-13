namespace GoTogether.Storage;

public interface IFileStorage
{
 /// <summary>
 /// Saves content and returns a public URL path (relative to the API host) that can be used by the UI
 /// Example: "/uploads/avatars/<userId>/<filename>.webp"
 /// </summary>
 Task<string> SaveAsync(
    Stream content,
    string contentType,
    string relativePath,
    CancellationToken ct
 );

 Task DeleteIfExistsAsync(string relativePath, CancellationToken ct);
}