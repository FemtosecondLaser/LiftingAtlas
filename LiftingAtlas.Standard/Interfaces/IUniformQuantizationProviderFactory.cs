namespace LiftingAtlas.Standard
{
    public interface IUniformQuantizationProviderFactory
    {
        IQuantizationProvider Create(UniformQuantizationInterval uniformQuantizationInterval);
    }
}
