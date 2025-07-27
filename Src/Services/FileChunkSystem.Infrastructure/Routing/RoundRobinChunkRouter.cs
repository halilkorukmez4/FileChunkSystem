using FileChunkSystem.Application.Abstractions.Storages;
using FileChunkSystem.Domain.Entities;

namespace FileChunkSystem.Infrastructure.Routing;

public sealed class RoundRobinChunkRouter(IEnumerable<IStorageProvider> providers) : IChunkRouter
{
    private readonly IReadOnlyList<string> _providerIds = [.. providers.Select(p => p.ProviderId)];

    private int _currentIndex = -1;

    public string SelectProvider()
    {
        var index = Interlocked.Increment(ref _currentIndex);

        return _providerIds[index % _providerIds.Count];
    }

    public string ResolveProviderForChunk(ChunkEntry chunk) => chunk.StorageProviderId;
}