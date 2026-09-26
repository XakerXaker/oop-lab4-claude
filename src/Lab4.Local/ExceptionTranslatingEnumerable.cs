using System.Collections;

namespace Lab4.Local;

/// <summary>
/// Translates exceptions thrown lazily during enumeration (a <c>yield</c> iterator cannot contain try/catch).
/// </summary>
internal sealed class ExceptionTranslatingEnumerable<T> : IEnumerable<T>
{
    private readonly IEnumerable<T> _inner;
    private readonly Func<Exception, Exception> _translate;

    public ExceptionTranslatingEnumerable(IEnumerable<T> inner, Func<Exception, Exception> translate)
    {
        _inner = inner;
        _translate = translate;
    }

    public IEnumerator<T> GetEnumerator() => new Enumerator(Guard(_inner.GetEnumerator), _translate);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private TResult Guard<TResult>(Func<TResult> action)
    {
        try
        {
            return action();
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException)
        {
            throw _translate(e);
        }
    }

    private sealed class Enumerator : IEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;
        private readonly Func<Exception, Exception> _translate;

        public Enumerator(IEnumerator<T> inner, Func<Exception, Exception> translate)
        {
            _inner = inner;
            _translate = translate;
        }

        public T Current => _inner.Current;

        object? IEnumerator.Current => Current;

        public bool MoveNext()
        {
            try
            {
                return _inner.MoveNext();
            }
            catch (Exception e) when (e is IOException or UnauthorizedAccessException)
            {
                throw _translate(e);
            }
        }

        public void Reset() => _inner.Reset();

        public void Dispose() => _inner.Dispose();
    }
}
