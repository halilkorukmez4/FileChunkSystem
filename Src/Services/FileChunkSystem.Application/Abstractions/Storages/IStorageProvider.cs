using FileChunkSystem.Domain.Entities;

namespace FileChunkSystem.Application.Abstractions.Storages;

/// <summary>
/// Her storage backend için plug-in abstraction (file system, DB, cloud vs).
/// </summary>
public interface IStorageProvider
{
    string ProviderId { get; }

    /// <summary>Chunk verisini sağlayıcıya yazar.</summary>
    Task<string> StoreChunkAsync(ChunkEntry chunk, Stream chunkData, CancellationToken cancellationToken = default);

    /// <summary>Chunk verisini sağlayıcıdan okur.</summary>
    Task<Stream> RetrieveChunkAsync(ChunkEntry chunk, CancellationToken cancellationToken = default);

    /// <summary>Sağlayıcının erişilebilir olup olmadığını kontrol eder.</summary>
    Task<bool> HealthCheckAsync(CancellationToken cancellationToken = default);
}