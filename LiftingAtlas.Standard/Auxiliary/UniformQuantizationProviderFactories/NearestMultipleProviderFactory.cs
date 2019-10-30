using System;

namespace LiftingAtlas.Standard
{
    public class NearestMultipleProviderFactory : IUniformQuantizationProviderFactory
    {
        public IQuantizationProvider Create(UniformQuantizationInterval uniformQuantizationInterval)
        {
            if (uniformQuantizationInterval == null)
                throw new ArgumentNullException(nameof(uniformQuantizationInterval));

            return new NearestMultipleProvider(uniformQuantizationInterval);
        }
    }
}
