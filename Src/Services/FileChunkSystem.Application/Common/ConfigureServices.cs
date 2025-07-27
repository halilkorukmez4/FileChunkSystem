using FastExpressionCompiler;
using FileChunkSystem.Application.Abstractions.Actors;
using FileChunkSystem.Application.Abstractions.Coordinators;
using FileChunkSystem.Application.Coordinators;
using FileChunkSystem.Application.Dependencies.Checksum;
using FileChunkSystem.Application.Dependencies.Checksum.Sha256;
using FileChunkSystem.Application.Dependencies.Chunking;
using FileChunkSystem.Application.Dtos;
using FileChunkSystem.Application.Features.MergeFile;
using FileChunkSystem.Application.Features.UploadFile;
using FileChunkSystem.Application.Services.Chunk;
using FileChunkSystem.Application.Services.File;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FileChunkSystem.Application.Common;

public static class ConfigureServices
{
    public static void AddApplicationLayer(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMediator(opts => opts.ServiceLifetime = ServiceLifetime.Scoped);

        serviceCollection.AddMapsterMapperDepends();

        serviceCollection.AddScoped<IChecksumService, Sha256ChecksumService>();
        serviceCollection.AddScoped<IChunkingService, ChunkingService>();

        serviceCollection.AddScoped<IChunkService, ChunkService>();
        serviceCollection.AddScoped<IFileService, FileService>();

        serviceCollection.AddActorWithCoordinator<UploadFileActor, UploadFileRequestDto, UploadFileCommandResponse>();
        serviceCollection.AddActorWithCoordinator<DownloadFileActor, DownloadFileRequestDto, DownloadFileCommandResponse>();
    }

    public static void AddActorWithCoordinator<TActor, TRequest, TResponse>(this IServiceCollection services) where TActor : class, IActor<TRequest, TResponse>
    {
        services.AddScoped<TActor>();
        services.AddScoped<IActor<TRequest, TResponse>, TActor>();
        services.AddScoped<ICoordinator<TRequest, TResponse>, Coordinator<TActor, TRequest, TResponse>>();
    }

    private static void AddMapsterMapperDepends(this IServiceCollection serviceCollection)
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config.Compiler = exp => exp.CompileFast();

        config.Apply(config.Scan(Assembly.GetExecutingAssembly()));

        serviceCollection.AddSingleton(config).AddScoped<IMapper, ServiceMapper>();
    }
}