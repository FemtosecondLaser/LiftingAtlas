using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Common;

namespace LiftingAtlas.Standard
{
    public class SystemDataSQLitePlannedCycleRepository : IPlannedCycleRepository, IDisposable
    {
        #region Private fields

        private readonly IGuidProvider guidProvider;
        private readonly SQLiteConnection connection;

        #endregion

        #region Constructors

        public SystemDataSQLitePlannedCycleRepository(IGuidProvider guidProvider, string connectionString)
        {
            if (guidProvider == null)
                throw new ArgumentNullException(nameof(guidProvider));

            if (connectionString == null)
                throw new ArgumentNullException(nameof(connectionString));

            this.guidProvider = guidProvider;

            this.connection = new SQLiteConnection(connectionString);
            this.connection.Open();

            #region Structure

            using (SQLiteCommand command = this.connection.CreateCommand())
            {
                command.CommandText =
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

                    ";

                command.ExecuteNonQuery();

                command.CommandText =
                    @"

                    CREATE TABLE IF NOT EXISTS 'LatestPlannedCycleList'
                    (
                        `lift`	INTEGER,
                        `plannedCycleGuid`	TEXT,
                        PRIMARY KEY(`lift`),
                        FOREIGN KEY(`plannedCycleGuid`) REFERENCES `PlannedCycleList`(`plannedCycleGuid`)
                            ON UPDATE CASCADE ON DELETE SET NULL
                    );

                    ";

                command.ExecuteNonQuery();
            }

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

            using (SQLiteTransaction transaction = connection.BeginTransaction())
            using (SQLiteCommand command = connection.CreateCommand())
            {
                try
                {
                    #region Planned cycle table creation

                    command.CommandText =
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

                        ";

                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                    #endregion

                    #region Planned cycle table population

                    foreach (TemplateSession<TemplateSet> templateSession in cycleTemplate.Sessions)
                    {
                        foreach (TemplateSet templateSet in templateSession.Sets)
                        {
                            command.Parameters.Clear();

                            command.Parameters.Add(new SQLiteParameter("@sessionNumber", templateSession.Number.Value));
                            command.Parameters.Add(new SQLiteParameter("@setNumber", templateSet.Number.Value));

                            if (templateSet.PlannedPercentageOfReferencePoint == null)
                            {
                                command.Parameters.Add(new SQLiteParameter("@plannedPercentageOfReferencePointLowerBound", null));
                                command.Parameters.Add(new SQLiteParameter("@plannedPercentageOfReferencePointUpperBound", null));
                            }
                            else
                            {
                                command.Parameters.Add(
                                    new SQLiteParameter(
                                        "@plannedPercentageOfReferencePointLowerBound",
                                        templateSet.PlannedPercentageOfReferencePoint.LowerBound
                                        ));
                                command.Parameters.Add(
                                    new SQLiteParameter(
                                        "@plannedPercentageOfReferencePointUpperBound",
                                        templateSet.PlannedPercentageOfReferencePoint.UpperBound
                                        ));
                            }

                            if (templateSet.PlannedRepetitions == null)
                            {
                                command.Parameters.Add(new SQLiteParameter("@plannedRepetitionsLowerBound", null));
                                command.Parameters.Add(new SQLiteParameter("@plannedRepetitionsUpperBound", null));
                            }
                            else
                            {
                                command.Parameters.Add(
                                    new SQLiteParameter(
                                        "@plannedRepetitionsLowerBound",
                                        templateSet.PlannedRepetitions.LowerBound.Value
                                        ));
                                command.Parameters.Add(
                                    new SQLiteParameter(
                                        "@plannedRepetitionsUpperBound",
                                        templateSet.PlannedRepetitions.UpperBound.Value
                                        ));
                            }

                            if (templateSet.WeightAdjustmentConstant == null)
                                command.Parameters.Add(new SQLiteParameter("@weightAdjustmentConstant", null));
                            else
                                command.Parameters.Add(
                                    new SQLiteParameter(
                                        "@weightAdjustmentConstant",
                                        templateSet.WeightAdjustmentConstant.Value
                                        ));

                            command.Parameters.Add(new SQLiteParameter("@note", templateSet.Note));

                            if (templateSet.PlannedPercentageOfReferencePoint == null)
                            {
                                command.Parameters.Add(new SQLiteParameter("@plannedWeightLowerBound", null));
                                command.Parameters.Add(new SQLiteParameter("@plannedWeightUpperBound", null));
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

                                command.Parameters.Add(
                                    new SQLiteParameter(
                                        "@plannedWeightLowerBound",
                                        plannedWeightLowerBound < 0.00 ? 0.00 : plannedWeightLowerBound
                                        ));
                                command.Parameters.Add(
                                    new SQLiteParameter(
                                        "@plannedWeightUpperBound",
                                        plannedWeightUpperBound < 0.00 ? 0.00 : plannedWeightUpperBound
                                        ));
                            }

                            command.Parameters.Add(new SQLiteParameter("@liftedRepetitions", null));
                            command.Parameters.Add(new SQLiteParameter("@liftedWeight", null));
                            command.Parameters.Add(new SQLiteParameter("@liftedTimestamp", null));

                            command.CommandText =
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
                                    @sessionNumber,
                                    @setNumber,
                                    @plannedPercentageOfReferencePointLowerBound,
                                    @plannedPercentageOfReferencePointUpperBound,
                                    @plannedRepetitionsLowerBound,
                                    @plannedRepetitionsUpperBound,
                                    @weightAdjustmentConstant,
                                    @note,
                                    @plannedWeightLowerBound,
                                    @plannedWeightUpperBound,
                                    @liftedRepetitions,
                                    @liftedWeight,
                                    @liftedTimestamp
                                );

                                ";

                            int plannedSetsInserted = await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                            if (plannedSetsInserted != 1)
                                throw new Exception(
                                    $"Unexpected amount of planned sets inserted into planned cycle table: {plannedSetsInserted}."
                                    );
                        }
                    }

                    #endregion

                    #region Inserting planned cycle into planned cycle list

                    command.Parameters.Clear();

                    command.Parameters.Add(new SQLiteParameter("@plannedCycleGuid", guidString));
                    command.Parameters.Add(new SQLiteParameter("@plannedLift", lift));
                    command.Parameters.Add(new SQLiteParameter("@referencePoint", referencePoint.Value));
                    command.Parameters.Add(new SQLiteParameter("@cycleTemplateName", cycleTemplate.CycleTemplateName.Name));
                    command.Parameters.Add(new SQLiteParameter("@templateLift", cycleTemplate.TemplateLift));

                    command.CommandText =
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
                            @plannedCycleGuid,
                            @plannedLift,
                            @referencePoint,
                            datetime('now'),
                            @cycleTemplateName,
                            @templateLift
                        );

                        ";

                    int plannedCyclesInserted = await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                    if (plannedCyclesInserted != 1)
                        throw new Exception(
                            $"Unexpected amount of planned cycles inserted into planned cycle list table: {plannedCyclesInserted}."
                            );

                    #endregion

                    #region Setting planned cycle as latest planned cycle

                    command.Parameters.Clear();

                    command.Parameters.Add(new SQLiteParameter("@lift", lift));

                    command.CommandText =
                        $@"

                        INSERT OR IGNORE INTO [LatestPlannedCycleList]
                        (
                            'lift',
                            'plannedCycleGuid'
                        )
                        VALUES
                        (
                            @lift,
                            NULL
                        );

                        ";

                    await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                    command.Parameters.Add(new SQLiteParameter("@plannedCycleGuid", guidString));

                    command.CommandText =
                        $@"

                        UPDATE [LatestPlannedCycleList]
                        SET [plannedCycleGuid] = @plannedCycleGuid

                        WHERE [lift] = @lift;

                        ";

                    int latestPlannedCyclesUpdated = await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                    if (latestPlannedCyclesUpdated != 1)
                        throw new Exception(
                            $"Unexpected amount of latest planned cycles updated in latest planned cycle list table: {latestPlannedCyclesUpdated}."
                            );

                    #endregion

                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw exception;
                }
            }
        }

        public async Task<Guid?> GetLatestPlannedCycleGuidAsync(Lift lift)
        {
            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.Parameters.Add(new SQLiteParameter("@lift", lift));

                command.CommandText =
                    $@"

                    SELECT
                        [plannedCycleGuid]
                    FROM [LatestPlannedCycleList]

                    WHERE [lift] = @lift

                    LIMIT 1;

                    ";

                using (DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    if (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        string plannedCycleGuid = await GetFieldClassAsync<string>(reader, 0).ConfigureAwait(false);

                        if (plannedCycleGuid == null)
                            return null;
                        else
                            return Guid.Parse(plannedCycleGuid);
                    }

                return null;
            }
        }

        public async Task<PlannedCycle<PlannedSession<PlannedSet>, PlannedSet>> GetPlannedCycleAsync(
            Guid plannedCycleGuid
            )
        {
            string guidString = plannedCycleGuid.ToString("N");

            #region Getting the planned cycle entry with the specified guid

            Lift plannedLift;
            double referencePoint;
            string cycleTemplateName;
            Lift templateLift;

            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.Parameters.Add(new SQLiteParameter("@plannedCycleGuid", guidString));

                command.CommandText =
                    $@"

                    SELECT
                        [plannedLift],
                        [referencePoint],
                        [cycleTemplateName],
                        [templateLift]
                    FROM [PlannedCycleList]

                    WHERE [plannedCycleGuid] = @plannedCycleGuid;

                    ";

                using (DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                    if (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        plannedLift = (Lift)await GetFieldStructAsync<long>(reader, 0).ConfigureAwait(false);
                        referencePoint = await GetFieldStructAsync<double>(reader, 1).ConfigureAwait(false);
                        cycleTemplateName = await GetFieldClassAsync<string>(reader, 2).ConfigureAwait(false);
                        templateLift = (Lift)await GetFieldStructAsync<long>(reader, 3).ConfigureAwait(false);
                    }
                    else
                    {
                        throw new ArgumentException(
                            "No planned cycle entry, with the specified guid, found in the planned cycle list. " +
                            $"Unfound planned cycle guid: {guidString}.",
                            nameof(plannedCycleGuid)
                            );
                    }
            }

            #endregion

            #region Getting the planned cycle for the lift

            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText =
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
                        [liftedWeight]
                    FROM [{guidString}]

                    ORDER BY [sessionNumber] ASC, [setNumber] ASC;

                    ";

                int sessionNumber;
                int setNumber;
                int? plannedPercentageOfReferencePointLowerBound;
                int? plannedPercentageOfReferencePointUpperBound;
                int? plannedRepetitionsLowerBound;
                int? plannedRepetitionsUpperBound;
                double? weightAdjustmentConstant;
                string note;
                double? plannedWeightLowerBound;
                double? plannedWeightUpperBound;
                int? liftedRepetitions;
                double? liftedWeight;

                using (DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    if (!reader.HasRows)
                        throw new Exception(
                            $"Planned cycle not found. Unfound planned cycle guid: {guidString}."
                            );

                    List<PlannedSession<PlannedSet>> plannedSessions = new List<PlannedSession<PlannedSet>>();

                    int? sessionCounter = null;
                    List<PlannedSet> countedSessionSetList = null;

                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        sessionNumber = (int)await GetFieldStructAsync<long>(reader, 0).ConfigureAwait(false);
                        setNumber = (int)await GetFieldStructAsync<long>(reader, 1).ConfigureAwait(false);
                        plannedPercentageOfReferencePointLowerBound = (int?)await GetFieldNullableStructAsync<long>(reader, 2).ConfigureAwait(false);
                        plannedPercentageOfReferencePointUpperBound = (int?)await GetFieldNullableStructAsync<long>(reader, 3).ConfigureAwait(false);
                        plannedRepetitionsLowerBound = (int?)await GetFieldNullableStructAsync<long>(reader, 4).ConfigureAwait(false);
                        plannedRepetitionsUpperBound = (int?)await GetFieldNullableStructAsync<long>(reader, 5).ConfigureAwait(false);
                        weightAdjustmentConstant = await GetFieldNullableStructAsync<double>(reader, 6).ConfigureAwait(false);
                        note = await GetFieldClassAsync<string>(reader, 7).ConfigureAwait(false);
                        plannedWeightLowerBound = await GetFieldNullableStructAsync<double>(reader, 8).ConfigureAwait(false);
                        plannedWeightUpperBound = await GetFieldNullableStructAsync<double>(reader, 9).ConfigureAwait(false);
                        liftedRepetitions = (int?)await GetFieldNullableStructAsync<long>(reader, 10).ConfigureAwait(false);
                        liftedWeight = await GetFieldNullableStructAsync<double>(reader, 11).ConfigureAwait(false);

                        if (sessionCounter == null)
                        {
                            countedSessionSetList = new List<PlannedSet>();
                            sessionCounter = sessionNumber;
                        }

                        if (sessionCounter != sessionNumber)
                        {
                            plannedSessions.Add(
                                new PlannedSession<PlannedSet>(
                                    new SessionNumber(sessionCounter.Value),
                                    countedSessionSetList
                                    ));

                            countedSessionSetList = new List<PlannedSet>();
                            sessionCounter = sessionNumber;
                        }

                        countedSessionSetList.Add(
                            PrimitivesToPlannedSet(
                                setNumber,
                                plannedPercentageOfReferencePointLowerBound,
                                plannedPercentageOfReferencePointUpperBound,
                                plannedRepetitionsLowerBound,
                                plannedRepetitionsUpperBound,
                                weightAdjustmentConstant,
                                note,
                                plannedWeightLowerBound,
                                plannedWeightUpperBound,
                                liftedRepetitions,
                                liftedWeight
                                ));
                    }

                    plannedSessions.Add(
                        new PlannedSession<PlannedSet>(
                            new SessionNumber(sessionCounter.Value),
                            countedSessionSetList
                            ));

                    return new PlannedCycle<PlannedSession<PlannedSet>, PlannedSet>(
                        Guid.Parse(guidString),
                        plannedLift,
                        new Weight(referencePoint),
                        new CycleTemplateName(cycleTemplateName),
                        templateLift,
                        plannedSessions
                        );
                }
            }

            #endregion
        }

        public async Task<PlannedSession<PlannedSet>> GetPlannedSessionAsync(
            Guid plannedCycleGuid,
            SessionNumber plannedSessionNumber
            )
        {
            if (plannedSessionNumber == null)
                throw new ArgumentNullException(nameof(plannedSessionNumber));

            string guidString = plannedCycleGuid.ToString("N");

            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.Parameters.Add(new SQLiteParameter("@sessionNumber", plannedSessionNumber.Value));

                command.CommandText =
                    $@"

                    SELECT
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

                    WHERE [sessionNumber] = @sessionNumber

                    ORDER BY [setNumber] ASC;

                    ";

                int setNumber;
                int? plannedPercentageOfReferencePointLowerBound;
                int? plannedPercentageOfReferencePointUpperBound;
                int? plannedRepetitionsLowerBound;
                int? plannedRepetitionsUpperBound;
                double? weightAdjustmentConstant;
                string note;
                double? plannedWeightLowerBound;
                double? plannedWeightUpperBound;
                int? liftedRepetitions;
                double? liftedWeight;

                using (DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    if (!reader.HasRows)
                        throw new ArgumentException(
                            "Planned session not found. " +
                            $"Cycle guid: {guidString}. " +
                            $"Session number: {plannedSessionNumber}."
                            );

                    List<PlannedSet> plannedSessionSets = new List<PlannedSet>();

                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        setNumber = (int)await GetFieldStructAsync<long>(reader, 0).ConfigureAwait(false);
                        plannedPercentageOfReferencePointLowerBound = (int?)await GetFieldNullableStructAsync<long>(reader, 1).ConfigureAwait(false);
                        plannedPercentageOfReferencePointUpperBound = (int?)await GetFieldNullableStructAsync<long>(reader, 2).ConfigureAwait(false);
                        plannedRepetitionsLowerBound = (int?)await GetFieldNullableStructAsync<long>(reader, 3).ConfigureAwait(false);
                        plannedRepetitionsUpperBound = (int?)await GetFieldNullableStructAsync<long>(reader, 4).ConfigureAwait(false);
                        weightAdjustmentConstant = await GetFieldNullableStructAsync<double>(reader, 5).ConfigureAwait(false);
                        note = await GetFieldClassAsync<string>(reader, 6).ConfigureAwait(false);
                        plannedWeightLowerBound = await GetFieldNullableStructAsync<double>(reader, 7).ConfigureAwait(false);
                        plannedWeightUpperBound = await GetFieldNullableStructAsync<double>(reader, 8).ConfigureAwait(false);
                        liftedRepetitions = (int?)await GetFieldNullableStructAsync<long>(reader, 9).ConfigureAwait(false);
                        liftedWeight = await GetFieldNullableStructAsync<double>(reader, 10).ConfigureAwait(false);

                        plannedSessionSets.Add(
                            PrimitivesToPlannedSet(
                                setNumber,
                                plannedPercentageOfReferencePointLowerBound,
                                plannedPercentageOfReferencePointUpperBound,
                                plannedRepetitionsLowerBound,
                                plannedRepetitionsUpperBound,
                                weightAdjustmentConstant,
                                note,
                                plannedWeightLowerBound,
                                plannedWeightUpperBound,
                                liftedRepetitions,
                                liftedWeight
                                ));
                    }

                    return new PlannedSession<PlannedSet>(
                        plannedSessionNumber,
                        plannedSessionSets
                        );
                }
            }
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

            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.Parameters.Add(new SQLiteParameter("@sessionNumber", plannedSessionNumber.Value));
                command.Parameters.Add(new SQLiteParameter("@setNumber", plannedSetNumber.Value));

                command.CommandText =
                    $@"

                    SELECT
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
                        [liftedWeight]
                    FROM [{guidString}]

                    WHERE [sessionNumber] = @sessionNumber AND [setNumber] = @setNumber

                    LIMIT 1;

                    ";

                int setNumber;
                int? plannedPercentageOfReferencePointLowerBound;
                int? plannedPercentageOfReferencePointUpperBound;
                int? plannedRepetitionsLowerBound;
                int? plannedRepetitionsUpperBound;
                double? weightAdjustmentConstant;
                string note;
                double? plannedWeightLowerBound;
                double? plannedWeightUpperBound;
                int? liftedRepetitions;
                double? liftedWeight;

                using (DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    if (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        setNumber = (int)await GetFieldStructAsync<long>(reader, 0).ConfigureAwait(false);
                        plannedPercentageOfReferencePointLowerBound = (int?)await GetFieldNullableStructAsync<long>(reader, 1).ConfigureAwait(false);
                        plannedPercentageOfReferencePointUpperBound = (int?)await GetFieldNullableStructAsync<long>(reader, 2).ConfigureAwait(false);
                        plannedRepetitionsLowerBound = (int?)await GetFieldNullableStructAsync<long>(reader, 3).ConfigureAwait(false);
                        plannedRepetitionsUpperBound = (int?)await GetFieldNullableStructAsync<long>(reader, 4).ConfigureAwait(false);
                        weightAdjustmentConstant = await GetFieldNullableStructAsync<double>(reader, 5).ConfigureAwait(false);
                        note = await GetFieldClassAsync<string>(reader, 6).ConfigureAwait(false);
                        plannedWeightLowerBound = await GetFieldNullableStructAsync<double>(reader, 7).ConfigureAwait(false);
                        plannedWeightUpperBound = await GetFieldNullableStructAsync<double>(reader, 8).ConfigureAwait(false);
                        liftedRepetitions = (int?)await GetFieldNullableStructAsync<long>(reader, 9).ConfigureAwait(false);
                        liftedWeight = await GetFieldNullableStructAsync<double>(reader, 10).ConfigureAwait(false);

                        return PrimitivesToPlannedSet(
                            setNumber,
                            plannedPercentageOfReferencePointLowerBound,
                            plannedPercentageOfReferencePointUpperBound,
                            plannedRepetitionsLowerBound,
                            plannedRepetitionsUpperBound,
                            weightAdjustmentConstant,
                            note,
                            plannedWeightLowerBound,
                            plannedWeightUpperBound,
                            liftedRepetitions,
                            liftedWeight
                            );
                    }
                    else
                    {
                        throw new ArgumentException(
                            "Planned set not found. " +
                            $"Cycle guid: {guidString}. " +
                            $"Session number: {plannedSessionNumber}. " +
                            $"Set number: {plannedSetNumber}."
                            );
                    }
                }
            }
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

            using (SQLiteTransaction transaction = connection.BeginTransaction())
            using (SQLiteCommand command = connection.CreateCommand())
            {
                try
                {
                    command.Parameters.Add(new SQLiteParameter("@liftedRepetitions", liftedValues.LiftedRepetitions.Value));
                    command.Parameters.Add(new SQLiteParameter("@liftedWeight", liftedValues.LiftedWeight.Value));
                    command.Parameters.Add(new SQLiteParameter("@sessionNumber", plannedSessionNumber.Value));
                    command.Parameters.Add(new SQLiteParameter("@setNumber", plannedSetNumber.Value));

                    command.CommandText =
                        $@"

                        UPDATE [{guidString}]

                        SET
                        [liftedRepetitions] = @liftedRepetitions,
                        [liftedWeight] = @liftedWeight,
                        [liftedTimestamp] = datetime('now')

                        WHERE [sessionNumber] = @sessionNumber AND [setNumber] = @setNumber;

                        ";

                    int plannedSetsLiftedValuesUpdated = await command.ExecuteNonQueryAsync().ConfigureAwait(false);

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

                    transaction.Commit();
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw exception;
                }
            }
        }

        public async Task<SessionSetNumber> GetCurrentPlannedSessionAndCurrentPlannedSetNumbersAsync(
            Guid plannedCycleGuid
            )
        {
            string guidString = plannedCycleGuid.ToString("N");

            using (SQLiteCommand command = connection.CreateCommand())
            {
                command.CommandText =
                    $@"

                    SELECT
                        [sessionNumber],
                        [setNumber]
                    FROM [{guidString}]

                    WHERE [liftedRepetitions] IS NULL AND [liftedWeight] IS NULL

                    ORDER BY [sessionNumber] ASC, [setNumber] ASC

                    LIMIT 1;

                    ";

                int sessionNumber;
                int setNumber;

                using (DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    if (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        sessionNumber = (int)await GetFieldStructAsync<long>(reader, 0).ConfigureAwait(false);
                        setNumber = (int)await GetFieldStructAsync<long>(reader, 1).ConfigureAwait(false);

                        return new SessionSetNumber(
                            new SessionNumber(sessionNumber),
                            new SetNumber(setNumber)
                            );
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public void Dispose()
        {
            connection.Dispose();
        }

        private async Task<T> GetFieldClassAsync<T>(
            DbDataReader reader,
            int ordinal
            ) where T : class
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            if (ordinal < 0)
                throw new ArgumentOutOfRangeException(nameof(ordinal));

            if (await reader.IsDBNullAsync(ordinal).ConfigureAwait(false))
                return null;
            else
                return await reader.GetFieldValueAsync<T>(ordinal).ConfigureAwait(false);
        }

        private async Task<T> GetFieldStructAsync<T>(
            DbDataReader reader,
            int ordinal
            ) where T : struct
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            if (ordinal < 0)
                throw new ArgumentOutOfRangeException(nameof(ordinal));

            return await reader.GetFieldValueAsync<T>(ordinal).ConfigureAwait(false);
        }

        private async Task<T?> GetFieldNullableStructAsync<T>(
            DbDataReader reader,
            int ordinal
            ) where T : struct
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            if (ordinal < 0)
                throw new ArgumentOutOfRangeException(nameof(ordinal));

            if (await reader.IsDBNullAsync(ordinal).ConfigureAwait(false))
                return null;
            else
                return await reader.GetFieldValueAsync<T>(ordinal).ConfigureAwait(false);
        }

        private PlannedSet PrimitivesToPlannedSet(
            int setNumberInt,
            int? plannedPercentageOfReferencePointLowerBoundInt,
            int? plannedPercentageOfReferencePointUpperBoundInt,
            int? plannedRepetitionsLowerBoundInt,
            int? plannedRepetitionsUpperBoundInt,
            double? weightAdjustmentConstantDouble,
            string noteString,
            double? plannedWeightLowerBoundDouble,
            double? plannedWeightUpperBoundDouble,
            int? liftedRepetitionsInt,
            double? liftedWeightDouble
            )
        {
            SetNumber number;
            PlannedPercentageOfReferencePoint plannedPercentageOfReferencePoint;
            PlannedRepetitions plannedRepetitions;
            PlannedWeight plannedWeight;
            LiftedValues liftedValues;
            WeightAdjustmentConstant weightAdjustmentConstant;
            string note;

            number = new SetNumber(setNumberInt);

            if (plannedPercentageOfReferencePointLowerBoundInt == null ||
                plannedPercentageOfReferencePointUpperBoundInt == null)
                plannedPercentageOfReferencePoint = null;
            else
                plannedPercentageOfReferencePoint =
                    new PlannedPercentageOfReferencePoint(
                        plannedPercentageOfReferencePointLowerBoundInt.Value,
                        plannedPercentageOfReferencePointUpperBoundInt.Value
                        );

            if (plannedRepetitionsLowerBoundInt == null ||
                plannedRepetitionsUpperBoundInt == null)
                plannedRepetitions = null;
            else
                plannedRepetitions =
                    new PlannedRepetitions(
                        new Repetitions(plannedRepetitionsLowerBoundInt.Value),
                        new Repetitions(plannedRepetitionsUpperBoundInt.Value)
                        );

            if (plannedWeightLowerBoundDouble == null ||
                plannedWeightUpperBoundDouble == null)
                plannedWeight = null;
            else
                plannedWeight =
                    new PlannedWeight(
                        new Weight(plannedWeightLowerBoundDouble.Value),
                        new Weight(plannedWeightUpperBoundDouble.Value)
                        );

            if (liftedRepetitionsInt == null ||
                liftedWeightDouble == null)
                liftedValues = null;
            else
                liftedValues =
                    new LiftedValues(
                        new Repetitions(liftedRepetitionsInt.Value),
                        new Weight(liftedWeightDouble.Value)
                        );

            if (weightAdjustmentConstantDouble == null)
                weightAdjustmentConstant = null;
            else
                weightAdjustmentConstant =
                    new WeightAdjustmentConstant(
                        weightAdjustmentConstantDouble.Value
                        );

            note = noteString;

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

        #endregion
    }
}
