namespace LiftingAtlas.Standard
{
    public interface INewPlannedCyclePresenter
    {
        void PresentNamesOfTemplateCyclesForTheLift(
            Lift lift
            );

        void PlanNewCycle(
            CycleTemplateName cycleTemplateName,
            Lift lift,
            Weight referencePoint,
            UniformQuantizationInterval uniformQuantizationInterval
            );
    }
}
