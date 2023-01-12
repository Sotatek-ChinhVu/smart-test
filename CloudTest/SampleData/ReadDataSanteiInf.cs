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

        string fileName = Path.Combine(rootPath, "SampleData", "DataSample.xlsx");
        var santeiInfs = new List<SanteiInf>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
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
                        if (numberCell > 7)
                        {
                            break;
                        }
                        text = c.CellValue?.Text ?? string.Empty;

                        switch (numberCell)
                        {
                            case 1:
                                int.TryParse(text, out int hpId);
                                santeiInf.HpId = hpId;
                                break;
                            case 2:
                                long.TryParse(text, out long ptId);
                                santeiInf.PtId = ptId;
                                break;
                            case 3:
                                santeiInf.ItemCd = text;
                                break;
                            case 4:
                                int.TryParse(text, out int seqNo);
                                santeiInf.SeqNo = seqNo;
                                break;
                            case 5:
                                int.TryParse(text, out int alertDays);
                                santeiInf.AlertDays = alertDays;
                                break;
                            case 6:
                                int.TryParse(text, out int alertTerm);
                                santeiInf.AlertTerm = alertTerm;
                                break;
                            case 7:
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

        string fileName = Path.Combine(rootPath, "SampleData", "DataSample.xlsx");
        var santeiInfDetails = new List<SanteiInfDetail>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "SANTEI_INF").WorksheetPart?.Worksheet.Elements<SheetData>().First();
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
                        if (numberCell > 7)
                        {
                            break;
                        }
                        text = c.CellValue?.Text ?? string.Empty;

                        switch (numberCell)
                        {
                            case 1:
                                int.TryParse(text, out int hpId);
                                santeiInfDetail.HpId = hpId;
                                break;
                            case 2:
                                long.TryParse(text, out long ptId);
                                santeiInfDetail.PtId = ptId;
                                break;
                            case 3:
                                santeiInfDetail.ItemCd = text;
                                break;
                            case 4:
                                int.TryParse(text, out int seqNo);
                                santeiInfDetail.SeqNo = seqNo;
                                break;
                            case 5:
                                int.TryParse(text, out int endDate);
                                santeiInfDetail.EndDate = endDate;
                                break;
                            case 6:
                                int.TryParse(text, out int kisanSbt);
                                santeiInfDetail.KisanSbt = kisanSbt;
                                break;
                            case 6:
                                int.TryParse(text, out int kisanDate);
                                santeiInfDetail.KisanDate = kisanDate;
                                break;
                            case 6:
                                santeiInfDetail.Byomei = text;
                                break;
                            case 6:
                                santeiInfDetail.HosokuComment = text;
                                break;
                            case 6:
                                santeiInfDetail.Comment = text;
                                break;
                            case 6:
                                int.TryParse(text, out int isDelete);
                                santeiInfDetail.IsDeleted = isDelete;
                                break;
                            case 7:
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

        string fileName = Path.Combine(rootPath, "SampleData", "DataSample.xlsx");
        var odrInfs = new List<OdrInf>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "SANTEI_INF").WorksheetPart?.Worksheet.Elements<SheetData>().First();
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
                        if (numberCell > 7)
                        {
                            break;
                        }
                        text = c.CellValue?.Text ?? string.Empty;

                        switch (numberCell)
                        {
                            case 1:
                                int.TryParse(text, out int hpId);
                                odrInf.HpId = hpId;
                                break;
                            case 3:
                                long.TryParse(text, out long raiinNo);
                                odrInf.RaiinNo = raiinNo;
                                break;
                            case 4:
                                long.TryParse(text, out long rpNo);
                                odrInf.RpNo = rpNo;
                                break;
                            case 5:
                                long.TryParse(text, out long rpEdaNo);
                                odrInf.RpEdaNo = rpEdaNo;
                                break;
                            case 2:
                                long.TryParse(text, out long ptId);
                                odrInf.PtId = ptId;
                                break;
                            case 6:
                                int.TryParse(text, out int sindate);
                                odrInf.SinDate = sindate;
                                break;
                            case 6:
                                int.TryParse(text, out int hokenPid);
                                odrInf.HokenPid = hokenPid;
                                break;
                            case 6:
                                int.TryParse(text, out int isDelete);
                                odrInf.IsDeleted = isDelete;
                                break;
                            case 7:
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

        string fileName = Path.Combine(rootPath, "SampleData", "DataSample.xlsx");
        var odrInfDetails = new List<OdrInfDetail>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "SANTEI_INF").WorksheetPart?.Worksheet.Elements<SheetData>().First();
            string text;
            if (sheetData != null)
            {
                foreach (var r in sheetData.Elements<Row>().Skip(1))
                {
                    var odrInfDetail = new OdrInfDetail();
                    var numberCell = 1;
                    foreach (var c in r.Elements<Cell>())
                    {
                        if (numberCell > 7)
                        {
                            break;
                        }
                        text = c.CellValue?.Text ?? string.Empty;

                        switch (numberCell)
                        {
                            case 1:
                                int.TryParse(text, out int hpId);
                                odrInfDetail.HpId = hpId;
                                break;
                            case 3:
                                long.TryParse(text, out long raiinNo);
                                odrInfDetail.RaiinNo = raiinNo;
                                break;
                            case 4:
                                long.TryParse(text, out long rpNo);
                                odrInfDetail.RpNo = rpNo;
                                break;
                            case 5:
                                long.TryParse(text, out long rpEdaNo);
                                odrInfDetail.RpEdaNo = rpEdaNo;
                                break;
                            case 5:
                                int.TryParse(text, out int rowNo);
                                odrInfDetail.RowNo = rowNo;
                                break;
                            case 2:
                                long.TryParse(text, out long ptId);
                                odrInfDetail.PtId = ptId;
                                break;
                            case 6:
                                int.TryParse(text, out int sindate);
                                odrInfDetail.SinDate = sindate;
                                break;
                            case 6:
                                odrInfDetail.ItemCd = text;
                                break;
                            case 6:
                                odrInfDetail.ItemName = text;
                                break;
                            case 6:
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

    private static Worksheet GetworksheetBySheetName(SpreadsheetDocument spreadsheetDocument, string sheetName)
    {
        var workbookPart = spreadsheetDocument.WorkbookPart;
        string relationshipId = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name.Equals(sheetName))?.Id;

        var worksheet = ((WorksheetPart)workbookPart.GetPartById(relationshipId)).Worksheet;

        return worksheet;
    }
}
