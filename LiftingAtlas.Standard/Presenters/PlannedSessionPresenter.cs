using System;
using System.Threading.Tasks;

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

        #region Events

        public event PlannedSessionDataPresentedEventHandler PlannedSessionDataPresented;

        #endregion

        #region Methods

        public async Task PresentPlannedSessionDataAsync(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber
            )
        {
            if (plannedSessionNumber == null)
                throw new ArgumentNullException(nameof(plannedSessionNumber));

            PlannedSession<PlannedSet> plannedSession =
                await this.plannedCycleRepository.GetPlannedSessionAsync(
                    plannedCycleGuid,
                    plannedSessionNumber
                    );

            this.plannedSessionView.OutputPlannedSessionSets(
                plannedSession.Sets,
                await GetCurrentPlannedSessionAndCurrentPlannedSetNumbersAsync(plannedCycleGuid)
                );

            this.PlannedSessionDataPresented?.Invoke();
        }

        private async Task<SessionSetNumber> GetCurrentPlannedSessionAndCurrentPlannedSetNumbersAsync(
            Guid plannedCycleGuid
            )
        {
            return await this.plannedCycleRepository.GetCurrentPlannedSessionAndCurrentPlannedSetNumbersAsync(
                plannedCycleGuid
                ).ConfigureAwait(false);
        }

        #endregion
    }
}
