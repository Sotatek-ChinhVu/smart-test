using AWSSDK.Constants;
using CsvHelper;
using CsvHelper.Configuration;
using Helper.Common;
using Helper.Messaging;
using Helper.Messaging.Data;
using Npgsql;
using System.Data;
using System.Globalization;

namespace AWSSDK.Common
{
    public static class UpdateDataTenant
    {
        private static List<string> _allTempTable;
        private static string _baseTable;
        private static string _tempTable;
        private static string _type;

        private static List<string> _keyColumns;
        private static List<string> _primaryKeyColumns;
        private static List<string> _columnHeaders;


        /// <summary>
        /// Excute update data tenant
        /// </summary>
        /// <param name="filePaths"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="database"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="subFoldersMasters"></param>
        /// <returns></returns>
        public static bool ExcuteUpdateDataTenant(string[] filePaths, string[] subFoldersMasters, string host, int port, string database,
           string user, string password, CancellationToken cancellationToken, IMessenger? messenger)
        {
            string connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password};";
            string filePathRun = string.Empty;

            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Begin a transaction
                    using (NpgsqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            //Execute all SQL files in a transaction
                            foreach (var filePath in filePaths)
                            {
                                var statusCallBack = messenger!.SendAsync(new StopCalcStatus());
                                bool isStopCalc = statusCallBack.Result.Result;
                                if (isStopCalc)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                                ExecuteSqlScriptNonQuery(filePath, connection, transaction);
                            }

                            foreach (var subFolder in subFoldersMasters)
                            {
                                var statusCallBack = messenger!.SendAsync(new StopCalcStatus());
                                bool isStopCalc = statusCallBack.Result.Result;
                                if (isStopCalc)
                                {
                                    transaction.Rollback();
                                    return false;
                                }
                                #region Run PreMstScript
                                string preMstScript = Path.Combine(subFolder, UpdateConst.PRE_MST_SCRIPT);
                                if (CIUtil.IsFileExisting(preMstScript))
                                {
                                    try
                                    {
                                        var existCode = ExecuteSqlFile(preMstScript, connection, transaction);
                                        if (!existCode)
                                        {
                                            return false;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // ErrorEndUpdate("Execute preMstScript fail: " + ex.Message);
                                        Console.WriteLine("Fail Run PreMstScript: " + ex.Message);
                                        return false;
                                    }
                                }
                                #endregion

                                #region Read Csv file
                                // Read file .h
                                var headerFiles = Directory.GetFiles(subFolder, "*.h");
                                if (headerFiles.Length > 0)
                                {
                                    _allTempTable = new List<string>();

                                    foreach (var headerFile in headerFiles)
                                    {
                                        //(headerFile.TrimEnd('h'));
                                        var texts = File.ReadAllLines(headerFile);
                                        if (texts.Length < 4)
                                        {
                                            Console.WriteLine("HEADERファイルが不正です。:" + headerFile);
                                            //IsCloseEnable = true;
                                            //Console.WriteLine(_moduleName, this, nameof(ReadCsvFile), "Header file not correct format. File: " + headerFile);
                                            return false;
                                        }

                                        //clean temp variable
                                        _baseTable = string.Empty;
                                        _tempTable = string.Empty;
                                        _type = string.Empty;
                                        _keyColumns = new List<string>();
                                        _primaryKeyColumns = new List<string>();

                                        var csvFile = $"{headerFile.TrimEnd('h')}csv";
                                        if (CIUtil.IsFileExisting(csvFile))
                                        {
                                            using (var reader = new StreamReader(csvFile))
                                            {
                                                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                                                {
                                                    HasHeaderRecord = false,
                                                }))
                                                {
                                                    csv.Read();
                                                    var records = csv.GetRecord<dynamic>() as System.Dynamic.ExpandoObject;
                                                    _columnHeaders = records.ToList().Select(x => x.Value.ToString()).ToList();
                                                }
                                            }

                                            string[] tempBase = texts[0].Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                                            if (tempBase.Length == 2)
                                            {
                                                _baseTable = tempBase[1].Trim(new char[] { '"' });
                                            }
                                            string[] tempTable = texts[1].Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                                            if (tempTable.Length == 2)
                                            {
                                                _tempTable = tempTable[1].Trim(new char[] { '"' });
                                                _allTempTable.Add(_tempTable);
                                            }

                                            string[] tempType = texts[2].Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                                            if (tempType.Length == 2)
                                            {
                                                _type = tempType[1];
                                            }
                                            string[] tempKeys = texts[3].Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                                            if (tempKeys.Length == 2)
                                            {
                                                _keyColumns = tempKeys[1].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim(new char[] { '"' })).ToList();
                                            }

                                            if (_type.ToUpper() != "CLRINS")
                                            {
                                                if (!string.IsNullOrEmpty(_baseTable))
                                                {
                                                    //create temp table
                                                    try
                                                    {
                                                        CreateTempTable(connection, transaction);
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        //Console.WriteLine(_moduleName, this, nameof(ReadCsvFile), e, headerFile);
                                                        Console.WriteLine("Fail to create temp table: " + e.Message);
                                                        return false;
                                                    }
                                                }

                                                //move data from temp Table into base Table
                                                try
                                                {
                                                    // Import data
                                                    string script = $"copy \"{_tempTable}\"";

                                                    int columnIndex = 0;
                                                    if (_columnHeaders.Count == 1)
                                                    {
                                                        script += $"(\"{_columnHeaders[0]}\")";
                                                    }
                                                    else
                                                    {
                                                        foreach (var keyColumn in _columnHeaders)
                                                        {
                                                            if (columnIndex == 0)
                                                            {
                                                                script += $"(\"{keyColumn}\",";
                                                            }
                                                            else if (columnIndex == _columnHeaders.Count - 1)
                                                            {
                                                                script += $"\"{keyColumn}\")";
                                                            }
                                                            else
                                                            {
                                                                script += $"\"{keyColumn}\",";
                                                            }
                                                            columnIndex++;
                                                        }
                                                    }
                                                    script += $" from '{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, csvFile)}' CSV HEADER encoding 'UTF8' null 'null_string';";

                                                    var exitCode = ExecuteSqlScriptNonQuery(script, connection, transaction);
                                                    if (!exitCode)
                                                    {
                                                        //ErrorEndUpdate("Execute csv fail: " + csvFile);
                                                        return false;
                                                    }

                                                    if (!string.IsNullOrEmpty(_baseTable))
                                                    {
                                                        if (_type.ToUpper() != "UPDATE")
                                                        {
                                                            // Create constraint
                                                            if (_primaryKeyColumns.Count == 0)
                                                            {
                                                                if (_type.ToUpper() == "INSERT" && _keyColumns.Count != 0)
                                                                {
                                                                    _primaryKeyColumns = _keyColumns;
                                                                }
                                                                else
                                                                {
                                                                    _primaryKeyColumns = GetAllPrimaryKeys(_baseTable, connection);
                                                                }
                                                            }
                                                            string createConstraintScript = $"ALTER TABLE \"{_tempTable}\" ADD CONSTRAINT \"PK_public.{_tempTable}\" PRIMARY KEY ";
                                                            int keyIndex = 0;
                                                            // Maybe we just create key exist in column headers (in csv file) only
                                                            //var primaryKeyTempColumn = _primaryKeyColumns.Where(x => _columnHeaders.Contains(x)).ToList();
                                                            var primaryKeyTempColumn = _primaryKeyColumns;
                                                            if (primaryKeyTempColumn.Count == 1)
                                                            {
                                                                createConstraintScript += $"(\"{primaryKeyTempColumn[0]}\")";
                                                            }
                                                            else
                                                            {
                                                                foreach (var keyColumn in primaryKeyTempColumn)
                                                                {
                                                                    if (keyIndex == 0)
                                                                    {
                                                                        createConstraintScript += $"(\"{keyColumn}\",";
                                                                    }
                                                                    else if (keyIndex == primaryKeyTempColumn.Count - 1)
                                                                    {
                                                                        createConstraintScript += $"\"{keyColumn}\")";
                                                                    }
                                                                    else
                                                                    {
                                                                        createConstraintScript += $"\"{keyColumn}\",";
                                                                    }

                                                                    keyIndex++;
                                                                }
                                                            }
                                                            createConstraintScript += ";";

                                                            var createConstraintsExitCode = ExecuteSqlScriptNonQuery(createConstraintScript, connection, transaction);
                                                            if (!createConstraintsExitCode)
                                                            {
                                                                //ErrorEndUpdate("Execute create constraints fail: " + createConstraintScript);
                                                                return false;
                                                            }
                                                        }

                                                        MoveDataToBaseTable(connection, transaction);
                                                    }
                                                    transaction.Commit();
                                                }
                                                catch (Exception ex)
                                                {
                                                    //Console.WriteLine(_moduleName, this, nameof(ReadCsvFile), ex, headerFile);
                                                    transaction.Rollback();
                                                    //ErrorEndUpdate("Execute Csv fail: " + ex.Message);
                                                    return false;
                                                }
                                            }
                                            //type == CLRINS
                                            else
                                            {
                                                string script = "TRUNCATE \"" + _baseTable + "\" CASCADE;";
                                                //first, truncate
                                                try
                                                {
                                                    ExecuteSqlScriptNonQuery(script, connection, transaction);

                                                }
                                                catch (Exception)
                                                {
                                                    //ErrorEndUpdate("Error with truncate table! " + csvFile);
                                                    transaction.Rollback();
                                                    return false;
                                                }
                                                transaction.Commit();

                                                //then, insert
                                                script = $"copy \"{_baseTable}\"";

                                                int columnIndex = 0;
                                                if (_columnHeaders.Count == 1)
                                                {
                                                    script += $"(\"{_columnHeaders[0]}\")";
                                                }
                                                else
                                                {
                                                    foreach (var keyColumn in _columnHeaders)
                                                    {
                                                        if (columnIndex == 0)
                                                        {
                                                            script += $"(\"{keyColumn}\",";
                                                        }
                                                        else if (columnIndex == _columnHeaders.Count - 1)
                                                        {
                                                            script += $"\"{keyColumn}\")";
                                                        }
                                                        else
                                                        {
                                                            script += $"\"{keyColumn}\",";
                                                        }
                                                        columnIndex++;
                                                    }
                                                }
                                                script += $" from '{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, csvFile)}' CSV HEADER encoding 'UTF8' null 'null_string';"; ;
                                                var exitCode = ExecuteSqlScriptNonQuery(script, connection, transaction);
                                                if (!exitCode)
                                                {
                                                    //ErrorEndUpdate("Execute csv fail with CLRINS: " + csvFile);
                                                    return false;
                                                }
                                            }

                                        }
                                        else
                                        {
                                            Console.WriteLine(subFolder + " Csv file not exist!");
                                        }
                                    }
                                }
                                else
                                {
                                    //UpdateProgressContent(folderPath);
                                    Console.WriteLine(subFolder + " Header file not exist!");
                                }

