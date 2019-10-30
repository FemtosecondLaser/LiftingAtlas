using System;
using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    public class PlannedCycle<T1, T2> : TemplateCycle<T1, T2>, IEquatable<PlannedCycle<T1, T2>> where T1 : PlannedSession<T2> where T2 : PlannedSet
    {
        #region Private fields

        private readonly Guid plannedCycleGuid;
        private readonly Lift plannedLift;
        private readonly Weight referencePoint;

        #endregion

        #region Constructors

        public PlannedCycle(
            Guid plannedCycleGuid,
            Lift plannedLift,
            Weight referencePoint,
            CycleTemplateName cycleTemplateName,
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
                throw new ArgumentException("Unspecified lift.", nameof(plannedLift));

            if (referencePoint == null)
                throw new ArgumentNullException(nameof(referencePoint));

            if (!templateLift.HasFlag(plannedLift))
                throw new ArgumentException(
                    "Lift is not the lift the cycle template is designed for.",
                    nameof(plannedLift)
                    );

            this.plannedCycleGuid = plannedCycleGuid;
            this.plannedLift = plannedLift;
            this.referencePoint = referencePoint;
        }

        #endregion

        #region Properties

        public Guid PlannedCycleGuid
        {
            get
            {
                return plannedCycleGuid;
            }
        }

        public Lift PlannedLift
        {
            get
            {
                return plannedLift;
            }
        }

        public Weight ReferencePoint
        {
            get
            {
                return referencePoint;
            }
        }

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

        public override bool Equals(object obj)
        {
            if (!(obj is PlannedCycle<T1, T2>))
                return false;

            return this.Equals((PlannedCycle<T1, T2>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int HashCode = this.PlannedCycleGuid != null ? this.PlannedCycleGuid.GetHashCode() : 0;
                HashCode = (HashCode * 397) ^ this.PlannedLift.GetHashCode();
                HashCode = (HashCode * 397) ^ (this.ReferencePoint != null ? this.ReferencePoint.GetHashCode() : 0);
                HashCode = (HashCode * 397) ^ this.Done.GetHashCode();
                HashCode = (HashCode * 397) ^ base.GetHashCode();
                return HashCode;
            }
        }

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

        public static bool operator ==(PlannedCycle<T1, T2> first, PlannedCycle<T1, T2> second)
        {
            if (ReferenceEquals(first, second))
                return true;

            if (((object)first == null) || ((object)second == null))
                return false;

            return first.Equals(second);
        }

        public static bool operator !=(PlannedCycle<T1, T2> first, PlannedCycle<T1, T2> second)
        {
            return !(first == second);
        }

        #endregion
    }
}
