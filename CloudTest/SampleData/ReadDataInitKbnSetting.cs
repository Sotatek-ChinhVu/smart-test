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
                    if (raiinKbnDetail.GrpCd == 0 && raiinKbnDetail.HpId == 0)
                    {
                        break;
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
                    if (raiinKbnInf.HpId == 0 && raiinKbnInf.PtId == 0)
                    {
                        break;
                    }
                    raiinKbnInfList.Add(raiinKbnInf);
                }
            }
        }
        return raiinKbnInfList;
    }

    public static List<RaiinKbnKoui> ReadRaiinKbnKoui()
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        string fileName = Path.Combine(rootPath, "SampleData", "DataInitKbnSetting.xlsx");
        var raiinKbnInfList = new List<RaiinKbnKoui>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
            var workbookPart = spreadsheetDocument.WorkbookPart;
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "RAIIN_KBN_KOUI").WorksheetPart?.Worksheet.Elements<SheetData>().First();
            string text;
            if (sheetData != null)
            {
                foreach (var r in sheetData.Elements<Row>().Skip(1))
                {
                    var raiinKbnKoui = new RaiinKbnKoui();
                    raiinKbnKoui.CreateId = 1;
                    raiinKbnKoui.CreateDate = DateTime.UtcNow;
                    raiinKbnKoui.UpdateId = 1;
                    raiinKbnKoui.UpdateDate = DateTime.UtcNow;
                    raiinKbnKoui.IsDeleted = 0;
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
                                raiinKbnKoui.HpId = hpId;
                                break;
                            case "B":
                                int.TryParse(text, out int grpId);
                                raiinKbnKoui.GrpId = grpId;
                                break;
                            case "C":
                                int.TryParse(text, out int kbnCd);
                                raiinKbnKoui.KbnCd = kbnCd;
                                break;
                            case "D":
                                int.TryParse(text, out int seqNo);
                                raiinKbnKoui.SeqNo = seqNo;
                                break;
                            case "E":
                                int.TryParse(text, out int kouiKbnId);
                                raiinKbnKoui.KouiKbnId = kouiKbnId;
                                break;
                            default:
                                break;
                        }
                        numberCell++;
                    }
                    if (raiinKbnKoui.HpId == 0 && raiinKbnKoui.GrpId == 0)
                    {
                        break;
                    }
                    raiinKbnInfList.Add(raiinKbnKoui);
                }
            }
        }
        return raiinKbnInfList;
    }

    public static List<KouiKbnMst> ReadKouiKbnMst()
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        string fileName = Path.Combine(rootPath, "SampleData", "DataInitKbnSetting.xlsx");
        var kouiKbnMstList = new List<KouiKbnMst>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
            var workbookPart = spreadsheetDocument.WorkbookPart;
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "KOUI_KBN_MST").WorksheetPart?.Worksheet.Elements<SheetData>().First();
            string text;
            if (sheetData != null)
            {
                foreach (var r in sheetData.Elements<Row>().Skip(1))
                {
                    var kouiKbnMst = new KouiKbnMst();
                    kouiKbnMst.CreateId = 1;
                    kouiKbnMst.CreateDate = DateTime.UtcNow;
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
                                kouiKbnMst.HpId = hpId;
                                break;
                            case "B":
                                int.TryParse(text, out int kouiKbnId);
                                kouiKbnMst.KouiKbnId = kouiKbnId;
                                break;
                            case "C":
                                int.TryParse(text, out int sortNo);
                                kouiKbnMst.SortNo = sortNo;
                                break;
                            case "D":
                                int.TryParse(text, out int kouiKbn1);
                                kouiKbnMst.KouiKbn1 = kouiKbn1;
                                break;
                            case "E":
                                int.TryParse(text, out int kouiKbn2);
                                kouiKbnMst.KouiKbn2 = kouiKbn2;
                                break;
                            case "F":
                                kouiKbnMst.KouiGrpName = text;
                                break;
                            case "G":
                                kouiKbnMst.KouiName = text;
                                break;
                            default:
                                break;
                        }
                        numberCell++;
                    }
                    if (kouiKbnMst.HpId == 0 && kouiKbnMst.KouiKbnId == 0)
                    {
                        break;
                    }
                    kouiKbnMstList.Add(kouiKbnMst);
                }
            }
        }
        return kouiKbnMstList;
    }
    
    public static List<RaiinKbItem> ReadRaiinKbnItem()
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        string fileName = Path.Combine(rootPath, "SampleData", "DataInitKbnSetting.xlsx");
        var raiinKbItemList = new List<RaiinKbItem>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
            var workbookPart = spreadsheetDocument.WorkbookPart;
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "RAIIN_KBN_ITEM").WorksheetPart?.Worksheet.Elements<SheetData>().First();
            string text;
            if (sheetData != null)
            {
                foreach (var r in sheetData.Elements<Row>().Skip(1))
                {
                    var raiinKbItem = new RaiinKbItem();
                    raiinKbItem.CreateId = 1;
                    raiinKbItem.CreateDate = DateTime.UtcNow;
                    raiinKbItem.UpdateId = 1;
                    raiinKbItem.UpdateDate = DateTime.UtcNow;
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
                                raiinKbItem.HpId = hpId;
                                break;
                            case "B":
                                int.TryParse(text, out int grpCd);
                                raiinKbItem.GrpCd = grpCd;
                                break;
                            case "C":
                                int.TryParse(text, out int kbnCd);
                                raiinKbItem.KbnCd = kbnCd;
                                break;
                            case "D":
                                int.TryParse(text, out int seqNo);
                                raiinKbItem.SeqNo = seqNo;
                                break;
                            case "E":
                                raiinKbItem.ItemCd = text;
                                break;
                            case "F":
                                int.TryParse(text, out int isExclude);
                                raiinKbItem.IsExclude = isExclude;
                                break;
                            case "G":
                                int.TryParse(text, out int sortNo);
                                raiinKbItem.SortNo = sortNo;
                                break;
                            default:
                                break;
                        }
                        numberCell++;
                    }
                    if (raiinKbItem.HpId == 0 && raiinKbItem.GrpCd == 0)
                    {
                        break;
                    }
                    raiinKbItemList.Add(raiinKbItem);
                }
            }
        }
        return raiinKbItemList;
    }
    
    public static List<RaiinKbnYayoku> ReadRaiinKbnYayoku()
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        string fileName = Path.Combine(rootPath, "SampleData", "DataInitKbnSetting.xlsx");
        var raiinKbnYayokuList = new List<RaiinKbnYayoku>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
            var workbookPart = spreadsheetDocument.WorkbookPart;
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "RAIIN_KBN_YOYAKU").WorksheetPart?.Worksheet.Elements<SheetData>().First();
            string text;
            if (sheetData != null)
            {
                foreach (var r in sheetData.Elements<Row>().Skip(1))
                {
                    var raiinKbItem = new RaiinKbnYayoku();
                    raiinKbItem.CreateId = 1;
                    raiinKbItem.CreateDate = DateTime.UtcNow;
                    raiinKbItem.UpdateId = 1;
                    raiinKbItem.UpdateDate = DateTime.UtcNow;
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
                                raiinKbItem.HpId = hpId;
                                break;
                            case "B":
                                int.TryParse(text, out int grpId);
                                raiinKbItem.GrpId = grpId;
                                break;
                            case "C":
                                int.TryParse(text, out int kbnCd);
                                raiinKbItem.KbnCd = kbnCd;
                                break;
                            case "D":
                                int.TryParse(text, out int seqNo);
                                raiinKbItem.SeqNo = seqNo;
                                break;
                            case "E":
                                int.TryParse(text, out int yoyakuCd);
                                raiinKbItem.YoyakuCd = yoyakuCd;
                                break;
                            default:
                                break;
                        }
                        numberCell++;
                    }
                    if (raiinKbItem.HpId == 0 && raiinKbItem.GrpId == 0)
                    {
                        break;
                    }
                    raiinKbnYayokuList.Add(raiinKbItem);
                }
            }
        }
        return raiinKbnYayokuList;
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
