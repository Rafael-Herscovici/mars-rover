namespace MarsRover.Enums
{
    public enum Mode
    {
        /// <summary>
        /// Allow the rover to go out of boundaries
        /// </summary>
        A,
        /// <summary>
        /// When the rover is instructed to cross boundary,
        /// Get as close to the boundary as possible and Continue processing commands
        /// </summary>
        C,
        /// <summary>
        /// When the rover is instructed to cross boundary,
        /// Get as close to the boundary as possible and Stop processing further commands
        /// </summary>
        S
    }
}
