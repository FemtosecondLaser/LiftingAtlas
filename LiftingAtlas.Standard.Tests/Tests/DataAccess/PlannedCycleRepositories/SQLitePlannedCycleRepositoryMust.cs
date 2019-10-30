using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LiftingAtlas.Standard.Tests
{
    [TestFixture]
    public class SQLitePlannedCycleRepositoryMust
    {
        public IGuidProvider guidProvider;
        public SQLitePlannedCycleRepository sQLitePlannedCycleRepository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            guidProvider = new HexFirstCharFRestZerosGuidProvider();
        }

        [SetUp]
        public void SetUp()
        {
            sQLitePlannedCycleRepository =
                new SQLitePlannedCycleRepository(
                    guidProvider,
                    ":memory:"
                    );
        }

        [TearDown]
        public void TearDown()
        {
            sQLitePlannedCycleRepository.Dispose();
        }

        #region PlanCycle

        [Test]
        public void PlanACycle()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            sQLitePlannedCycleRepository.PlanCycle(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> systemUnderTestPlannedCycle =
                sQLitePlannedCycleRepository.GetPlannedCycle(
                    sQLitePlannedCycleRepository.GetLatestPlannedCycleGuid(plannedLift).Value
                    );

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> referencePlannedCycle =
                REFERENCECYCLESB.PlannedCycle(
                    guidProvider,
                    plannedLift,
                    referencePoint,
                    quantizationProvider
                    );

            Assert.That(
                systemUnderTestPlannedCycle,
                Is.EqualTo(referencePlannedCycle),
                "Cycle planned by the system under test must match reference planned cycle."
                );
        }

        private static IEnumerable<IQuantizationProvider> QuantizationProviders()
        {
            yield return null;
            yield return new NearestMultipleProvider(new UniformQuantizationInterval(1.00));
            yield return new NearestMultipleProvider(new UniformQuantizationInterval(2.50));
        }

        [Test]
        [TestCaseSource(nameof(QuantizationProviders))]
        public void PlanACycleApplyingQuantizationMethodOfSuppliedQuantizationProvider(IQuantizationProvider quantizationProvider)
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            sQLitePlannedCycleRepository.PlanCycle(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> systemUnderTestPlannedCycle =
                sQLitePlannedCycleRepository.GetPlannedCycle(
                    sQLitePlannedCycleRepository.GetLatestPlannedCycleGuid(plannedLift).Value
                    );

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> referencePlannedCycle =
                REFERENCECYCLESB.PlannedCycle(
                    guidProvider,
                    plannedLift,
                    referencePoint,
                    quantizationProvider
                    );

            Assert.That(
                systemUnderTestPlannedCycle,
                Is.EqualTo(referencePlannedCycle),
                "Cycle planned, applying quantization method of supplied quantization provider, " +
                "by the system under test must match reference planned cycle."
                );
        }

        [Test]
        public void ThrowArgumentNullExceptionIfPlanningACycleAndCycleTemplateIsNull()
        {
            Lift plannedLift = Lift.Deadlift;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                null;

            Assert.Throws(
                typeof(ArgumentNullException),
                () => sQLitePlannedCycleRepository.PlanCycle(
                    templateCycle,
                    plannedLift,
                    referencePoint,
                    quantizationProvider
                    ),
                "System under test must throw argument null exception " +
                "if planning a cycle and cycle template is null."
                );
        }

        [Test]
        public void ThrowArgumentExceptionIfPlanningACycleForUnspecifiedLift()
        {
            Lift plannedLift = Lift.None;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            Assert.Throws(
                typeof(ArgumentException),
                () => sQLitePlannedCycleRepository.PlanCycle(
                    templateCycle,
                    plannedLift,
                    referencePoint,
                    quantizationProvider
                    ),
                "System under test must throw argument exception if planning a cycle for unspecified lift."
                );
        }

        [Test]
        [TestCase(Lift.Squat)]
        [TestCase(Lift.BenchPress)]
        public void NotThrowExceptionIfPlanningACycleForTheLiftItIsDesignedFor(Lift plannedLift)
        {
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            Assert.DoesNotThrow(
                () => sQLitePlannedCycleRepository.PlanCycle(
                    templateCycle,
                    plannedLift,
                    referencePoint,
                    quantizationProvider
                    ),
                "System under test must not throw exception if planning a cycle for the lift it is designed for."
                );
        }

        [Test]
        [TestCase(Lift.Deadlift)]
        public void ThrowArgumentExceptionIfPlanningACycleForTheLiftItIsNotDesignedFor(Lift plannedLift)
        {
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            Assert.Throws(
                typeof(ArgumentException),
                () => sQLitePlannedCycleRepository.PlanCycle(
                    templateCycle,
                    plannedLift,
                    referencePoint,
                    quantizationProvider
                    ),
                "System under test must throw argument exception " +
                "if planning a cycle for the lift it is not designed for."
                );
        }

        [Test]
        public void ThrowExceptionIfPlanningACycleWithGuidWhichIsMappedToAStoredPlannedCycle()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            sQLitePlannedCycleRepository.PlanCycle(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            Assert.That(
                () => sQLitePlannedCycleRepository.PlanCycle(
                    templateCycle,
                    plannedLift,
                    referencePoint,
                    quantizationProvider
                    ),
                Throws.Exception,
                "System under test must throw exception if planning a cycle " +
                "with guid which is mapped to a stored planned cycle."
                );
        }

        
        #endregion

        #region GetLatestPlannedCycleGuid

        [Test]
        public void ReturnNonNullLatestPlannedCycleGuidForTheLiftIfLatestPlannedCycleForTheLiftExists()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            sQLitePlannedCycleRepository.PlanCycle(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            Assert.That(
                sQLitePlannedCycleRepository.GetLatestPlannedCycleGuid(plannedLift),
                Is.Not.EqualTo(null),
                "System under test must return non-null latest planned cycle guid for the lift " +
                "if latest planned cycle for the lift exists."
                );
        }

        [Test]
        [TestCase(Lift.Squat)]
        [TestCase(Lift.BenchPress)]
        [TestCase(Lift.Deadlift)]
        public void ReturnNullLatestPlannedCycleGuidForTheLiftIfLatestPlannedCycleForTheLiftDoesNotExist(Lift lift)
        {
            Assert.That(
                sQLitePlannedCycleRepository.GetLatestPlannedCycleGuid(lift),
                Is.EqualTo(null),
                "System under test must return null latest planned cycle guid for the lift " +
                "if latest planned cycle for the lift does not exist."
                );
        }

        
        #endregion

        #region GetPlannedCycle

        [Test]
        public void ReturnNonNullPlannedCycleIfProvidedGuidOfStoredPlannedCycle()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            sQLitePlannedCycleRepository.PlanCycle(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            Assert.That(
                sQLitePlannedCycleRepository.GetPlannedCycle(
                    sQLitePlannedCycleRepository.GetLatestPlannedCycleGuid(plannedLift).Value
                    ),
                Is.Not.EqualTo(null),
                "Must return non-null planned cycle if provided guid of stored planned cycle."
                );
        }

        [Test]
        public void ThrowArgumentExceptionIfProvidedGuidDoesNotMapStoredPlannedCycle()
        {
            Assert.Throws(
                typeof(ArgumentException),
                () => sQLitePlannedCycleRepository.GetPlannedCycle(
                    guidProvider.GetGuid()
                    ),
                "Must throw argument exception if provided guid does not map stored planned cycle."
                );
        }

        #endregion

        #region GetPlannedSession

        [Test]
        public void ReturnPlannedSession()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            sQLitePlannedCycleRepository.PlanCycle(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> systemUnderTestPlannedCycle =
                sQLitePlannedCycleRepository.GetPlannedCycle(
                    sQLitePlannedCycleRepository.GetLatestPlannedCycleGuid(plannedLift).Value
                    );

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> referencePlannedCycle =
                REFERENCECYCLESB.PlannedCycle(
                    guidProvider,
                    plannedLift,
                    referencePoint,
                    quantizationProvider
                    );

            foreach (PlannedSession<PlannedSet> referencePlannedSession in referencePlannedCycle.Sessions)
                Assert.That(
                    sQLitePlannedCycleRepository.GetPlannedSession(
                        systemUnderTestPlannedCycle.PlannedCycleGuid,
                        referencePlannedSession.Number
                        ),
                    Is.EqualTo(referencePlannedSession),
                    "Planned session returned by the system under test must" +
                    "match reference planned session."
                    );
        }

        [Test]
        public void ThrowExceptionIfRequestingToReturnPlannedSessionWithUnmappedPlannedCycleGuid()
        {
            Assert.That(
                () => sQLitePlannedCycleRepository.GetPlannedSession(
                    guidProvider.GetGuid(),
                    new SessionNumber(1)
                    ),
                Throws.Exception,
                "System under test must throw exception if requesting " +
                "to return planned session, specifying planned cycle guid " +
                "which is not mapped to a stored planned cycle."
                );
        }

        [Test]
        public void ThrowExceptionIfRequestingToReturnPlannedSessionWhichDoesNotExist()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            sQLitePlannedCycleRepository.PlanCycle(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> systemUnderTestPlannedCycle =
                sQLitePlannedCycleRepository.GetPlannedCycle(
                    sQLitePlannedCycleRepository.GetLatestPlannedCycleGuid(plannedLift).Value
                    );

            Assert.Throws(
                typeof(ArgumentException),
                () => sQLitePlannedCycleRepository.GetPlannedSession(
                    systemUnderTestPlannedCycle.PlannedCycleGuid,
                    new SessionNumber(397)
                    ),
                "System under test must throw argument exception if requesting " +
                "to return planned session which does not exist."
                );
        }

        #endregion

        #region GetPlannedSet

        [Test]
        public void ReturnPlannedSet()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            sQLitePlannedCycleRepository.PlanCycle(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> systemUnderTestPlannedCycle =
                sQLitePlannedCycleRepository.GetPlannedCycle(
                    sQLitePlannedCycleRepository.GetLatestPlannedCycleGuid(plannedLift).Value
                    );

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> referencePlannedCycle =
                REFERENCECYCLESB.PlannedCycle(
                    guidProvider,
                    plannedLift,
                    referencePoint,
                    quantizationProvider
                    );

            foreach (PlannedSession<PlannedSet> referencePlannedSession in referencePlannedCycle.Sessions)
                foreach (PlannedSet referencePlannedSet in referencePlannedSession.Sets)
                    Assert.That(
                        sQLitePlannedCycleRepository.GetPlannedSet(
                            systemUnderTestPlannedCycle.PlannedCycleGuid,
                            referencePlannedSession.Number,
                            referencePlannedSet.Number
                            ),
                        Is.EqualTo(referencePlannedSet),
                        "Planned set returned by the system under test must" +
                        "match reference planned set."
                        );
        }

        [Test]
        public void ThrowExceptionIfRequestingToReturnPlannedSetWithUnmappedPlannedCycleGuid()
        {
            Assert.That(
                () => sQLitePlannedCycleRepository.GetPlannedSet(
                    guidProvider.GetGuid(),
                    new SessionNumber(1),
                    new SetNumber(1)
                    ),
                Throws.Exception,
                "System under test must throw exception if requesting " +
                "to return planned set, specifying planned cycle guid " +
                "which is not mapped to a stored planned cycle."
                );
        }

        [Test]
        public void ThrowExceptionIfRequestingToReturnPlannedSetWhichDoesNotExist()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            sQLitePlannedCycleRepository.PlanCycle(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> systemUnderTestPlannedCycle =
                sQLitePlannedCycleRepository.GetPlannedCycle(
                    sQLitePlannedCycleRepository.GetLatestPlannedCycleGuid(plannedLift).Value
                    );

            Assert.Throws(
                typeof(ArgumentException),
                () => sQLitePlannedCycleRepository.GetPlannedSet(
                    systemUnderTestPlannedCycle.PlannedCycleGuid,
                    new SessionNumber(397),
                    new SetNumber(397)
                    ),
                "System under test must throw argument exception if requesting " +
                "to return planned set which does not exist."
                );
        }

        #endregion

        #region UpdatePlannedSetLiftedValues

        [Test]
        public void UpdateLiftedValuesOfPlannedSet()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            sQLitePlannedCycleRepository.PlanCycle(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            Guid guidOfCycleToUpdateSetOf =
                sQLitePlannedCycleRepository.GetLatestPlannedCycleGuid(plannedLift).Value;

            SessionNumber sessionToUpdate = new SessionNumber(1);
            SetNumber setToUpdate = new SetNumber(1);

            PlannedSet plannedSetToUpdate =
                sQLitePlannedCycleRepository.GetPlannedSet(
                    guidOfCycleToUpdateSetOf,
                    sessionToUpdate,
                    setToUpdate
                    );

            LiftedValues liftedValues =
                new LiftedValues(
                    new Repetitions(plannedSetToUpdate.PlannedRepetitions.UpperBound),
                    new Weight(plannedSetToUpdate.PlannedWeight.UpperBound)
                    );

            sQLitePlannedCycleRepository.UpdatePlannedSetLiftedValues(
                guidOfCycleToUpdateSetOf,
                sessionToUpdate,
                setToUpdate,
                liftedValues
                );

            Assert.That(
                 sQLitePlannedCycleRepository.GetPlannedSet(
                    guidOfCycleToUpdateSetOf,
                    sessionToUpdate,
                    setToUpdate
                    )
                 .LiftedValues,
                Is.EqualTo(liftedValues),
                "Lifted values of planned set must be updated."
                );
        }

        [Test]
        public void ThrowExceptionIfRequestingToUpdateLiftedValuesOfPlannedSetWithUnmappedPlannedCycleGuid()
        {
            Assert.That(
                () => sQLitePlannedCycleRepository.UpdatePlannedSetLiftedValues(
                    guidProvider.GetGuid(),
                    new SessionNumber(1),
                    new SetNumber(1),
                    new LiftedValues(new Repetitions(1), new Weight(1.00))
                    ),
                Throws.Exception,
                "System under test must throw exception if requesting " +
                "to update lifted values of planned set, specifying planned cycle guid " +
                "which is not mapped to a stored planned cycle."
                );
        }

        [Test]
        public void ThrowArgumentExceptionIfUpdatingLiftedValuesOfPlannedSetWhichDoesNotExist()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            sQLitePlannedCycleRepository.PlanCycle(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            Guid guidOfCycleToUpdateSetOf =
                sQLitePlannedCycleRepository.GetLatestPlannedCycleGuid(plannedLift).Value;

            Assert.Throws(
                typeof(ArgumentException),
                () => sQLitePlannedCycleRepository.UpdatePlannedSetLiftedValues(
                    guidOfCycleToUpdateSetOf,
                    new SessionNumber(397),
                    new SetNumber(397),
                    new LiftedValues(new Repetitions(1), new Weight(1.00))
                    ),
                "System under test must throw argument exception " +
                "if updating lifted values of planned set which does not exist."
                );
        }

        #endregion

        #region GetCurrentPlannedSessionAndCurrentPlannedSetNumbers

        [Test]
        public void ReturnCurrentPlannedSessionAndCurrentPlannedSetNumbersIfTheCycleIsNotDone()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            sQLitePlannedCycleRepository.PlanCycle(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            Guid plannedCycleGuid =
                sQLitePlannedCycleRepository.GetLatestPlannedCycleGuid(plannedLift).Value;

            SessionSetNumber currentPlannedSessionAndCurrentPlannedSetNumbers =
                sQLitePlannedCycleRepository.GetCurrentPlannedSessionAndCurrentPlannedSetNumbers(plannedCycleGuid);

            Assert.That(
                currentPlannedSessionAndCurrentPlannedSetNumbers,
                Is.Not.Null,
                "Current planned session and current planned set numbers must be returned."
                );
        }

        [Test]
        public void ReturnNullCurrentPlannedSessionAndCurrentPlannedSetNumbersIfTheCycleIsDone()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            sQLitePlannedCycleRepository.PlanCycle(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            Guid plannedCycleGuid =
                sQLitePlannedCycleRepository.GetLatestPlannedCycleGuid(plannedLift).Value;

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> plannedCycle =
                sQLitePlannedCycleRepository.GetPlannedCycle(plannedCycleGuid);

            foreach (PlannedSession<PlannedSet> plannedSession in plannedCycle.Sessions)
                foreach (PlannedSet plannedSet in plannedSession.Sets)
                {
                    LiftedValues liftedValues =
                        new LiftedValues(
                            new Repetitions(plannedSet.PlannedRepetitions?.UpperBound ?? 1),
                            new Weight(plannedSet.PlannedWeight?.UpperBound ?? 1.00)
                            );

                    sQLitePlannedCycleRepository.UpdatePlannedSetLiftedValues(
                        plannedCycleGuid,
                        plannedSession.Number,
                        plannedSet.Number,
                        liftedValues
                        );
                }

            SessionSetNumber currentPlannedSessionAndCurrentPlannedSetNumbers =
                sQLitePlannedCycleRepository.GetCurrentPlannedSessionAndCurrentPlannedSetNumbers(plannedCycleGuid);

            Assert.That(
                currentPlannedSessionAndCurrentPlannedSetNumbers,
                Is.Null,
                "Null current planned session and current planned set numbers must be returned."
                );
        }

        #endregion
    }
}
