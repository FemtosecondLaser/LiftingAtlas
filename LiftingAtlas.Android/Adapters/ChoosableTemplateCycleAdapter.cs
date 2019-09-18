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
using static Android.Views.View;

namespace LiftingAtlas.Droid
{
    public class ChoosableTemplateCycleAdapter : BaseAdapter<string>, INotifyViewTemplateCycleRequested
    {
        private Activity activity;
        private IList<string> cycleTemplateNames;

        public ChoosableTemplateCycleAdapter(Activity activity) : base()
        {
            if (activity == null)
                throw new ArgumentNullException(nameof(activity));

            this.activity = activity;

            this.cycleTemplateNames = new List<string>();
        }

        public event ViewTemplateCycleRequestedEventHandler ViewTemplateCycleRequested;

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

            if (convertView == null)
                view = this.activity.LayoutInflater.Inflate(
                    Resource.Layout.item_choosable_template_cycle,
                    parent,
                    false
                    );

            view.FindViewById<TextView>(Resource.Id.cycle_template_name_textview).Text =
                this.cycleTemplateNames[position] ?? this.activity.GetString(Resource.String.not_available);

            if (convertView == null)
            {
                Button viewButton = view.FindViewById<Button>(Resource.Id.view_button);
                viewButton.Click += ViewButton_Click;
            }

            return view;
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            View parent = (sender as Button).Parent as View;
            string cycleTemplateName =
                parent.FindViewById<TextView>(Resource.Id.cycle_template_name_textview).Text;

            OnViewTemplateCycleRequested(cycleTemplateName);
        }

        protected void OnViewTemplateCycleRequested(string cycleTemplateName)
        {
            ViewTemplateCycleRequested?.Invoke(
                this,
                new ViewTemplateCycleRequestedEventArgs(cycleTemplateName)
                );
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