using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task PresentNamesOfAllTemplateCyclesAsync()
        {
            IReadOnlyList<CycleTemplateName> namesOfAllTemplateCycles =
                await this.templateCycleProviderMaster.NamesOfAllTemplateCyclesAsync();

            this.templateCyclesView.OutputNamesOfTemplateCycles(namesOfAllTemplateCycles);
        }

        public async Task PresentNamesOfTemplateCyclesForTheLiftAsync(Lift lift)
        {
            IReadOnlyList<CycleTemplateName> namesOfTemplateCyclesForTheLift =
                await this.templateCycleProviderMaster.NamesOfTemplateCyclesForTheLiftAsync(lift);

            this.templateCyclesView.OutputNamesOfTemplateCycles(namesOfTemplateCyclesForTheLift);
        }

        #endregion
    }
}
