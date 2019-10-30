using System;

namespace LiftingAtlas.Standard
{
    [FlagsAttribute]
    public enum Lift : byte
    {
        None = 0,

        Squat = 1,

        BenchPress = 2,

        Deadlift = 4
    }
}
