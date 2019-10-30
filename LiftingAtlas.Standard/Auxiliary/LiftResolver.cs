using System;
using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    public static class LiftResolver
    {
        #region Private fields

        private const string none = "None";
        private const string squat = "Squat";
        private const string benchPress = "Bench Press";
        private const string deadlift = "Deadlift";
        private static readonly string[] liftStrings;

        #endregion

        #region Constructors

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

        public static string[] LiftStrings()
        {
            string[] liftStrings = new string[LiftResolver.liftStrings.Length];

            for (int i = 0; i < liftStrings.Length; i++)
                liftStrings[i] = LiftResolver.liftStrings[i];

            return liftStrings;
        }

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
