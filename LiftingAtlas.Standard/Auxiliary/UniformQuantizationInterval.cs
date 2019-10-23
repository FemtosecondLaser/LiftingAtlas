using System;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Uniform quantization interval.
    /// </summary>
    public class UniformQuantizationInterval
    {
        #region Private fields

        /// <summary>
        /// A field behind <see cref="Value"/>.
        /// </summary>
        private readonly double value;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates uniform quantization interval.
        /// </summary>
        /// <param name="value">Uniform quantization interval value. Must be a finite number. Must be greater than 0.</param>
        /// <exception cref="ArgumentException"><paramref name="value"/> is not a finite number.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is not greater than 0.</exception>
        public UniformQuantizationInterval(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Not a finite number.", nameof(value));

            if (!(value > 0.00))
                throw new ArgumentOutOfRangeException(nameof(value));

            this.value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Value.
        /// </summary>
        public double Value
        {
            get
            {
                return value;
            }
        }

        #endregion
    }
}
