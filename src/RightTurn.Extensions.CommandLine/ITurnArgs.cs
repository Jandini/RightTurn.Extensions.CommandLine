namespace RightTurn.Extensions.CommandLine
{
    public interface ITurnArgs
    {
        object Options { get; }
        T GetOptions<T>();
        bool TryGetOptions<T>(out T args);
    }
}
