using System;
using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Resolver of <see cref="Lift"/>.
    /// </summary>
    public static class LiftResolver
    {
        #region Private fields

        /// <summary>
        /// <see cref="Lift.None"/> string representation.
        /// </summary>
        private const string none = "None";

        /// <summary>
        /// <see cref="Lift.Squat"/> string representation.
        /// </summary>
        private const string squat = "Squat";

        /// <summary>
        /// <see cref="Lift.BenchPress"/> string representation.
        /// </summary>
        private const string benchPress = "Bench Press";

        /// <summary>
        /// <see cref="Lift.Deadlift"/> string representation.
        /// </summary>
        private const string deadlift = "Deadlift";

        /// <summary>
        /// String representation of <see cref="Lift"/> values.
        /// Does not include string representation of <see cref="Lift.None"/>.
        /// </summary>
        private static readonly string[] liftStrings;

        #endregion

        #region Constructors

        /// <summary>
        /// Static constructor.
        /// </summary>
        static LiftResolver()
        {
            Lift[] lifts = (Lift[])Enum.GetValues(typeof(Lift));
            List<string> liftStringList = new List<string>(lifts.Length);

            foreach (Lift lift in lifts)
                if (!(lift == Lift.None))
                    liftStringList.Add(LiftToString(lift));

            liftStrings = liftStringList.ToArray();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns string representation of <see cref="Lift"/> values.
        /// Does not include string representation of <see cref="Lift.None"/>.
        /// </summary>
        /// <returns>String representation of <see cref="Lift"/> values.
        /// Does not include string representation of <see cref="Lift.None"/>.</returns>
        public static string[] LiftStrings()
        {
            string[] liftStrings = new string[LiftResolver.liftStrings.Length];

            for (int i = 0; i < liftStrings.Length; i++)
                liftStrings[i] = LiftResolver.liftStrings[i];

            return liftStrings;
        }

        /// <summary>
        /// Outputs string representation of <see cref="Lift"/>.
        /// </summary>
        /// <param name="lift">Lift to output string representation of.</param>
        /// <returns>String representation of <paramref name="lift"/>.</returns>
        /// <exception cref="ArgumentException">Unsupported <paramref name="lift"/>.</exception>
        public static string LiftToString(Lift lift)
        {
            switch (lift)
            {
                case Lift.None:
                    return none;

                case Lift.Squat:
                    return squat;

                case Lift.BenchPress:
                    return benchPress;

                case Lift.Deadlift:
                    return deadlift;

                default:
                    throw new ArgumentException("Unsupported lift.", nameof(lift));
            }
        }

        /// <summary>
        /// Returns <see cref="Lift"/> of input <see cref="Lift"/> string representation.
        /// </summary>
        /// <param name="lift">String representation of <see cref="Lift"/>.</param>
        /// <returns><see cref="Lift"/> of <paramref name="lift"/>.</returns>
        /// <exception cref="ArgumentException">Unsupported <paramref name="lift"/>.</exception>
        public static Lift StringToLift(string lift)
        {
            switch (lift)
            {
                case none:
                    return Lift.None;

                case squat:
                    return Lift.Squat;

                case benchPress:
                    return Lift.BenchPress;

                case deadlift:
                    return Lift.Deadlift;

                default:
                    throw new ArgumentException("Unsupported lift.", nameof(lift));
            }
        }

        #endregion
    }
}
