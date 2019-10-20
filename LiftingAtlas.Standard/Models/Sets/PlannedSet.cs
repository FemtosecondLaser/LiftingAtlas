using System;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Minimal planning unit of cycle.
    /// </summary>
    public class PlannedSet : TemplateSet, IEquatable<PlannedSet>
    {
        #region Private fields

        /// <summary>
        /// A field behind <see cref="PlannedWeight"/>.
        /// </summary>
        private readonly NonNegativeDBLRange plannedWeight;

        /// <summary>
        /// A field behind <see cref="LiftedValues"/>.
        /// </summary>
        private (int liftedRepetitions, double liftedWeight)? liftedValues;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a set.
        /// </summary>
        /// <param name="number">Sequential number. Must not be less than 1.</param>
        /// <param name="plannedPercentageOfReferencePoint">Planned percentage of reference point.</param>
        /// <param name="plannedRepetitions">Amount of repetitions.</param>
        /// <param name="plannedWeight">Planned weight.</param>
        /// <param name="liftedValues">Lifted values. If not null - liftedRepetitions, liftedWeight must not be less than 0.</param>
        /// <param name="weightAdjustmentConstant">Constant to add to weight range in order to derive planned weight.</param>
        /// <param name="note">A note.</param>
        /// <exception cref="ArgumentOutOfRangeException">At least one of the following is not null and less than 0:
        /// <paramref name="plannedWeight"/>, <paramref name="liftedValues"/>.liftedRepetitions,
        /// <paramref name="liftedValues"/>.liftedWeight.</exception>
        public PlannedSet(
            int number,
            NonNegativeI32Range plannedPercentageOfReferencePoint,
            NonNegativeI32Range plannedRepetitions,
            NonNegativeDBLRange plannedWeight,
            (int liftedRepetitions, double liftedWeight)? liftedValues,
            double? weightAdjustmentConstant = null,
            string note = null
            ) : base(
                number,
                plannedPercentageOfReferencePoint,
                plannedRepetitions,
                weightAdjustmentConstant,
                note
                )
        {
            if (liftedValues != null && (liftedValues.Value.liftedRepetitions < 0 || liftedValues.Value.liftedWeight < 0.00))
                throw new ArgumentOutOfRangeException(nameof(liftedValues));

            this.plannedWeight = plannedWeight;
            this.liftedValues = liftedValues;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Planned weight.
        /// </summary>
        public NonNegativeDBLRange PlannedWeight
        {
            get
            {
                return plannedWeight;
            }
        }

        /// <summary>
        /// Lifted values.
        /// </summary>
        public (int liftedRepetitions, double liftedWeight)? LiftedValues
        {
            get
            {
                return liftedValues;
            }
        }

        /// <summary>
        /// Indicates if this set is done.
        /// </summary>
        public bool Done
        {
            get
            {
                return LiftedValues != null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines if <paramref name="weight"/> falls within planned range.
        /// </summary>
        /// <param name="weight">Weight.</param>
        /// <returns>True if <paramref name="weight"/> falls within planned range
        /// or if planned range does not exist;
        /// otherwise, false.</returns>
        public bool WeightWithinPlannedRange(double weight)
        {
            if (this.PlannedWeight == null)
                return true;

            return this.PlannedWeight.InRange(weight, true, true);
        }

        /// <summary>
        /// Compares this instance of the class with an object.
        /// </summary>
        /// <param name="obj">An object to compare with.</param>
        /// <returns>Comparison result.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is PlannedSet))
                return false;

            return this.Equals((PlannedSet)obj);
        }

        /// <summary>
        /// Computes the hash code for this object.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int HashCode = this.PlannedWeight != null ? this.PlannedWeight.GetHashCode() : 0;
                HashCode = (HashCode * 397) ^ (this.LiftedValues != null ? this.LiftedValues.GetHashCode() : 0);
                HashCode = (HashCode * 397) ^ this.Done.GetHashCode();
                HashCode = (HashCode * 397) ^ base.GetHashCode();
                return HashCode;
            }
        }

        /// <summary>
        /// Compares this instance of the class with an instance of <see cref="PlannedSet"/>.
        /// </summary>
        /// <param name="other">An instance of <see cref="PlannedSet"/> to compare with.</param>
        /// <returns>Comparison result.</returns>
        public bool Equals(PlannedSet other)
        {
            if ((object)other == null)
                return false;

            return (
                (this.PlannedWeight == other.PlannedWeight)
                &&
                (this.LiftedValues == other.LiftedValues)
                &&
                (this.Done == other.Done)
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
        public static bool operator ==(PlannedSet first, PlannedSet second)
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
        public static bool operator !=(PlannedSet first, PlannedSet second)
        {
            return !(first == second);
        }

        #endregion
    }
}
