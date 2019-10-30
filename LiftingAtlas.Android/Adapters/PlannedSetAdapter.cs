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
    public class PlannedSetAdapter : BaseAdapter<PlannedSet>
    {
        private Activity activity;
        private SessionNumber plannedSessionNumber;
        private IList<PlannedSet> plannedSets;
        private SessionSetNumber currentPlannedSessionAndCurrentPlannedSetNumbers;

        public PlannedSetAdapter(Activity activity) : base()
        {
            if (activity == null)
                throw new ArgumentNullException(nameof(activity));

            this.activity = activity;

            this.plannedSets = new List<PlannedSet>();
        }

        public override PlannedSet this[int position]
        {
            get
            {
                return this.plannedSets[position];
            }
        }

        public override int Count
        {
            get
            {
                return this.plannedSets.Count;
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
                view = this.activity.LayoutInflater.Inflate(
                    Resource.Layout.item_planned_set,
                    parent,
                    false
                    );

            PlannedSet plannedSet = this.plannedSets[position];

            view.FindViewById<TextView>(Resource.Id.set_number_textview).Text =
                plannedSet.Number.ToString();

            view.FindViewById<TextView>(Resource.Id.planned_weight_textview).Text =
                plannedSet.PlannedWeight?.ToString() ?? this.activity.GetString(Resource.String.not_available);

            view.FindViewById<TextView>(Resource.Id.planned_repetitions_textview).Text =
                plannedSet.PlannedRepetitions?.ToString() ?? this.activity.GetString(Resource.String.not_available);

            view.FindViewById<TextView>(Resource.Id.planned_percentage_of_reference_point_and_weight_adjustment_constant_textview).Text =
                $"{plannedSet.PlannedPercentageOfReferencePoint?.ToString('%')}{plannedSet.WeightAdjustmentConstant?.ToString("+#;-#")}"
                ??
                this.activity.GetString(Resource.String.not_available);

            view.FindViewById<TextView>(Resource.Id.current_textview).Visibility =
                SetIsCurrent(plannedSet) ? ViewStates.Visible : ViewStates.Gone;

            view.FindViewById<TextView>(Resource.Id.done_textview).Visibility =
                plannedSet.Done ? ViewStates.Visible : ViewStates.Gone;

            TextView noteTextView = view.FindViewById<TextView>(Resource.Id.note_textview);
            noteTextView.Visibility = plannedSet.Note == null ? ViewStates.Gone : ViewStates.Visible;
            noteTextView.Text = plannedSet.Note;

            return view;
        }

        public void SetPlannedSets(
            SessionNumber plannedSessionNumber,
            IList<PlannedSet> plannedSets,
            SessionSetNumber currentPlannedSessionAndCurrentPlannedSetNumbers
            )
        {
            if (plannedSessionNumber == null)
                throw new ArgumentNullException(nameof(plannedSessionNumber));

            if (plannedSets == null && currentPlannedSessionAndCurrentPlannedSetNumbers != null)
                throw new ArgumentException(
                    $"{nameof(currentPlannedSessionAndCurrentPlannedSetNumbers)} can not be non-null " +
                    $"if {nameof(plannedSets)} is null.",
                    nameof(currentPlannedSessionAndCurrentPlannedSetNumbers)
                    );

            this.NotifyDataSetInvalidated();

            this.plannedSets.Clear();

            this.plannedSessionNumber = plannedSessionNumber;

            this.currentPlannedSessionAndCurrentPlannedSetNumbers = currentPlannedSessionAndCurrentPlannedSetNumbers;

            if (plannedSets != null)
                foreach (PlannedSet plannedSet in plannedSets)
                    this.plannedSets.Add(plannedSet);

            this.NotifyDataSetChanged();
        }

        private bool SetIsCurrent(PlannedSet plannedSet)
        {
            if (plannedSet == null)
                throw new ArgumentNullException(nameof(plannedSet));

            if (this.currentPlannedSessionAndCurrentPlannedSetNumbers == null)
                return false;

            return (
                (this.plannedSessionNumber == this.currentPlannedSessionAndCurrentPlannedSetNumbers.SessionNumber)
                &&
                (plannedSet.Number == this.currentPlannedSessionAndCurrentPlannedSetNumbers.SetNumber)
                );
        }
    }
}