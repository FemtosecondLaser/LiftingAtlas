using System;

namespace LiftingAtlas.Standard
{
    public class NearestMultipleProvider : IQuantizationProvider
    {
        #region Private fields

        private readonly UniformQuantizationInterval uniformQuantizationInterval;

        #endregion

        #region Constructors

        public NearestMultipleProvider(UniformQuantizationInterval uniformQuantizationInterval)
        {
            if (uniformQuantizationInterval == null)
                throw new ArgumentNullException(nameof(uniformQuantizationInterval));

            this.uniformQuantizationInterval = uniformQuantizationInterval;
        }

        #endregion

        #region Methods

        public double Quantize(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Not a finite number.", nameof(value));

            double q = uniformQuantizationInterval.Value;

            return Math.Round((value / q), MidpointRounding.AwayFromZero) * q;
        }

        #endregion
    }
}
