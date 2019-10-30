using System;
using System.Collections.Generic;

namespace LiftingAtlas.Standard.Tests
{
    public static class REFERENCECYCLESB
    {
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

        public static (CycleTemplateName CycleTemplateName, Lift TemplateLift) CycleTemplateNameAndLift()
        {
            return (new CycleTemplateName("REFERENCECYCLESB"), Lift.Squat | Lift.BenchPress);
        }

        public static TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> TemplateCycle()
        {
            Lift lift = Lift.Squat | Lift.BenchPress;

            TemplateSet cycleS01S01 =
                new TemplateSet(new SetNumber(1), new PlannedPercentageOfReferencePoint(20, 20), new PlannedRepetitions(new Repetitions(8), new Repetitions(8)));
            TemplateSet cycleS01S02 =
                new TemplateSet(new SetNumber(2), new PlannedPercentageOfReferencePoint(30, 30), new PlannedRepetitions(new Repetitions(7), new Repetitions(7)));
            TemplateSet cycleS01S03 =
                new TemplateSet(new SetNumber(3), new PlannedPercentageOfReferencePoint(40, 40), new PlannedRepetitions(new Repetitions(6), new Repetitions(6)));
            TemplateSet cycleS01S04 =
                new TemplateSet(new SetNumber(4), new PlannedPercentageOfReferencePoint(50, 50), new PlannedRepetitions(new Repetitions(5), new Repetitions(5)));
            TemplateSet cycleS01S05 =
                new TemplateSet(new SetNumber(5), new PlannedPercentageOfReferencePoint(60, 60), new PlannedRepetitions(new Repetitions(4), new Repetitions(4)));
            TemplateSet cycleS01S06 =
                new TemplateSet(new SetNumber(6), new PlannedPercentageOfReferencePoint(70, 70), new PlannedRepetitions(new Repetitions(4), new Repetitions(4)));
            TemplateSet cycleS01S07 =
                new TemplateSet(new SetNumber(7), new PlannedPercentageOfReferencePoint(70, 70), new PlannedRepetitions(new Repetitions(4), new Repetitions(4)));
            TemplateSet cycleS01S08 =
                new TemplateSet(new SetNumber(8), new PlannedPercentageOfReferencePoint(70, 70), new PlannedRepetitions(new Repetitions(4), new Repetitions(4)));
            List<TemplateSet> cycleS01SList = new List<TemplateSet>();
            cycleS01SList.Add(cycleS01S01);
            cycleS01SList.Add(cycleS01S02);
            cycleS01SList.Add(cycleS01S03);
            cycleS01SList.Add(cycleS01S04);
            cycleS01SList.Add(cycleS01S05);
            cycleS01SList.Add(cycleS01S06);
            cycleS01SList.Add(cycleS01S07);
            cycleS01SList.Add(cycleS01S08);
            TemplateSession<TemplateSet> cycleS01 = new TemplateSession<TemplateSet>(new SessionNumber(1), cycleS01SList);

            TemplateSet cycleS02S01 =
                new TemplateSet(new SetNumber(1), new PlannedPercentageOfReferencePoint(20, 20), new PlannedRepetitions(new Repetitions(8), new Repetitions(8)));
            TemplateSet cycleS02S02 =
                new TemplateSet(new SetNumber(2), new PlannedPercentageOfReferencePoint(40, 40), new PlannedRepetitions(new Repetitions(6), new Repetitions(6)));
            TemplateSet cycleS02S03 =
                new TemplateSet(new SetNumber(3), new PlannedPercentageOfReferencePoint(60, 60), new PlannedRepetitions(new Repetitions(4), new Repetitions(4)));
            TemplateSet cycleS02S04 =
                new TemplateSet(new SetNumber(4), new PlannedPercentageOfReferencePoint(70, 70), new PlannedRepetitions(new Repetitions(3), new Repetitions(3)));
            TemplateSet cycleS02S05 =
                new TemplateSet(new SetNumber(5), new PlannedPercentageOfReferencePoint(80, 80), new PlannedRepetitions(new Repetitions(2), new Repetitions(2)));
            TemplateSet cycleS02S06 =
                new TemplateSet(new SetNumber(6), new PlannedPercentageOfReferencePoint(85, 85), new PlannedRepetitions(new Repetitions(1), new Repetitions(1)));
            TemplateSet cycleS02S07 =
                new TemplateSet(new SetNumber(7), new PlannedPercentageOfReferencePoint(90, 90), new PlannedRepetitions(new Repetitions(1), new Repetitions(1)));
            TemplateSet cycleS02S08 =
                new TemplateSet(new SetNumber(8), new PlannedPercentageOfReferencePoint(95, 95), new PlannedRepetitions(new Repetitions(1), new Repetitions(1)));
            TemplateSet cycleS02S09 =
                new TemplateSet(new SetNumber(9), new PlannedPercentageOfReferencePoint(100, 100), new PlannedRepetitions(new Repetitions(1), new Repetitions(1)));
            TemplateSet cycleS02S10 =
                new TemplateSet(new SetNumber(10), null, new PlannedRepetitions(new Repetitions(1), new Repetitions(1)), note: "Maximum");
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
            TemplateSession<TemplateSet> cycleS02 = new TemplateSession<TemplateSet>(new SessionNumber(2), cycleS02SList);

            List<TemplateSession<TemplateSet>> cycleSessions = new List<TemplateSession<TemplateSet>>();
            cycleSessions.Add(cycleS01);
            cycleSessions.Add(cycleS02);

            return new TemplateCycle<TemplateSession<TemplateSet>, TemplateSet>(new CycleTemplateName("REFERENCECYCLESB"), lift, cycleSessions);
        }

        public static PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> PlannedCycle(
            IGuidProvider guidProvider,
            Lift lift,
            Weight referencePoint,
            IQuantizationProvider quantizationProvider
            )
        {
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle = TemplateCycle();

            if (guidProvider == null)
                throw new ArgumentNullException(nameof(guidProvider));

            if (lift == Lift.None)
                throw new ArgumentException("Unspecified lift.", nameof(lift));

            if (!templateCycle.TemplateLift.HasFlag(lift))
                throw new ArgumentException("Lift is not the lift the cycle template is designed for.", nameof(lift));

            if (referencePoint == null)
                throw new ArgumentNullException(nameof(referencePoint));

            List<PlannedSession<PlannedSet>> plannedSessions = new List<PlannedSession<PlannedSet>>(templateCycle.Sessions.Count);

            foreach (TemplateSession<TemplateSet> templateSession in templateCycle.Sessions)
            {
                List<PlannedSet> plannedSetList = new List<PlannedSet>(templateSession.Sets.Count);

                foreach (TemplateSet templateSet in templateSession.Sets)
                {
                    PlannedWeight plannedWeight;

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

                        plannedWeight = new PlannedWeight(new Weight(plannedWeightLowerBound), new Weight(plannedWeightUpperBound));
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
