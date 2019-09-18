using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LiftingAtlas.Standard.Tests
{
    [TestFixture]
    public class SQLitePlannedCycleRepositoryMust
    {
        /// <summary>
        /// Guid provider.
        /// </summary>
        public IGuidProvider guidProvider;

        /// <summary>
        /// SQLite planned cycle repository.
        /// </summary>
        public SQLitePlannedCycleRepository sQLitePlannedCycleRepository;

        /// <summary>
        /// One time set up.
        /// </summary>
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            guidProvider = new HexFirstCharFRestZerosGuidProvider();
        }

        /// <summary>
        /// Set up.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            sQLitePlannedCycleRepository =
                new SQLitePlannedCycleRepository(
                    guidProvider,
                    ":memory:"
                    );
        }

        /// <summary>
        /// Tear down.
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            sQLitePlannedCycleRepository.Dispose();
        }

        #region PlanCycle

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to plan a cycle. The cycle, planned by the system under test,
        /// must match reference planned cycle.
        /// </summary>
        [Test]
        public void PlanACycle()
        {
            Lift plannedLift = Lift.Squat;
            double referencePoint = 137.5;
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

        /// <summary>
        /// Enumerable quantization providers.
        /// </summary>
        /// <returns>Quantization provider.</returns>
        private static IEnumerable<IQuantizationProvider> QuantizationProviders()
        {
            yield return null;
            yield return new NearestTwoPointFiveMultipleProvider();
        }

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to plan a cycle using supplied quantization provider.
        /// </summary>
        [Test]
        [TestCaseSource("QuantizationProviders")]
        public void PlanACycleApplyingQuantizationMethodOfSuppliedQuantizationProvider(IQuantizationProvider quantizationProvider)
        {
            Lift plannedLift = Lift.Squat;
            double referencePoint = 137.5;
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

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to throw <see cref="ArgumentNullException"/> if planning a cycle
        /// and cycle template is null.
        /// </summary>
        [Test]
        public void ThrowArgumentNullExceptionIfPlanningACycleAndCycleTemplateIsNull()
        {
            Lift plannedLift = Lift.Deadlift;
            double referencePoint = 137.5;
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

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to throw <see cref="ArgumentException"/> if planning a cycle for unspecified lift.
        /// </summary>
        [Test]
        public void ThrowArgumentExceptionIfPlanningACycleForUnspecifiedLift()
        {
            Lift plannedLift = Lift.None;
            double referencePoint = 137.5;
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

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to not throw exception if planning a cycle for the lift it is designed for.
        /// </summary>
        [Test]
        [TestCase(Lift.Squat)]
        [TestCase(Lift.BenchPress)]
        public void NotThrowExceptionIfPlanningACycleForTheLiftItIsDesignedFor(Lift plannedLift)
        {
            double referencePoint = 137.5;
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

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to throw <see cref="ArgumentException"/> if planning a cycle for the lift it is not designed for.
        /// </summary>
        [Test]
        [TestCase(Lift.Deadlift)]
        public void ThrowArgumentExceptionIfPlanningACycleForTheLiftItIsNotDesignedFor(Lift plannedLift)
        {
            double referencePoint = 137.5;
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

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to throw <see cref="ArgumentOutOfRangeException"/>
        /// if planning a cycle using negative reference point.
        /// </summary>
        [Test]
        public void ThrowArgumentOutOfRangeExceptionIfPlanningACycleUsingNegativeReferencePoint()
        {
            Lift plannedLift = Lift.Squat;
            double referencePoint = -1;
            IQuantizationProvider quantizationProvider = null;
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> templateCycle =
                REFERENCECYCLESB.TemplateCycle();

            Assert.Throws(
                typeof(ArgumentOutOfRangeException),
                () => sQLitePlannedCycleRepository.PlanCycle(
                    templateCycle,
                    plannedLift,
                    referencePoint,
                    quantizationProvider
                    ),
                "System under test must throw argument out of range exception " +
                "if planning a cycle using negative reference point."
                );
        }

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to throw exception if planning a cycle with guid
        /// which is mapped to a stored planned cycle.
        /// </summary>
        [Test]
        public void ThrowExceptionIfPlanningACycleWithGuidWhichIsMappedToAStoredPlannedCycle()
        {
            Lift plannedLift = Lift.Squat;
            double referencePoint = 137.5;
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

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to return non-null latest planned cycle guid for the lift,
        /// if latest planned cycle for the lift exists.
        /// </summary>
        [Test]
        public void ReturnNonNullLatestPlannedCycleGuidForTheLiftIfLatestPlannedCycleForTheLiftExists()
        {
            Lift plannedLift = Lift.Squat;
            double referencePoint = 137.5;
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

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to return null latest planned cycle guid for the lift,
        /// if latest planned cycle for the lift does not exist.
        /// </summary>
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

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to return non-null planned cycle if provided guid of stored planned cycle.
        /// </summary>
        [Test]
        public void ReturnNonNullPlannedCycleIfProvidedGuidOfStoredPlannedCycle()
        {
            Lift plannedLift = Lift.Squat;
            double referencePoint = 137.5;
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

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to throw <see cref="ArgumentException"/> if provided guid does not map stored planned cycle.
        /// </summary>
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

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to return planned session. Returned planned session
        /// must match reference planned session.
        /// </summary>
        [Test]
        public void ReturnPlannedSession()
        {
            Lift plannedLift = Lift.Squat;
            double referencePoint = 137.5;
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

            foreach (
                PlannedSession<PlannedSet> referencePlannedSession
                in
                referencePlannedCycle.Sessions
                )
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

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to throw exception if requesting to return planned session,
        /// specifying planned cycle guid which is not mapped to a stored planned cycle.
        /// </summary>
        [Test]
        public void ThrowExceptionIfRequestingToReturnPlannedSessionWithUnmappedPlannedCycleGuid()
        {
            Assert.That(
                () => sQLitePlannedCycleRepository.GetPlannedSession(
                    guidProvider.GetGuid(),
                    1
                    ),
                Throws.Exception,
                "System under test must throw exception if requesting " +
                "to return planned session, specifying planned cycle guid " +
                "which is not mapped to a stored planned cycle."
                );
        }

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to throw <see cref="ArgumentException"/> if requesting
        /// to return planned session, which does not exist.
        /// </summary>
        [Test]
        public void ThrowExceptionIfRequestingToReturnPlannedSessionWhichDoesNotExist()
        {
            Lift plannedLift = Lift.Squat;
            double referencePoint = 137.5;
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
                    397
                    ),
                "System under test must throw argument exception if requesting " +
                "to return planned session which does not exist."
                );
        }

        #endregion

        #region GetPlannedSet

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to return planned set. Returned planned set
        /// must match reference planned set.
        /// </summary>
        [Test]
        public void ReturnPlannedSet()
        {
            Lift plannedLift = Lift.Squat;
            double referencePoint = 137.5;
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

            foreach (
                PlannedSession<PlannedSet> referencePlannedSession
                in
                referencePlannedCycle.Sessions
                )
                foreach (
                    PlannedSet referencePlannedSet
                    in
                    referencePlannedSession.Sets
                    )
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

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to throw exception if requesting to return planned set,
        /// specifying planned cycle guid which is not mapped to a stored planned cycle.
        /// </summary>
        [Test]
        public void ThrowExceptionIfRequestingToReturnPlannedSetWithUnmappedPlannedCycleGuid()
        {
            Assert.That(
                () => sQLitePlannedCycleRepository.GetPlannedSet(
                    guidProvider.GetGuid(),
                    1,
                    1
                    ),
                Throws.Exception,
                "System under test must throw exception if requesting " +
                "to return planned set, specifying planned cycle guid " +
                "which is not mapped to a stored planned cycle."
                );
        }

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to throw <see cref="ArgumentException"/> if requesting
        /// to return planned set, which does not exist.
        /// </summary>
        [Test]
        public void ThrowExceptionIfRequestingToReturnPlannedSetWhichDoesNotExist()
        {
            Lift plannedLift = Lift.Squat;
            double referencePoint = 137.5;
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
                    397,
                    397
                    ),
                "System under test must throw argument exception if requesting " +
                "to return planned set which does not exist."
                );
        }

        #endregion

        #region UpdatePlannedSetLiftedValues

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to update lifted values of planned set.
        /// </summary>
        [Test]
        public void UpdateLiftedValuesOfPlannedSet()
        {
            Lift plannedLift = Lift.Squat;
            double referencePoint = 137.5;
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

            int sessionToUpdate = 1;
            int setToUpdate = 1;

            PlannedSet plannedSetToUpdate =
                sQLitePlannedCycleRepository.GetPlannedSet(
                    guidOfCycleToUpdateSetOf,
                    sessionToUpdate,
                    setToUpdate
                    );

            (int liftedRepetitions, double liftedWeight) liftedValues =
                (
                plannedSetToUpdate.PlannedRepetitions.UpperBound,
                plannedSetToUpdate.PlannedWeight.UpperBound
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

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to throw exception if requesting to update lifted values of planned set,
        /// specifying planned cycle guid which is not mapped to a stored planned cycle.
        /// </summary>
        [Test]
        public void ThrowExceptionIfRequestingToUpdateLiftedValuesOfPlannedSetWithUnmappedPlannedCycleGuid()
        {
            Assert.That(
                () => sQLitePlannedCycleRepository.UpdatePlannedSetLiftedValues(
                    guidProvider.GetGuid(),
                    1,
                    1,
                    (1, 1.00)
                    ),
                Throws.Exception,
                "System under test must throw exception if requesting " +
                "to update lifted values of planned set, specifying planned cycle guid " +
                "which is not mapped to a stored planned cycle."
                );
        }

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to throw <see cref="ArgumentException"/> if updating lifted values
        /// of planned set which does not exist.
        /// </summary>
        [Test]
        public void ThrowArgumentExceptionIfUpdatingLiftedValuesOfPlannedSetWhichDoesNotExist()
        {
            Lift plannedLift = Lift.Squat;
            double referencePoint = 137.5;
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
                    397,
                    397,
                    (1, 1.00)
                    ),
                "System under test must throw argument exception " +
                "if updating lifted values of planned set which does not exist."
                );
        }

        /// <summary>
        /// Enumerable lifted values with at least a single value less than zero.
        /// </summary>
        /// <returns>Lifted values with at least a single value less than zero.</returns>
        private static IEnumerable<TestCaseData> LiftedValuesWithAtLeastASingleValueLessThanZero()
        {
            yield return new TestCaseData((-1, 1.00));
            yield return new TestCaseData((1, -1.00));
            yield return new TestCaseData((-1, -1.00));
        }

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to throw <see cref="ArgumentOutOfRangeException"/>
        /// if updating lifted values of planned set and
        /// at least a single lifted value is less than zero.
        /// </summary>
        [Test]
        [TestCaseSource("LiftedValuesWithAtLeastASingleValueLessThanZero")]
        public void ThrowArgumentOutOfRangeExceptionIfUpdatingLiftedValuesOfPlannedSetAndAtLeastASingleLiftedValueIsLessThanZero(
            (int liftedRepetitions, double liftedWeight) liftedValues
            )
        {
            Lift plannedLift = Lift.Squat;
            double referencePoint = 137.5;
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
                typeof(ArgumentOutOfRangeException),
                () => sQLitePlannedCycleRepository.UpdatePlannedSetLiftedValues(
                    guidOfCycleToUpdateSetOf,
                    1,
                    1,
                    liftedValues
                    ),
                "System under test must throw argument out of range exception " +
                "if updating lifted values of planned set " +
                "and at least a single lifted value is less than zero."
                );
        }

        #endregion

        #region GetCurrentPlannedSessionAndCurrentPlannedSetNumbers

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to return current planned session and current planned set numbers
        /// if the cycle is not done.
        /// </summary>
        [Test]
        public void ReturnCurrentPlannedSessionAndCurrentPlannedSetNumbersIfTheCycleIsNotDone()
        {
            Lift plannedLift = Lift.Squat;
            double referencePoint = 137.5;
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

            (int currentPlannedSessionNumber, int currentPlannedSetNumber)? currentPlannedSessionAndCurrentPlannedSetNumbers =
                sQLitePlannedCycleRepository.GetCurrentPlannedSessionAndCurrentPlannedSetNumbers(plannedCycleGuid);

            Assert.That(
                currentPlannedSessionAndCurrentPlannedSetNumbers.Value,
                Is.Not.Null,
                "Current planned session and current planned set numbers must be returned."
                );
        }

        /// <summary>
        /// Tests ability of <see cref="SQLitePlannedCycleRepository"/>
        /// to return null current planned session and current planned set numbers
        /// if the cycle is done.
        /// </summary>
        [Test]
        public void ReturnNullCurrentPlannedSessionAndCurrentPlannedSetNumbersIfTheCycleIsDone()
        {
            Lift plannedLift = Lift.Squat;
            double referencePoint = 137.5;
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
                    (int liftedRepetitions, double liftedWeight) liftedValues =
                        (
                        plannedSet.PlannedRepetitions?.UpperBound ?? 1,
                        plannedSet.PlannedWeight?.UpperBound ?? 1
                        );

                    sQLitePlannedCycleRepository.UpdatePlannedSetLiftedValues(
                        plannedCycleGuid,
                        plannedSession.Number,
                        plannedSet.Number,
                        liftedValues
                        );
                }

            (int currentPlannedSessionNumber, int currentPlannedSetNumber)? currentPlannedSessionAndCurrentPlannedSetNumbers =
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
