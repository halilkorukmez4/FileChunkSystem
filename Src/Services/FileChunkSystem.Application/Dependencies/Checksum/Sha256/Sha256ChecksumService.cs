using System.Security.Cryptography;

namespace FileChunkSystem.Application.Dependencies.Checksum.Sha256;

public sealed class Sha256ChecksumService : IChecksumService
{
    public string ComputeChecksum(ReadOnlySpan<byte> data)
    {
        Span<byte> hash = stackalloc byte[32];

        SHA256.HashData(data, hash);

        return Convert.ToHexString(hash);
    }

    public async Task<string> ComputeChecksumAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        if (stream is null) throw new(nameof(stream));

        if (!stream.CanRead) throw new("Stream is not readable.");

        using var sha = SHA256.Create();

        byte[] hash = await sha.ComputeHashAsync(stream, cancellationToken);

        if (stream.CanSeek) stream.Position = 0;

        return Convert.ToHexString(hash);
    }
}