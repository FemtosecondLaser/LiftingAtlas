using System;

namespace LiftingAtlas.Standard
{
    public interface ICurrentPlannedCyclePresenter
    {
        void PresentCurrentPlannedCycleDataForTheLift(
            Lift lift
            );

        Guid? GetCurrentPlannedCycleGuid(
            Lift lift
            );

        SessionNumber GetCurrentPlannedSessionNumber(
            Guid plannedCycleGuid
            );
    }
}
