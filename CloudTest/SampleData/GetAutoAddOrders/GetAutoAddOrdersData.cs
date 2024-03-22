using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity.Tenant;

namespace CloudUnitTest.SampleData.GetAutoAddOrders
{
    public class GetAutoAddOrdersData
    {
        public static List<SanteiGrpDetail> ReadSanteiGrpDetail()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "GetAutoAddOrders", "GetAutoAddOrdersSample.xlsx");
            var santeiGrpDetails = new List<SanteiGrpDetail>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "SanteiGrpDetail").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var santeiGrpDetail = new SanteiGrpDetail();
                        santeiGrpDetail.CreateId = 1;
                        santeiGrpDetail.CreateDate = DateTime.UtcNow;
                        santeiGrpDetail.UpdateId = 1;
                        santeiGrpDetail.UpdateDate = DateTime.UtcNow;
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
                                    santeiGrpDetail.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int santeiGrpCd);
                                    santeiGrpDetail.SanteiGrpCd = santeiGrpCd;
                                    break;
                                case "C":
                                    santeiGrpDetail.ItemCd = text;
                                    break;
                                default:
                                    break;
                            }
                        }
                        santeiGrpDetails.Add(santeiGrpDetail);
                    }
                }
            }

            return santeiGrpDetails;
        }

        public static List<SanteiAutoOrder> ReadSanteiAutoOrder()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "GetAutoAddOrders", "GetAutoAddOrdersSample.xlsx");
            var santeiAutoOrders = new List<SanteiAutoOrder>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "SanteiAutoOrder").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var santeiAutoOrder = new SanteiAutoOrder();
                        santeiAutoOrder.CreateId = 1;
                        santeiAutoOrder.CreateDate = DateTime.UtcNow;
                        santeiAutoOrder.UpdateId = 1;
                        santeiAutoOrder.UpdateDate = DateTime.UtcNow;
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
                                    santeiAutoOrder.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int santeiGrpCd);
                                    santeiAutoOrder.SanteiGrpCd = santeiGrpCd;
                                    break;
                                case "C":
                                    int.TryParse(text, out int seqNo);
                                    santeiAutoOrder.SeqNo = seqNo;
                                    break;
                                case "E":
                                    int.TryParse(text, out int endDate);
                                    santeiAutoOrder.EndDate = endDate;
                                    break;
                                case "H":
                                    int.TryParse(text, out int termCnt);
                                    santeiAutoOrder.TermCnt = termCnt;
                                    break;
                                case "I":
                                    int.TryParse(text, out int termSbt);
                                    santeiAutoOrder.TermSbt = termSbt;
                                    break;
                                case "J":
                                    int.TryParse(text, out int cntType);
                                    santeiAutoOrder.CntType = cntType;
                                    break;
                                default:
                                    break;
                            }
                        }
                        santeiAutoOrders.Add(santeiAutoOrder);
                    }
                }
            }

            return santeiAutoOrders;
        }

        public static List<SanteiAutoOrderDetail> ReadSanteiAutoOrderDetail()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "GetAutoAddOrders", "GetAutoAddOrdersSample.xlsx");
            var santeiAutoOrderDetails = new List<SanteiAutoOrderDetail>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "SanteiAutoOrderDetail").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var santeiAutoOrderDetail = new SanteiAutoOrderDetail();
                        santeiAutoOrderDetail.CreateId = 1;
                        santeiAutoOrderDetail.CreateDate = DateTime.UtcNow;
                        santeiAutoOrderDetail.UpdateId = 1;
                        santeiAutoOrderDetail.UpdateDate = DateTime.UtcNow;
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
                                    santeiAutoOrderDetail.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int santeiGrpCd);
                                    santeiAutoOrderDetail.SanteiGrpCd = santeiGrpCd;
                                    break;
                                case "C":
                                    int.TryParse(text, out int seqNo);
                                    santeiAutoOrderDetail.SeqNo = seqNo;
                                    break;
                                case "D":
                                    santeiAutoOrderDetail.ItemCd = text;
                                    break;
                                case "E":
                                    double.TryParse(text, out double suryo);
                                    santeiAutoOrderDetail.Suryo = suryo;
                                    break;
                                default:
                                    break;
                            }
                        }
                        santeiAutoOrderDetails.Add(santeiAutoOrderDetail);
                    }
                }
            }

            return santeiAutoOrderDetails;
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
