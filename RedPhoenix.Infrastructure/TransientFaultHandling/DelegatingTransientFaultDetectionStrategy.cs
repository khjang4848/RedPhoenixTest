namespace RedPhoenix.Infrastructure.TransientFaultHandling;
public class DelegatingTransientFaultDetectionStrategy(Func<Exception, bool> func)
    : TransientFaultDetectionStrategy
{
    private readonly Func<Exception, bool> _func = func
          ?? throw new ArgumentNullException(nameof(func));

    public override bool IsTransientException(Exception exception)
    {
        return exception == null ? throw new ArgumentNullException(nameof(exception))
            : _func.Invoke(exception);
    }
}