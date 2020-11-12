using MarsRover.Enums;
using MarsRover.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace MarsRover.Services
{
    public interface IController
    {
        /// <summary>
        /// Move the rover
        /// </summary>
        /// <param name="commandSequence">A command sequence</param>
        IController Move(string commandSequence);
        /// <summary>
        /// The rover the controller is controlling
        /// </summary>
        Rover Rover { get; set; }
    }

    /// <summary>
    /// This would be our Rover control software
    /// Responsibilities: Parse received commands, Process received commands (Currently only Move)
    /// </summary>
    public class Controller : IController
    {
        private bool _shouldProcessCommand = true;

        public Controller(ControllerOptions options)
        {
            Options = options;
        }

        public Rover Rover { get; set; }
        public ControllerOptions Options { get; set; }

        public IController Move(string commandSequence)
        {
            if (!ValidateAndParseCommands(commandSequence, out var commands))
            {
                return this;
            }

            foreach (var str in commands)
            {
                // We test those just in case someone changed our regex in the validation
                if (!int.TryParse(str[1..], out var steps))
                {
                    throw new InvalidCastException(nameof(steps));
                }

                if (!char.TryParse(str.Substring(0, 1), out var direction) &&
                    !Enum.IsDefined(typeof(Direction), (int)direction))
                {
                    throw new ArgumentOutOfRangeException(nameof(Direction));
                }

                Move((Direction)direction, steps);
            }

            return this;
        }

        /// <summary>
        /// Moves the _rover 
        /// </summary>
        /// <param name="direction">The bearing we want to move it in (string formatted as 'L' or 'R')</param>
        /// <param name="steps">The steps to move</param>
        private void Move(Direction direction, int steps)
        {
            if (!_shouldProcessCommand)
            {
                return;
            }

            SetRoverBearingByDirection(Rover, direction);

            if (Rover.Bearing == Bearing.W || Rover.Bearing == Bearing.S)
            {
                steps *= -1;
            }

            switch (Rover.Bearing)
            {
                case Bearing.E:
                case Bearing.W:
                    Rover.PosX = CalculatePosition(Rover.PosX, steps);
                    break;
                case Bearing.N:
                case Bearing.S:
                    Rover.PosY = CalculatePosition(Rover.PosY, steps);
                    break;
                default:
                    // just in case someone modifies our enum, for example, adds NW
                    throw new ArgumentOutOfRangeException(nameof(Bearing));
            }
        }

        private int CalculatePosition(int pos, int steps)
        {
            var diff = pos + steps;
            if (diff < 0)
            {
                // The calculation took us out of boundary, should we stop processing the next commands?
                _shouldProcessCommand = Options.Mode == Mode.C || Options.Mode == Mode.A;
            }

            if (Options.Mode == Mode.A)
            {
                return steps + pos;
            }

            // Calculate how many steps to get to the boundary
            while (diff < 0)
            {
                steps++;
                diff++;
            }
            return steps + pos;
        }

        private static void SetRoverBearingByDirection(IRover rover, Direction direction)
        {
            var arr = Enum.GetValues(typeof(Bearing));
            var currentIndex = Array.IndexOf(arr, rover.Bearing);

            switch (direction)
            {
                case Direction.Right:
                    rover.Bearing = (Bearing)(currentIndex == 3 ? 0 : currentIndex + 1);
                    return;
                case Direction.Left:
                    rover.Bearing = (Bearing)(currentIndex == 0 ? arr.Length - 1 : currentIndex - 1);
                    return;
            }
        }

        private static bool ValidateAndParseCommands(string commandSequence, out string[] commands)
        {
            commands = GetCommandsFromString(commandSequence);

            if (!commandSequence.Contains(" ") &&
                (commands.Any() && commands.All(x => x.StartsWith("L") || x.StartsWith("R"))))
            {
                return true;
            }

            commands = new string[0];
            return false;
        }

        private static string[] GetCommandsFromString(string cmd)
        {
            return (Regex.Split(cmd, @"([L,R]-?\d+)", RegexOptions.IgnoreCase | RegexOptions.Compiled))
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();
        }
    }
}
