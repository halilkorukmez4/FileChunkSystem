using FileChunkSystem.Domain.Entities;

namespace FileChunkSystem.Application.Services.Chunk;

public interface IChunkService
{
    Task<List<ChunkEntry>> GetByFileIdAsync(Guid fileId, CancellationToken cancellationToken = default);
    Task<ChunkEntry?> GetByIdAsync(Guid chunkId, CancellationToken cancellationToken = default);
    Task AddAsync(ChunkEntry chunk, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<ChunkEntry> chunks, CancellationToken cancellationToken = default);
    Task DeleteByFileIdAsync(Guid fileId, CancellationToken cancellationToken = default);
}