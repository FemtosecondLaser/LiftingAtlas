using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    public interface INewPlannedCycleView
    {
        void OutputNamesOfTemplateCycles(
            IReadOnlyList<CycleTemplateName> namesOfTemplateCycles
            );
    }
}
