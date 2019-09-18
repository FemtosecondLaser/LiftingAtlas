using System;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Guid formatter.
    /// </summary>
    public interface IGuidFormatter
    {
        /// <summary>
        /// Outputs formatted guid.
        /// </summary>
        /// <param name="guid">Guid to format.</param>
        /// <returns>Formatted guid.</returns>
        Guid FormatGuid(Guid guid);
    }
}
