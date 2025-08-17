namespace RedPhoenix.Infrastructure.TransientFaultHandling;

public class DelegatingRetryIntervalStrategy(Func<int, TimeSpan> func,
    bool immediateFirstRetry)
    : RetryIntervalStrategy(immediateFirstRetry)
{
    private readonly Func<int, TimeSpan> _func = func
       ?? throw new ArgumentNullException(nameof(func));


    protected override TimeSpan GetIntervalFromZeroBasedTick(int tick)
        => _func.Invoke(tick);
}