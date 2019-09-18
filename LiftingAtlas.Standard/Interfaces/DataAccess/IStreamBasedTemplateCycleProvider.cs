using System.IO;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Stream-based template cycle provider.
    /// </summary>
    public interface IStreamBasedTemplateCycleProvider
    {
        /// <summary>
        /// Cycle template name and lift.
        /// </summary>
        /// <param name="stream">Stream representing cycle template.</param>
        /// <returns>Cycle template name and lift.</returns>
        (string CycleTemplateName, Lift TemplateLift) CycleTemplateNameAndLift(Stream stream);

        /// <summary>
        /// Template cycle.
        /// </summary>
        /// <param name="stream">Stream representing cycle template.</param>
        /// <returns>Template cycle.</returns>
        TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> TemplateCycle(Stream stream);
    }
}
