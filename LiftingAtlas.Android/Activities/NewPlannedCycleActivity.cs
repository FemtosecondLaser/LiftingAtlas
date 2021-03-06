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
using Android.Content;
using System.Collections.Generic;
using Android.Views.InputMethods;
using Android.Views;
using Android.Support.Constraints;

namespace LiftingAtlas.Droid
{
    [Activity(Label = "@string/new_planned_cycle")]
    public class NewPlannedCycleActivity : AppCompatActivity, INewPlannedCycleView
    {
        private Lift lift;
        private Toolbar toolbar;
        private ListView cycleTemplatesListView;
        private TextInputLayout referencePointTextInputLayout;
        private TextInputEditText referencePointTextInputEditText;
        private TextInputLayout uniformQuantizationIntervalTextInputLayout;
        private TextInputEditText uniformQuantizationIntervalTextInputEditText;
        private Button planNewCycleButton;
        private TextView noCycleTemplatesFoundTextView;
        private ProgressBar cycleTemplatesProgressBar;
        private Group cycleTemplatesViewGroup;
        private ChoosableTemplateCycleAdapter choosableTemplateCycleAdapter;
        private AlertDialog planNewCycleErrorAlertDialog;
        private StringBuilder planNewCycleErrorStringBuilder;
        private INewPlannedCyclePresenter newPlannedCyclePresenter;
        private ILifetimeScope lifetimeScope;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.SetContentView(Resource.Layout.activity_new_planned_cycle);

            string liftString = base.Intent.Extras.GetString(BundleKeys.Lift);
            this.lift = LiftResolver.StringToLift(liftString);

            this.toolbar = this.FindViewById<Toolbar>(Resource.Id.toolbar);
            this.cycleTemplatesListView = this.FindViewById<ListView>(Resource.Id.cycle_templates_listview);
            this.referencePointTextInputLayout = this.FindViewById<TextInputLayout>(Resource.Id.reference_point_textinputlayout);
            this.referencePointTextInputEditText = this.FindViewById<TextInputEditText>(Resource.Id.reference_point_textinputedittext);
            this.uniformQuantizationIntervalTextInputLayout = this.FindViewById<TextInputLayout>(Resource.Id.uniform_quantization_interval_textinputlayout);
            this.uniformQuantizationIntervalTextInputEditText = this.FindViewById<TextInputEditText>(Resource.Id.uniform_quantization_interval_textinputedittext);
            this.planNewCycleButton = this.FindViewById<Button>(Resource.Id.plan_new_cycle_button);
            this.noCycleTemplatesFoundTextView = this.FindViewById<TextView>(Resource.Id.no_cycle_templates_found_textview);
            this.cycleTemplatesProgressBar = this.FindViewById<ProgressBar>(Resource.Id.cycle_templates_progressbar);
            this.cycleTemplatesViewGroup = this.FindViewById<Group>(Resource.Id.cycle_templates_view_group);

            this.SetSupportActionBar(toolbar);
            this.SupportActionBar.Title = this.GetString(LiftSpecificStringIdResolver.NewLiftCycleStringId(this.lift));

            this.choosableTemplateCycleAdapter = new ChoosableTemplateCycleAdapter(this);
            this.cycleTemplatesListView.Adapter = this.choosableTemplateCycleAdapter;
        }

        protected async override void OnResume()
        {
            base.OnResume();

            this.cycleTemplatesViewGroup.Visibility = ViewStates.Gone;
            this.noCycleTemplatesFoundTextView.Visibility = ViewStates.Gone;
            this.cycleTemplatesProgressBar.Visibility = ViewStates.Visible;

            this.lifetimeScope = App.Container.BeginLifetimeScope();

            this.newPlannedCyclePresenter =
                this.lifetimeScope.Resolve<INewPlannedCyclePresenter>(
                    new TypedParameter(typeof(INewPlannedCycleView), this)
                    );

            this.newPlannedCyclePresenter.NamesOfTemplateCyclesPresented +=
                NewPlannedCyclePresenter_NamesOfTemplateCyclesPresented;

            await this.newPlannedCyclePresenter.PresentNamesOfTemplateCyclesForTheLiftAsync(this.lift);

            using (AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(this))
            {
                alertDialogBuilder.SetPositiveButton(Resource.String.ok, handler: null);
                this.planNewCycleErrorAlertDialog = alertDialogBuilder.Create();
            }

            this.planNewCycleButton.Click += PlanNewCycleButton_Click;

            this.choosableTemplateCycleAdapter.ViewTemplateCycleRequested += ChoosableTemplateCycleAdapter_ViewTemplateCycleRequested;
        }

