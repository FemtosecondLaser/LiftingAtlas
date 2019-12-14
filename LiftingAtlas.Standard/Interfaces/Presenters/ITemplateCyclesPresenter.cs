using System.Threading.Tasks;

namespace LiftingAtlas.Standard
{
    public interface ITemplateCyclesPresenter
    {
        event NamesOfTemplateCyclesPresentedEventHandler NamesOfTemplateCyclesPresented;

        Task PresentNamesOfAllTemplateCyclesAsync();

        Task PresentNamesOfTemplateCyclesForTheLiftAsync(
            Lift lift
            );
    }
}
