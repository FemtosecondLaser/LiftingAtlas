namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Planned set view.
    /// </summary>
    public interface IPlannedSetView
    {
        /// <summary>
        /// Outputs planned percentage of reference point.
        /// </summary>
        /// <param name="plannedPercentageOfReferencePoint">
        /// Planned percentage of reference point to output.</param>
        void OutputPlannedPercentageOfReferencePoint(
            string plannedPercentageOfReferencePoint
            );

        /// <summary>
        /// Outputs weight adjustment constant.
        /// </summary>
        /// <param name="weightAdjustmentConstant">
        /// Weight adjustment constant to output.</param>
        void OutputWeightAdjustmentConstant(
            string weightAdjustmentConstant
            );

        /// <summary>
        /// Outputs planned weight.
        /// </summary>
        /// <param name="plannedWeight">
        /// Planned weight to output.</param>
        void OutputPlannedWeight(
            string plannedWeight
            );

        /// <summary>
        /// Outputs planned repetitions.
        /// </summary>
        /// <param name="plannedRepetitions">
        /// Planned repetitions to output.</param>
        void OutputPlannedRepetitions(
            string plannedRepetitions
            );

        /// <summary>
        /// Outputs lifted values.
        /// </summary>
        /// <param name="liftedWeight">
        /// Lifted weight to output.</param>
        /// <param name="liftedRepetitions">
        /// Lifted repetitions to output.</param>
        void OutputLiftedValues(
            string liftedWeight,
            string liftedRepetitions
            );

        /// <summary>
        /// Outputs note.
        /// </summary>
        /// <param name="note">
        /// Note to output.</param>
        void OutputNote(
            string note
            );
    }
}
