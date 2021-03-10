using System;

namespace RightTurn.Extensions.CommandLine
{
    internal class TurnArgs : ITurnArgs
    {
        public object Args { get; private set; }        
        
        public TurnArgs(object args)
        {
            Args = args;
        }

        public T GetArgs<T>()
        {
            return (T)Args;
        }

        public bool TryGetArgs<T>(out T args)
        {
            if (Args is T t)
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
