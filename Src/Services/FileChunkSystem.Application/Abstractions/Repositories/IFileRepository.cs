using FileChunkSystem.Domain.Entities;

namespace FileChunkSystem.Application.Abstractions.Repositories;

/// <summary>
/// FileEntry CRUD ve query abstraction. Uygulamada DI ile dışarıdan gelir.
/// </summary>
public interface IFileRepository : IRepository<FileEntry>;