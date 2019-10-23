namespace LiftingAtlas.Standard
{
    /// <summary>
    /// New planned cycle presenter.
    /// </summary>
    public interface INewPlannedCyclePresenter
    {
        /// <summary>
        /// Presents names of template cycles for the <see cref="Lift"/>. 
        /// </summary>
        /// <param name="lift"><see cref="Lift"/>, to present names of template cycles for.</param>
        void PresentNamesOfTemplateCyclesForTheLift(
            Lift lift
            );

        /// <summary>
        /// Plans new cycle.
        /// </summary>
        /// <param name="cycleTemplateName">Name of cycle template used for planning.</param>
        /// <param name="lift">Lift to plan new cycle for.</param>
        /// <param name="referencePoint">Reference point used to plan new cycle.</param>
        /// <param name="uniformQuantizationInterval">Uniform quantization interval.</param>
        void PlanNewCycle(
            string cycleTemplateName,
            Lift lift,
            double referencePoint,
            UniformQuantizationInterval uniformQuantizationInterval
            );
    }
}
