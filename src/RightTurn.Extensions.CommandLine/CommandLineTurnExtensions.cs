using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace RightTurn.Extensions.CommandLine
{
    public static class CommandLineTurnExtensions
    {
        private static Type[] LoadVerbs()
        {
            var verbs = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !new[] { "Microsoft.", "System.", "netstandard", "CommandLine.", "RightTurn.", "Serilog." }.Any(a => assembly.FullName.StartsWith(a, StringComparison.Ordinal)))
                .SelectMany(x => x.GetTypes())
                .Where(t => t.GetCustomAttribute<VerbAttribute>() != null)
                .ToArray();

            if (verbs.Length == 0)
                throw new Exception("At least one command line verb is required.");

            return verbs;
        }

        public static ITurn WithParser(this ITurn turn, Parser parser)
        {
            turn.Directions.Add(parser);
            return turn;
        }

        public static ITurn ParseVerbs(this ITurn turn, string[] args, Type[] verbs, Action<ParserResult<object>> unparsed = null)
        {
            object parsed = default;

            var parserResult = turn.GetParser().ParseArguments(args, verbs);

            parserResult
                .WithNotParsed((e) => { unparsed?.Invoke(parserResult); Environment.Exit(1); })
                .WithParsed((o) => { parsed = o; });

            turn.Directions.Add<ITurnArgs>(new TurnArgs(parsed));            

            return turn;
        }


        /// <summary>
        /// Parse command line options and adds the options object to the directions container. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="turn"></param>
        /// <param name="args"></param>
        /// <param name="unparsed"></param>
        /// <returns></returns>
        public static ITurn ParseOptions<T>(this ITurn turn, string[] args, Action<ParserResult<T>> unparsed = null)
        {
            T parsed = default;

            var result = turn.GetParser().ParseArguments<T>(args);

            result
                .WithNotParsed((e) => { unparsed?.Invoke(result); Environment.Exit(1); })
                .WithParsed((o) => { parsed = o; });

            turn.Directions.Add<ITurnArgs>(new TurnArgs(parsed));

            return turn;
        }

        /// <summary>
        /// Creates singleton from parsed options and adds it to the service collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="turn"></param>
        /// <returns></returns>
        public static ITurn WithOptionsAsSingleton<T>(this ITurn turn) where T: class
        {
            turn.Directions.Get<IServiceCollection>()
                .AddSingleton(turn.Directions.Get<ITurnArgs>().GetOptions<T>());

            return turn;
        }

        /// <summary>
        /// Creates singleton service with from parsed options as the implementation and adds it to the service collection.
        /// Another way to archive this without this extension: 
        /// .WithTurn((turn) => turn.Directions.ServiceCollection().AddSingleton<IArgs>(turn.Directions.Get<Args>()))
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="turn"></param>
        /// <returns></returns>
        public static ITurn WithOptionsAsSingleton<TService, TImplementation>(this ITurn turn) 
            where TService : class
            where TImplementation : class, TService
        {
            turn.Directions.Get<IServiceCollection>()
                .AddSingleton<TService>(turn.Directions.Get<ITurnArgs>().GetOptions<TImplementation>());
            return turn;
        }
             

        public static ITurn ParseOptions<T>(this ITurn turn, string[] args, out T options, Action<ParserResult<T>> unparsed = null)
        {
            turn.ParseOptions(args, unparsed);
            options = turn.Directions.Get<ITurnArgs>().GetOptions<T>();
            return turn;
        }

        public static ITurn ParseVerbs(this ITurn turn, string[] args, Action<ParserResult<object>> unparsed = null)
        {
            turn.ParseVerbs(args, turn.Directions.GetVerbs() ?? LoadVerbs(), unparsed);
            return turn;
        }
    }
}
