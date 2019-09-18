using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Template cycles presenter.
    /// </summary>
    public class TemplateCyclesPresenter : ITemplateCyclesPresenter
    {
        #region Private fields

        /// <summary>
        /// Template cycles view.
        /// </summary>
        private readonly ITemplateCyclesView templateCyclesView;

        /// <summary>
        /// Template cycle provider Master.
        /// </summary>
        private readonly ITemplateCycleProviderMaster templateCycleProviderMaster;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates template cycles presenter.
        /// </summary>
        /// <param name="templateCyclesView">Template cycles view. Must not be null.</param>
        /// <param name="templateCycleProviderMaster">Template cycle provider master.
        /// Must not be null.</param>
        /// <exception cref="ArgumentNullException"><paramref name="templateCyclesView"/>
        /// or <paramref name="templateCycleProviderMaster"/> is null.</exception>
        public TemplateCyclesPresenter(
            ITemplateCyclesView templateCyclesView,
            ITemplateCycleProviderMaster templateCycleProviderMaster
            )
        {
            if (templateCyclesView == null)
                throw new ArgumentNullException(nameof(templateCyclesView));

            if (templateCycleProviderMaster == null)
                throw new ArgumentNullException(nameof(templateCycleProviderMaster));

            this.templateCyclesView = templateCyclesView;
            this.templateCycleProviderMaster = templateCycleProviderMaster;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Presents names of all template cycles.
        /// </summary>
        public void PresentNamesOfAllTemplateCycles()
        {
            string[] namesOfAllTemplateCycles =
                this.templateCycleProviderMaster.NamesOfAllTemplateCycles();

            this.templateCyclesView.OutputNamesOfTemplateCycles(namesOfAllTemplateCycles);
        }

        /// <summary>
        /// Presents names of template cycles for the <see cref="Lift"/>.
        /// </summary>
        /// <param name="lift"><see cref="Lift"/>, to present names of template cycles for.</param>
        public void PresentNamesOfTemplateCyclesForTheLift(Lift lift)
        {
            string[] namesOfTemplateCyclesForTheLift =
                this.templateCycleProviderMaster.NamesOfTemplateCyclesForTheLift(lift);

            this.templateCyclesView.OutputNamesOfTemplateCycles(namesOfTemplateCyclesForTheLift);
        }

        #endregion
    }
}
