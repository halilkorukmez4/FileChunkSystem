namespace FileChunkSystem.Application.Checksum;

public interface IChecksumService
{
    string ComputeChecksum(ReadOnlySpan<byte> data);
    Task<string> ComputeChecksumAsync(Stream stream, CancellationToken cancellationToken = default);
}