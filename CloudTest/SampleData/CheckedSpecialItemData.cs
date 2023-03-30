using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity.Tenant;

namespace CloudUnitTest.SampleData
{
    public static class CheckedSpecialItemData
    {
        //Create COM Objects. Create a COM object for everything that is referenced
        public static List<RaiinInf> ReadRainInf()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedSpecialDataSample.xlsx");
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
                                    long.TryParse(text, out long raiinNo);
                                    raiinInf.RaiinNo = raiinNo;
                                    break;
                                case "C":
                                    long.TryParse(text, out long ptId);
                                    raiinInf.PtId = ptId;
                                    break;
                                case "D":
                                    int.TryParse(text, out int sinDate);
                                    raiinInf.SinDate = sinDate;
                                    break;
                                case "E":
                                    long.TryParse(text, out long oyaRaiinNo);
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
                    }
                }
            }

            return raiinInfs;
        }

        //Create COM Objects. Create a COM object for everything that is referenced
        public static List<DensiSanteiKaisu> ReadDensiSanteiKaisu()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedSpecialDataSample.xlsx");
            var densiSanteiKaisus = new List<DensiSanteiKaisu>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "DENSI_SANTEI_KAISU").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var densiSanteiKaisu = new DensiSanteiKaisu();
                        densiSanteiKaisu.CreateId = 1;
                        densiSanteiKaisu.CreateDate = DateTime.UtcNow;
                        densiSanteiKaisu.UpdateId = 1;
                        densiSanteiKaisu.UpdateDate = DateTime.UtcNow;
                        var numberCell = 1;
                        foreach (var c in r.Elements<Cell>())
                        {
                            if (numberCell > 15)
                            {
                                break;
                            }
                            text = c.CellValue?.Text ?? string.Empty;
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
                            }

                            switch (numberCell)
                            {
                                case 1:
                                    int.TryParse(text, out int hpId);
                                    densiSanteiKaisu.HpId = hpId;
                                    break;
                                case 2:
                                    densiSanteiKaisu.ItemCd = text;
                                    break;
                                case 3:
                                    int.TryParse(text, out int unitCd);
                                    densiSanteiKaisu.UnitCd = unitCd;
                                    break;
                                case 4:
                                    int.TryParse(text, out int startDate);
                                    densiSanteiKaisu.StartDate = startDate;
                                    break;
                                case 5:
                                    int.TryParse(text, out int seqNo);
                                    densiSanteiKaisu.SeqNo = seqNo;
                                    break;
                                case 6:
                                    int.TryParse(text, out int userSetting);
                                    densiSanteiKaisu.UserSetting = userSetting;
                                    break;
                                case 7:
                                    int.TryParse(text, out int maxCount);
                                    densiSanteiKaisu.MaxCount = maxCount;
                                    break;
                                case 8:
                                    int.TryParse(text, out int spJyoken);
                                    densiSanteiKaisu.SpJyoken = spJyoken;
                                    break;
                                case 9:
                                    int.TryParse(text, out int endDate);
                                    densiSanteiKaisu.EndDate = endDate;
                                    break;
                                case 10:
                                    int.TryParse(text, out int targetKbn);
                                    densiSanteiKaisu.TargetKbn = targetKbn;
                                    break;
                                case 11:
                                    int.TryParse(text, out int termCount);
                                    densiSanteiKaisu.TermCount = termCount;
                                    break;
                                case 12:
                                    int.TryParse(text, out int termSbt);
                                    densiSanteiKaisu.TermSbt = termSbt;
                                    break;
                                case 13:
                                    int.TryParse(text, out int isValid);
                                    densiSanteiKaisu.IsInvalid = isValid;
                                    break;
                                case 14:
                                    int.TryParse(text, out int id);
                                    densiSanteiKaisu.Id = id;
                                    break;
                                case 15:
                                    int.TryParse(text, out int itemGrpCd);
                                    densiSanteiKaisu.ItemGrpCd = itemGrpCd;
                                    break;
                                default:
                                    break;
                            }
                            numberCell++;
                        }
                        densiSanteiKaisus.Add(densiSanteiKaisu);
                    }
                }
            }

            return densiSanteiKaisus;
        }

        //Create COM Objects. Create a COM object for everything that is referenced
        public static List<TenMst> ReadTenMst()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedSpecialDataSample.xlsx");
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
                                case "E":
                                    tenMst.MasterSbt = text;
                                    break;
                                case "F":
                                    int.TryParse(text, out int sinkouiKbn);
                                    tenMst.SinKouiKbn = sinkouiKbn;
                                    break;
                                case "G":
                                    tenMst.Name = text;
                                    break;
                                case "AO":
                                    int.TryParse(text, out int maxCount);
                                    tenMst.MaxCount = maxCount;
                                    break;
                                case "H":
                                    tenMst.KanaName1 = text;
                                    break;
                                case "I":
                                    tenMst.KanaName2 = text;
                                    break;
                                case "Q":
                                    int.TryParse(text, out int tenId);
                                    tenMst.TenId = tenId;
                                    break;
                                case "R":
                                    double.TryParse(text, out double ten);
                                    tenMst.Ten = ten;
                                    break;
                                default:
                                    break;
                            }
                        }
                        tenMsts.Add(tenMst);
                    }
                }
            }

            return tenMsts;
        }

        public static List<SinRpInf> ReadSinRpInf()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedSpecialDataSample.xlsx");
            var sinRpInfs = new List<SinRpInf>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "SIN_RP_INF").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var sinRpInf = new SinRpInf();
                        sinRpInf.CreateId = 1;
                        sinRpInf.CreateDate = DateTime.UtcNow;
                        sinRpInf.UpdateId = 1;
                        sinRpInf.UpdateDate = DateTime.UtcNow;
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
                                    sinRpInf.HpId = hpId;
                                    break;
                                case "B":
                                    long.TryParse(text, out long ptId);
                                    sinRpInf.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int sinYm);
                                    sinRpInf.SinYm = sinYm;
                                    break;
                                case "D":
                                    int.TryParse(text, out int rpNo);
                                    sinRpInf.RpNo = rpNo;
                                    break;
                                case "E":
                                    int.TryParse(text, out int sinKouiKbn);
                                    sinRpInf.SinKouiKbn = sinKouiKbn;
                                    break;
                                case "F":
                                    int.TryParse(text, out int sinId);
                                    sinRpInf.SinId = sinId;
                                    break;
                                case "H":
                                    int.TryParse(text, out int santeiKbn);
                                    sinRpInf.SanteiKbn = santeiKbn;
                                    break;
                                case "P":
                                    int.TryParse(text, out int hokenKbn);
                                    sinRpInf.HokenKbn = hokenKbn;
                                    break;
                                default:
                                    break;
                            }
                        }
                        sinRpInfs.Add(sinRpInf);
                    }
                }
            }

            return sinRpInfs;
        }

        public static List<SinKouiDetail> ReadSinKouiDetail()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedSpecialDataSample.xlsx");
            var sinKouiDetails = new List<SinKouiDetail>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "SIN_KOUI_DETAIL").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var sinRpInf = new SinKouiDetail();
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
                                    sinRpInf.HpId = hpId;
                                    break;
                                case "B":
                                    long.TryParse(text, out long ptId);
                                    sinRpInf.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int sinYm);
                                    sinRpInf.SinYm = sinYm;
                                    break;
                                case "D":
                                    int.TryParse(text, out int rpNo);
                                    sinRpInf.RpNo = rpNo;
                                    break;
                                case "E":
                                    int.TryParse(text, out int seqNo);
                                    sinRpInf.SeqNo = seqNo;
                                    break;
                                case "H":
                                    sinRpInf.ItemCd = text;
                                    break;
                                case "I":
                                    sinRpInf.ItemName = text;
                                    break;
                                case "J":
                                    var check = double.TryParse(text, out double suryo);
                                    suryo = check ? double.Parse(text, System.Globalization.CultureInfo.InvariantCulture) : 0;
                                    sinRpInf.Suryo = suryo;
                                    break;
                                case "O":
                                    double.TryParse(text, out double ten);
                                    sinRpInf.Ten = ten;
                                    break;
                                default:
                                    break;
                            }
                        }
                        sinKouiDetails.Add(sinRpInf);
                    }
                }
            }

            return sinKouiDetails;
        }

        public static List<SinKouiCount> ReadSinKouiCount()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedSpecialDataSample.xlsx");
            var sinKouiCounts = new List<SinKouiCount>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "SIN_KOUI_COUNT").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var sinKouiCount = new SinKouiCount();
                        sinKouiCount.CreateId = 1;
                        sinKouiCount.CreateDate = DateTime.UtcNow;
                        sinKouiCount.UpdateId = 1;
                        sinKouiCount.UpdateDate = DateTime.UtcNow;
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
                                    sinKouiCount.HpId = hpId;
                                    break;
                                case "B":
                                    long.TryParse(text, out long ptId);
                                    sinKouiCount.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int sinYm);
                                    sinKouiCount.SinYm = sinYm;
                                    break;
                                case "D":
                                    int.TryParse(text, out int sinDay);
                                    sinKouiCount.SinDay = sinDay;
                                    break;
                                case "E":
                                    long.TryParse(text, out long raiinNo);
                                    sinKouiCount.RaiinNo = raiinNo;
                                    break;
                                case "F":
                                    int.TryParse(text, out int rpNo);
                                    sinKouiCount.RpNo = rpNo;
                                    break;
                                case "G":
                                    int.TryParse(text, out int seqNo);
                                    sinKouiCount.SeqNo = seqNo;
                                    break;
                                case "H":
                                    int.TryParse(text, out int count);
                                    sinKouiCount.Count = count;
                                    break;
                                case "O":
                                    int.TryParse(text, out int sinDate);
                                    sinKouiCount.SinDate = sinDate;
                                    break;
                                default:
                                    break;
                            }
                        }
                        sinKouiCounts.Add(sinKouiCount);
                    }
                }
            }

            return sinKouiCounts;
        }

        public static List<HokenMst> ReadHokenMst()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedSpecialDataSample.xlsx");
            var hokenMsts = new List<HokenMst>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "HOKEN_MST").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var hokenMst = new HokenMst();
                        hokenMst.CreateId = 1;
                        hokenMst.CreateDate = DateTime.UtcNow;
                        hokenMst.UpdateId = 1;
                        hokenMst.UpdateDate = DateTime.UtcNow;
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
                                    hokenMst.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int prefNo);
                                    hokenMst.PrefNo = prefNo;
                                    break;
                                case "C":
                                    int.TryParse(text, out int hokenNo);
                                    hokenMst.HokenNo = hokenNo;
                                    break;
                                case "D":
                                    int.TryParse(text, out int hokenEdaNo);
                                    hokenMst.HokenEdaNo = hokenEdaNo;
                                    break;
                                case "E":
                                    int.TryParse(text, out int startDate);
                                    hokenMst.StartDate = startDate;
                                    break;
                                case "F":
                                    int.TryParse(text, out int endDate);
                                    hokenMst.EndDate = endDate;
                                    break;
                                case "G":
                                    int.TryParse(text, out int sortNo);
                                    hokenMst.SortNo = sortNo;
                                    break;
                                case "H":
                                    int.TryParse(text, out int hokenSbtKbn);
                                    hokenMst.HokenSbtKbn = hokenSbtKbn;
                                    break;
                                case "I":
                                    hokenMst.Houbetu = text;
                                    break;
                                case "J":
                                    hokenMst.HokenName = text;
                                    break;
                                case "K":
                                    hokenMst.HokenSname = text;
                                    break;
                                case "L":
                                    hokenMst.HokenNameCd = text;
                                    break;
                                default:
                                    break;
                            }
                        }
                        hokenMsts.Add(hokenMst);
                    }
                }
            }

            return hokenMsts;
        }

        public static List<PtHokenInf> ReadPtHokenInf()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedSpecialDataSample.xlsx");
            var hokenMsts = new List<PtHokenInf>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PT_HOKEN_INF").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var hokenMst = new PtHokenInf();
                        hokenMst.CreateId = 1;
                        hokenMst.CreateDate = DateTime.UtcNow;
                        hokenMst.UpdateId = 1;
                        hokenMst.UpdateDate = DateTime.UtcNow;
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
                                    hokenMst.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int ptId);
                                    hokenMst.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int hokenId);
                                    hokenMst.HokenId = hokenId;
                                    break;
                                case "D":
                                    long.TryParse(text, out long seqNo);
                                    hokenMst.SeqNo = seqNo;
                                    break;
                                case "E":
                                    int.TryParse(text, out int hokenNo);
                                    hokenMst.HokenNo = hokenNo;
                                    break;
                                case "F":
                                    int.TryParse(text, out int hokenEdaNo);
                                    hokenMst.HokenEdaNo = hokenEdaNo;
                                    break;
                                case "G":
                                    hokenMst.HokensyaNo = text;
                                    break;
                                case "H":
                                    hokenMst.Kigo = text;
                                    break;
                                case "I":
                                    hokenMst.Bango = text;
                                    break;
                                case "J":
                                    int.TryParse(text, out int honkeKbn);
                                    hokenMst.HonkeKbn = honkeKbn;
                                    break;
                                case "K":
                                    int.TryParse(text, out int hokenKbn);
                                    hokenMst.HokenKbn = hokenKbn;
                                    break;
                                case "L":
                                    hokenMst.Houbetu = text;
                                    break;
                                case "T":
                                    int.TryParse(text, out int startDate);
                                    hokenMst.StartDate = startDate;
                                    break;
                                case "U":
                                    int.TryParse(text, out int endDate);
                                    hokenMst.EndDate = endDate;
                                    break;
                                default:
                                    break;
                            }
                        }
                        hokenMsts.Add(hokenMst);
                    }
                }
            }

            return hokenMsts;
        }

        public static List<PtKohi> ReadPtKoHi()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedSpecialDataSample.xlsx");
            var kohis = new List<PtKohi>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PT_KOHI").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var kohi = new PtKohi();
                        kohi.CreateId = 1;
                        kohi.CreateDate = DateTime.UtcNow;
                        kohi.UpdateId = 1;
                        kohi.UpdateDate = DateTime.UtcNow;
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
                                    kohi.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int ptId);
                                    kohi.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int hokenId);
                                    kohi.HokenId = hokenId;
                                    break;
                                case "D":
                                    long.TryParse(text, out long seqNo);
                                    kohi.SeqNo = seqNo;
                                    break;
                                case "E":
                                    int.TryParse(text, out int prefNo);
                                    kohi.PrefNo = prefNo;
                                    break;
                                case "F":
                                    int.TryParse(text, out int hokenNo);
                                    kohi.HokenNo = hokenNo;
                                    break;
                                case "G":
                                    int.TryParse(text, out int hokenEdaNo);
                                    kohi.HokenEdaNo = hokenEdaNo;
                                    break;
                                case "H":
                                    kohi.FutansyaNo = text;
                                    break;
                                case "I":
                                    kohi.JyukyusyaNo = text;
                                    break;
                                case "M":
                                    int.TryParse(text, out int startDate);
                                    kohi.StartDate = startDate;
                                    break;
                                case "N":
                                    int.TryParse(text, out int endDate);
                                    kohi.EndDate = endDate;
                                    break;
                                default:
                                    break;
                            }
                        }
                        kohis.Add(kohi);
                    }
                }
            }

            return kohis;
        }

        public static List<PtHokenCheck> ReadPtHokenCheck()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedSpecialDataSample.xlsx");
            var ptHokenChecks = new List<PtHokenCheck>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PT_HOKEN_CHECK").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptHokenCheck = new PtHokenCheck();
                        ptHokenCheck.CreateId = 1;
                        ptHokenCheck.CreateDate = DateTime.UtcNow;
                        ptHokenCheck.UpdateId = 1;
                        ptHokenCheck.UpdateDate = DateTime.UtcNow;
                        ptHokenCheck.CheckDate = DateTime.UtcNow;
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
                                    ptHokenCheck.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int ptId);
                                    ptHokenCheck.PtID = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int hokenGrP);
                                    ptHokenCheck.HokenGrp = hokenGrP;
                                    break;
                                case "D":
                                    int.TryParse(text, out int hokenId);
                                    ptHokenCheck.HokenId = hokenId;
                                    break;
                                case "E":
                                    int.TryParse(text, out int seqNo);
                                    ptHokenCheck.SeqNo = seqNo;
                                    break;
                                //case "F":
                                //    DateTime.TryParse(text, out DateTime checkDate);
                                //    ptHokenCheck.CheckDate = checkDate;
                                //    break;
                                case "G":
                                    int.TryParse(text, out int checkId);
                                    ptHokenCheck.CheckId = checkId;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptHokenChecks.Add(ptHokenCheck);
                    }
                }
            }

            return ptHokenChecks;
        }

        public static List<PtInf> ReadPtInf()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedSpecialDataSample.xlsx");
            var ptInfs = new List<PtInf>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PT_INF").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptInf = new PtInf();
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
                                    int.TryParse(text, out int ptId);
                                    ptInf.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int seqNo);
                                    ptInf.SeqNo = seqNo;
                                    break;
                                case "D":
                                    int.TryParse(text, out int ptNum);
                                    ptInf.PtNum = ptNum;
                                    break;
                                case "E":
                                    ptInf.KanaName = text;
                                    break;
                                case "F":
                                    ptInf.Name = text;
                                    break;
                                case "G":
                                    int.TryParse(text, out int sex);
                                    ptInf.Sex = sex;
                                    break;
                                case "H":
                                    int.TryParse(text, out int birthDay);
                                    ptInf.Birthday = birthDay;
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

        public static List<UserMst> ReadUserMst()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedSpecialDataSample.xlsx");
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
                                    ptInf.Id = id;
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
                                    ptInf.LoginPass = text;
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

        public static List<HokensyaMst> ReadHokenSyaMst()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedSpecialDataSample.xlsx");
            var hokensyaMsts = new List<HokensyaMst>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "HOKENSYA_MST").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var hokensyaMst = new HokensyaMst();
                        hokensyaMst.CreateId = 1;
                        hokensyaMst.CreateDate = DateTime.UtcNow;
                        hokensyaMst.UpdateId = 1;
                        hokensyaMst.UpdateDate = DateTime.UtcNow;
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
                                    hokensyaMst.HpId = hpId;
                                    break;
                                case "B":
                                    hokensyaMst.HokensyaNo = text;
                                    break;
                                case "C":
                                    hokensyaMst.Name = text;
                                    break;
                                case "D":
                                    hokensyaMst.KanaName = text;
                                    break;
                                case "E":
                                    hokensyaMst.HoubetuKbn = text;
                                    break;
                                case "F":
                                    int.TryParse(text, out int prefNo);
                                    hokensyaMst.PrefNo = prefNo;
                                    break;
                                case "M":
                                    hokensyaMst.PostCode = text;
                                    break;
                                case "N":
                                    hokensyaMst.Address1 = text;
                                    break;
                                case "O":
                                    hokensyaMst.Address2 = text;
                                    break;
                                case "P":
                                    hokensyaMst.Tel1 = text;
                                    break;
                                default:
                                    break;
                            }
                        }
                        hokensyaMsts.Add(hokensyaMst);
                    }
                }
            }

            return hokensyaMsts;
        }

        public static List<RoudouMst> ReadRoudouMst()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedSpecialDataSample.xlsx");
            var roudouMsts = new List<RoudouMst>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "ROUDOU_MST").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var roudou = new RoudouMst();
                        roudou.CreateDate = DateTime.UtcNow;
                        roudou.UpdateDate = DateTime.UtcNow;
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
                                    roudou.RoudouCd = text;
                                    break;
                                case "B":
                                    roudou.RoudouName = text;
                                    break;
                                default:
                                    break;
                            }
                        }
                        roudouMsts.Add(roudou);
                    }
                }
            }

            return roudouMsts;
        }

        public static List<PtRousaiTenki> ReadPtRouSaiTenKi()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedSpecialDataSample.xlsx");
            var ptRousaiTenkis = new List<PtRousaiTenki>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PT_ROUSAI_TENKI").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptRousaiTenki = new PtRousaiTenki();
                        ptRousaiTenki.CreateId = 1;
                        ptRousaiTenki.CreateDate = DateTime.UtcNow;
                        ptRousaiTenki.UpdateId = 1;
                        ptRousaiTenki.UpdateDate = DateTime.UtcNow;
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
                                    ptRousaiTenki.HpId = hpId;
                                    break;
                                case "B":
                                    long.TryParse(text, out long ptId);
                                    ptRousaiTenki.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int hokenId);
                                    ptRousaiTenki.HokenId = hokenId;
                                    break;
                                case "D":
                                    int.TryParse(text, out int seqNo);
                                    ptRousaiTenki.SeqNo = seqNo;
                                    break;
                                case "E":
                                    int.TryParse(text, out int endDate);
                                    ptRousaiTenki.EndDate = endDate;
                                    break;
                                case "F":
                                    int.TryParse(text, out int sinkei);
                                    ptRousaiTenki.Sinkei = sinkei;
                                    break;
                                case "G":
                                    int.TryParse(text, out int tenki);
                                    ptRousaiTenki.Tenki = tenki;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptRousaiTenkis.Add(ptRousaiTenki);
                    }
                }
            }

            return ptRousaiTenkis;
        }

        public static List<PtHokenPattern> ReadPtHokenPattern()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedSpecialDataSample.xlsx");
            var ptHokenPatterns = new List<PtHokenPattern>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PT_HOKEN_PATTERN").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptHokenPattern = new PtHokenPattern();
                        ptHokenPattern.CreateId = 1;
                        ptHokenPattern.CreateDate = DateTime.UtcNow;
                        ptHokenPattern.UpdateId = 1;
                        ptHokenPattern.UpdateDate = DateTime.UtcNow;
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
                                    ptHokenPattern.HpId = hpId;
                                    break;
                                case "B":
                                    long.TryParse(text, out long ptId);
                                    ptHokenPattern.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int hokenPId);
                                    ptHokenPattern.HokenPid = hokenPId;
                                    break;
                                case "D":
                                    int.TryParse(text, out int seqNo);
                                    ptHokenPattern.SeqNo = seqNo;
                                    break;
                                case "E":
                                    int.TryParse(text, out int hokenKbn);
                                    ptHokenPattern.HokenKbn = hokenKbn;
                                    break;
                                case "F":
                                    int.TryParse(text, out int hokensbtCd);
                                    ptHokenPattern.HokenSbtCd = hokensbtCd;
                                    break;
                                case "G":
                                    int.TryParse(text, out int hokenId);
                                    ptHokenPattern.HokenId = hokenId;
                                    break;
                                case "H":
                                    int.TryParse(text, out int kohi1Id);
                                    ptHokenPattern.Kohi1Id = kohi1Id;
                                    break;
                                case "I":
                                    int.TryParse(text, out int kohi2Id);
                                    ptHokenPattern.Kohi2Id = kohi2Id;
                                    break;
                                case "J":
                                    int.TryParse(text, out int kohi3Id);
                                    ptHokenPattern.Kohi3Id = kohi3Id;
                                    break;
                                case "K":
                                    int.TryParse(text, out int kohi4Id);
                                    ptHokenPattern.Kohi4Id = kohi4Id;
                                    break;
                                case "M":
                                    int.TryParse(text, out int startDate);
                                    ptHokenPattern.StartDate = startDate;
                                    break;
                                case "N":
                                    int.TryParse(text, out int endDate);
                                    ptHokenPattern.EndDate = endDate;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptHokenPatterns.Add(ptHokenPattern);
                    }
                }
            }

            return ptHokenPatterns;
        }

        private static Worksheet GetworksheetBySheetName(SpreadsheetDocument spreadsheetDocument, string sheetName)
        {

            var workbookPart = spreadsheetDocument.WorkbookPart;
            StringValue relationshipId = workbookPart?.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name != null && s.Name.Equals(sheetName))?.Id ?? string.Empty;

            var worksheet = workbookPart != null ? ((WorksheetPart)workbookPart.GetPartById(relationshipId.Value ?? string.Empty)).Worksheet : new();

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
