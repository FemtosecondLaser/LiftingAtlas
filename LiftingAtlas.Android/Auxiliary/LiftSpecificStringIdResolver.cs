using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LiftingAtlas.Standard;

namespace LiftingAtlas.Droid
{
    public static class LiftSpecificStringIdResolver
    {
        public static int CurrentLiftCycleStringId(Lift lift)
        {
            if (lift == Lift.None)
                throw new ArgumentException($"Unspecified {nameof(lift)}.");

            switch (lift)
            {
                case Lift.Squat:
                    return Resource.String.current_squat_cycle;

                case Lift.BenchPress:
                    return Resource.String.current_bench_press_cycle;

                case Lift.Deadlift:
                    return Resource.String.current_deadlift_cycle;

                default:
                    throw new ArgumentException($"Unsupported lift: {lift}.");
            }
        }

        public static int NewLiftCycleStringId(Lift lift)
        {
            if (lift == Lift.None)
                throw new ArgumentException($"Unspecified {nameof(lift)}.");

            switch (lift)
            {
                case Lift.Squat:
                    return Resource.String.new_squat_cycle;

                case Lift.BenchPress:
                    return Resource.String.new_bench_press_cycle;

                case Lift.Deadlift:
                    return Resource.String.new_deadlift_cycle;

                default:
                    throw new ArgumentException($"Unsupported lift: {lift}.");
            }
        }

        public static int PlannedLiftSessionStringId(Lift lift)
        {
            if (lift == Lift.None)
                throw new ArgumentException($"Unspecified {nameof(lift)}.");

            switch (lift)
            {
                case Lift.Squat:
                    return Resource.String.planned_squat_session;

                case Lift.BenchPress:
                    return Resource.String.planned_bench_press_session;

                case Lift.Deadlift:
                    return Resource.String.planned_deadlift_session;

                default:
                    throw new ArgumentException($"Unsupported lift: {lift}.");
            }
        }

        public static int PlannedLiftSetStringId(Lift lift)
        {
            if (lift == Lift.None)
                throw new ArgumentException($"Unspecified {nameof(lift)}.");

            switch (lift)
            {
                case Lift.Squat:
                    return Resource.String.planned_squat_set;

                case Lift.BenchPress:
                    return Resource.String.planned_bench_press_set;

                case Lift.Deadlift:
                    return Resource.String.planned_deadlift_set;

                default:
                    throw new ArgumentException($"Unsupported lift: {lift}.");
            }
        }

        public static int LiftStringId(Lift lift)
        {
            if (lift == Lift.None)
                throw new ArgumentException($"Unspecified {nameof(lift)}.");

            switch (lift)
            {
                case Lift.Squat:
                    return Resource.String.squat;

                case Lift.BenchPress:
                    return Resource.String.bench_press;

                case Lift.Deadlift:
                    return Resource.String.deadlift;

                default:
                    throw new ArgumentException($"Unsupported lift: {lift}.");
            }
        }
    }
}