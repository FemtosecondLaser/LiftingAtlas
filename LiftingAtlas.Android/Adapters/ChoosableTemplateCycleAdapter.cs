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
using static Android.Views.View;

namespace LiftingAtlas.Droid
{
    public class ChoosableTemplateCycleAdapter : BaseAdapter<CycleTemplateName>, INotifyViewTemplateCycleRequested
    {
        private Activity activity;
        private List<CycleTemplateName> cycleTemplateNames;

        public ChoosableTemplateCycleAdapter(Activity activity) : base()
        {
            if (activity == null)
                throw new ArgumentNullException(nameof(activity));

            this.activity = activity;

            this.cycleTemplateNames = new List<CycleTemplateName>();
        }

        public event ViewTemplateCycleRequestedEventHandler ViewTemplateCycleRequested;

        public override CycleTemplateName this[int position]
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
            CycleTemplateName cycleTemplateName =
                new CycleTemplateName(parent.FindViewById<TextView>(Resource.Id.cycle_template_name_textview).Text);

            OnViewTemplateCycleRequested(cycleTemplateName);
        }

        protected void OnViewTemplateCycleRequested(CycleTemplateName cycleTemplateName)
        {
            if (cycleTemplateName == null)
                throw new ArgumentNullException(nameof(cycleTemplateName));

            ViewTemplateCycleRequested?.Invoke(
                new ViewTemplateCycleRequestedEventArgs(cycleTemplateName)
                );
        }

        public void SetCycleTemplateNames(IReadOnlyList<CycleTemplateName> cycleTemplateNames)
        {
            NotifyDataSetInvalidated();

            this.cycleTemplateNames.Clear();

            if (cycleTemplateNames != null)
                foreach (CycleTemplateName cycleTemplateName in cycleTemplateNames)
                    this.cycleTemplateNames.Add(cycleTemplateName);

            NotifyDataSetChanged();
        }
    }
}