using FileChunkSystem.Application.Abstractions.Storages;
using FileChunkSystem.Domain.Entities;
using FileChunkSystem.Infrastructure.Constants;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace FileChunkSystem.Infrastructure.Storages.Mongo;

public sealed class MongoStorageProvider(IGridFSBucket bucket) : IStorageProvider
{
    public string ProviderId => StorageProviderTypes.MongoDb;

    public async Task<string> WriteChunkAsync(ChunkEntry chunk, Stream chunkData, CancellationToken ct = default)
    {
        var fileName = $"chunk_{chunk.Id}";

        var objectId = await bucket.UploadFromStreamAsync(fileName, chunkData, cancellationToken: ct);

        return objectId.ToString();
    }

    public async Task<Stream> ReadChunkAsync(ChunkEntry chunk, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(chunk.StorageHandle))
            throw new($"StorageHandle is missing for chunk {chunk.Id}");

        if (!ObjectId.TryParse(chunk.StorageHandle, out var objectId))
            throw new($"Invalid MongoDB ObjectId: {chunk.StorageHandle}");

        MemoryStream stream = new((int)chunk.Size);

        await bucket.DownloadToStreamAsync(objectId, stream, cancellationToken: ct);

        stream.Position = 0;

        if (stream.Length == 0)
            throw new("MongoDB: Chunk {ChunkId} retrieved but EMPTY!");
        
        return stream;
    }

    public async Task<bool> HealthCheckAsync(CancellationToken ct = default)
    {
        var collections = await bucket.Database.ListCollectionNamesAsync(cancellationToken: ct);

        await collections.AnyAsync(ct);

        return true;
    }
}