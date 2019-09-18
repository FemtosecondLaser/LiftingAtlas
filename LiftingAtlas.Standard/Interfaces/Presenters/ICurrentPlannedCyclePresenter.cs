using System;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Current planned cycle presenter.
    /// </summary>
    public interface ICurrentPlannedCyclePresenter
    {
        /// <summary>
        /// Presents current planned cycle data for the <see cref="Lift"/>. 
        /// </summary>
        /// <param name="lift"><see cref="Lift"/>,
        /// to present current planned cycle data for.</param>
        void PresentCurrentPlannedCycleDataForTheLift(
            Lift lift
            );

        /// <summary>
        /// Gets current planned cycle guid for the <see cref="Lift"/>.
        /// </summary>
        /// <param name="lift"><see cref="Lift"/>,
        /// to get current planned cycle guid for.</param>
        /// <returns>Current planned cycle guid.</returns>
        Guid? GetCurrentPlannedCycleGuid(
            Lift lift
            );

        /// <summary>
        /// Gets current planned session number for planned cycle.
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle to return
        /// current planned session number for.
        /// <returns>Current planned session number.</returns>
        int? GetCurrentPlannedSessionNumber(
            Guid plannedCycleGuid
            );
    }
}
