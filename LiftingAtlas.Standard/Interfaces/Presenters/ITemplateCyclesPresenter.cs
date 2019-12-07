using System.Threading.Tasks;

namespace LiftingAtlas.Standard
{
    public interface ITemplateCyclesPresenter
    {
        Task PresentNamesOfAllTemplateCyclesAsync();

        Task PresentNamesOfTemplateCyclesForTheLiftAsync(
            Lift lift
            );
    }
}
