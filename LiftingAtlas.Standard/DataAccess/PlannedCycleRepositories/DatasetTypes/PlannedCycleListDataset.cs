namespace LiftingAtlas.Standard
{
    public class PlannedCycleListDataset
    {
        public string plannedCycleGuid { get; set; }

        public Lift plannedLift { get; set; }

        public double referencePoint { get; set; }

        public string plannedTimestamp { get; set; }

        public string cycleTemplateName { get; set; }

        public Lift templateLift { get; set; }
    }
}
