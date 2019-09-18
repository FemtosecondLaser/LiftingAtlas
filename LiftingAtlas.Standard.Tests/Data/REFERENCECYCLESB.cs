using System;
using System.Collections.Generic;

namespace LiftingAtlas.Standard.Tests
{
    /// <summary>
    /// Class containing REFERENCECYCLESB cycle data.
    /// The cycle is designed for squat and bench press.
    /// </summary>
    public static class REFERENCECYCLESB
    {
        /// <summary>
        /// Template cycle XML string.
        /// </summary>
        /// <returns>Template cycle XML string.</returns>
        public static string XML()
        {
            return
                @"
                <?xml version='1.0' encoding='utf-8'?>
                <Cycle CycleTemplateName='REFERENCECYCLESB'>
                  <Lifts>
                    <Lift>Squat</Lift>
                    <Lift>Bench press</Lift>
                  </Lifts>
                  <Sessions>
                    <Session>
                      <Number>1</Number>
                      <Sets>
                        <Set>
                          <Number>1</Number>
                          <PlannedPercentageOfReferencePoint>
                            <LowerBound>20</LowerBound>
                            <UpperBound>20</UpperBound>
                          </PlannedPercentageOfReferencePoint>
                          <PlannedRepetitions>
                            <LowerBound>8</LowerBound>
                            <UpperBound>8</UpperBound>
                          </PlannedRepetitions>
                          <WeightAdjustmentConstant />
                          <Note />
                        </Set>
                        <Set>
                          <Number>2</Number>
                          <PlannedPercentageOfReferencePoint>
                            <LowerBound>30</LowerBound>
                            <UpperBound>30</UpperBound>
                          </PlannedPercentageOfReferencePoint>
                          <PlannedRepetitions>
                            <LowerBound>7</LowerBound>
                            <UpperBound>7</UpperBound>
                          </PlannedRepetitions>
                          <WeightAdjustmentConstant />
                          <Note />
                        </Set>
                        <Set>
                          <Number>3</Number>
                          <PlannedPercentageOfReferencePoint>
                            <LowerBound>40</LowerBound>
                            <UpperBound>40</UpperBound>
                          </PlannedPercentageOfReferencePoint>
                          <PlannedRepetitions>
                            <LowerBound>6</LowerBound>
                            <UpperBound>6</UpperBound>
                          </PlannedRepetitions>
                          <WeightAdjustmentConstant />
                          <Note />
                        </Set>
                        <Set>
                          <Number>4</Number>
                          <PlannedPercentageOfReferencePoint>
                            <LowerBound>50</LowerBound>
                            <UpperBound>50</UpperBound>
                          </PlannedPercentageOfReferencePoint>
                          <PlannedRepetitions>
                            <LowerBound>5</LowerBound>
                            <UpperBound>5</UpperBound>
                          </PlannedRepetitions>
                          <WeightAdjustmentConstant />
                          <Note />
                        </Set>
                        <Set>
                          <Number>5</Number>
                          <PlannedPercentageOfReferencePoint>
                            <LowerBound>60</LowerBound>
                            <UpperBound>60</UpperBound>
                          </PlannedPercentageOfReferencePoint>
                          <PlannedRepetitions>
                            <LowerBound>4</LowerBound>
                            <UpperBound>4</UpperBound>
                          </PlannedRepetitions>
                          <WeightAdjustmentConstant />
                          <Note />
                        </Set>
                        <Set>
                          <Number>6</Number>
                          <PlannedPercentageOfReferencePoint>
                            <LowerBound>70</LowerBound>
                            <UpperBound>70</UpperBound>
                          </PlannedPercentageOfReferencePoint>
                          <PlannedRepetitions>
                            <LowerBound>4</LowerBound>
                            <UpperBound>4</UpperBound>
                          </PlannedRepetitions>
                          <WeightAdjustmentConstant />
                          <Note />
                        </Set>
                        <Set>
                          <Number>7</Number>
                          <PlannedPercentageOfReferencePoint>
                            <LowerBound>70</LowerBound>
                            <UpperBound>70</UpperBound>
                          </PlannedPercentageOfReferencePoint>
                          <PlannedRepetitions>
                            <LowerBound>4</LowerBound>
                            <UpperBound>4</UpperBound>
                          </PlannedRepetitions>
                          <WeightAdjustmentConstant />
                          <Note />
                        </Set>
                        <Set>
                          <Number>8</Number>
                          <PlannedPercentageOfReferencePoint>
                            <LowerBound>70</LowerBound>
                            <UpperBound>70</UpperBound>
                          </PlannedPercentageOfReferencePoint>
                          <PlannedRepetitions>
                            <LowerBound>4</LowerBound>
                            <UpperBound>4</UpperBound>
                          </PlannedRepetitions>
                          <WeightAdjustmentConstant />
                          <Note />
                        </Set>
                      </Sets>
                    </Session>
                    <Session>
                      <Number>2</Number>
                      <Sets>
                        <Set>
                          <Number>1</Number>
                          <PlannedPercentageOfReferencePoint>
                            <LowerBound>20</LowerBound>
                            <UpperBound>20</UpperBound>
                          </PlannedPercentageOfReferencePoint>
                          <PlannedRepetitions>
                            <LowerBound>8</LowerBound>
                            <UpperBound>8</UpperBound>
                          </PlannedRepetitions>
                          <WeightAdjustmentConstant />
                          <Note />
                        </Set>
                        <Set>
                          <Number>2</Number>
                          <PlannedPercentageOfReferencePoint>
                            <LowerBound>40</LowerBound>
                            <UpperBound>40</UpperBound>
                          </PlannedPercentageOfReferencePoint>
                          <PlannedRepetitions>
                            <LowerBound>6</LowerBound>
                            <UpperBound>6</UpperBound>
                          </PlannedRepetitions>
                          <WeightAdjustmentConstant />
                          <Note />
                        </Set>
                        <Set>
                          <Number>3</Number>
                          <PlannedPercentageOfReferencePoint>
                            <LowerBound>60</LowerBound>
                            <UpperBound>60</UpperBound>
                          </PlannedPercentageOfReferencePoint>
                          <PlannedRepetitions>
                            <LowerBound>4</LowerBound>
                            <UpperBound>4</UpperBound>
                          </PlannedRepetitions>
                          <WeightAdjustmentConstant />
                          <Note />
                        </Set>
                        <Set>
                          <Number>4</Number>
                          <PlannedPercentageOfReferencePoint>
                            <LowerBound>70</LowerBound>
                            <UpperBound>70</UpperBound>
                          </PlannedPercentageOfReferencePoint>
                          <PlannedRepetitions>
                            <LowerBound>3</LowerBound>
                            <UpperBound>3</UpperBound>
                          </PlannedRepetitions>
                          <WeightAdjustmentConstant />
                          <Note />
                        </Set>
                        <Set>
                          <Number>5</Number>
                          <PlannedPercentageOfReferencePoint>
                            <LowerBound>80</LowerBound>
                            <UpperBound>80</UpperBound>
                          </PlannedPercentageOfReferencePoint>
                          <PlannedRepetitions>
                            <LowerBound>2</LowerBound>
                            <UpperBound>2</UpperBound>
                          </PlannedRepetitions>
                          <WeightAdjustmentConstant />
                          <Note />
                        </Set>
                        <Set>
                          <Number>6</Number>
                          <PlannedPercentageOfReferencePoint>
                            <LowerBound>85</LowerBound>
                            <UpperBound>85</UpperBound>
                          </PlannedPercentageOfReferencePoint>
                          <PlannedRepetitions>
                            <LowerBound>1</LowerBound>
                            <UpperBound>1</UpperBound>
                          </PlannedRepetitions>
                          <WeightAdjustmentConstant />
                          <Note />
                        </Set>
                        <Set>
                          <Number>7</Number>
                          <PlannedPercentageOfReferencePoint>
                            <LowerBound>90</LowerBound>
                            <UpperBound>90</UpperBound>
                          </PlannedPercentageOfReferencePoint>
                          <PlannedRepetitions>
                            <LowerBound>1</LowerBound>
                            <UpperBound>1</UpperBound>
                          </PlannedRepetitions>
                          <WeightAdjustmentConstant />
                          <Note />
                        </Set>
                        <Set>
                          <Number>8</Number>
                          <PlannedPercentageOfReferencePoint>
                            <LowerBound>95</LowerBound>
                            <UpperBound>95</UpperBound>
                          </PlannedPercentageOfReferencePoint>
                          <PlannedRepetitions>
                            <LowerBound>1</LowerBound>
                            <UpperBound>1</UpperBound>
                          </PlannedRepetitions>
                          <WeightAdjustmentConstant />
                          <Note />
                        </Set>
                        <Set>
                          <Number>9</Number>
                          <PlannedPercentageOfReferencePoint>
                            <LowerBound>100</LowerBound>
                            <UpperBound>100</UpperBound>
                          </PlannedPercentageOfReferencePoint>
                          <PlannedRepetitions>
                            <LowerBound>1</LowerBound>
                            <UpperBound>1</UpperBound>
                          </PlannedRepetitions>
                          <WeightAdjustmentConstant />
                          <Note />
                        </Set>
                        <Set>
                          <Number>10</Number>
                          <PlannedPercentageOfReferencePoint />
                          <PlannedRepetitions>
                            <LowerBound>1</LowerBound>
                            <UpperBound>1</UpperBound>
                          </PlannedRepetitions>
                          <WeightAdjustmentConstant />
                          <Note>Maximum</Note>
                        </Set>
                      </Sets>
                    </Session>
                  </Sessions>
                </Cycle>
                ".Trim();
        }

