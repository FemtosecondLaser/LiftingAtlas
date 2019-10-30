namespace LiftingAtlas.Standard
{
    public interface ITemplateCyclesPresenter
    {
        void PresentNamesOfAllTemplateCycles();

        void PresentNamesOfTemplateCyclesForTheLift(
            Lift lift
            );
    }
}
