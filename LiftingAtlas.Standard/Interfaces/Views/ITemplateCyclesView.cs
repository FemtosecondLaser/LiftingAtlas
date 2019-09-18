using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Template cycles view.
    /// </summary>
    public interface ITemplateCyclesView
    {
        /// <summary>
        /// Outputs names of template cycles.
        /// </summary>
        /// <param name="namesOfTemplateCycles">Names of template cycles
        /// to output.</param>
        void OutputNamesOfTemplateCycles(
            IList<string> namesOfTemplateCycles
            );
    }
}
