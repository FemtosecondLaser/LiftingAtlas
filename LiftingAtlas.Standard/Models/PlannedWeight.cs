using System;

namespace LiftingAtlas.Standard
{
    public class PlannedWeight : IEquatable<PlannedWeight>
    {
        #region Private fields

        private readonly Weight lowerBound;
        private readonly Weight upperBound;

        #endregion

        #region Constructors

        public PlannedWeight(Weight lowerBound, Weight upperBound)
        {
            if (lowerBound == null)
                throw new ArgumentNullException(nameof(lowerBound));

            if (upperBound == null)
                throw new ArgumentNullException(nameof(upperBound));

            if (upperBound < lowerBound)
                throw new ArgumentException($"Upper bound is less than lower bound.", nameof(upperBound));

            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
        }

        #endregion

        #region Properties

        public Weight LowerBound
        {
            get
            {
                return lowerBound;
            }
        }

        public Weight UpperBound
        {
            get
            {
                return upperBound;
            }
        }

        #endregion

        #region Methods

        public bool InRange(Weight weight)
        {
            return weight >= this.LowerBound && weight <= this.UpperBound;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PlannedWeight))
                return false;

            return this.Equals((PlannedWeight)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int HashCode = this.LowerBound.GetHashCode();
                HashCode = (HashCode * 397) ^ this.UpperBound.GetHashCode();
                return HashCode;
            }
        }

        public bool Equals(PlannedWeight other)
        {
            if ((object)other == null)
                return false;

            return (
                (this.LowerBound == other.LowerBound)
                &&
                (this.UpperBound == other.UpperBound)
                );
        }

        public override string ToString()
        {
            if (this.LowerBound == this.UpperBound)
                return this.LowerBound.ToString();

            return $"{this.LowerBound.ToString()}–{this.UpperBound.ToString()}";
        }

        public string ToString(char numberSuffix)
        {
            if (this.LowerBound == this.UpperBound)
                return $"{this.LowerBound.ToString()}{numberSuffix}";

            return $"{this.LowerBound.ToString()}{numberSuffix}–{this.UpperBound.ToString()}{numberSuffix}";
        }

        #endregion

        #region Operators

        public static bool operator ==(PlannedWeight first, PlannedWeight second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(PlannedWeight first, PlannedWeight second)
        {
            return !(first == second);
        }

        #endregion
    }
}
