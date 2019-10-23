using System;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Provides nearest multiple.
    /// </summary>
    public class NearestMultipleProvider : IQuantizationProvider
    {
        #region Private fields

        /// <summary>
        /// Uniform quantization interval.
        /// </summary>
        private readonly UniformQuantizationInterval uniformQuantizationInterval;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates nearest multiple provider.
        /// </summary>
        /// <param name="uniformQuantizationInterval">Uniform quantization interval. Must not be null.</param>
        /// <exception cref="ArgumentNullException"><paramref name="uniformQuantizationInterval"/> is null.</exception>
        public NearestMultipleProvider(UniformQuantizationInterval uniformQuantizationInterval)
        {
            if (uniformQuantizationInterval == null)
                throw new ArgumentNullException(nameof(uniformQuantizationInterval));

            this.uniformQuantizationInterval = uniformQuantizationInterval;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Quantizes <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value to quantize. Must be a finite number.</param>
        /// <returns>Quantized <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/> is not a finite number.</exception>
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
