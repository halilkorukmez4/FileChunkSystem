namespace FileChunkSystem.Application.Abstractions.Actors;

public interface IActor<TRequest, TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}