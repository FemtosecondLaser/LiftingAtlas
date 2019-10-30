using System;

namespace LiftingAtlas.Standard
{
    public class WeightAdjustmentConstant : IEquatable<WeightAdjustmentConstant>
    {
        #region Private fields

        private readonly double value;

        #endregion

        #region Constructors

        public WeightAdjustmentConstant(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Value is not finite.", nameof(value));

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

        #region Methods

        public override bool Equals(object obj)
        {
            if (!(obj is WeightAdjustmentConstant))
                return false;

            return this.Equals((WeightAdjustmentConstant)obj);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public bool Equals(WeightAdjustmentConstant other)
        {
            if ((object)other == null)
                return false;

            return this.Value == other.Value;
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        public string ToString(string format)
        {
            return this.Value.ToString(format);
        }

        #endregion

        #region Operators

        public static implicit operator double(WeightAdjustmentConstant weightAdjustmentConstant)
        {
            return weightAdjustmentConstant.Value;
        }

        public static bool operator ==(WeightAdjustmentConstant first, WeightAdjustmentConstant second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(WeightAdjustmentConstant first, WeightAdjustmentConstant second)
        {
            return !(first == second);
        }

        #endregion
    }
}
