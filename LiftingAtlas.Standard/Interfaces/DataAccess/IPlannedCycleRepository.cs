using System;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Planned cycle repository.
    /// </summary>
    public interface IPlannedCycleRepository
    {
        /// <summary>
        /// Plans a cycle.
        /// </summary>
        /// <param name="cycleTemplate">Cycle template used for planning.</param>
        /// <param name="lift">Lift to plan a cycle for.</param>
        /// <param name="referencePoint">Reference point used to plan a cycle.</param>
        /// <param name="quantizationProvider">Provider of qunatization method for weight planning.</param>
        void PlanCycle(
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> cycleTemplate,
            Lift lift,
            double referencePoint,
            IQuantizationProvider quantizationProvider
            );

        /// <summary>
        /// Gets latest planned cycle guid for the <paramref name="lift"/>.
        /// </summary>
        /// <param name="lift">Lift to get latest planned cycle guid for.</param>
        /// <returns>Latest planned cycle guid for the <paramref name="lift"/>.</returns>
        Guid? GetLatestPlannedCycleGuid(Lift lift);

        /// <summary>
        /// Gets planned cycle.
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle to get.</param>
        /// <returns>Planned cycle.</returns>
        PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> GetPlannedCycle(Guid plannedCycleGuid);

        /// <summary>
        /// Gets planned session.
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle containing planned session to get.</param>
        /// <param name="plannedSessionNumber">Number of planned session to get.</param>
        /// <returns>Planned session.</returns>
        PlannedSession<PlannedSet> GetPlannedSession(
            Guid plannedCycleGuid,
            int plannedSessionNumber
            );

        /// <summary>
        /// Gets planned set.
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle containing planned set to get.</param>
        /// <param name="plannedSessionNumber">Number of planned session containing planned set to get.</param>
        /// <param name="plannedSetNumber">Number of planned set to get.</param>
        /// <returns>Planned set.</returns>
        PlannedSet GetPlannedSet(
            Guid plannedCycleGuid,
            int plannedSessionNumber,
            int plannedSetNumber
            );

        /// <summary>
        /// Updates lifted values of planned set.
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle containing the set to update.</param>
        /// <param name="plannedSessionNumber">Number of planned session containing the set to update.</param>
        /// <param name="plannedSetNumber">Number of planned set to update lifted values of.</param>
        /// <param name="liftedValues">Lifted values.</param>
        void UpdatePlannedSetLiftedValues(
            Guid plannedCycleGuid,
            int plannedSessionNumber,
            int plannedSetNumber,
            (int liftedRepetitions, double liftedWeight) liftedValues
            );

        /// <summary>
        /// Gets numbers of current planned session and current planned set.
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle to return
        /// current planned session and current planned set numbers for.</param>
        /// <returns>Current planned session and current planned set numbers
        /// or null if no planned session and planned set are current.</returns>
        (int currentPlannedSessionNumber, int currentPlannedSetNumber)? GetCurrentPlannedSessionAndCurrentPlannedSetNumbers(
            Guid plannedCycleGuid
            );
    }
}
