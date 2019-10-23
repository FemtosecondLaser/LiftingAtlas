namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Provides quantization method.
    /// </summary>
    public interface IQuantizationProvider
    {
        /// <summary>
        /// Quantizes <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value to quantize.</param>
        /// <returns>Quantized <paramref name="value"/>.</returns>
        double Quantize(double value);
    }
}
