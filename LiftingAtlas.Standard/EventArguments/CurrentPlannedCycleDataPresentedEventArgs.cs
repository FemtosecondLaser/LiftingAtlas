using System;
using System.Collections.Generic;
using System.Text;

namespace LiftingAtlas.Standard
{
    public class CurrentPlannedCycleDataPresentedEventArgs : EventArgs
    {
        private readonly bool currentPlannedCycleExists;

        public CurrentPlannedCycleDataPresentedEventArgs(bool currentPlannedCycleExists)
        {
            this.currentPlannedCycleExists = currentPlannedCycleExists;
        }

        public bool CurrentPlannedCycleExists
        {
            get
            {
                return currentPlannedCycleExists;
            }
        }
    }
}
