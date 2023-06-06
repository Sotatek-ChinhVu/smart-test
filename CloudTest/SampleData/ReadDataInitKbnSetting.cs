using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Entity.Tenant;

namespace CloudUnitTest.SampleData;

public static class ReadDataInitKbnSetting
{
    public static List<RaiinKbnMst> ReadRaiinKbnMst()
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        string fileName = Path.Combine(rootPath, "SampleData", "DataInitKbnSetting.xlsx");
        var raiinKbnMstList = new List<RaiinKbnMst>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
            var workbookPart = spreadsheetDocument.WorkbookPart;
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "RAIIN_KBN_MST").WorksheetPart?.Worksheet.Elements<SheetData>().First();
            string text;
            if (sheetData != null)
            {
                foreach (var r in sheetData.Elements<Row>().Skip(1))
                {
                    var raiinKbnMst = new RaiinKbnMst();
                    raiinKbnMst.CreateId = 1;
                    raiinKbnMst.CreateDate = DateTime.UtcNow;
                    raiinKbnMst.UpdateId = 1;
                    raiinKbnMst.UpdateDate = DateTime.UtcNow;
                    raiinKbnMst.IsDeleted = 0;
                    var numberCell = 1;
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
                                raiinKbnMst.HpId = hpId;
                                break;
                            case "B":
                                int.TryParse(text, out int grpCd);
                                raiinKbnMst.GrpCd = grpCd;
                                break;
                            case "C":
                                int.TryParse(text, out int sortNo);
                                raiinKbnMst.SortNo = sortNo;
                                break;
                            case "D":
                                raiinKbnMst.GrpName = text;
                                break;
                            default:
                                break;
                        }
                        numberCell++;
                    }
                    raiinKbnMstList.Add(raiinKbnMst);
                }
            }
        }
        return raiinKbnMstList;
    }

    public static List<RaiinKbnDetail> ReadRaiinKbnDetail()
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        string fileName = Path.Combine(rootPath, "SampleData", "DataInitKbnSetting.xlsx");
        var raiinKbnDetailList = new List<RaiinKbnDetail>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
            var workbookPart = spreadsheetDocument.WorkbookPart;
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "RAIIN_KBN_DETAIL").WorksheetPart?.Worksheet.Elements<SheetData>().First();
            string text;
            if (sheetData != null)
            {
                foreach (var r in sheetData.Elements<Row>().Skip(1))
                {
                    var raiinKbnDetail = new RaiinKbnDetail();
                    raiinKbnDetail.CreateId = 1;
                    raiinKbnDetail.CreateDate = DateTime.UtcNow;
                    raiinKbnDetail.UpdateId = 1;
                    raiinKbnDetail.UpdateDate = DateTime.UtcNow;
                    raiinKbnDetail.IsDeleted = 0;
                    var numberCell = 1;
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
                                raiinKbnDetail.HpId = hpId;
                                break;
                            case "B":
                                int.TryParse(text, out int grpCd);
                                raiinKbnDetail.GrpCd = grpCd;
                                break;
                            case "C":
                                int.TryParse(text, out int kbnCd);
                                raiinKbnDetail.KbnCd = kbnCd;
                                break;
                            case "D":
                                int.TryParse(text, out int sortNo);
                                raiinKbnDetail.SortNo = sortNo;
                                break;
                            case "E":
                                raiinKbnDetail.KbnName = text;
                                break;
                            case "F":
                                raiinKbnDetail.ColorCd = text;
                                break;
                            case "G":
                                int.TryParse(text, out int isConfirmed);
                                raiinKbnDetail.IsConfirmed = isConfirmed;
                                break;
                            case "H":
                                int.TryParse(text, out int isAuto);
                                raiinKbnDetail.IsAuto = isAuto;
                                break;
                            case "I":
                                int.TryParse(text, out int isAutoDelete);
                                raiinKbnDetail.IsAutoDelete = isAutoDelete;
                                break;
                            default:
                                break;
                        }
                        numberCell++;
                    }
                    raiinKbnDetailList.Add(raiinKbnDetail);
                }
            }
        }
        return raiinKbnDetailList;
    }

    public static List<RaiinKbnInf> ReadRaiinKbnInf()
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        string fileName = Path.Combine(rootPath, "SampleData", "DataInitKbnSetting.xlsx");
        var raiinKbnInfList = new List<RaiinKbnInf>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
            var workbookPart = spreadsheetDocument.WorkbookPart;
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "RAIIN_KBN_INF").WorksheetPart?.Worksheet.Elements<SheetData>().First();
            string text;
            if (sheetData != null)
            {
                foreach (var r in sheetData.Elements<Row>().Skip(1))
                {
                    var raiinKbnInf = new RaiinKbnInf();
                    raiinKbnInf.CreateId = 1;
                    raiinKbnInf.CreateDate = DateTime.UtcNow;
                    raiinKbnInf.UpdateId = 1;
                    raiinKbnInf.UpdateDate = DateTime.UtcNow;
                    raiinKbnInf.IsDelete = 0;
                    var numberCell = 1;
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
                                raiinKbnInf.HpId = hpId;
                                break;
                            case "B":
                                long.TryParse(text, out long ptId);
                                raiinKbnInf.PtId = ptId;
                                break;
                            case "C":
                                int.TryParse(text, out int sinDate);
                                raiinKbnInf.SinDate = sinDate;
                                break;
                            case "D":
                                long.TryParse(text, out long raiinNo);
                                raiinKbnInf.RaiinNo = raiinNo;
                                break;
                            case "E":
                                int.TryParse(text, out int grpId);
                                raiinKbnInf.GrpId = grpId;
                                break;
                            case "F":
                                long.TryParse(text, out long seqNo);
                                raiinKbnInf.SeqNo = seqNo;
                                break;
                            case "G":
                                int.TryParse(text, out int kbnCd);
                                raiinKbnInf.KbnCd = kbnCd;
                                break;
                            default:
                                break;
                        }
                        numberCell++;
                    }
                    raiinKbnInfList.Add(raiinKbnInf);
                }
            }
        }
        return raiinKbnInfList;
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

    private static Worksheet GetworksheetBySheetName(SpreadsheetDocument spreadsheetDocument, string sheetName)
    {
        var workbookPart = spreadsheetDocument.WorkbookPart;
        var temp = workbookPart?.Workbook.Descendants<Sheet>();
        StringValue relationshipId = workbookPart?.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name != null && s.Name.Equals(sheetName))?.Id ?? string.Empty;

        var worksheet = (workbookPart != null && !string.IsNullOrEmpty(relationshipId.ToString())) ? ((WorksheetPart)workbookPart.GetPartById(relationshipId?.Value ?? string.Empty)).Worksheet : new();

        return worksheet;
    }
}
