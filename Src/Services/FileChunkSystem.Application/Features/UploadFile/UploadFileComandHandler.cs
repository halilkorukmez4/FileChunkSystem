using FileChunkSystem.Application.Abstractions.Coordinators;
using FileChunkSystem.Application.Dtos;
using Mediator;

namespace FileChunkSystem.Application.Features.UploadFile;

public sealed class UploadFileCommandHandler(ICoordinator<UploadFileRequestDto, UploadFileCommandResponse> coordinator) : IRequestHandler<UploadFileCommandRequest, List<UploadFileCommandResponse>>
{
    public async ValueTask<List<UploadFileCommandResponse>> Handle(UploadFileCommandRequest request, CancellationToken cancellationToken)
        => await coordinator.HandleAllAsync(request.Files, cancellationToken);
}