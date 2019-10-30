using System.IO;

namespace LiftingAtlas.Standard
{
    public interface IStreamBasedTemplateCycleProvider
    {
        (CycleTemplateName CycleTemplateName, Lift TemplateLift) CycleTemplateNameAndLift(Stream stream);

        TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> TemplateCycle(Stream stream);
    }
}
