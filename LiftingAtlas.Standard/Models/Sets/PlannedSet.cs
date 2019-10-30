using System;

namespace LiftingAtlas.Standard
{
    public class PlannedSet : TemplateSet, IEquatable<PlannedSet>
    {
        #region Private fields

        private readonly PlannedWeight plannedWeight;
        private LiftedValues liftedValues;

        #endregion

        #region Constructors

        public PlannedSet(
            SetNumber number,
            PlannedPercentageOfReferencePoint plannedPercentageOfReferencePoint,
            PlannedRepetitions plannedRepetitions,
            PlannedWeight plannedWeight,
            LiftedValues liftedValues,
            WeightAdjustmentConstant weightAdjustmentConstant = null,
            string note = null
            ) : base(
                number,
                plannedPercentageOfReferencePoint,
                plannedRepetitions,
                weightAdjustmentConstant,
                note
                )
        {
            this.plannedWeight = plannedWeight;
            this.liftedValues = liftedValues;
        }

        #endregion

        #region Properties

        public PlannedWeight PlannedWeight
        {
            get
            {
                return plannedWeight;
            }
        }

        public LiftedValues LiftedValues
        {
            get
            {
                return liftedValues;
            }
        }

        public bool Done
        {
            get
            {
                return LiftedValues != null;
            }
        }

        #endregion

        #region Methods

        public bool WeightWithinPlannedRange(Weight weight)
        {
            if (weight == null)
                throw new ArgumentNullException(nameof(weight));

            if (this.PlannedWeight == null)
                return true;

            return this.PlannedWeight.InRange(weight);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PlannedSet))
                return false;

            return this.Equals((PlannedSet)obj);
        }

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

        public static bool operator ==(PlannedSet first, PlannedSet second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(PlannedSet first, PlannedSet second)
        {
            return !(first == second);
        }

        #endregion
    }
}
