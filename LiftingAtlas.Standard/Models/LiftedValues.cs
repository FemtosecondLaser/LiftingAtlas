using System;

namespace LiftingAtlas.Standard
{
    public class LiftedValues : IEquatable<LiftedValues>
    {
        #region Private fields

        private readonly Repetitions liftedRepetitions;
        private readonly Weight liftedWeight;

        #endregion

        #region Constructors

        public LiftedValues(Repetitions liftedRepetitions, Weight liftedWeight)
        {
            if (liftedRepetitions == null)
                throw new ArgumentNullException(nameof(liftedRepetitions));

            if (liftedWeight == null)
                throw new ArgumentNullException(nameof(liftedWeight));

            this.liftedRepetitions = liftedRepetitions;
            this.liftedWeight = liftedWeight;
        }

        #endregion

        #region Properties

        public Repetitions LiftedRepetitions
        {
            get
            {
                return liftedRepetitions;
            }
        }

        public Weight LiftedWeight
        {
            get
            {
                return liftedWeight;
            }
        }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if (!(obj is LiftedValues))
                return false;

            return this.Equals((LiftedValues)obj);
        }

        public override int GetHashCode()
        {
            return (this.LiftedRepetitions, this.LiftedWeight).GetHashCode();
        }

        public bool Equals(LiftedValues other)
        {
            if ((object)other == null)
                return false;

            return (this.LiftedRepetitions, this.LiftedWeight).Equals(
                (other.LiftedRepetitions, other.LiftedWeight)
                );
        }

        #endregion

        #region Operators

        public static bool operator ==(LiftedValues first, LiftedValues second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(LiftedValues first, LiftedValues second)
        {
            return !(first == second);
        }

        #endregion
    }
}
