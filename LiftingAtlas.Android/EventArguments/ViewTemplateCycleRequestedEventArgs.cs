using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace LiftingAtlas.Droid
{
    public class ViewTemplateCycleRequestedEventArgs : EventArgs
    {
        private readonly string cycleTemplateName;

        public ViewTemplateCycleRequestedEventArgs(string cycleTemplateName)
        {
            this.cycleTemplateName = cycleTemplateName;
        }

        public virtual string CycleTemplateName
        {
            get
            {
                return this.cycleTemplateName;
            }
        }
    }
}