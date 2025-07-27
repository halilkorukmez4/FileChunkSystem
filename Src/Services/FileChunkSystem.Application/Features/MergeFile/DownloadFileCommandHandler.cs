using FileChunkSystem.Application.Abstractions.Coordinators;
using FileChunkSystem.Application.Dtos;
using Mediator;

namespace FileChunkSystem.Application.Features.MergeFile;

public sealed class DownloadFileCommandHandler(ICoordinator<DownloadFileRequestDto, DownloadFileCommandResponse> coordinator) : IRequestHandler<DownloadFileCommandRequest, List<DownloadFileCommandResponse>>
{
    public async ValueTask<List<DownloadFileCommandResponse>> Handle(DownloadFileCommandRequest request, CancellationToken cancellationToken) 
        => await coordinator.HandleAllAsync(request.Requests, cancellationToken);
}