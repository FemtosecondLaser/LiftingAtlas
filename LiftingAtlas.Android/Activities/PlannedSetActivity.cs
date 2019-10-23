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
using Autofac;
using LiftingAtlas.Standard;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using TextInputLayout = Android.Support.Design.Widget.TextInputLayout;
using TextInputEditText = Android.Support.Design.Widget.TextInputEditText;
using Android.Support.Constraints;
using Android.Views.InputMethods;

namespace LiftingAtlas.Droid
{
    [Activity(Label = "@string/planned_set")]
    public class PlannedSetActivity : AppCompatActivity, IPlannedSetView
    {
        private Lift lift;
        private Guid plannedCycleGuid;
        private string plannedCycleTemplateName;
        private string plannedCycleReferencePoint;
        private int plannedSessionNumber;
        private int plannedSetNumber;
        private Toolbar toolbar;
        private ScrollView setInformationScrollView;
        private TextView cycleTemplateNameLabelTextView;
        private TextView cycleTemplateNameTextView;
        private TextView referencePointLabelTextView;
        private TextView referencePointTextView;
        private TextView sessionNumberLabelTextView;
        private TextView sessionNumberTextView;
        private TextView setNumberLabelTextView;
        private TextView setNumberTextView;
        private TextView plannedPercentageOfReferencePointLabelTextView;
        private TextView plannedPercentageOfReferencePointTextView;
        private TextView weightAdjustmentConstantLabelTextView;
        private TextView weightAdjustmentConstantTextView;
        private TextView plannedWeightLabelTextView;
        private TextView plannedWeightTextView;
        private TextView plannedRepetitionsLabelTextView;
        private TextView plannedRepetitionsTextView;
        private TextView liftedWeightLabelTextView;
        private TextView liftedWeightTextView;
        private TextView liftedRepetitionsLabelTextView;
        private TextView liftedRepetitionsTextView;
        private TextView noteLabelTextView;
        private TextView noteTextView;
        private TextInputLayout liftedWeightTextInputLayout;
        private TextInputEditText liftedWeightTextInputEditText;
        private TextInputLayout liftedRepetitionsTextInputLayout;
        private TextInputEditText liftedRepetitionsTextInputEditText;
        private Button registerLiftedValuesButton;
        private AlertDialog registerLiftedValuesErrorAlertDialog;
        private StringBuilder registerLiftedValuesErrorStringBuilder;
        private AlertDialog registerLiftedValuesWarningAlertDialog;
        private StringBuilder registerLiftedValuesWarningStringBuilder;
        private IPlannedSetPresenter plannedSetPresenter;
        private ILifetimeScope lifetimeScope;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.SetContentView(Resource.Layout.activity_planned_set);

            string liftString = base.Intent.Extras.GetString(BundleKeys.Lift);
            this.lift = LiftResolver.StringToLift(liftString);
            this.plannedCycleGuid = Guid.Parse(base.Intent.Extras.GetString(BundleKeys.PlannedCycleGuid));
            this.plannedCycleTemplateName = base.Intent.Extras.GetString(BundleKeys.PlannedCycleTemplateName);
            this.plannedCycleReferencePoint = base.Intent.Extras.GetString(BundleKeys.PlannedCycleReferencePoint);
            this.plannedSessionNumber = base.Intent.Extras.GetInt(BundleKeys.SessionNumber);
            this.plannedSetNumber = base.Intent.Extras.GetInt(BundleKeys.SetNumber);

