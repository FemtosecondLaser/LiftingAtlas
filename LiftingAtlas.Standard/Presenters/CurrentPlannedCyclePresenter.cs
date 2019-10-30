using System;

namespace LiftingAtlas.Standard
{
    public class CurrentPlannedCyclePresenter : ICurrentPlannedCyclePresenter
    {
        #region Private fields

        private readonly ICurrentPlannedCycleView currentPlannedCycleView;
        private readonly IPlannedCycleRepository plannedCycleRepository;

        #endregion

        #region Constructors

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
                currentPlannedCycleForTheLift.ReferencePoint
                );

            this.currentPlannedCycleView.OutputCurrentPlannedCycleSessions(
                currentPlannedCycleForTheLift.Sessions
                );
        }

        public Guid? GetCurrentPlannedCycleGuid(Lift lift)
        {
            return this.plannedCycleRepository.GetLatestPlannedCycleGuid(lift);
        }

        public SessionNumber GetCurrentPlannedSessionNumber(Guid plannedCycleGuid)
        {
            SessionSetNumber currentPlannedSessionAndCurrentPlannedSetNumbers =
                this.plannedCycleRepository.GetCurrentPlannedSessionAndCurrentPlannedSetNumbers(
                    plannedCycleGuid
                    );

            if (currentPlannedSessionAndCurrentPlannedSetNumbers != null)
                return currentPlannedSessionAndCurrentPlannedSetNumbers.SessionNumber;

            return null;
        }

        #endregion
    }
}
