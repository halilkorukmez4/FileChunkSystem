using FileChunkSystem.Application.Abstractions.Repositories;
using FileChunkSystem.Domain.Entities;
using FileChunkSystem.Infrastructure.Contexts;

namespace FileChunkSystem.Infrastructure.Repositories.EntityFramework.File;

public sealed class EfFileRepository(ApplicationDbContext context) : EfRepository<FileEntry>(context: context), IFileRepository;