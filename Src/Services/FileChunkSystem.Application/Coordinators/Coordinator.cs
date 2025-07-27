using FileChunkSystem.Application.Abstractions.Actors;
using FileChunkSystem.Application.Abstractions.Coordinators;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Collections.Concurrent;

namespace FileChunkSystem.Application.Coordinators;

public sealed class Coordinator<TActor, TRequest, TResponse>(IServiceScopeFactory scopeFactory) : ICoordinator<TRequest, TResponse>  where TActor : IActor<TRequest, TResponse>
{
    public async Task<List<TResponse>> HandleAllAsync(IEnumerable<TRequest> requests, CancellationToken  cancellationToken)
    {
        ConcurrentBag<TResponse> results = [];

        await Parallel.ForEachAsync
        (
            requests,

            new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount, CancellationToken = cancellationToken },

            async (request, ct) =>
            {
                try
                {
                    await using var scope = scopeFactory.CreateAsyncScope();

                    var actor = scope.ServiceProvider.GetRequiredService<TActor>();

                    var result = await actor.HandleAsync(request, ct);

                    results.Add(result);
                }
                catch (Exception ex)
                {
                    Log.ForContext("Coordinator", new { Actor = typeof(TActor).Name, Request = request }, destructureObjects: true).Error(ex, "HandleAllAsync failed");
                }
            }
        );

        return [.. results];
    }
}