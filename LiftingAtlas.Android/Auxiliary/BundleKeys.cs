namespace LiftingAtlas.Droid
{
    /// <summary>
    /// Bundle keys.
    /// </summary>
    public static class BundleKeys
    {
        #region Private fields

        /// <summary>
        /// A field behind <see cref="Lift"/>.
        /// </summary>
        private const string lift = "Lift";

        /// <summary>
        /// A field behind <see cref="SessionNumber"/>.
        /// </summary>
        private const string sessionNumber = "SessionNumber";

        /// <summary>
        /// A field behind <see cref="SetNumber"/>.
        /// </summary>
        private const string setNumber = "SetNumber";

        /// <summary>
        /// A field behind <see cref="PlannedCycleGuid"/>.
        /// </summary>
        private const string plannedCycleGuid = "Planned Cycle Guid";

        /// <summary>
        /// A field behind <see cref="PlannedCycleTemplateName"/>.
        /// </summary>
        private const string plannedCycleTemplateName = "Planned Cycle Template Name";

        /// <summary>
        /// A field behind <see cref="PlannedCycleReferencePoint"/>.
        /// </summary>
        private const string plannedCycleReferencePoint = "Planned Cycle Reference Point";

        /// <summary>
        /// A field behind <see cref="CycleTemplateName"/>.
        /// </summary>
        private const string cycleTemplateName = "Cycle Template Name";

        #endregion

        #region Properties

        /// <summary>
        /// <see cref="Lift"/> key.
        /// </summary>
        public static string Lift
        {
            get
            {
                return lift;
            }
        }

        /// <summary>
        /// <see cref="SessionNumber"/> key.
        /// </summary>
        public static string SessionNumber
        {
            get
            {
                return sessionNumber;
            }
        }

        /// <summary>
        /// <see cref="SetNumber"/> key.
        /// </summary>
        public static string SetNumber
        {
            get
            {
                return setNumber;
            }
        }

        /// <summary>
        /// <see cref="PlannedCycleGuid"/> key.
        /// </summary>
        public static string PlannedCycleGuid
        {
            get
            {
                return plannedCycleGuid;
            }
        }

        /// <summary>
        /// <see cref="PlannedCycleTemplateName"/> key.
        /// </summary>
        public static string PlannedCycleTemplateName
        {
            get
            {
                return plannedCycleTemplateName;
            }
        }

        /// <summary>
        /// <see cref="PlannedCycleReferencePoint"/> key.
        /// </summary>
        public static string PlannedCycleReferencePoint
        {
            get
            {
                return plannedCycleReferencePoint;
            }
        }

        /// <summary>
        /// <see cref="CycleTemplateName"/> key.
        /// </summary>
        public static string CycleTemplateName
        {
            get
            {
                return cycleTemplateName;
            }
        }

        #endregion
    }
}