using System;
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

namespace LiftingAtlas.Droid
{
    [Activity(Label = "@string/new_planned_cycle")]
    public class NewPlannedCycleActivity : AppCompatActivity, INewPlannedCycleView
    {
        private Lift lift;
        private Toolbar toolbar;
        private ListView cycleTemplatesListView;
        private ChoosableTemplateCycleAdapter choosableTemplateCycleAdapter;
        private TextInputLayout referencePointTextInputLayout;
        private TextInputEditText referencePointTextInputEditText;
        private Button planNewCycleButton;
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
            this.planNewCycleButton = this.FindViewById<Button>(Resource.Id.plan_new_cycle_button);

            this.SetSupportActionBar(toolbar);
            this.SupportActionBar.Title = this.GetString(LiftSpecificStringIdResolver.NewLiftCycleStringId(this.lift));

            this.choosableTemplateCycleAdapter = new ChoosableTemplateCycleAdapter(this);
            this.cycleTemplatesListView.Adapter = this.choosableTemplateCycleAdapter;
        }

        protected override void OnResume()
        {
            base.OnResume();

            this.lifetimeScope = App.Container.BeginLifetimeScope();

            this.newPlannedCyclePresenter =
                this.lifetimeScope.Resolve<INewPlannedCyclePresenter>(
                    new TypedParameter(typeof(INewPlannedCycleView), this)
                    );

            this.newPlannedCyclePresenter.PresentNamesOfTemplateCyclesForTheLift(this.lift);

            using (AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(this))
            {
                alertDialogBuilder.SetPositiveButton(Resource.String.ok, handler: null);
                this.planNewCycleErrorAlertDialog = alertDialogBuilder.Create();
            }

            this.planNewCycleButton.Click += PlanNewCycleButton_Click;

            this.choosableTemplateCycleAdapter.ViewTemplateCycleRequested += ChoosableTemplateCycleAdapter_ViewTemplateCycleRequested;
        }

        private void PlanNewCycleButton_Click(object sender, EventArgs e)
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

            if (referencePoint < 0.00)
                this.planNewCycleErrorStringBuilder.AppendLine(
                    this.GetString(Resource.String.reference_point_must_not_be_less_than_0_dot)
                    );

            if (this.planNewCycleErrorStringBuilder.Length > 0)
            {
                this.planNewCycleErrorAlertDialog.SetMessage(
                    this.planNewCycleErrorStringBuilder.ToString().TrimEnd()
                    );

                this.planNewCycleErrorAlertDialog.Show();

                return;
            }

            string selectedCycleTemplateName = this.choosableTemplateCycleAdapter[checkedItemPosition];

            this.newPlannedCyclePresenter.PlanNewCycle(
                selectedCycleTemplateName,
                this.lift,
                referencePoint
                );

            this.Finish();
        }

        private void ChoosableTemplateCycleAdapter_ViewTemplateCycleRequested(
            object sender,
            ViewTemplateCycleRequestedEventArgs e
            )
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

        public void OutputNamesOfTemplateCycles(IList<string> namesOfTemplateCycles)
        {
            this.choosableTemplateCycleAdapter.SetCycleTemplateNames(namesOfTemplateCycles);
        }
    }
}