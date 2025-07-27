using FileChunkSystem.Application.Coordinators;
using FileChunkSystem.Application.Dtos;
using FileChunkSystem.Application.Services.File;

namespace FileChunkSystem.Application.Features.MergeFile;

public sealed class DownloadFileActor(IFileService fileService) : ActorBase<DownloadFileRequestDto, DownloadFileCommandResponse>
{
    protected override async Task<DownloadFileCommandResponse> ExecuteAsync(DownloadFileRequestDto request, CancellationToken cancellationToken)
    {
        var file = await fileService.GetByIdAsync(request.FileId, cancellationToken) ?? throw new($"File not found. Id: {request.FileId}");

        var contentStream = await fileService.OpenReadStreamAsync(request.FileId, cancellationToken);

        return new(file.Name, file.MimeType, file.Size, contentStream);
    }
}