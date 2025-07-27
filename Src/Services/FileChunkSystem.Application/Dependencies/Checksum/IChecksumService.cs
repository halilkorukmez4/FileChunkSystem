namespace FileChunkSystem.Application.Dependencies.Checksum;

public interface IChecksumService
{
    string ComputeChecksum(ReadOnlySpan<byte> data);
    Task<string> ComputeChecksumAsync(Stream stream, CancellationToken ct = default);
}