namespace FileChunkSystem.Application.Features.UploadFile;

public sealed record UploadFileCommandResponse
(
    Guid FileId,
    string FileName,
    long Size,
    string Checksum
);