using System;
using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Class containing list of sessions.
    /// </summary>
    public class TemplateCycle<T1, T2> : BaseCycle<T1, T2>, IEquatable<TemplateCycle<T1, T2>> where T1 : TemplateSession<T2> where T2 : TemplateSet
    {
        #region Private fields

        /// <summary>
        /// A field behind <see cref="CycleTemplateName"/>.
        /// </summary>
        private readonly string cycleTemplateName;

        /// <summary>
        /// A field behind <see cref="TemplateLift"/>.
        /// </summary>
        private readonly Lift templateLift;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a cycle.
        /// </summary>
        /// <param name="cycleTemplateName">Cycle template name. Must not be null, empty or contain only white-space characters.</param>
        /// <param name="templateLift">Lift this cycle template is designed for. Must be specified.</param>
        /// <param name="sessions">Collection of sessions this cycle consists of. Must not be null. Must contain more than 0 sessions.</param>
        /// <exception cref="ArgumentException"><paramref name="templateLift"/> is unspecified or
        /// <paramref name="cycleTemplateName"/> is either null, empty or contains only white-space characters.</exception>
        public TemplateCycle(string cycleTemplateName, Lift templateLift, List<T1> sessions) : base(sessions)
        {
            if (string.IsNullOrWhiteSpace(cycleTemplateName))
                throw new ArgumentException(
                    "Cycle template name must not be null, empty or contain only white-space characters.",
                    nameof(cycleTemplateName)
                    );

            if (templateLift == Lift.None)
                throw new ArgumentException("Unspecified lift.", nameof(templateLift));

            this.cycleTemplateName = cycleTemplateName;
            this.templateLift = templateLift;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Cycle template name.
        /// </summary>
        public string CycleTemplateName
        {
            get
            {
                return cycleTemplateName;
            }
        }

        /// <summary>
        /// Lift this cycle template is designed for.
        /// </summary>
        public Lift TemplateLift
        {
            get
            {
                return templateLift;
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
            if (!(obj is TemplateCycle<T1, T2>))
                return false;

            return this.Equals((TemplateCycle<T1, T2>)obj);
        }

        /// <summary>
        /// Computes the hash code for this object.
        /// </summary>
        /// <returns>Hash code.</returns>
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

        /// <summary>
        /// Compares this instance of the class with an instance of <see cref="TemplateCycle{T1, T2}"/>.
        /// </summary>
        /// <param name="other">An instance of <see cref="TemplateCycle{T1, T2}"/> to compare with.</param>
        /// <returns>Comparison result.</returns>
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

        /// <summary>
        /// Determines equality of operands.
        /// </summary>
        /// <param name="first">First operand.</param>
        /// <param name="second">Second operand.</param>
        /// <returns>True if operands are equal;
        /// otherwise, false.</returns>
        public static bool operator ==(TemplateCycle<T1, T2> first, TemplateCycle<T1, T2> second)
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
        public static bool operator !=(TemplateCycle<T1, T2> first, TemplateCycle<T1, T2> second)
        {
            return !(first == second);
        }

        #endregion
    }
}
