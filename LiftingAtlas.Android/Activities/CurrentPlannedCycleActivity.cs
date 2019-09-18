using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Autofac;
using LiftingAtlas.Standard;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace LiftingAtlas.Droid
{
    [Activity(Label = "@string/current_planned_cycle")]
    public class CurrentPlannedCycleActivity : AppCompatActivity, ICurrentPlannedCycleView
    {
        private Lift lift;
        private Guid? currentPlannedCycleGuid;
        private string currentPlannedCycleTemplateName;
        private string currentPlannedCycleReferencePoint;
        private Toolbar toolbar;
        private TextView cycleTemplateNameTextView;
        private TextView referencePointTextView;
        private ListView sessionsListView;
        private PlannedSessionAdapter plannedSessionAdapter;
        private ICurrentPlannedCyclePresenter currentPlannedCyclePresenter;
        private ILifetimeScope lifetimeScope;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.SetContentView(Resource.Layout.activity_current_planned_cycle);

            string liftString = base.Intent.Extras.GetString(BundleKeys.Lift);
            this.lift = LiftResolver.StringToLift(liftString);

            this.toolbar = this.FindViewById<Toolbar>(Resource.Id.toolbar);
            this.cycleTemplateNameTextView = this.FindViewById<TextView>(Resource.Id.cycle_template_name_textview);
            this.referencePointTextView = this.FindViewById<TextView>(Resource.Id.reference_point_textview);
            this.sessionsListView = this.FindViewById<ListView>(Resource.Id.sessions_listview);

            this.SetSupportActionBar(this.toolbar);
            this.SupportActionBar.Title = this.GetString(LiftSpecificStringIdResolver.CurrentLiftCycleStringId(this.lift));

            this.plannedSessionAdapter = new PlannedSessionAdapter(this);
            this.sessionsListView.Adapter = this.plannedSessionAdapter;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.current_cycle, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.new_cycle:
                    Bundle bundle = new Bundle(1);
                    bundle.PutString(BundleKeys.Lift, LiftResolver.LiftToString(lift));
                    Intent intent = new Intent(this, typeof(NewPlannedCycleActivity));
                    intent.PutExtras(bundle);
                    base.StartActivity(intent);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            this.lifetimeScope = App.Container.BeginLifetimeScope();

            this.currentPlannedCyclePresenter =
                this.lifetimeScope.Resolve<ICurrentPlannedCyclePresenter>(
                    new TypedParameter(typeof(ICurrentPlannedCycleView), this)
                    );

            this.currentPlannedCycleGuid =
                this.currentPlannedCyclePresenter.GetCurrentPlannedCycleGuid(
                    lift
                    );

            this.currentPlannedCyclePresenter.PresentCurrentPlannedCycleDataForTheLift(
                lift
                );

            this.sessionsListView.ItemClick += SessionsListView_ItemClick;
        }

        protected override void OnPause()
        {
            base.OnPause();

            this.sessionsListView.ItemClick -= SessionsListView_ItemClick;

            this.lifetimeScope.Dispose();
        }
        private void SessionsListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            PlannedSession<PlannedSet> plannedSession = this.plannedSessionAdapter[e.Position];

            Bundle bundle = new Bundle(5);
            bundle.PutString(BundleKeys.Lift, LiftResolver.LiftToString(lift));
            bundle.PutString(BundleKeys.PlannedCycleGuid, this.currentPlannedCycleGuid.ToString());
            bundle.PutString(BundleKeys.PlannedCycleTemplateName, this.currentPlannedCycleTemplateName);
            bundle.PutString(BundleKeys.PlannedCycleReferencePoint, this.currentPlannedCycleReferencePoint);
            bundle.PutInt(BundleKeys.SessionNumber, plannedSession.Number);
            Intent intent = new Intent(this, typeof(PlannedSessionActivity));
            intent.PutExtras(bundle);
            base.StartActivity(intent);
        }

        public void OutputCurrentPlannedCycleTemplateName(string currentPlannedCycleTemplateName)
        {
            this.currentPlannedCycleTemplateName = currentPlannedCycleTemplateName;

            this.cycleTemplateNameTextView.Text =
                this.currentPlannedCycleTemplateName ?? this.GetString(Resource.String.not_available);
        }

        public void OutputCurrentPlannedCycleReferencePoint(string currentPlannedCycleReferencePoint)
        {
            this.currentPlannedCycleReferencePoint = currentPlannedCycleReferencePoint;

            this.referencePointTextView.Text =
                this.currentPlannedCycleReferencePoint ?? this.GetString(Resource.String.not_available);
        }

        public void OutputCurrentPlannedCycleSessions(
            IList<PlannedSession<PlannedSet>> currentPlannedCycleSessions
            )
        {
            int? currentPlannedSessionNumber;

            if (this.currentPlannedCycleGuid == null)
                currentPlannedSessionNumber = null;
            else
                currentPlannedSessionNumber =
                    this.currentPlannedCyclePresenter.GetCurrentPlannedSessionNumber(
                        this.currentPlannedCycleGuid.Value
                        );

            this.plannedSessionAdapter.SetPlannedSessions(
                currentPlannedCycleSessions,
                currentPlannedSessionNumber
                );
        }
    }
}