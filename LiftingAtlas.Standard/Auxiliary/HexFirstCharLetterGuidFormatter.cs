using System;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Guid formatter. Provides a method to format guid in such way
    /// so that the first character of hexadecimal representation is a letter.
    /// </summary>
    public class HexFirstCharLetterGuidFormatter : IGuidFormatter
    {
        /// <summary>
        /// Formats guid in such way so that the first character
        /// of hexadecimal representation is a letter.
        /// </summary>
        /// <param name="guid">Guid to format. Must not be null.</param>
        /// <returns>Formatted guid.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="guid"/> is null.</exception>
        public Guid FormatGuid(Guid guid)
        {
            if (guid == null)
                throw new ArgumentNullException(nameof(guid));

            byte[] guidBytes = guid.ToByteArray();
            guidBytes[3] |= 240; /* 11110000 */
            return new Guid(guidBytes);
        }
    }
}
