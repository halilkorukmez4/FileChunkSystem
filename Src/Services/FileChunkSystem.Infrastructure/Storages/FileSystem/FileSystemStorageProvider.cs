using FileChunkSystem.Application.Abstractions.Storages;
using FileChunkSystem.Domain.Entities;
using FileChunkSystem.Infrastructure.Constants;
using Microsoft.Extensions.Configuration;

namespace FileChunkSystem.Infrastructure.Storages.FileSystem;

public sealed class FileSystemStorageProvider : IStorageProvider
{
    public string ProviderId => StorageProviderTypes.FileSystem;

    private readonly string _rootDirectory;

    public FileSystemStorageProvider(IConfiguration configuration)
    {
        _rootDirectory = configuration["StorageRoot"] ?? throw new("StorageRoot config value is missing.");

        Directory.CreateDirectory(_rootDirectory);
    }

    public async Task<string> WriteChunkAsync(ChunkEntry chunk, Stream chunkData, CancellationToken cancellationToken = default)
    {
        var subFolder = Path.Combine(_rootDirectory, chunk.FileEntryId.ToString("N"));

        Directory.CreateDirectory(subFolder);

        var fileName = $"{chunk.Sequence:D6}-{chunk.Id:N}.chunk";

        var fullPath = Path.Combine(subFolder, fileName);

        await using var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);

        await chunkData.CopyToAsync(fileStream, cancellationToken);

        await fileStream.FlushAsync(cancellationToken);

        return Path.GetRelativePath(_rootDirectory, fullPath);
    }

    public async Task<Stream> ReadChunkAsync(ChunkEntry chunk, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(_rootDirectory, chunk.StorageHandle ?? string.Empty);

        if (!File.Exists(fullPath)) throw new($"Chunk not found. Path : {fullPath}");

        var stream = new FileStream
        (
            path: fullPath,
            mode: FileMode.Open,
            access: FileAccess.Read,
            share: FileShare.Read,
            bufferSize: 81920,
            options: FileOptions.Asynchronous | FileOptions.SequentialScan
        );

        return await Task.FromResult(stream);
    }

    public Task<bool> HealthCheckAsync(CancellationToken cancellationToken = default)
    {
        var testFile = Path.Combine(_rootDirectory, "healthcheck.tmp");

        File.WriteAllText(testFile, "OK");

        File.Delete(testFile);

        return Task.FromResult(true);
    }
}
