using System;

namespace LiftingAtlas.Standard
{
    public class PlannedRepetitions : IEquatable<PlannedRepetitions>
    {
        #region Private fields

        private readonly Repetitions lowerBound;
        private readonly Repetitions upperBound;

        #endregion

        #region Constructors

        public PlannedRepetitions(Repetitions lowerBound, Repetitions upperBound)
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

        public Repetitions LowerBound
        {
            get
            {
                return lowerBound;
            }
        }

        public Repetitions UpperBound
        {
            get
            {
                return upperBound;
            }
        }

        #endregion

        #region Methods

        public bool InRange(Repetitions repetitions)
        {
            return repetitions >= this.LowerBound && repetitions <= this.UpperBound;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PlannedRepetitions))
                return false;

            return this.Equals((PlannedRepetitions)obj);
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

        public bool Equals(PlannedRepetitions other)
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

        public static bool operator ==(PlannedRepetitions first, PlannedRepetitions second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(PlannedRepetitions first, PlannedRepetitions second)
        {
            return !(first == second);
        }

        #endregion
    }
}
