namespace FileChunkSystem.Application.Dtos;

public sealed class ChunkPackageDto
{
    public required int Sequence { get; init; }           // 0, 1, 2...
    public required long Offset { get; init; }            // Byte offset
    public required int Size { get; init; }               // Gerçek boyutu (chunkSize'den küçük olabilir)
    public required byte[] Buffer { get; init; }          // Chunk verisi
    public required string Checksum { get; init; }        // SHA256 hash
}