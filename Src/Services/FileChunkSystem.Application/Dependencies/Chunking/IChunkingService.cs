using FileChunkSystem.Application.Dtos;

namespace FileChunkSystem.Application.Dependencies.Chunking;

public interface IChunkingService
{
    /// <summary>
    /// Dosya stream’ini chunk’lara böler ve her bir chunk’ı metadata + içerik olarak döner.
    /// </summary>
    IAsyncEnumerable<ChunkPackageDto> SplitAsync(Stream input, CancellationToken cancellationToken = default);
}