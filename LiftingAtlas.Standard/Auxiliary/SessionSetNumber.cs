using System;

namespace LiftingAtlas.Standard
{
    public class SessionSetNumber : IEquatable<SessionSetNumber>
    {
        #region Private fields

        private readonly SessionNumber sessionNumber;
        private readonly SetNumber setNumber;

        #endregion

        #region Constructors

        public SessionSetNumber(SessionNumber sessionNumber, SetNumber setNumber)
        {
            if (sessionNumber == null)
                throw new ArgumentNullException(nameof(sessionNumber));

            if (setNumber == null)
                throw new ArgumentNullException(nameof(setNumber));

            this.sessionNumber = sessionNumber;
            this.setNumber = setNumber;
        }

        #endregion

        #region Properties

        public SessionNumber SessionNumber
        {
            get
            {
                return sessionNumber;
            }
        }

        public SetNumber SetNumber
        {
            get
            {
                return setNumber;
            }
        }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if (!(obj is SessionSetNumber))
                return false;

            return this.Equals((SessionSetNumber)obj);
        }

        public override int GetHashCode()
        {
            return (this.SessionNumber, this.SetNumber).GetHashCode();
        }

        public bool Equals(SessionSetNumber other)
        {
            if ((object)other == null)
                return false;

            return (this.SessionNumber, this.SetNumber).Equals(
                (other.SessionNumber, other.SetNumber)
                );
        }

        #endregion

        #region Operators

        public static bool operator ==(SessionSetNumber first, SessionSetNumber second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(SessionSetNumber first, SessionSetNumber second)
        {
            return !(first == second);
        }

        #endregion
    }
}
