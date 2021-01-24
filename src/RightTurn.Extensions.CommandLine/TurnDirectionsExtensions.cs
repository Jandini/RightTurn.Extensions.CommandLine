using CommandLine;
using System;

namespace RightTurn.Extensions.CommandLine
{
    public static class TurnDirectionsExtensions
    {
        public static Type[] GetVerbs(this ITurnDirections directions) => directions.TryGet<Type[]>();
        public static Parser GetParser(this ITurn turn) => turn.Directions.TryGet<Parser>() ?? Parser.Default;
    }
}
