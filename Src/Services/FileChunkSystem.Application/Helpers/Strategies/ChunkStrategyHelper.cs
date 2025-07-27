namespace FileChunkSystem.Application.Helpers.Strategies;

public static class ChunkStrategyHelper
{
    // Sabit boyutlu parçalama
    public static IEnumerable<(long Offset, int Size)> FixedChunks(long fileSize, int chunkSize)
    {
        if (fileSize <= 0 || chunkSize <= 0)
            yield break;

        long offset = 0;
        while (offset < fileSize)
        {
            int size = (int)Math.Min(chunkSize, fileSize - offset);
            yield return (offset, size);
            offset += size;
        }
    }

    //chunk sayısına göre parçalama
    public static IEnumerable<(long Offset, int Size)> DynamicChunks(long fileSize, int targetChunkCount = 16, int minSize = 512 * 1024, int maxSize = 8 * 1024 * 1024)
    {
        if (fileSize <= 0 || targetChunkCount <= 0)
            yield break;

        int optimalSize = (int)Math.Clamp(fileSize / targetChunkCount, minSize, maxSize);

        long offset = 0;

        while (offset < fileSize)
        {
            int size = (int)Math.Min(optimalSize, fileSize - offset);

            yield return (offset, size);

            offset += size;
        }
    }
}
