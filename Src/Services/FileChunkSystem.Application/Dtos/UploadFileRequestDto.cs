namespace FileChunkSystem.Application.Dtos;

public sealed record UploadFileRequestDto
(
    Stream FileStream,
    string FileName,
    string MimeType,
    long FileSize,
    Dictionary<string, string>? Metadata
);