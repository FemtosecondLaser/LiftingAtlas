using System;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Lift.
    /// </summary>
    [FlagsAttribute]
    public enum Lift : byte
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0,

        /// <summary>
        /// Squat.
        /// </summary>
        Squat = 1,

        /// <summary>
        /// Bench press.
        /// </summary>
        BenchPress = 2,

        /// <summary>
        /// Deadlift.
        /// </summary>
        Deadlift = 4
    }
}
