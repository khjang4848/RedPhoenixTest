namespace RedPhoenix.Infrastructure.TransientFaultHandling;
public class TransientFaultDetectionStrategy
{
    public virtual bool IsTransientException(Exception exception)
    {
        return exception == null ? throw new ArgumentNullException(nameof(exception)) : true;
    }
}