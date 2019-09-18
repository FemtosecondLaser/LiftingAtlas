using System;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Planned set presenter.
    /// </summary>
    public interface IPlannedSetPresenter
    {
        /// <summary>
        /// Presents planned set data. 
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle,
        /// to present set data for.</param>
        /// <param name="plannedSessionNumber">
        /// Number of planned session, to present set data for.</param>
        /// <param name="plannedSetNumber">
        /// Number of planned set, to present data for.</param>
        void PresentPlannedSetData(
            Guid plannedCycleGuid,
            int plannedSessionNumber,
            int plannedSetNumber
            );

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
        bool PlannedSetIsCurrent(
            Guid plannedCycleGuid,
            int plannedSessionNumber,
            int plannedSetNumber
            );

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
        bool RepetitionsWithinPlannedRange(
            Guid plannedCycleGuid,
            int plannedSessionNumber,
            int plannedSetNumber,
            int repetitions
            );

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
        bool WeightWithinPlannedRange(
            Guid plannedCycleGuid,
            int plannedSessionNumber,
            int plannedSetNumber,
            double weight
            );

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
        void UpdatePlannedSetLiftedValues(
            Guid plannedCycleGuid,
            int plannedSessionNumber,
            int plannedSetNumber,
            (int liftedRepetitions, double liftedWeight) liftedValues
            );
    }
}
