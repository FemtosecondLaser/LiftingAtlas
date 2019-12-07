using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    public interface ITemplateCycleView
    {
        void OutputTemplateSessions(
            IReadOnlyList<(string templateSession, IReadOnlyList<(int start, int end)> noteReferencePositions)> templateSessionsAndNoteReferencePositions
            );

        void OutputTemplateSetNotes(
            IReadOnlyList<(string templateSetNote, (int start, int end) noteReferencePosition)> templateSetNotesAndNoteReferencePositions
            );
    }
}
