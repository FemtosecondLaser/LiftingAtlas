namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Template cycles presenter.
    /// </summary>
    public interface ITemplateCyclesPresenter
    {
        /// <summary>
        /// Presents names of all template cycles.
        /// </summary>
        void PresentNamesOfAllTemplateCycles();

        /// <summary>
        /// Presents names of template cycles for the <see cref="Lift"/>. 
        /// </summary>
        /// <param name="lift"><see cref="Lift"/>, to present names of template cycles for.</param>
        void PresentNamesOfTemplateCyclesForTheLift(
            Lift lift
            );
    }
}
