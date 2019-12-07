using System;
using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    public interface ICurrentPlannedCycleView
    {
        void OutputCurrentPlannedCycleTemplateName(
            CycleTemplateName currentPlannedCycleTemplateName
            );

        void OutputCurrentPlannedCycleReferencePoint(
            Weight currentPlannedCycleReferencePoint
            );

        void OutputCurrentPlannedCycleSessions(
            IReadOnlyList<PlannedSession<PlannedSet>> currentPlannedCycleSessions,
            SessionNumber currentPlannedSessionNumber
            );
    }
}
