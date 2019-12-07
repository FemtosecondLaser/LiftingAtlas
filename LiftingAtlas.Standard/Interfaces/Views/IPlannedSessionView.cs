using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    public interface IPlannedSessionView
    {
        void OutputPlannedSessionSets(
            IReadOnlyList<PlannedSet> plannedSessionSets,
            SessionSetNumber currentPlannedSessionAndCurrentPlannedSetNumbers
            );
    }
}
