using System.Security.Cryptography;

namespace FileChunkSystem.Application.Dependencies.Checksum.Sha256;

public sealed class Sha256ChecksumService : IChecksumService
{
    public string ComputeChecksum(ReadOnlySpan<byte> data)
    {
        return Convert.ToHexString(SHA256.HashData(data.ToArray()));
    }

    public async Task<string> ComputeChecksumAsync(Stream stream, CancellationToken ct = default)
    {
        using var sha = SHA256.Create();

        var hash = await sha.ComputeHashAsync(stream, ct);

        stream.Position = 0;

        return Convert.ToHexString(hash);
    }
}