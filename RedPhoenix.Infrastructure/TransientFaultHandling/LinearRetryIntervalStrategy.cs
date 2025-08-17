namespace RedPhoenix.Infrastructure.TransientFaultHandling;
public class LinearRetryIntervalStrategy(TimeSpan initialInterval,
    TimeSpan increment,
    TimeSpan maximumInterval,
    bool immediateFirstRetry)
    : RetryIntervalStrategy(immediateFirstRetry)
{
    public TimeSpan InitialInterval { get; } = initialInterval;

    public TimeSpan Increment { get; } = increment;

    public TimeSpan MaximumInterval { get; } = maximumInterval;


    protected override TimeSpan GetIntervalFromZeroBasedTick(int tick)
        => TimeSpan.FromTicks(
            Math.Min(
                MaximumInterval.Ticks,
                InitialInterval.Ticks + (Increment.Ticks * tick)));

}