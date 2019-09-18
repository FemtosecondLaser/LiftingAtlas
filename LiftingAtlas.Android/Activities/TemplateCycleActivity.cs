using System;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using LiftingAtlas.Standard;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using TextInputLayout = Android.Support.Design.Widget.TextInputLayout;
using TextInputEditText = Android.Support.Design.Widget.TextInputEditText;
using Autofac;
using System.Text;
using Android.Views;
using System.Collections.Generic;

namespace LiftingAtlas.Droid
{
    [Activity(Label = "@string/cycle_template")]
    public class TemplateCycleActivity : AppCompatActivity, ITemplateCycleView
    {
        private string cycleTemplateName;
        private Toolbar toolbar;
        private ListView templateSessionsListView;
        private TemplateSessionAdapter templateSessionAdapter;
        private ListView templateSetNotesListView;
        private TemplateSetNoteAdapter templateSetNoteAdapter;
        private ITemplateCyclePresenter templateCyclePresenter;
        private ILifetimeScope lifetimeScope;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.SetContentView(Resource.Layout.activity_template_cycle);

            this.cycleTemplateName = base.Intent.Extras.GetString(BundleKeys.CycleTemplateName);

            this.toolbar = this.FindViewById<Toolbar>(Resource.Id.toolbar);
            this.templateSessionsListView = this.FindViewById<ListView>(Resource.Id.template_sessions_listview);
            this.templateSetNotesListView = this.FindViewById<ListView>(Resource.Id.template_set_notes_listview);

            this.SetSupportActionBar(this.toolbar);
            this.SupportActionBar.Title = $"{this.GetString(Resource.String.cycle_template_colon)} {this.cycleTemplateName}";

            this.templateSessionsListView.Divider = null;

            this.templateSetNotesListView.Divider = null;

            this.templateSessionAdapter = new TemplateSessionAdapter(this);
            this.templateSessionsListView.Adapter = this.templateSessionAdapter;

            this.templateSetNoteAdapter = new TemplateSetNoteAdapter(this);
            this.templateSetNotesListView.Adapter = this.templateSetNoteAdapter;
        }

        protected override void OnResume()
        {
            base.OnResume();

            this.lifetimeScope = App.Container.BeginLifetimeScope();

            this.templateCyclePresenter =
                this.lifetimeScope.Resolve<ITemplateCyclePresenter>(
                    new TypedParameter(typeof(ITemplateCycleView), this)
                    );

            this.templateCyclePresenter.PresentTemplateCycleData(
                this.cycleTemplateName
                );
        }

        protected override void OnPause()
        {
            base.OnPause();

            this.lifetimeScope.Dispose();
        }

        public void OutputTemplateSessions(
            IList<(string templateSession, IList<NonNegativeI32Range> NoteReferencePositions)> templateSessionsAndNoteReferencePositions
            )
        {
            this.templateSessionAdapter.SetTemplateSessionsAndNoteReferencePositions(templateSessionsAndNoteReferencePositions);
        }

        public void OutputTemplateSetNotes(
            IList<(string templateSetNote, NonNegativeI32Range NoteReferencePosition)> templateSetNotesAndNoteReferencePositions
            )
        {
            this.templateSetNoteAdapter.SetTemplateSetNotes(templateSetNotesAndNoteReferencePositions);
        }
    }
}