using System;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Minimal planning unit of cycle.
    /// </summary>
    public abstract class BaseSet : IEquatable<BaseSet>
    {
        #region Private fields

        /// <summary>
        /// A field behind <see cref="PlannedPercentageOfReferencePoint"/>.
        /// </summary>
        private readonly NonNegativeI32Range plannedPercentageOfReferencePoint;

        /// <summary>
        /// A field behind <see cref="PlannedRepetitions"/>.
        /// </summary>
        private readonly NonNegativeI32Range plannedRepetitions;

        /// <summary>
        /// A field behind <see cref="WeightAdjustmentConstant"/>
        /// </summary>
        private readonly double? weightAdjustmentConstant;

        /// <summary>
        /// A field behind <see cref="Note"/>.
        /// </summary>
        private readonly string note;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a set.
        /// </summary>
        /// <param name="plannedPercentageOfReferencePoint">Planned percentage of reference point.</param>
        /// <param name="plannedRepetitions">Amount of repetitions. Must not be null.</param>
        /// <param name="weightAdjustmentConstant">Constant to add to weight range in order to derive planned weight.</param>
        /// <param name="note">A note.</param>
        /// <exception cref="ArgumentNullException"><paramref name="plannedRepetitions"/> is null.</exception>
        public BaseSet(
            NonNegativeI32Range plannedPercentageOfReferencePoint,
            NonNegativeI32Range plannedRepetitions,
            double? weightAdjustmentConstant = null,
            string note = null
            )
        {
            if (plannedRepetitions == null)
                throw new ArgumentNullException(nameof(plannedRepetitions));

            this.plannedPercentageOfReferencePoint = plannedPercentageOfReferencePoint;
            this.plannedRepetitions = plannedRepetitions;
            this.weightAdjustmentConstant = weightAdjustmentConstant;
            this.note = note;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Planned percentage of reference point.
        /// </summary>
        public NonNegativeI32Range PlannedPercentageOfReferencePoint
        {
            get
            {
                return plannedPercentageOfReferencePoint;
            }
        }

        /// <summary>
        /// Planned amount of times to lift weight during this set.
        /// </summary>
        public NonNegativeI32Range PlannedRepetitions
        {
            get
            {
                return plannedRepetitions;
            }
        }

        /// <summary>
        /// Constant to add to weight range in order to derive planned weight.
        /// </summary>
        public double? WeightAdjustmentConstant
        {
            get
            {
                return weightAdjustmentConstant;
            }
        }

        /// <summary>
        /// A note.
        /// </summary>
        public string Note
        {
            get
            {
                return note;
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
            if (!(obj is BaseSet))
                return false;

            return this.Equals((BaseSet)obj);
        }

        /// <summary>
        /// Computes the hash code for this object.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int HashCode = this.PlannedPercentageOfReferencePoint != null ? this.PlannedPercentageOfReferencePoint.GetHashCode() : 0;
                HashCode = (HashCode * 397) ^ (this.PlannedRepetitions != null ? this.PlannedRepetitions.GetHashCode() : 0);
                HashCode = (HashCode * 397) ^ (this.WeightAdjustmentConstant != null ? this.WeightAdjustmentConstant.GetHashCode() : 0);
                HashCode = (HashCode * 397) ^ (this.Note != null ? this.Note.GetHashCode() : 0);
                return HashCode;
            }
        }

        /// <summary>
        /// Compares this instance of the class with an instance of <see cref="BaseSet"/>.
        /// </summary>
        /// <param name="other">An instance of <see cref="BaseSet"/> to compare with.</param>
        /// <returns>Comparison result.</returns>
        public bool Equals(BaseSet other)
        {
            if ((object)other == null)
                return false;

            return (
                (this.PlannedPercentageOfReferencePoint == other.PlannedPercentageOfReferencePoint)
                &&
                (this.PlannedRepetitions == other.PlannedRepetitions)
                &&
                (this.WeightAdjustmentConstant == other.WeightAdjustmentConstant)
                &&
                (this.Note == other.Note)
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
        public static bool operator ==(BaseSet first, BaseSet second)
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
        public static bool operator !=(BaseSet first, BaseSet second)
        {
            return !(first == second);
        }

        #endregion
    }
}
