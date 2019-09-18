using System;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Provides quantization method.
    /// </summary>
    public class NearestTwoPointFiveMultipleProvider : IQuantizationProvider
    {
        /// <summary>
        /// Rounds <paramref name="input"/> half away from zero to nearest multiple of 2.5.
        /// </summary>
        /// <param name="input">Input to round to nearest multiple of 2.5.</param>
        /// <returns><paramref name="input"/> rounded to nearest multiple of 2.5.</returns>
        public double Quantize(double input)
        {
            return Math.Round((input / 2.5), MidpointRounding.AwayFromZero) * 2.5;
        }
    }
}
