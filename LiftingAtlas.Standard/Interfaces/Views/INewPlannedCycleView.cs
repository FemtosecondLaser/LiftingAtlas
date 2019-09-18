using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// New planned cycle view.
    /// </summary>
    public interface INewPlannedCycleView
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
