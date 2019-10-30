using System;
using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    public class TemplateSession<T> : BaseSession<T>, IEquatable<TemplateSession<T>> where T : TemplateSet
    {
        #region Private fields

        private readonly SessionNumber number;

        #endregion

        #region Constructors

        public TemplateSession(SessionNumber number, List<T> sets) : base(sets)
        {
            if (number == null)
                throw new ArgumentNullException(nameof(number));

            this.number = number;
        }

        #endregion

        #region Properties

        public SessionNumber Number
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
            if (!(obj is TemplateSession<T>))
                return false;

            return this.Equals((TemplateSession<T>)obj);
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

        public static bool operator ==(TemplateSession<T> first, TemplateSession<T> second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(TemplateSession<T> first, TemplateSession<T> second)
        {
            return !(first == second);
        }

        #endregion
    }
}
