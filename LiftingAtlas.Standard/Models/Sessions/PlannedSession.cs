using System;
using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    public class PlannedSession<T> : TemplateSession<T>, IEquatable<PlannedSession<T>> where T : PlannedSet
    {
        #region Constructors

        public PlannedSession(
            SessionNumber number,
            List<T> sets
            ) : base(
                number,
                sets
                )
        {

        }

        #endregion

        #region Properties

        public bool Done
        {
            get
            {
                foreach (T set in this.Sets)
                    if (!set.Done)
                        return false;

                return true;
            }
        }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if (!(obj is PlannedSession<T>))
                return false;

            return this.Equals((PlannedSession<T>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int HashCode = this.Done.GetHashCode();
                HashCode = (HashCode * 397) ^ base.GetHashCode();
                return HashCode;
            }
        }

        public bool Equals(PlannedSession<T> other)
        {
            if ((object)other == null)
                return false;

            return (
                (this.Done == other.Done)
                &&
                base.Equals(other)
                );
        }

        #endregion

        #region Operators

        public static bool operator ==(PlannedSession<T> first, PlannedSession<T> second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(PlannedSession<T> first, PlannedSession<T> second)
        {
            return !(first == second);
        }

        #endregion
    }
}
