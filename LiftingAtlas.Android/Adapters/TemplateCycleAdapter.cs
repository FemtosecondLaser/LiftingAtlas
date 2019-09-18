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
    public class TemplateCycleAdapter : BaseAdapter<string>
    {
        private Activity activity;
        private IList<string> cycleTemplateNames;

        public TemplateCycleAdapter(Activity activity) : base()
        {
            if (activity == null)
                throw new ArgumentNullException(nameof(activity));

            this.activity = activity;

            this.cycleTemplateNames = new List<string>();
        }

        public override string this[int position]
        {
            get
            {
                return this.cycleTemplateNames[position];
            }
        }

        public override int Count
        {
            get
            {
                return this.cycleTemplateNames.Count;
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
                    Resource.Layout.item_template_cycle,
                    parent,
                    false
                    );

            view.FindViewById<TextView>(Resource.Id.cycle_template_name_textview).Text =
                this.cycleTemplateNames[position];

            return view;
        }

        public void SetCycleTemplateNames(IList<string> cycleTemplateNames)
        {
            NotifyDataSetInvalidated();

            this.cycleTemplateNames.Clear();

            if (cycleTemplateNames != null)
                foreach (string cycleTemplateName in cycleTemplateNames)
                    this.cycleTemplateNames.Add(cycleTemplateName);

            NotifyDataSetChanged();
        }
    }
}