        private async void PlanNewCycleButton_Click(object sender, EventArgs e)
        {
            View senderView = sender as View;

            if (senderView != null)
                senderView.Enabled = false;

            try
            {
                this.planNewCycleErrorStringBuilder = new StringBuilder();

                int checkedItemPosition = this.cycleTemplatesListView.CheckedItemPosition;

                if (checkedItemPosition == -1)
                    this.planNewCycleErrorStringBuilder.AppendLine(
                        this.GetString(Resource.String.select_cycle_template_dot)
                        );

                double referencePoint;

                if (!double.TryParse(this.referencePointTextInputEditText.Text, out referencePoint))
                    this.planNewCycleErrorStringBuilder.AppendLine(
                        this.GetString(Resource.String.enter_reference_point_dot)
                        );
                else
                {
                    if (double.IsNaN(referencePoint) || double.IsInfinity(referencePoint))
                        this.planNewCycleErrorStringBuilder.AppendLine(
                            this.GetString(Resource.String.reference_point_must_be_a_finite_number)
                            );

                    if (referencePoint < 0.00)
                        this.planNewCycleErrorStringBuilder.AppendLine(
                            this.GetString(Resource.String.reference_point_must_not_be_less_than_0_dot)
                            );
                }

                double uniformQuantizationInterval;

                if (!double.TryParse(this.uniformQuantizationIntervalTextInputEditText.Text, out uniformQuantizationInterval))
                    this.planNewCycleErrorStringBuilder.AppendLine(
                        this.GetString(Resource.String.enter_uniform_quantization_interval_dot)
                        );
                else
                {
                    if (double.IsNaN(uniformQuantizationInterval) || double.IsInfinity(uniformQuantizationInterval))
                        this.planNewCycleErrorStringBuilder.AppendLine(
                            this.GetString(Resource.String.uniform_quantization_interval_must_be_a_finite_number)
                            );

                    if (!(uniformQuantizationInterval > 0.00))
                        this.planNewCycleErrorStringBuilder.AppendLine(
                            this.GetString(Resource.String.uniform_quantization_interval_must_be_greater_than_0)
                            );
                }

                if (this.planNewCycleErrorStringBuilder.Length > 0)
                {
                    this.planNewCycleErrorAlertDialog.SetMessage(
                        this.planNewCycleErrorStringBuilder.ToString().TrimEnd()
                        );

                    this.planNewCycleErrorAlertDialog.Show();

                    return;
                }

                CycleTemplateName selectedCycleTemplateName = this.choosableTemplateCycleAdapter[checkedItemPosition];

                await this.newPlannedCyclePresenter.PlanNewCycleAsync(
                    selectedCycleTemplateName,
                    this.lift,
                    new Weight(referencePoint),
                    new UniformQuantizationInterval(uniformQuantizationInterval)
                    );

                this.Finish();
            }
            finally
            {
                if (senderView != null)
                    senderView.Enabled = true;
            }
        }

        private void ChoosableTemplateCycleAdapter_ViewTemplateCycleRequested(ViewTemplateCycleRequestedEventArgs e)
        {
            Bundle bundle = new Bundle();
            bundle.PutString(BundleKeys.CycleTemplateName, e.CycleTemplateName);
            Intent intent = new Intent(this, typeof(TemplateCycleActivity));
            intent.PutExtras(bundle);
            base.StartActivity(intent);
        }

        protected override void OnPause()
        {
            base.OnPause();

            this.choosableTemplateCycleAdapter.ViewTemplateCycleRequested -= ChoosableTemplateCycleAdapter_ViewTemplateCycleRequested;

            this.planNewCycleButton.Click -= PlanNewCycleButton_Click;

            this.planNewCycleErrorAlertDialog.Dispose();

            this.newPlannedCyclePresenter.NamesOfTemplateCyclesPresented -=
                NewPlannedCyclePresenter_NamesOfTemplateCyclesPresented;

            this.lifetimeScope.Dispose();
        }

        public override void Finish()
        {
            base.Finish();

            InputMethodManager inputMethodManager = (InputMethodManager)this.GetSystemService(Activity.InputMethodService);

            View currentlyFocusedView = this.CurrentFocus;

            if (currentlyFocusedView != null)
                inputMethodManager.HideSoftInputFromWindow(currentlyFocusedView.WindowToken, HideSoftInputFlags.None);
        }

        public void OutputNamesOfTemplateCycles(IReadOnlyList<CycleTemplateName> namesOfTemplateCycles)
        {
            this.choosableTemplateCycleAdapter.SetCycleTemplateNames(namesOfTemplateCycles);
        }

        private void NewPlannedCyclePresenter_NamesOfTemplateCyclesPresented(
            NamesOfTemplateCyclesPresentedEventArgs e
            )
        {
            this.cycleTemplatesProgressBar.Visibility = ViewStates.Gone;

            if (e.PresentedTemplateCycleNameCount > 0)
            {
                this.noCycleTemplatesFoundTextView.Visibility = ViewStates.Gone;
                this.cycleTemplatesViewGroup.Visibility = ViewStates.Visible;
            }
            else
            {
                this.cycleTemplatesViewGroup.Visibility = ViewStates.Gone;
                this.noCycleTemplatesFoundTextView.Visibility = ViewStates.Visible;
            }
        }
    }
}