using FileChunkSystem.Application.Dtos;
using Mediator;

namespace FileChunkSystem.Application.Features.UploadFile;

public sealed record UploadFileCommandRequest(List<UploadFileRequestDto> Files) : IRequest<List<UploadFileCommandResponse>>;