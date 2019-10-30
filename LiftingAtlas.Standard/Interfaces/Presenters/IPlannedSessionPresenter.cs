using System;

namespace LiftingAtlas.Standard
{
    public interface IPlannedSessionPresenter
    {
        void PresentPlannedSessionData(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber
            );

        SessionSetNumber GetCurrentPlannedSessionAndCurrentPlannedSetNumbers(
            Guid plannedCycleGuid
            );
    }
}
