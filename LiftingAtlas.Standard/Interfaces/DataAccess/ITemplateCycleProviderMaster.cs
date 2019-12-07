using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiftingAtlas.Standard
{
    public interface ITemplateCycleProviderMaster
    {
        Task<IReadOnlyList<CycleTemplateName>> NamesOfAllTemplateCyclesAsync();

        Task<IReadOnlyList<CycleTemplateName>> NamesOfTemplateCyclesForTheLiftAsync(Lift lift);

        Task<TemplateCycle<TemplateSession<TemplateSet>, TemplateSet>> TemplateCycleAsync(
            CycleTemplateName cycleTemplateName
            );
    }
}
