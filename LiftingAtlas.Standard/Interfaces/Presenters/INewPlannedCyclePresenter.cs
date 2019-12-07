using System.Threading.Tasks;

namespace LiftingAtlas.Standard
{
    public interface INewPlannedCyclePresenter
    {
        Task PresentNamesOfTemplateCyclesForTheLiftAsync(
            Lift lift
            );

        Task PlanNewCycleAsync(
            CycleTemplateName cycleTemplateName,
            Lift lift,
            Weight referencePoint,
            UniformQuantizationInterval uniformQuantizationInterval
            );
    }
}
