using Entity.Tenant;
using Helper.Common;
using Npgsql;
using System.Data;
using static AWSSDK.Common.UpdateDataTenant;

namespace AWSSDK.Common
{
    public static class EffectRecord
    {

        const string TenMstTableName = "TenMst";
        const string DensiHaiHanCustomTableName = "DENSI_HAIHAN_CUSTOM";
        const string DensiHaihanDayTableName = "DENSI_HAIHAN_Day";
        const string DensiHaihanKarteTableName = "DENSI_HAIHAN_KARTE";
        const string DensiHaihanMonthTableName = "DENSI_HAIHAN_MONTH";
        const string DensiHaihanWeekTableName = "DENSI_HAIHAN_WEEK";
        const string DensiHojyoTableName = "DENSI_HOJYO";
        const string DensiHoukatuTableName = "DENSI_HOUKATU";
        const string DensiHoukatuGrpTableName = "DENSI_HOUKATU_GRP";
        const string DensiSanteiKaisuTableName = "DENSI_SANTEI_KAISU";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="tempTable"></param>
        /// <returns></returns>
        static List<TempGenerationMst> GetTempGenerationMsts(NpgsqlConnection connection, string tempTable)
        {
            List<TempGenerationMst> tempTenMsts = new List<TempGenerationMst>();


            string script = $"SELECT * FROM {tempTable}";

            using (NpgsqlCommand command = new NpgsqlCommand(script, connection))
            {
                command.CommandType = CommandType.Text;

                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TempGenerationMst tempTenMst = new TempGenerationMst
                        {
                            Hp_Id = Convert.ToInt32(reader["Hp_Id"]),
                            Houkatu_Grp_No = Convert.ToString(reader["Houkatu_Grp_No"]),
                            Item_Cd = Convert.ToString(reader["Item_Cd"]),
                            Item_Cd1 = Convert.ToString(reader["Item_Cd1"]),
                            Item_Cd2 = Convert.ToString(reader["Item_Cd2"]),
                            Unit_Cd = Convert.ToInt32(reader["Unit_Cd"]),
                            User_Setting = Convert.ToInt32(reader["User_Setting"]),
                            Start_Date = Convert.ToInt32(reader["Start_Date"]),
                            End_Date = Convert.ToInt32(reader["End_Date"]),
                        };

                        tempTenMsts.Add(tempTenMst);
                    }
                }
            }

