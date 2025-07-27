using FileChunkSystem.Application.Abstractions.Repositories;
using FileChunkSystem.Application.Abstractions.Storages;
using FileChunkSystem.Domain.Entities;
using FileChunkSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Buffers;
using System.IO.Pipelines;

namespace FileChunkSystem.Application.Services.File;

public sealed class FileService(IFileRepository fileRepository, IStorageProviderDispatcher storageProvider) : IFileService
{
    public Task<FileEntry?> GetByIdAsync(Guid fileId, CancellationToken cancellationToken = default)
        => fileRepository.GetByIdAsync(fileId, cancellationToken);

    public async Task AddAsync(FileEntry file, CancellationToken cancellationToken = default)
    {
        await fileRepository.AddAsync(file, cancellationToken);

        await fileRepository.SaveChangesAsync(cancellationToken);
    }

    public Task<List<FileEntry>> GetStuckFilesAsync(DateTime before, CancellationToken cancellationToken)
    {
        return fileRepository
            .Set()
            .Where(f => f.Status == FileStatus.Uploading && f.CreatedAt < before)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateStatusAsync(Guid fileId, FileStatus newStatus, CancellationToken cancellationToken)
    {
        await fileRepository
            .Set()
            .Where(f => f.Id == fileId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(f => f.Status, newStatus), cancellationToken);
    }

    public async Task<Stream> OpenReadStreamAsync(Guid fileId, CancellationToken cancellationToken = default)
    {
        var file = await fileRepository.Set().Select(x => new FileEntry
        {
            Checksum = x.Checksum,
            Chunks = x.Chunks,
            MimeType = x.MimeType,
            Name = x.Name,
            Size = x.Size,
            Id = x.Id,
            Metadata = x.Metadata,
            OriginalFullPath = x.OriginalFullPath,
            Status = x.Status,
            Version = x.Version

        }).FirstOrDefaultAsync(x => x.Id == fileId, cancellationToken) ?? throw new("File not found.");

        var chunks = file.Chunks.OrderBy(c => c.Sequence).ToList();

        if (chunks.Count == 0)
            throw new("No chunks found for file.");

        var pipe = new Pipe(new PipeOptions(pauseWriterThreshold: 512 * 1024, resumeWriterThreshold: 256 * 1024));

        _ = Task.Run(async () =>
        {
            var buffer = ArrayPool<byte>.Shared.Rent(128 * 1024);

            try
            {
                var writer = pipe.Writer;

                for (var i = 0; i < chunks.Count; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var chunk = chunks[i];

                    var provider = storageProvider.Use(chunk.StorageProviderId);

                    await using var chunkStream = await provider.RetrieveChunkAsync(chunk, cancellationToken);

                    if (chunkStream == null || !chunkStream.CanRead)
                        throw new($"Chunk {chunk.Id} stream is not readable.");

                    int bytesRead;

                    while ((bytesRead = await chunkStream.ReadAsync(buffer.AsMemory(0, buffer.Length), cancellationToken)) > 0)
                    {
                        var memory = writer.GetMemory(bytesRead);

                        buffer.AsSpan(0, bytesRead).CopyTo(memory.Span);

                        writer.Advance(bytesRead);

                        var flushResult = await writer.FlushAsync(cancellationToken);

                        if (flushResult.IsCompleted || flushResult.IsCanceled) break;
                    }
                }

                await writer.CompleteAsync();
            }
            catch (Exception ex)
            {
                await pipe.Writer.CompleteAsync(ex);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }

        }, cancellationToken);

        return pipe.Reader.AsStream();
    }
}