using FileChunkSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileChunkSystem.Infrastructure.Contexts;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public required DbSet<FileEntry> Files { get; set; }
    public required DbSet<ChunkEntry> Chunks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}