using System;
using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Class containing list of sets.
    /// </summary>
    public class PlannedSession<T> : TemplateSession<T>, IEquatable<PlannedSession<T>> where T : PlannedSet
    {
        #region Constructors

        /// <summary>
        /// Creates a session.
        /// </summary>
        /// <param name="number">Sequential number. Must not be less than 1.</param>
        /// <param name="sets">Collection of sets this session consists of. Must not be null. Must contain more than 0 sets.</param>
        public PlannedSession(
            int number,
            List<T> sets
            ) : base(
                number,
                sets
                )
        {

        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates if this session is done.
        /// </summary>
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

        /// <summary>
        /// Compares this instance of the class with an object.
        /// </summary>
        /// <param name="obj">An object to compare with.</param>
        /// <returns>Comparison result.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is PlannedSession<T>))
                return false;

            return this.Equals((PlannedSession<T>)obj);
        }

        /// <summary>
        /// Computes the hash code for this object.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int HashCode = this.Done.GetHashCode();
                HashCode = (HashCode * 397) ^ base.GetHashCode();
                return HashCode;
            }
        }

        /// <summary>
        /// Compares this instance of the class with an instance of <see cref="PlannedSession{T}"/>.
        /// </summary>
        /// <param name="other">An instance of <see cref="PlannedSession{T}"/> to compare with.</param>
        /// <returns>Comparison result.</returns>
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

        /// <summary>
        /// Determines equality of operands.
        /// </summary>
        /// <param name="first">First operand.</param>
        /// <param name="second">Second operand.</param>
        /// <returns>True if operands are equal;
        /// otherwise, false.</returns>
        public static bool operator ==(PlannedSession<T> first, PlannedSession<T> second)
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
        public static bool operator !=(PlannedSession<T> first, PlannedSession<T> second)
        {
            return !(first == second);
        }

        #endregion
    }
}
