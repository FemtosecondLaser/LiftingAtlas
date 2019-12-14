using System;
using System.Threading.Tasks;

namespace LiftingAtlas.Standard
{
    public class PlannedSetPresenter : IPlannedSetPresenter
    {
        #region Private fields

        private readonly IPlannedSetView plannedSetView;
        private readonly IPlannedCycleRepository plannedCycleRepository;

        #endregion

        #region Constructors

        public PlannedSetPresenter(
            IPlannedSetView plannedSetView,
            IPlannedCycleRepository plannedCycleRepository
            )
        {
            if (plannedSetView == null)
                throw new ArgumentNullException(nameof(plannedSetView));

            if (plannedCycleRepository == null)
                throw new ArgumentNullException(nameof(plannedCycleRepository));

            this.plannedSetView = plannedSetView;
            this.plannedCycleRepository = plannedCycleRepository;
        }

        #endregion

        #region Events

        public event PlannedSetDataPresentedEventHandler PlannedSetDataPresented;

        #endregion

        #region Methods

        public async Task PresentPlannedSetDataAsync(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber
            )
        {
            PlannedSet plannedSet =
                await this.plannedCycleRepository.GetPlannedSetAsync(
                    plannedCycleGuid,
                    plannedSessionNumber,
                    plannedSetNumber
                    );

            this.plannedSetView.OutputPlannedPercentageOfReferencePoint(
                plannedSet.PlannedPercentageOfReferencePoint
                );

            this.plannedSetView.OutputWeightAdjustmentConstant(
                plannedSet.WeightAdjustmentConstant
                );

            this.plannedSetView.OutputPlannedWeight(
                plannedSet.PlannedWeight
                );

            this.plannedSetView.OutputPlannedRepetitions(
                plannedSet.PlannedRepetitions
                );

            this.plannedSetView.OutputLiftedValues(
                plannedSet.LiftedValues
                );

            this.plannedSetView.OutputNote(
                plannedSet.Note
                );

            this.PlannedSetDataPresented?.Invoke(
                new PlannedSetDataPresentedEventArgs(
                    await PlannedSetIsCurrentAsync(
                        plannedCycleGuid,
                        plannedSessionNumber,
                        plannedSetNumber
                        )));
        }

        private async Task<bool> PlannedSetIsCurrentAsync(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber
            )
        {
            SessionSetNumber currentPlannedSessionAndCurrentPlannedSetNumbers =
                await this.plannedCycleRepository.GetCurrentPlannedSessionAndCurrentPlannedSetNumbersAsync(
                    plannedCycleGuid
                    ).ConfigureAwait(false);

            if (currentPlannedSessionAndCurrentPlannedSetNumbers == null)
                return false;

            return (
                (currentPlannedSessionAndCurrentPlannedSetNumbers.SessionNumber == plannedSessionNumber)
                &&
                (currentPlannedSessionAndCurrentPlannedSetNumbers.SetNumber == plannedSetNumber)
                );
        }

        public async Task<bool> RepetitionsWithinPlannedRangeAsync(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber,
            Repetitions repetitions
            )
        {
            PlannedSet plannedSet =
                await this.plannedCycleRepository.GetPlannedSetAsync(
                    plannedCycleGuid,
                    plannedSessionNumber,
                    plannedSetNumber
                    ).ConfigureAwait(false);

            return plannedSet.RepetitionsWithinPlannedRange(repetitions);
        }

        public async Task<bool> WeightWithinPlannedRangeAsync(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber,
            Weight weight
            )
        {
            PlannedSet plannedSet =
                await this.plannedCycleRepository.GetPlannedSetAsync(
                    plannedCycleGuid,
                    plannedSessionNumber,
                    plannedSetNumber
                    ).ConfigureAwait(false);

            return plannedSet.WeightWithinPlannedRange(weight);
        }

        public async Task UpdatePlannedSetLiftedValuesAsync(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber,
            LiftedValues liftedValues
            )
        {
            await this.plannedCycleRepository.UpdatePlannedSetLiftedValuesAsync(
                plannedCycleGuid,
                plannedSessionNumber,
                plannedSetNumber,
                liftedValues
                ).ConfigureAwait(false);
        }

        #endregion
    }
}
