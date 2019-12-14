using System;
using System.Collections.Generic;
using System.Text;

namespace LiftingAtlas.Standard
{
    public class PlannedSetDataPresentedEventArgs : EventArgs
    {
        private readonly bool plannedSetIsCurrent;

        public PlannedSetDataPresentedEventArgs(bool plannedSetIsCurrent)
        {
            this.plannedSetIsCurrent = plannedSetIsCurrent;
        }

        public bool PlannedSetIsCurrent
        {
            get
            {
                return plannedSetIsCurrent;
            }
        }
    }
}
