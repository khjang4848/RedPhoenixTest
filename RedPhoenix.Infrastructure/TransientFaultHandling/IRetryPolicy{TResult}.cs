namespace RedPhoenix.Infrastructure.TransientFaultHandling;
public interface IRetryPolicy<TResult> : IRetryPolicy
{
    Task<TResult> Run(
        Func<CancellationToken, Task<TResult>> operation,
        CancellationToken cancellationToken);

    Task<TResult> Run<T>(
        Func<T, CancellationToken, Task<TResult>> operation,
        T arg,
        CancellationToken cancellationToken);
}