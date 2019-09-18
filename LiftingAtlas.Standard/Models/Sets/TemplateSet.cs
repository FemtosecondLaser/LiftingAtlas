using System;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Minimal planning unit of cycle.
    /// </summary>
    public class TemplateSet : BaseSet, IEquatable<TemplateSet>
    {
        #region Private fields

        /// <summary>
        /// A field behind <see cref="Number"/>.
        /// </summary>
        private readonly int number;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a set.
        /// </summary>
        /// <param name="number">Sequential number. Must not be less than 1.</param>
        /// <param name="plannedPercentageOfReferencePoint">Planned percentage of reference point.</param>
        /// <param name="plannedRepetitions">Amount of repetitions.</param>
        /// <param name="weightAdjustmentConstant">Constant to add to weight range in order to derive planned weight.</param>
        /// <param name="note">A note.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="number"/> is less than 1.</exception>
        public TemplateSet(
            int number,
            NonNegativeI32Range plannedPercentageOfReferencePoint,
            NonNegativeI32Range plannedRepetitions,
            double? weightAdjustmentConstant = null,
            string note = null
            ) : base(
                plannedPercentageOfReferencePoint,
                plannedRepetitions,
                weightAdjustmentConstant,
                note
                )
        {
            if (number < 1)
                throw new ArgumentOutOfRangeException(nameof(number));

            this.number = number;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Sequential number.
        /// </summary>
        public int Number
        {
            get
            {
                return number;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Compares this instance of the class with an object.
        /// </summary>
        /// <param name="obj">An object to compare with.</param>
        /// <returns>Comparison result.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is TemplateSet))
                return false;

            return this.Equals((TemplateSet)obj);
        }

        /// <summary>
        /// Computes the hash code for this object.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int HashCode = this.Number.GetHashCode();
                HashCode = (HashCode * 397) ^ base.GetHashCode();
                return HashCode;
            }
        }

        /// <summary>
        /// Compares this instance of the class with an instance of <see cref="TemplateSet"/>.
        /// </summary>
        /// <param name="other">An instance of <see cref="TemplateSet"/> to compare with.</param>
        /// <returns>Comparison result.</returns>
        public bool Equals(TemplateSet other)
        {
            if ((object)other == null)
                return false;

            return (
                (this.Number == other.Number)
                &&
                base.Equals(other)
                );
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
        public static bool operator ==(TemplateSet first, TemplateSet second)
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
        public static bool operator !=(TemplateSet first, TemplateSet second)
        {
            return !(first == second);
        }

        #endregion
    }
}
