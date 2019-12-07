using System;
using System.Threading.Tasks;

namespace LiftingAtlas.Standard
{
    public interface IPlannedSetPresenter
    {
        Task PresentPlannedSetDataAsync(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber
            );

        Task<bool> PlannedSetIsCurrentAsync(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber
            );

        Task<bool> RepetitionsWithinPlannedRangeAsync(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber,
            Repetitions repetitions
            );

        Task<bool> WeightWithinPlannedRangeAsync(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber,
            Weight weight
            );

        Task UpdatePlannedSetLiftedValuesAsync(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber,
            LiftedValues liftedValues
            );
    }
}
