﻿using System;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// New planned cycle presenter.
    /// </summary>
    public class NewPlannedCyclePresenter : INewPlannedCyclePresenter
    {
        #region Private fields

        /// <summary>
        /// New planned cycle view.
        /// </summary>
        private readonly INewPlannedCycleView newPlannedCycleView;

        /// <summary>
        /// Template cycle provider Master.
        /// </summary>
        private readonly ITemplateCycleProviderMaster templateCycleProviderMaster;

        /// <summary>
        /// Planned cycle repository.
        /// </summary>
        private readonly IPlannedCycleRepository plannedCycleRepository;

        /// <summary>
        /// Uniform quantization provider factory.
        /// </summary>
        private readonly IUniformQuantizationProviderFactory uniformQuantizationProviderFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates new planned cycle presenter.
        /// </summary>
        /// <param name="newPlannedCycleView">New planned cycle view. Must not be null.</param>
        /// <param name="templateCycleProviderMaster">Template cycle provider master.
        /// Must not be null.</param>
        /// <param name="plannedCycleRepository">Planned cycle repository.
        /// Must not be null.</param>
        /// <param name="uniformQuantizationProviderFactory">Uniform quantization provider factory.
        /// Must not be null.</param>
        /// <exception cref="ArgumentNullException"><paramref name="newPlannedCycleView"/>,
        /// <paramref name="templateCycleProviderMaster"/>, <paramref name="plannedCycleRepository"/> or
        /// <paramref name="uniformQuantizationProviderFactory"/> is null.</exception>
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

        /// <summary>
        /// Plans new cycle.
        /// </summary>
        /// <param name="cycleTemplateName">Name of cycle template used for planning. Must not be null.</param>
        /// <param name="lift">Lift to plan new cycle for. Must be specified.</param>
        /// <param name="referencePoint">Reference point used to plan new cycle. Must not be less than 0.</param>
        /// <param name="uniformQuantizationInterval">Uniform quantization interval. Must not be null.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cycleTemplateName"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="lift"/> is unspecified.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="referencePoint"/> is less than 0.</exception>
        public void PlanNewCycle(
            string cycleTemplateName,
            Lift lift,
            double referencePoint,
            UniformQuantizationInterval uniformQuantizationInterval
            )
        {
            if (cycleTemplateName == null)
                throw new ArgumentNullException(nameof(cycleTemplateName));

            if (lift == Lift.None)
                throw new ArgumentException("Unspecified lift.", nameof(lift));

            if (referencePoint < 0.00)
                throw new ArgumentOutOfRangeException(nameof(referencePoint));

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

        /// <summary>
        /// Presents names of template cycles for the <see cref="Lift"/>. 
        /// </summary>
        /// <param name="lift"><see cref="Lift"/>, to present names of template cycles for.</param>
        public void PresentNamesOfTemplateCyclesForTheLift(Lift lift)
        {
            string[] namesOfTemplateCyclesForTheLift =
                this.templateCycleProviderMaster.NamesOfTemplateCyclesForTheLift(lift);

            this.newPlannedCycleView.OutputNamesOfTemplateCycles(namesOfTemplateCyclesForTheLift);
        }

        #endregion
    }
}
