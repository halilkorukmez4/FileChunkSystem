using FileChunkSystem.Application.Dependencies.Checksum;
using FileChunkSystem.Application.Dtos;
using FileChunkSystem.Application.Helpers.Strategies;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace FileChunkSystem.Application.Dependencies.Chunking;

public sealed class ChunkingService(IChecksumService checksumService) : IChunkingService
{
    public async IAsyncEnumerable<ChunkPackageDto> SplitAsync(Stream input, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (!input.CanRead) throw new("Input stream is not readable.");
        if (!input.CanSeek) throw new("Input stream must support seeking.");

        var sequence = 0;

        foreach (var (offset, size) in ChunkStrategyHelper.DynamicChunks(input.Length, minSize: 3))
        {
            cancellationToken.ThrowIfCancellationRequested();

            input.Seek(offset, SeekOrigin.Begin);

            byte[] buffer = ArrayPool<byte>.Shared.Rent(size);

            try
            {
                int bytesRead = await input.ReadAsync(buffer.AsMemory(0, size), cancellationToken);

                if (bytesRead == 0) continue;

                var chunkBuffer = buffer.AsSpan(0, bytesRead).ToArray();

                string checksum = checksumService.ComputeChecksum(buffer.AsSpan(0, bytesRead));

                yield return new ChunkPackageDto
                {
                    Sequence = sequence++,
                    Offset = offset,
                    Size = bytesRead,
                    Buffer = chunkBuffer,
                    Checksum = checksum
                };
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }
}