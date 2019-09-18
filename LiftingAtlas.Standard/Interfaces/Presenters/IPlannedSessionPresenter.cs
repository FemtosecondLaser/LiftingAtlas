using System;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Planned session presenter.
    /// </summary>
    public interface IPlannedSessionPresenter
    {
        /// <summary>
        /// Presents planned session data. 
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle,
        /// to present session data for.</param>
        /// <param name="plannedSessionNumber">
        /// Number of planned session, to present data for.</param>
        void PresentPlannedSessionData(
            Guid plannedCycleGuid,
            int plannedSessionNumber
            );

        /// <summary>
        /// Gets current planned session and current planned set numbers for planned cycle.
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle to return
        /// current planned session and current planned set numbers for.
        /// <returns>Current planned session and current planned set numbers.</returns>
        (int currentPlannedSessionNumber, int currentPlannedSetNumber)? GetCurrentPlannedSessionAndCurrentPlannedSetNumbers(
            Guid plannedCycleGuid
            );
    }
}