        /// <summary>
        /// Cycle template name and lift.
        /// </summary>
        /// <returns>Cycle template name and lift.</returns>
        public static (string CycleTemplateName, Lift TemplateLift) CycleTemplateNameAndLift()
        {
            return ("REFERENCECYCLESB", Lift.Squat | Lift.BenchPress);
        }

        /// <summary>
        /// Template cycle.
        /// </summary>
        /// <returns>Template cycle.</returns>
        public static TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> TemplateCycle()
        {
            Lift lift = Lift.Squat | Lift.BenchPress;

            TemplateSet cycleS01S01 = new TemplateSet(1, new NonNegativeI32Range(20, 20), new NonNegativeI32Range(8, 8));
            TemplateSet cycleS01S02 = new TemplateSet(2, new NonNegativeI32Range(30, 30), new NonNegativeI32Range(7, 7));
            TemplateSet cycleS01S03 = new TemplateSet(3, new NonNegativeI32Range(40, 40), new NonNegativeI32Range(6, 6));
            TemplateSet cycleS01S04 = new TemplateSet(4, new NonNegativeI32Range(50, 50), new NonNegativeI32Range(5, 5));
            TemplateSet cycleS01S05 = new TemplateSet(5, new NonNegativeI32Range(60, 60), new NonNegativeI32Range(4, 4));
            TemplateSet cycleS01S06 = new TemplateSet(6, new NonNegativeI32Range(70, 70), new NonNegativeI32Range(4, 4));
            TemplateSet cycleS01S07 = new TemplateSet(7, new NonNegativeI32Range(70, 70), new NonNegativeI32Range(4, 4));
            TemplateSet cycleS01S08 = new TemplateSet(8, new NonNegativeI32Range(70, 70), new NonNegativeI32Range(4, 4));
            List<TemplateSet> cycleS01SList = new List<TemplateSet>();
            cycleS01SList.Add(cycleS01S01);
            cycleS01SList.Add(cycleS01S02);
            cycleS01SList.Add(cycleS01S03);
            cycleS01SList.Add(cycleS01S04);
            cycleS01SList.Add(cycleS01S05);
            cycleS01SList.Add(cycleS01S06);
            cycleS01SList.Add(cycleS01S07);
            cycleS01SList.Add(cycleS01S08);
            TemplateSession<TemplateSet> cycleS01 = new TemplateSession<TemplateSet>(1, cycleS01SList);

            TemplateSet cycleS02S01 = new TemplateSet(1, new NonNegativeI32Range(20, 20), new NonNegativeI32Range(8, 8));
            TemplateSet cycleS02S02 = new TemplateSet(2, new NonNegativeI32Range(40, 40), new NonNegativeI32Range(6, 6));
            TemplateSet cycleS02S03 = new TemplateSet(3, new NonNegativeI32Range(60, 60), new NonNegativeI32Range(4, 4));
            TemplateSet cycleS02S04 = new TemplateSet(4, new NonNegativeI32Range(70, 70), new NonNegativeI32Range(3, 3));
            TemplateSet cycleS02S05 = new TemplateSet(5, new NonNegativeI32Range(80, 80), new NonNegativeI32Range(2, 2));
            TemplateSet cycleS02S06 = new TemplateSet(6, new NonNegativeI32Range(85, 85), new NonNegativeI32Range(1, 1));
            TemplateSet cycleS02S07 = new TemplateSet(7, new NonNegativeI32Range(90, 90), new NonNegativeI32Range(1, 1));
            TemplateSet cycleS02S08 = new TemplateSet(8, new NonNegativeI32Range(95, 95), new NonNegativeI32Range(1, 1));
            TemplateSet cycleS02S09 = new TemplateSet(9, new NonNegativeI32Range(100, 100), new NonNegativeI32Range(1, 1));
            TemplateSet cycleS02S10 = new TemplateSet(10, null, new NonNegativeI32Range(1, 1), note: "Maximum");
            List<TemplateSet> cycleS02SList = new List<TemplateSet>();
            cycleS02SList.Add(cycleS02S01);
            cycleS02SList.Add(cycleS02S02);
            cycleS02SList.Add(cycleS02S03);
            cycleS02SList.Add(cycleS02S04);
            cycleS02SList.Add(cycleS02S05);
            cycleS02SList.Add(cycleS02S06);
            cycleS02SList.Add(cycleS02S07);
            cycleS02SList.Add(cycleS02S08);
            cycleS02SList.Add(cycleS02S09);
            cycleS02SList.Add(cycleS02S10);
            TemplateSession<TemplateSet> cycleS02 = new TemplateSession<TemplateSet>(2, cycleS02SList);

            List<TemplateSession<TemplateSet>> cycleSessions = new List<TemplateSession<TemplateSet>>();
            cycleSessions.Add(cycleS01);
            cycleSessions.Add(cycleS02);

            return new TemplateCycle<TemplateSession<TemplateSet>, TemplateSet>("REFERENCECYCLESB", lift, cycleSessions);
        }

