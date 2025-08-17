namespace RedPhoenix.Infrastructure.TransientFaultHandling;

using System.Threading;

public static class TransientFaultHandlingExtensions
{
    public static Task Run(this IRetryPolicy retryPolicy, Func<Task> operation)
    {
        if (retryPolicy == null)
        {
            throw new ArgumentNullException(nameof(retryPolicy));
        }

        if (operation == null)
        {
            throw new ArgumentNullException(nameof(operation));
        }

        return retryPolicy.Run(cancellationToken => operation.Invoke(),
            CancellationToken.None);
    }

    public static Task Run<T>(this IRetryPolicy retryPolicy,
        Func<T, Task> operation, T arg)
    {
        if (retryPolicy == null)
        {
            throw new ArgumentNullException(nameof(retryPolicy));
        }

        if (operation == null)
        {
            throw new ArgumentNullException(nameof(operation));
        }

        return retryPolicy.Run(MakeWrapper(operation), arg,
            CancellationToken.None);
    }

    public static Task<TResult> Run<TResult>(this IRetryPolicy<TResult> retryPolicy,
        Func<Task<TResult>> operation)
    {
        if (retryPolicy == null)
        {
            throw new ArgumentNullException(nameof(retryPolicy));
        }

        if (operation == null)
        {
            throw new ArgumentNullException(nameof(operation));
        }

        return retryPolicy.Run(
            cancellationToken => operation.Invoke(),
            CancellationToken.None);
    }

    public static Task<TResult> Run<TResult, T>(this IRetryPolicy<TResult> retryPolicy,
        Func<T, Task<TResult>> operation, T arg)
    {
        if (retryPolicy == null)
        {
            throw new ArgumentNullException(nameof(retryPolicy));
        }

        if (operation == null)
        {
            throw new ArgumentNullException(nameof(operation));
        }

        return retryPolicy.Run(
            MakeWrapper(operation),
            arg,
            CancellationToken.None);
    }

    private static Func<T, CancellationToken, Task> MakeWrapper<T>
        (Func<T, Task> operation)
        => (arg, cancellationToken) => operation.Invoke(arg);

    private static Func<T, CancellationToken, Task<TResult>> MakeWrapper<TResult, T>
        (Func<T, Task<TResult>> operation)
        => (arg, cancellationToken) => operation.Invoke(arg);
}