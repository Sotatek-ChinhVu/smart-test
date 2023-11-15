using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity.Tenant;

namespace CloudUnitTest.SampleData
{
    public static class CommonCheckerData
    {
        public static List<PtByomei> ReadPtByomei()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CommonCheckerTest.xlsx");
            var ptByomeis = new List<PtByomei>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {

                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PT_BYOMEI").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptByomei = new PtByomei();
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
                                    ptByomei.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int ptId);
                                    ptByomei.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int seqNo);
                                    ptByomei.SeqNo = seqNo;
                                    break;
                                case "D":
                                    ptByomei.ByomeiCd = text;
                                    break;
                                case "E":
                                    int.TryParse(text, out int sortNo);
                                    ptByomei.SortNo = sortNo;
                                    break;
                                case "F":
                                    ptByomei.SyusyokuCd1 = text;
                                    break;
                                case "G":
                                    ptByomei.SyusyokuCd2 = text;
                                    break;
                                case "H":
                                    ptByomei.SyusyokuCd3 = text;
                                    break;
                                case "I":
                                    ptByomei.SyusyokuCd4 = text;
                                    break;
                                case "J":
                                    ptByomei.SyusyokuCd5 = text;
                                    break;
                                case "K":
                                    ptByomei.SyusyokuCd6 = text;
                                    break;
                                case "L":
                                    ptByomei.SyusyokuCd7 = text;
                                    break;
                                case "M":
                                    ptByomei.SyusyokuCd8 = text;
                                    break;
                                case "N":
                                    ptByomei.SyusyokuCd8 = text;
                                    break;
                                case "O":
                                    ptByomei.SyusyokuCd10 = text;
                                    break;
                                case "P":
                                    ptByomei.SyusyokuCd11 = text;
                                    break;
                                case "Q":
                                    ptByomei.SyusyokuCd12 = text;
                                    break;
                                case "R":
                                    ptByomei.SyusyokuCd13 = text;
                                    break;
                                case "S":
                                    ptByomei.SyusyokuCd14 = text;
                                    break;
                                case "T":
                                    ptByomei.SyusyokuCd15 = text;
                                    break;
                                case "U":
                                    ptByomei.SyusyokuCd16 = text;
                                    break;
                                case "V":
                                    ptByomei.SyusyokuCd17 = text;
                                    break;
                                case "W":
                                    ptByomei.SyusyokuCd18 = text;
                                    break;
                                case "X":
                                    ptByomei.SyusyokuCd19 = text;
                                    break;
                                case "Y":
                                    ptByomei.SyusyokuCd20 = text;
                                    break;
                                case "Z":
                                    ptByomei.SyusyokuCd21 = text;
                                    break;
                                case "AA":
                                    ptByomei.Byomei = text;
                                    break;
                                case "AB":
                                    int.TryParse(text, out int startDate);
                                    ptByomei.StartDate = startDate;
                                    break;
                                case "AC":
                                    int.TryParse(text, out int tenkiKbn);
                                    ptByomei.TenkiKbn = tenkiKbn;
                                    break;
                                case "AD":
                                    int.TryParse(text, out int tenkiDate);
                                    ptByomei.TenkiDate = tenkiDate;
                                    break;
                                case "AE":
                                    int.TryParse(text, out int syubyoKbn);
                                    ptByomei.SyubyoKbn = syubyoKbn;
                                    break;
                                case "AF":
                                    int.TryParse(text, out int sikkanKbn);
                                    ptByomei.SikkanKbn = sikkanKbn;
                                    break;
                                case "AG":
                                    int.TryParse(text, out int nanByoCd);
                                    ptByomei.NanByoCd = nanByoCd;
                                    break;
                                case "AH":
                                    ptByomei.HosokuCmt = text;
                                    break;
                                case "AI":
                                    int.TryParse(text, out int hokenPid);
                                    ptByomei.HokenPid = hokenPid;
                                    break;
                                case "AJ":
                                    int.TryParse(text, out int togetuByomei);
                                    ptByomei.TogetuByomei = togetuByomei;
                                    break;
                                case "AK":
                                    int.TryParse(text, out int isNodspRece);
                                    ptByomei.IsNodspRece = isNodspRece;
                                    break;
                                case "AL":
                                    int.TryParse(text, out int isNodspKarte);
                                    ptByomei.IsNodspKarte = isNodspKarte;
                                    break;
                                case "AM":
                                    int.TryParse(text, out int isDeleted);
                                    ptByomei.IsDeleted = isDeleted;
                                    break;
                                case "AN":
                                    ptByomei.CreateDate = DateTime.UtcNow;
                                    break;
                                case "AO":
                                    int.TryParse(text, out int createId);
                                    ptByomei.CreateId = createId;
                                    break;
                                case "AP":
                                    ptByomei.CreateMachine = text;
                                    break;
                                case "AQ":
                                    ptByomei.UpdateDate = DateTime.UtcNow;
                                    break;
                                case "AR":
                                    int.TryParse(text, out int updateId);
                                    ptByomei.UpdateId = updateId;
                                    break;
                                case "AS":
                                    ptByomei.UpdateMachine = text;
                                    break;
                                case "AT":
                                    int.TryParse(text, out int id);
                                    ptByomei.Id = id;
                                    break;
                                case "AU":
                                    int.TryParse(text, out int isImportant);
                                    ptByomei.IsImportant = isImportant;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptByomeis.Add(ptByomei);
                    }
                }
            }

            return ptByomeis;
        }

        public static List<TenMst> ReadTenMst(string itemCd, string yjCd)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CommonCheckerTest.xlsx");
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
                                    var newItemCd = (text + itemCd);
                                    tenMst.ItemCd = newItemCd.Length > 10 ? newItemCd.Substring(0, 10) : newItemCd;
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
                                    int.TryParse(text, out int sinKouiKbn);
                                    tenMst.SinKouiKbn = sinKouiKbn;
                                    break;
                                case "G":
                                    tenMst.Name = text;
                                    break;
                                case "Q":
                                    int.TryParse(text, out int tenId);
                                    tenMst.TenId = tenId;
                                    break;
                                case "R":
                                    double.TryParse(text, out double ten);
                                    tenMst.Ten = ten;
                                    break;
                                case "W":
                                    int.TryParse(text, out int odrTermVal);
                                    tenMst.OdrTermVal = odrTermVal;
                                    break;
                                case "X":
                                    int.TryParse(text, out int defaultVal);
                                    tenMst.DefaultVal = defaultVal;
                                    break;
                                case "Z":
                                    int.TryParse(text, out int isAdopted);
                                    tenMst.IsAdopted = isAdopted;
                                    break;
                                case "AA":
                                    int.TryParse(text, out int koukiKbn);
                                    tenMst.KoukiKbn = koukiKbn;
                                    break;
                                case "AB":
                                    int.TryParse(text, out int hokatuKensa);
                                    tenMst.HokatuKensa = hokatuKensa;
                                    break;
                                case "AC":
                                    int.TryParse(text, out int byomeiKbn);
                                    tenMst.ByomeiKbn = byomeiKbn;
                                    break;
                                case "AD":
                                    int.TryParse(text, out int igakukanri);
                                    tenMst.Igakukanri = igakukanri;
                                    break;
                                case "AE":
                                    int.TryParse(text, out int jitudayCount);
                                    tenMst.JitudayCount = jitudayCount;
                                    break;
                                case "AF":
                                    int.TryParse(text, out int jituday);
                                    tenMst.Jituday = jituday;
                                    break;
                                case "AG":
                                    int.TryParse(text, out int dayCount);
                                    tenMst.DayCount = dayCount;
                                    break;
                                case "AH":
                                    int.TryParse(text, out int drugKanrenKbn);
                                    tenMst.DrugKanrenKbn = drugKanrenKbn;
                                    break;
                                case "AI":
                                    int.TryParse(text, out int kizamiId);
                                    tenMst.KizamiId = kizamiId;
                                    break;
                                case "AJ":
                                    int.TryParse(text, out int kizamiMin);
                                    tenMst.KizamiMin = kizamiMin;
                                    break;
                                case "AK":
                                    int.TryParse(text, out int kizamiMax);
                                    tenMst.KizamiMax = kizamiMax;
                                    break;
                                case "AL":
                                    int.TryParse(text, out int kizamiVal);
                                    tenMst.KizamiVal = kizamiVal;
                                    break;
                                case "AM":
                                    int.TryParse(text, out int kizamiTen);
                                    tenMst.KizamiTen = kizamiTen;
                                    break;
                                case "AN":
                                    int.TryParse(text, out int kizamiErr);
                                    tenMst.KizamiErr = kizamiErr;
                                    break;
                                case "AO":
                                    int.TryParse(text, out int maxCount);
                                    tenMst.MaxCount = maxCount;
                                    break;
                                case "AP":
                                    int.TryParse(text, out int maxCountErr);
                                    tenMst.MaxCountErr = maxCountErr;
                                    break;
                                case "AQ":
                                    tenMst.TyuCd = text;
                                    break;
                                case "AR":
                                    tenMst.TyuSeq = text;
                                    break;
                                case "AS":
                                    int.TryParse(text, out int tusokuAge);
                                    tenMst.TusokuAge = tusokuAge;
                                    break;
                                case "AT":
                                    tenMst.MinAge = text;
                                    break;
                                case "AU":
                                    tenMst.MaxAge = text;
                                    break;
                                case "AV":
                                    int.TryParse(text, out int timeKasanKbn);
                                    tenMst.TimeKasanKbn = timeKasanKbn;
                                    break;
                                case "AW":
                                    int.TryParse(text, out int futekiKbn);
                                    tenMst.FutekiKbn = futekiKbn;
                                    break;
                                case "AX":
                                    int.TryParse(text, out int futekiSisetuKbn);
                                    tenMst.FutekiSisetuKbn = futekiSisetuKbn;
                                    break;
                                case "AY":
                                    int.TryParse(text, out int syotiNyuyojiKbn);
                                    tenMst.SyotiNyuyojiKbn = syotiNyuyojiKbn;
                                    break;
                                case "AZ":
                                    int.TryParse(text, out int lowWeightKbn);
                                    tenMst.LowWeightKbn = lowWeightKbn;
                                    break;
                                case "BA":
                                    int.TryParse(text, out int handanKbn);
                                    tenMst.HandanKbn = handanKbn;
                                    break;
                                case "BB":
                                    int.TryParse(text, out int handanGrpKbn);
                                    tenMst.HandanGrpKbn = handanGrpKbn;
                                    break;
                                case "BC":
                                    int.TryParse(text, out int TeigenKbn);
                                    tenMst.TeigenKbn = TeigenKbn;
                                    break;
                                case "BD":
                                    int.TryParse(text, out int sekituiKbn);
                                    tenMst.SekituiKbn = sekituiKbn;
                                    break;
                                case "BE":
                                    int.TryParse(text, out int KeibuKbn);
                                    tenMst.KeibuKbn = KeibuKbn;
                                    break;
                                case "BF":
                                    int.TryParse(text, out int AutoHougouKbn);
                                    tenMst.AutoHougouKbn = AutoHougouKbn;
                                    break;
                                case "BG":
                                    int.TryParse(text, out int GairaiKanriKbn);
                                    tenMst.GairaiKanriKbn = GairaiKanriKbn;
                                    break;
                                case "BH":
                                    int.TryParse(text, out int TusokuTargetKbn);
                                    tenMst.TusokuTargetKbn = TusokuTargetKbn;
                                    break;
                                case "BI":
                                    int.TryParse(text, out int HokatuKbn);
                                    tenMst.HokatuKbn = HokatuKbn;
                                    break;
                                case "BJ":
                                    int.TryParse(text, out int TyoonpaNaisiKbn);
                                    tenMst.TyoonpaNaisiKbn = TyoonpaNaisiKbn;
                                    break;
                                case "BK":
                                    int.TryParse(text, out int AutoFungoKbn);
                                    tenMst.AutoFungoKbn = AutoFungoKbn;
                                    break;
                                case "BL":
                                    int.TryParse(text, out int TyoonpaGyokoKbn);
                                    tenMst.TyoonpaGyokoKbn = TyoonpaGyokoKbn;
                                    break;
                                case "BM":
                                    int.TryParse(text, out int GazoKasan);
                                    tenMst.GazoKasan = GazoKasan;
                                    break;
                                case "BN":
                                    int.TryParse(text, out int KansatuKbn);
                                    tenMst.KansatuKbn = KansatuKbn;
                                    break;
                                case "BO":
                                    int.TryParse(text, out int MasuiKbn);
                                    tenMst.MasuiKbn = MasuiKbn;
                                    break;
                                case "BP":
                                    int.TryParse(text, out int FukubikuNaisiKasan);
                                    tenMst.FukubikuNaisiKasan = FukubikuNaisiKasan;
                                    break;
                                case "BQ":
                                    int.TryParse(text, out int FukubikuKotunanKasan);
                                    tenMst.FukubikuKotunanKasan = FukubikuKotunanKasan;
                                    break;
                                case "BR":
                                    int.TryParse(text, out int MasuiKasan);
                                    tenMst.MasuiKasan = MasuiKasan;
                                    break;
                                case "BS":
                                    int.TryParse(text, out int MoniterKasan);
                                    tenMst.MoniterKasan = MoniterKasan;
                                    break;
                                case "DX":
                                    var newYjcd = text + yjCd;
                                    tenMst.YjCd = newYjcd.Length > 12 ? newYjcd.Substring(0, 12) : newYjcd;
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

        public static List<DrugDayLimit> ReadDrugDayLimit(string itemCd)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CommonCheckerTest.xlsx");
            var drugDayLimits = new List<DrugDayLimit>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "DRUG_DAY_LIMIT").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var drugDay = new DrugDayLimit();
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
                                case "B":
                                    int.TryParse(text, out int hpId);
                                    drugDay.HpId = hpId;
                                    break;
                                case "C":
                                    drugDay.ItemCd = text + itemCd;
                                    break;
                                case "D":
                                    int.TryParse(text, out int seqNo);
                                    drugDay.SeqNo = seqNo;
                                    break;
                                case "E":
                                    int.TryParse(text, out int limitDay);
                                    drugDay.LimitDay = limitDay;
                                    break;
                                case "F":
                                    int.TryParse(text, out int isDeleted);
                                    drugDay.IsDeleted = isDeleted;
                                    break;
                                case "G":
                                    drugDay.CreateDate = DateTime.UtcNow;
                                    break;
                                case "H":
                                    int.TryParse(text, out int createId);
                                    drugDay.CreateId = createId;
                                    break;
                                case "J":
                                    drugDay.UpdateDate = DateTime.UtcNow;
                                    break;
                                case "K":
                                    int.TryParse(text, out int updateId);
                                    drugDay.UpdateId = updateId;
                                    break;
                                case "M":
                                    int.TryParse(text, out int startDate);
                                    drugDay.StartDate = startDate;
                                    break;
                                case "N":
                                    int.TryParse(text, out int endDate);
                                    drugDay.EndDate = endDate;
                                    break;
                                default:
                                    break;
                            }
                        }
                        drugDayLimits.Add(drugDay);
                    }
                }
            }

            return drugDayLimits;
        }

        public static List<M10DayLimit> ReadM10DayLimit(string yjCd)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CommonCheckerTest.xlsx");
            var m10DayLimits = new List<M10DayLimit>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "M10_DAY_LIMIT").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var m10Day = new M10DayLimit();
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
                                    m10Day.YjCd = text + yjCd;
                                    break;
                                case "B":
                                    int.TryParse(text, out int seqNo);
                                    m10Day.SeqNo = seqNo;
                                    break;
                                case "C":
                                    int.TryParse(text, out int LimitDay);
                                    m10Day.LimitDay = LimitDay;
                                    break;
                                case "D":
                                    m10Day.StDate = text;
                                    break;
                                case "E":
                                    m10Day.EdDate = text;
                                    break;
                                default:
                                    break;
                            }
                        }
                        m10DayLimits.Add(m10Day);
                    }
                }
            }

            return m10DayLimits;
        }

        public static List<M42ContraindiDisCon> ReadM42ContaindiDisCon(string byotaiCd)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CommonCheckerTest.xlsx");
            var m42Contraindis = new List<M42ContraindiDisCon>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "M42_CONTRAINDI_DIS_CON").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var m42 = new M42ContraindiDisCon();
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
                                    m42.ByotaiCd = text + byotaiCd;
                                    break;
                                case "B":
                                    m42.StandardByotai = text;
                                    break;
                                case "C":
                                    int.TryParse(text, out int ByotaiKbn);
                                    m42.ByotaiKbn = ByotaiKbn;
                                    break;
                                case "D":
                                    m42.Byomei = text;
                                    break;
                                case "E":
                                    m42.Icd10 = text;
                                    break;
                                case "F":
                                    m42.ReceCd = text;
                                    break;
                                default:
                                    break;
                            }
                        }
                        m42Contraindis.Add(m42);
                    }
                }
            }

            return m42Contraindis;
        }

        public static List<M42ContraindiDrugMainEx> ReadM42ContaindiDrugMainEx(string key)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CommonCheckerTest.xlsx");
            var m42Contraindis = new List<M42ContraindiDrugMainEx>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "M42_CONTRAINDI_DRUG_MAIN_EX").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var m42 = new M42ContraindiDrugMainEx();
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
                                    m42.YjCd = text + key;
                                    break;
                                case "B":
                                    int.TryParse(text, out int TenpuLevel);
                                    m42.TenpuLevel = TenpuLevel;
                                    break;
                                case "C":
                                    m42.ByotaiCd = text + key;
                                    break;
                                case "D":
                                    m42.CmtCd = text;
                                    break;
                                case "E":
                                    int.TryParse(text, out int Stage);
                                    m42.Stage = Stage;
                                    break;
                                case "F":
                                    m42.KioCd = text;
                                    break;
                                case "G":
                                    m42.FamilyCd = text;
                                    break;
                                case "H":
                                    m42.KijyoCd = text;
                                    break;
                                default:
                                    break;
                            }
                        }
                        m42Contraindis.Add(m42);
                    }
                }
            }

            return m42Contraindis;
        }

        public static List<PtInf> ReadPtInf()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CommonCheckerTest.xlsx");
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
                                    int.TryParse(text, out int birthday);
                                    ptInf.Birthday = birthday;
                                    break;
                                case "I":
                                    int.TryParse(text, out int isDead);
                                    ptInf.IsDead = isDead;
                                    break;
                                case "J":
                                    ptInf.DeathDate = 0;
                                    break;
                                case "AF":
                                    int.TryParse(text, out int isRyosyoDetail);
                                    ptInf.IsRyosyoDetail = isRyosyoDetail;
                                    break;
                                case "AG":
                                    int.TryParse(text, out int primaryDoctor);
                                    ptInf.PrimaryDoctor = primaryDoctor;
                                    break;
                                case "AH":
                                    int.TryParse(text, out int isTester);
                                    ptInf.IsTester = isTester;
                                    break;
                                case "AI":
                                    int.TryParse(text, out int isDelete);
                                    ptInf.IsDelete = isDelete;
                                    break;
                                case "AJ":
                                    ptInf.CreateDate = DateTime.UtcNow;
                                    break;
                                case "AM":
                                    ptInf.UpdateDate = DateTime.UtcNow;
                                    break;
                                case "AP":
                                    int.TryParse(text, out int mainHokenPid);
                                    ptInf.MainHokenPid = mainHokenPid;
                                    break;
                                case "AQ":
                                    int.TryParse(text, out int referenceNo);
                                    ptInf.ReferenceNo = referenceNo;
                                    break;
                                case "AR":
                                    int.TryParse(text, out int limitConsFlg);
                                    ptInf.LimitConsFlg = limitConsFlg;
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

        public static List<PtAlrgyDrug> ReadPtAlrgyDrug()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CommonCheckerTest.xlsx");
            var ptAlrgyDrugs = new List<PtAlrgyDrug>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PT_ALRGY_DRUG").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptAlrgy = new PtAlrgyDrug();
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
                                    ptAlrgy.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int ptId);
                                    ptAlrgy.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int seqNo);
                                    ptAlrgy.SeqNo = seqNo;
                                    break;
                                case "D":
                                    int.TryParse(text, out int sortNo);
                                    ptAlrgy.SortNo = sortNo;
                                    break;
                                case "E":
                                    ptAlrgy.ItemCd = text;
                                    break;
                                case "F":
                                    ptAlrgy.DrugName = text;
                                    break;
                                case "G":
                                    int.TryParse(text, out int startDate);
                                    ptAlrgy.StartDate = startDate;
                                    break;
                                case "H":
                                    int.TryParse(text, out int endDate);
                                    ptAlrgy.EndDate = endDate;
                                    break;
                                case "I":
                                    ptAlrgy.Cmt = text;
                                    break;
                                case "J":
                                    int.TryParse(text, out int isDeleted);
                                    ptAlrgy.IsDeleted = isDeleted;
                                    break;
                                case "K":
                                    ptAlrgy.CreateDate = DateTime.UtcNow;
                                    break;
                                case "L":
                                    int.TryParse(text, out int createId);
                                    ptAlrgy.CreateId = createId;
                                    break;
                                case "M":
                                    ptAlrgy.CreateMachine = text;
                                    break;
                                case "N":
                                    ptAlrgy.UpdateDate = DateTime.UtcNow;
                                    break;
                                case "O":
                                    int.TryParse(text, out int updateId);
                                    ptAlrgy.UpdateId = updateId;
                                    break;
                                case "P":
                                    ptAlrgy.UpdateMachine = text;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptAlrgyDrugs.Add(ptAlrgy);
                    }
                }
            }

            return ptAlrgyDrugs;
        }

        public static List<KinkiMst> ReadKinkiMst(string key)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CommonCheckerTest.xlsx");
            var kinkiMsts = new List<KinkiMst>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "KINKI_MST").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var kinkiMst = new KinkiMst();
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
                                    kinkiMst.HpId = hpId;
                                    break;
                                case "B":
                                    kinkiMst.ACd = text + key;
                                    break;
                                case "C":
                                    kinkiMst.BCd = text + key;
                                    break;
                                case "D":
                                    int.TryParse(text, out int seqNo);
                                    kinkiMst.SeqNo = seqNo;
                                    break;
                                case "E":
                                    int.TryParse(text, out int isDeleted);
                                    kinkiMst.IsDeleted = isDeleted;
                                    break;
                                case "F":
                                    kinkiMst.CreateDate = DateTime.UtcNow;
                                    break;
                                case "G":
                                    kinkiMst.CreateId = 2;
                                    break;
                                case "H":
                                    kinkiMst.CreateMachine = text;
                                    break;
                                case "I":
                                    kinkiMst.UpdateDate = DateTime.UtcNow;
                                    break;
                                case "J":
                                    kinkiMst.UpdateId = 2;
                                    break;
                                case "K":
                                    kinkiMst.UpdateMachine = text;
                                    break;
                                case "L":
                                    int.TryParse(text, out int id);
                                    kinkiMst.Id = id;
                                    break;
                                default:
                                    break;
                            }
                        }
                        kinkiMsts.Add(kinkiMst);
                    }
                }
            }

            return kinkiMsts;
        }

        public static List<PtOtherDrug> ReadPtOtherDrug(int ptId)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CommonCheckerTest.xlsx");
            var ptOtherDrugs = new List<PtOtherDrug>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PT_OTHER_DRUG").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptOtherDrug = new PtOtherDrug();
                        foreach (var c in r.Elements<Cell>())
                        {
                            text = c.CellValue?.Text ?? string.Empty;
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
                            }
                            var columnName = GetColumnName(c.CellReference?.ToString() ?? string.Empty);

                            ptOtherDrug.PtId = ptId;
                            switch (columnName)
                            {
                                case "A":
                                    int.TryParse(text, out int hpId);
                                    ptOtherDrug.HpId = hpId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int seqNo);
                                    ptOtherDrug.SeqNo = seqNo;
                                    break;
                                case "D":
                                    int.TryParse(text, out int sortNo);
                                    ptOtherDrug.SortNo = sortNo;
                                    break;
                                case "E":
                                    ptOtherDrug.ItemCd = text;
                                    break;
                                case "F":
                                    ptOtherDrug.DrugName = text;
                                    break;
                                case "G":
                                    ptOtherDrug.StartDate = 0;
                                    break;
                                case "H":
                                    ptOtherDrug.EndDate = 99999999;
                                    break;
                                case "I":
                                    ptOtherDrug.Cmt = text;
                                    break;
                                case "J":
                                    ptOtherDrug.IsDeleted = 0;
                                    break;
                                case "K":
                                    ptOtherDrug.CreateDate = DateTime.UtcNow;
                                    break;
                                case "L":
                                    ptOtherDrug.CreateId = 2;
                                    break;
                                case "M":
                                    ptOtherDrug.CreateMachine = "TEST";
                                    break;
                                case "N":
                                    ptOtherDrug.UpdateDate = DateTime.UtcNow;
                                    break;
                                case "O":
                                    ptOtherDrug.UpdateId = 2;
                                    break;
                                case "P":
                                    ptOtherDrug.UpdateMachine = "TEST";
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptOtherDrugs.Add(ptOtherDrug);
                    }
                }
            }

            return ptOtherDrugs;
        }

        ///public static List<PtOtherDrug> ReadPtOtherDrug()
        ///{
        ///    var rootPath = Environment.CurrentDirectory;
        ///    rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        ///    string fileName = Path.Combine(rootPath, "SampleData", "CommonCheckerTest.xlsx");
        ///    var ptOtherDrugs = new List<PtOtherDrug>();
        ///    using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        ///    {
        ///        var workbookPart = spreadsheetDocument.WorkbookPart;
        ///        var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PT_OTHER_DRUG").WorksheetPart?.Worksheet.Elements<SheetData>().First();
        ///        string text;
        ///        if (sheetData != null)
        ///        {
        ///            foreach (var r in sheetData.Elements<Row>().Skip(1))
        ///            {
        ///                var ptOtherDrug = new PtOtherDrug();
        ///                foreach (var c in r.Elements<Cell>())
        ///                {
        ///                    text = c.CellValue?.Text ?? string.Empty;
        ///                    if (c.DataType != null && c.DataType == CellValues.SharedString)
        ///                    {
        ///                        var stringId = Convert.ToInt32(c.InnerText);
        ///                        text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
        ///                    }
        ///                    var columnName = GetColumnName(c.CellReference?.ToString() ?? string.Empty);

        ///                    switch (columnName)
        ///                    {
        ///                        case "A":
        ///                            int.TryParse(text, out int hpId);
        ///                            ptOtherDrug.HpId = hpId;
        ///                            break;
        ///                        case "B":
        ///                            long.TryParse(text, out long ptId);
        ///                            ptOtherDrug.PtId = ptId;
        ///                            break;
        ///                        case "C":
        ///                            int.TryParse(text, out int seqNo);
        ///                            ptOtherDrug.SeqNo = seqNo;
        ///                            break;
        ///                        case "D":
        ///                            int.TryParse(text, out int sortNo);
        ///                            ptOtherDrug.SortNo = sortNo;
        ///                            break;
        ///                        case "E":
        ///                            ptOtherDrug.ItemCd = text;
        ///                            break;
        ///                        case "F":
        ///                            ptOtherDrug.DrugName = text;
        ///                            break;
        ///                        case "G":
        ///                            int.TryParse(text, out int startDate);
        ///                            ptOtherDrug.StartDate = startDate;
        ///                            break;
        ///                        case "H":
        ///                            int.TryParse(text, out int endDate);
        ///                            ptOtherDrug.EndDate = endDate;
        ///                            break;
        ///                        case "I":
        ///                            ptOtherDrug.Cmt = text;
        ///                            break;
        ///                        case "J":
        ///                            ptOtherDrug.IsDeleted = 0;
        ///                            break;
        ///                        case "K":
        ///                            ptOtherDrug.CreateDate = DateTime.UtcNow;
        ///                            break;
        ///                        case "L":
        ///                            ptOtherDrug.CreateId = 2;
        ///                            break;
        ///                        case "M":
        ///                            ptOtherDrug.CreateMachine = "UNITTEST";
        ///                            break;
        ///                        case "N":
        ///                            ptOtherDrug.UpdateDate = DateTime.UtcNow;
        ///                            break;
        ///                        case "O":
        ///                            ptOtherDrug.UpdateId = 2;
        ///                            break;
        ///                        case "P":
        ///                            ptOtherDrug.UpdateMachine = "UNITTEST";
        ///                            break;
        ///                        default:
        ///                            break;
        ///                    }
        ///                }
        ///                ptOtherDrugs.Add(ptOtherDrug);
        ///            }
        ///        }
        ///    }

        ///    return ptOtherDrugs;
        ///}

        public static List<M12FoodAlrgy> ReadM12FoodAlrgy(string key)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CommonCheckerTest.xlsx");
            var m12FoodAlrgys = new List<M12FoodAlrgy>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "M12_FOOD_ALRGY").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var m12FoodAlrgy = new M12FoodAlrgy();
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
                                    m12FoodAlrgy.KikinCd = text;
                                    break;
                                case "B":
                                    m12FoodAlrgy.YjCd = text + key;
                                    break;
                                case "C":
                                    int.TryParse(text, out int foodKbn);
                                    m12FoodAlrgy.FoodKbn = foodKbn + key;
                                    break;
                                case "D":
                                    m12FoodAlrgy.TenpuLevel = text;
                                    break;
                                case "E":
                                    m12FoodAlrgy.AttentionCmt = text;
                                    break;
                                case "F":
                                    m12FoodAlrgy.WorkingMechanism = text;
                                    break;
                                default:
                                    break;
                            }
                        }
                        m12FoodAlrgys.Add(m12FoodAlrgy);
                    }
                }
            }

            return m12FoodAlrgys;
        }

        public static List<PtOtcDrug> ReadPtOtcDrug()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CommonCheckerTest.xlsx");
            var ptOtcDrugs = new List<PtOtcDrug>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PT_OTC_DRUG").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptOtcDrug = new PtOtcDrug();
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
                                    ptOtcDrug.HpId = hpId;
                                    break;
                                case "B":
                                    long.TryParse(text, out long ptId);
                                    ptOtcDrug.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int seqNo);
                                    ptOtcDrug.SeqNo = seqNo;
                                    break;
                                case "D":
                                    int.TryParse(text, out int sortNo);
                                    ptOtcDrug.SortNo = sortNo;
                                    break;
                                case "E":
                                    int.TryParse(text, out int serialNum);
                                    ptOtcDrug.SerialNum = serialNum;
                                    break;
                                case "F":
                                    ptOtcDrug.TradeName = text;
                                    break;
                                case "G":
                                    int.TryParse(text, out int startDate);
                                    ptOtcDrug.StartDate = startDate;
                                    break;
                                case "H":
                                    int.TryParse(text, out int endDate);
                                    ptOtcDrug.EndDate = endDate;
                                    break;
                                case "I":
                                    ptOtcDrug.Cmt = text;
                                    break;
                                case "J":
                                    int.TryParse(text, out int isDeleted);
                                    ptOtcDrug.IsDeleted = isDeleted;
                                    break;
                                case "K":
                                    ptOtcDrug.CreateDate = DateTime.UtcNow;
                                    break;
                                case "L":
                                    int.TryParse(text, out int createId);
                                    ptOtcDrug.CreateId = createId;
                                    break;
                                case "M":
                                    ptOtcDrug.CreateMachine = text;
                                    break;
                                case "N":
                                    ptOtcDrug.UpdateDate = DateTime.UtcNow;
                                    break;
                                case "O":
                                    int.TryParse(text, out int updateId);
                                    ptOtcDrug.UpdateId = updateId;
                                    break;
                                case "P":
                                    ptOtcDrug.UpdateMachine = text;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptOtcDrugs.Add(ptOtcDrug);
                    }
                }
            }

            return ptOtcDrugs;
        }

        public static List<M38Ingredients> ReadM38Ingredients(string key)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CommonCheckerTest.xlsx");
            var m38Ingredients = new List<M38Ingredients>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "M38_INGREDIENTS").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var m38Ingredient = new M38Ingredients();
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
                                    int.TryParse(text, out int serialNum);
                                    m38Ingredient.SerialNum = serialNum;
                                    break;
                                case "B":
                                    m38Ingredient.SeibunCd = text;
                                    break;
                                case "C":
                                    int.TryParse(text, out int sbt);
                                    m38Ingredient.Sbt = sbt;
                                    break;
                                default:
                                    break;
                            }
                        }
                        m38Ingredients.Add(m38Ingredient);
                    }
                }
            }

            return m38Ingredients;
        }


        public static List<PtSupple> ReadMPtSupple(string key)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CommonCheckerTest.xlsx");
            var ptSupples = new List<PtSupple>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PT_SUPPLE").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptSupple = new PtSupple();
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
                                    ptSupple.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int ptId);
                                    ptSupple.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int seqNo);
                                    ptSupple.SeqNo = seqNo;
                                    break;
                                case "D":
                                    int.TryParse(text, out int sortNo);
                                    ptSupple.SortNo = sortNo;
                                    break;
                                case "E":
                                    ptSupple.IndexCd = text;
                                    break;
                                case "F":
                                    ptSupple.IndexWord = text;
                                    break;
                                case "G":
                                    int.TryParse(text, out int startDate);
                                    ptSupple.StartDate = startDate;
                                    break;
                                case "H":
                                    int.TryParse(text, out int endDate);
                                    ptSupple.EndDate = endDate;
                                    break;
                                case "I":
                                    ptSupple.Cmt = text;
                                    break;
                                case "J":
                                    int.TryParse(text, out int isDeleted);
                                    ptSupple.IsDeleted = isDeleted;
                                    break;
                                case "K":
                                    ptSupple.CreateDate = DateTime.UtcNow;
                                    break;
                                case "L":
                                    int.TryParse(text, out int createId);
                                    ptSupple.CreateId = createId;
                                    break;
                                case "M":
                                    ptSupple.CreateMachine = text;
                                    break;
                                case "N":
                                    ptSupple.UpdateDate = DateTime.UtcNow;
                                    break;
                                case "O":
                                    int.TryParse(text, out int updateId);
                                    ptSupple.UpdateId = updateId;
                                    break;
                                case "P":
                                    ptSupple.UpdateMachine = text;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptSupples.Add(ptSupple);
                    }
                }
            }

            return ptSupples;
        }

        public static List<M41SuppleIndexdef> ReadM41SuppleIndexdef()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CommonCheckerTest.xlsx");
            var m41SuppleIndexdefs = new List<M41SuppleIndexdef>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "M41_SUPPLE_INDEXDEF").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var m41SuppleIndexdef = new M41SuppleIndexdef();
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
                                    m41SuppleIndexdef.SeibunCd = text;
                                    break;
                                case "B":
                                    m41SuppleIndexdef.IndexWord = text;
                                    break;
                                case "C":
                                    m41SuppleIndexdef.TokuhoFlg = text;
                                    break;
                                default:
                                    break;
                            }
                        }
                        m41SuppleIndexdefs.Add(m41SuppleIndexdef);
                    }
                }
            }

            return m41SuppleIndexdefs;
        }

        public static List<M41SuppleIndexcode> ReadM41SuppleIndexcode()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CommonCheckerTest.xlsx");
            var m41SuppleIndexcodes = new List<M41SuppleIndexcode>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "M41_SUPPLE_INDEXCODE").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var m41SuppleIndexcode = new M41SuppleIndexcode();
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
                                    m41SuppleIndexcode.SeibunCd = text;
                                    break;
                                case "B":
                                    m41SuppleIndexcode.IndexCd = text;
                                    break;
                                default:
                                    break;
                            }
                        }
                        m41SuppleIndexcodes.Add(m41SuppleIndexcode);
                    }
                }
            }

            return m41SuppleIndexcodes;
        }

        public static List<M01Kinki> ReadM01Kinki()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CommonCheckerTest.xlsx");
            var m01Kinkis = new List<M01Kinki>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "M01_KINKI").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var m01Kinki = new M01Kinki();
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
                                    m01Kinki.ACd = text;
                                    break;
                                case "B":
                                    m01Kinki.BCd = text;
                                    break;
                                case "C":
                                    m01Kinki.CmtCd = text;
                                    break;
                                case "D":
                                    m01Kinki.SayokijyoCd = text;
                                    break;
                                case "E":
                                    m01Kinki.KyodoCd = text;
                                    break;
                                case "F":
                                    m01Kinki.Kyodo = text;
                                    break;
                                case "G":
                                    m01Kinki.DataKbn = text;
                                    break;
                                default:
                                    break;
                            }
                        }
                        m01Kinkis.Add(m01Kinki);
                    }
                }
            }

            return m01Kinkis;
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
