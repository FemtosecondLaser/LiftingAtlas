using System;
using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Current planned cycle view.
    /// </summary>
    public interface ICurrentPlannedCycleView
    {
        /// <summary>
        /// Outputs current planned cycle template name.
        /// </summary>
        /// <param name="currentPlannedCycleTemplateName">
        /// Current planned cycle template name to output.</param>
        void OutputCurrentPlannedCycleTemplateName(
            string currentPlannedCycleTemplateName
            );

        /// <summary>
        /// Outputs current planned cycle reference point.
        /// </summary>
        /// <param name="currentPlannedCycleReferencePoint">
        /// Current planned cycle reference point to output.</param>
        void OutputCurrentPlannedCycleReferencePoint(
            string currentPlannedCycleReferencePoint
            );

        /// <summary>
        /// Outputs current planned cycle sessions.
        /// </summary>
        /// <param name="currentPlannedCycleSessions">
        /// Current planned cycle sessions to output.</param>
        void OutputCurrentPlannedCycleSessions(
            IList<PlannedSession<PlannedSet>> currentPlannedCycleSessions
            );
    }
}
