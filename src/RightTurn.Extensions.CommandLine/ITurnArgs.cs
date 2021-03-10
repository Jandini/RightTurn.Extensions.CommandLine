namespace RightTurn.Extensions.CommandLine
{
    public interface ITurnArgs
    {
        object Args { get; }
        T GetArgs<T>();
        bool TryGetArgs<T>(out T args);
    }
}
