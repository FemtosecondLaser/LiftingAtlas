using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    public interface ITemplateCyclesView
    {
        void OutputNamesOfTemplateCycles(
            IReadOnlyList<CycleTemplateName> namesOfTemplateCycles
            );
    }
}
