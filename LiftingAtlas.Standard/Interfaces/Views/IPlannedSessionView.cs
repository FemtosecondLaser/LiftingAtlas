using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    public interface IPlannedSessionView
    {
        void OutputPlannedSessionSets(
            IList<PlannedSet> plannedSessionSets
            );
    }
}
