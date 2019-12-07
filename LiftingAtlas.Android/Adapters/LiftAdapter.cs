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

namespace LiftingAtlas.Droid
{
    public class LiftAdapter : BaseAdapter<string>, ISpinnerAdapter
    {
        private Activity activity;
        private List<string> lifts;

        public LiftAdapter(Activity activity) : base()
        {
            if (activity == null)
                throw new ArgumentNullException(nameof(activity));

            this.activity = activity;

            this.lifts = new List<string>();
        }

        public override string this[int position]
        {
            get
            {
                return this.lifts[position];
            }
        }

        public int GetItemPosition(string item)
        {
            return this.lifts.IndexOf(item);
        }

        public override int Count
        {
            get
            {
                return this.lifts.Count;
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
                    Resource.Layout.item_lift,
                    parent,
                    false
                    );

            view.FindViewById<TextView>(Resource.Id.lift_textview).Text =
                this.lifts[position];

            return view;
        }

        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view == null)
                view = this.activity.LayoutInflater.Inflate(
                    Resource.Layout.item_lift,
                    parent,
                    false
                    );

            view.FindViewById<TextView>(Resource.Id.lift_textview).Text =
                this.lifts[position];

            return view;
        }

        public void SetLifts(IList<string> lifts)
        {
            this.NotifyDataSetInvalidated();

            this.lifts.Clear();

            if (lifts != null)
                foreach (string lift in lifts)
                    this.lifts.Add(lift);

            this.NotifyDataSetChanged();
        }
    }
}