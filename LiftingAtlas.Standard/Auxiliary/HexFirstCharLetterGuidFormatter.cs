using System;

namespace LiftingAtlas.Standard
{
    public class HexFirstCharLetterGuidFormatter : IGuidFormatter
    {
        public Guid FormatGuid(Guid guid)
        {
            if (guid == null)
                throw new ArgumentNullException(nameof(guid));

            byte[] guidBytes = guid.ToByteArray();
            guidBytes[3] |= 240;   
            return new Guid(guidBytes);
        }
    }
}
