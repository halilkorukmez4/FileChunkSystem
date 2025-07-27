namespace FileChunkSystem.Application.Abstractions.Coordinators;

public interface ICoordinator<TRequest, TResponse>
{
    Task<List<TResponse>> HandleAllAsync(IEnumerable<TRequest> requests, CancellationToken cancellationToken);
}