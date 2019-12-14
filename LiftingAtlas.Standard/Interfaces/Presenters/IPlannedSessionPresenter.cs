using System;
using System.Threading.Tasks;

namespace LiftingAtlas.Standard
{
    public interface IPlannedSessionPresenter
    {
        event PlannedSessionDataPresentedEventHandler PlannedSessionDataPresented;

        Task PresentPlannedSessionDataAsync(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber
            );
    }
}
