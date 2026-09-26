using Lab4.Commands.Parsing.Parameters;

namespace Lab4.Commands.Parsing.Flags;

public abstract class FlagHandlerBase<TBuilder> : IFlagHandler<TBuilder>
{
    private IFlagHandler<TBuilder>? _next;

    public IFlagHandler<TBuilder> AddNext(IFlagHandler<TBuilder> next)
    {
        if (_next is null)
            _next = next;
        else
            _next.AddNext(next);

        return this;
    }

    public abstract ParameterResult Handle(string flag, ArgumentReader reader, TBuilder builder);

    protected ParameterResult HandleNext(string flag, ArgumentReader reader, TBuilder builder)
    {
        return _next?.Handle(flag, reader, builder) ?? ParameterResult.NotHandled;
    }
}
