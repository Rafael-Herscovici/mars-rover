using MarsRover.Enums;
using System;

namespace MarsRover.Models
{
    /// <summary>
    /// Rover interface
    /// </summary>
    public interface IRover
    {
        /// <summary>
        /// The rover's current X Position
        /// </summary>
        int PosX { get; set; }
        /// <summary>
        /// The rover's current Y Position
        /// </summary>
        int PosY { get; set; }
        /// <summary>
        /// The rover's <see cref="Bearing"/>
        /// </summary>
        Bearing Bearing { get; set; }
    }

    public class Rover : IRover
    {
        public int PosX { get; set; }
        public int PosY { get; set; }
        public Bearing Bearing { get; set; }

        public override string ToString()
        {
            return $"{PosX}, {PosY}, {Enum.GetName(typeof(Bearing), Bearing)}";
        }
    }
}
