using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Template cycle view.
    /// </summary>
    public interface ITemplateCycleView
    {
        /// <summary>
        /// Outputs template sessions.
        /// </summary>
        /// <param name="templateSessionsAndNoteReferencePositions">
        /// Template sessions and note reference positions (within template sessions) to output.</param>
        void OutputTemplateSessions(
            IList<(string templateSession, IList<NonNegativeI32Range> NoteReferencePositions)> templateSessionsAndNoteReferencePositions
            );

        /// <summary>
        /// Outputs template set notes.
        /// </summary>
        /// <param name="templateSetNotesAndNoteReferencePositions">
        /// Template set notes and note reference positions (within template set notes) to output.</param>
        void OutputTemplateSetNotes(
            IList<(string templateSetNote, NonNegativeI32Range NoteReferencePosition)> templateSetNotesAndNoteReferencePositions
            );
    }
}
