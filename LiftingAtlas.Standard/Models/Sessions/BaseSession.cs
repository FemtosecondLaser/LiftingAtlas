using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Class containing list of sets.
    /// </summary>
    public abstract class BaseSession<T> : IEquatable<BaseSession<T>> where T : BaseSet
    {
        #region Private fields

        /// <summary>
        /// A field behind <see cref="Sets"/>.
        /// </summary>
        private readonly List<T> sets;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a session.
        /// </summary>
        /// <param name="sets">Collection of sets this session consists of. Must not be null. Must contain more than 0 sets.</param>
        /// <exception cref="ArgumentNullException"><paramref name="sets"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="sets"/> does not contain more than 0 sets.</exception>
        public BaseSession(List<T> sets)
        {
            if (sets == null)
                throw new ArgumentNullException(nameof(sets));

            if (!(sets.Count > 0))
                throw new ArgumentException($"{nameof(sets)} must contain more than 0 sets.");

            this.sets = sets;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Collection of sets this session consists of.
        /// </summary>
        public ReadOnlyCollection<T> Sets
        {
            get
            {
                return sets.AsReadOnly();
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
            if (!(obj is BaseSession<T>))
                return false;

            return this.Equals((BaseSession<T>)obj);
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

                if (this.Sets != null)
                    foreach (T set in this.Sets)
                        HashCode = (HashCode * 397) ^ (set != null ? set.GetHashCode() : 0);

                return HashCode;
            }
        }

        /// <summary>
        /// Compares this instance of the class with an instance of <see cref="BaseSession{T}"/>.
        /// </summary>
        /// <param name="other">An instance of <see cref="BaseSession{T}"/> to compare with.</param>
        /// <returns>Comparison result.</returns>
        public bool Equals(BaseSession<T> other)
        {
            if ((object)other == null)
                return false;

            return this.Sets.ListEquals(other.Sets);
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
        public static bool operator ==(BaseSession<T> first, BaseSession<T> second)
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
        public static bool operator !=(BaseSession<T> first, BaseSession<T> second)
        {
            return !(first == second);
        }

        #endregion
    }
}
