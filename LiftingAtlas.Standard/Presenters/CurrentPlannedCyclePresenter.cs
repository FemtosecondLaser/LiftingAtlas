using System;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Current planned cycle presenter.
    /// </summary>
    public class CurrentPlannedCyclePresenter : ICurrentPlannedCyclePresenter
    {
        #region Private fields

        /// <summary>
        /// Current planned cycle view.
        /// </summary>
        private readonly ICurrentPlannedCycleView currentPlannedCycleView;

        /// <summary>
        /// Planned cycle repository.
        /// </summary>
        private readonly IPlannedCycleRepository plannedCycleRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates current planned cycle presenter.
        /// </summary>
        /// <param name="currentPlannedCycleView">Current planned cycle view.
        /// Must not be null.</param>
        /// <param name="plannedCycleRepository">Planned cycle repository.
        /// Must not be null.</param>
        /// <exception cref="ArgumentNullException"><paramref name="currentPlannedCycleView"/>
        /// or <paramref name="plannedCycleRepository"/> is null.</exception>
        public CurrentPlannedCyclePresenter(
            ICurrentPlannedCycleView currentPlannedCycleView,
            IPlannedCycleRepository plannedCycleRepository
            )
        {
            if (currentPlannedCycleView == null)
                throw new ArgumentNullException(nameof(currentPlannedCycleView));

            if (plannedCycleRepository == null)
                throw new ArgumentNullException(nameof(plannedCycleRepository));

            this.currentPlannedCycleView = currentPlannedCycleView;
            this.plannedCycleRepository = plannedCycleRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Presents current planned cycle data for the <see cref="Lift"/>. 
        /// </summary>
        /// <param name="lift"><see cref="Lift"/>, to present current planned cycle data for.</param>
        public void PresentCurrentPlannedCycleDataForTheLift(Lift lift)
        {
            Guid? latestPlannedCycleForTheLiftGuid = this.plannedCycleRepository.GetLatestPlannedCycleGuid(lift);

            if (latestPlannedCycleForTheLiftGuid == null)
            {
                this.currentPlannedCycleView.OutputCurrentPlannedCycleTemplateName(null);
                this.currentPlannedCycleView.OutputCurrentPlannedCycleReferencePoint(null);
                this.currentPlannedCycleView.OutputCurrentPlannedCycleSessions(null);
                return;
            }

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> currentPlannedCycleForTheLift =
                this.plannedCycleRepository.GetPlannedCycle(latestPlannedCycleForTheLiftGuid.Value);

            this.currentPlannedCycleView.OutputCurrentPlannedCycleTemplateName(
                currentPlannedCycleForTheLift.CycleTemplateName
                );

            this.currentPlannedCycleView.OutputCurrentPlannedCycleReferencePoint(
                currentPlannedCycleForTheLift.ReferencePoint.ToString()
                );

            this.currentPlannedCycleView.OutputCurrentPlannedCycleSessions(
                currentPlannedCycleForTheLift.Sessions
                );
        }

        /// <summary>
        /// Gets current planned cycle guid for the <see cref="Lift"/>.
        /// </summary>
        /// <param name="lift"><see cref="Lift"/>,
        /// to get current planned cycle guid for.</param>
        /// <returns>Current planned cycle guid or
        /// null if current planned cycle for the lift does not exist.</returns>
        public Guid? GetCurrentPlannedCycleGuid(Lift lift)
        {
            return this.plannedCycleRepository.GetLatestPlannedCycleGuid(lift);
        }

        /// <summary>
        /// Gets current planned session number for planned cycle.
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle to return
        /// current planned session number for.
        /// <returns>Current planned session number or
        /// null if current planned session for the planned cycle does not exist.</returns>
        public int? GetCurrentPlannedSessionNumber(Guid plannedCycleGuid)
        {
            (int currentPlannedSessionNumber, int currentPlannedSetNumber)? currentPlannedSessionAndCurrentPlannedSetNumbers =
                this.plannedCycleRepository.GetCurrentPlannedSessionAndCurrentPlannedSetNumbers(
                    plannedCycleGuid
                    );

            return currentPlannedSessionAndCurrentPlannedSetNumbers?.currentPlannedSessionNumber;
        }

        #endregion
    }
}
