using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Class containing list of sessions.
    /// </summary>
    public abstract class BaseCycle<T1, T2> : IEquatable<BaseCycle<T1, T2>> where T1 : BaseSession<T2> where T2 : BaseSet
    {
        #region Private fields

        /// <summary>
        /// A field behind <see cref="Sessions"/>.
        /// </summary>
        private readonly List<T1> sessions;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a cycle.
        /// </summary>
        /// <param name="sessions">Collection of sessions this cycle consists of. Must not be null. Must contain more than 0 sessions.</param>
        /// <exception cref="ArgumentNullException"><paramref name="sessions"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="sessions"/> does not contain more than 0 sessions.</exception>
        public BaseCycle(List<T1> sessions)
        {
            if (sessions == null)
                throw new ArgumentNullException(nameof(sessions));

            if (!(sessions.Count > 0))
                throw new ArgumentException($"{nameof(sessions)} must contain more than 0 sessions.");

            this.sessions = sessions;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Collection of sessions this cycle consists of.
        /// </summary>
        public ReadOnlyCollection<T1> Sessions
        {
            get
            {
                return sessions.AsReadOnly();
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
            if (!(obj is BaseCycle<T1, T2>))
                return false;

            return this.Equals((BaseCycle<T1, T2>)obj);
        }

        /// <summary>
        /// Computes the hash code for this object.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int HashCode = 0;

                if (this.Sessions != null)
                    foreach (T1 session in this.Sessions)
                        HashCode = (HashCode * 397) ^ (session != null ? session.GetHashCode() : 0);

                return HashCode;
            }
        }

        /// <summary>
        /// Compares this instance of the class with an instance of <see cref="BaseCycle{T1, T2}"/>.
        /// </summary>
        /// <param name="other">An instance of <see cref="BaseCycle{T1, T2}"/> to compare with.</param>
        /// <returns>Comparison result.</returns>
        public bool Equals(BaseCycle<T1, T2> other)
        {
            if ((object)other == null)
                return false;

            return this.Sessions.ListEquals(other.Sessions);
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
        public static bool operator ==(BaseCycle<T1, T2> first, BaseCycle<T1, T2> second)
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
        public static bool operator !=(BaseCycle<T1, T2> first, BaseCycle<T1, T2> second)
        {
            return !(first == second);
        }

        #endregion
    }
}
