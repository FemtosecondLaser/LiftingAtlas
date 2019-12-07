using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace LiftingAtlas.Standard
{
    public class XMLStreamBasedTemplateCycleProvider : IStreamBasedTemplateCycleProvider
    {
        public async Task<(CycleTemplateName CycleTemplateName, Lift TemplateLift)> CycleTemplateNameAndLiftAsync(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            CycleTemplateName cycleTemplateName = null;
            Lift? templateCycleLift = null;

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.Async = true;
            readerSettings.IgnoreWhitespace = true;

            using (XmlReader reader = XmlReader.Create(stream, readerSettings))
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    if (reader.IsStartElement())
                        switch (reader.Name)
                        {
                            case "Cycle":
                                if (cycleTemplateName == null)
                                    cycleTemplateName =
                                        new CycleTemplateName(
                                            reader.GetAttribute("CycleTemplateName")
                                            );
                                break;

                            case "Lifts":
                                if (templateCycleLift == null)
                                    using (XmlReader subReader = reader.ReadSubtree())
                                        templateCycleLift =
                                            await ReadLiftsAsync(subReader)
                                            .ConfigureAwait(false);
                                break;

                            default:
                                break;
                        }

                    if (cycleTemplateName != null && templateCycleLift != null)
                        break;
                }

            return (cycleTemplateName, templateCycleLift.Value);
        }

        public async Task<TemplateCycle<TemplateSession<TemplateSet>, TemplateSet>> TemplateCycleAsync(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            CycleTemplateName cycleTemplateName = null;
            Lift? templateCycleLift = null;
            List<TemplateSession<TemplateSet>> templateSessions = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle = null;

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.Async = true;
            readerSettings.IgnoreWhitespace = true;

            using (XmlReader reader = XmlReader.Create(stream, readerSettings))
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    if (reader.IsStartElement())
                        switch (reader.Name)
                        {
                            case "Cycle":
                                if (cycleTemplateName == null)
                                    cycleTemplateName =
                                        new CycleTemplateName(
                                            reader.GetAttribute("CycleTemplateName")
                                            );
                                break;

                            case "Lifts":
                                if (templateCycleLift == null)
                                    using (XmlReader subReader = reader.ReadSubtree())
                                        templateCycleLift =
                                            await ReadLiftsAsync(subReader)
                                            .ConfigureAwait(false);
                                break;

                            case "Sessions":
                                if (templateSessions == null)
                                    using (XmlReader subReader = reader.ReadSubtree())
                                        templateSessions =
                                            await ReadSessionsAsync(subReader)
                                            .ConfigureAwait(false);
                                break;

                            default:
                                break;
                        }

                    if (cycleTemplateName != null && templateCycleLift != null && templateSessions != null)
                        break;
                }

            templateCycle = new TemplateCycle<TemplateSession<TemplateSet>, TemplateSet>(
                cycleTemplateName,
                templateCycleLift.Value,
                templateSessions
                );

            return templateCycle;
        }

        private async Task<Lift> ReadLiftsAsync(XmlReader reader)
        {
            Lift templateCycleLifts = Lift.None;

            while (await reader.ReadAsync().ConfigureAwait(false))
                if (reader.IsStartElement() && reader.Name == "Lift")
                    using (XmlReader subReader = reader.ReadSubtree())
                        templateCycleLifts |=
                            await ReadElementContentAsLiftAsync(subReader)
                            .ConfigureAwait(false);
            return templateCycleLifts;
        }

        private async Task<Lift> ReadElementContentAsLiftAsync(XmlReader reader)
        {
            if (reader.ReadState == ReadState.Initial)
                await reader.ReadAsync().ConfigureAwait(false);

            switch (await reader.ReadElementContentAsStringAsync().ConfigureAwait(false))
            {
                case "Squat":
                    return Lift.Squat;

                case "Bench press":
                    return Lift.BenchPress;

                case "Deadlift":
                    return Lift.Deadlift;

                default:
                    return Lift.None;
            }
        }

        private async Task<List<TemplateSession<TemplateSet>>> ReadSessionsAsync(XmlReader reader)
        {
            List<TemplateSession<TemplateSet>> templateSessions = new List<TemplateSession<TemplateSet>>();

            while (await reader.ReadAsync().ConfigureAwait(false))
                if (reader.IsStartElement() && reader.Name == "Session")
                    using (XmlReader subReader = reader.ReadSubtree())
                        templateSessions.Add(await ReadSessionAsync(subReader).ConfigureAwait(false));

            return templateSessions;
        }

        private async Task<TemplateSession<TemplateSet>> ReadSessionAsync(XmlReader reader)
        {
            TemplateSession<TemplateSet> templateSession = null;
            SessionNumber number = null;
            List<TemplateSet> templateSets = null;

            while (await reader.ReadAsync().ConfigureAwait(false))
                if (reader.IsStartElement())
                    switch (reader.Name)
                    {
                        case "Number":
                            using (XmlReader subReader = reader.ReadSubtree())
                                number =
                                    new SessionNumber(
                                        await ReadElementContentAsIntAsync(subReader)
                                        .ConfigureAwait(false)
                                        );
                            break;

                        case "Sets":
                            using (XmlReader subReader = reader.ReadSubtree())
                                templateSets =
                                    await ReadSetsAsync(subReader)
                                    .ConfigureAwait(false);
                            break;

                        default:
                            break;
                    }

            templateSession = new TemplateSession<TemplateSet>(number, templateSets);

            return templateSession;
        }

        private async Task<List<TemplateSet>> ReadSetsAsync(XmlReader reader)
        {
            List<TemplateSet> templateSets = new List<TemplateSet>();

            while (await reader.ReadAsync().ConfigureAwait(false))
                if (reader.IsStartElement() && reader.Name == "Set")
                    using (XmlReader subReader = reader.ReadSubtree())
                        templateSets.Add(await ReadSetAsync(subReader).ConfigureAwait(false));

            return templateSets;
        }

        private async Task<TemplateSet> ReadSetAsync(XmlReader reader)
        {
            TemplateSet templateSet = null;
            SetNumber number = null;
            PlannedPercentageOfReferencePoint plannedPercentageOfReferencePoint = null;
            PlannedRepetitions plannedRepetitions = null;
            WeightAdjustmentConstant weightAdjustmentConstant = null;
            string note = null;

            while (await reader.ReadAsync().ConfigureAwait(false))
                if (reader.IsStartElement())
                    switch (reader.Name)
                    {
                        case "Number":
                            using (XmlReader subReader = reader.ReadSubtree())
                                number =
                                    new SetNumber(
                                        await ReadElementContentAsIntAsync(subReader)
                                        .ConfigureAwait(false)
                                        );
                            break;

                        case "PlannedPercentageOfReferencePoint":
                            using (XmlReader subReader = reader.ReadSubtree())
                                plannedPercentageOfReferencePoint =
                                    await ReadPlannedPercentageOfReferencePointAsync(subReader)
                                    .ConfigureAwait(false);
                            break;

                        case "PlannedRepetitions":
                            using (XmlReader subReader = reader.ReadSubtree())
                                plannedRepetitions =
                                    await ReadPlannedRepetitionsAsync(subReader)
                                    .ConfigureAwait(false);
                            break;

                        case "WeightAdjustmentConstant":
                            string weightAdjustmentConstantString;
                            using (XmlReader subReader = reader.ReadSubtree())
                                weightAdjustmentConstantString =
                                    await ReadElementContentAsStringAsync(subReader)
                                    .ConfigureAwait(false);
                            if (!string.IsNullOrEmpty(weightAdjustmentConstantString))
                                weightAdjustmentConstant =
                                    new WeightAdjustmentConstant(double.Parse(weightAdjustmentConstantString));
                            break;

                        case "Note":
                            string noteString;
                            using (XmlReader subReader = reader.ReadSubtree())
                                noteString =
                                    await ReadElementContentAsStringAsync(subReader)
                                    .ConfigureAwait(false);
                            if (!string.IsNullOrEmpty(noteString))
                                note = noteString;
                            break;

                        default:
                            break;
                    }

            templateSet = new TemplateSet(
                number,
                plannedPercentageOfReferencePoint,
                plannedRepetitions,
                weightAdjustmentConstant,
                note
                );

            return templateSet;
        }

        private async Task<PlannedPercentageOfReferencePoint> ReadPlannedPercentageOfReferencePointAsync(XmlReader reader)
        {
            PlannedPercentageOfReferencePoint plannedPercentageOfReferencePoint = null;
            int? lowerBound = null, upperBound = null;

            while (await reader.ReadAsync().ConfigureAwait(false))
                if (reader.IsStartElement())
                    switch (reader.Name)
                    {
                        case "LowerBound":
                            using (XmlReader subReader = reader.ReadSubtree())
                                lowerBound =
                                    await ReadElementContentAsIntAsync(subReader)
                                    .ConfigureAwait(false);
                            break;

                        case "UpperBound":
                            using (XmlReader subReader = reader.ReadSubtree())
                                upperBound =
                                    await ReadElementContentAsIntAsync(subReader)
                                    .ConfigureAwait(false);
                            break;

                        default:
                            break;
                    }

            if (lowerBound != null && upperBound != null)
                plannedPercentageOfReferencePoint =
                    new PlannedPercentageOfReferencePoint(lowerBound.Value, upperBound.Value);

            return plannedPercentageOfReferencePoint;
        }

        private async Task<PlannedRepetitions> ReadPlannedRepetitionsAsync(XmlReader reader)
        {
            PlannedRepetitions plannedRepetitions = null;
            int? lowerBound = null, upperBound = null;

            while (await reader.ReadAsync().ConfigureAwait(false))
                if (reader.IsStartElement())
                    switch (reader.Name)
                    {
                        case "LowerBound":
                            using (XmlReader subReader = reader.ReadSubtree())
                                lowerBound =
                                    await ReadElementContentAsIntAsync(subReader)
                                    .ConfigureAwait(false);
                            break;

                        case "UpperBound":
                            using (XmlReader subReader = reader.ReadSubtree())
                                upperBound =
                                    await ReadElementContentAsIntAsync(subReader)
                                    .ConfigureAwait(false);
                            break;

                        default:
                            break;
                    }

            if (lowerBound != null && upperBound != null)
                plannedRepetitions =
                    new PlannedRepetitions(
                        new Repetitions(lowerBound.Value),
                        new Repetitions(upperBound.Value)
                        );

            return plannedRepetitions;
        }

        private async Task<int> ReadElementContentAsIntAsync(XmlReader reader)
        {
            if (reader.ReadState == ReadState.Initial)
                await reader.ReadAsync()
                    .ConfigureAwait(false);

            return (int)await reader.ReadElementContentAsAsync(typeof(int), null)
                .ConfigureAwait(false);
        }

        private async Task<string> ReadElementContentAsStringAsync(XmlReader reader)
        {
            if (reader.ReadState == ReadState.Initial)
                await reader.ReadAsync()
                    .ConfigureAwait(false);

            return await reader.ReadElementContentAsStringAsync()
                .ConfigureAwait(false);
        }
    }
}
