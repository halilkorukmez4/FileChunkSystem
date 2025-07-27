using FileChunkSystem.Application.Abstractions.Repositories;
using FileChunkSystem.Domain.Entities;

namespace FileChunkSystem.Application.Services.Chunk;

public sealed class ChunkService(IChunkRepository chunkRepository) : IChunkService
{
    public Task<List<ChunkEntry>> GetByFileIdAsync(Guid fileId, CancellationToken cancellationToken = default)
        => chunkRepository.GetListAsync(x => x.FileEntryId == fileId, cancellationToken);

    public Task<ChunkEntry?> GetByIdAsync(Guid chunkId, CancellationToken cancellationToken = default)
        => chunkRepository.GetByIdAsync(chunkId, cancellationToken);

    public Task AddAsync(ChunkEntry chunk, CancellationToken cancellationToken = default)
        => chunkRepository.AddAsync(chunk, cancellationToken);

    public async Task AddRangeAsync(IEnumerable<ChunkEntry> chunks, CancellationToken cancellationToken = default)
    {
        await chunkRepository.AddRangeAsync(chunks, cancellationToken);

        await chunkRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteByFileIdAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        var model = await chunkRepository.GetByIdAsync(fileId, cancellationToken);

        if (model is null) return;

        await chunkRepository.RemoveAsync(model, cancellationToken);
    }
}