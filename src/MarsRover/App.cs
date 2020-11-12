using System;
using System.Diagnostics.CodeAnalysis;
using MarsRover.Models;
using MarsRover.Services;

namespace MarsRover
{
    // Excluded since we want to tests the services
    [ExcludeFromCodeCoverage]
    public class App
    {
        private readonly IDispatcher _dispatcher;
        public App(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void Run(CmdOptions options)
        {
            // Drop off the rover in the specified location
            var controller = _dispatcher.SetPosition(options.InitialXPos, options.InitialYPos, options.Direction);

            // Move the rover as per instructions
            controller.Move(options.Command);

            // Write the final location of the rover
            Console.WriteLine(controller.Rover);
        }
    }
}
