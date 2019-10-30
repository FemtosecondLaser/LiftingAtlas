using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LiftingAtlas.Standard
{
    public abstract class BaseSession<T> : IEquatable<BaseSession<T>> where T : BaseSet
    {
        #region Private fields

        private readonly List<T> sets;

        #endregion

        #region Constructors

        public BaseSession(List<T> sets)
        {
            if (sets == null)
                throw new ArgumentNullException(nameof(sets));

            if (!(sets.Count > 0))
                throw new ArgumentException("Sets must contain more than 0 sets.", nameof(sets));

            this.sets = sets;
        }

        #endregion

        #region Properties

        public ReadOnlyCollection<T> Sets
        {
            get
            {
                return sets.AsReadOnly();
            }
        }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if (!(obj is BaseSession<T>))
                return false;

            return this.Equals((BaseSession<T>)obj);
        }

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

        public bool Equals(BaseSession<T> other)
        {
            if ((object)other == null)
                return false;

            return this.Sets.ListEquals(other.Sets);
        }

        #endregion

        #region Operators

        public static bool operator ==(BaseSession<T> first, BaseSession<T> second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(BaseSession<T> first, BaseSession<T> second)
        {
            return !(first == second);
        }

        #endregion
    }
}
