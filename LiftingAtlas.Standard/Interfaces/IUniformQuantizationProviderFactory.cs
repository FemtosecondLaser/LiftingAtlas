namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Uniform quantization provider factory.
    /// </summary>
    public interface IUniformQuantizationProviderFactory
    {
        /// <summary>
        /// Creates uniform quantization provider.
        /// </summary>
        /// <param name="uniformQuantizationInterval">Uniform quantization interval.</param>
        /// <returns>Quantization provider.</returns>
        IQuantizationProvider Create(UniformQuantizationInterval uniformQuantizationInterval);
    }
}