            this.toolbar = this.FindViewById<Toolbar>(Resource.Id.toolbar);
            this.setInformationScrollView = this.FindViewById<ScrollView>(Resource.Id.set_information_scrollview);
            this.cycleTemplateNameLabelTextView = this.FindViewById<TextView>(Resource.Id.cycle_template_name_label_textview);
            this.cycleTemplateNameTextView = this.FindViewById<TextView>(Resource.Id.cycle_template_name_textview);
            this.referencePointLabelTextView = this.FindViewById<TextView>(Resource.Id.reference_point_label_textview);
            this.referencePointTextView = this.FindViewById<TextView>(Resource.Id.reference_point_textview);
            this.sessionNumberLabelTextView = this.FindViewById<TextView>(Resource.Id.session_number_label_textview);
            this.sessionNumberTextView = this.FindViewById<TextView>(Resource.Id.session_number_textview);
            this.setNumberLabelTextView = this.FindViewById<TextView>(Resource.Id.set_number_label_textview);
            this.setNumberTextView = this.FindViewById<TextView>(Resource.Id.set_number_textview);
            this.plannedPercentageOfReferencePointLabelTextView = this.FindViewById<TextView>(Resource.Id.planned_percentage_of_reference_point_label_textview);
            this.plannedPercentageOfReferencePointTextView = this.FindViewById<TextView>(Resource.Id.planned_percentage_of_reference_point_textview);
            this.weightAdjustmentConstantLabelTextView = this.FindViewById<TextView>(Resource.Id.weight_adjustment_constant_label_textview);
            this.weightAdjustmentConstantTextView = this.FindViewById<TextView>(Resource.Id.weight_adjustment_constant_textview);
            this.plannedWeightLabelTextView = this.FindViewById<TextView>(Resource.Id.planned_weight_label_textview);
            this.plannedWeightTextView = this.FindViewById<TextView>(Resource.Id.planned_weight_textview);
            this.plannedRepetitionsLabelTextView = this.FindViewById<TextView>(Resource.Id.planned_repetitions_label_textview);
            this.plannedRepetitionsTextView = this.FindViewById<TextView>(Resource.Id.planned_repetitions_textview);
            this.liftedWeightLabelTextView = this.FindViewById<TextView>(Resource.Id.lifted_weight_label_textview);
            this.liftedWeightTextView = this.FindViewById<TextView>(Resource.Id.lifted_weight_textview);
            this.liftedRepetitionsLabelTextView = this.FindViewById<TextView>(Resource.Id.lifted_repetitions_label_textview);
            this.liftedRepetitionsTextView = this.FindViewById<TextView>(Resource.Id.lifted_repetitions_textview);
            this.noteLabelTextView = this.FindViewById<TextView>(Resource.Id.note_label_textview);
            this.noteTextView = this.FindViewById<TextView>(Resource.Id.note_textview);
            this.liftedWeightTextInputLayout= this.FindViewById<TextInputLayout>(Resource.Id.lifted_weight_textinputlayout);
            this.liftedWeightTextInputEditText = this.FindViewById<TextInputEditText>(Resource.Id.lifted_weight_textinputedittext);
            this.liftedRepetitionsTextInputLayout = this.FindViewById<TextInputLayout>(Resource.Id.lifted_repetitions_textinputlayout);
            this.liftedRepetitionsTextInputEditText = this.FindViewById<TextInputEditText>(Resource.Id.lifted_repetitions_textinputedittext);
            this.registerLiftedValuesButton = this.FindViewById<Button>(Resource.Id.register_lifted_values_button);

            this.SetSupportActionBar(this.toolbar);
            this.SupportActionBar.Title = this.GetString(LiftSpecificStringIdResolver.PlannedLiftSetStringId(this.lift));

            this.cycleTemplateNameTextView.Text = this.plannedCycleTemplateName;
            this.referencePointTextView.Text = this.plannedCycleReferencePoint;
            this.sessionNumberTextView.Text = this.plannedSessionNumber.ToString();
            this.setNumberTextView.Text = this.plannedSetNumber.ToString();
        }

        protected override void OnResume()
        {
            base.OnResume();

            this.lifetimeScope = App.Container.BeginLifetimeScope();

            this.plannedSetPresenter =
                this.lifetimeScope.Resolve<IPlannedSetPresenter>(
                    new TypedParameter(typeof(IPlannedSetView), this)
                    );

            this.plannedSetPresenter.PresentPlannedSetData(
                this.plannedCycleGuid,
                this.plannedSessionNumber,
                this.plannedSetNumber
                );

            if (!this.plannedSetPresenter.PlannedSetIsCurrent(
                this.plannedCycleGuid,
                this.plannedSessionNumber,
                this.plannedSetNumber
                ))
            {
                this.liftedWeightTextInputLayout.Visibility = ViewStates.Gone;
                this.liftedRepetitionsTextInputLayout.Visibility = ViewStates.Gone;
                this.registerLiftedValuesButton.Visibility = ViewStates.Gone;
            }

            using (AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(this))
            {
                alertDialogBuilder.SetPositiveButton(Resource.String.ok, handler: null);
                this.registerLiftedValuesErrorAlertDialog = alertDialogBuilder.Create();
            }

            using (AlertDialog.Builder alertDialogBuilder = new AlertDialog.Builder(this))
            {
                alertDialogBuilder.SetPositiveButton(Resource.String.yes, handler: RegisterLiftedValuesWarningAlertDialogYes);
                alertDialogBuilder.SetNegativeButton(Resource.String.no, handler: null);
                this.registerLiftedValuesWarningAlertDialog = alertDialogBuilder.Create();
            }

            this.registerLiftedValuesButton.Click += RegisterLiftedValuesButton_Click;
            this.liftedWeightTextInputEditText.FocusChange += LiftedWeightTextInputEditText_FocusChange;
            this.liftedRepetitionsTextInputEditText.FocusChange += LiftedRepetitionsTextInputEditText_FocusChange;
            this.setInformationScrollView.LayoutChange += SetInformationScrollView_LayoutChange;
        }

