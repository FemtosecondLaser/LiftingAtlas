using System.IO;
using System.Threading.Tasks;

namespace LiftingAtlas.Standard
{
    public interface IStreamBasedTemplateCycleProvider
    {
        Task<(CycleTemplateName CycleTemplateName, Lift TemplateLift)> CycleTemplateNameAndLiftAsync(Stream stream);

        Task<TemplateCycle<TemplateSession<TemplateSet>, TemplateSet>> TemplateCycleAsync(Stream stream);
    }
}
