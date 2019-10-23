using System;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Nearest multiple provider factory.
    /// </summary>
    public class NearestMultipleProviderFactory : IUniformQuantizationProviderFactory
    {
        /// <summary>
        /// Creates uniform quantization provider.
        /// </summary>
        /// <param name="uniformQuantizationInterval">Uniform quantization interval. Must not be null.</param>
        /// <exception cref="ArgumentNullException"><paramref name="uniformQuantizationInterval"/> is null.</exception>
        /// <returns>Quantization provider.</returns>
        public IQuantizationProvider Create(UniformQuantizationInterval uniformQuantizationInterval)
        {
            if (uniformQuantizationInterval == null)
                throw new ArgumentNullException(nameof(uniformQuantizationInterval));

            return new NearestMultipleProvider(uniformQuantizationInterval);
        }
    }
}
