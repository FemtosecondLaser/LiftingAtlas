using SQLite;
using System;
using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    /// <summary>
    /// SQLite planned cycle repository.
    /// </summary>
    public class SQLitePlannedCycleRepository : IPlannedCycleRepository, IDisposable
    {
        #region Private fields

        /// <summary>
        /// Guid provider.
        /// </summary>
        private readonly IGuidProvider guidProvider;

        /// <summary>
        /// SQLite planned cycle repository connection.
        /// </summary>
        private readonly SQLiteConnection connection;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates SQLite planned cycle repository.
        /// </summary>
        /// <param name="guidProvider">Guid provider. Must not be null.</param>
        /// <param name="connectionString">Connection string. Must not be null.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guidProvider"/> or
        /// <paramref name="connectionString"/> is null.</exception>
        public SQLitePlannedCycleRepository(IGuidProvider guidProvider, string connectionString)
        {
            if (guidProvider == null)
                throw new ArgumentNullException(nameof(guidProvider));

            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));

            this.guidProvider = guidProvider;
            this.connection = new SQLiteConnection(
                connectionString,
                SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex,
                true
                );

            CreateStructure();
        }

        #endregion

        #region Methods

        #region Structure Creation

        /// <summary>
        /// Creates database structure.
        /// </summary>
        private void CreateStructure()
        {
            CreatePlannedCycleList();
            CreateLatestPlannedCycleList();
        }

        /// <summary>
        /// Creates table storing planned cycle list.
        /// </summary>
        private void CreatePlannedCycleList()
        {
            connection.Execute
                (@"

                CREATE TABLE IF NOT EXISTS 'PlannedCycleList'
                (
                    `plannedCycleGuid`	TEXT,
                    `plannedLift`	INTEGER NOT NULL,
                    'referencePoint' REAL NOT NULL,
                    `plannedTimestamp`	TEXT NOT NULL,
                    `cycleTemplateName`	TEXT NOT NULL,
                    `templateLift`	INTEGER NOT NULL,
                    PRIMARY KEY(`plannedCycleGuid`)
                );

                ");
        }

        /// <summary>
        /// Creates table storing latest planned cycle list.
        /// </summary>
        private void CreateLatestPlannedCycleList()
        {
            connection.Execute
                (@"
                
                CREATE TABLE IF NOT EXISTS 'LatestPlannedCycleList'
                (
                    `lift`	INTEGER,
                    `plannedCycleGuid`	TEXT,
                    PRIMARY KEY(`lift`),
                    FOREIGN KEY(`plannedCycleGuid`) REFERENCES `PlannedCycleList`(`plannedCycleGuid`)
                        ON UPDATE CASCADE ON DELETE SET NULL
                );
                
                ");
        }

        #endregion

        /// <summary>
        /// Plans a cycle.
        /// </summary>
        /// <param name="cycleTemplate">Cycle template used for planning. Must not be null.</param>
        /// <param name="lift">Lift to plan a cycle for. Must be specified.
        /// Must be the lift <paramref name="cycleTemplate"/> is designed for.</param>
        /// <param name="referencePoint">Reference point used to plan a cycle. Must not be less than 0.</param>
        /// <param name="quantizationProvider">Provider of quantization method for weight planning.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cycleTemplate"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="lift"/> is unspecified or
        /// <paramref name="lift"/> is not the <see cref="Lift"/> <paramref name="cycleTemplate"/> is designed for.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="referencePoint"/> is less than 0.</exception>
        /// <exception cref="Exception">Unexpected amount of inserted or updated rows during the operation.</exception>
        public void PlanCycle(
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> cycleTemplate,
            Lift lift,
            double referencePoint,
            IQuantizationProvider quantizationProvider
            )
        {
            if (cycleTemplate == null)
                throw new ArgumentNullException(nameof(cycleTemplate));

            if (lift == Lift.None)
                throw new ArgumentException($"Unspecified {nameof(lift)}.");

            if (!cycleTemplate.TemplateLift.HasFlag(lift))
                throw new ArgumentException($"{nameof(lift)} is not the lift the cycle template is designed for.");

            if (referencePoint < 0.00)
                throw new ArgumentOutOfRangeException(nameof(referencePoint));

            string guidString = this.guidProvider.GetGuid().ToString("N");

            connection.BeginTransaction();

            try
            {
                #region Planned cycle table creation

                connection.Execute
                    ($@"

                    CREATE TABLE `{guidString}`
                    (
	                    `sessionNumber`	INTEGER,
	                    `setNumber`	INTEGER,
	                    `plannedPercentageOfReferencePointLowerBound`	INTEGER DEFAULT NULL,
	                    `plannedPercentageOfReferencePointUpperBound`	INTEGER DEFAULT NULL,
	                    `plannedRepetitionsLowerBound`	INTEGER DEFAULT NULL,
	                    `plannedRepetitionsUpperBound`	INTEGER DEFAULT NULL,
	                    `weightAdjustmentConstant`	REAL DEFAULT NULL,
	                    `note`	TEXT DEFAULT NULL,
	                    `plannedWeightLowerBound`	REAL DEFAULT NULL,
	                    `plannedWeightUpperBound`	REAL DEFAULT NULL,
	                    `liftedRepetitions`	INTEGER DEFAULT NULL,
	                    `liftedWeight`	REAL DEFAULT NULL,
                        `liftedTimestamp`	TEXT DEFAULT NULL,
	                    PRIMARY KEY(`sessionNumber`,`setNumber`)
                    );

                    ");

                #endregion

                #region Planned cycle table population

                foreach (TemplateSession<TemplateSet> templateSession in cycleTemplate.Sessions)
                {
                    foreach (TemplateSet templateSet in templateSession.Sets)
                    {
                        object[] arguments = new object[13];

                        arguments[0] = templateSession.Number;
                        arguments[1] = templateSet.Number;

                        if (templateSet.PlannedPercentageOfReferencePoint == null)
                        {
                            arguments[2] = null;
                            arguments[3] = null;
                        }
                        else
                        {
                            arguments[2] = templateSet.PlannedPercentageOfReferencePoint.LowerBound;
                            arguments[3] = templateSet.PlannedPercentageOfReferencePoint.UpperBound;
                        }

                        if (templateSet.PlannedRepetitions == null)
                        {
                            arguments[4] = null;
                            arguments[5] = null;
                        }
                        else
                        {
                            arguments[4] = templateSet.PlannedRepetitions.LowerBound;
                            arguments[5] = templateSet.PlannedRepetitions.UpperBound;
                        }

                        arguments[6] = templateSet.WeightAdjustmentConstant;
                        arguments[7] = templateSet.Note;

                        if (templateSet.PlannedPercentageOfReferencePoint == null)
                        {
                            arguments[8] = null;
                            arguments[9] = null;
                        }
                        else
                        {
                            double plannedWeightLowerBound, plannedWeightUpperBound;

                            plannedWeightLowerBound =
                                templateSet.PlannedPercentageOfReferencePoint.LowerBound / 100.00 * referencePoint;
                            plannedWeightUpperBound =
                                templateSet.PlannedPercentageOfReferencePoint.UpperBound / 100.00 * referencePoint;

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

                            arguments[8] = plannedWeightLowerBound < 0.00 ? 0.00 : plannedWeightLowerBound;
                            arguments[9] = plannedWeightUpperBound < 0.00 ? 0.00 : plannedWeightUpperBound;
                        }

                        arguments[10] = null;
                        arguments[11] = null;
                        arguments[12] = null;

                        int plannedSetsInserted = connection.Execute
                            ($@"

                            INSERT INTO [{guidString}]
                            (
                                'sessionNumber',
                                'setNumber',
                                'plannedPercentageOfReferencePointLowerBound',
                                'plannedPercentageOfReferencePointUpperBound',
                                'plannedRepetitionsLowerBound',
                                'plannedRepetitionsUpperBound',
                                'weightAdjustmentConstant',
                                'note',
                                'plannedWeightLowerBound',
                                'plannedWeightUpperBound',
                                'liftedRepetitions',
                                'liftedWeight',
                                'liftedTimestamp'
                            )
                            VALUES
                            (
                                ?,
                                ?,
                                ?,
                                ?,
                                ?,
                                ?,
                                ?,
                                ?,
                                ?,
                                ?,
                                ?,
                                ?,
                                ?
                            );

                            ",
                            arguments
                            );

                        if (plannedSetsInserted != 1)
                            throw new Exception(
                                $"Unexpected amount of planned sets inserted into planned cycle table: {plannedSetsInserted}."
                                );
                    }
                }

                #endregion

                #region Inserting planned cycle into planned cycle list

                int plannedCyclesInserted = connection.Execute
                    ($@"

                    INSERT INTO [PlannedCycleList]
                    (
                        'plannedCycleGuid',
                        'plannedLift',
                        'referencePoint',
                        'plannedTimestamp',
                        'cycleTemplateName',
                        'templateLift'
                    )
                    VALUES
                    (
                        ?,
                        ?,
                        ?,
                        datetime('now'),
                        ?,
                        ?
                    );

                    ",
                    guidString,
                    lift,
                    referencePoint,
                    cycleTemplate.CycleTemplateName,
                    cycleTemplate.TemplateLift
                    );

                if (plannedCyclesInserted != 1)
                    throw new Exception(
                        $"Unexpected amount of planned cycles inserted into planned cycle list table: {plannedCyclesInserted}."
                        );

                #endregion

                #region Setting planned cycle as latest planned cycle

                connection.Execute
                    ($@"

                    INSERT OR IGNORE INTO [LatestPlannedCycleList]
                    (
                        'lift',
                        'plannedCycleGuid'
                    )
                    VALUES
                    (
                        ?,
                        NULL
                    );

                    ",
                    lift
                    );

                int latestPlannedCyclesUpdated = connection.Execute
                    ($@"

                    UPDATE [LatestPlannedCycleList]
                    SET [plannedCycleGuid] = ?
                    WHERE [lift] = ?;

                    ",
                    guidString,
                    lift
                    );

                if (latestPlannedCyclesUpdated != 1)
                    throw new Exception(
                        $"Unexpected amount of latest planned cycles updated in latest planned cycle list table: {latestPlannedCyclesUpdated}."
                        );

                #endregion

                connection.Commit();
            }
            catch (Exception exception)
            {
                connection.Rollback();
                throw exception;
            }
        }

        /// <summary>
        /// Gets latest planned cycle guid for the <paramref name="lift"/>.
        /// </summary>
        /// <param name="lift">Lift to get latest planned cycle guid for.</param>
        /// <returns>Latest planned cycle guid for the <paramref name="lift"/> or
        /// null if latest planned cycle for the lift does not exist.</returns>
        public Guid? GetLatestPlannedCycleGuid(Lift lift)
        {
            LatestPlannedCycleListDataset latestPlannedCycleListRow = connection.FindWithQuery<LatestPlannedCycleListDataset>
                ($@"

                SELECT
                    [lift],
                    [plannedCycleGuid]
                FROM [LatestPlannedCycleList]
                WHERE [lift] = ?;

                ",
                lift
                );

            if (latestPlannedCycleListRow == null || string.IsNullOrWhiteSpace(latestPlannedCycleListRow.plannedCycleGuid))
                return null; /* No cycle planned for the lift. */

            return Guid.Parse(latestPlannedCycleListRow.plannedCycleGuid);
        }

        /// <summary>
        /// Gets planned cycle.
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle to get.
        /// Must be the guid of existing planned cycle.</param>
        /// <returns>Planned cycle.</returns>
        /// <exception cref="ArgumentException">No planned cycle entry, with the specified guid,
        /// found in the planned cycle list.</exception>
        /// <exception cref="Exception">Cycle referenced in the planned cycle list not found.</exception>
        public PlannedCycle<PlannedSession<PlannedSet>, PlannedSet> GetPlannedCycle(Guid plannedCycleGuid)
        {
            string guidString = plannedCycleGuid.ToString("N");

            #region Getting the planned cycle entry with the specified guid

            PlannedCycleListDataset plannedCycleListRow = connection.FindWithQuery<PlannedCycleListDataset>
                ($@"

                SELECT
                    [plannedCycleGuid],
                    [plannedLift],
                    [referencePoint],
                    [plannedTimestamp],
                    [cycleTemplateName],
                    [templateLift]
                FROM [PlannedCycleList]
                WHERE [plannedCycleGuid] = ?;

                ",
                guidString
                );

            #endregion

            if (plannedCycleListRow == null)
                throw new ArgumentException(
                    "No planned cycle entry, with the specified guid, found in the planned cycle list. " +
                    $"Unfound planned cycle guid: {guidString}.",
                    nameof(plannedCycleGuid)
                    );

            #region Getting the planned cycle for the lift

            List<PlannedCycleDataset> plannedCycleRows = connection.Query<PlannedCycleDataset>
                ($@"

                SELECT
                    [sessionNumber],
                    [setNumber],
                    [plannedPercentageOfReferencePointLowerBound],
                    [plannedPercentageOfReferencePointUpperBound],
                    [plannedRepetitionsLowerBound],
                    [plannedRepetitionsUpperBound],
                    [weightAdjustmentConstant],
                    [note],
                    [plannedWeightLowerBound],
                    [plannedWeightUpperBound],
                    [liftedRepetitions],
                    [liftedWeight],
                    [liftedTimestamp]
                FROM [{guidString}]
                ORDER BY [sessionNumber] ASC, [setNumber] ASC;

                ");

            #endregion

            if (plannedCycleRows == null)
                throw new Exception(
                    $"Planned cycle not found. Unfound planned cycle guid: {guidString}."
                    );

            List<PlannedSession<PlannedSet>> plannedSessions = new List<PlannedSession<PlannedSet>>();

            int? sessionCounter = null;
            List<PlannedSet> countedSessionSetList = null;

            for (int i = 0; i < plannedCycleRows.Count; i++)
            {
                PlannedCycleDataset plannedCycleRow = plannedCycleRows[i];

                if (sessionCounter == null) /* First session. */
                {
                    countedSessionSetList = new List<PlannedSet>();
                    sessionCounter = plannedCycleRow.sessionNumber;
                }

                if (sessionCounter != plannedCycleRow.sessionNumber)
                {
                    plannedSessions.Add(
                        new PlannedSession<PlannedSet>(sessionCounter.Value, countedSessionSetList)
                        );

                    countedSessionSetList = new List<PlannedSet>();
                    sessionCounter = plannedCycleRow.sessionNumber;
                }

                countedSessionSetList.Add(plannedCycleRow.ToPlannedSet());

                if (i == (plannedCycleRows.Count - 1)) /* Last session. */
                {
                    plannedSessions.Add(
                        new PlannedSession<PlannedSet>(sessionCounter.Value, countedSessionSetList)
                        );
                }
            }

            return new PlannedCycle<PlannedSession<PlannedSet>, PlannedSet>(
                Guid.Parse(guidString),
                plannedCycleListRow.plannedLift,
                plannedCycleListRow.referencePoint,
                plannedCycleListRow.cycleTemplateName,
                plannedCycleListRow.templateLift,
                plannedSessions
                );
        }

        /// <summary>
        /// Gets planned session.
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle, containing planned session to get.
        /// Must be the guid of existing planned cycle.</param>
        /// <param name="plannedSessionNumber">Number of planned session to get.
        /// Must be the number of existing planned session.</param>
        /// <returns>Planned session.</returns>
        /// <exception cref="ArgumentException">No planned session with the speified number found.</exception>
        public PlannedSession<PlannedSet> GetPlannedSession(
            Guid plannedCycleGuid,
            int plannedSessionNumber
            )
        {
            string guidString = plannedCycleGuid.ToString("N");

            #region Getting the planned session

            List<PlannedCycleDataset> plannedSessionRows = connection.Query<PlannedCycleDataset>
                ($@"

                SELECT
                    [sessionNumber],
                    [setNumber],
                    [plannedPercentageOfReferencePointLowerBound],
                    [plannedPercentageOfReferencePointUpperBound],
                    [plannedRepetitionsLowerBound],
                    [plannedRepetitionsUpperBound],
                    [weightAdjustmentConstant],
                    [note],
                    [plannedWeightLowerBound],
                    [plannedWeightUpperBound],
                    [liftedRepetitions],
                    [liftedWeight],
                    [liftedTimestamp]
                FROM [{guidString}]
                WHERE [sessionNumber] = ?
                ORDER BY [setNumber] ASC;

                ",
                plannedSessionNumber
                );

            #endregion

            if (plannedSessionRows == null || !(plannedSessionRows.Count > 0))
                throw new ArgumentException(
                    "Planned session not found. " +
                    $"Cycle guid: {guidString}. " +
                    $"Session number: {plannedSessionNumber}."
                    );

            List<PlannedSet> plannedSessionSets = new List<PlannedSet>();

            foreach (PlannedCycleDataset plannedSetRow in plannedSessionRows)
                plannedSessionSets.Add(plannedSetRow.ToPlannedSet());

            return new PlannedSession<PlannedSet>(
                plannedSessionNumber,
                plannedSessionSets
                );
        }

        /// <summary>
        /// Gets planned set.
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle, containing planned set to get.
        /// Must be the guid of existing planned cycle.</param>
        /// <param name="plannedSessionNumber">Number of planned session, containing planned set to get.
        /// Must be the number of existing planned session.</param>
        /// <param name="plannedSetNumber">Number of planned set to get.
        /// Must be the number of existing planned set.</param>
        /// <returns>Planned set.</returns>
        /// <exception cref="ArgumentException">No planned set with the speified number found.</exception>
        /// <exception cref="Exception">Unexpected amount of rows retrieved.</exception>
        public PlannedSet GetPlannedSet(
            Guid plannedCycleGuid,
            int plannedSessionNumber,
            int plannedSetNumber
            )
        {
            string guidString = plannedCycleGuid.ToString("N");

            #region Getting the planned set

            List<PlannedCycleDataset> plannedSetRows = connection.Query<PlannedCycleDataset>
                ($@"

                SELECT
                    [sessionNumber],
                    [setNumber],
                    [plannedPercentageOfReferencePointLowerBound],
                    [plannedPercentageOfReferencePointUpperBound],
                    [plannedRepetitionsLowerBound],
                    [plannedRepetitionsUpperBound],
                    [weightAdjustmentConstant],
                    [note],
                    [plannedWeightLowerBound],
                    [plannedWeightUpperBound],
                    [liftedRepetitions],
                    [liftedWeight],
                    [liftedTimestamp]
                FROM [{guidString}]
                WHERE [sessionNumber] = ? AND [setNumber] = ?;

                ",
                plannedSessionNumber,
                plannedSetNumber
                );

            #endregion

            if (plannedSetRows == null || !(plannedSetRows.Count > 0))
                throw new ArgumentException(
                    "Planned set not found. " +
                    $"Cycle guid: {guidString}. " +
                    $"Session number: {plannedSessionNumber}. " +
                    $"Set number: {plannedSetNumber}."
                    );

            if (plannedSetRows.Count != 1)
                throw new Exception($"Unexpected amount of rows retrieved: {plannedSetRows.Count}.");

            return plannedSetRows[0].ToPlannedSet();
        }

        /// <summary>
        /// Updates lifted values of planned set.
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle, containing planned set to update.
        /// Must be the guid of existing planned cycle.</param>
        /// <param name="plannedSessionNumber">Number of planned session, containing planned set to update.
        /// Must be the number of existing planned session.</param>
        /// <param name="plannedSetNumber">Number of planned set, to update lifted values of.
        /// Must be the number of existing planned set.</param>
        /// <param name="liftedValues">Lifted values. liftedRepetitions, liftedWeight must not be less than 0.</param>
        /// <exception cref="ArgumentOutOfRangeException">At least one of the
        /// <paramref name="liftedValues"/> is less than 0.</exception>
        public void UpdatePlannedSetLiftedValues(
            Guid plannedCycleGuid,
            int plannedSessionNumber,
            int plannedSetNumber,
            (int liftedRepetitions, double liftedWeight) liftedValues
            )
        {
            if (liftedValues.liftedRepetitions < 0 || liftedValues.liftedWeight < 0.00)
                throw new ArgumentOutOfRangeException(nameof(liftedValues));

            string guidString = plannedCycleGuid.ToString("N");

            connection.BeginTransaction();

            try
            {
                int plannedSetsLiftedValuesUpdated = connection.Execute
                    ($@"

                    UPDATE [{guidString}]

                    SET
                    [liftedRepetitions] = ?,
                    [liftedWeight] = ?,
                    [liftedTimestamp] = datetime('now')

                    WHERE
                    [sessionNumber] = ?
                    AND
                    [setNumber] = ?;

                    ",                    
                    liftedValues.liftedRepetitions,
                    liftedValues.liftedWeight,
                    plannedSessionNumber,
                    plannedSetNumber
                    );

                if (!(plannedSetsLiftedValuesUpdated > 0))
                    throw new ArgumentException(
                        "Lifted values of planned set not updated. " +
                        $"Cycle guid: {guidString}. " +
                        $"Session number: {plannedSessionNumber}. " +
                        $"Set number: {plannedSetNumber}."
                        );

                if (plannedSetsLiftedValuesUpdated != 1)
                    throw new Exception(
                        "Unexpected amount of planned sets lifted values updated " +
                        $"in planned cycle table: {plannedSetsLiftedValuesUpdated}."
                        );

                connection.Commit();
            }
            catch (Exception exception)
            {
                connection.Rollback();
                throw exception;
            }
        }

        /// <summary>
        /// Gets numbers of current planned session and current planned set.
        /// </summary>
        /// <param name="plannedCycleGuid">Guid of planned cycle, to return
        /// current planned session and current planned set numbers for.
        /// Must be the guid of existing planned cycle.</param>
        /// <returns>Current planned session and current planned set numbers
        /// or null if no planned session and planned set are current.</returns>
        public (int currentPlannedSessionNumber, int currentPlannedSetNumber)? GetCurrentPlannedSessionAndCurrentPlannedSetNumbers(
            Guid plannedCycleGuid
            )
        {
            string guidString = plannedCycleGuid.ToString("N");

            PlannedCycleDataset currentPlannedCycleRow = connection.FindWithQuery<PlannedCycleDataset>
                ($@"

                SELECT
                    [sessionNumber],
                    [setNumber],
                    [plannedPercentageOfReferencePointLowerBound],
                    [plannedPercentageOfReferencePointUpperBound],
                    [plannedRepetitionsLowerBound],
                    [plannedRepetitionsUpperBound],
                    [weightAdjustmentConstant],
                    [note],
                    [plannedWeightLowerBound],
                    [plannedWeightUpperBound],
                    [liftedRepetitions],
                    [liftedWeight],
                    [liftedTimestamp]
                FROM [{guidString}]

                WHERE
                [liftedRepetitions] IS NULL
                AND
                [liftedWeight] IS NULL

                ORDER BY [sessionNumber] ASC, [setNumber] ASC

                LIMIT 1;

                ");

            if (currentPlannedCycleRow == null)
                return null;

            return (currentPlannedCycleRow.sessionNumber, currentPlannedCycleRow.setNumber);
        }

        /// <summary>
        /// Cleans up resources.
        /// </summary>
        public void Dispose()
        {
            connection.Dispose();
        }

        #endregion
    }
}
