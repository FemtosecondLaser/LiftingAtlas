using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LiftingAtlas.Standard
{
    public abstract class BaseCycle<T1, T2> : IEquatable<BaseCycle<T1, T2>> where T1 : BaseSession<T2> where T2 : BaseSet
    {
        #region Private fields

        private readonly List<T1> sessions;

        #endregion

        #region Constructors

        public BaseCycle(List<T1> sessions)
        {
            if (sessions == null)
                throw new ArgumentNullException(nameof(sessions));

            if (!(sessions.Count > 0))
                throw new ArgumentException("Sessions must contain more than 0 sessions.", nameof(sessions));

            this.sessions = sessions;
        }

        #endregion

        #region Properties

        public ReadOnlyCollection<T1> Sessions
        {
            get
            {
                return sessions.AsReadOnly();
            }
        }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if (!(obj is BaseCycle<T1, T2>))
                return false;

            return this.Equals((BaseCycle<T1, T2>)obj);
        }

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

        public bool Equals(BaseCycle<T1, T2> other)
        {
            if ((object)other == null)
                return false;

            return this.Sessions.ListEquals(other.Sessions);
        }

        #endregion

        #region Operators

        public static bool operator ==(BaseCycle<T1, T2> first, BaseCycle<T1, T2> second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(BaseCycle<T1, T2> first, BaseCycle<T1, T2> second)
        {
            return !(first == second);
        }

        #endregion
    }
}
