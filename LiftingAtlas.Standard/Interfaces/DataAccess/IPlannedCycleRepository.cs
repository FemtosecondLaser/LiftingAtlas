using System;
using System.Threading.Tasks;

namespace LiftingAtlas.Standard
{
    public interface IPlannedCycleRepository
    {
        Task PlanCycleAsync(
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> cycleTemplate,
            Lift lift,
            Weight referencePoint,
            IQuantizationProvider quantizationProvider
            );

        Task<Guid?> GetLatestPlannedCycleGuidAsync(Lift lift);

        Task<PlannedCycle<PlannedSession<PlannedSet>, PlannedSet>> GetPlannedCycleAsync(
            Guid plannedCycleGuid
            );

        Task<PlannedSession<PlannedSet>> GetPlannedSessionAsync(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber
            );

        Task<PlannedSet> GetPlannedSetAsync(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber
            );

        Task UpdatePlannedSetLiftedValuesAsync(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber,
            LiftedValues liftedValues
            );

        Task<SessionSetNumber> GetCurrentPlannedSessionAndCurrentPlannedSetNumbersAsync(
            Guid plannedCycleGuid
            );
    }
}
