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
using Toolbar = Android.Support.V7.Widget.Toolbar;


namespace LiftingAtlas.Droid
{
    [Activity(Label = "@string/planned_session")]
    public class PlannedSessionActivity : AppCompatActivity, IPlannedSessionView
    {
        private Lift lift;
        private Guid plannedCycleGuid;
        private string plannedCycleTemplateName;
        private string plannedCycleReferencePoint;
        private int plannedSessionNumber;
        private Toolbar toolbar;
        private TextView cycleTemplateNameTextView;
        private TextView referencePointTextView;
        private TextView sessionNumberTextView;
        private ListView setsListView;
        private ProgressBar setsProgressBar;
        private PlannedSetAdapter plannedSetAdapter;
        private IPlannedSessionPresenter plannedSessionPresenter;
        private ILifetimeScope lifetimeScope;
        private bool mustSelectCurrentSet;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.SetContentView(Resource.Layout.activity_planned_session);

            string liftString = base.Intent.Extras.GetString(BundleKeys.Lift);
            this.lift = LiftResolver.StringToLift(liftString);
            this.plannedCycleGuid = Guid.Parse(base.Intent.Extras.GetString(BundleKeys.PlannedCycleGuid));
            this.plannedCycleTemplateName = base.Intent.Extras.GetString(BundleKeys.PlannedCycleTemplateName);
            this.plannedCycleReferencePoint = base.Intent.Extras.GetString(BundleKeys.PlannedCycleReferencePoint);
            this.plannedSessionNumber = base.Intent.Extras.GetInt(BundleKeys.SessionNumber);

            this.toolbar = this.FindViewById<Toolbar>(Resource.Id.toolbar);
            this.cycleTemplateNameTextView = this.FindViewById<TextView>(Resource.Id.cycle_template_name_textview);
            this.referencePointTextView = this.FindViewById<TextView>(Resource.Id.reference_point_textview);
            this.sessionNumberTextView = this.FindViewById<TextView>(Resource.Id.session_number_textview);
            this.setsListView = this.FindViewById<ListView>(Resource.Id.sets_listview);
            this.setsProgressBar = this.FindViewById<ProgressBar>(Resource.Id.sets_progressbar);

            this.SetSupportActionBar(this.toolbar);
            this.SupportActionBar.Title = this.GetString(LiftSpecificStringIdResolver.PlannedLiftSessionStringId(this.lift));

            this.cycleTemplateNameTextView.Text = this.plannedCycleTemplateName;
            this.referencePointTextView.Text = this.plannedCycleReferencePoint;
            this.sessionNumberTextView.Text = this.plannedSessionNumber.ToString();

            this.plannedSetAdapter = new PlannedSetAdapter(this);
            this.setsListView.Adapter = this.plannedSetAdapter;

            this.mustSelectCurrentSet = true;
        }

        protected async override void OnResume()
        {
            base.OnResume();

            this.setsListView.Visibility = ViewStates.Gone;
            this.setsProgressBar.Visibility = ViewStates.Visible;

            this.lifetimeScope = App.Container.BeginLifetimeScope();

            this.plannedSessionPresenter =
                this.lifetimeScope.Resolve<IPlannedSessionPresenter>(
                    new TypedParameter(typeof(IPlannedSessionView), this)
                    );

            this.plannedSessionPresenter.PlannedSessionDataPresented +=
                PlannedSessionPresenter_PlannedSessionDataPresented;

            await this.plannedSessionPresenter.PresentPlannedSessionDataAsync(
                this.plannedCycleGuid,
                new SessionNumber(this.plannedSessionNumber)
                );

            this.setsListView.ItemClick += SetsListView_ItemClick;

            this.setsListView.Post(
                () =>
                {
                    if (this.mustSelectCurrentSet)
                    {
                        int? currentPlannedSetPosition = this.plannedSetAdapter.CurrentPlannedSetPosition;

                        if (currentPlannedSetPosition != null)
                            this.setsListView.SetSelection(
                                currentPlannedSetPosition.Value
                                );

                        this.mustSelectCurrentSet = false;
                    }
                }
                );
        }

        protected override void OnPause()
        {
            base.OnPause();

            this.setsListView.ItemClick -= SetsListView_ItemClick;

            this.plannedSessionPresenter.PlannedSessionDataPresented -=
                PlannedSessionPresenter_PlannedSessionDataPresented;

            this.lifetimeScope.Dispose();
        }

        private void SetsListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            PlannedSet plannedSet = this.plannedSetAdapter[e.Position];

            Bundle bundle = new Bundle(6);
            bundle.PutString(BundleKeys.Lift, LiftResolver.LiftToString(lift));
            bundle.PutString(BundleKeys.PlannedCycleGuid, this.plannedCycleGuid.ToString());
            bundle.PutString(BundleKeys.PlannedCycleTemplateName, this.plannedCycleTemplateName);
            bundle.PutString(BundleKeys.PlannedCycleReferencePoint, this.plannedCycleReferencePoint);
            bundle.PutInt(BundleKeys.SessionNumber, plannedSessionNumber);
            bundle.PutInt(BundleKeys.SetNumber, plannedSet.Number);
            Intent intent = new Intent(this, typeof(PlannedSetActivity));
            intent.PutExtras(bundle);
            base.StartActivity(intent);
        }

        public void OutputPlannedSessionSets(
            IReadOnlyList<PlannedSet> plannedSessionSets,
            SessionSetNumber currentPlannedSessionAndCurrentPlannedSetNumbers
            )
        {
            this.plannedSetAdapter.SetPlannedSets(
                new SessionNumber(this.plannedSessionNumber),
                plannedSessionSets,
                currentPlannedSessionAndCurrentPlannedSetNumbers
                );
        }

        private void PlannedSessionPresenter_PlannedSessionDataPresented()
        {
            this.setsProgressBar.Visibility = ViewStates.Gone;
            this.setsListView.Visibility = ViewStates.Visible;
        }
    }
}