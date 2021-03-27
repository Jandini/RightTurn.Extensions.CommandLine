using Microsoft.Extensions.DependencyInjection;
using System;

namespace RightTurn.Extensions.CommandLine
{
    public static class TurnCommandLine
    {
        public static int Take<TService, TImplementation, TOptions>(string[] args, Func<TService, int> run, Action<IServiceCollection> services = null)
            where TOptions : class
            where TService : class
            where TImplementation : class, TService => new Turn()
                .ParseOptions<TOptions>(args)
                .WithOptionsAsSingleton<TOptions>()
                .WithServices(services)
                .Take<TService, TImplementation>(run);

        public static void Take<TService, TImplementation, TOptions>(string[] args, Action<TService> run, Action<IServiceCollection> services = null)
            where TOptions : class
            where TService : class
            where TImplementation : class, TService => new Turn()
                .ParseOptions<TOptions>(args)
                .WithOptionsAsSingleton<TOptions>()
                .WithServices(services)
                .WithDirections()
                .Take<TService, TImplementation>(run);

        public static void Take<TService, TImplementation>(string[] args, Action<TService> run, Action<IServiceCollection> services = null)
            where TService : class
            where TImplementation : class, TService => new Turn()
                .ParseVerbs(args)
                .WithServices(services)
                .Take<TService, TImplementation>((service) => { run.Invoke(service); return 0; });


        public static int Take<TService, TImplementation>(string[] args, Func<TService, int> run, Action<IServiceCollection> services = null)
            where TService : class
            where TImplementation : class, TService => new Turn()
                .ParseVerbs(args)
                .WithServices((services) => services.AddTransient<TService, TImplementation>())
                .WithServices(services)
                .Take<TService, TImplementation>(run);


        public static TService Take<TService, TImplementation>(string[] args, Action<IServiceCollection> services = null)
            where TService : class
            where TImplementation : class, TService => new Turn()
                .ParseVerbs(args)
                .WithServices((services) => { services.AddTransient<TService, TImplementation>(); })
                .WithServices(services)
                .Take()
                .GetRequiredService<TService>();

        public static int Take(string[] args, Action<IServiceCollection> services, Func<IServiceProvider, int> run) => new Turn()
            .ParseVerbs(args)
            .WithServices(services)
            .Take(run);
    }
}
