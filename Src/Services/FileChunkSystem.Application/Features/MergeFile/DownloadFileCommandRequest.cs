using FileChunkSystem.Application.Dtos;
using Mediator;

namespace FileChunkSystem.Application.Features.MergeFile;

public sealed record DownloadFileCommandRequest(List<DownloadFileRequestDto> Requests) : IRequest<List<DownloadFileCommandResponse>>;