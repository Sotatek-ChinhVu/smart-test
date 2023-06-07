using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity.Tenant;
using Moq;

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

                                    ptByomei.CreateDate = DateTime.Parse(text);
                                    break;
                                case "AO":
                                    int.TryParse(text, out int createId);
                                    ptByomei.CreateId = createId;
                                    break;
                                case "AP":
                                    ptByomei.CreateMachine = text;
                                    break;
                                case "AQ":
                                    ptByomei.UpdateDate = DateTime.Parse(text);
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
                                    tenMst.ItemCd = text + itemCd;
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
                                    tenMst.SyotiNyuyojiKbn= syotiNyuyojiKbn;
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
                                    tenMst.TeigenKbn= TeigenKbn;
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
                                    tenMst.AutoHougouKbn= AutoHougouKbn;
                                    break;
                                case "BG":
                                    int.TryParse(text, out int GairaiKanriKbn);
                                    tenMst.GairaiKanriKbn = GairaiKanriKbn;
                                    break;
                                case "BH":
                                    int.TryParse(text, out int TusokuTargetKbn);
                                    tenMst.TusokuTargetKbn= TusokuTargetKbn;
                                    break;
                                case "BI":
                                    int.TryParse(text, out int HokatuKbn);
                                    tenMst.HokatuKbn= HokatuKbn;
                                    break;
                                case "BJ":
                                    int.TryParse(text, out int TyoonpaNaisiKbn);
                                    tenMst.TyoonpaNaisiKbn= TyoonpaNaisiKbn;
                                    break;
                                case "BK":
                                    int.TryParse(text, out int AutoFungoKbn);
                                    tenMst.AutoFungoKbn= AutoFungoKbn;
                                    break;
                                case "BL":
                                    int.TryParse(text, out int TyoonpaGyokoKbn);
                                    tenMst.TyoonpaGyokoKbn= TyoonpaGyokoKbn;
                                    break;
                                case "BM":
                                    int.TryParse(text, out int GazoKasan);
                                    tenMst.GazoKasan= GazoKasan;
                                    break;
                                case "BN":
                                    int.TryParse(text, out int KansatuKbn);
                                    tenMst.KansatuKbn= KansatuKbn;
                                    break;
                                case "BO":
                                    int.TryParse(text, out int MasuiKbn);
                                    tenMst.MasuiKbn= MasuiKbn;
                                    break;
                                case "BP":
                                    int.TryParse(text, out int FukubikuNaisiKasan);
                                    tenMst.FukubikuNaisiKasan= FukubikuNaisiKasan;
                                    break;
                                case "BQ":
                                    int.TryParse(text, out int FukubikuKotunanKasan);
                                    tenMst.FukubikuKotunanKasan= FukubikuKotunanKasan;
                                    break;
                                case "BR":
                                    int.TryParse(text, out int MasuiKasan) ;
                                    tenMst.MasuiKasan= MasuiKasan;
                                    break;
                                case "BS":
                                    int.TryParse(text, out int MoniterKasan);
                                    tenMst.MoniterKasan= MoniterKasan;
                                    break;
                                case "DX":
                                    tenMst.YjCd = text + yjCd;
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
