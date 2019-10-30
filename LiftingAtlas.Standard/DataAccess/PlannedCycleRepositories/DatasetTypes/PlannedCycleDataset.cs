namespace LiftingAtlas.Standard
{
    public class PlannedCycleDataset
    {
        public int sessionNumber { get; set; }

        public int setNumber { get; set; }

        public int? plannedPercentageOfReferencePointLowerBound { get; set; }

        public int? plannedPercentageOfReferencePointUpperBound { get; set; }

        public int? plannedRepetitionsLowerBound { get; set; }

        public int? plannedRepetitionsUpperBound { get; set; }

        public double? weightAdjustmentConstant { get; set; }

        public string note { get; set; }

        public double? plannedWeightLowerBound { get; set; }

        public double? plannedWeightUpperBound { get; set; }

        public int? liftedRepetitions { get; set; }

        public double? liftedWeight { get; set; }

        public string liftedTimestamp { get; set; }
    }
}