        /// <summary>
        /// Planned cycle.
        /// </summary>
        /// <param name="guidProvider">Guid provider. Must not be null.</param>
        /// <param name="lift">Lift to plan a cycle for. Must be specified.
        /// Must be the lift the cycle template is designed for.</param>
        /// <param name="referencePoint">Reference point used to plan a cycle. Must not be less than 0.</param>
        /// <param name="quantizationProvider">Provider of quantization method for weight planning.</param>
        /// <returns>Planned cycle.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="guidProvider"/> is null.
        /// <exception cref="ArgumentException"><paramref name="lift"/> is unspecified or
        /// <paramref name="lift"/> is not the lift the cycle template is designed for.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="referencePoint"/> is less than 0.</exception>
        public static PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> PlannedCycle(
            IGuidProvider guidProvider,
            Lift lift,
            double referencePoint,
            IQuantizationProvider quantizationProvider
            )
        {
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle = TemplateCycle();

            if (guidProvider == null)
                throw new ArgumentNullException(nameof(guidProvider));

            if (lift == Lift.None)
                throw new ArgumentException($"Unspecified {nameof(lift)}.");

            if (!templateCycle.TemplateLift.HasFlag(lift))
                throw new ArgumentException($"{nameof(lift)} is not the lift the cycle template is designed for.");

            if (referencePoint < 0.00)
                throw new ArgumentOutOfRangeException(nameof(referencePoint));

            List<PlannedSession<PlannedSet>> plannedSessions = new List<PlannedSession<PlannedSet>>(templateCycle.Sessions.Count);

            foreach (TemplateSession<TemplateSet> templateSession in templateCycle.Sessions)
            {
                List<PlannedSet> plannedSetList = new List<PlannedSet>(templateSession.Sets.Count);

                foreach (TemplateSet templateSet in templateSession.Sets)
                {
                    NonNegativeDBLRange plannedWeight;

                    if (templateSet.PlannedPercentageOfReferencePoint == null)
                    {
                        plannedWeight = null;
                    }
                    else
                    {
                        double plannedWeightLowerBound, plannedWeightUpperBound;

                        plannedWeightLowerBound = templateSet.PlannedPercentageOfReferencePoint.LowerBound / 100.00 * referencePoint;
                        plannedWeightUpperBound = templateSet.PlannedPercentageOfReferencePoint.UpperBound / 100.00 * referencePoint;

                        if (templateSet.WeightAdjustmentConstant != null)
                        {
                            plannedWeightLowerBound += templateSet.WeightAdjustmentConstant.Value;
                            plannedWeightUpperBound += templateSet.WeightAdjustmentConstant.Value;
                        }

                        if (quantizationProvider != null)
                        {
                            plannedWeightLowerBound = quantizationProvider.Quantize(plannedWeightLowerBound);
                            plannedWeightUpperBound = quantizationProvider.Quantize(plannedWeightUpperBound);
                        }

                        plannedWeight = new NonNegativeDBLRange(plannedWeightLowerBound, plannedWeightUpperBound);
                    }

                    PlannedSet plannedSet =
                        new PlannedSet(
                            templateSet.Number,
                            templateSet.PlannedPercentageOfReferencePoint,
                            templateSet.PlannedRepetitions,
                            plannedWeight,
                            null,
                            templateSet.WeightAdjustmentConstant,
                            templateSet.Note
                            );

                    plannedSetList.Add(plannedSet);
                }

                plannedSessions.Add(new PlannedSession<PlannedSet>(templateSession.Number, plannedSetList));
            }

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> plannedCycle =
                new PlannedCycle<PlannedSession<PlannedSet>, PlannedSet>(
                    guidProvider.GetGuid(),
                    lift,
                    referencePoint,
                    templateCycle.CycleTemplateName,
                    templateCycle.TemplateLift,
                    plannedSessions
                    );

            return plannedCycle;
        }
    }
}
