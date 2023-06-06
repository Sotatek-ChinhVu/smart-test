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

        public static List<SystemConf> ReadSystemConf()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CommonCheckerTest.xlsx");
            var systemConfs = new List<SystemConf>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {

                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "SYSTEM_CONF").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var systemConf = new SystemConf();
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
                                    systemConf.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int ptId);
                                    systemConf.GrpCd = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int seqNo);
                                    systemConf.GrpEdaNo = seqNo;
                                    break;
                                case "D":
                                    int.TryParse(text, out int val);
                                    systemConf.Val = val;
                                    break;
                                case "E":
                                    systemConf.Param = text;
                                    break;
                                case "F":
                                    systemConf.Biko = text;
                                    break;
                                case "G":
                                    systemConf.CreateDate = DateTime.Now;
                                    break;
                                case "H":
                                    int.TryParse(text, out int createId);
                                    systemConf.CreateId = createId;
                                    break;
                                case "I":
                                    systemConf.CreateMachine = text;
                                    break;
                                case "J":
                                    systemConf.UpdateDate = DateTime.Now;
                                    break;
                                case "K":
                                    int.TryParse(text, out int updateId);
                                    systemConf.UpdateId = updateId;
                                    break;
                                case "L":
                                    systemConf.UpdateMachine = text;
                                    break;
                                default:
                                    break;
                            }
                        }
                        systemConfs.Add(systemConf);
                    }
                }
            }

            return systemConfs;
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
