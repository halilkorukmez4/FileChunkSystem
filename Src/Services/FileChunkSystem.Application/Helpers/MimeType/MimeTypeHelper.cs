using Microsoft.AspNetCore.StaticFiles;

namespace FileChunkSystem.Application.Helpers.MimeType;

public static class MimeTypeHelper
{
    private static readonly FileExtensionContentTypeProvider _provider = new();

    public static string GetMimeType(string fileNameOrPath)
    {
        if (!_provider.TryGetContentType(fileNameOrPath, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        return contentType;
    }
}