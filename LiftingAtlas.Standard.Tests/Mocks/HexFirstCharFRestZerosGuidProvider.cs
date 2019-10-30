using System;

namespace LiftingAtlas.Standard.Tests
{
    public class HexFirstCharFRestZerosGuidProvider : IGuidProvider
    {
        public Guid GetGuid()
        {
            return new Guid(
                new byte[16] { 0, 0, 0, 240, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                );
        }
    }
}
