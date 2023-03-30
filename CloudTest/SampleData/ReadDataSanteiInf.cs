using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity.Tenant;

namespace CloudUnitTest.SampleData;

public static class ReadDataSanteiInf
{
    public static List<SanteiInf> ReadSanteiInf()
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        string fileName = Path.Combine(rootPath, "SampleData", "DataUnitTestSantei.xlsx");
        var santeiInfs = new List<SanteiInf>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
            var workbookPart = spreadsheetDocument.WorkbookPart;
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "SANTEI_INF").WorksheetPart?.Worksheet.Elements<SheetData>().First();
            string text;
            if (sheetData != null)
            {
                foreach (var r in sheetData.Elements<Row>().Skip(1))
                {
                    var santeiInf = new SanteiInf();
                    santeiInf.CreateId = 1;
                    santeiInf.CreateDate = DateTime.UtcNow;
                    santeiInf.UpdateId = 1;
                    santeiInf.UpdateDate = DateTime.UtcNow;
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
                                santeiInf.HpId = hpId;
                                break;
                            case "B":
                                long.TryParse(text, out long ptId);
                                santeiInf.PtId = ptId;
                                break;
                            case "C":
                                santeiInf.ItemCd = text;
                                break;
                            case "D":
                                int.TryParse(text, out int seqNo);
                                santeiInf.SeqNo = seqNo;
                                break;
                            case "E":
                                int.TryParse(text, out int alertDays);
                                santeiInf.AlertDays = alertDays;
                                break;
                            case "F":
                                int.TryParse(text, out int alertTerm);
                                santeiInf.AlertTerm = alertTerm;
                                break;
                            case "G":
                                long.TryParse(text, out long id);
                                santeiInf.Id = id;
                                break;
                            default:
                                break;
                        }
                        numberCell++;
                    }
                    santeiInfs.Add(santeiInf);
                }
            }
        }
        return santeiInfs;
    }

    public static List<SanteiInfDetail> ReadSanteiInfDetail()
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        string fileName = Path.Combine(rootPath, "SampleData", "DataUnitTestSantei.xlsx");
        var santeiInfDetails = new List<SanteiInfDetail>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
            var workbookPart = spreadsheetDocument.WorkbookPart;
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "SANTEI_INF_DETAIL").WorksheetPart?.Worksheet.Elements<SheetData>().First();
            string text;
            if (sheetData != null)
            {
                foreach (var r in sheetData.Elements<Row>().Skip(1))
                {
                    var santeiInfDetail = new SanteiInfDetail();
                    santeiInfDetail.CreateId = 1;
                    santeiInfDetail.CreateDate = DateTime.UtcNow;
                    santeiInfDetail.UpdateId = 1;
                    santeiInfDetail.UpdateDate = DateTime.UtcNow;
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
                                santeiInfDetail.HpId = hpId;
                                break;
                            case "B":
                                long.TryParse(text, out long ptId);
                                santeiInfDetail.PtId = ptId;
                                break;
                            case "C":
                                santeiInfDetail.ItemCd = text;
                                break;
                            case "D":
                                int.TryParse(text, out int seqNo);
                                santeiInfDetail.SeqNo = seqNo;
                                break;
                            case "E":
                                int.TryParse(text, out int endDate);
                                santeiInfDetail.EndDate = endDate;
                                break;
                            case "F":
                                int.TryParse(text, out int kisanSbt);
                                santeiInfDetail.KisanSbt = kisanSbt;
                                break;
                            case "G":
                                int.TryParse(text, out int kisanDate);
                                santeiInfDetail.KisanDate = kisanDate;
                                break;
                            case "H":
                                santeiInfDetail.Byomei = text;
                                break;
                            case "I":
                                santeiInfDetail.HosokuComment = text;
                                break;
                            case "J":
                                santeiInfDetail.Comment = text;
                                break;
                            case "K":
                                int.TryParse(text, out int isDelete);
                                santeiInfDetail.IsDeleted = isDelete;
                                break;
                            case "L":
                                long.TryParse(text, out long id);
                                santeiInfDetail.Id = id;
                                break;
                            default:
                                break;
                        }
                        numberCell++;
                    }
                    santeiInfDetails.Add(santeiInfDetail);
                }
            }
        }
        return santeiInfDetails;
    }

    public static List<OdrInf> ReadOrderInf()
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        string fileName = Path.Combine(rootPath, "SampleData", "DataUnitTestSantei.xlsx");
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
                                odrInf.HpId = hpId;
                                break;
                            case "B":
                                long.TryParse(text, out long raiinNo);
                                odrInf.RaiinNo = raiinNo;
                                break;
                            case "C":
                                long.TryParse(text, out long rpNo);
                                odrInf.RpNo = rpNo;
                                break;
                            case "D":
                                long.TryParse(text, out long rpEdaNo);
                                odrInf.RpEdaNo = rpEdaNo;
                                break;
                            case "E":
                                long.TryParse(text, out long ptId);
                                odrInf.PtId = ptId;
                                break;
                            case "F":
                                int.TryParse(text, out int sindate);
                                odrInf.SinDate = sindate;
                                break;
                            case "G":
                                int.TryParse(text, out int hokenPid);
                                odrInf.HokenPid = hokenPid;
                                break;
                            case "Q":
                                int.TryParse(text, out int isDelete);
                                odrInf.IsDeleted = isDelete;
                                break;
                            case "R":
                                long.TryParse(text, out long id);
                                odrInf.Id = id;
                                break;
                            default:
                                break;
                        }
                        numberCell++;
                    }
                    odrInfs.Add(odrInf);
                }
            }
        }
        return odrInfs;
    }

    public static List<OdrInfDetail> ReadOrderInfDetail()
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        string fileName = Path.Combine(rootPath, "SampleData", "DataUnitTestSantei.xlsx");
        var odrInfDetails = new List<OdrInfDetail>();
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
                                odrInfDetail.HpId = hpId;
                                break;
                            case "B":
                                long.TryParse(text, out long raiinNo);
                                odrInfDetail.RaiinNo = raiinNo;
                                break;
                            case "C":
                                long.TryParse(text, out long rpNo);
                                odrInfDetail.RpNo = rpNo;
                                break;
                            case "D":
                                long.TryParse(text, out long rpEdaNo);
                                odrInfDetail.RpEdaNo = rpEdaNo;
                                break;
                            case "E":
                                int.TryParse(text, out int rowNo);
                                odrInfDetail.RowNo = rowNo;
                                break;
                            case "F":
                                long.TryParse(text, out long ptId);
                                odrInfDetail.PtId = ptId;
                                break;
                            case "G":
                                int.TryParse(text, out int sindate);
                                odrInfDetail.SinDate = sindate;
                                break;
                            case "I":
                                odrInfDetail.ItemCd = text;
                                break;
                            case "J":
                                odrInfDetail.ItemName = text;
                                break;
                            case "K":
                                int.TryParse(text, out int suryo);
                                odrInfDetail.Suryo = suryo;
                                break;
                            default:
                                break;
                        }
                        numberCell++;
                    }
                    odrInfDetails.Add(odrInfDetail);
                }
            }
        }
        return odrInfDetails;
    }

    public static List<TenMst> ReadTenMst()
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
        string fileName = Path.Combine(rootPath, "SampleData", "DataUnitTestSantei.xlsx");
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
                            case "G":
                                tenMst.Name = text;
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
        StringValue relationshipId = workbookPart?.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name != null && s.Equals(sheetName))?.Id ?? string.Empty;

        var worksheet = workbookPart != null ? ((WorksheetPart)workbookPart.GetPartById(relationshipId.Value ?? string.Empty)).Worksheet : new();

        return worksheet;
    }
}
