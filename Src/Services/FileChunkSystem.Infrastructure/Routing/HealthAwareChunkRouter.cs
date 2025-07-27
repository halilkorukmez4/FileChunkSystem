using FileChunkSystem.Application.Abstractions.Storages;
using FileChunkSystem.Domain.Entities;

namespace FileChunkSystem.Infrastructure.Routing;

public sealed class HealthAwareChunkRouter(IEnumerable<IStorageProvider> providers) : IChunkRouter
{
    private readonly IReadOnlyList<IStorageProvider> _providers = [.. providers];

    private readonly Random _random = new();

    public string SelectProvider()
    {
        var healthy = _providers.Where(p => p.HealthCheckAsync().GetAwaiter().GetResult()).ToList();

        if (healthy.Count == 0) throw new("No healthy providers available");

        var index = _random.Next(healthy.Count);

        return healthy[index].ProviderId;
    }

    public string ResolveProviderForChunk(ChunkEntry chunk) => chunk.StorageProviderId;
}