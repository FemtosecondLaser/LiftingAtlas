using System;

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

        #region Methods

        public void PlanNewCycle(
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
                this.templateCycleProviderMaster.TemplateCycle(cycleTemplateName);

            this.plannedCycleRepository.PlanCycle(
                templateCycle,
                lift,
                referencePoint,
                this.uniformQuantizationProviderFactory.Create(uniformQuantizationInterval)
                );
        }

        public void PresentNamesOfTemplateCyclesForTheLift(Lift lift)
        {
            CycleTemplateName[] namesOfTemplateCyclesForTheLift =
                this.templateCycleProviderMaster.NamesOfTemplateCyclesForTheLift(lift);

            this.newPlannedCycleView.OutputNamesOfTemplateCycles(namesOfTemplateCyclesForTheLift);
        }

        #endregion
    }
}
