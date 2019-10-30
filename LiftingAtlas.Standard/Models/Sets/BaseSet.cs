using System;

namespace LiftingAtlas.Standard
{
    public abstract class BaseSet : IEquatable<BaseSet>
    {
        #region Private fields

        private readonly PlannedPercentageOfReferencePoint plannedPercentageOfReferencePoint;
        private readonly PlannedRepetitions plannedRepetitions;
        private readonly WeightAdjustmentConstant weightAdjustmentConstant;
        private readonly string note;

        #endregion

        #region Constructors

        public BaseSet(
            PlannedPercentageOfReferencePoint plannedPercentageOfReferencePoint,
            PlannedRepetitions plannedRepetitions,
            WeightAdjustmentConstant weightAdjustmentConstant = null,
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

        public PlannedPercentageOfReferencePoint PlannedPercentageOfReferencePoint
        {
            get
            {
                return plannedPercentageOfReferencePoint;
            }
        }

        public PlannedRepetitions PlannedRepetitions
        {
            get
            {
                return plannedRepetitions;
            }
        }

        public WeightAdjustmentConstant WeightAdjustmentConstant
        {
            get
            {
                return weightAdjustmentConstant;
            }
        }

        public string Note
        {
            get
            {
                return note;
            }
        }

        #endregion

        #region Methods

        public bool RepetitionsWithinPlannedRange(Repetitions repetitions)
        {
            if (repetitions == null)
                throw new ArgumentNullException(nameof(repetitions));

            return this.PlannedRepetitions.InRange(repetitions);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BaseSet))
                return false;

            return this.Equals((BaseSet)obj);
        }

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

        public static bool operator ==(BaseSet first, BaseSet second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(BaseSet first, BaseSet second)
        {
            return !(first == second);
        }

        #endregion
    }
}
