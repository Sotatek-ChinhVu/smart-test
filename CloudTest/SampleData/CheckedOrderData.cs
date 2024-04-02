using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity.Tenant;

namespace CloudUnitTest.SampleData
{
    public static class CheckedOrderData
    {
        //Create COM Objects. Create a COM object for everything that is referenced
        public static List<RaiinInf> ReadRainInf(int randomKey)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            int count = 1;
            string fileName = Path.Combine(rootPath, "SampleData", "CheckedOrderDataSample.xlsx");
            var raiinInfs = new List<RaiinInf>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {

                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "RAIIN_INF").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var raiinInf = new RaiinInf();
                        raiinInf.CreateId = 1;
                        raiinInf.CreateDate = DateTime.UtcNow;
                        raiinInf.UpdateId = 1;
                        raiinInf.UpdateDate = DateTime.UtcNow;
                        foreach (var c in r.Elements<Cell>())
                        {
                            text = c.CellValue?.Text ?? string.Empty;
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
                            }
                            var columnName = GetColumnName(c.CellReference?.ToString() ?? string.Empty);

                            switch (columnName)
                            {
                                case "A":
                                    int.TryParse(text, out int hpId);
                                    raiinInf.HpId = hpId;
                                    break;
                                case "B":
                                    raiinInf.RaiinNo = long.MaxValue - randomKey - count;
                                    break;
                                case "C":
                                    var ptId = long.Parse(text, System.Globalization.CultureInfo.InvariantCulture);
                                    raiinInf.PtId = ptId;
                                    break;
                                case "D":
                                    int.TryParse(text, out int sinDate);
                                    raiinInf.SinDate = sinDate;
                                    break;
                                case "E":
                                    var oyaRaiinNo = long.Parse(text, System.Globalization.CultureInfo.InvariantCulture);
                                    raiinInf.OyaRaiinNo = oyaRaiinNo;
                                    break;
                                case "F":
                                    int.TryParse(text, out int status);
                                    raiinInf.Status = status;
                                    break;
                                case "G":
                                    int.TryParse(text, out int isYokaku);
                                    raiinInf.IsYoyaku = isYokaku;
                                    break;
                                case "H":
                                    int.TryParse(text, out int isYokakuId);
                                    raiinInf.YoyakuId = isYokakuId;
                                    break;
                                case "I":
                                    int.TryParse(text, out int uketukeSBT);
                                    raiinInf.UketukeSbt = uketukeSBT;
                                    break;
                                case "K":
                                    raiinInf.UketukeTime = text;
                                    break;
                                case "L":
                                    int.TryParse(text, out int uketukeId);
                                    raiinInf.UketukeId = uketukeId;
                                    break;
                                case "M":
                                    int.TryParse(text, out int uketukeNo);
                                    raiinInf.UketukeNo = uketukeNo;
                                    break;
                                case "N":
                                    raiinInf.SinStartTime = text;
                                    break;
                                case "O":
                                    raiinInf.SinEndTime = text;
                                    break;
                                case "P":
                                    raiinInf.KaikeiTime = text;
                                    break;
                                case "Q":
                                    int.TryParse(text, out int kaikeId);
                                    raiinInf.KaikeiId = kaikeId;
                                    break;
                                case "R":
                                    int.TryParse(text, out int kaId);
                                    raiinInf.KaId = kaId;
                                    break;
                                case "S":
                                    int.TryParse(text, out int tantoId);
                                    raiinInf.TantoId = tantoId;
                                    break;
                                case "T":
                                    int.TryParse(text, out int hokenPId);
                                    raiinInf.HokenPid = hokenPId;
                                    break;
                                case "U":
                                    int.TryParse(text, out int syosaisinKbn);
                                    raiinInf.SyosaisinKbn = syosaisinKbn;
                                    break;
                                case "V":
                                    int.TryParse(text, out int jikanKbn);
                                    raiinInf.SyosaisinKbn = jikanKbn;
                                    break;
                                case "W":
                                    int.TryParse(text, out int isDeleted);
                                    raiinInf.IsDeleted = isDeleted;
                                    break;
                                default:
                                    break;
                            }
                        }
                        raiinInfs.Add(raiinInf);
                        count++;
                    }
                }
            }

            return raiinInfs;
        }

        public static List<OdrInf> ReadOdrInf(int randomKey)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedOrderDataSample.xlsx");
            var odrInfs = new List<OdrInf>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "ODR_INF").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var odrInf = new OdrInf();
                        odrInf.CreateId = 1;
                        odrInf.CreateDate = DateTime.UtcNow;
                        odrInf.UpdateId = 1;
                        odrInf.UpdateDate = DateTime.UtcNow;
                        foreach (var c in r.Elements<Cell>())
                        {
                            text = c.CellValue?.Text ?? string.Empty;
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
                            }
                            var columnName = GetColumnName(c.CellReference?.ToString() ?? string.Empty);
                            switch (columnName)
                            {
                                case "A":
                                    int.TryParse(text, out int hpId);
                                    odrInf.HpId = hpId;
                                    break;
                                case "B":
                                    odrInf.RaiinNo = long.MaxValue - randomKey;
                                    break;
                                case "C":
                                    int.TryParse(text, out int rpNo);
                                    odrInf.RpNo = rpNo;
                                    break;
                                case "D":
                                    int.TryParse(text, out int rpEdaNo);
                                    odrInf.RpEdaNo = rpEdaNo;
                                    break;
                                case "E":
                                    var ptId = long.Parse(text, System.Globalization.CultureInfo.InvariantCulture);
                                    odrInf.PtId = ptId;
                                    break;
                                case "F":
                                    int.TryParse(text, out int sinDate);
                                    odrInf.SinDate = sinDate;
                                    break;
                                case "G":
                                    int.TryParse(text, out int hokenPId);
                                    odrInf.HokenPid = hokenPId;
                                    break;
                                case "H":
                                    int.TryParse(text, out int odrKouiKbn);
                                    odrInf.OdrKouiKbn = odrKouiKbn;
                                    break;
                                default:
                                    break;
                            }
                        }
                        odrInfs.Add(odrInf);
                    }
                }
            }

            return odrInfs;
        }

        public static List<OdrInfDetail> ReadOdrInfDetail(int randomKey)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedOrderDataSample.xlsx");
            var odrDetails = new List<OdrInfDetail>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "ODR_INF_DETAIL").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var odrInfDetail = new OdrInfDetail();
                        foreach (var c in r.Elements<Cell>())
                        {
                            text = c.CellValue?.Text ?? string.Empty;
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
                            }
                            var columnName = GetColumnName(c.CellReference?.ToString() ?? string.Empty);
                            switch (columnName)
                            {
                                case "A":
                                    int.TryParse(text, out int hpId);
                                    odrInfDetail.HpId = hpId;
                                    break;
                                case "B":
                                    odrInfDetail.RaiinNo = long.MaxValue - randomKey;
                                    break;
                                case "C":
                                    int.TryParse(text, out int rpNo);
                                    odrInfDetail.RpNo = rpNo;
                                    break;
                                case "D":
                                    int.TryParse(text, out int rpEdaNo);
                                    odrInfDetail.RpEdaNo = rpEdaNo;
                                    break;
                                case "E":
                                    int.TryParse(text, out int rownNo);
                                    odrInfDetail.RowNo = rownNo;
                                    break;
                                case "F":
                                    var ptId = long.Parse(text, System.Globalization.CultureInfo.InvariantCulture);
                                    odrInfDetail.PtId = ptId;
                                    break;
                                case "G":
                                    int.TryParse(text, out int sinDate);
                                    odrInfDetail.SinDate = sinDate;
                                    break;
                                case "H":
                                    int.TryParse(text, out int sinKouiKbn);
                                    odrInfDetail.SinKouiKbn = sinKouiKbn;
                                    break;
                                case "I":
                                    odrInfDetail.ItemCd = text;
                                    break;
                                case "J":
                                    odrInfDetail.ItemName = text;
                                    break;
                                default:
                                    break;
                            }
                        }
                        odrDetails.Add(odrInfDetail);
                    }
                }
            }

            return odrDetails;
        }

        public static List<PtSanteiConf> ReadPtSanteiConf(int randomKey)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedOrderDataSample.xlsx");
            var ptSanteiConfs = new List<PtSanteiConf>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PT_SANTEI_CONF").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptSanteiConf = new PtSanteiConf();
                        ptSanteiConf.CreateId = 1;
                        ptSanteiConf.CreateDate = DateTime.UtcNow;
                        ptSanteiConf.UpdateId = 1;
                        ptSanteiConf.UpdateDate = DateTime.UtcNow;

                        foreach (var c in r.Elements<Cell>())
                        {
                            text = c.CellValue?.Text ?? string.Empty;
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
                            }
                            var columnName = GetColumnName(c.CellReference?.ToString() ?? string.Empty);
                            switch (columnName)
                            {
                                case "A":
                                    int.TryParse(text, out int hpId);
                                    ptSanteiConf.HpId = hpId;
                                    break;
                                case "B":
                                    ptSanteiConf.PtId = long.MaxValue - randomKey;
                                    break;
                                case "C":
                                    int.TryParse(text, out int kbnNo);
                                    ptSanteiConf.KbnNo = kbnNo;
                                    break;
                                case "D":
                                    int.TryParse(text, out int edaNo);
                                    ptSanteiConf.EdaNo = edaNo;
                                    break;
                                case "H":
                                    int.TryParse(text, out int startDate);
                                    ptSanteiConf.StartDate = startDate;
                                    break;
                                case "I":
                                    int.TryParse(text, out int endDate);
                                    ptSanteiConf.EndDate = endDate;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptSanteiConfs.Add(ptSanteiConf);
                    }
                }
            }

            return ptSanteiConfs;
        }

        public static List<PtSanteiConf> ReadPtSanteiConfToNoCheckSantei(int randomKey)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedOrderDataSample.xlsx");
            var ptSanteiConfs = new List<PtSanteiConf>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PT_SANTEI_CONF").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptSanteiConf = new PtSanteiConf();
                        ptSanteiConf.CreateId = 1;
                        ptSanteiConf.CreateDate = DateTime.UtcNow;
                        ptSanteiConf.UpdateId = 1;
                        ptSanteiConf.UpdateDate = DateTime.UtcNow;

                        foreach (var c in r.Elements<Cell>())
                        {
                            text = c.CellValue?.Text ?? string.Empty;
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
                            }
                            var columnName = GetColumnName(c.CellReference?.ToString() ?? string.Empty);
                            switch (columnName)
                            {
                                case "A":
                                    int.TryParse(text, out int hpId);
                                    ptSanteiConf.HpId = hpId;
                                    break;
                                case "B":
                                    ptSanteiConf.PtId = long.MaxValue - randomKey;
                                    break;
                                case "C":
                                    int.TryParse(text, out int kbnNo);
                                    ptSanteiConf.KbnNo = 3;
                                    break;
                                case "D":
                                    int.TryParse(text, out int edaNo);
                                    ptSanteiConf.EdaNo = 1;
                                    break;
                                case "H":
                                    int.TryParse(text, out int startDate);
                                    ptSanteiConf.StartDate = startDate;
                                    break;
                                case "I":
                                    int.TryParse(text, out int endDate);
                                    ptSanteiConf.EndDate = endDate;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptSanteiConfs.Add(ptSanteiConf);
                    }
                }
            }

            return ptSanteiConfs;
        }

        public static List<UserMst> ReadUserMst(int randomKey)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedOrderDataSample.xlsx");
            var ptInfs = new List<UserMst>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "USER_MST").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptInf = new UserMst();
                        ptInf.CreateId = 1;
                        ptInf.CreateDate = DateTime.UtcNow;
                        ptInf.UpdateId = 1;
                        ptInf.UpdateDate = DateTime.UtcNow;
                        foreach (var c in r.Elements<Cell>())
                        {
                            text = c.CellValue?.Text ?? string.Empty;
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
                            }
                            var columnName = GetColumnName(c.CellReference?.ToString() ?? string.Empty);
                            switch (columnName)
                            {
                                case "A":
                                    int.TryParse(text, out int hpId);
                                    ptInf.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int id);
                                    ptInf.Id = 0;
                                    break;
                                case "C":
                                    int.TryParse(text, out int userId);
                                    ptInf.UserId = userId;
                                    break;
                                case "D":
                                    int.TryParse(text, out int jobCd);
                                    ptInf.JobCd = jobCd;
                                    break;
                                case "E":
                                    int.TryParse(text, out int managerKbn);
                                    ptInf.ManagerKbn = managerKbn;
                                    break;
                                case "F":
                                    int.TryParse(text, out int kaId);
                                    ptInf.KaId = kaId;
                                    break;
                                case "G":
                                    ptInf.KanaName = text;
                                    break;
                                case "H":
                                    ptInf.Name = text;
                                    break;
                                case "I":
                                    ptInf.Sname = text;
                                    break;
                                case "J":
                                    ptInf.LoginId = text;
                                    break;
                                case "K":
                                    //ptInf.LoginPass = text;
                                    break;
                                case "M":
                                    int.TryParse(text, out int startDate);
                                    ptInf.StartDate = startDate;
                                    break;
                                case "N":
                                    int.TryParse(text, out int endDate);
                                    ptInf.EndDate = endDate;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptInfs.Add(ptInf);
                    }
                }
            }

            return ptInfs;
        }


        public static List<ByomeiMst> ReadByomeiMst(string byomeiCd)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedOrderDataSample.xlsx");
            var byomeiMsts = new List<ByomeiMst>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "BYOMEI_MST").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var byomeiMst = new ByomeiMst();
                        byomeiMst.CreateId = 1;
                        byomeiMst.CreateDate = DateTime.UtcNow;
                        byomeiMst.UpdateId = 1;
                        byomeiMst.UpdateDate = DateTime.UtcNow;

                        foreach (var c in r.Elements<Cell>())
                        {
                            text = c.CellValue?.Text ?? string.Empty;
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
                            }
                            var columnName = GetColumnName(c.CellReference?.ToString() ?? string.Empty);
                            switch (columnName)
                            {
                                case "A":
                                    int.TryParse(text, out int hpId);
                                    byomeiMst.HpId = hpId;
                                    break;
                                case "B":
                                    byomeiMst.ByomeiCd = byomeiCd;
                                    break;
                                case "C":
                                    byomeiMst.Byomei = text;
                                    break;
                                case "D":
                                    byomeiMst.Sbyomei = text;
                                    break;
                                case "H":
                                    byomeiMst.KanaName1 = text;
                                    break;
                                case "M":
                                    int.TryParse(text, out int sikkanCd);
                                    byomeiMst.SikkanCd = sikkanCd;
                                    break;
                                case "N":
                                    int.TryParse(text, out int tandokuKinsi);
                                    byomeiMst.TandokuKinsi = tandokuKinsi;
                                    break;
                                case "O":
                                    int.TryParse(text, out int hokenGai);
                                    byomeiMst.HokenGai = hokenGai;
                                    break;
                                case "P":
                                    byomeiMst.ByomeiKanri = text;
                                    break;
                                case "Q":
                                    byomeiMst.SaitakuKbn = text;
                                    break;
                                case "R":
                                    byomeiMst.KoukanCd = text;
                                    break;
                                case "S":
                                    int.TryParse(text, out int syusaiDate);
                                    byomeiMst.SyusaiDate = syusaiDate;
                                    break;
                                case "T":
                                    int.TryParse(text, out int updDate);
                                    byomeiMst.UpdDate = updDate;
                                    break;
                                case "U":
                                    int.TryParse(text, out int delDate);
                                    byomeiMst.DelDate = delDate;
                                    break;
                                case "V":
                                    int.TryParse(text, out int nanbyoCd);
                                    byomeiMst.NanbyoCd = nanbyoCd;
                                    break;
                                case "W":
                                    byomeiMst.Icd101 = text;
                                    break;
                                case "X":
                                    byomeiMst.Icd102 = text;
                                    break;
                                case "Y":
                                    byomeiMst.Icd1012013 = text;
                                    break;
                                case "Z":
                                    byomeiMst.Icd1022013 = text;
                                    break;
                                case "AA":
                                    int.TryParse(text, out int isAdopted);
                                    byomeiMst.IsAdopted = isAdopted;
                                    break;
                                case "AB":
                                    byomeiMst.SyusyokuKbn = text;
                                    break;
                                default:
                                    break;
                            }
                        }
                        byomeiMsts.Add(byomeiMst);
                    }
                }
            }

            return byomeiMsts;
        }


        public static List<TekiouByomeiMst> ReadTekiouByomeiMst(string byomeiCd, string itemCd)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedOrderDataSample.xlsx");
            var tekiouByomeiMsts = new List<TekiouByomeiMst>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "TEKIOU_BYOMEI_MST").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var tekiouByomeiMst = new TekiouByomeiMst();
                        tekiouByomeiMst.CreateId = 1;
                        tekiouByomeiMst.CreateDate = DateTime.UtcNow;
                        tekiouByomeiMst.UpdateId = 1;
                        tekiouByomeiMst.UpdateDate = DateTime.UtcNow;

                        foreach (var c in r.Elements<Cell>())
                        {
                            text = c.CellValue?.Text ?? string.Empty;
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
                            }
                            var columnName = GetColumnName(c.CellReference?.ToString() ?? string.Empty);
                            switch (columnName)
                            {
                                case "A":
                                    int.TryParse(text, out int hpId);
                                    tekiouByomeiMst.HpId = hpId;
                                    break;
                                case "B":
                                    tekiouByomeiMst.ItemCd = itemCd;
                                    break;
                                case "C":
                                    tekiouByomeiMst.ByomeiCd = byomeiCd;
                                    break;
                                case "D":
                                    int.TryParse(text, out int isInvalid);
                                    tekiouByomeiMst.IsInvalid = isInvalid;
                                    break;
                                case "E":
                                    int.TryParse(text, out int isInvalidTokusyo);
                                    tekiouByomeiMst.IsInvalidTokusyo = isInvalidTokusyo;
                                    break;
                                case "F":
                                    int.TryParse(text, out int editKbn);
                                    tekiouByomeiMst.EditKbn = editKbn;
                                    break;
                                case "G":
                                    int.TryParse(text, out int systemData);
                                    tekiouByomeiMst.SystemData = systemData;
                                    break;
                                case "N":
                                    int.TryParse(text, out int startYM);
                                    tekiouByomeiMst.StartYM = startYM;
                                    break;
                                case "O":
                                    int.TryParse(text, out int endYM);
                                    tekiouByomeiMst.EndYM = endYM;
                                    break;
                                default:
                                    break;
                            }
                        }
                        tekiouByomeiMsts.Add(tekiouByomeiMst);
                    }
                }
            }

            return tekiouByomeiMsts;
        }

        public static List<TenMst> ReadTenMst()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            int count = 1;
            string fileName = Path.Combine(rootPath, "SampleData", "CheckedOrderDataSample.xlsx");
            var tenMsts = new List<TenMst>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {

                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "TEN_MST").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var tenMst = new TenMst();
                        tenMst.CreateId = 1;
                        tenMst.CreateDate = DateTime.UtcNow;
                        tenMst.UpdateId = 1;
                        tenMst.UpdateDate = DateTime.UtcNow;
                        foreach (var c in r.Elements<Cell>())
                        {
                            text = c.CellValue?.Text ?? string.Empty;
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
                            }
                            var columnName = GetColumnName(c.CellReference?.ToString() ?? string.Empty);

                            switch (columnName)
                            {
                                case "A":
                                    int.TryParse(text, out int hpId);
                                    tenMst.HpId = hpId;
                                    break;
                                case "B":
                                    tenMst.ItemCd = text;
                                    break;
                                case "C":
                                    int.TryParse(text, out int startDate);
                                    tenMst.StartDate = startDate;
                                    break;
                                case "D":
                                    int.TryParse(text, out int endDate);
                                    tenMst.EndDate = endDate;
                                    break;
                                case "DI":
                                    tenMst.CdKbn = text;
                                    break;
                                case "DL":
                                    int.TryParse(text, out int cdKbnoo);
                                    tenMst.CdKbnno = cdKbnoo;
                                    break;
                                case "DV":
                                    tenMst.Kokuji2 = text;
                                    break;
                                case "DM":
                                    int.TryParse(text, out int cdEdaNo);
                                    tenMst.CdEdano = cdEdaNo;
                                    break;
                                default:
                                    break;
                            }
                        }
                        tenMsts.Add(tenMst);
                        count++;
                    }
                }
            }

            return tenMsts;
        }

        private static Worksheet GetworksheetBySheetName(SpreadsheetDocument spreadsheetDocument, string sheetName)
        {
            var workbookPart = spreadsheetDocument.WorkbookPart;
            StringValue relationshipId = workbookPart?.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name != null && s.Name.Equals(sheetName))?.Id ?? string.Empty;

            var worksheet = (workbookPart != null && !string.IsNullOrEmpty(relationshipId.ToString())) ? ((WorksheetPart)workbookPart.GetPartById(relationshipId?.Value ?? string.Empty)).Worksheet : new();

            return worksheet;
        }

        private static string GetColumnName(string text)
        {
            var check = int.TryParse(text.Skip(1).FirstOrDefault().ToString(), out int number);
            if (check)
            {
                return text.FirstOrDefault().ToString();
            }
            else
            {
                return text.Substring(0, 2);
            }
        }
    }
}
