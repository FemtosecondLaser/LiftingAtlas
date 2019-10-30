using SQLite;
using System;
using System.Collections.Generic;

namespace LiftingAtlas.Standard
{
    public class SQLitePlannedCycleRepository : IPlannedCycleRepository, IDisposable
    {
        #region Private fields

        private readonly IGuidProvider guidProvider;
        private readonly SQLiteConnection connection;

        #endregion

        #region Constructors

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

        private void CreateStructure()
        {
            CreatePlannedCycleList();
            CreateLatestPlannedCycleList();
        }

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

        public void PlanCycle(
            TemplateCycle<TemplateSession<TemplateSet>, TemplateSet> cycleTemplate,
            Lift lift,
            Weight referencePoint,
            IQuantizationProvider quantizationProvider
            )
        {
            if (cycleTemplate == null)
                throw new ArgumentNullException(nameof(cycleTemplate));

            if (lift == Lift.None)
                throw new ArgumentException("Unspecified lift.", nameof(lift));

            if (!cycleTemplate.TemplateLift.HasFlag(lift))
                throw new ArgumentException("Lift is not the lift the cycle template is designed for.", nameof(lift));

            if (referencePoint == null)
                throw new ArgumentNullException(nameof(referencePoint));

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

                        arguments[0] = templateSession.Number.Value;
                        arguments[1] = templateSet.Number.Value;

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
                            arguments[4] = templateSet.PlannedRepetitions.LowerBound.Value;
                            arguments[5] = templateSet.PlannedRepetitions.UpperBound.Value;
                        }

                        if (templateSet.WeightAdjustmentConstant == null)
                            arguments[6] = null;
                        else
                            arguments[6] = templateSet.WeightAdjustmentConstant.Value;

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
                    referencePoint.Value,
                    cycleTemplate.CycleTemplateName.Name,
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
                return null;        

            return Guid.Parse(latestPlannedCycleListRow.plannedCycleGuid);
        }

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

                if (sessionCounter == null)    
                {
                    countedSessionSetList = new List<PlannedSet>();
                    sessionCounter = plannedCycleRow.sessionNumber;
                }

                if (sessionCounter != plannedCycleRow.sessionNumber)
                {
                    plannedSessions.Add(
                        new PlannedSession<PlannedSet>(
                            new SessionNumber(sessionCounter.Value),
                            countedSessionSetList
                            )
                        );

                    countedSessionSetList = new List<PlannedSet>();
                    sessionCounter = plannedCycleRow.sessionNumber;
                }

                countedSessionSetList.Add(plannedCycleRow.ToPlannedSet());

                if (i == (plannedCycleRows.Count - 1))    
                {
                    plannedSessions.Add(
                        new PlannedSession<PlannedSet>(
                            new SessionNumber(sessionCounter.Value),
                            countedSessionSetList
                            )
                        );
                }
            }

            return new PlannedCycle<PlannedSession<PlannedSet>, PlannedSet>(
                Guid.Parse(guidString),
                plannedCycleListRow.plannedLift,
                new Weight(plannedCycleListRow.referencePoint),
                new CycleTemplateName(plannedCycleListRow.cycleTemplateName),
                plannedCycleListRow.templateLift,
                plannedSessions
                );
        }

        public PlannedSession<PlannedSet> GetPlannedSession(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber
            )
        {
            if (plannedSessionNumber == null)
                throw new ArgumentNullException(nameof(plannedSessionNumber));

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
                plannedSessionNumber.Value
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

        public PlannedSet GetPlannedSet(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber
            )
        {
            if (plannedSessionNumber == null)
                throw new ArgumentNullException(nameof(plannedSessionNumber));

            if (plannedSetNumber == null)
                throw new ArgumentNullException(nameof(plannedSetNumber));

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
                plannedSessionNumber.Value,
                plannedSetNumber.Value
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

        public void UpdatePlannedSetLiftedValues(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber,
            SetNumber plannedSetNumber,
            LiftedValues liftedValues
            )
        {
            if (plannedSessionNumber == null)
                throw new ArgumentNullException(nameof(plannedSessionNumber));

            if (plannedSetNumber == null)
                throw new ArgumentNullException(nameof(plannedSetNumber));

            if (liftedValues == null)
                throw new ArgumentNullException(nameof(liftedValues));

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
                    liftedValues.LiftedRepetitions.Value,
                    liftedValues.LiftedWeight.Value,
                    plannedSessionNumber.Value,
                    plannedSetNumber.Value
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

        public SessionSetNumber GetCurrentPlannedSessionAndCurrentPlannedSetNumbers(
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

            return new SessionSetNumber(
                new SessionNumber(currentPlannedCycleRow.sessionNumber),
                new SetNumber(currentPlannedCycleRow.setNumber)
                );
        }

        public void Dispose()
        {
            connection.Dispose();
        }

        #endregion
    }
}
