using System;
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
    public class PlannedSessionAdapter : BaseAdapter<PlannedSession<PlannedSet>>
    {
        private Activity activity;
        private IList<PlannedSession<PlannedSet>> plannedSessions;
        private int? currentPlannedSessionNumber;

        public PlannedSessionAdapter(Activity activity) : base()
        {
            if (activity == null)
                throw new ArgumentNullException(nameof(activity));

            this.activity = activity;

            this.plannedSessions = new List<PlannedSession<PlannedSet>>();
        }

        public override PlannedSession<PlannedSet> this[int position]
        {
            get
            {
                return this.plannedSessions[position];
            }
        }

        public override int Count
        {
            get
            {
                return this.plannedSessions.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view == null)
                view = this.activity.LayoutInflater.Inflate(Resource.Layout.item_planned_session, parent, false);

            PlannedSession<PlannedSet> plannedSession = this.plannedSessions[position];

            view.FindViewById<TextView>(Resource.Id.session_number_textview).Text =
                plannedSession.Number.ToString();

            view.FindViewById<TextView>(Resource.Id.sets_textview).Text =
                plannedSession.Sets.Count.ToString();

            view.FindViewById<TextView>(Resource.Id.current_textview).Visibility =
                SessionIsCurrent(plannedSession) ? ViewStates.Visible : ViewStates.Gone;

            view.FindViewById<TextView>(Resource.Id.done_textview).Visibility =
                plannedSession.Done ? ViewStates.Visible : ViewStates.Gone;

            return view;
        }

        public void SetPlannedSessions(
            IList<PlannedSession<PlannedSet>> plannedSessions,
            int? currentPlannedSessionNumber
            )
        {
            if (plannedSessions == null && currentPlannedSessionNumber != null)
                throw new ArgumentException(
                    $"{nameof(currentPlannedSessionNumber)} can not be non-null if {nameof(plannedSessions)} is null.",
                    nameof(currentPlannedSessionNumber)
                    );

            this.NotifyDataSetInvalidated();

            this.plannedSessions.Clear();

            this.currentPlannedSessionNumber = currentPlannedSessionNumber;

            if (plannedSessions != null)
                foreach (PlannedSession<PlannedSet> plannedSession in plannedSessions)
                    this.plannedSessions.Add(plannedSession);

            this.NotifyDataSetChanged();
        }

        private bool SessionIsCurrent(PlannedSession<PlannedSet> plannedSession)
        {
            if (plannedSession == null)
                throw new ArgumentNullException(nameof(plannedSession));

            if (this.currentPlannedSessionNumber == null)
                return false;

            return plannedSession.Number == this.currentPlannedSessionNumber.Value;
        }
    }
}