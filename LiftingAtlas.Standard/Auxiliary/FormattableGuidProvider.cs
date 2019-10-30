using System;
using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    public class FormattableGuidProvider : IGuidProvider
    {
        #region Private fields

        private readonly ICollection<IGuidFormatter> guidFormatters;

        #endregion

        #region Constructors

        public FormattableGuidProvider(ICollection<IGuidFormatter> guidFormatters)
        {
            this.guidFormatters = guidFormatters;
        }

        #endregion

        #region Properties

        private bool GuidFormattingEnabled
        {
            get
            {
                return this.guidFormatters != null;
            }
        }

        #endregion

        #region Methods

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
