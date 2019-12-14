using System;
using System.Collections.Generic;
using System.Text;

namespace LiftingAtlas.Standard
{
    public class NamesOfTemplateCyclesPresentedEventArgs : EventArgs
    {
        private readonly int presentedTemplateCycleNameCount;

        public NamesOfTemplateCyclesPresentedEventArgs(int presentedTemplateCycleNameCount)
        {
            if (presentedTemplateCycleNameCount < 0)
                throw new ArgumentOutOfRangeException(nameof(presentedTemplateCycleNameCount));

            this.presentedTemplateCycleNameCount = presentedTemplateCycleNameCount;
        }

        public int PresentedTemplateCycleNameCount
        {
            get
            {
                return presentedTemplateCycleNameCount;
            }
        }
    }
}
