# RightTurn.Extensions.CommandLine

Provides [CommandLine Parser](https://github.com/commandlineparser/commandline) extensions for [RightTurn](https://github.com/Jandini/RightTurn)

## Quick Start

###### Parse verbs

```C#
static void Main(string[] args) => new Turn()
    .ParseVerbs(args)
    .WithDirections()
    .Take((provider) =>
    {
        var directions = provider.GetRequiredService<ITurnDirections>();

        switch (directions.Get<ITurnArgs>().Options)
        {
            case IVerbOptions1 options:
                provider.GetRequiredService<IVerbOptions1>().Run(options);
                break;

            case IVerbOptions2 options:
                provider.GetRequiredService<IVerbOptions2>().Run(options);
                break;

            default:
                throw new NotImplementedException();
        };
    });
```




###### Customize parser
```C#
new Turn()
    .WithParser(new Parser((settings) => { settings.HelpWriter = null; }))
    .ParseVerbs(args, (result) =>
    {
        Console.WriteLine(HelpText.AutoBuild(result, h =>
        {
            h.Heading = title;
            h.AdditionalNewLineAfterOption = false;
            h.Copyright = string.Empty;
            h.AddDashesToOption = true;

            return HelpText.DefaultParsingErrorsHandler(result, h);
        }, e => e, true));
    })
```



