using FileChunkSystem.Application.Abstractions.Storages;
using Microsoft.Extensions.DependencyInjection;

namespace FileChunkSystem.Infrastructure.Storages;

public sealed class RandomDispatchingStorageProvider(IServiceProvider serviceProvider) : IStorageProviderDispatcher
{
    private readonly string[] _providerKeys = ["fs", "mongo"];
    private readonly Random _random = new();

    public IStorageProvider Use(string providerKey)
    {
        if (!_providerKeys.Contains(providerKey))
            throw new($"Unknown provider key: {providerKey}");

        return serviceProvider.GetRequiredKeyedService<IStorageProvider>(providerKey);
    }

    public IStorageProvider Use()
    {
        var providerKey = _providerKeys[_random.Next(_providerKeys.Length)];
        return serviceProvider.GetRequiredKeyedService<IStorageProvider>(providerKey);
    }
}