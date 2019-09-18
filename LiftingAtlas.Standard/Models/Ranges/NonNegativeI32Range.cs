using System;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Class representing non-negative 32-bit integer range.
    /// </summary>
    public class NonNegativeI32Range
    {
        #region Private fields

        /// <summary>
        /// Field behind <see cref="LowerBound"/>.
        /// </summary>
        private readonly int lowerBound;

        /// <summary>
        /// Field behind <see cref="UpperBound"/>.
        /// </summary>
        private readonly int upperBound;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates non-negative 32-bit integer range.
        /// <paramref name="lowerBound"/> must not be less than 0.
        /// <paramref name="upperBound"/> must not be less than 0.
        /// <paramref name="upperBound"/> must not be less than <paramref name="lowerBound"/>.
        /// </summary>
        /// <param name="lowerBound">Lower bound of the range.</param>
        /// <param name="upperBound">Upper bound of the range.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="lowerBound"/>
        /// or <paramref name="upperBound"/> is less than 0.</exception>
        /// <exception cref="ArgumentException"><paramref name="upperBound"/> is less than
        /// <paramref name="lowerBound"/>.</exception>
        public NonNegativeI32Range(int lowerBound, int upperBound)
        {
            if (lowerBound < 0)
                throw new ArgumentOutOfRangeException(nameof(lowerBound));

            if (upperBound < 0)
                throw new ArgumentOutOfRangeException(nameof(upperBound));

            if (upperBound < lowerBound)
                throw new ArgumentException($"{nameof(upperBound)} is less than {nameof(lowerBound)}.");

            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Lower bound of the range.
        /// </summary>
        public int LowerBound
        {
            get
            {
                return lowerBound;
            }
        }

        /// <summary>
        /// Upper bound of the range.
        /// </summary>
        public int UpperBound
        {
            get
            {
                return upperBound;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines if <paramref name="value"/> falls within a range
        /// specified by <see cref="LowerBound"/> and <see cref="UpperBound"/>.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="includeLowerBound">Include <see cref="LowerBound"/>.</param>
        /// <param name="includeUpperBound">Include <see cref="UpperBound"/>.</param>
        /// <returns>True if <paramref name="value"/> falls within a range
        /// specified by <see cref="LowerBound"/> and <see cref="UpperBound"/>;
        /// otherwise, false.</returns>
        public bool InRange(int value, bool includeLowerBound, bool includeUpperBound)
        {
            return (
                (includeLowerBound ? value >= this.LowerBound : value > this.LowerBound)
                &&
                (includeUpperBound ? value <= this.UpperBound : value < this.UpperBound)
                );
        }

        /// <summary>
        /// Compares this instance of the class with an object.
        /// </summary>
        /// <param name="obj">An object to compare with.</param>
        /// <returns>Comparison result.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is NonNegativeI32Range))
                return false;

            return this.Equals((NonNegativeI32Range)obj);
        }

        /// <summary>
        /// Computes the hash code for this object.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int HashCode = this.LowerBound.GetHashCode();
                HashCode = (HashCode * 397) ^ this.UpperBound.GetHashCode();
                return HashCode;
            }
        }

        /// <summary>
        /// Compares this instance of the class with an instance of <see cref="NonNegativeI32Range"/>.
        /// </summary>
        /// <param name="other">An instance of <see cref="NonNegativeI32Range"/> to compare with.</param>
        /// <returns>Comparison result.</returns>
        public bool Equals(NonNegativeI32Range other)
        {
            if ((object)other == null)
                return false;

            return (
                (this.LowerBound == other.LowerBound)
                &&
                (this.UpperBound == other.UpperBound)
                );
        }

        /// <summary>
        /// Returns string representation of the range.
        /// </summary>
        /// <returns>String representation of the range.</returns>
        public override string ToString()
        {
            if (this.LowerBound == this.UpperBound)
                return this.LowerBound.ToString();

            return $"{this.LowerBound.ToString()}–{this.UpperBound.ToString()}";
        }

        /// <summary>
        /// Returns string representation of the range with a suffix appended.
        /// </summary>
        /// <param name="numberSuffix">Suffix to append to the number(s).</param>
        /// <returns>String representation of the range with a suffix appended.</returns>
        public string ToString(char numberSuffix)
        {
            if (this.LowerBound == this.UpperBound)
                return $"{this.LowerBound.ToString()}{numberSuffix}";

            return $"{this.LowerBound.ToString()}{numberSuffix}–{this.UpperBound.ToString()}{numberSuffix}";
        }

        #endregion

        #region Operators

        /// <summary>
        /// Determines equality of operands.
        /// </summary>
        /// <param name="first">First operand.</param>
        /// <param name="second">Second operand.</param>
        /// <returns>True if operands are equal;
        /// otherwise, false.</returns>
        public static bool operator ==(NonNegativeI32Range first, NonNegativeI32Range second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Equals(second);
        }

        /// <summary>
        /// Determines inequality of operands.
        /// </summary>
        /// <param name="first">First operand.</param>
        /// <param name="second">Second operand.</param>
        /// <returns>True if operands are unequal;
        /// otherwise, false.</returns>
        public static bool operator !=(NonNegativeI32Range first, NonNegativeI32Range second)
        {
            return !(first == second);
        }

        #endregion
    }
}