        private void LiftedWeightTextInputEditText_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                this.setInformationScrollView.SmoothScrollTo(
                    this.plannedWeightTextView.Left,
                    this.plannedWeightTextView.Bottom
                    );

                this.cycleTemplateNameLabelTextView.Enabled = false;
                this.cycleTemplateNameTextView.Enabled = false;
                this.referencePointLabelTextView.Enabled = false;
                this.referencePointTextView.Enabled = false;
                this.sessionNumberLabelTextView.Enabled = false;
                this.sessionNumberTextView.Enabled = false;
                this.setNumberLabelTextView.Enabled = false;
                this.setNumberTextView.Enabled = false;
                this.plannedPercentageOfReferencePointLabelTextView.Enabled = false;
                this.plannedPercentageOfReferencePointTextView.Enabled = false;
                this.weightAdjustmentConstantLabelTextView.Enabled = false;
                this.weightAdjustmentConstantTextView.Enabled = false;
                this.plannedWeightLabelTextView.Enabled = true;
                this.plannedWeightTextView.Enabled = true;
                this.plannedRepetitionsLabelTextView.Enabled = false;
                this.plannedRepetitionsTextView.Enabled = false;
                this.liftedWeightLabelTextView.Enabled = false;
                this.liftedWeightTextView.Enabled = false;
                this.liftedRepetitionsLabelTextView.Enabled = false;
                this.liftedRepetitionsTextView.Enabled = false;
                this.noteLabelTextView.Enabled = false;
                this.noteTextView.Enabled = false;

