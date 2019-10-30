using System;

namespace LiftingAtlas.Standard
{
    public class TemplateSet : BaseSet, IEquatable<TemplateSet>
    {
        #region Private fields

        private readonly SetNumber number;

        #endregion

        #region Constructors

        public TemplateSet(
            SetNumber number,
            PlannedPercentageOfReferencePoint plannedPercentageOfReferencePoint,
            PlannedRepetitions plannedRepetitions,
            WeightAdjustmentConstant weightAdjustmentConstant = null,
            string note = null
            ) : base(
                plannedPercentageOfReferencePoint,
                plannedRepetitions,
                weightAdjustmentConstant,
                note
                )
        {
            if (number == null)
                throw new ArgumentNullException(nameof(number));

            this.number = number;
        }

        #endregion

        #region Properties

        public SetNumber Number
        {
            get
            {
                return number;
            }
        }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if (!(obj is TemplateSet))
                return false;

            return this.Equals((TemplateSet)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int HashCode = this.Number != null ? this.Number.GetHashCode() : 0;
                HashCode = (HashCode * 397) ^ base.GetHashCode();
                return HashCode;
            }
        }

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

        public static bool operator ==(TemplateSet first, TemplateSet second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(TemplateSet first, TemplateSet second)
        {
            return !(first == second);
        }

        #endregion
    }
}
