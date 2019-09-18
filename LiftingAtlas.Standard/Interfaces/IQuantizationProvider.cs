namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Provides quantization method.
    /// </summary>
    public interface IQuantizationProvider
    {
        /// <summary>
        /// Quantizes <paramref name="input"/>.
        /// </summary>
        /// <param name="input">Input to quantize.</param>
        /// <returns>Quantized <paramref name="input"/>.</returns>
        double Quantize(double input);
    }
}
