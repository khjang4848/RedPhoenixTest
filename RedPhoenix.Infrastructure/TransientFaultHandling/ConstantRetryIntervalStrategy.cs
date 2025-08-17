namespace RedPhoenix.Infrastructure.TransientFaultHandling;

public class ConstantRetryIntervalStrategy(TimeSpan interval, bool immediateFirstRetry)
    : RetryIntervalStrategy(immediateFirstRetry)
{
    public ConstantRetryIntervalStrategy(TimeSpan interval)
        : this(interval, immediateFirstRetry: false)
    {
    }

    public TimeSpan Interval { get; } = interval;

    protected override TimeSpan GetIntervalFromZeroBasedTick(int tick)
        => Interval;
}
