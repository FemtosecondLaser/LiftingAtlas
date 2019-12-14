using System.Threading.Tasks;

namespace LiftingAtlas.Standard
{
    public interface INewPlannedCyclePresenter
    {
        event NamesOfTemplateCyclesPresentedEventHandler NamesOfTemplateCyclesPresented;

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
