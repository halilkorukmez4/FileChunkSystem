namespace FileChunkSystem.Domain.Enums;

public enum FileStatus
{
    Uploading,   // Chunking başladı
    Ready,       // Chunklar tamamlandı
    Completed,   // DB kayıtları tamamlandı
    Error,       // Chunk/store sırasında hata oldu
    Expired,     // Çok uzun süre Uploading kaldı
    Deleted
}