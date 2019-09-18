namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Template cycle presenter.
    /// </summary>
    public interface ITemplateCyclePresenter
    {
        /// <summary>
        /// Presents template cycle data.
        /// </summary>
        /// <param name="cycleTemplateName">Cycle template name,
        /// to present template cycle data for.</param>
        void PresentTemplateCycleData(
            string cycleTemplateName
            );
    }
}
