using FileChunkSystem.Application.Abstractions.Storages;
using FileChunkSystem.Domain.Entities;

namespace FileChunkSystem.Infrastructure.Routing;

public sealed class RandomChunkRouter(IEnumerable<IStorageProvider> providers) : IChunkRouter
{
    private readonly IReadOnlyList<string> _providerIds = [.. providers.Select(p => p.ProviderId)];

    private readonly Random _random = new();

    public string SelectProvider()
    {
        var index = _random.Next(_providerIds.Count);

        return _providerIds[index];
    }

    public string ResolveProviderForChunk(ChunkEntry chunk) => chunk.StorageProviderId;
}