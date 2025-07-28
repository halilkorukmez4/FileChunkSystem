using FileChunkSystem.Domain.Entities;

namespace FileChunkSystem.Application.Abstractions.Storages;

public interface IStorageProvider
{
    string ProviderId { get; }

    Task<string> WriteChunkAsync(ChunkEntry chunk, Stream chunkData, CancellationToken cancellationToken = default);

    Task<Stream> ReadChunkAsync(ChunkEntry chunk, CancellationToken cancellationToken = default);

    Task<bool> HealthCheckAsync(CancellationToken cancellationToken = default);
}