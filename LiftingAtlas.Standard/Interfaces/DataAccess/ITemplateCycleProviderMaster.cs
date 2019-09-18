namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Template cycle provider Master.
    /// </summary>
    public interface ITemplateCycleProviderMaster
    {
        /// <summary>
        /// Returns names of all template cycles.
        /// </summary>
        /// <returns>Names of all template cycles.</returns>
        string[] NamesOfAllTemplateCycles();

        /// <summary>
        /// Returns names of template cycles for the <see cref="Lift"/>. 
        /// </summary>
        /// <param name="lift"><see cref="Lift"/>, to return names of template cycles for.</param>
        /// <returns>Names of template cycles for the <see cref="Lift"/>.</returns>
        string[] NamesOfTemplateCyclesForTheLift(Lift lift);

        /// <summary>
        /// Returns template cycle.
        /// </summary>
        /// <param name="cycleTemplateName">Name of template cycle to return.</param>
        /// <returns>Template cycle.</returns>
        TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> TemplateCycle(string cycleTemplateName);
    }
}
