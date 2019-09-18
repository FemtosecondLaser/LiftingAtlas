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
    public class TemplateSessionAdapter : BaseAdapter<(string templateSession, IList<NonNegativeI32Range> NoteReferencePositions)>
    {
        private Activity activity;
        private IList<(string templateSession, IList<NonNegativeI32Range> NoteReferencePositions)> templateSessionsAndNoteReferencePositions;

        public TemplateSessionAdapter(Activity activity) : base()
        {
            if (activity == null)
                throw new ArgumentNullException(nameof(activity));

            this.activity = activity;

            this.templateSessionsAndNoteReferencePositions =
                new List<(string templateSession, IList<NonNegativeI32Range> NoteReferencePositions)>();
        }

        public override (string templateSession, IList<NonNegativeI32Range> NoteReferencePositions) this[int position]
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

            (string templateSession, IList<NonNegativeI32Range> NoteReferencePositions) templateSessionAndNoteReferencePositions =
                this.templateSessionsAndNoteReferencePositions[position];

            SpannableString templateSession = new SpannableString(templateSessionAndNoteReferencePositions.templateSession);
            if (templateSessionAndNoteReferencePositions.NoteReferencePositions != null)
                foreach (NonNegativeI32Range NoteReferencePosition in templateSessionAndNoteReferencePositions.NoteReferencePositions)
                    templateSession.SetSpan(
                        new ForegroundColorSpan(new Color(ContextCompat.GetColor(this.activity, Resource.Color.colorAccent))),
                        NoteReferencePosition.LowerBound,
                        NoteReferencePosition.UpperBound,
                        SpanTypes.ExclusiveExclusive
                        );

            view.FindViewById<TextView>(Resource.Id.template_session_textview).SetText(templateSession, TextView.BufferType.Spannable);

            return view;
        }

        public void SetTemplateSessionsAndNoteReferencePositions(
            IList<(string templateSession, IList<NonNegativeI32Range> NoteReferencePositions)> templateSessionsAndNoteReferencePositions
            )
        {
            this.NotifyDataSetInvalidated();

            this.templateSessionsAndNoteReferencePositions.Clear();

            if (templateSessionsAndNoteReferencePositions != null)
                foreach (
                    (string, IList<NonNegativeI32Range>) templateSessionAndNoteReferencePositions
                    in
                    templateSessionsAndNoteReferencePositions
                    )
                    this.templateSessionsAndNoteReferencePositions.Add(templateSessionAndNoteReferencePositions);

            this.NotifyDataSetChanged();
        }
    }
}