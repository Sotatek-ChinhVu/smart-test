using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity.Tenant;

namespace CloudUnitTest.SampleData
{
    public static class CheckedDiseaseData
    {
        //Create COM Objects. Create a COM object for everything that is referenced
        public static List<ByomeiMst> ReadByomeiMst(string byomeiCd)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedDiseaseSample.xlsx");
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
                                case "E":
                                    byomeiMst.KanaName1 = text;
                                    break;
                                case "F":
                                    byomeiMst.KanaName2 = text;
                                    break;
                                case "G":
                                    byomeiMst.KanaName3 = text;
                                    break;
                                case "H":
                                    byomeiMst.KanaName4 = text;
                                    break;
                                case "I":
                                    byomeiMst.KanaName5 = text;
                                    break;
                                case "J":
                                    byomeiMst.KanaName6 = text;
                                    break;
                                case "K":
                                    byomeiMst.KanaName7 = text;
                                    break;
                                case "L":
                                    byomeiMst.IkoCd = text;
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

        //Create COM Objects. Create a COM object for everything that is referenced
        public static List<TenMst> ReadTenMst(string itemCd)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedDiseaseSample.xlsx");
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
                                    tenMst.ItemCd = itemCd;
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

        public static List<TekiouByomeiMst> ReadTekiouByomeiMst(string itemCd, string byomeiCd)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedDiseaseSample.xlsx");
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
                                    int.TryParse(text, out int systemDate);
                                    tekiouByomeiMst.SystemData = systemDate;
                                    break;
                                case "H":
                                    int.TryParse(text, out int startYM);
                                    tekiouByomeiMst.StartYM = startYM;
                                    break;
                                case "I":
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