                                #endregion
                                #region Run MstScript
                                // Excuted file sql Mst
                                string mstScript = Path.Combine(subFolder, UpdateConst.MST_SCRIPT);
                                if (CIUtil.IsFileExisting(mstScript))
                                {
                                    try
                                    {
                                        var existCode = ExecuteSqlFile(mstScript, connection, transaction);
                                        if (!existCode)
                                        {
                                            return false;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        // ErrorEndUpdate("Execute preMstScript fail: " + ex.Message);
                                        Console.WriteLine("Fail Run MstScript: " + ex.Message);
                                        return false;
                                    }
                                }
                                #endregion
                            }


                            // If everything is successful, commit the transaction
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            // If there's an error, rollback the transaction
                            transaction.Rollback();
                            Console.WriteLine($"Error executing SQL files: {ex.Message}");
                            // Save SYSTEM_CHANGE_LOG
                            using (NpgsqlCommand command = new NpgsqlCommand(QueryConstant.SaveSystemChangeLog, connection))
                            {
                                command.Parameters.AddWithValue("@FileName", filePathRun);
                                command.Parameters.AddWithValue("@IsPG", 1);
                                command.Parameters.AddWithValue("@IsDB", 1);
                                command.Parameters.AddWithValue("@IsMaster", 1);
                                command.Parameters.AddWithValue("@IsNote", 0);
                                command.Parameters.AddWithValue("@Status", 9);
                                command.Parameters.AddWithValue("@ErrMessage", ex.Message);
                                command.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                command.Parameters.AddWithValue("@UpdateDate", DateTime.Now);
                                command.Parameters.AddWithValue("@IsRun", 0);
                                command.Parameters.AddWithValue("@IsDrugPhoto", 0);

                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                return false;
            }

