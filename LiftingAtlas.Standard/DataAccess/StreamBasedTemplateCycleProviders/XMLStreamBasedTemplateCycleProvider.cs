using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// XML stream-based template cycle provider.
    /// </summary>
    public class XMLStreamBasedTemplateCycleProvider : IStreamBasedTemplateCycleProvider
    {
        /// <summary>
        /// Reads cycle template name and lift.
        /// </summary>
        /// <param name="stream">XML stream representing cycle template. Must not be null.</param>
        /// <returns>Cycle template name and lift.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
        public (string CycleTemplateName, Lift TemplateLift) CycleTemplateNameAndLift(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            string cycleTemplateName = null;
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
                                    cycleTemplateName = reader.GetAttribute("CycleTemplateName");
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

        /// <summary>
        /// Reads template cycle.
        /// </summary>
        /// <param name="stream">XML stream representing cycle template. Must not be null.</param>
        /// <returns>Template cycle.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
        public TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> TemplateCycle(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            string cycleTemplateName = null;
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
                                    cycleTemplateName = reader.GetAttribute("CycleTemplateName");
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

        /// <summary>
        /// Reads Lifts section of XML template cycle.
        /// </summary>
        /// <param name="reader">Reader of Lifts section of XML template cycle.</param>
        /// <returns>Lifts the template cycle is designed for.</returns>
        private Lift ReadLifts(XmlReader reader)
        {
            Lift templateCycleLifts = Lift.None;

            while (reader.Read())
                if (reader.IsStartElement() && reader.Name == "Lift")
                    using (XmlReader subReader = reader.ReadSubtree())
                        templateCycleLifts |= ReadElementContentAsLift(subReader);
            return templateCycleLifts;
        }

        /// <summary>
        /// Reads content of an element and returns it as Lift.
        /// </summary>
        /// <param name="reader">Reader of content.</param>
        /// <returns>Content of an element as Lift.</returns>
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

        /// <summary>
        /// Reads Sessions section of XML template cycle.
        /// </summary>
        /// <param name="reader">Reader of Sessions section of XML template cycle.</param>
        /// <returns>List of template sessions.</returns>
        private List<TemplateSession<TemplateSet>> ReadSessions(XmlReader reader)
        {
            List<TemplateSession<TemplateSet>> templateSessions = new List<TemplateSession<TemplateSet>>();

            while (reader.Read())
                if (reader.IsStartElement() && reader.Name == "Session")
                    using (XmlReader subReader = reader.ReadSubtree())
                        templateSessions.Add(ReadSession(subReader));

            return templateSessions;
        }

        /// <summary>
        /// Reads Session section of XML template cycle.
        /// </summary>
        /// <param name="reader">Reader of Session section of XML template cycle.</param>
        /// <returns>Template session.</returns>
        private TemplateSession<TemplateSet> ReadSession(XmlReader reader)
        {
            TemplateSession<TemplateSet> templateSession = null;
            int? number = null;
            List<TemplateSet> templateSets = null;

            while (reader.Read())
                if (reader.IsStartElement())
                    switch (reader.Name)
                    {
                        case "Number":
                            using (XmlReader subReader = reader.ReadSubtree())
                                number = ReadElementContentAsInt(subReader);
                            break;

                        case "Sets":
                            using (XmlReader subReader = reader.ReadSubtree())
                                templateSets = ReadSets(subReader);
                            break;

                        default:
                            break;
                    }

            templateSession = new TemplateSession<TemplateSet>(number.Value, templateSets);

            return templateSession;
        }

        /// <summary>
        /// Reads Sets section of XML template cycle.
        /// </summary>
        /// <param name="reader">Reader of Sets section of XML template cycle.</param>
        /// <returns>List of template sets.</returns>
        private List<TemplateSet> ReadSets(XmlReader reader)
        {
            List<TemplateSet> templateSets = new List<TemplateSet>();

            while (reader.Read())
                if (reader.IsStartElement() && reader.Name == "Set")
                    using (XmlReader subReader = reader.ReadSubtree())
                        templateSets.Add(ReadSet(subReader));

            return templateSets;
        }

        /// <summary>
        /// Reads Set section of XML template cycle.
        /// </summary>
        /// <param name="reader">Reader of Set section of XML template cycle.</param>
        /// <returns>Template set.</returns>
        private TemplateSet ReadSet(XmlReader reader)
        {
            TemplateSet templateSet = null;
            int? number = null;
            NonNegativeI32Range plannedPercentageOfReferencePoint = null, plannedRepetitions = null;
            double? weightAdjustmentConstant = null;
            string note = null;

            while (reader.Read())
                if (reader.IsStartElement())
                    switch (reader.Name)
                    {
                        case "Number":
                            using (XmlReader subReader = reader.ReadSubtree())
                                number = ReadElementContentAsInt(subReader);
                            break;

                        case "PlannedPercentageOfReferencePoint":
                            using (XmlReader subReader = reader.ReadSubtree())
                                plannedPercentageOfReferencePoint = ReadRange(subReader);
                            break;

                        case "PlannedRepetitions":
                            using (XmlReader subReader = reader.ReadSubtree())
                                plannedRepetitions = ReadRange(subReader);
                            break;

                        case "WeightAdjustmentConstant":
                            string weightAdjustmentConstantString;
                            using (XmlReader subReader = reader.ReadSubtree())
                                weightAdjustmentConstantString = ReadElementContentAsString(subReader);
                            if (!string.IsNullOrEmpty(weightAdjustmentConstantString))
                                weightAdjustmentConstant = double.Parse(weightAdjustmentConstantString);
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
                number.Value,
                plannedPercentageOfReferencePoint,
                plannedRepetitions,
                weightAdjustmentConstant,
                note
                );

            return templateSet;
        }

        /// <summary>
        /// Reads range representing section of XML template cycle.
        /// </summary>
        /// <param name="reader">Reader of range representing section of XML template cycle.</param>
        /// <returns>Non-negative 32-bit integer range.</returns>
        private NonNegativeI32Range ReadRange(XmlReader reader)
        {
            NonNegativeI32Range range = null;
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
                range = new NonNegativeI32Range(lowerBound.Value, upperBound.Value);

            return range;
        }

        /// <summary>
        /// Reads content of an element and returns it as 32-bit signed integer.
        /// </summary>
        /// <param name="reader">Reader of content.</param>
        /// <returns>Content of an element as 32-bit signed integer.</returns>
        private int ReadElementContentAsInt(XmlReader reader)
        {
            if (reader.ReadState == ReadState.Initial)
                reader.Read();

            return reader.ReadElementContentAsInt();
        }

        /// <summary>
        /// Reads content of an element and returns it as string.
        /// </summary>
        /// <param name="reader">Reader of content.</param>
        /// <returns>Content of an element as string.</returns>
        private string ReadElementContentAsString(XmlReader reader)
        {
            if (reader.ReadState == ReadState.Initial)
                reader.Read();

            return reader.ReadElementContentAsString();
        }
    }
}
