using CommandLine;
using CommandLine.Text;
using MarsRover.Models;
using MarsRover.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MarsRover
{
    // Excluded since we want to tests the services
    [ExcludeFromCodeCoverage]
    internal class Program
    {
        private static int Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<CmdOptions>(args);

            var exitCode = ValidateArguments(args, result);
            if (exitCode == 0)
            {
                return Init(result);
            }

            Console.WriteLine(HelpText.AutoBuild(result, _ => _, _ => _));
            return exitCode;

        }

        private static int Init(ParserResult<CmdOptions> result)
        {
            try
            {
                result
                    .WithParsed(opt =>
                    {
                        var serviceProvider = new ServiceCollection()
                            .AddSingleton(options => new ControllerOptions { Mode = opt.Mode })
                            .AddScoped<IDispatcher, Dispatcher>()
                            .AddScoped<IController, Controller>()
                            .BuildServiceProvider();

                        var dispatcher = serviceProvider.GetRequiredService<IDispatcher>();
                        new App(dispatcher).Run(opt);
                    });
                return 0;
            }
            // Even though its not ok to catch a generic exception,
            // This is our only way to catch ANY exception from the app and return a failed exitCode
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static int ValidateArguments(IReadOnlyList<string> args, ParserResult<CmdOptions> parserResult)
        {
            var exitCode = 0;
            if (!args.Any() || args[0].Equals("help", StringComparison.InvariantCultureIgnoreCase))
            {
                exitCode = 1;
            }

            switch (parserResult.Tag)
            {
                case ParserResultType.NotParsed:
                    parserResult.WithNotParsed(HandleParseError);
                    exitCode = 1;
                    break;
                case ParserResultType.Parsed:
                    parserResult.WithParsed(x =>
                    {
                        if (x.ShouldRunWithDefaultParameters)
                        {
                            exitCode = 0;
                        }
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ParserResultType));
            }

            return exitCode;
        }

        private static void HandleParseError(IEnumerable errors)
        {
            foreach (var err in errors)
            {
                Console.WriteLine(err.ToString());
            }
        }
    }
}
