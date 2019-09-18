namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Type representing a row of the LatestPlannedCycleList table.
    /// </summary>
    public class LatestPlannedCycleListDataset
    {
        /// <summary>
        /// Lift.
        /// </summary>
        public Lift lift { get; set; }

        /// <summary>
        /// Planned cycle guid.
        /// </summary>
        public string plannedCycleGuid { get; set; }
    }
}
