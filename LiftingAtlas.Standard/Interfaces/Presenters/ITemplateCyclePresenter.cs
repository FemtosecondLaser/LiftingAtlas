using System.Threading.Tasks;

namespace LiftingAtlas.Standard
{
    public interface ITemplateCyclePresenter
    {
        event TemplateCycleDataPresentedEventHandler TemplateCycleDataPresented;

        Task PresentTemplateCycleDataAsync(
            CycleTemplateName cycleTemplateName
            );
    }
}
