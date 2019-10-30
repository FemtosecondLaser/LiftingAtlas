using System;

namespace LiftingAtlas.Standard
{
    public class Weight : IEquatable<Weight>
    {
        #region Private fields

        private readonly double value;

        #endregion

        #region Constructors

        public Weight(double value)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Value is not finite.", nameof(value));

            if (value < 0.00)
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

        #region Methods

        public override bool Equals(object obj)
        {
            if (!(obj is Weight))
                return false;

            return this.Equals((Weight)obj);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public bool Equals(Weight other)
        {
            if ((object)other == null)
                return false;

            return this.Value == other.Value;
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        #endregion

        #region Operators

        public static implicit operator double(Weight weight)
        {
            return weight.Value;
        }

        public static bool operator ==(Weight first, Weight second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(Weight first, Weight second)
        {
            return !(first == second);
        }

        public static bool operator <(Weight first, Weight second)
        {
            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Value < second.Value;
        }

        public static bool operator >(Weight first, Weight second)
        {
            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Value > second.Value;
        }

        public static bool operator <=(Weight first, Weight second)
        {
            return first < second || first == second;
        }

        public static bool operator >=(Weight first, Weight second)
        {
            return first > second || first == second;
        }

        #endregion
    }
}
