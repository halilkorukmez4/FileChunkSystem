using FileChunkSystem.Domain.Entities;
using FileChunkSystem.Domain.Enums;

namespace FileChunkSystem.Application.Services.File;

public interface IFileService
{
    Task<FileEntry?> GetByIdAsync(Guid fileId, CancellationToken cancellationToken = default);
    Task<Stream> OpenReadStreamAsync(Guid fileId, CancellationToken cancellationToken = default);
    Task AddAsync(FileEntry file, CancellationToken cancellationToken = default);
    Task<List<FileEntry>> GetStuckFilesAsync(DateTime before, CancellationToken cancellationToken);
    Task UpdateStatusAsync(Guid fileId, FileStatus newStatus, CancellationToken cancellationToken);
}