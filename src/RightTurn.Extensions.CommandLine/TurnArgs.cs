using System;

namespace RightTurn.Extensions.CommandLine
{
    internal class TurnArgs : ITurnArgs
    {
        public object Options { get; private set; }        
        
        public TurnArgs(object options)
        {
            Options = options;
        }

        public T GetOptions<T>()
        {
            return (T)Options;
        }

        public bool TryGetOptions<T>(out T args)
        {
            if (Options is T t)
            {
                args = t;
                return true;
            }
            else
            {
                args = default;
                return false;
            }
        }

    }
}
