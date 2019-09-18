using System;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Guid provider.
    /// </summary>
    public interface IGuidProvider
    {
        /// <summary>
        /// Outputs guid.
        /// </summary>
        /// <returns>Guid.</returns>
        Guid GetGuid();
    }
}
