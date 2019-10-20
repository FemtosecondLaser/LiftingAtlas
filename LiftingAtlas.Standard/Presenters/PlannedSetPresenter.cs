using System;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Planned set presenter.
    /// </summary>
    public class PlannedSetPresenter : IPlannedSetPresenter
    {
        #region Private fields

        /// <summary>
        /// Planned set view.
        /// </summary>
        private readonly IPlannedSetView plannedSetView;

        /// <summary>
        /// Planned cycle repository.
        /// </summary>
        private readonly IPlannedCycleRepository plannedCycleRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates planned set presenter.
        /// </summary>
        /// <param name="plannedSetView">Planned set view.
        /// Must not be null.</param>
        /// <param name="plannedCycleRepository">Planned cycle repository.
        /// Must not be null.</param>
        /// <exception cref="ArgumentNullException"><paramref name="plannedSetView"/>
        /// or <paramref name="plannedCycleRepository"/> is null.</exception>
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

        /// <summary>
        /// Presents planned set data. 
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle,
        /// to present set data for.</param>
        /// <param name="plannedSessionNumber">
        /// Number of planned session, to present set data for.</param>
        /// <param name="plannedSetNumber">
        /// Number of planned set, to present data for.</param>
        public void PresentPlannedSetData(
            Guid plannedCycleGuid,
            int plannedSessionNumber,
            int plannedSetNumber
            )
        {
            PlannedSet plannedSet = this.plannedCycleRepository.GetPlannedSet(
                plannedCycleGuid,
                plannedSessionNumber,
                plannedSetNumber
                );

            this.plannedSetView.OutputPlannedPercentageOfReferencePoint(
                plannedSet.PlannedPercentageOfReferencePoint?.ToString()
                );

            this.plannedSetView.OutputWeightAdjustmentConstant(
                plannedSet.WeightAdjustmentConstant?.ToString("+#;-#")
                );

            this.plannedSetView.OutputPlannedWeight(
                plannedSet.PlannedWeight?.ToString()
                );

            this.plannedSetView.OutputPlannedRepetitions(
                plannedSet.PlannedRepetitions?.ToString()
                );

            this.plannedSetView.OutputLiftedValues(
                plannedSet.LiftedValues?.liftedWeight.ToString(),
                plannedSet.LiftedValues?.liftedRepetitions.ToString()
                );

            this.plannedSetView.OutputNote(
                plannedSet.Note
                );
        }

        /// <summary>
        /// Determines if planned set is current.
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle,
        /// planned set belongs to.</param>
        /// <param name="plannedSessionNumber">
        /// Number of planned session, planned set belongs to.</param>
        /// <param name="plannedSetNumber">
        /// Number of planned set to assess.</param>
        /// <returns>True if planned set is current;
        /// otherwise, false.</returns>
        public bool PlannedSetIsCurrent
            (Guid plannedCycleGuid,
            int plannedSessionNumber,
            int plannedSetNumber
            )
        {
            (int currentPlannedSessionNumber, int currentPlannedSetNumber)? currentPlannedSessionAndCurrentPlannedSetNumbers =
                this.plannedCycleRepository.GetCurrentPlannedSessionAndCurrentPlannedSetNumbers(
                    plannedCycleGuid
                    );

            if (currentPlannedSessionAndCurrentPlannedSetNumbers == null)
                return false;

            return (
                (currentPlannedSessionAndCurrentPlannedSetNumbers.Value.currentPlannedSessionNumber == plannedSessionNumber)
                &&
                (currentPlannedSessionAndCurrentPlannedSetNumbers.Value.currentPlannedSetNumber == plannedSetNumber)
                );
        }

        /// <summary>
        /// Determines if <paramref name="repetitions"/> fall within planned range.
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle,
        /// planned set belongs to.</param>
        /// <param name="plannedSessionNumber">
        /// Number of planned session, planned set belongs to.</param>
        /// <param name="plannedSetNumber">
        /// Number of planned set, planned repetition range of
        /// to use in assessment.</param>
        /// <param name="repetitions">Repetitions.</param>
        /// <returns>True if <paramref name="repetitions"/>
        /// fall within planned range;
        /// otherwise, false.</returns>
        public bool RepetitionsWithinPlannedRange(
            Guid plannedCycleGuid,
            int plannedSessionNumber,
            int plannedSetNumber,
            int repetitions
            )
        {
            PlannedSet plannedSet = this.plannedCycleRepository.GetPlannedSet(
                plannedCycleGuid,
                plannedSessionNumber,
                plannedSetNumber
                );

            return plannedSet.RepetitionsWithinPlannedRange(repetitions);
        }

        /// <summary>
        /// Determines if <paramref name="weight"/> falls within planned range.
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle,
        /// planned set belongs to.</param>
        /// <param name="plannedSessionNumber">
        /// Number of planned session, planned set belongs to.</param>
        /// <param name="plannedSetNumber">
        /// Number of planned set, planned weight range of
        /// to use in assessment.</param>
        /// <param name="weight">Weight.</param>
        /// <returns>True if <paramref name="weight"/>
        /// falls within planned range;
        /// otherwise, false.</returns>
        public bool WeightWithinPlannedRange(
            Guid plannedCycleGuid,
            int plannedSessionNumber,
            int plannedSetNumber,
            double weight
            )
        {
            PlannedSet plannedSet = this.plannedCycleRepository.GetPlannedSet(
                plannedCycleGuid,
                plannedSessionNumber,
                plannedSetNumber
                );

            return plannedSet.WeightWithinPlannedRange(weight);
        }

        /// <summary>
        /// Updates lifted values of planned set.
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle,
        /// containing planned set to update.</param>
        /// <param name="plannedSessionNumber">Number of planned session,
        /// containing planned set to update.</param>
        /// <param name="plannedSetNumber">Number of planned set,
        /// to update lifted values of.</param>
        /// <param name="liftedValues">Lifted values.</param>
        public void UpdatePlannedSetLiftedValues(
            Guid plannedCycleGuid,
            int plannedSessionNumber,
            int plannedSetNumber,
            (int liftedRepetitions, double liftedWeight) liftedValues
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
