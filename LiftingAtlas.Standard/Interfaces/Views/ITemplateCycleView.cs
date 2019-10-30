using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    public interface ITemplateCycleView
    {
        void OutputTemplateSessions(
            IList<(string templateSession, IList<(int start, int end)> noteReferencePositions)> templateSessionsAndNoteReferencePositions
            );

        void OutputTemplateSetNotes(
            IList<(string templateSetNote, (int start, int end) noteReferencePosition)> templateSetNotesAndNoteReferencePositions
            );
    }
}
