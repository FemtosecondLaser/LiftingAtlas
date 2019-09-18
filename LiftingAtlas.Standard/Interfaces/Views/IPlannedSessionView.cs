using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Planned session view.
    /// </summary>
    public interface IPlannedSessionView
    {
        /// <summary>
        /// Outputs planned session sets.
        /// </summary>
        /// <param name="plannedSessionSets">
        /// Planned session sets to output.</param>
        void OutputPlannedSessionSets(
            IList<PlannedSet> plannedSessionSets
            );
    }
}
