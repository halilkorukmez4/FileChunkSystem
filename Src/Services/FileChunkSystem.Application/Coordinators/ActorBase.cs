using FileChunkSystem.Application.Abstractions.Actors;

namespace FileChunkSystem.Application.Coordinators;

public abstract class ActorBase<TRequest, TResponse> : IActor<TRequest, TResponse>
{
    public async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await ExecuteAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            // burada loglama yaparız şimdi geçtim unutmazsam yapıcam

            throw new($"Actor '{typeof(TRequest).Name}' failed to execute.", ex);
        }
    }

    protected abstract Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken);
}