namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Type representing a row of planned cycle.
    /// </summary>
    public class PlannedCycleDataset
    {
        /// <summary>
        /// Session number.
        /// </summary>
        public int sessionNumber { get; set; }

        /// <summary>
        /// Set number.
        /// </summary>
        public int setNumber { get; set; }

        /// <summary>
        /// Planned percentage of reference point lower bound.
        /// </summary>
        public int? plannedPercentageOfReferencePointLowerBound { get; set; }

        /// <summary>
        /// Planned percentage of reference point upper bound.
        /// </summary>
        public int? plannedPercentageOfReferencePointUpperBound { get; set; }

        /// <summary>
        /// Pplanned repetitions lower bound.
        /// </summary>
        public int? plannedRepetitionsLowerBound { get; set; }

        /// <summary>
        /// Pplanned repetitions upper bound.
        /// </summary>
        public int? plannedRepetitionsUpperBound { get; set; }

        /// <summary>
        /// Weight adjustment constant.
        /// </summary>
        public double? weightAdjustmentConstant { get; set; }

        /// <summary>
        /// Note.
        /// </summary>
        public string note { get; set; }

        /// <summary>
        /// Planned weight lower bound.
        /// </summary>
        public double? plannedWeightLowerBound { get; set; }

        /// <summary>
        /// Planned weight upper bound.
        /// </summary>
        public double? plannedWeightUpperBound { get; set; }

        /// <summary>
        /// Lifted repetitions.
        /// </summary>
        public int? liftedRepetitions { get; set; }

        /// <summary>
        /// Lifted weight.
        /// </summary>
        public double? liftedWeight { get; set; }

        /// <summary>
        /// Lifted timestamp.
        /// </summary>
        public string liftedTimestamp { get; set; }
    }
}
