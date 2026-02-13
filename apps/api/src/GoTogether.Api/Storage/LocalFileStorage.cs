using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace GoTogether.Storage;

public sealed class LocalFileStorage : IFileStorage
{
    private readonly string _root;
    private readonly string _contentRootPath;

    public LocalFileStorage(IOptions<LocalStorageOptions> options, IHostEnvironment env)
    {
        _root = options.Value.UploadRoot ?? "uploads";
        _contentRootPath = env.ContentRootPath;
    }

    public async Task<string> SaveAsync(
        Stream content,
        string contentType,
        string relativePath,
        CancellationToken ct)
    {
        var safeRelativePath = relativePath.Replace('\\', '/').TrimStart('/');

        // Write to <ContentRootPath>/<uploads>/<relative>
        var fullPath = Path.Combine(_contentRootPath, _root, safeRelativePath);

        var dir = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrWhiteSpace(dir))
            Directory.CreateDirectory(dir);

        await using var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
        await content.CopyToAsync(fs, ct);

        // Public URL path that matches your UseStaticFiles RequestPath
        return $"/{_root}/{safeRelativePath}".Replace('\\', '/');
    }

    public Task DeleteIfExistsAsync(string relativePath, CancellationToken ct)
    {
        var safeRelativePath = relativePath.Replace('\\', '/').TrimStart('/');
        var fullPath = Path.Combine(_contentRootPath, _root, safeRelativePath);

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return Task.CompletedTask;
    }
}
