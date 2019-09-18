namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Type representing a row of the PlannedCycleList table.
    /// </summary>
    public class PlannedCycleListDataset
    {
        /// <summary>
        /// Planned cycle guid.
        /// </summary>
        public string plannedCycleGuid { get; set; }

        /// <summary>
        /// Planned lift.
        /// </summary>
        public Lift plannedLift { get; set; }

        /// <summary>
        /// Reference point.
        /// </summary>
        public double referencePoint { get; set; }

        /// <summary>
        /// Planned timestamp.
        /// </summary>
        public string plannedTimestamp { get; set; }

        /// <summary>
        /// Cycle template name.
        /// </summary>
        public string cycleTemplateName { get; set; }

        /// <summary>
        /// Template lift.
        /// </summary>
        public Lift templateLift { get; set; }
    }
}
