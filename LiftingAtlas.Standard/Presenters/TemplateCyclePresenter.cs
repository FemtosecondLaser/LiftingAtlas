using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// Template cycle presenter.
    /// </summary>
    public class TemplateCyclePresenter : ITemplateCyclePresenter
    {
        #region Private fields

        /// <summary>
        /// Template cycle view.
        /// </summary>
        private readonly ITemplateCycleView templateCycleView;

        /// <summary>
        /// Template cycle provider Master.
        /// </summary>
        private readonly ITemplateCycleProviderMaster templateCycleProviderMaster;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates template cycle presenter.
        /// </summary>
        /// <param name="templateCycleView">Template cycle view. Must not be null.</param>
        /// <param name="templateCycleProviderMaster">Template cycle provider master.
        /// Must not be null.</param>
        /// <exception cref="ArgumentNullException"><paramref name="templateCycleView"/>
        /// or <paramref name="templateCycleProviderMaster"/> is null.</exception>
        public TemplateCyclePresenter(
            ITemplateCycleView templateCycleView,
            ITemplateCycleProviderMaster templateCycleProviderMaster
            )
        {
            if (templateCycleView == null)
                throw new ArgumentNullException(nameof(templateCycleView));

            if (templateCycleProviderMaster == null)
                throw new ArgumentNullException(nameof(templateCycleProviderMaster));

            this.templateCycleView = templateCycleView;
            this.templateCycleProviderMaster = templateCycleProviderMaster;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Presents template cycle data.
        /// </summary>
        /// <param name="cycleTemplateName">Cycle template name,
        /// to present template cycle data for. Must not be null.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="cycleTemplateName"/> is null.</exception>
        public void PresentTemplateCycleData(string cycleTemplateName)
        {
            if (cycleTemplateName == null)
                throw new ArgumentNullException(nameof(cycleTemplateName));

            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                this.templateCycleProviderMaster.TemplateCycle(cycleTemplateName);

            StringBuilder templateSessionsBuilder = new StringBuilder();
            List<(string templateSession, IList<NonNegativeI32Range> NoteReferencePositions)> templateSessionsAndNoteReferencePositions =
                new List<(string templateSession, IList<NonNegativeI32Range> NoteReferencePositions)>();

            StringBuilder templateSetNoteBuilder = new StringBuilder();
            Dictionary<string, int> templateSetNoteDictionary = new Dictionary<string, int>();
            List<(string templateSetNote, NonNegativeI32Range NoteReferencePosition)> templateSetNotesAndNoteReferencePositions =
                new List<(string templateSetNote, NonNegativeI32Range NoteReferencePosition)>();

            for (int i = 0; i < templateCycle.Sessions.Count; i++)
            {
                List<NonNegativeI32Range> templateSessionNoteReferencePositions = new List<NonNegativeI32Range>();

                templateSessionsBuilder.Clear();

                TemplateSession<TemplateSet> templateSession = templateCycle.Sessions[i];

                templateSessionsBuilder.Append($"{templateSession.Number}).");

                for (int j = 0; j < templateSession.Sets.Count; j++)
                {
                    int consecutiveIdenticalSetCount = 0;
                    for (int k = j + 1; k < templateSession.Sets.Count; k++)
                    {
                        if ((BaseSet)templateSession.Sets[j] == (BaseSet)templateSession.Sets[k])
                            consecutiveIdenticalSetCount++;
                        else
                            break;
                    }

                    if (j != 0)
                        templateSessionsBuilder.Append(',');

                    templateSessionsBuilder.Append(' ');

                    bool plannedPercentageOfReferencePointExists =
                        templateSession.Sets[j].PlannedPercentageOfReferencePoint != null;

                    bool weightAdjustmentConstantExists =
                        templateSession.Sets[j].WeightAdjustmentConstant != null;

                    if (plannedPercentageOfReferencePointExists)
                        templateSessionsBuilder.Append(
                            $"{templateSession.Sets[j].PlannedPercentageOfReferencePoint.ToString('%')}"
                            );

                    if (weightAdjustmentConstantExists)
                        templateSessionsBuilder.Append(
                            $"{templateSession.Sets[j].WeightAdjustmentConstant.Value.ToString("+#;-#")}"
                            );

                    bool noteExists =
                        templateSession.Sets[j].Note != null;

                    if (noteExists)
                    {
                        int noteReferenceNumber;

                        if (!templateSetNoteDictionary.TryGetValue(templateSession.Sets[j].Note, out noteReferenceNumber))
                            templateSetNoteDictionary.Add(
                                templateSession.Sets[j].Note,
                                (noteReferenceNumber = (templateSetNoteDictionary.Count + 1))
                                );

                        int noteReferenceStartPosition, noteReferenceEndPosition;

                        noteReferenceStartPosition = templateSessionsBuilder.Length;

                        templateSessionsBuilder.Append(
                            noteReferenceNumber.ToString().DecimalDigitStringToSuperscriptEquivalent()
                            );

                        noteReferenceEndPosition = templateSessionsBuilder.Length;

                        templateSessionNoteReferencePositions.Add(
                            new NonNegativeI32Range(noteReferenceStartPosition, noteReferenceEndPosition)
                            );
                    }

                    templateSessionsBuilder.Append('/');

                    if (consecutiveIdenticalSetCount > 0)
                        templateSessionsBuilder.Append($"{1 + consecutiveIdenticalSetCount}x");

                    templateSessionsBuilder.Append(templateSession.Sets[j].PlannedRepetitions.ToString());

                    j += consecutiveIdenticalSetCount;
                }

                templateSessionsAndNoteReferencePositions.Add(
                    (templateSessionsBuilder.ToString(), templateSessionNoteReferencePositions)
                    );
            }

            if (templateSetNoteDictionary.Count > 0)
                foreach (KeyValuePair<string, int> note in templateSetNoteDictionary.OrderBy(note => note.Value))
                {
                    int noteReferenceStartPosition, noteReferenceEndPosition;

                    templateSetNoteBuilder.Clear();

                    noteReferenceStartPosition = templateSetNoteBuilder.Length;

                    templateSetNoteBuilder.Append(
                        note.Value.ToString().DecimalDigitStringToSuperscriptEquivalent()
                        );

                    noteReferenceEndPosition = templateSetNoteBuilder.Length;

                    templateSetNoteBuilder.Append(' ');

                    templateSetNoteBuilder.Append(note.Key);

                    templateSetNotesAndNoteReferencePositions.Add(
                        (templateSetNoteBuilder.ToString(), new NonNegativeI32Range(noteReferenceStartPosition, noteReferenceEndPosition))
                        );
                }

            this.templateCycleView.OutputTemplateSessions(templateSessionsAndNoteReferencePositions);
            this.templateCycleView.OutputTemplateSetNotes(templateSetNotesAndNoteReferencePositions);
        }

        #endregion
    }
}
