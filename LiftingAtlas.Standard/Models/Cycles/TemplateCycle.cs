using System;
using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    public class TemplateCycle<T1, T2> : BaseCycle<T1, T2>, IEquatable<TemplateCycle<T1, T2>> where T1 : TemplateSession<T2> where T2 : TemplateSet
    {
        #region Private fields

        private readonly CycleTemplateName cycleTemplateName;
        private readonly Lift templateLift;

        #endregion

        #region Constructors

        public TemplateCycle(
            CycleTemplateName cycleTemplateName,
            Lift templateLift,
            List<T1> sessions
            ) : base(
                sessions
                )
        {
            if (cycleTemplateName == null)
                throw new ArgumentNullException(nameof(cycleTemplateName));

            if (templateLift == Lift.None)
                throw new ArgumentException("Unspecified lift.", nameof(templateLift));

            this.cycleTemplateName = cycleTemplateName;
            this.templateLift = templateLift;
        }

        #endregion

        #region Properties

        public CycleTemplateName CycleTemplateName
        {
            get
            {
                return cycleTemplateName;
            }
        }

        public Lift TemplateLift
        {
            get
            {
                return templateLift;
            }
        }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if (!(obj is TemplateCycle<T1, T2>))
                return false;

            return this.Equals((TemplateCycle<T1, T2>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int HashCode = this.CycleTemplateName != null ? this.CycleTemplateName.GetHashCode() : 0;
                HashCode = (HashCode * 397) ^ this.TemplateLift.GetHashCode();
                HashCode = (HashCode * 397) ^ base.GetHashCode();
                return HashCode;
            }
        }

        public bool Equals(TemplateCycle<T1, T2> other)
        {
            if ((object)other == null)
                return false;

            return (
                (this.CycleTemplateName == other.CycleTemplateName)
                &&
                (this.TemplateLift == other.TemplateLift)
                &&
                base.Equals(other)
                );
        }

        #endregion

        #region Operators

        public static bool operator ==(TemplateCycle<T1, T2> first, TemplateCycle<T1, T2> second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(TemplateCycle<T1, T2> first, TemplateCycle<T1, T2> second)
        {
            return !(first == second);
        }

        #endregion
    }
}