            return tempTenMsts;
        }

        private static T MapToEntity<T>(NpgsqlDataReader reader) where T : new()
        {
            T entity = new T();

            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                try
                {
                    // Try to get the column index using GetOrdinal
                    int columnIndex = reader.GetOrdinal(property.Name);

                    // Check if the column is not DBNull
                    if (!reader.IsDBNull(columnIndex))
                    {
                        var value = reader[property.Name];
                        property.SetValue(entity, value is DBNull ? null : value);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading column {property.Name}: {ex.Message}");
                }
            }

            return entity;
        }

        private static T GetEntityByCriteria<T>(NpgsqlConnection connection, string tableName, string[] columns, object[] values, string? optionScript = null) where T : new()
        {
            T entity = default(T);

            if (columns.Length != values.Length)
            {
                throw new ArgumentException("Columns and values arrays must have the same length.");
            }

            string selectScript = $"SELECT * FROM {tableName} WHERE ";

            for (int i = 0; i < columns.Length; i++)
            {
                selectScript += $"{columns[i]} = @{columns[i]}";

                if (i < columns.Length - 1)
                {
                    selectScript += " AND ";
                }
            }

            if (string.IsNullOrEmpty(selectScript))
            {
                selectScript += $" {optionScript}";
            }
            selectScript += " LIMIT 1";

            using (NpgsqlCommand selectCommand = new NpgsqlCommand(selectScript, connection))
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    selectCommand.Parameters.AddWithValue($"@{columns[i]}", values[i]);
                }

                using (NpgsqlDataReader reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        entity = MapToEntity<T>(reader);
                    }
                }
            }

            return entity;
        }

        private static void DeleteEntitiesByCriteria<T>(NpgsqlConnection connection, NpgsqlTransaction transaction, string tableName, string[] columns, List<object[]> valuesList)
        {

            using (NpgsqlCommand deleteCommand = new NpgsqlCommand("", connection, transaction))
            {
                for (int i = 0; i < valuesList.Count; i++)
                {
                    object[] values = valuesList[i];

                    string deleteScript = $"DELETE FROM {tableName} WHERE {GetWhereClause(columns, i)}";
                    deleteCommand.CommandText = deleteScript;

                    AddParameters(deleteCommand, columns, values, i);

                    deleteCommand.ExecuteNonQuery();
                }
            }

            string GetWhereClause(string[] columns, int index)
            {
                return string.Join(" AND ", columns.Select((column, i) => $"{column} = @{column}_{index}"));
            }

            void AddParameters(NpgsqlCommand command, string[] columns, object[] values, int index)
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    command.Parameters.AddWithValue($"@{columns[i]}_{index}", values[i]);
                }
            }
        }

        private static TenMst GetLatestTenMst(NpgsqlConnection connection, TempGenerationMst tempTenMst)
        {
            string[] columns = { "HP_ID", "ItemCd", "StartDate" };
            object[] values = { tempTenMst.Hp_Id, tempTenMst.Item_Cd, tempTenMst.Start_Date };
            string optionScript = " ORDER BY StartDate DESC";
            return GetEntityByCriteria<TenMst>(connection, TenMstTableName, columns, values, optionScript);
        }

        private static TenMst GetTenMst(NpgsqlConnection connection, TempGenerationMst tempTenMst)
        {
            string[] columns = { "HP_ID", "ITEM_CD", "START_DATE" };
            object[] values = { tempTenMst.Hp_Id, tempTenMst.Item_Cd, tempTenMst.Start_Date };

            return GetEntityByCriteria<TenMst>(connection, TenMstTableName, columns, values);
        }

        private static TenMst GetTenMstMother(NpgsqlConnection connection, TenMst tenMst)
        {
            string[] columns = { "HP_ID", "ITEM_CD", "START_DATE" };
            object[] values = { tenMst.HpId, tenMst.ItemCd, tenMst.StartDate };

            return GetEntityByCriteria<TenMst>(connection, TenMstTableName, columns, values);
        }

        private static void DeleteTenMsts(NpgsqlConnection connection, NpgsqlTransaction transaction, List<TenMst> tenMsts)
        {
            List<object[]> valuesList = new List<object[]>();

            string[] columns = { "HP_ID", "ITEM_CD", "START_DATE" };

            foreach (TenMst tenMst in tenMsts)
            {
                valuesList.Add(new object[] { tenMst.HpId, tenMst.ItemCd, tenMst.StartDate });
            }

            DeleteEntitiesByCriteria<TenMst>(connection, transaction, TenMstTableName, columns, valuesList);
        }

        private static T GetDensiHaihan<T>(NpgsqlConnection connection, TempGenerationMst tempTenMst, string tableName) where T : new()
        {
            string[] columns = { "HP_ID", "ITEM_CD1", "ITEM_CD2", "USER_SETTING", "START_DATE" };
            object[] values = { tempTenMst.Hp_Id, tempTenMst.Item_Cd1, tempTenMst.Item_Cd2, tempTenMst.User_Setting, tempTenMst.Start_Date };

            return GetEntityByCriteria<T>(connection, tableName, columns, values);
        }

        private static T GetLatestDensiHaihan<T>(NpgsqlConnection connection, TempGenerationMst tempTenMst, string tableName) where T : new()
        {
            string[] columns = { "HP_ID", "ITEM_CD1", "ITEM_CD2", "USER_SETTING", "START_DATE" };
            object[] values = { tempTenMst.Hp_Id, tempTenMst.Item_Cd1, tempTenMst.Item_Cd2, tempTenMst.User_Setting, tempTenMst.Start_Date };
            string optionScript = " ORDER BY StartDate DESC";
            return GetEntityByCriteria<T>(connection, tableName, columns, values, optionScript);
        }

        private static void DeleteDensiHaihan<T>(NpgsqlConnection connection, NpgsqlTransaction transaction, List<T> entities, string tableName)
        {
            List<object[]> valuesList = new List<object[]>();

            var columns = new string[] { "ID", "HP_ID", "ITEM_CD1", "SEQ_NO", "USER_SETTING" };

            foreach (var entity in entities)
            {
                var id = typeof(T).GetProperty("Id").GetValue(entity);
                var hpId = typeof(T).GetProperty("HpId").GetValue(entity);
                var itemCd1 = typeof(T).GetProperty("ItemCd1").GetValue(entity);
                var seqNo = typeof(T).GetProperty("SeqNo").GetValue(entity);
                var userSetting = typeof(T).GetProperty("UserSetting").GetValue(entity);

                valuesList.Add(new object[] { id, hpId, itemCd1, seqNo, userSetting });
            }

            DeleteEntitiesByCriteria<T>(connection, transaction, tableName, columns, valuesList);
        }

        private static DensiHoukatu GetDensiHoukatu(NpgsqlConnection connection, TempGenerationMst tempTenMst)
        {
            string[] columns = { "HP_ID", "ITEM_CD", "USER_SETTING", "START_DATE" };
            object[] values = { tempTenMst.Hp_Id, tempTenMst.Item_Cd, tempTenMst.User_Setting, tempTenMst.Start_Date };

            return GetEntityByCriteria<DensiHoukatu>(connection, DensiHoukatuTableName, columns, values);
        }

        private static DensiHoukatu GetLatestDensiHoukatu(NpgsqlConnection connection, TempGenerationMst tempTenMst)
        {
            string[] columns = { "HP_ID", "ITEM_CD", "USER_SETTING", "START_DATE" };
            object[] values = { tempTenMst.Hp_Id, tempTenMst.Item_Cd, tempTenMst.User_Setting, tempTenMst.Start_Date };
            string optionScript = " ORDER BY StartDate DESC";
            return GetEntityByCriteria<DensiHoukatu>(connection, DensiHoukatuTableName, columns, values, optionScript);
        }

        private static void DeleteDensiHoukatu(NpgsqlConnection connection, NpgsqlTransaction transaction, List<DensiHoukatu> densiHoukatus)
        {
            List<object[]> valuesList = new List<object[]>();

            string[] columns = { "HP_ID", "ITEM_CD", "START_DATE", "SEQ_NO", "USER_SETTING" };

            foreach (DensiHoukatu item in densiHoukatus)
            {
                valuesList.Add(new object[] { item.HpId, item.ItemCd, item.StartDate, item.SeqNo, item.UserSetting });
            }

            DeleteEntitiesByCriteria<DensiHoukatu>(connection, transaction, DensiHoukatuTableName, columns, valuesList);
        }

        private static DensiHojyo GetLatestDensiHojyo(NpgsqlConnection connection, TempGenerationMst tempTenMst)
        {
            string[] columns = { "HP_ID", "ItemCd", "StartDate" };
            object[] values = { tempTenMst.Hp_Id, tempTenMst.Item_Cd, tempTenMst.Start_Date };
            string optionScript = " ORDER BY StartDate DESC";
            return GetEntityByCriteria<DensiHojyo>(connection, DensiHojyoTableName, columns, values, optionScript);
        }

        private static DensiHojyo GetDensiHojyo(NpgsqlConnection connection, TempGenerationMst tempTenMst)
        {
            string[] columns = { "HP_ID", "ITEM_CD", "START_DATE" };
            object[] values = { tempTenMst.Hp_Id, tempTenMst.Item_Cd, tempTenMst.Start_Date };

            return GetEntityByCriteria<DensiHojyo>(connection, DensiHojyoTableName, columns, values);
        }

        private static void DeleteDensiHojyos(NpgsqlConnection connection, NpgsqlTransaction transaction, List<DensiHojyo> densiHojyos)
        {
            List<object[]> valuesList = new List<object[]>();

            string[] columns = { "HP_ID", "ITEM_CD", "START_DATE" };

            foreach (DensiHojyo item in densiHojyos)
            {
                valuesList.Add(new object[] { item.HpId, item.ItemCd, item.StartDate });
            }

            DeleteEntitiesByCriteria<DensiHojyo>(connection, transaction, DensiHojyoTableName, columns, valuesList);
        }

        private static DensiHoukatuGrp GetDensiHoukatuGrp(NpgsqlConnection connection, TempGenerationMst tempTenMst)
        {
            string[] columns = { "HP_ID", "HOUKATU_GRP_NO", "ITEM_CD", "USER_SETTING", "START_DATE" };
            object[] values = { tempTenMst.Hp_Id, tempTenMst.Houkatu_Grp_No, tempTenMst.Item_Cd, tempTenMst.User_Setting, tempTenMst.Start_Date };

            return GetEntityByCriteria<DensiHoukatuGrp>(connection, DensiHoukatuGrpTableName, columns, values);
        }

        private static DensiHoukatuGrp GetLatestDensiHoukatuGrp(NpgsqlConnection connection, TempGenerationMst tempTenMst)
        {
            string[] columns = { "HP_ID", "HOUKATU_GRP_NO", "ITEM_CD", "USER_SETTING", "START_DATE" };
            object[] values = { tempTenMst.Hp_Id, tempTenMst.Houkatu_Grp_No, tempTenMst.Item_Cd, tempTenMst.User_Setting, tempTenMst.Start_Date };
            string optionScript = " ORDER BY StartDate DESC";
            return GetEntityByCriteria<DensiHoukatuGrp>(connection, DensiHoukatuGrpTableName, columns, values, optionScript);
        }

        private static void DeleteDensiHoukatuGrp(NpgsqlConnection connection, NpgsqlTransaction transaction, List<DensiHoukatuGrp> densiHoukatus)
        {
            List<object[]> valuesList = new List<object[]>();

            string[] columns = { "HP_ID", "HOUKATU_GRP_NO", "ITEM_CD", "START_DATE", "SEQ_NO", "USER_SETTING" };

            foreach (DensiHoukatuGrp item in densiHoukatus)
            {
                valuesList.Add(new object[] { item.HpId, item.HoukatuGrpNo, item.ItemCd, item.StartDate, item.SeqNo, item.UserSetting });
            }

            DeleteEntitiesByCriteria<DensiHoukatuGrp>(connection, transaction, DensiHoukatuGrpTableName, columns, valuesList);
        }

        private static DensiSanteiKaisu GetDensiSanteiKaisu(NpgsqlConnection connection, TempGenerationMst tempTenMst)
        {
            string[] columns = { "HP_ID", "ITEM_CD", "UNIT_CD", "USER_SETTING", "START_DATE" };
            object[] values = { tempTenMst.Hp_Id, tempTenMst.Item_Cd, tempTenMst.Unit_Cd, tempTenMst.User_Setting, tempTenMst.Start_Date };

            return GetEntityByCriteria<DensiSanteiKaisu>(connection, DensiHoukatuGrpTableName, columns, values);
        }

        private static DensiSanteiKaisu GetLatestDensiSanteiKaisu(NpgsqlConnection connection, TempGenerationMst tempTenMst)
        {
            string[] columns = { "HP_ID", "ITEM_CD", "UNIT_CD", "USER_SETTING", "START_DATE" };
            object[] values = { tempTenMst.Hp_Id, tempTenMst.Item_Cd, tempTenMst.Unit_Cd, tempTenMst.User_Setting, tempTenMst.Start_Date };
            string optionScript = " ORDER BY StartDate DESC";
            return GetEntityByCriteria<DensiSanteiKaisu>(connection, DensiSanteiKaisuTableName, columns, values, optionScript);
        }

        private static void DeleteDensiSanteiKaisus(NpgsqlConnection connection, NpgsqlTransaction transaction, List<DensiSanteiKaisu> densiHoukatus)
        {
            List<object[]> valuesList = new List<object[]>();

            string[] columns = { "HP_ID", "ITEM_CD", "SEQ_NO", "USER_SETTING", "ID" };

            foreach (DensiSanteiKaisu item in densiHoukatus)
            {
                valuesList.Add(new object[] { item.HpId, item.ItemCd, item.SeqNo, item.UserSetting, item.Id });
            }

            DeleteEntitiesByCriteria<DensiSanteiKaisu>(connection, transaction, DensiSanteiKaisuTableName, columns, valuesList);
        }


        public static void ChangeTenMstGeneration(NpgsqlConnection connection, NpgsqlTransaction transaction, string tempTable)
        {
            try
            {
                List<TempGenerationMst> tempTenMsts = GetTempGenerationMsts(connection, tempTable);

                var deleteTenMsts = new List<TenMst>();
                foreach (var tempTenMst in tempTenMsts)
                {
                    var latestTenMst = GetLatestTenMst(connection, tempTenMst);

                    var newTenMst = GetTenMst(connection, tempTenMst);

                    // Update fields of tenMst
                    if (latestTenMst != null && newTenMst != null)
                    {
                        newTenMst.KanaName3 = latestTenMst.KanaName3;
                        newTenMst.KanaName4 = latestTenMst.KanaName4;
                        newTenMst.KanaName5 = latestTenMst.KanaName5;
                        newTenMst.KanaName6 = latestTenMst.KanaName6;
                        newTenMst.KanaName7 = latestTenMst.KanaName7;
                        newTenMst.RyosyuName = latestTenMst.RyosyuName;
                        newTenMst.CnvUnitName = latestTenMst.CnvUnitName;
                        newTenMst.OdrTermVal = latestTenMst.OdrTermVal;
                        newTenMst.CnvTermVal = latestTenMst.CnvTermVal;
                        newTenMst.DefaultVal = latestTenMst.DefaultVal;
                        newTenMst.IsAdopted = latestTenMst.IsAdopted;
                        newTenMst.AgeCheck = latestTenMst.AgeCheck;
                        newTenMst.IsNosearch = latestTenMst.IsNosearch;
                        newTenMst.IsNodspKarte = latestTenMst.IsNodspKarte;
                        newTenMst.IsNodspRece = latestTenMst.IsNodspRece;
                        newTenMst.IsNodspPaperRece = latestTenMst.IsNodspPaperRece;
                        newTenMst.IsNodspRyosyu = latestTenMst.IsNodspRyosyu;
                        newTenMst.JihiSbt = latestTenMst.JihiSbt;
                        newTenMst.KazeiKbn = latestTenMst.KazeiKbn;
                        newTenMst.YohoKbn = latestTenMst.YohoKbn;
                        newTenMst.FukuyoRise = latestTenMst.FukuyoRise;
                        newTenMst.FukuyoMorning = latestTenMst.FukuyoMorning;
                        newTenMst.FukuyoDaytime = latestTenMst.FukuyoDaytime;
                        newTenMst.FukuyoNight = latestTenMst.FukuyoNight;
                        newTenMst.FukuyoSleep = latestTenMst.FukuyoSleep;
                        newTenMst.SuryoRoundupKbn = latestTenMst.SuryoRoundupKbn;
                        newTenMst.KensaFukusuSantei = latestTenMst.KensaFukusuSantei;
                        newTenMst.SanteigaiKbn = latestTenMst.SanteigaiKbn;
                        newTenMst.KensaItemCd = latestTenMst.KensaItemCd;
                        newTenMst.KensaItemSeqNo = latestTenMst.KensaItemSeqNo;
                        newTenMst.RenkeiCd1 = latestTenMst.RenkeiCd1;
                        newTenMst.RenkeiCd2 = latestTenMst.RenkeiCd2;
                        newTenMst.IsNodspYakutai = latestTenMst.IsNodspYakutai;
                        newTenMst.ZaikeiPoint = latestTenMst.ZaikeiPoint;
                        newTenMst.KensaLabel = latestTenMst.KensaLabel;
                        newTenMst.RousaiKbn = latestTenMst.RousaiKbn;
                        newTenMst.SisiKbn = latestTenMst.SisiKbn;
                        newTenMst.SaiketuKbn = latestTenMst.SaiketuKbn;
                        newTenMst.KohatuKbn = latestTenMst.KohatuKbn;
                        newTenMst.IsDeleted = latestTenMst.IsDeleted;

                        var tenMstMother = GetTenMstMother(connection, latestTenMst);
                        if (tenMstMother != null)
                        {
                            if (latestTenMst.Name != tenMstMother.Name)
                            {
                                newTenMst.Name = latestTenMst.Name;
                            }
                            if (latestTenMst.OdrUnitName != tenMstMother.OdrUnitName)
                            {
                                newTenMst.OdrUnitName = latestTenMst.OdrUnitName;
                            }
                            if (latestTenMst.CmtKbn != tenMstMother.CmtKbn)
                            {
                                newTenMst.CmtKbn = latestTenMst.CmtKbn;
                            }
                            if (latestTenMst.ChusyaDrugSbt != tenMstMother.ChusyaDrugSbt)
                            {
                                newTenMst.ChusyaDrugSbt = latestTenMst.ChusyaDrugSbt;
                            }
                        }
                    }

                    bool allowDelete = false;
                    if (latestTenMst == null)
                    {
                        //1
                    }
                    else if (latestTenMst.EndDate > tempTenMst.End_Date)
                    {
                        if (tempTenMst.Start_Date <= latestTenMst.StartDate)
                        {
                            allowDelete = true;
                        }
                        else
                        {
                            latestTenMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempTenMst.Start_Date).AddDays(-1));
                        }
                    }
                    else if (latestTenMst.EndDate == tempTenMst.End_Date)
                    {
                        if (latestTenMst.StartDate < tempTenMst.Start_Date)
                        {
                            latestTenMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempTenMst.Start_Date).AddDays(-1));
                            //1
                        }
                        else if (latestTenMst.StartDate >= tempTenMst.Start_Date)
                        {
                            allowDelete = true;
                        }
                    }
                    else if (latestTenMst.EndDate < tempTenMst.End_Date)
                    {
                        if (tempTenMst.Start_Date <= latestTenMst.StartDate)
                        {
                            allowDelete = true;
                        }
                        else if (tempTenMst.Start_Date <= latestTenMst.EndDate)
                        {
                            latestTenMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempTenMst.Start_Date).AddDays(-1));
                            //1
                        }
                        else if (tempTenMst.Start_Date > latestTenMst.EndDate)
                        {
                            //1
                        }
                        else
                        {
                            allowDelete = true;
                        }
                    }
                    if (allowDelete)
                    {
                        var delTempTenMst = GetTenMst(connection, tempTenMst);
                        if (delTempTenMst != null)
                        {
                            deleteTenMsts.Add(delTempTenMst);
                        }
                    }
                }
                if (deleteTenMsts.Count > 0)
                {
                    DeleteTenMsts(connection, transaction, deleteTenMsts);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(_moduleName, this, nameof(ChangeTenMstGeneration), ex);
                throw;
            }
        }

        public static void ChangeDensiHaiHanCustomGeneration(NpgsqlConnection connection, NpgsqlTransaction transaction, string tempTable)
        {
            try
            {
                List<TempGenerationMst> tempMsts = GetTempGenerationMsts(connection, tempTable);

                var deleteMsts = new List<DensiHaihanCustom>();
                foreach (var tempMst in tempMsts)
                {
                    var latestMst = GetDensiHaihan<DensiHaihanCustom>(connection, tempMst, DensiHaiHanCustomTableName);

                    var newEntity = GetDensiHaihan<DensiHaihanCustom>(connection, tempMst, DensiHaiHanCustomTableName);
                    if (latestMst != null && newEntity != null)
                    {
                        newEntity.IsInvalid = latestMst.IsInvalid;
                    }

                    bool allowDelete = false;
                    if (latestMst == null)
                    {
                        //1
                    }
                    else if (latestMst.EndDate > tempMst.End_Date)
                    {
                        allowDelete = true;
                    }
                    else if (latestMst.EndDate == tempMst.End_Date)
                    {
                        if (latestMst.StartDate < tempMst.Start_Date)
                        {
                            latestMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempMst.Start_Date).AddDays(-1));
                            //1
                        }
                        else if (latestMst.StartDate > tempMst.Start_Date)
                        {
                            allowDelete = true;
                        }
                    }
                    else if (latestMst.EndDate < tempMst.End_Date)
                    {
                        if (tempMst.Start_Date < latestMst.StartDate)
                        {
                            allowDelete = true;
                        }
                        else if (tempMst.Start_Date <= latestMst.EndDate)
                        {
                            latestMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempMst.Start_Date).AddDays(-1));
                            //1
                        }
                        else if (tempMst.Start_Date == CIUtil.DateTimeToInt(CIUtil.IntToDate(latestMst.EndDate).AddDays(1)))
                        {
                            //1
                        }
                        else
                        {
                            allowDelete = true;
                        }
                    }
                    if (allowDelete)
                    {
                        var delTempMst = GetDensiHaihan<DensiHaihanCustom>(connection, tempMst, DensiHaiHanCustomTableName);
                        if (delTempMst != null)
                        {
                            deleteMsts.Add(delTempMst);
                        }
                    }
                }
                if (deleteMsts.Count > 0)
                {
                    DeleteDensiHaihan<DensiHaihanCustom>(connection, transaction, deleteMsts, DensiHaiHanCustomTableName);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(_moduleName, this, nameof(ChangeDensiHaiHanCustomGeneration), ex);
                throw;
            }
        }

        public static void ChangeDensiHaiHanDayGeneration(NpgsqlConnection connection, NpgsqlTransaction transaction, string tempTable)
        {
            try
            {
                List<TempGenerationMst> tempMsts = GetTempGenerationMsts(connection, tempTable);

                var deleteMsts = new List<DensiHaihanDay>();
                foreach (var tempMst in tempMsts)
                {
                    var latestMst = GetLatestDensiHaihan<DensiHaihanDay>(connection, tempMst, DensiHaihanDayTableName);

                    var newEntity = GetDensiHaihan<DensiHaihanDay>(connection, tempMst, DensiHaihanDayTableName);
                    if (latestMst != null && newEntity != null)
                    {
                        newEntity.IsInvalid = latestMst.IsInvalid;
                    }

                    bool allowDelete = false;
                    if (latestMst == null)
                    {
                        //1
                    }
                    else if (latestMst.EndDate > tempMst.End_Date)
                    {
                        allowDelete = true;
                    }
                    else if (latestMst.EndDate == tempMst.End_Date)
                    {
                        if (latestMst.StartDate < tempMst.Start_Date)
                        {
                            latestMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempMst.Start_Date).AddDays(-1));
                            //1
                        }
                        else if (latestMst.StartDate > tempMst.Start_Date)
                        {
                            allowDelete = true;
                        }
                    }
                    else if (latestMst.EndDate < tempMst.End_Date)
                    {
                        if (tempMst.Start_Date < latestMst.StartDate)
                        {
                            allowDelete = true;
                        }
                        else if (tempMst.Start_Date <= latestMst.EndDate)
                        {
                            latestMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempMst.Start_Date).AddDays(-1));
                            //1
                        }
                        else if (tempMst.Start_Date == CIUtil.DateTimeToInt(CIUtil.IntToDate(latestMst.EndDate).AddDays(1)))
                        {
                            //1
                        }
                        else
                        {
                            allowDelete = true;
                        }
                    }
                    if (allowDelete)
                    {
                        var delTempMst = GetDensiHaihan<DensiHaihanDay>(connection, tempMst, DensiHaihanDayTableName);
                        if (delTempMst != null)
                        {
                            deleteMsts.Add(delTempMst);
                        }
                    }
                }
                if (deleteMsts.Count > 0)
                {
                    DeleteDensiHaihan<DensiHaihanDay>(connection, transaction, deleteMsts, DensiHaihanDayTableName);
                }
            }
            catch (Exception ex)
            {
                // Console.WriteLine(_moduleName, this, nameof(ChangeDensiHaiHanDayGeneration), ex);
                throw;
            }
        }

        public static void ChangeDensiHaihanKarteGeneration(NpgsqlConnection connection, NpgsqlTransaction transaction, string tempTable)
        {
            try
            {
                List<TempGenerationMst> tempMsts = GetTempGenerationMsts(connection, tempTable);
                var deleteMsts = new List<DensiHaihanKarte>();
                foreach (var tempMst in tempMsts)
                {
                    var latestMst = GetLatestDensiHaihan<DensiHaihanKarte>(connection, tempMst, DensiHaihanKarteTableName);

                    var newEntity = GetDensiHaihan<DensiHaihanKarte>(connection, tempMst, DensiHaihanKarteTableName);
                    if (latestMst != null && newEntity != null)
                    {
                        newEntity.IsInvalid = latestMst.IsInvalid;
                    }

                    bool allowDelete = false;
                    if (latestMst == null)
                    {
                        //1
                    }
                    else if (latestMst.EndDate > tempMst.End_Date)
                    {
                        allowDelete = true;
                    }
                    else if (latestMst.EndDate == tempMst.End_Date)
                    {
                        if (latestMst.StartDate < tempMst.Start_Date)
                        {
                            latestMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempMst.Start_Date).AddDays(-1));
                            //1
                        }
                        else if (latestMst.StartDate > tempMst.Start_Date)
                        {
                            allowDelete = true;
                        }
                    }
                    else if (latestMst.EndDate < tempMst.End_Date)
                    {
                        if (tempMst.Start_Date < latestMst.StartDate)
                        {
                            allowDelete = true;
                        }
                        else if (tempMst.Start_Date <= latestMst.EndDate)
                        {
                            latestMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempMst.Start_Date).AddDays(-1));
                            //1
                        }
                        else if (tempMst.Start_Date == CIUtil.DateTimeToInt(CIUtil.IntToDate(latestMst.EndDate).AddDays(1)))
                        {
                            //1
                        }
                        else
                        {
                            allowDelete = true;
                        }
                    }
                    if (allowDelete)
                    {
                        var delTempMst = GetDensiHaihan<DensiHaihanKarte>(connection, tempMst, DensiHaihanKarteTableName);
                        if (delTempMst != null)
                        {
                            deleteMsts.Add(delTempMst);
                        }
                    }
                }
                if (deleteMsts.Count > 0)
                {
                    DeleteDensiHaihan<DensiHaihanKarte>(connection, transaction, deleteMsts, DensiHaihanKarteTableName);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(_moduleName, this, nameof(ChangeDensiHaihanKarteGeneration), ex);
                throw;
            }
        }

        public static void ChangeDensiHaihanMonthGeneration(NpgsqlConnection connection, NpgsqlTransaction transaction, string tempTable)
        {
            try
            {
                List<TempGenerationMst> tempMsts = GetTempGenerationMsts(connection, tempTable);
                var deleteMsts = new List<DensiHaihanMonth>();
                foreach (var tempMst in tempMsts)
                {
                    var latestMst = GetLatestDensiHaihan<DensiHaihanMonth>(connection, tempMst, DensiHaihanMonthTableName);

                    var newEntity = GetDensiHaihan<DensiHaihanMonth>(connection, tempMst, DensiHaihanMonthTableName);

                    if (latestMst != null && newEntity != null)
                    {
                        newEntity.IsInvalid = latestMst.IsInvalid;
                    }

                    bool allowDelete = false;
                    if (latestMst == null)
                    {
                        //1
                    }
                    else if (latestMst.EndDate > tempMst.End_Date)
                    {
                        allowDelete = true;
                    }
                    else if (latestMst.EndDate == tempMst.End_Date)
                    {
                        if (latestMst.StartDate < tempMst.Start_Date)
                        {
                            latestMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempMst.Start_Date).AddDays(-1));
                            //1
                        }
                        else if (latestMst.StartDate > tempMst.Start_Date)
                        {
                            allowDelete = true;
                        }
                    }
                    else if (latestMst.EndDate < tempMst.End_Date)
                    {
                        if (tempMst.Start_Date < latestMst.StartDate)
                        {
                            allowDelete = true;
                        }
                        else if (tempMst.Start_Date <= latestMst.EndDate)
                        {
                            latestMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempMst.Start_Date).AddDays(-1));
                            //1
                        }
                        else if (tempMst.Start_Date == CIUtil.DateTimeToInt(CIUtil.IntToDate(latestMst.EndDate).AddDays(1)))
                        {
                            //1
                        }
                        else
                        {
                            allowDelete = true;
                        }
                    }
                    if (allowDelete)
                    {
                        var delTempMst = GetDensiHaihan<DensiHaihanMonth>(connection, tempMst, DensiHaihanMonthTableName);
                        if (delTempMst != null)
                        {
                            deleteMsts.Add(delTempMst);
                        }
                    }
                }
                if (deleteMsts.Count > 0)
                {
                    DeleteDensiHaihan<DensiHaihanMonth>(connection, transaction, deleteMsts, DensiHaihanMonthTableName);
                }
            }
            catch (Exception ex)
            {
                // Console.WriteLine(_moduleName, this, nameof(ChangeDensiHaihanMonthGeneration), ex);
                throw;
            }
        }

        public static void ChangeDensiHaihanWeekGeneration(NpgsqlConnection connection, NpgsqlTransaction transaction, string tempTable)
        {
            try
            {
                List<TempGenerationMst> tempMsts = GetTempGenerationMsts(connection, tempTable);

                var deleteMsts = new List<DensiHaihanWeek>();
                foreach (var tempMst in tempMsts)
                {
                    var latestMst = GetLatestDensiHaihan<DensiHaihanWeek>(connection, tempMst, DensiHaihanWeekTableName);

                    var newEntity = GetDensiHaihan<DensiHaihanWeek>(connection, tempMst, DensiHaihanWeekTableName);

                    if (latestMst != null && newEntity != null)
                    {
                        newEntity.IsInvalid = latestMst.IsInvalid;
                    }

                    bool allowDelete = false;
                    if (latestMst == null)
                    {
                        //1
                    }
                    else if (latestMst.EndDate > tempMst.End_Date)
                    {
                        allowDelete = true;
                    }
                    else if (latestMst.EndDate == tempMst.End_Date)
                    {
                        if (latestMst.StartDate < tempMst.Start_Date)
                        {
                            latestMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempMst.Start_Date).AddDays(-1));
                            //1
                        }
                        else if (latestMst.StartDate > tempMst.Start_Date)
                        {
                            allowDelete = true;
                        }
                    }
                    else if (latestMst.EndDate < tempMst.End_Date)
                    {
                        if (tempMst.Start_Date < latestMst.StartDate)
                        {
                            allowDelete = true;
                        }
                        else if (tempMst.Start_Date <= latestMst.EndDate)
                        {
                            latestMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempMst.Start_Date).AddDays(-1));
                            //1
                        }
                        else if (tempMst.Start_Date == CIUtil.DateTimeToInt(CIUtil.IntToDate(latestMst.EndDate).AddDays(1)))
                        {
                            //1
                        }
                        else
                        {
                            allowDelete = true;
                        }
                    }
                    if (allowDelete)
                    {
                        var delTempMst = GetDensiHaihan<DensiHaihanWeek>(connection, tempMst, DensiHaihanWeekTableName);
                        if (delTempMst != null)
                        {
                            deleteMsts.Add(delTempMst);
                        }
                    }
                }
                if (deleteMsts.Count > 0)
                {
                    DeleteDensiHaihan<DensiHaihanWeek>(connection, transaction, deleteMsts, DensiHaihanWeekTableName);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(_moduleName, this, nameof(ChangeDensiHaihanWeekGeneration), ex);
                throw;
            }
        }

        public static void ChangeDensiHojyoGeneration(NpgsqlConnection connection, NpgsqlTransaction transaction, string tempTable)
        {
            try
            {
                List<TempGenerationMst> tempMsts = GetTempGenerationMsts(connection, tempTable);

                var deleteMsts = new List<DensiHojyo>();
                foreach (var tempMst in tempMsts)
                {
                    var latestMst = GetLatestDensiHojyo(connection, tempMst);

                    bool allowDelete = false;
                    if (latestMst == null)
                    {
                        //1
                    }
                    else if (latestMst.EndDate > tempMst.End_Date)
                    {
                        allowDelete = true;
                    }
                    else if (latestMst.EndDate == tempMst.End_Date)
                    {
                        if (latestMst.StartDate < tempMst.Start_Date)
                        {
                            latestMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempMst.Start_Date).AddDays(-1));
                            //1
                        }
                        else if (latestMst.StartDate > tempMst.Start_Date)
                        {
                            allowDelete = true;
                        }
                    }
                    else if (latestMst.EndDate < tempMst.End_Date)
                    {
                        if (tempMst.Start_Date < latestMst.StartDate)
                        {
                            allowDelete = true;
                        }
                        else if (tempMst.Start_Date <= latestMst.EndDate)
                        {
                            latestMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempMst.Start_Date).AddDays(-1));
                            //1
                        }
                        else if (tempMst.Start_Date == CIUtil.DateTimeToInt(CIUtil.IntToDate(latestMst.EndDate).AddDays(1)))
                        {
                            //1
                        }
                        else
                        {
                            allowDelete = true;
                        }
                    }
                    if (allowDelete)
                    {
                        var delTempMst = GetDensiHojyo(connection, tempMst);
                        if (delTempMst != null)
                        {
                            deleteMsts.Add(delTempMst);
                        }
                    }
                }
                if (deleteMsts.Count > 0)
                {
                    DeleteDensiHojyos(connection, transaction, deleteMsts);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(_moduleName, this, nameof(ChangeDensiHojyoGeneration), ex);
                throw;
            }
        }

        public static void ChangeDensiHoukatuGeneration(NpgsqlConnection connection, NpgsqlTransaction transaction, string tempTable)
        {
            try
            {
                List<TempGenerationMst> tempMsts = GetTempGenerationMsts(connection, tempTable);

                var deleteMsts = new List<DensiHoukatu>();
                foreach (var tempMst in tempMsts)
                {
                    var latestMst = GetLatestDensiHoukatu(connection, tempMst);

                    var newEntity = GetDensiHoukatu(connection, tempMst);
                    if (latestMst != null && newEntity != null)
                    {
                        newEntity.IsInvalid = latestMst.IsInvalid;
                    }

                    bool allowDelete = false;
                    if (latestMst == null)
                    {
                        //1
                    }
                    else if (latestMst.EndDate > tempMst.End_Date)
                    {
                        allowDelete = true;
                    }
                    else if (latestMst.EndDate == tempMst.End_Date)
                    {
                        if (latestMst.StartDate < tempMst.Start_Date)
                        {
                            latestMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempMst.Start_Date).AddDays(-1));
                            //1
                        }
                        else if (latestMst.StartDate > tempMst.Start_Date)
                        {
                            allowDelete = true;
                        }
                    }
                    else if (latestMst.EndDate < tempMst.End_Date)
                    {
                        if (tempMst.Start_Date < latestMst.StartDate)
                        {
                            allowDelete = true;
                        }
                        else if (tempMst.Start_Date <= latestMst.EndDate)
                        {
                            latestMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempMst.Start_Date).AddDays(-1));
                            //1
                        }
                        else if (tempMst.Start_Date == CIUtil.DateTimeToInt(CIUtil.IntToDate(latestMst.EndDate).AddDays(1)))
                        {
                            //1
                        }
                        else
                        {
                            allowDelete = true;
                        }
                    }
                    if (allowDelete)
                    {
                        var delTempMst = GetDensiHoukatu(connection, tempMst);
                        if (delTempMst != null)
                        {
                            deleteMsts.Add(delTempMst);
                        }
                    }
                }
                if (deleteMsts.Count > 0)
                {
                    DeleteDensiHoukatu(connection, transaction, deleteMsts);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(_moduleName, this, nameof(ChangeDensiHoukatuGeneration), ex);
                throw;
            }
        }

        public static void ChangeDensiHoukatuGrpGeneration(NpgsqlConnection connection, NpgsqlTransaction transaction, string tempTable)
        {
            try
            {
                List<TempGenerationMst> tempMsts = GetTempGenerationMsts(connection, tempTable);

                var deleteMsts = new List<DensiHoukatuGrp>();
                foreach (var tempMst in tempMsts)
                {
                    var latestMst = GetLatestDensiHoukatuGrp(connection, tempMst);
                    var newEntity = GetDensiHoukatuGrp(connection, tempMst);
                    if (latestMst != null && newEntity != null)
                    {
                        newEntity.IsInvalid = latestMst.IsInvalid;
                    }

                    bool allowDelete = false;
                    if (latestMst == null)
                    {
                        //1
                    }
                    else if (latestMst.EndDate > tempMst.End_Date)
                    {
                        allowDelete = true;
                    }
                    else if (latestMst.EndDate == tempMst.End_Date)
                    {
                        if (latestMst.StartDate < tempMst.Start_Date)
                        {
                            latestMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempMst.Start_Date).AddDays(-1));
                            //1
                        }
                        else if (latestMst.StartDate > tempMst.Start_Date)
                        {
                            allowDelete = true;
                        }
                    }
                    else if (latestMst.EndDate < tempMst.End_Date)
                    {
                        if (tempMst.Start_Date < latestMst.StartDate)
                        {
                            allowDelete = true;
                        }
                        else if (tempMst.Start_Date <= latestMst.EndDate)
                        {
                            latestMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempMst.Start_Date).AddDays(-1));
                            //1
                        }
                        else if (tempMst.Start_Date == CIUtil.DateTimeToInt(CIUtil.IntToDate(latestMst.EndDate).AddDays(1)))
                        {
                            //1
                        }
                        else
                        {
                            allowDelete = true;
                        }
                    }
                    if (allowDelete)
                    {
                        var delTempMst = GetDensiHoukatuGrp(connection, tempMst);
                        if (delTempMst != null)
                        {
                            deleteMsts.Add(delTempMst);
                        }
                    }
                }
                if (deleteMsts.Count > 0)
                {
                    DeleteDensiHoukatuGrp(connection, transaction, deleteMsts);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(_moduleName, this, nameof(ChangeDensiHoukatuGrpGeneration), ex);
                throw;
            }
        }

        public static void ChangeDensiSanteiKaisuGeneration(NpgsqlConnection connection, NpgsqlTransaction transaction, string tempTable)
        {
            try
            {
                List<TempGenerationMst> tempMsts = GetTempGenerationMsts(connection, tempTable);

                var deleteMsts = new List<DensiSanteiKaisu>();
                foreach (var tempMst in tempMsts)
                {
                    var latestMst = GetLatestDensiSanteiKaisu(connection, tempMst);

                    var newEntity = GetDensiSanteiKaisu(connection, tempMst);

                    if (latestMst != null && newEntity != null)
                    {
                        newEntity.IsInvalid = latestMst.IsInvalid;
                    }

                    bool allowDelete = false;
                    if (latestMst == null)
                    {
                        //1
                    }
                    else if (latestMst.EndDate > tempMst.End_Date)
                    {
                        allowDelete = true;
                    }
                    else if (latestMst.EndDate == tempMst.End_Date)
                    {
                        if (latestMst.StartDate < tempMst.Start_Date)
                        {
                            latestMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempMst.Start_Date).AddDays(-1));
                            //1
                        }
                        else if (latestMst.StartDate > tempMst.Start_Date)
                        {
                            allowDelete = true;
                        }
                    }
                    else if (latestMst.EndDate < tempMst.End_Date)
                    {
                        if (tempMst.Start_Date < latestMst.StartDate)
                        {
                            allowDelete = true;
                        }
                        else if (tempMst.Start_Date <= latestMst.EndDate)
                        {
                            latestMst.EndDate = CIUtil.DateTimeToInt(CIUtil.IntToDate(tempMst.Start_Date).AddDays(-1));
                            //1
                        }
                        else if (tempMst.Start_Date == CIUtil.DateTimeToInt(CIUtil.IntToDate(latestMst.EndDate).AddDays(1)))
                        {
                            //1
                        }
                        else
                        {
                            allowDelete = true;
                        }
                    }
                    if (allowDelete)
                    {
                        var delTempMst = GetDensiSanteiKaisu(connection, tempMst);
                        if (delTempMst != null)
                        {
                            deleteMsts.Add(delTempMst);
                        }
                    }
                }
                if (deleteMsts.Count > 0)
                {
                    DeleteDensiSanteiKaisus(connection, transaction, deleteMsts);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(_moduleName, this, nameof(ChangeDensiSanteiKaisuGeneration), ex);
                throw;
            }
        }

    }
}
