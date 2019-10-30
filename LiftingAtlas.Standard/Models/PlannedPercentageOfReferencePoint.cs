using System;

namespace LiftingAtlas.Standard
{
    public class PlannedPercentageOfReferencePoint : IEquatable<PlannedPercentageOfReferencePoint>
    {
        #region Private fields

        private readonly int lowerBound;
        private readonly int upperBound;

        #endregion

        #region Constructors

        public PlannedPercentageOfReferencePoint(int lowerBound, int upperBound)
        {
            if (lowerBound < 0)
                throw new ArgumentOutOfRangeException(nameof(lowerBound));

            if (upperBound < 0)
                throw new ArgumentOutOfRangeException(nameof(upperBound));

            if (upperBound < lowerBound)
                throw new ArgumentException($"Upper bound is less than lower bound.", nameof(upperBound));

            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
        }

        #endregion

        #region Properties

        public int LowerBound
        {
            get
            {
                return lowerBound;
            }
        }

        public int UpperBound
        {
            get
            {
                return upperBound;
            }
        }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if (!(obj is PlannedPercentageOfReferencePoint))
                return false;

            return this.Equals((PlannedPercentageOfReferencePoint)obj);
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

        public bool Equals(PlannedPercentageOfReferencePoint other)
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

        public static bool operator ==(
            PlannedPercentageOfReferencePoint first,
            PlannedPercentageOfReferencePoint second
            )
        {
            if (ReferenceEquals(first, second))
                return true;

            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(
            PlannedPercentageOfReferencePoint first,
            PlannedPercentageOfReferencePoint second
            )
        {
            return !(first == second);
        }

        #endregion
    }
}
