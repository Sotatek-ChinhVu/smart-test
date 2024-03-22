using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity.Tenant;

namespace CloudUnitTest.SampleData.TodayOdrRepository
{
    public class FromHistoryData
    {
        //Create COM Objects. Create a COM object for everything that is referenced
        public static List<TenMst> ReadTenMst()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "TodayOdrRepository", "FromHistorySample.xlsx");
            var tenMsts = new List<TenMst>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "TenMst").WorksheetPart?.Worksheet.Elements<SheetData>().First();
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
                                case "U":
                                    tenMst.OdrUnitName = text;
                                    break;
                                case "V":
                                    tenMst.CnvUnitName = text;
                                    break;
                                case "EP":
                                    tenMst.IpnNameCd = text;
                                    break;
                                case "EZ":
                                    tenMst.KensaItemCd = text;
                                    break;
                                case "DX":
                                    tenMst.YjCd = text;
                                    break;
                                case "DB":
                                    int.TryParse(text, out int kohatuKbn);
                                    tenMst.KohatuKbn = kohatuKbn;
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

        public static List<KensaMst> ReadKensaMst()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "TodayOdrRepository", "FromHistorySample.xlsx");
            var kensaMsts = new List<KensaMst>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "KensaMst").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var mst = new KensaMst();
                        mst.CreateId = 1;
                        mst.CreateDate = DateTime.UtcNow;
                        mst.UpdateId = 1;
                        mst.UpdateDate = DateTime.UtcNow;
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
                                    mst.HpId = hpId;
                                    break;
                                case "B":
                                    mst.KensaItemCd = text;
                                    break;
                                case "C":
                                    int.TryParse(text, out int kensaItemSeqNo);
                                    mst.KensaItemSeqNo = kensaItemSeqNo;
                                    break;
                                case "T":
                                    mst.CenterItemCd1 = text;
                                    break;
                                case "U":
                                    mst.CenterItemCd2 = text;
                                    break;
                                default:
                                    break;

                            }
                        }
                        kensaMsts.Add(mst);
                    }
                }
            }

            return kensaMsts;
        }

        public static List<IpnNameMst> ReadIpnNameMst()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "TodayOdrRepository", "FromHistorySample.xlsx");
            var ipnNameMsts = new List<IpnNameMst>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "IpnNameMst").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ipnNameMst = new IpnNameMst();
                        ipnNameMst.CreateId = 1;
                        ipnNameMst.CreateDate = DateTime.UtcNow;
                        ipnNameMst.UpdateId = 1;
                        ipnNameMst.UpdateDate = DateTime.UtcNow;
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
                                    ipnNameMst.IpnNameCd = text;
                                    break;
                                case "B":
                                    int.TryParse(text, out int startDate);
                                    ipnNameMst.StartDate = startDate;
                                    break;
                                case "C":
                                    int.TryParse(text, out int seqNo);
                                    ipnNameMst.SeqNo = seqNo;
                                    break;
                                case "D":
                                    int.TryParse(text, out int endDate);
                                    ipnNameMst.EndDate = endDate;
                                    break;
                                case "E":
                                    ipnNameMst.IpnName = text;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ipnNameMsts.Add(ipnNameMst);
                    }
                }
            }

            return ipnNameMsts;
        }

        public static List<YohoSetMst> ReadYohoSetMst()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "TodayOdrRepository", "FromHistorySample.xlsx");
            var yohoSetMsts = new List<YohoSetMst>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "YohoSetMst").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var yohoSetMst = new YohoSetMst();
                        yohoSetMst.CreateId = 1;
                        yohoSetMst.CreateDate = DateTime.UtcNow;
                        yohoSetMst.UpdateId = 1;
                        yohoSetMst.UpdateDate = DateTime.UtcNow;
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
                                    yohoSetMst.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int userId);
                                    yohoSetMst.UserId = userId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int sortNo);
                                    yohoSetMst.SortNo = sortNo;
                                    break;
                                case "D":

                                    yohoSetMst.ItemCd = text;
                                    break;
                                default:
                                    break;
                            }
                        }
                        yohoSetMsts.Add(yohoSetMst);
                    }
                }
            }
            return yohoSetMsts;
        }

        public static List<IpnKasanExclude> ReadIpnKasanExclude()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "TodayOdrRepository", "FromHistorySample.xlsx");
            var ipnKasanExcludes = new List<IpnKasanExclude>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "IpnKasanExclude").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ipnKasanExclude = new IpnKasanExclude();
                        ipnKasanExclude.CreateId = 1;
                        ipnKasanExclude.CreateDate = DateTime.UtcNow;
                        ipnKasanExclude.UpdateId = 1;
                        ipnKasanExclude.UpdateDate = DateTime.UtcNow;
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
                                    ipnKasanExclude.IpnNameCd = text;
                                    break;
                                case "B":
                                    int.TryParse(text, out int startDate);
                                    ipnKasanExclude.StartDate = startDate;
                                    break;
                                case "C":
                                    int.TryParse(text, out int seqNo);
                                    ipnKasanExclude.SeqNo = seqNo;
                                    break;
                                case "D":
                                    int.TryParse(text, out int endDate);
                                    ipnKasanExclude.EndDate = endDate;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ipnKasanExcludes.Add(ipnKasanExclude);
                    }
                }
            }
            return ipnKasanExcludes;
        }

        public static List<IpnKasanExcludeItem> ReadIpnKasanExcludeItem()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "TodayOdrRepository", "FromHistorySample.xlsx");
            var ipnKasanExcludeItems = new List<IpnKasanExcludeItem>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "IpnKasanExcludeItem").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ipnKasanExcludeItem = new IpnKasanExcludeItem();
                        ipnKasanExcludeItem.CreateId = 1;
                        ipnKasanExcludeItem.CreateDate = DateTime.UtcNow;
                        ipnKasanExcludeItem.UpdateId = 1;
                        ipnKasanExcludeItem.UpdateDate = DateTime.UtcNow;
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
                                    ipnKasanExcludeItem.ItemCd = text;
                                    break;
                                case "B":
                                    int.TryParse(text, out int startDate);
                                    ipnKasanExcludeItem.StartDate = startDate;
                                    break;
                                case "C":
                                    int.TryParse(text, out int endDate);
                                    ipnKasanExcludeItem.EndDate = endDate;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ipnKasanExcludeItems.Add(ipnKasanExcludeItem);
                    }
                }
            }
            return ipnKasanExcludeItems;
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
