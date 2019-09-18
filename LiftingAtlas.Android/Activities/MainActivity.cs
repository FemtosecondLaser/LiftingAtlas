using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Android.Content;
using LiftingAtlas.Standard;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using System.Collections.Generic;

namespace LiftingAtlas.Droid
{
    [Activity(Theme = "@style/MainViewTheme")]
    public class MainActivity : AppCompatActivity
    {
        private Toolbar toolbar;
        private Button currentCycleButton;
        private Button cycleTemplatesButton;
        private AlertDialog currentCycleLiftSelection;
        private Dictionary<string, string> liftStringResourceResolvedLiftDictionary;
        private string[] liftStringResourceStrings;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.SetContentView(Resource.Layout.activity_main);

            this.toolbar = this.FindViewById<Toolbar>(Resource.Id.toolbar);
            this.currentCycleButton = this.FindViewById<Button>(Resource.Id.current_cycle_button);
            this.cycleTemplatesButton = this.FindViewById<Button>(Resource.Id.cycle_templates_button);

            this.SetSupportActionBar(this.toolbar);

            string[] resolvedLiftStrings = LiftResolver.LiftStrings();
            this.liftStringResourceResolvedLiftDictionary = new Dictionary<string, string>(resolvedLiftStrings.Length);
            this.liftStringResourceStrings = new string[resolvedLiftStrings.Length];
            for (int i = 0; i < resolvedLiftStrings.Length; i++)
            {
                this.liftStringResourceStrings[i] =
                    this.GetString(
                        LiftSpecificStringIdResolver.LiftStringId(
                            LiftResolver.StringToLift(
                                resolvedLiftStrings[i]
                                )));

                this.liftStringResourceResolvedLiftDictionary.Add(
                    this.liftStringResourceStrings[i],
                    resolvedLiftStrings[i]
                    );
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            this.currentCycleButton.Click += CurrentCycleButton_Click;

            this.cycleTemplatesButton.Click += CycleTemplatesButton_Click;

            using (AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(this))
            {
                alertDialogBuilder.SetTitle(Resource.String.current_cycle);
                alertDialogBuilder.SetItems(this.liftStringResourceStrings, Lift_Click);
                this.currentCycleLiftSelection = alertDialogBuilder.Create();
            }
        }

        private void CurrentCycleButton_Click(object sender, EventArgs e)
        {
            this.currentCycleLiftSelection.Show();
        }
        
        private void Lift_Click(object sender, DialogClickEventArgs e)
        {            
            Bundle bundle = new Bundle(1);
            bundle.PutString(
                BundleKeys.Lift,
                this.liftStringResourceResolvedLiftDictionary[this.liftStringResourceStrings[e.Which]]
                );
            Intent intent = new Intent(this, typeof(CurrentPlannedCycleActivity));
            intent.PutExtras(bundle);
            base.StartActivity(intent);
        }

        private void CycleTemplatesButton_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(TemplateCyclesActivity));
            base.StartActivity(intent);
        }

        protected override void OnPause()
        {
            base.OnPause();

            this.currentCycleButton.Click -= CurrentCycleButton_Click;

            this.cycleTemplatesButton.Click -= CycleTemplatesButton_Click;

            this.currentCycleLiftSelection.Dispose();
        }
    }
}