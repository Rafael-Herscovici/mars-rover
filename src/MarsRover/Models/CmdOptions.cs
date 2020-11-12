using CommandLine;
using MarsRover.Enums;

namespace MarsRover.Models
{
    public class CmdOptions
    {
        [Option('r', "RunWithDefaults", Required = false, Default = false, HelpText = "Run with default parameters.")]
        public bool ShouldRunWithDefaultParameters { get; set; }

        [Option('m', "Mode", Required = false, Default = Mode.S, HelpText = "\n\rA = Allow the rover to get off grid\n\rC = Get as close to boundary and continue processing.\n\rS = Get as close to boundary and stop processing.")]
        public Mode Mode { get; set; }

        [Option('x', "XPos", Required = false, Default = 10, HelpText = "Initial X Position.")]
        public int InitialXPos { get; set; }

        [Option('y', "YPos", Required = false, Default = 10, HelpText = "Initial Y Position.")]
        public int InitialYPos { get; set; }

        [Option('d', "Direction", Required = false, Default = Bearing.N, HelpText = "Initial direction.\n\rN = North\n\rE = East\n\rS = South\n\rW = West")]
        public Bearing Direction { get; set; }

        [Option('c', "Command", Required = false, Default = "R1R3L2L1", HelpText = "The command to process.\n\rNOTE: A command will not be processed if its invalid.")]
        public string Command { get; set; }
    }
}
