using System;
using System.Threading.Tasks;

namespace LiftingAtlas.Standard
{
    public interface ICurrentPlannedCyclePresenter
    {
        event CurrentPlannedCycleDataPresentedEventHandler CurrentPlannedCycleDataPresented;

        Task PresentCurrentPlannedCycleDataForTheLiftAsync(
            Lift lift
            );

        Task<Guid?> GetCurrentPlannedCycleGuidAsync(
            Lift lift
            );
    }
}
