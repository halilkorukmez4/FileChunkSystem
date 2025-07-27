using FileChunkSystem.Application.Abstractions.Repositories;
using FileChunkSystem.Application.Abstractions.Storages;
using FileChunkSystem.Application.Common;
using FileChunkSystem.Infrastructure.Contexts;
using FileChunkSystem.Infrastructure.Repositories.EntityFramework.Chunk;
using FileChunkSystem.Infrastructure.Repositories.EntityFramework.File;
using FileChunkSystem.Infrastructure.Routing;
using FileChunkSystem.Infrastructure.Storages;
using FileChunkSystem.Infrastructure.Storages.FileSystem;
using FileChunkSystem.Infrastructure.Storages.Mongo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace FileChunkSystem.Infrastructure.Common;

public static class ConfigureServices
{
    public static void AddInfrastructureLayer(this IServiceCollection serviceCollection, ConfigurationManager configuration)
    {
        serviceCollection.AddMediator(opts => opts.ServiceLifetime = ServiceLifetime.Scoped);

        var postgresConnection = configuration["PostgreConnections:ConnectionString"];

        serviceCollection.AddDbContextPool<ApplicationDbContext>(options => options.UseNpgsql(postgresConnection));

        var mongoUrl = configuration["MongoConnections:Url"];
        var mongoDbName = configuration["MongoConnections:Database"];
        var mongoBucket = configuration["MongoConnections:Bucket"];

        serviceCollection.AddSingleton<IMongoClient>(sp => new MongoClient(mongoUrl));

        serviceCollection.AddSingleton<IGridFSBucket>(sp =>
        {
            var database = sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDbName);

            return new GridFSBucket(database, new GridFSBucketOptions{ BucketName = mongoBucket });
        });

        serviceCollection.AddScoped<IChunkRepository, EfChunkRepository>();
        serviceCollection.AddScoped<IFileRepository, EfFileRepository>();

        serviceCollection.AddScoped<IChunkRouter, RoundRobinChunkRouter>();
        serviceCollection.AddScoped<IChunkRouter, RandomChunkRouter>();
        serviceCollection.AddScoped<IChunkRouter, HealthAwareChunkRouter>();

        serviceCollection.AddKeyedSingleton<IStorageProvider, MongoStorageProvider>("mongo");
        serviceCollection.AddKeyedSingleton<IStorageProvider, FileSystemStorageProvider>("fs");

        serviceCollection.AddSingleton<RandomDispatchingStorageProvider>();

        serviceCollection.AddSingleton<IStorageProviderDispatcher>(sp => sp.GetRequiredService<RandomDispatchingStorageProvider>());

        serviceCollection.AddApplicationLayer();
    }
}