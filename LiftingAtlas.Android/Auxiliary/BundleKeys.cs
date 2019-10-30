namespace LiftingAtlas.Droid
{
    public static class BundleKeys
    {
        #region Private fields

        private const string lift = "Lift";
        private const string sessionNumber = "SessionNumber";
        private const string setNumber = "SetNumber";
        private const string plannedCycleGuid = "Planned Cycle Guid";
        private const string plannedCycleTemplateName = "Planned Cycle Template Name";
        private const string plannedCycleReferencePoint = "Planned Cycle Reference Point";
        private const string cycleTemplateName = "Cycle Template Name";

        #endregion

        #region Properties

        public static string Lift
        {
            get
            {
                return lift;
            }
        }

        public static string SessionNumber
        {
            get
            {
                return sessionNumber;
            }
        }

        public static string SetNumber
        {
            get
            {
                return setNumber;
            }
        }

        public static string PlannedCycleGuid
        {
            get
            {
                return plannedCycleGuid;
            }
        }

        public static string PlannedCycleTemplateName
        {
            get
            {
                return plannedCycleTemplateName;
            }
        }

        public static string PlannedCycleReferencePoint
        {
            get
            {
                return plannedCycleReferencePoint;
            }
        }

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