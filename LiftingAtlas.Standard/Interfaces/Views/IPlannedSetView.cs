namespace LiftingAtlas.Standard
{
    public interface IPlannedSetView
    {
        void OutputPlannedPercentageOfReferencePoint(
            PlannedPercentageOfReferencePoint plannedPercentageOfReferencePoint
            );

        void OutputWeightAdjustmentConstant(
            WeightAdjustmentConstant weightAdjustmentConstant
            );

        void OutputPlannedWeight(
            PlannedWeight plannedWeight
            );

        void OutputPlannedRepetitions(
            PlannedRepetitions plannedRepetitions
            );

        void OutputLiftedValues(
            LiftedValues liftedValues
            );

        void OutputNote(
            string note
            );
    }
}
