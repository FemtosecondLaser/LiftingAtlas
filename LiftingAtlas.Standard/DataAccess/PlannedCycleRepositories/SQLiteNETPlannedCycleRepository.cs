using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiftingAtlas.Standard
{
    public class SQLiteNETPlannedCycleRepository : IPlannedCycleRepository, IDisposable
    {
        #region Private fields

        private readonly IGuidProvider guidProvider;
        private readonly SQLiteAsyncConnection connection;

        #endregion

        #region Constructors

        public SQLiteNETPlannedCycleRepository(IGuidProvider guidProvider, string connectionString)
        {
            if (guidProvider == null)
                throw new ArgumentNullException(nameof(guidProvider));

            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));

            this.guidProvider = guidProvider;

            this.connection =
                new SQLiteAsyncConnection(
                    connectionString,
                    SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex,
                    true
                    );

            #region Structure

            this.connection.GetConnection().Execute(
                @"

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

            this.connection.GetConnection().Execute(
                @"
                
                CREATE TABLE IF NOT EXISTS 'LatestPlannedCycleList'
                (
                    `lift`	INTEGER,
                    `plannedCycleGuid`	TEXT,
                    PRIMARY KEY(`lift`),
                    FOREIGN KEY(`plannedCycleGuid`) REFERENCES `PlannedCycleList`(`plannedCycleGuid`)
                        ON UPDATE CASCADE ON DELETE SET NULL
                );
                
                ");

            #endregion
        }

        #endregion

        #region Methods

        public async Task PlanCycleAsync(
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

            await connection.RunInTransactionAsync(
                connection =>
                {
                    #region Planned cycle table creation

                    connection.Execute(
                        $@"

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

                            int plannedSetsInserted = connection.Execute(
                                $@"

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

                    int plannedCyclesInserted = connection.Execute(
                        $@"

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

                    connection.Execute(
                        $@"

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

                    int latestPlannedCyclesUpdated = connection.Execute(
                        $@"

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
                }
                ).ConfigureAwait(false);
        }

        public async Task<Guid?> GetLatestPlannedCycleGuidAsync(Lift lift)
        {
            LatestPlannedCycleListDataset latestPlannedCycleListRow =
                await connection.FindWithQueryAsync<LatestPlannedCycleListDataset>(
                    $@"

                    SELECT
                        [lift],
                        [plannedCycleGuid]
                    FROM [LatestPlannedCycleList]

                    WHERE [lift] = ?

                    LIMIT 1;

                    ",
                    lift
                    ).ConfigureAwait(false);

            if (latestPlannedCycleListRow == null || latestPlannedCycleListRow.plannedCycleGuid == null)
                return null;

            return Guid.Parse(latestPlannedCycleListRow.plannedCycleGuid);
        }

        public async Task<PlannedCycle<PlannedSession<PlannedSet>, PlannedSet>> GetPlannedCycleAsync(
            Guid plannedCycleGuid
            )
        {
            string guidString = plannedCycleGuid.ToString("N");

            #region Getting the planned cycle entry with the specified guid

            PlannedCycleListDataset plannedCycleListRow =
                await connection.FindWithQueryAsync<PlannedCycleListDataset>(
                    $@"

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
                    ).ConfigureAwait(false);

            #endregion

            if (plannedCycleListRow == null)
                throw new ArgumentException(
                    "No planned cycle entry, with the specified guid, found in the planned cycle list. " +
                    $"Unfound planned cycle guid: {guidString}.",
                    nameof(plannedCycleGuid)
                    );

            #region Getting the planned cycle for the lift

            List<PlannedCycleDataset> plannedCycleRows =
                await connection.QueryAsync<PlannedCycleDataset>(
                    $@"

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

                    ").ConfigureAwait(false);

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
                            ));

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
                            ));
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

        public async Task<PlannedSession<PlannedSet>> GetPlannedSessionAsync(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber
            )
        {
            if (plannedSessionNumber == null)
                throw new ArgumentNullException(nameof(plannedSessionNumber));

            string guidString = plannedCycleGuid.ToString("N");

            List<PlannedCycleDataset> plannedSessionRows =
                await connection.QueryAsync<PlannedCycleDataset>(
                    $@"

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
                    ).ConfigureAwait(false);

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

        public async Task<PlannedSet> GetPlannedSetAsync(
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

            List<PlannedCycleDataset> plannedSetRows =
                await connection.QueryAsync<PlannedCycleDataset>(
                    $@"

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

                    WHERE [sessionNumber] = ? AND [setNumber] = ?

                    LIMIT 1;

                    ",
                    plannedSessionNumber.Value,
                    plannedSetNumber.Value
                    ).ConfigureAwait(false);

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

        public async Task UpdatePlannedSetLiftedValuesAsync(
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

            await connection.RunInTransactionAsync(
                connection =>
                {
                    int plannedSetsLiftedValuesUpdated = connection.Execute(
                        $@"

                        UPDATE [{guidString}]

                        SET
                        [liftedRepetitions] = ?,
                        [liftedWeight] = ?,
                        [liftedTimestamp] = datetime('now')

                        WHERE [sessionNumber] = ? AND [setNumber] = ?;

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
                }
                ).ConfigureAwait(false);
        }

        public async Task<SessionSetNumber> GetCurrentPlannedSessionAndCurrentPlannedSetNumbersAsync(
            Guid plannedCycleGuid
            )
        {
            string guidString = plannedCycleGuid.ToString("N");

            PlannedCycleDataset currentPlannedCycleRow =
                await connection.FindWithQueryAsync<PlannedCycleDataset>(
                    $@"

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

                    WHERE [liftedRepetitions] IS NULL AND [liftedWeight] IS NULL

                    ORDER BY [sessionNumber] ASC, [setNumber] ASC

                    LIMIT 1;

                    ").ConfigureAwait(false);

            if (currentPlannedCycleRow == null)
                return null;

            return new SessionSetNumber(
                new SessionNumber(currentPlannedCycleRow.sessionNumber),
                new SetNumber(currentPlannedCycleRow.setNumber)
                );
        }

        public void Dispose()
        {
            connection.CloseAsync().Wait();
        }

        #region Dataset types

        private class LatestPlannedCycleListDataset
        {
            public Lift lift { get; set; }
            public string plannedCycleGuid { get; set; }
        }

        private class PlannedCycleDataset
        {
            public int sessionNumber { get; set; }
            public int setNumber { get; set; }
            public int? plannedPercentageOfReferencePointLowerBound { get; set; }
            public int? plannedPercentageOfReferencePointUpperBound { get; set; }
            public int? plannedRepetitionsLowerBound { get; set; }
            public int? plannedRepetitionsUpperBound { get; set; }
            public double? weightAdjustmentConstant { get; set; }
            public string note { get; set; }
            public double? plannedWeightLowerBound { get; set; }
            public double? plannedWeightUpperBound { get; set; }
            public int? liftedRepetitions { get; set; }
            public double? liftedWeight { get; set; }
            public string liftedTimestamp { get; set; }

            public PlannedSet ToPlannedSet()
            {
                SetNumber number;
                PlannedPercentageOfReferencePoint plannedPercentageOfReferencePoint;
                PlannedRepetitions plannedRepetitions;
                PlannedWeight plannedWeight;
                LiftedValues liftedValues;
                WeightAdjustmentConstant weightAdjustmentConstant;
                string note;

                number = new SetNumber(this.setNumber);

                if (this.plannedPercentageOfReferencePointLowerBound == null ||
                    this.plannedPercentageOfReferencePointUpperBound == null)
                    plannedPercentageOfReferencePoint = null;
                else
                    plannedPercentageOfReferencePoint =
                        new PlannedPercentageOfReferencePoint(
                            this.plannedPercentageOfReferencePointLowerBound.Value,
                            this.plannedPercentageOfReferencePointUpperBound.Value
                            );

                if (this.plannedRepetitionsLowerBound == null ||
                    this.plannedRepetitionsUpperBound == null)
                    plannedRepetitions = null;
                else
                    plannedRepetitions =
                        new PlannedRepetitions(
                            new Repetitions(this.plannedRepetitionsLowerBound.Value),
                            new Repetitions(this.plannedRepetitionsUpperBound.Value)
                            );

                if (this.plannedWeightLowerBound == null ||
                    this.plannedWeightUpperBound == null)
                    plannedWeight = null;
                else
                    plannedWeight =
                        new PlannedWeight(
                            new Weight(this.plannedWeightLowerBound.Value),
                            new Weight(this.plannedWeightUpperBound.Value)
                            );

                if (this.liftedRepetitions == null ||
                    this.liftedWeight == null)
                    liftedValues = null;
                else
                    liftedValues =
                        new LiftedValues(
                            new Repetitions(this.liftedRepetitions.Value),
                            new Weight(this.liftedWeight.Value)
                            );

                if (this.weightAdjustmentConstant == null)
                    weightAdjustmentConstant = null;
                else
                    weightAdjustmentConstant =
                        new WeightAdjustmentConstant(
                            this.weightAdjustmentConstant.Value
                            );

                note = this.note;

                return new PlannedSet(
                    number,
                    plannedPercentageOfReferencePoint,
                    plannedRepetitions,
                    plannedWeight,
                    liftedValues,
                    weightAdjustmentConstant,
                    note
                    );
            }
        }

        private class PlannedCycleListDataset
        {
            public string plannedCycleGuid { get; set; }
            public Lift plannedLift { get; set; }
            public double referencePoint { get; set; }
            public string plannedTimestamp { get; set; }
            public string cycleTemplateName { get; set; }
            public Lift templateLift { get; set; }
        }

        #endregion

        #endregion
    }
}
