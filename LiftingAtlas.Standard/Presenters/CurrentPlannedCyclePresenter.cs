using System;
using System.Threading.Tasks;

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

        #region Events

        public event CurrentPlannedCycleDataPresentedEventHandler CurrentPlannedCycleDataPresented;

        #endregion

        #region Methods

        public async Task PresentCurrentPlannedCycleDataForTheLiftAsync(Lift lift)
        {
            Guid? latestPlannedCycleForTheLiftGuid =
                await this.plannedCycleRepository.GetLatestPlannedCycleGuidAsync(lift);

            if (latestPlannedCycleForTheLiftGuid == null)
            {
                this.currentPlannedCycleView.OutputCurrentPlannedCycleTemplateName(null);
                this.currentPlannedCycleView.OutputCurrentPlannedCycleReferencePoint(null);
                this.currentPlannedCycleView.OutputCurrentPlannedCycleSessions(null, null);

                this.CurrentPlannedCycleDataPresented?.Invoke(new CurrentPlannedCycleDataPresentedEventArgs(false));

                return;
            }

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> currentPlannedCycleForTheLift =
                await this.plannedCycleRepository.GetPlannedCycleAsync(latestPlannedCycleForTheLiftGuid.Value);

            this.currentPlannedCycleView.OutputCurrentPlannedCycleTemplateName(
                currentPlannedCycleForTheLift.CycleTemplateName
                );

            this.currentPlannedCycleView.OutputCurrentPlannedCycleReferencePoint(
                currentPlannedCycleForTheLift.ReferencePoint
                );

            this.currentPlannedCycleView.OutputCurrentPlannedCycleSessions(
                currentPlannedCycleForTheLift.Sessions,
                await GetCurrentPlannedSessionNumberAsync(latestPlannedCycleForTheLiftGuid.Value)
                );

            this.CurrentPlannedCycleDataPresented?.Invoke(new CurrentPlannedCycleDataPresentedEventArgs(true));
        }

        public async Task<Guid?> GetCurrentPlannedCycleGuidAsync(Lift lift)
        {
            return await this.plannedCycleRepository.GetLatestPlannedCycleGuidAsync(lift)
                .ConfigureAwait(false);
        }

        private async Task<SessionNumber> GetCurrentPlannedSessionNumberAsync(Guid plannedCycleGuid)
        {
            SessionSetNumber currentPlannedSessionAndCurrentPlannedSetNumbers =
                await this.plannedCycleRepository.GetCurrentPlannedSessionAndCurrentPlannedSetNumbersAsync(
                    plannedCycleGuid
                    ).ConfigureAwait(false);

            if (currentPlannedSessionAndCurrentPlannedSetNumbers != null)
                return currentPlannedSessionAndCurrentPlannedSetNumbers.SessionNumber;

            return null;
        }

        #endregion
    }
}
