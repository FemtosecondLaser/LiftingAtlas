﻿using System;

namespace LiftingAtlas.Standard
{
    public class Repetitions : IEquatable<Repetitions>
    {
        #region Private fields

        private readonly int value;

        #endregion

        #region Constructors

        public Repetitions(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            this.value = value;
        }

        #endregion

        #region Properties

        public int Value
        {
            get
            {
                return value;
            }
        }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if (!(obj is Repetitions))
                return false;

            return this.Equals((Repetitions)obj);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public bool Equals(Repetitions other)
        {
            if ((object)other == null)
                return false;

            return this.Value == other.Value;
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        #endregion

        #region Operators

        public static implicit operator int(Repetitions repetitions)
        {
            return repetitions.Value;
        }

        public static bool operator ==(Repetitions first, Repetitions second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(Repetitions first, Repetitions second)
        {
            return !(first == second);
        }

        public static bool operator <(Repetitions first, Repetitions second)
        {
            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Value < second.Value;
        }

        public static bool operator >(Repetitions first, Repetitions second)
        {
            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Value > second.Value;
        }

        public static bool operator <=(Repetitions first, Repetitions second)
        {
            return first < second || first == second;
        }

        public static bool operator >=(Repetitions first, Repetitions second)
        {
            return first > second || first == second;
        }

        #endregion
    }
}
