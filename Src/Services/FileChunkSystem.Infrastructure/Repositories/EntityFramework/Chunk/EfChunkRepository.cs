using FileChunkSystem.Application.Abstractions.Repositories;
using FileChunkSystem.Domain.Entities;
using FileChunkSystem.Infrastructure.Contexts;

namespace FileChunkSystem.Infrastructure.Repositories.EntityFramework.Chunk;

public sealed class EfChunkRepository(ApplicationDbContext context) : EfRepository<ChunkEntry>(context: context), IChunkRepository;