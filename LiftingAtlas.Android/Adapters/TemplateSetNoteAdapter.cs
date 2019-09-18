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
    public class TemplateSetNoteAdapter : BaseAdapter<(string templateSetNote, NonNegativeI32Range NoteReferencePosition)>
    {
        private Activity activity;
        private IList<(string templateSetNote, NonNegativeI32Range NoteReferencePosition)> templateSetNotesAndNoteReferencePositions;

        public TemplateSetNoteAdapter(Activity activity) : base()
        {
            if (activity == null)
                throw new ArgumentNullException(nameof(activity));

            this.activity = activity;

            this.templateSetNotesAndNoteReferencePositions =
                new List<(string templateSetNote, NonNegativeI32Range NoteReferencePosition)>();
        }

        public override (string templateSetNote, NonNegativeI32Range NoteReferencePosition) this[int position]
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

            (string templateSetNote, NonNegativeI32Range NoteReferencePosition) templateSetNoteAndNoteReferencePosition
                = this.templateSetNotesAndNoteReferencePositions[position];

            SpannableString templateSetNote = new SpannableString(templateSetNoteAndNoteReferencePosition.templateSetNote);
            if (templateSetNoteAndNoteReferencePosition.NoteReferencePosition != null)
                templateSetNote.SetSpan(
                    new ForegroundColorSpan(new Color(ContextCompat.GetColor(this.activity, Resource.Color.colorAccent))),
                    templateSetNoteAndNoteReferencePosition.NoteReferencePosition.LowerBound,
                    templateSetNoteAndNoteReferencePosition.NoteReferencePosition.UpperBound,
                    SpanTypes.ExclusiveExclusive
                    );

            view.FindViewById<TextView>(Resource.Id.template_set_note_textview).SetText(templateSetNote, TextView.BufferType.Spannable);

            return view;
        }

        public void SetTemplateSetNotes(
            IList<(string templateSetNote, NonNegativeI32Range NoteReferencePosition)> templateSetNotesAndNoteReferencePositions
            )
        {
            this.NotifyDataSetInvalidated();

            this.templateSetNotesAndNoteReferencePositions.Clear();

            if (templateSetNotesAndNoteReferencePositions != null)
                foreach (
                    (string templateSetNote, NonNegativeI32Range NoteReferencePosition) templateSetNoteAndNoteReferencePosition
                    in
                    templateSetNotesAndNoteReferencePositions
                    )
                    this.templateSetNotesAndNoteReferencePositions.Add(templateSetNoteAndNoteReferencePosition);

            this.NotifyDataSetChanged();
        }
    }
}