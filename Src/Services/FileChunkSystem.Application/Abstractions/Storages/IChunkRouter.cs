using FileChunkSystem.Domain.Entities;

namespace FileChunkSystem.Application.Abstractions.Storages;

public interface IChunkRouter
{
    /// <summary>
    /// Yeni bir chunk için uygun providerId’yi döner.
    /// Örn: "pgsql", "mongo", "redis"
    /// </summary>
    string SelectProvider();

    /// <summary>
    /// Belirli bir chunk’ın tekrar okunması veya silinmesi için hangi provider kullanıldığını verir.
    /// (isteğe bağlı)
    /// </summary>
    string ResolveProviderForChunk(ChunkEntry chunk);
}