using System;
using System.Threading.Tasks;

namespace LiftingAtlas.Standard
{
    public interface IPlannedSessionPresenter
    {
        Task PresentPlannedSessionDataAsync(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber
            );
    }
}
