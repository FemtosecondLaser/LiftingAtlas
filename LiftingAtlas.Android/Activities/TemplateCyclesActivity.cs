﻿using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using LiftingAtlas.Standard;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using TextInputLayout = Android.Support.Design.Widget.TextInputLayout;
using TextInputEditText = Android.Support.Design.Widget.TextInputEditText;
using Autofac;
using System.Text;
using Android.Views;
using System.Collections.Generic;
using Android.Content;

namespace LiftingAtlas.Droid
{
    [Activity(Label = "@string/cycle_templates")]
    public class TemplateCyclesActivity : AppCompatActivity, ITemplateCyclesView
    {
        private string everyLiftStringRepresentation;
        private Toolbar toolbar;
        private Spinner cycleTemplatesLiftSpinner;
        private LiftAdapter liftAdapter;
        private ListView cycleTemplatesListView;
        private TextView noCycleTemplatesFoundTextView;
        private ProgressBar cycleTemplatesProgressBar;
        private TemplateCycleAdapter templateCycleAdapter;
        private ITemplateCyclesPresenter templateCyclesPresenter;
        private ILifetimeScope lifetimeScope;
        private Dictionary<string, string> liftStringResourceResolvedLiftDictionary;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.SetContentView(Resource.Layout.activity_template_cycles);

            this.toolbar = this.FindViewById<Toolbar>(Resource.Id.toolbar);
            this.cycleTemplatesLiftSpinner = this.FindViewById<Spinner>(Resource.Id.cycle_templates_lift_spinner);
            this.cycleTemplatesListView = this.FindViewById<ListView>(Resource.Id.cycle_templates_listview);
            this.noCycleTemplatesFoundTextView = this.FindViewById<TextView>(Resource.Id.no_cycle_templates_found_textview);
            this.cycleTemplatesProgressBar = this.FindViewById<ProgressBar>(Resource.Id.cycle_templates_progressbar);

            this.SetSupportActionBar(this.toolbar);
            this.SupportActionBar.Title = this.GetString(Resource.String.cycle_templates);

            this.everyLiftStringRepresentation = this.GetString(Resource.String.every);

            string[] resolvedLiftStrings = LiftResolver.LiftStrings();
            List<string> liftSelection = new List<string>(resolvedLiftStrings.Length + 1);
            this.liftStringResourceResolvedLiftDictionary = new Dictionary<string, string>(resolvedLiftStrings.Length);
            liftSelection.Add(everyLiftStringRepresentation);

            for (int i = 0; i < resolvedLiftStrings.Length; i++)
            {
                string liftStringResourceString =
                    this.GetString(
                        LiftSpecificStringIdResolver.LiftStringId(
                            LiftResolver.StringToLift(
                                resolvedLiftStrings[i]
                                )));

                liftSelection.Add(liftStringResourceString);

                this.liftStringResourceResolvedLiftDictionary.Add(
                    liftStringResourceString,
                    resolvedLiftStrings[i]
                    );
            }

            this.liftAdapter = new LiftAdapter(this);
            this.liftAdapter.SetLifts(liftSelection);
            this.cycleTemplatesLiftSpinner.Adapter = this.liftAdapter;
            this.cycleTemplatesLiftSpinner.SetSelection(
                this.liftAdapter.GetItemPosition(everyLiftStringRepresentation)
                );

            this.templateCycleAdapter = new TemplateCycleAdapter(this);
            this.cycleTemplatesListView.Adapter = this.templateCycleAdapter;
        }

        protected override void OnResume()
        {
            base.OnResume();

            this.lifetimeScope = App.Container.BeginLifetimeScope();

            this.templateCyclesPresenter =
                this.lifetimeScope.Resolve<ITemplateCyclesPresenter>(
                    new TypedParameter(typeof(ITemplateCyclesView), this)
                    );

            this.templateCyclesPresenter.NamesOfTemplateCyclesPresented +=
                TemplateCyclesPresenter_NamesOfTemplateCyclesPresented;

            this.cycleTemplatesLiftSpinner.ItemSelected += CycleTemplatesLiftSpinner_ItemSelected;

            this.cycleTemplatesListView.ItemClick += CycleTemplatesListView_ItemClick;
        }

        protected override void OnPause()
        {
            base.OnPause();

            this.cycleTemplatesListView.ItemClick -= CycleTemplatesListView_ItemClick;

            this.cycleTemplatesLiftSpinner.ItemSelected -= CycleTemplatesLiftSpinner_ItemSelected;

            this.templateCyclesPresenter.NamesOfTemplateCyclesPresented -=
                TemplateCyclesPresenter_NamesOfTemplateCyclesPresented;

            this.lifetimeScope.Dispose();
        }

        public void OutputNamesOfTemplateCycles(IReadOnlyList<CycleTemplateName> namesOfTemplateCycles)
        {
            this.templateCycleAdapter.SetCycleTemplateNames(namesOfTemplateCycles);
        }

        private async void CycleTemplatesLiftSpinner_ItemSelected(
            object sender,
            AdapterView.ItemSelectedEventArgs e
            )
        {
            View senderView = sender as View;

            if (senderView != null)
                senderView.Enabled = false;

            try
            {
                this.cycleTemplatesListView.Visibility = ViewStates.Gone;
                this.noCycleTemplatesFoundTextView.Visibility = ViewStates.Gone;
                this.cycleTemplatesProgressBar.Visibility = ViewStates.Visible;

                string selectedLift =
                    this.liftAdapter[e.Position];

                if (selectedLift == everyLiftStringRepresentation)
                    await this.templateCyclesPresenter.PresentNamesOfAllTemplateCyclesAsync();
                else
                    await this.templateCyclesPresenter.PresentNamesOfTemplateCyclesForTheLiftAsync(
                        LiftResolver.StringToLift(this.liftStringResourceResolvedLiftDictionary[selectedLift])
                        );
            }
            finally
            {
                if (senderView != null)
                    senderView.Enabled = true;
            }
        }

        private void CycleTemplatesListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Bundle bundle = new Bundle();
            bundle.PutString(
                BundleKeys.CycleTemplateName,
                this.templateCycleAdapter[e.Position]
                );
            Intent intent = new Intent(this, typeof(TemplateCycleActivity));
            intent.PutExtras(bundle);
            base.StartActivity(intent);
        }

        private void TemplateCyclesPresenter_NamesOfTemplateCyclesPresented(
            NamesOfTemplateCyclesPresentedEventArgs e
            )
        {
            this.cycleTemplatesProgressBar.Visibility = ViewStates.Gone;

            if (e.PresentedTemplateCycleNameCount > 0)
            {
                this.noCycleTemplatesFoundTextView.Visibility = ViewStates.Gone;
                this.cycleTemplatesListView.Visibility = ViewStates.Visible;
            }
            else
            {
                this.cycleTemplatesListView.Visibility = ViewStates.Gone;
                this.noCycleTemplatesFoundTextView.Visibility = ViewStates.Visible;
            }
        }
    }
}