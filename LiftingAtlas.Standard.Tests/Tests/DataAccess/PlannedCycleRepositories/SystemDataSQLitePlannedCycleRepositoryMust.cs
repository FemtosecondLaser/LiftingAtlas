using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace LiftingAtlas.Standard.Tests
{
    [TestFixture]
    public class SystemDataSQLitePlannedCycleRepositoryMust
    {
        public IGuidProvider guidProvider;
        public SystemDataSQLitePlannedCycleRepository sQLitePlannedCycleRepository;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Mock<IGuidProvider> guidProviderMock = new Mock<IGuidProvider>();

            guidProviderMock
                .Setup(guidProvider => guidProvider.GetGuid())
                .Returns(new Guid(new byte[16] { 0, 0, 0, 240, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }));

            guidProvider = guidProviderMock.Object;
        }

        [SetUp]
        public void SetUp()
        {
            sQLitePlannedCycleRepository =
                new SystemDataSQLitePlannedCycleRepository(
                    guidProvider,
                    "Data Source=:memory:;"
                    );
        }

        [TearDown]
        public void TearDown()
        {
            sQLitePlannedCycleRepository.Dispose();
        }

        #region PlanCycle

        [Test]
        public async Task PlanACycle()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            await sQLitePlannedCycleRepository.PlanCycleAsync(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            Guid? latestPlannedCycleGuid =
                await sQLitePlannedCycleRepository.GetLatestPlannedCycleGuidAsync(plannedLift);

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> systemUnderTestPlannedCycle =
                await sQLitePlannedCycleRepository.GetPlannedCycleAsync(
                    latestPlannedCycleGuid.Value
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
        public async Task PlanACycleApplyingQuantizationMethodOfSuppliedQuantizationProvider(IQuantizationProvider quantizationProvider)
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            await sQLitePlannedCycleRepository.PlanCycleAsync(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            Guid? latestPlannedCycleGuid =
                await sQLitePlannedCycleRepository.GetLatestPlannedCycleGuidAsync(plannedLift);

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> systemUnderTestPlannedCycle =
                await sQLitePlannedCycleRepository.GetPlannedCycleAsync(
                    latestPlannedCycleGuid.Value
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

            Assert.ThrowsAsync<ArgumentNullException>(
                async () => await sQLitePlannedCycleRepository.PlanCycleAsync(
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

            Assert.ThrowsAsync<ArgumentException>(
                async () => await sQLitePlannedCycleRepository.PlanCycleAsync(
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

            Assert.DoesNotThrowAsync(
                async () => await sQLitePlannedCycleRepository.PlanCycleAsync(
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

            Assert.ThrowsAsync<ArgumentException>(
                async () => await sQLitePlannedCycleRepository.PlanCycleAsync(
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
        public async Task ThrowSQLiteExceptionIfPlanningACycleWithGuidWhichIsMappedToAStoredPlannedCycle()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            await sQLitePlannedCycleRepository.PlanCycleAsync(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            Assert.ThrowsAsync<SQLiteException>(
                async () => await sQLitePlannedCycleRepository.PlanCycleAsync(
                    templateCycle,
                    plannedLift,
                    referencePoint,
                    quantizationProvider
                    ),
                "System under test must throw exception if planning a cycle " +
                "with guid which is mapped to a stored planned cycle."
                );
        }

        #endregion

        #region GetLatestPlannedCycleGuid

        [Test]
        public async Task ReturnNonNullLatestPlannedCycleGuidForTheLiftIfLatestPlannedCycleForTheLiftExists()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            await sQLitePlannedCycleRepository.PlanCycleAsync(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            Assert.That(
                await sQLitePlannedCycleRepository.GetLatestPlannedCycleGuidAsync(plannedLift),
                Is.Not.EqualTo(null),
                "System under test must return non-null latest planned cycle guid for the lift " +
                "if latest planned cycle for the lift exists."
                );
        }

        [Test]
        [TestCase(Lift.Squat)]
        [TestCase(Lift.BenchPress)]
        [TestCase(Lift.Deadlift)]
        public async Task ReturnNullLatestPlannedCycleGuidForTheLiftIfLatestPlannedCycleForTheLiftDoesNotExist(Lift lift)
        {
            Assert.That(
                await sQLitePlannedCycleRepository.GetLatestPlannedCycleGuidAsync(lift),
                Is.EqualTo(null),
                "System under test must return null latest planned cycle guid for the lift " +
                "if latest planned cycle for the lift does not exist."
                );
        }

        #endregion

        #region GetPlannedCycle

        [Test]
        public async Task ReturnNonNullPlannedCycleIfProvidedGuidOfStoredPlannedCycle()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            await sQLitePlannedCycleRepository.PlanCycleAsync(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            Guid? latestPlannedCycleGuid =
                await sQLitePlannedCycleRepository.GetLatestPlannedCycleGuidAsync(plannedLift);

            Assert.That(
                await sQLitePlannedCycleRepository.GetPlannedCycleAsync(
                    latestPlannedCycleGuid.Value
                    ),
                Is.Not.EqualTo(null),
                "Must return non-null planned cycle if provided guid of stored planned cycle."
                );
        }

        [Test]
        public void ThrowArgumentExceptionIfProvidedGuidDoesNotMapStoredPlannedCycle()
        {
            Assert.ThrowsAsync<ArgumentException>(
                async () => await sQLitePlannedCycleRepository.GetPlannedCycleAsync(
                    guidProvider.GetGuid()
                    ),
                "Must throw argument exception if provided guid does not map stored planned cycle."
                );
        }

        #endregion

        #region GetPlannedSession

        [Test]
        public async Task ReturnPlannedSession()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            await sQLitePlannedCycleRepository.PlanCycleAsync(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            Guid? latestPlannedCycleGuid =
                await sQLitePlannedCycleRepository.GetLatestPlannedCycleGuidAsync(plannedLift);

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> systemUnderTestPlannedCycle =
                await sQLitePlannedCycleRepository.GetPlannedCycleAsync(
                    latestPlannedCycleGuid.Value
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
                    await sQLitePlannedCycleRepository.GetPlannedSessionAsync(
                        systemUnderTestPlannedCycle.PlannedCycleGuid,
                        referencePlannedSession.Number
                        ),
                    Is.EqualTo(referencePlannedSession),
                    "Planned session returned by the system under test must" +
                    "match reference planned session."
                    );
        }

        [Test]
        public void ThrowSQLiteExceptionIfRequestingToReturnPlannedSessionWithUnmappedPlannedCycleGuid()
        {
            Assert.ThrowsAsync<SQLiteException>(
                async () => await sQLitePlannedCycleRepository.GetPlannedSessionAsync(
                    guidProvider.GetGuid(),
                    new SessionNumber(1)
                    ),
                "System under test must throw exception if requesting " +
                "to return planned session, specifying planned cycle guid " +
                "which is not mapped to a stored planned cycle."
                );
        }

        [Test]
        public async Task ThrowArgumentExceptionIfRequestingToReturnPlannedSessionWhichDoesNotExist()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            await sQLitePlannedCycleRepository.PlanCycleAsync(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            Guid? latestPlannedCycleGuid =
                await sQLitePlannedCycleRepository.GetLatestPlannedCycleGuidAsync(plannedLift);

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> systemUnderTestPlannedCycle =
                await sQLitePlannedCycleRepository.GetPlannedCycleAsync(
                    latestPlannedCycleGuid.Value
                    );

            Assert.ThrowsAsync<ArgumentException>(
                async () => await sQLitePlannedCycleRepository.GetPlannedSessionAsync(
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
        public async Task ReturnPlannedSet()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            await sQLitePlannedCycleRepository.PlanCycleAsync(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            Guid? latestPlannedCycleGuid =
                await sQLitePlannedCycleRepository.GetLatestPlannedCycleGuidAsync(plannedLift);

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> systemUnderTestPlannedCycle =
                await sQLitePlannedCycleRepository.GetPlannedCycleAsync(
                    latestPlannedCycleGuid.Value
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
                        await sQLitePlannedCycleRepository.GetPlannedSetAsync(
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
        public void ThrowSQLiteExceptionIfRequestingToReturnPlannedSetWithUnmappedPlannedCycleGuid()
        {
            Assert.ThrowsAsync<SQLiteException>(
                async () => await sQLitePlannedCycleRepository.GetPlannedSetAsync(
                    guidProvider.GetGuid(),
                    new SessionNumber(1),
                    new SetNumber(1)
                    ),
                "System under test must throw exception if requesting " +
                "to return planned set, specifying planned cycle guid " +
                "which is not mapped to a stored planned cycle."
                );
        }

        [Test]
        public async Task ThrowArgumentExceptionIfRequestingToReturnPlannedSetWhichDoesNotExist()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            await sQLitePlannedCycleRepository.PlanCycleAsync(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            Guid? latestPlannedCycleGuid =
                await sQLitePlannedCycleRepository.GetLatestPlannedCycleGuidAsync(plannedLift);

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> systemUnderTestPlannedCycle =
                await sQLitePlannedCycleRepository.GetPlannedCycleAsync(
                    latestPlannedCycleGuid.Value
                    );

            Assert.ThrowsAsync<ArgumentException>(
                async () => await sQLitePlannedCycleRepository.GetPlannedSetAsync(
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
        public async Task UpdateLiftedValuesOfPlannedSet()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            await sQLitePlannedCycleRepository.PlanCycleAsync(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            Guid? guidOfCycleToUpdateSetOf =
                await sQLitePlannedCycleRepository.GetLatestPlannedCycleGuidAsync(plannedLift);

            SessionNumber sessionToUpdate = new SessionNumber(1);
            SetNumber setToUpdate = new SetNumber(1);

            PlannedSet plannedSetToUpdate =
                await sQLitePlannedCycleRepository.GetPlannedSetAsync(
                    guidOfCycleToUpdateSetOf.Value,
                    sessionToUpdate,
                    setToUpdate
                    );

            LiftedValues liftedValues =
                new LiftedValues(
                    new Repetitions(plannedSetToUpdate.PlannedRepetitions.UpperBound),
                    new Weight(plannedSetToUpdate.PlannedWeight.UpperBound)
                    );

            await sQLitePlannedCycleRepository.UpdatePlannedSetLiftedValuesAsync(
                guidOfCycleToUpdateSetOf.Value,
                sessionToUpdate,
                setToUpdate,
                liftedValues
                );

            PlannedSet plannedSet =
                await sQLitePlannedCycleRepository.GetPlannedSetAsync(
                    guidOfCycleToUpdateSetOf.Value,
                    sessionToUpdate,
                    setToUpdate
                    );

            Assert.That(plannedSet.LiftedValues,
                Is.EqualTo(liftedValues),
                "Lifted values of planned set must be updated."
                );
        }

        [Test]
        public void ThrowSQLiteExceptionIfRequestingToUpdateLiftedValuesOfPlannedSetWithUnmappedPlannedCycleGuid()
        {
            Assert.ThrowsAsync<SQLiteException>(
                async () => await sQLitePlannedCycleRepository.UpdatePlannedSetLiftedValuesAsync(
                    guidProvider.GetGuid(),
                    new SessionNumber(1),
                    new SetNumber(1),
                    new LiftedValues(new Repetitions(1), new Weight(1.00))
                    ),
                "System under test must throw exception if requesting " +
                "to update lifted values of planned set, specifying planned cycle guid " +
                "which is not mapped to a stored planned cycle."
                );
        }

        [Test]
        public async Task ThrowArgumentExceptionIfUpdatingLiftedValuesOfPlannedSetWhichDoesNotExist()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            await sQLitePlannedCycleRepository.PlanCycleAsync(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            Guid? guidOfCycleToUpdateSetOf =
                await sQLitePlannedCycleRepository.GetLatestPlannedCycleGuidAsync(plannedLift);

            Assert.ThrowsAsync<ArgumentException>(
                async () => await sQLitePlannedCycleRepository.UpdatePlannedSetLiftedValuesAsync(
                    guidOfCycleToUpdateSetOf.Value,
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
        public async Task ReturnCurrentPlannedSessionAndCurrentPlannedSetNumbersIfTheCycleIsNotDone()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            await sQLitePlannedCycleRepository.PlanCycleAsync(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            Guid? plannedCycleGuid =
                await sQLitePlannedCycleRepository.GetLatestPlannedCycleGuidAsync(plannedLift);

            SessionSetNumber currentPlannedSessionAndCurrentPlannedSetNumbers =
                await sQLitePlannedCycleRepository.GetCurrentPlannedSessionAndCurrentPlannedSetNumbersAsync(
                    plannedCycleGuid.Value
                    );

            Assert.That(
                currentPlannedSessionAndCurrentPlannedSetNumbers,
                Is.Not.Null,
                "Current planned session and current planned set numbers must be returned."
                );
        }

        [Test]
        public async Task ReturnNullCurrentPlannedSessionAndCurrentPlannedSetNumbersIfTheCycleIsDone()
        {
            Lift plannedLift = Lift.Squat;
            Weight referencePoint = new Weight(137.5);
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            await sQLitePlannedCycleRepository.PlanCycleAsync(
                templateCycle,
                plannedLift,
                referencePoint,
                quantizationProvider
                );

            Guid? plannedCycleGuid =
                await sQLitePlannedCycleRepository.GetLatestPlannedCycleGuidAsync(plannedLift);

            PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> plannedCycle =
                await sQLitePlannedCycleRepository.GetPlannedCycleAsync(plannedCycleGuid.Value);

            foreach (PlannedSession<PlannedSet> plannedSession in plannedCycle.Sessions)
                foreach (PlannedSet plannedSet in plannedSession.Sets)
                {
                    LiftedValues liftedValues =
                        new LiftedValues(
                            new Repetitions(plannedSet.PlannedRepetitions?.UpperBound ?? 1),
                            new Weight(plannedSet.PlannedWeight?.UpperBound ?? 1.00)
                            );

                    await sQLitePlannedCycleRepository.UpdatePlannedSetLiftedValuesAsync(
                        plannedCycleGuid.Value,
                        plannedSession.Number,
                        plannedSet.Number,
                        liftedValues
                        );
                }

            SessionSetNumber currentPlannedSessionAndCurrentPlannedSetNumbers =
                await sQLitePlannedCycleRepository.GetCurrentPlannedSessionAndCurrentPlannedSetNumbersAsync(
                    plannedCycleGuid.Value
                    );

            Assert.That(
                currentPlannedSessionAndCurrentPlannedSetNumbers,
                Is.Null,
                "Null current planned session and current planned set numbers must be returned."
                );
        }

        #endregion
    }
}
