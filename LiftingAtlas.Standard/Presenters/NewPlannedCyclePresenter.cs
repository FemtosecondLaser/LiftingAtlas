using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiftingAtlas.Standard
{
    public class NewPlannedCyclePresenter : INewPlannedCyclePresenter
    {
        #region Private fields

        private readonly INewPlannedCycleView newPlannedCycleView;
        private readonly ITemplateCycleProviderMaster templateCycleProviderMaster;
        private readonly IPlannedCycleRepository plannedCycleRepository;
        private readonly IUniformQuantizationProviderFactory uniformQuantizationProviderFactory;

        #endregion

        #region Constructors

        public NewPlannedCyclePresenter(
            INewPlannedCycleView newPlannedCycleView,
            ITemplateCycleProviderMaster templateCycleProviderMaster,
            IPlannedCycleRepository plannedCycleRepository,
            IUniformQuantizationProviderFactory uniformQuantizationProviderFactory
            )
        {
            if (newPlannedCycleView == null)
                throw new ArgumentNullException(nameof(newPlannedCycleView));

            if (templateCycleProviderMaster == null)
                throw new ArgumentNullException(nameof(templateCycleProviderMaster));

            if (plannedCycleRepository == null)
                throw new ArgumentNullException(nameof(plannedCycleRepository));

            if (uniformQuantizationProviderFactory == null)
                throw new ArgumentNullException(nameof(uniformQuantizationProviderFactory));

            this.newPlannedCycleView = newPlannedCycleView;
            this.templateCycleProviderMaster = templateCycleProviderMaster;
            this.plannedCycleRepository = plannedCycleRepository;
            this.uniformQuantizationProviderFactory = uniformQuantizationProviderFactory;
        }

        #endregion

        #region Events

        public event NamesOfTemplateCyclesPresentedEventHandler NamesOfTemplateCyclesPresented;

        #endregion

        #region Methods

        public async Task PlanNewCycleAsync(
            CycleTemplateName cycleTemplateName,
            Lift lift,
            Weight referencePoint,
            UniformQuantizationInterval uniformQuantizationInterval
            )
        {
            if (cycleTemplateName == null)
                throw new ArgumentNullException(nameof(cycleTemplateName));

            if (lift == Lift.None)
                throw new ArgumentException("Unspecified lift.", nameof(lift));

            if (referencePoint == null)
                throw new ArgumentNullException(nameof(referencePoint));

            if (uniformQuantizationInterval == null)
                throw new ArgumentNullException(nameof(uniformQuantizationInterval));

            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                await this.templateCycleProviderMaster.TemplateCycleAsync(cycleTemplateName)
                .ConfigureAwait(false);

            await this.plannedCycleRepository.PlanCycleAsync(
                templateCycle,
                lift,
                referencePoint,
                this.uniformQuantizationProviderFactory.Create(uniformQuantizationInterval)
                ).ConfigureAwait(false);
        }

        public async Task PresentNamesOfTemplateCyclesForTheLiftAsync(Lift lift)
        {
            IReadOnlyList<CycleTemplateName> namesOfTemplateCyclesForTheLift =
                await this.templateCycleProviderMaster.NamesOfTemplateCyclesForTheLiftAsync(lift);

            this.newPlannedCycleView.OutputNamesOfTemplateCycles(namesOfTemplateCyclesForTheLift);

            this.NamesOfTemplateCyclesPresented?.Invoke(
                new NamesOfTemplateCyclesPresentedEventArgs(
                    namesOfTemplateCyclesForTheLift == null ? 0 : namesOfTemplateCyclesForTheLift.Count
                    ));
        }

        #endregion
    }
}
