using System;

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

        #region Methods

        public void PresentPlannedSetData(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber
            )
        {
            PlannedSet plannedSet = this.plannedCycleRepository.GetPlannedSet(
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
        }

        public bool PlannedSetIsCurrent(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber
            )
        {
            SessionSetNumber currentPlannedSessionAndCurrentPlannedSetNumbers =
                this.plannedCycleRepository.GetCurrentPlannedSessionAndCurrentPlannedSetNumbers(
                    plannedCycleGuid
                    );

            if (currentPlannedSessionAndCurrentPlannedSetNumbers == null)
                return false;

            return (
                (currentPlannedSessionAndCurrentPlannedSetNumbers.SessionNumber == plannedSessionNumber)
                &&
                (currentPlannedSessionAndCurrentPlannedSetNumbers.SetNumber == plannedSetNumber)
                );
        }

        public bool RepetitionsWithinPlannedRange(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber,
            Repetitions repetitions
            )
        {
            PlannedSet plannedSet = this.plannedCycleRepository.GetPlannedSet(
                plannedCycleGuid,
                plannedSessionNumber,
                plannedSetNumber
                );

            return plannedSet.RepetitionsWithinPlannedRange(repetitions);
        }

        public bool WeightWithinPlannedRange(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber,
            Weight weight
            )
        {
            PlannedSet plannedSet = this.plannedCycleRepository.GetPlannedSet(
                plannedCycleGuid,
                plannedSessionNumber,
                plannedSetNumber
                );

            return plannedSet.WeightWithinPlannedRange(weight);
        }

        public void UpdatePlannedSetLiftedValues(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber,
            LiftedValues liftedValues
            )
        {
            this.plannedCycleRepository.UpdatePlannedSetLiftedValues(
                plannedCycleGuid,
                plannedSessionNumber,
                plannedSetNumber,
                liftedValues
                );
        }

        #endregion
    }
}
