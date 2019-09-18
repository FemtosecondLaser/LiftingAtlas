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
        private PlannedSetAdapter plannedSetAdapter;
        private IPlannedSessionPresenter plannedSessionPresenter;
        private ILifetimeScope lifetimeScope;

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

            this.SetSupportActionBar(this.toolbar);
            this.SupportActionBar.Title = this.GetString(LiftSpecificStringIdResolver.PlannedLiftSessionStringId(this.lift));

            this.cycleTemplateNameTextView.Text = this.plannedCycleTemplateName;
            this.referencePointTextView.Text = this.plannedCycleReferencePoint;
            this.sessionNumberTextView.Text = this.plannedSessionNumber.ToString();

            this.plannedSetAdapter = new PlannedSetAdapter(this);
            this.setsListView.Adapter = this.plannedSetAdapter;
        }

        protected override void OnResume()
        {
            base.OnResume();

            this.lifetimeScope = App.Container.BeginLifetimeScope();

            this.plannedSessionPresenter =
                this.lifetimeScope.Resolve<IPlannedSessionPresenter>(
                    new TypedParameter(typeof(IPlannedSessionView), this)
                    );

            this.plannedSessionPresenter.PresentPlannedSessionData(
                this.plannedCycleGuid,
                this.plannedSessionNumber
                );

            this.setsListView.ItemClick += SetsListView_ItemClick;
        }

        protected override void OnPause()
        {
            base.OnPause();

            this.setsListView.ItemClick -= SetsListView_ItemClick;

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
            IList<PlannedSet> plannedSessionSets
            )
        {
            (int currentPlannedSessionNumber, int currentPlannedSetNumber)? currentPlannedSessionAndCurrentPlannedSetNumbers =
                this.plannedSessionPresenter.GetCurrentPlannedSessionAndCurrentPlannedSetNumbers(
                    this.plannedCycleGuid
                    );

            this.plannedSetAdapter.SetPlannedSets(
                this.plannedSessionNumber,
                plannedSessionSets,
                currentPlannedSessionAndCurrentPlannedSetNumbers
                );
        }
    }
}