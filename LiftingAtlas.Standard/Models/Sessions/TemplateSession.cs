using System;
using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Class containing list of sets.
    /// </summary>
    public class TemplateSession<T> : BaseSession<T>, IEquatable<TemplateSession<T>> where T : TemplateSet
    {
        #region Private fields

        /// <summary>
        /// A field behind <see cref="Number"/>.
        /// </summary>
        private readonly int number;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a session.
        /// </summary>
        /// <param name="number">Sequential number. Must not be less than 1.</param>
        /// <param name="sets">Collection of sets this session consists of. Must not be null. Must contain more than 0 sets.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="number"/> is less than 1.</exception>
        public TemplateSession(int number, List<T> sets) : base(sets)
        {
            if (number < 1)
                throw new ArgumentOutOfRangeException(nameof(number));

            this.number = number;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Sequential number.
        /// </summary>
        public int Number
        {
            get
            {
                return number;
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
            if (!(obj is TemplateSession<T>))
                return false;

            return this.Equals((TemplateSession<T>)obj);
        }

        /// <summary>
        /// Computes the hash code for this object.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int HashCode = this.Number.GetHashCode();
                HashCode = (HashCode * 397) ^ base.GetHashCode();
                return HashCode;
            }
        }

        /// <summary>
        /// Compares this instance of the class with an instance of <see cref="TemplateSession{T}"/>.
        /// </summary>
        /// <param name="other">An instance of <see cref="TemplateSession{T}"/> to compare with.</param>
        /// <returns>Comparison result.</returns>
        public bool Equals(TemplateSession<T> other)
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

        /// <summary>
        /// Determines equality of operands.
        /// </summary>
        /// <param name="first">First operand.</param>
        /// <param name="second">Second operand.</param>
        /// <returns>True if operands are equal;
        /// otherwise, false.</returns>
        public static bool operator ==(TemplateSession<T> first, TemplateSession<T> second)
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
        public static bool operator !=(TemplateSession<T> first, TemplateSession<T> second)
        {
            return !(first == second);
        }

        #endregion
    }
}