                return;
            }

            if (!e.HasFocus)
            {
                EnableSetInformationScrollViewTextViews();

                return;
            }
        }
        private void LiftedRepetitionsTextInputEditText_FocusChange(object sender, View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                this.setInformationScrollView.SmoothScrollTo(
                    this.plannedRepetitionsTextView.Left,
                    this.plannedRepetitionsTextView.Bottom
                    );

                this.cycleTemplateNameLabelTextView.Enabled = false;
                this.cycleTemplateNameTextView.Enabled = false;
                this.referencePointLabelTextView.Enabled = false;
                this.referencePointTextView.Enabled = false;
                this.sessionNumberLabelTextView.Enabled = false;
                this.sessionNumberTextView.Enabled = false;
                this.setNumberLabelTextView.Enabled = false;
                this.setNumberTextView.Enabled = false;
                this.plannedPercentageOfReferencePointLabelTextView.Enabled = false;
                this.plannedPercentageOfReferencePointTextView.Enabled = false;
                this.weightAdjustmentConstantLabelTextView.Enabled = false;
                this.weightAdjustmentConstantTextView.Enabled = false;
                this.plannedWeightLabelTextView.Enabled = false;
                this.plannedWeightTextView.Enabled = false;
                this.plannedRepetitionsLabelTextView.Enabled = true;
                this.plannedRepetitionsTextView.Enabled = true;
                this.liftedWeightLabelTextView.Enabled = false;
                this.liftedWeightTextView.Enabled = false;
                this.liftedRepetitionsLabelTextView.Enabled = false;
                this.liftedRepetitionsTextView.Enabled = false;
                this.noteLabelTextView.Enabled = false;
                this.noteTextView.Enabled = false;

                return;
            }

            if (!e.HasFocus)
            {
                EnableSetInformationScrollViewTextViews();

                return;
            }
        }

        private void SetInformationScrollView_LayoutChange(object sender, View.LayoutChangeEventArgs e)
        {
            if (this.liftedWeightTextInputEditText.IsFocused)
            {
                this.setInformationScrollView.SmoothScrollTo(
                    this.plannedWeightTextView.Left,
                    this.plannedWeightTextView.Bottom
                    );

                return;
            }

            if (this.liftedRepetitionsTextInputEditText.IsFocused)
            {
                this.setInformationScrollView.SmoothScrollTo(
                    this.plannedRepetitionsTextView.Left,
                    this.plannedRepetitionsTextView.Top
                    );

                return;
            }
        }

        private void RegisterLiftedValuesButton_Click(object sender, EventArgs e)
        {
            this.registerLiftedValuesErrorStringBuilder = new StringBuilder();
            this.registerLiftedValuesWarningStringBuilder = new StringBuilder();

            double liftedWeight;
            int liftedRepetitions;

            if (!double.TryParse(this.liftedWeightTextInputEditText.Text, out liftedWeight))
                this.registerLiftedValuesErrorStringBuilder.AppendLine(
                    this.GetString(Resource.String.enter_lifted_weight_dot)
                    );
            else
            {
                if (liftedWeight < 0.00)
                    this.registerLiftedValuesErrorStringBuilder.AppendLine(
                        this.GetString(Resource.String.lifted_weight_must_not_be_less_than_0_dot)
                        );
            }

            if (!int.TryParse(this.liftedRepetitionsTextInputEditText.Text, out liftedRepetitions))
                this.registerLiftedValuesErrorStringBuilder.AppendLine(
                    this.GetString(Resource.String.enter_lifted_repetitions_dot)
                    );
            else
            {
                if (liftedRepetitions < 0)
                    this.registerLiftedValuesErrorStringBuilder.AppendLine(
                        this.GetString(Resource.String.lifted_repetitions_must_not_be_less_than_0_dot)
                        );
            }

            if (this.registerLiftedValuesErrorStringBuilder.Length > 0)
            {
                this.registerLiftedValuesErrorAlertDialog.SetMessage(
                    this.registerLiftedValuesErrorStringBuilder.ToString().TrimEnd()
                    );

                this.registerLiftedValuesErrorAlertDialog.Show();

                return;
            }

            bool liftedWeightWithinPlannedRange;
            bool liftedRepetitionsWithinPlannedRange;

            liftedWeightWithinPlannedRange =
                this.plannedSetPresenter.WeightWithinPlannedRange(
                    this.plannedCycleGuid,
                    this.plannedSessionNumber,
                    this.plannedSetNumber,
                    liftedWeight
                    );

            liftedRepetitionsWithinPlannedRange =
                this.plannedSetPresenter.RepetitionsWithinPlannedRange(
                    this.plannedCycleGuid,
                    this.plannedSessionNumber,
                    this.plannedSetNumber,
                    liftedRepetitions
                    );

            if (!liftedWeightWithinPlannedRange)
                this.registerLiftedValuesWarningStringBuilder.AppendLine(
                    this.GetString(Resource.String.lifted_weight_is_outside_of_planned_range_dot)
                    );

            if (!liftedRepetitionsWithinPlannedRange)
                this.registerLiftedValuesWarningStringBuilder.AppendLine(
                    this.GetString(Resource.String.lifted_repetitions_are_outside_of_planned_range_dot)
                    );

            if (this.registerLiftedValuesWarningStringBuilder.Length > 0)
            {
                this.registerLiftedValuesWarningStringBuilder.AppendLine(
                    this.GetString(Resource.String.are_you_sure_question_mark)
                    );

                this.registerLiftedValuesWarningAlertDialog.SetMessage(
                    this.registerLiftedValuesWarningStringBuilder.ToString().TrimEnd()
                    );

                this.registerLiftedValuesWarningAlertDialog.Show();

                return;
            }

            this.plannedSetPresenter.UpdatePlannedSetLiftedValues(
                this.plannedCycleGuid,
                this.plannedSessionNumber,
                this.plannedSetNumber,
                (liftedRepetitions, liftedWeight)
                );

            this.Finish();
        }

        private void RegisterLiftedValuesWarningAlertDialogYes(object sender, DialogClickEventArgs e)
        {
            this.registerLiftedValuesErrorStringBuilder = new StringBuilder();

            double liftedWeight;
            int liftedRepetitions;

            if (!double.TryParse(this.liftedWeightTextInputEditText.Text, out liftedWeight))
                this.registerLiftedValuesErrorStringBuilder.AppendLine(
                    this.GetString(Resource.String.enter_lifted_weight_dot)
                    );

            if (liftedWeight < 0.00)
                this.registerLiftedValuesErrorStringBuilder.AppendLine(
                    this.GetString(Resource.String.lifted_weight_must_not_be_less_than_0_dot)
                    );

            if (!int.TryParse(this.liftedRepetitionsTextInputEditText.Text, out liftedRepetitions))
                this.registerLiftedValuesErrorStringBuilder.AppendLine(
                    this.GetString(Resource.String.enter_lifted_repetitions_dot)
                    );

            if (liftedRepetitions < 0)
                this.registerLiftedValuesErrorStringBuilder.AppendLine(
                    this.GetString(Resource.String.lifted_repetitions_must_not_be_less_than_0_dot)
                    );

            if (this.registerLiftedValuesErrorStringBuilder.Length > 0)
            {
                this.registerLiftedValuesErrorAlertDialog.SetMessage(
                    this.registerLiftedValuesErrorStringBuilder.ToString().TrimEnd()
                    );

                this.registerLiftedValuesErrorAlertDialog.Show();

                return;
            }

            this.plannedSetPresenter.UpdatePlannedSetLiftedValues(
                this.plannedCycleGuid,
                this.plannedSessionNumber,
                this.plannedSetNumber,
                (liftedRepetitions, liftedWeight)
                );

            this.Finish();
        }

        protected override void OnPause()
        {
            base.OnPause();

            this.setInformationScrollView.LayoutChange -= SetInformationScrollView_LayoutChange;
            this.liftedRepetitionsTextInputEditText.FocusChange -= LiftedRepetitionsTextInputEditText_FocusChange;
            this.liftedWeightTextInputEditText.FocusChange -= LiftedWeightTextInputEditText_FocusChange;
            this.registerLiftedValuesButton.Click -= RegisterLiftedValuesButton_Click;

            this.registerLiftedValuesErrorAlertDialog.Dispose();
            this.registerLiftedValuesWarningAlertDialog.Dispose();

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

        public void OutputPlannedPercentageOfReferencePoint(string plannedPercentageOfReferencePoint)
        {
            this.plannedPercentageOfReferencePointTextView.Text =
                plannedPercentageOfReferencePoint ?? this.GetString(Resource.String.not_available);
        }

        public void OutputWeightAdjustmentConstant(string weightAdjustmentConstant)
        {
            this.weightAdjustmentConstantTextView.Text =
                weightAdjustmentConstant ?? this.GetString(Resource.String.not_available);
        }

        public void OutputPlannedWeight(string plannedWeight)
        {
            this.plannedWeightTextView.Text =
                plannedWeight ?? this.GetString(Resource.String.not_available);
        }

        public void OutputPlannedRepetitions(string plannedRepetitions)
        {
            this.plannedRepetitionsTextView.Text =
                plannedRepetitions ?? this.GetString(Resource.String.not_available);
        }

        public void OutputLiftedValues(string liftedWeight, string liftedRepetitions)
        {
            this.liftedWeightTextView.Text =
                liftedWeight ?? this.GetString(Resource.String.not_available);

            this.liftedRepetitionsTextView.Text =
                liftedRepetitions ?? this.GetString(Resource.String.not_available);
        }

        public void OutputNote(string note)
        {
            this.noteTextView.Text =
                note ?? this.GetString(Resource.String.not_available);
        }

        private void EnableSetInformationScrollViewTextViews()
        {
            this.cycleTemplateNameLabelTextView.Enabled = true;
            this.cycleTemplateNameTextView.Enabled = true;
            this.referencePointLabelTextView.Enabled = true;
            this.referencePointTextView.Enabled = true;
            this.sessionNumberLabelTextView.Enabled = true;
            this.sessionNumberTextView.Enabled = true;
            this.setNumberLabelTextView.Enabled = true;
            this.setNumberTextView.Enabled = true;
            this.plannedPercentageOfReferencePointLabelTextView.Enabled = true;
            this.plannedPercentageOfReferencePointTextView.Enabled = true;
            this.weightAdjustmentConstantLabelTextView.Enabled = true;
            this.weightAdjustmentConstantTextView.Enabled = true;
            this.plannedWeightLabelTextView.Enabled = true;
            this.plannedWeightTextView.Enabled = true;
            this.plannedRepetitionsLabelTextView.Enabled = true;
            this.plannedRepetitionsTextView.Enabled = true;
            this.liftedWeightLabelTextView.Enabled = true;
            this.liftedWeightTextView.Enabled = true;
            this.liftedRepetitionsLabelTextView.Enabled = true;
            this.liftedRepetitionsTextView.Enabled = true;
            this.noteLabelTextView.Enabled = true;
            this.noteTextView.Enabled = true;
        }
    }
}