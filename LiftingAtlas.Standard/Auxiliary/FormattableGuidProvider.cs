using System;
using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Guid provider. Applies optional formatting.
    /// </summary>
    public class FormattableGuidProvider : IGuidProvider
    {
        #region Private fields

        /// <summary>
        /// Guid formatters.
        /// </summary>
        private readonly ICollection<IGuidFormatter> guidFormatters;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates formattable guid provider.
        /// </summary>
        /// <param name="guidFormatters">Guid formatters.</param>
        public FormattableGuidProvider(ICollection<IGuidFormatter> guidFormatters)
        {
            this.guidFormatters = guidFormatters;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates if guid formatting is enabled.
        /// </summary>
        private bool GuidFormattingEnabled
        {
            get
            {
                return this.guidFormatters != null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Outputs guid.
        /// </summary>
        /// <returns>Guid.</returns>
        public Guid GetGuid()
        {
            Guid guid = Guid.NewGuid();

            if (GuidFormattingEnabled)
                foreach (IGuidFormatter guidFormatter in this.guidFormatters)
                    guid = guidFormatter.FormatGuid(guid);

            return guid;
        }

        #endregion
    }
}
