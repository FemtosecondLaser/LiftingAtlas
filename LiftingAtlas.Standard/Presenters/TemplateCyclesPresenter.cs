using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiftingAtlas.Standard
{
    public class TemplateCyclesPresenter : ITemplateCyclesPresenter
    {
        #region Private fields

        private readonly ITemplateCyclesView templateCyclesView;
        private readonly ITemplateCycleProviderMaster templateCycleProviderMaster;

        #endregion

        #region Constructors

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

        public void PresentNamesOfAllTemplateCycles()
        {
            CycleTemplateName[] namesOfAllTemplateCycles =
                this.templateCycleProviderMaster.NamesOfAllTemplateCycles();

            this.templateCyclesView.OutputNamesOfTemplateCycles(namesOfAllTemplateCycles);
        }

        public void PresentNamesOfTemplateCyclesForTheLift(Lift lift)
        {
            CycleTemplateName[] namesOfTemplateCyclesForTheLift =
                this.templateCycleProviderMaster.NamesOfTemplateCyclesForTheLift(lift);

            this.templateCyclesView.OutputNamesOfTemplateCycles(namesOfTemplateCyclesForTheLift);
        }

        #endregion
    }
}
