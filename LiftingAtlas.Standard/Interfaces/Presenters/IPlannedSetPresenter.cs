using System;

namespace LiftingAtlas.Standard
{
    public interface IPlannedSetPresenter
    {
        void PresentPlannedSetData(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber
            );

        bool PlannedSetIsCurrent(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber
            );

        bool RepetitionsWithinPlannedRange(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber,
            Repetitions repetitions
            );

        bool WeightWithinPlannedRange(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber,
            Weight weight
            );

        void UpdatePlannedSetLiftedValues(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber,
            LiftedValues liftedValues
            );
    }
}
