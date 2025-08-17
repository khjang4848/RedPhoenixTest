namespace RedPhoenix.Infrastructure.TransientFaultHandling;
public class DelegatingTransientFaultDetectionStrategy<T>(Func<Exception, bool> exceptionFunc,
    Func<T, bool> resultFunc)
    : TransientFaultDetectionStrategy<T>
{
    private static readonly Func<Exception, bool> _trueConstantExceptionFunc;

    private readonly Func<Exception, bool> _exceptionFunc = exceptionFunc ??
                                                            throw new ArgumentNullException(nameof(exceptionFunc));
    private readonly Func<T, bool> _resultFunc = resultFunc ??
                                                 throw new ArgumentNullException(nameof(resultFunc));


    public DelegatingTransientFaultDetectionStrategy(Func<T, bool> resultFunc)
        : this(_trueConstantExceptionFunc, resultFunc)
    {
    }

    static DelegatingTransientFaultDetectionStrategy()
    {
        _trueConstantExceptionFunc = exception => true;
    }

    public override bool IsTransientException(Exception exception)
    {
        if (exception == null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        return _exceptionFunc.Invoke(exception);
    }

    public override bool IsTransientResult(T result)
        => _resultFunc.Invoke(result);
}