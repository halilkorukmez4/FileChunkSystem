using FileChunkSystem.Domain.Entities;

namespace FileChunkSystem.Application.Abstractions.Repositories;

/// <summary>
/// ChunkEntry CRUD ve bulk işlemler.
/// </summary>
public interface IChunkRepository : IRepository<ChunkEntry>;