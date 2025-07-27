namespace FileChunkSystem.Application.Features.MergeFile;

public sealed record DownloadFileCommandResponse
(
    string FileName,
    string MimeType,
    long Size,
    Stream Content
);