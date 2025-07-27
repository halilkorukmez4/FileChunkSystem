using FileChunkSystem.Domain.Common;

namespace FileChunkSystem.Domain.Entities;

public sealed class ChunkEntry : BaseEntity
{
    public required Guid FileEntryId { get; set; }                  // Ana dosya referansı
    public FileEntry? FileEntry { get; set; }
    public required int Sequence { get; set; }                 // 0, 1, 2...
    public required long Offset { get; set; }                  // Dosya içindeki pozisyon (byte)
    public required long Size { get; set; }                    // Byte cinsinden boyutu
    public required string Checksum { get; set; }              // SHA256 string
    public required string StorageProviderId { get; set; }     // Nerede tutuluyor
    public required bool IsReplica { get; set; }               // Yedek mi
    public string? StorageHandle { get; set; }
}