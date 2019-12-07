using System;
using System.Threading.Tasks;

namespace LiftingAtlas.Standard
{
    public interface ICurrentPlannedCyclePresenter
    {
        Task PresentCurrentPlannedCycleDataForTheLiftAsync(
            Lift lift
            );

        Task<Guid?> GetCurrentPlannedCycleGuidAsync(
            Lift lift
            );
    }
}
