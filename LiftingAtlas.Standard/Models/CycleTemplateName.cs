using System;

namespace LiftingAtlas.Standard
{
    public class CycleTemplateName : IEquatable<CycleTemplateName>
    {
        #region Private fields

        private readonly string name;

        #endregion

        #region Constructors

        public CycleTemplateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(
                    "Name must not be null, empty or contain only white-space characters.",
                    nameof(name)
                    );

            this.name = name;
        }

        #endregion

        #region Properties

        public string Name
        {
            get
            {
                return name;
            }
        }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if (!(obj is CycleTemplateName))
                return false;

            return this.Equals((CycleTemplateName)obj);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public bool Equals(CycleTemplateName other)
        {
            if ((object)other == null)
                return false;

            return this.Name == other.Name;
        }

        public override string ToString()
        {
            return this.Name;
        }

        #endregion

        #region Operators

        public static implicit operator string(CycleTemplateName cycleTemplateName)
        {
            return cycleTemplateName?.Name;
        }

        public static bool operator ==(CycleTemplateName first, CycleTemplateName second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(CycleTemplateName first, CycleTemplateName second)
        {
            return !(first == second);
        }

        #endregion
    }
}
