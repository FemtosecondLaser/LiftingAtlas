using System;

namespace LiftingAtlas.Standard.Tests
{
    /// <summary>
    /// Guid provider. Provides guid, hex representation of which is: f0000000000000000000000000000000.
    /// </summary>
    public class HexFirstCharFRestZerosGuidProvider : IGuidProvider
    {
        /// <summary>
        /// Outputs guid, hex representation of which is: f0000000000000000000000000000000.
        /// </summary>
        /// <returns>Guid.</returns>
        public Guid GetGuid()
        {
            return new Guid(
                new byte[16] { 0, 0, 0, 240, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                );
        }
    }
}
