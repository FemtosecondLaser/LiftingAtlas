using System.Threading.Tasks;

namespace LiftingAtlas.Standard
{
    public interface ITemplateCyclePresenter
    {
        Task PresentTemplateCycleDataAsync(
            CycleTemplateName cycleTemplateName
            );
    }
}
