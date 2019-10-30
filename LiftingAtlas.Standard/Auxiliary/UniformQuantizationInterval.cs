using System;

namespace LiftingAtlas.Standard
{
    public class UniformQuantizationInterval
    {
        #region Private fields

        private readonly double value;

        #endregion

        #region Constructors

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