            catch (Exception ex)
            {
                return false;
                Console.WriteLine($"Error connecting to the database: {ex.Message}");
            }
        }

        private static void CreateTempTable(NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            if (string.IsNullOrEmpty(_tempTable))
            {
                _tempTable = _baseTable + "_temp";
            }
            // Clone from base table exclude constraints
            string script = "DROP TABLE IF EXISTS \"" + _tempTable + "\"; "
                + @"CREATE TABLE """ + _tempTable + @""" AS SELECT * FROM """ + _baseTable + @""" WHERE 1 = 2;";
            try
            {
                //Console.WriteLine(_moduleName, this, nameof(CreateTempTable), "Create temp table: " + script);
                // Execute the SQL command
                using (NpgsqlCommand command = new NpgsqlCommand(script, connection, transaction))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(_moduleName, this, nameof(CreateTempTable), ex, script);
                throw;
            }
            finally
            {
                _allTempTable.Add(_tempTable);
            }
        }

        private static List<string> GetAllPrimaryKeys(string tableName, NpgsqlConnection connection)
        {
            List<string> primaryKeyColumns = new List<string>();

            string script = @"SELECT KC.column_name 
                              FROM information_schema.table_constraints tc, information_schema.key_column_usage kc 
                              WHERE tc.constraint_type = 'PRIMARY KEY'
                                 AND kc.table_name = tc.table_name
                                 AND kc.table_name = @_baseTable";
            using (NpgsqlCommand command = new NpgsqlCommand(script, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@_baseTable", tableName);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string columnName = reader["column_name"].ToString();
                        primaryKeyColumns.Add(columnName);
                    }
                }
            }
            return primaryKeyColumns;
        }

        private static void MoveDataToBaseTable(NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            string script = string.Empty;
            try
            {
                void AddScript()
                {
                    for (int i = 0; i < _keyColumns.Count; i++)
                    {
                        script += "A.\"" + _keyColumns[i] + "\" = B.\"" + _keyColumns[i] + "\"";
                        if (i < _keyColumns.Count - 1)
                        {
                            script += " AND ";
                        }
                    }
                }
                if (_keyColumns.Count == 0)
                {
                    _keyColumns = GetAllPrimaryKeys(_baseTable, connection);
                }
                Console.WriteLine("KeyColumns: " + string.Join(",", _keyColumns.ToArray()));
                string msg = string.Empty;
                switch (_type.ToUpper())
                {
                    case "INSERT":
                    case "GENERATION_ADD":
                        script += "INSERT INTO \"" + _baseTable + "\"";
                        int columnIndex = 0;
                        script += "(";
                        string columnScript = string.Empty;
                        foreach (var columnName in _columnHeaders)
                        {
                            if (columnIndex == _columnHeaders.Count - 1)
                            {
                                columnScript += $"\"{columnName}\"";
                            }
                            else
                            {
                                columnScript += $"\"{columnName}\",";
                            }
                            columnIndex++;
                        }
                        script += columnScript;
                        script += ")";
                        script += $" SELECT {columnScript} FROM \"" + _tempTable + "\" B WHERE NOT EXISTS (SELECT 1 FROM \"" + _baseTable + "\" A WHERE ";
                        AddScript();
                        script += ")";
                        msg = "追加レコード数";
                        break;
                    case "DELETE":
                        script = "DELETE FROM \"" + _baseTable + "\" A WHERE EXISTS (SELECT 1 FROM \"" + _tempTable + "\" B WHERE ";
                        AddScript();
                        script += ")";
                        msg = "削除レコード数";
                        break;
                    case "DELINS":
                        script = "DELETE FROM \"" + _baseTable + "\" A WHERE EXISTS (SELECT 1 FROM \"" + _tempTable + "\" B WHERE ";
                        AddScript();
                        script += ");";
                        script += "INSERT INTO \"" + _baseTable + "\" SELECT * FROM \"" + _tempTable + "\" B WHERE NOT EXISTS (SELECT 1 FROM \"" + _baseTable + "\" A WHERE ";
                        AddScript();
                        script += ")";
                        msg = "追加レコード数";
                        break;
                    case "UPDATE":
                        script = "UPDATE \"" + _baseTable + "\" A SET (";
                        for (int i = 0; i < _columnHeaders.Count; i++)
                        {
                            script += "\"" + _columnHeaders[i] + "\"";
                            if (i < _columnHeaders.Count - 1)
                            {
                                script += ",";
                            }
                        }
                        script += ") = (";
                        for (int i = 0; i < _columnHeaders.Count; i++)
                        {
                            script += "B.\"" + _columnHeaders[i] + "\"";
                            if (i < _columnHeaders.Count - 1)
                            {
                                script += ",";
                            }
                        }
                        script += ") FROM \"" + _tempTable + "\" B WHERE ";
                        AddScript();
                        msg = "更新レコード数";
                        break;
                }
                //Console.WriteLine(_moduleName, this, nameof(MoveDataToBaseTable), "Update master: " + script);

                int effectRecordCount = 0;
                using (NpgsqlCommand command = new NpgsqlCommand(script, connection))
                {
                    command.CommandType = CommandType.Text;
                    effectRecordCount = command.ExecuteNonQuery();
                }

                if (string.IsNullOrEmpty(msg))
                {
                    msg = "変更したレコード数";
                }
                //Console.WriteLine(_moduleName, this, nameof(MoveDataToBaseTable), msg + ": " + effectRecordCount);

                if (effectRecordCount > 0 && _type.ToUpper() == "GENERATION_ADD")
                {
                    switch (_baseTable)
                    {
                        case "TEN_MST":
                            EffectRecord.ChangeTenMstGeneration(connection, transaction, _tempTable);
                            break;
                        case "DENSI_HAIHAN_CUSTOM":
                            EffectRecord.ChangeDensiHaiHanCustomGeneration(connection, transaction, _tempTable);
                            break;
                        case "DENSI_HAIHAN_DAY":
                            EffectRecord.ChangeDensiHaiHanDayGeneration(connection, transaction, _tempTable);
                            break;
                        case "DENSI_HAIHAN_KARTE":
                            EffectRecord.ChangeDensiHaihanKarteGeneration(connection, transaction, _tempTable);
                            break;
                        case "DENSI_HAIHAN_MONTH":
                            EffectRecord.ChangeDensiHaihanMonthGeneration(connection, transaction, _tempTable);
                            break;
                        case "DENSI_HAIHAN_WEEK":
                            EffectRecord.ChangeDensiHaihanWeekGeneration(connection, transaction, _tempTable);
                            break;
                        case "DENSI_HOJYO":
                            EffectRecord.ChangeDensiHojyoGeneration(connection, transaction, _tempTable);
                            break;
                        case "DENSI_HOUKATU":
                            EffectRecord.ChangeDensiHoukatuGeneration(connection, transaction, _tempTable);
                            break;
                        case "DENSI_HOUKATU_GRP":
                            EffectRecord.ChangeDensiHoukatuGrpGeneration(connection, transaction, _tempTable);
                            break;
                        case "DENSI_SANTEI_KAISU":
                            EffectRecord.ChangeDensiSanteiKaisuGeneration(connection, transaction, _tempTable);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(_moduleName, this, nameof(MoveDataToBaseTable), ex, script);
                throw;
            }
        }

        private static bool ExecuteSqlScriptNonQuery(string sqlScript, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {

            try
            {
                // Execute the SQL command
                using (NpgsqlCommand command = new NpgsqlCommand(sqlScript, connection, transaction))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Execute SqlFile fail: " + ex.Message);
                return false;
            }
        }

        public static bool ExecuteSqlFile(string filePath, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            try
            {
                // Read the content of the SQL file
                string sqlScript;
                using (StreamReader reader = new StreamReader(filePath))
                {
                    sqlScript = reader.ReadToEnd();
                }

                // Execute the SQL command
                using (NpgsqlCommand command = new NpgsqlCommand("", connection, transaction))
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Execute Sql Files: {ex.Message}");
                return false;
            }
        }

        public class TempGenerationMst
        {
            public int Hp_Id { get; set; }

            public string Houkatu_Grp_No { get; set; }

            public string Item_Cd { get; set; }

            public string Item_Cd1 { get; set; }

            public string Item_Cd2 { get; set; }

            public int Unit_Cd { get; set; }

            public int User_Setting { get; set; }

            public int Start_Date { get; set; }

            public int End_Date { get; set; }
        }
    }
}
