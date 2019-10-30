using System;

namespace LiftingAtlas.Standard
{
    public class PlannedSessionPresenter : IPlannedSessionPresenter
    {
        #region Private fields

        private readonly IPlannedSessionView plannedSessionView;
        private readonly IPlannedCycleRepository plannedCycleRepository;

        #endregion

        #region Constructors

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

        public void PresentPlannedSessionData(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber
            )
        {
            if (plannedSessionNumber == null)
                throw new ArgumentNullException(nameof(plannedSessionNumber));

            PlannedSession<PlannedSet> plannedSession =
                this.plannedCycleRepository.GetPlannedSession(
                    plannedCycleGuid,
                    plannedSessionNumber
                    );

            this.plannedSessionView.OutputPlannedSessionSets(
                plannedSession.Sets
                );
        }

        public SessionSetNumber GetCurrentPlannedSessionAndCurrentPlannedSetNumbers(
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
