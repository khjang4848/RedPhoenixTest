namespace RedPhoenix.Infrastructure.TransientFaultHandling;
public interface IRetryPolicy
{
    Task Run(Func<CancellationToken, Task> operation,
        CancellationToken cancellation);

    Task Run<T>(Func<T, CancellationToken, Task> operation, T arg,
        CancellationToken cancellation);
}
