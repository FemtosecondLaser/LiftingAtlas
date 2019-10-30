using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using LiftingAtlas.Standard;

namespace LiftingAtlas.Droid
{
    public class TemplateSessionAdapter : BaseAdapter<(string templateSession, IList<(int start, int end)> noteReferencePositions)>
    {
        private Activity activity;
        private IList<(string templateSession, IList<(int start, int end)> noteReferencePositions)> templateSessionsAndNoteReferencePositions;

        public TemplateSessionAdapter(Activity activity) : base()
        {
            if (activity == null)
                throw new ArgumentNullException(nameof(activity));

            this.activity = activity;

            this.templateSessionsAndNoteReferencePositions =
                new List<(string templateSession, IList<(int start, int end)> noteReferencePositions)>();
        }

        public override (string templateSession, IList<(int start, int end)> noteReferencePositions) this[int position]
        {
            get
            {
                return this.templateSessionsAndNoteReferencePositions[position];
            }
        }

        public override int Count
        {
            get
            {
                return this.templateSessionsAndNoteReferencePositions.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override bool IsEnabled(int position)
        {
            return false;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view == null)
                view = this.activity.LayoutInflater.Inflate(
                    Resource.Layout.item_template_session,
                    parent,
                    false
                    );

            (string templateSession, IList<(int start, int end)> noteReferencePositions) templateSessionAndNoteReferencePositions =
                this.templateSessionsAndNoteReferencePositions[position];

            SpannableString templateSession = new SpannableString(templateSessionAndNoteReferencePositions.templateSession);
            if (templateSessionAndNoteReferencePositions.noteReferencePositions != null)
                foreach ((int start, int end) NoteReferencePosition in templateSessionAndNoteReferencePositions.noteReferencePositions)
                    templateSession.SetSpan(
                        new ForegroundColorSpan(new Color(ContextCompat.GetColor(this.activity, Resource.Color.colorAccent))),
                        NoteReferencePosition.start,
                        NoteReferencePosition.end,
                        SpanTypes.ExclusiveExclusive
                        );

            view.FindViewById<TextView>(Resource.Id.template_session_textview).SetText(templateSession, TextView.BufferType.Spannable);

            return view;
        }

        public void SetTemplateSessionsAndNoteReferencePositions(
            IList<(string templateSession, IList<(int start, int end)> noteReferencePositions)> templateSessionsAndNoteReferencePositions
            )
        {
            this.NotifyDataSetInvalidated();

            this.templateSessionsAndNoteReferencePositions.Clear();

            if (templateSessionsAndNoteReferencePositions != null)
                foreach (
                    (string, IList<(int start, int end)>) templateSessionAndNoteReferencePositions
                    in
                    templateSessionsAndNoteReferencePositions
                    )
                    this.templateSessionsAndNoteReferencePositions.Add(templateSessionAndNoteReferencePositions);

            this.NotifyDataSetChanged();
        }
    }
}