using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftingAtlas.Standard
{
    public class TemplateCyclePresenter : ITemplateCyclePresenter
    {
        #region Private fields

        private readonly ITemplateCycleView templateCycleView;
        private readonly ITemplateCycleProviderMaster templateCycleProviderMaster;

        #endregion

        #region Constructors

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

        #region Events

        public event TemplateCycleDataPresentedEventHandler TemplateCycleDataPresented;

        #endregion

        #region Methods

        public async Task PresentTemplateCycleDataAsync(CycleTemplateName cycleTemplateName)
        {
            if (cycleTemplateName == null)
                throw new ArgumentNullException(nameof(cycleTemplateName));

            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                await this.templateCycleProviderMaster.TemplateCycleAsync(cycleTemplateName);

            StringBuilder templateSessionsBuilder = new StringBuilder();
            List<(string templateSession, IReadOnlyList<(int start, int end)> noteReferencePositions)> templateSessionsAndNoteReferencePositions =
                new List<(string templateSession, IReadOnlyList<(int start, int end)> noteReferencePositions)>();

            StringBuilder templateSetNoteBuilder = new StringBuilder();
            Dictionary<string, int> templateSetNoteDictionary = new Dictionary<string, int>();
            List<(string templateSetNote, (int start, int end) NoteReferencePosition)> templateSetNotesAndNoteReferencePositions =
                new List<(string templateSetNote, (int start, int end) NoteReferencePosition)>();

            for (int i = 0; i < templateCycle.Sessions.Count; i++)
            {
                List<(int start, int end)> templateSessionNoteReferencePositions = new List<(int start, int end)>();

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

                        templateSessionNoteReferencePositions.Add((noteReferenceStartPosition, noteReferenceEndPosition));
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
                        (templateSetNoteBuilder.ToString(), (noteReferenceStartPosition, noteReferenceEndPosition))
                        );
                }

            this.templateCycleView.OutputTemplateSessions(templateSessionsAndNoteReferencePositions);
            this.templateCycleView.OutputTemplateSetNotes(templateSetNotesAndNoteReferencePositions);

            this.TemplateCycleDataPresented?.Invoke();
        }

        #endregion
    }
}
