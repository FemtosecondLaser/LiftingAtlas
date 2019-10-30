namespace LiftingAtlas.Standard
{
    public interface ITemplateCycleProviderMaster
    {
        CycleTemplateName[] NamesOfAllTemplateCycles();

        CycleTemplateName[] NamesOfTemplateCyclesForTheLift(Lift lift);

        TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> TemplateCycle(CycleTemplateName cycleTemplateName);
    }
}
