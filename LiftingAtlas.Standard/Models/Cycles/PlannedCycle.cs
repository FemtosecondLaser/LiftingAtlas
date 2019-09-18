using System;
using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Class containing list of sessions.
    /// </summary>
    public class PlannedCycle<T1, T2> : TemplateCycle<T1, T2>, IEquatable<PlannedCycle<T1, T2>> where T1 : PlannedSession<T2> where T2 : PlannedSet
    {
        #region Private fields

        /// <summary>
        /// A field behind <see cref="PlannedCycleGuid"/>.
        /// </summary>
        private readonly Guid plannedCycleGuid;

        /// <summary>
        /// A field behind <see cref="PlannedLift"/>.
        /// </summary>
        private readonly Lift plannedLift;

        /// <summary>
        /// A field behind <see cref="ReferencePoint"/>.
        /// </summary>
        private readonly double referencePoint;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a cycle.
        /// </summary>
        /// <param name="plannedCycleGuid">Planned cycle guid. Must not be null.</param>
        /// <param name="plannedLift">Lift this cycle is planned for. Must be specified.
        /// Must be the lift the cycle template is designed for.</param>
        /// <param name="referencePoint">Reference point used to plan this cycle. Must not be less than 0.</param>
        /// <param name="cycleTemplateName">Cycle template name. Must not be null, empty or contain only white-space characters.</param>
        /// <param name="templateLift">Lift this cycle template is designed for. Must be specified.</param>
        /// <param name="sessions">Collection of sessions this cycle consists of. Must not be null. Must contain more than 0 sessions.</param>
        /// <exception cref="ArgumentNullException"><paramref name="plannedCycleGuid"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="plannedLift"/> is unspecified
        /// or is not the lift the cycle template is designed for.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="referencePoint"/> is less than 0.</exception>
        public PlannedCycle(
            Guid plannedCycleGuid,
            Lift plannedLift,
            double referencePoint,
            string cycleTemplateName,
            Lift templateLift,
            List<T1> sessions
            ) : base(
                cycleTemplateName,
                templateLift,
                sessions
                )
        {
            if (plannedCycleGuid == null)
                throw new ArgumentNullException(nameof(plannedCycleGuid));

            if (plannedLift == Lift.None)
                throw new ArgumentException($"Unspecified {nameof(plannedLift)}.");

            if (!templateLift.HasFlag(plannedLift))
                throw new ArgumentException($"{nameof(plannedLift)} is not the lift the cycle template is designed for.");

            if (referencePoint < 0.00)
                throw new ArgumentOutOfRangeException(nameof(referencePoint));

            this.plannedCycleGuid = plannedCycleGuid;
            this.plannedLift = plannedLift;
            this.referencePoint = referencePoint;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Planned cycle guid.
        /// </summary>
        public Guid PlannedCycleGuid
        {
            get
            {
                return plannedCycleGuid;
            }
        }

        /// <summary>
        /// Lift this cycle is planned for.
        /// </summary>
        public Lift PlannedLift
        {
            get
            {
                return plannedLift;
            }
        }

        /// <summary>
        /// Reference point used to plan this cycle.
        /// </summary>
        public double ReferencePoint
        {
            get
            {
                return referencePoint;
            }
        }

        /// <summary>
        /// Indicates if this cycle is done.
        /// </summary>
        public bool Done
        {
            get
            {
                foreach (T1 session in this.Sessions)
                    if (!session.Done)
                        return false;

                return true;
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
            if (!(obj is PlannedCycle<T1, T2>))
                return false;

            return this.Equals((PlannedCycle<T1, T2>)obj);
        }

        /// <summary>
        /// Computes the hash code for this object.
        /// </summary>
        /// <returns>Hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int HashCode = this.PlannedCycleGuid != null ? this.PlannedCycleGuid.GetHashCode() : 0;
                HashCode = (HashCode * 397) ^ this.PlannedLift.GetHashCode();
                HashCode = (HashCode * 397) ^ this.ReferencePoint.GetHashCode();
                HashCode = (HashCode * 397) ^ this.Done.GetHashCode();
                HashCode = (HashCode * 397) ^ base.GetHashCode();
                return HashCode;
            }
        }

        /// <summary>
        /// Compares this instance of the class with an instance of <see cref="PlannedCycle{T1, T2}"/>.
        /// </summary>
        /// <param name="other">An instance of <see cref="PlannedCycle{T1, T2}"/> to compare with.</param>
        /// <returns>Comparison result.</returns>
        public bool Equals(PlannedCycle<T1, T2> other)
        {
            if ((object)other == null)
                return false;

            return (
                (this.PlannedCycleGuid == other.PlannedCycleGuid)
                &&
                (this.PlannedLift == other.PlannedLift)
                &&
                (this.ReferencePoint == other.ReferencePoint)
                &&
                (this.Done == other.Done)
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
        public static bool operator ==(PlannedCycle<T1, T2> first, PlannedCycle<T1, T2> second)
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
        public static bool operator !=(PlannedCycle<T1, T2> first, PlannedCycle<T1, T2> second)
        {
            return !(first == second);
        }

        #endregion
    }
}
