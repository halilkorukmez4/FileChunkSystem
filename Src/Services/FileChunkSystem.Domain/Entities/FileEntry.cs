using FileChunkSystem.Domain.Common;
using FileChunkSystem.Domain.Enums;

namespace FileChunkSystem.Domain.Entities;

public sealed class FileEntry : BaseEntity
{
    public required string Name { get; set; }                       // Dosya adı (örneğin 'invoice-2023.pdf')

    public required long Size { get; set; }                         // Byte cinsinden toplam boyut

    public required string Checksum { get; set; }                   // SHA256
                                                                   
    public required string MimeType { get; set; }                   // application/pdf, image/jpeg vs.

    public string? OriginalFullPath { get; set; }                   // upload edildiği tam yol (isteğe bağlı)
                                                                   
    public int Version { get; set; } = 1;                           // Versiyon takibi (0.1, 1, 2 ...)

    public FileStatus Status { get; set; } = FileStatus.Uploading;

    public DateTime? LastAccessedAt { get; set; }

    public Dictionary<string, string>? Metadata { get; set; }       // İsteğe bağlı ekstra bilgiler

    public required IList<ChunkEntry> Chunks { get; set; }  // Parçalar
}