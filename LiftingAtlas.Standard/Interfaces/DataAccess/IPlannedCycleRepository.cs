using System;

namespace LiftingAtlas.Standard
{
    public interface IPlannedCycleRepository
    {
        void PlanCycle(
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> cycleTemplate,
            Lift lift,
            Weight referencePoint,
            IQuantizationProvider quantizationProvider
            );

        Guid? GetLatestPlannedCycleGuid(Lift lift);

        PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> GetPlannedCycle(Guid plannedCycleGuid);

        PlannedSession<PlannedSet> GetPlannedSession(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber
            );

        PlannedSet GetPlannedSet(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber
            );

        void UpdatePlannedSetLiftedValues(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber,
            LiftedValues liftedValues
            );

        SessionSetNumber GetCurrentPlannedSessionAndCurrentPlannedSetNumbers(
            Guid plannedCycleGuid
            );
    }
}
