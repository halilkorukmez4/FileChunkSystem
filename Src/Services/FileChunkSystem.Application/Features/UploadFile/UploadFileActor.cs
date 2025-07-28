using FileChunkSystem.Application.Abstractions.Storages;
using FileChunkSystem.Application.Checksum;
using FileChunkSystem.Application.Chunking;
using FileChunkSystem.Application.Coordinators;
using FileChunkSystem.Application.Dtos;
using FileChunkSystem.Application.Services.Chunk;
using FileChunkSystem.Application.Services.File;
using FileChunkSystem.Domain.Entities;
using FileChunkSystem.Domain.Enums;

namespace FileChunkSystem.Application.Features.UploadFile;

public sealed class UploadFileActor(IFileService fileService, IChunkService chunkService, IChunkingService chunkingService, IChecksumService checksumService, IStorageProviderDispatcher storageProvider) : ActorBase<UploadFileRequestDto, UploadFileCommandResponse>
{
    protected override async Task<UploadFileCommandResponse> ExecuteAsync(UploadFileRequestDto request, CancellationToken cancellationToken)
    {
        var fileId = Guid.NewGuid();
        long totalSize = 0;
        List<ChunkEntry> chunkEntries = [];

        var fileEntry = new FileEntry
        {
            Id = fileId,
            Name = request.FileName,
            MimeType = request.MimeType,
            Metadata = request.Metadata,
            Chunks = [],
            Size = 0,
            Checksum = string.Empty
        };

        await foreach (var chunk in chunkingService.SplitAsync(request.FileStream, cancellationToken))
        {
            var resolvedProvider = storageProvider.Use();

            var chunkEntry = new ChunkEntry
            {
                FileEntryId = fileId,
                Sequence = chunk.Sequence,
                Offset = chunk.Offset,
                Size = chunk.Size,
                Checksum = chunk.Checksum,
                IsReplica = false,
                StorageProviderId = resolvedProvider.ProviderId
            };

            await using MemoryStream stream = new(chunk.Buffer, 0, chunk.Size, writable: false);
            chunkEntry.StorageHandle = await resolvedProvider.WriteChunkAsync(chunkEntry, stream, cancellationToken);
            chunkEntries.Add(chunkEntry);
            totalSize += chunk.Size;
        }

        if (request.FileStream.CanSeek)
            request.FileStream.Seek(0, SeekOrigin.Begin);

        var fileChecksum = await checksumService.ComputeChecksumAsync(request.FileStream, cancellationToken);

        fileEntry.Size = totalSize;
        fileEntry.Checksum = fileChecksum;
        fileEntry.Status = FileStatus.Completed;

        await fileService.AddAsync(fileEntry, cancellationToken);
        await chunkService.AddRangeAsync(chunkEntries, cancellationToken);

        return new(fileEntry.Id, fileEntry.Name, fileEntry.Size, fileEntry.MimeType);
    }
}