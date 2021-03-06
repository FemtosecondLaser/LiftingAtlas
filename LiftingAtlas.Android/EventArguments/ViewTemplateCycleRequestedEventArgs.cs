﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LiftingAtlas.Standard;

namespace LiftingAtlas.Droid
{
    public class ViewTemplateCycleRequestedEventArgs : EventArgs
    {
        private readonly CycleTemplateName cycleTemplateName;

        public ViewTemplateCycleRequestedEventArgs(CycleTemplateName cycleTemplateName)
        {
            if (cycleTemplateName == null)
                throw new ArgumentNullException(nameof(cycleTemplateName));

            this.cycleTemplateName = cycleTemplateName;
        }

        public CycleTemplateName CycleTemplateName
        {
            get
            {
                return cycleTemplateName;
            }
        }
    }
}