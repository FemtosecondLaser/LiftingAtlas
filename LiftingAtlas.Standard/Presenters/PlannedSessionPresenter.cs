using System;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Planned session presenter.
    /// </summary>
    public class PlannedSessionPresenter : IPlannedSessionPresenter
    {
        #region Private fields

        /// <summary>
        /// Planned session view.
        /// </summary>
        private readonly IPlannedSessionView plannedSessionView;

        /// <summary>
        /// Planned cycle repository.
        /// </summary>
        private readonly IPlannedCycleRepository plannedCycleRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates planned session presenter.
        /// </summary>
        /// <param name="plannedSessionView">Planned session view.
        /// Must not be null.</param>
        /// <param name="plannedCycleRepository">Planned cycle repository.
        /// Must not be null.</param>
        /// <exception cref="ArgumentNullException"><paramref name="plannedSessionView"/>
        /// or <paramref name="plannedCycleRepository"/> is null.</exception>
        public PlannedSessionPresenter(
            IPlannedSessionView plannedSessionView,
            IPlannedCycleRepository plannedCycleRepository
            )
        {
            if (plannedSessionView == null)
                throw new ArgumentNullException(nameof(plannedSessionView));

            if (plannedCycleRepository == null)
                throw new ArgumentNullException(nameof(plannedCycleRepository));

            this.plannedSessionView = plannedSessionView;
            this.plannedCycleRepository = plannedCycleRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Presents planned session data. 
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle,
        /// to present session data for.</param>
        /// <param name="plannedSessionNumber">
        /// Number of planned session, to present data for.</param>
        public void PresentPlannedSessionData(
            Guid plannedCycleGuid,
            int plannedSessionNumber
            )
        {
            PlannedSession<PlannedSet> plannedSession =
                this.plannedCycleRepository.GetPlannedSession(
                    plannedCycleGuid,
                    plannedSessionNumber
                    );

            this.plannedSessionView.OutputPlannedSessionSets(
                plannedSession.Sets
                );
        }

        /// <summary>
        /// Gets current planned session and current planned set numbers for planned cycle.
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle to return
        /// current planned session and current planned set numbers for.
        /// <returns>Current planned session and current planned set numbers or
        /// null if current planned session and current planned set for the planned cycle do not exist.</returns>
        public (int currentPlannedSessionNumber, int currentPlannedSetNumber)? GetCurrentPlannedSessionAndCurrentPlannedSetNumbers(
            Guid plannedCycleGuid
            )
        {
            return this.plannedCycleRepository.GetCurrentPlannedSessionAndCurrentPlannedSetNumbers(
                plannedCycleGuid
                );
        }

        #endregion
    }
}
