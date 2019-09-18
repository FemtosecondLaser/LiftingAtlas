using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace LiftingAtlas.Droid
{
    [Activity(Theme = "@style/StartupTheme", MainLauncher = true, NoHistory = true)]
    public class StartupActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Intent intent = new Intent(this, typeof(MainActivity));

            base.StartActivity(intent);

            this.Finish();
        }
    }
}