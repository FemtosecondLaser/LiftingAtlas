using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace LiftingAtlas.Standard
{
    public class XMLStreamBasedTemplateCycleProvider : IStreamBasedTemplateCycleProvider
    {
        public (CycleTemplateName CycleTemplateName, Lift TemplateLift) CycleTemplateNameAndLift(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            CycleTemplateName cycleTemplateName = null;
            Lift? templateCycleLift = null;

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreWhitespace = true;

            using (XmlReader reader = XmlReader.Create(stream, readerSettings))
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                        switch (reader.Name)
                        {
                            case "Cycle":
                                if (cycleTemplateName == null)
                                    cycleTemplateName =
                                        new CycleTemplateName(reader.GetAttribute("CycleTemplateName"));
                                break;

                            case "Lifts":
                                if (templateCycleLift == null)
                                    using (XmlReader subReader = reader.ReadSubtree())
                                        templateCycleLift = ReadLifts(subReader);
                                break;

                            default:
                                break;
                        }

                    if (cycleTemplateName != null && templateCycleLift != null)
                        break;
                }

            return (cycleTemplateName, templateCycleLift.Value);
        }

        public TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> TemplateCycle(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            CycleTemplateName cycleTemplateName = null;
            Lift? templateCycleLift = null;
            List<TemplateSession<TemplateSet>> templateSessions = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle = null;

            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.IgnoreWhitespace = true;

            using (XmlReader reader = XmlReader.Create(stream, readerSettings))
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                        switch (reader.Name)
                        {
                            case "Cycle":
                                if (cycleTemplateName == null)
                                    cycleTemplateName =
                                        new CycleTemplateName(reader.GetAttribute("CycleTemplateName"));
                                break;

                            case "Lifts":
                                if (templateCycleLift == null)
                                    using (XmlReader subReader = reader.ReadSubtree())
                                        templateCycleLift = ReadLifts(subReader);
                                break;

                            case "Sessions":
                                if (templateSessions == null)
                                    using (XmlReader subReader = reader.ReadSubtree())
                                        templateSessions = ReadSessions(subReader);
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

        private Lift ReadLifts(XmlReader reader)
        {
            Lift templateCycleLifts = Lift.None;

            while (reader.Read())
                if (reader.IsStartElement() && reader.Name == "Lift")
                    using (XmlReader subReader = reader.ReadSubtree())
                        templateCycleLifts |= ReadElementContentAsLift(subReader);
            return templateCycleLifts;
        }

        private Lift ReadElementContentAsLift(XmlReader reader)
        {
            if (reader.ReadState == ReadState.Initial)
                reader.Read();

            switch (reader.ReadElementContentAsString())
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

        private List<TemplateSession<TemplateSet>> ReadSessions(XmlReader reader)
        {
            List<TemplateSession<TemplateSet>> templateSessions = new List<TemplateSession<TemplateSet>>();

            while (reader.Read())
                if (reader.IsStartElement() && reader.Name == "Session")
                    using (XmlReader subReader = reader.ReadSubtree())
                        templateSessions.Add(ReadSession(subReader));

            return templateSessions;
        }

        private TemplateSession<TemplateSet> ReadSession(XmlReader reader)
        {
            TemplateSession<TemplateSet> templateSession = null;
            SessionNumber number = null;
            List<TemplateSet> templateSets = null;

            while (reader.Read())
                if (reader.IsStartElement())
                    switch (reader.Name)
                    {
                        case "Number":
                            using (XmlReader subReader = reader.ReadSubtree())
                                number = new SessionNumber(ReadElementContentAsInt(subReader));
                            break;

                        case "Sets":
                            using (XmlReader subReader = reader.ReadSubtree())
                                templateSets = ReadSets(subReader);
                            break;

                        default:
                            break;
                    }

            templateSession = new TemplateSession<TemplateSet>(number, templateSets);

            return templateSession;
        }

        private List<TemplateSet> ReadSets(XmlReader reader)
        {
            List<TemplateSet> templateSets = new List<TemplateSet>();

            while (reader.Read())
                if (reader.IsStartElement() && reader.Name == "Set")
                    using (XmlReader subReader = reader.ReadSubtree())
                        templateSets.Add(ReadSet(subReader));

            return templateSets;
        }

        private TemplateSet ReadSet(XmlReader reader)
        {
            TemplateSet templateSet = null;
            SetNumber number = null;
            PlannedPercentageOfReferencePoint plannedPercentageOfReferencePoint = null;
            PlannedRepetitions plannedRepetitions = null;
            WeightAdjustmentConstant weightAdjustmentConstant = null;
            string note = null;

            while (reader.Read())
                if (reader.IsStartElement())
                    switch (reader.Name)
                    {
                        case "Number":
                            using (XmlReader subReader = reader.ReadSubtree())
                                number = new SetNumber(ReadElementContentAsInt(subReader));
                            break;

                        case "PlannedPercentageOfReferencePoint":
                            using (XmlReader subReader = reader.ReadSubtree())
                                plannedPercentageOfReferencePoint = ReadPlannedPercentageOfReferencePoint(subReader);
                            break;

                        case "PlannedRepetitions":
                            using (XmlReader subReader = reader.ReadSubtree())
                                plannedRepetitions = ReadPlannedRepetitions(subReader);
                            break;

                        case "WeightAdjustmentConstant":
                            string weightAdjustmentConstantString;
                            using (XmlReader subReader = reader.ReadSubtree())
                                weightAdjustmentConstantString = ReadElementContentAsString(subReader);
                            if (!string.IsNullOrEmpty(weightAdjustmentConstantString))
                                weightAdjustmentConstant =
                                    new WeightAdjustmentConstant(double.Parse(weightAdjustmentConstantString));
                            break;

                        case "Note":
                            string noteString;
                            using (XmlReader subReader = reader.ReadSubtree())
                                noteString = ReadElementContentAsString(subReader);
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

        private PlannedPercentageOfReferencePoint ReadPlannedPercentageOfReferencePoint(XmlReader reader)
        {
            PlannedPercentageOfReferencePoint plannedPercentageOfReferencePoint = null;
            int? lowerBound = null, upperBound = null;

            while (reader.Read())
                if (reader.IsStartElement())
                    switch (reader.Name)
                    {
                        case "LowerBound":
                            using (XmlReader subReader = reader.ReadSubtree())
                                lowerBound = ReadElementContentAsInt(subReader);
                            break;

                        case "UpperBound":
                            using (XmlReader subReader = reader.ReadSubtree())
                                upperBound = ReadElementContentAsInt(subReader);
                            break;

                        default:
                            break;
                    }

            if (lowerBound != null && upperBound != null)
                plannedPercentageOfReferencePoint =
                    new PlannedPercentageOfReferencePoint(lowerBound.Value, upperBound.Value);

            return plannedPercentageOfReferencePoint;
        }

        private PlannedRepetitions ReadPlannedRepetitions(XmlReader reader)
        {
            PlannedRepetitions plannedRepetitions = null;
            int? lowerBound = null, upperBound = null;

            while (reader.Read())
                if (reader.IsStartElement())
                    switch (reader.Name)
                    {
                        case "LowerBound":
                            using (XmlReader subReader = reader.ReadSubtree())
                                lowerBound = ReadElementContentAsInt(subReader);
                            break;

                        case "UpperBound":
                            using (XmlReader subReader = reader.ReadSubtree())
                                upperBound = ReadElementContentAsInt(subReader);
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

        private int ReadElementContentAsInt(XmlReader reader)
        {
            if (reader.ReadState == ReadState.Initial)
                reader.Read();

            return reader.ReadElementContentAsInt();
        }

        private string ReadElementContentAsString(XmlReader reader)
        {
            if (reader.ReadState == ReadState.Initial)
                reader.Read();

            return reader.ReadElementContentAsString();
        }
    }
}
