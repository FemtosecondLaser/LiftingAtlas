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
    public class TemplateSetNoteAdapter : BaseAdapter<(string templateSetNote, (int start, int end) noteReferencePosition)>
    {
        private Activity activity;
        private IList<(string templateSetNote, (int start, int end) noteReferencePosition)> templateSetNotesAndNoteReferencePositions;

        public TemplateSetNoteAdapter(Activity activity) : base()
        {
            if (activity == null)
                throw new ArgumentNullException(nameof(activity));

            this.activity = activity;

            this.templateSetNotesAndNoteReferencePositions =
                new List<(string templateSetNote, (int start, int end) noteReferencePosition)>();
        }

        public override (string templateSetNote, (int start, int end) noteReferencePosition) this[int position]
        {
            get
            {
                return this.templateSetNotesAndNoteReferencePositions[position];
            }
        }

        public override int Count
        {
            get
            {
                return this.templateSetNotesAndNoteReferencePositions.Count;
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
                    Resource.Layout.item_template_set_note,
                    parent,
                    false
                    );

            (string templateSetNote, (int start, int end) noteReferencePosition) templateSetNoteAndNoteReferencePosition
                = this.templateSetNotesAndNoteReferencePositions[position];

            SpannableString templateSetNote = new SpannableString(templateSetNoteAndNoteReferencePosition.templateSetNote);

            templateSetNote.SetSpan(
                new ForegroundColorSpan(new Color(ContextCompat.GetColor(this.activity, Resource.Color.colorAccent))),
                templateSetNoteAndNoteReferencePosition.noteReferencePosition.start,
                templateSetNoteAndNoteReferencePosition.noteReferencePosition.end,
                SpanTypes.ExclusiveExclusive
                );

            view.FindViewById<TextView>(Resource.Id.template_set_note_textview).SetText(templateSetNote, TextView.BufferType.Spannable);

            return view;
        }

        public void SetTemplateSetNotes(
            IList<(string templateSetNote, (int start, int end) noteReferencePosition)> templateSetNotesAndNoteReferencePositions
            )
        {
            this.NotifyDataSetInvalidated();

            this.templateSetNotesAndNoteReferencePositions.Clear();

            if (templateSetNotesAndNoteReferencePositions != null)
                foreach (
                    (string templateSetNote, (int start, int end) noteReferencePosition) templateSetNoteAndNoteReferencePosition
                    in
                    templateSetNotesAndNoteReferencePositions
                    )
                    this.templateSetNotesAndNoteReferencePositions.Add(templateSetNoteAndNoteReferencePosition);

            this.NotifyDataSetChanged();
        }
    }
}