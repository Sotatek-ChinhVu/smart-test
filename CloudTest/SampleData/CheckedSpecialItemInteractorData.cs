using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity.Tenant;

namespace CloudUnitTest.SampleData
{
    public class CheckedSpecialItemInteractorData
    {

        public static List<ItemGrpMst> ReadItemGrpMst(int hpId)
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "CheckedSpecialItemInteractorSample.xlsx");
            var itemGrpMsts = new List<ItemGrpMst>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {

                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "ItemGrpMst").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var itemGrpMst = new ItemGrpMst();
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
                                    itemGrpMst.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int grpSbt);
                                    itemGrpMst.GrpSbt = grpSbt;
                                    break;
                                case "C":
                                    int.TryParse(text, out int itemGrpCd);
                                    itemGrpMst.ItemGrpCd = itemGrpCd;
                                    break;
                                case "D":
                                    int.TryParse(text, out int startDate);
                                    itemGrpMst.StartDate = startDate;
                                    break;
                                case "E":
                                    int.TryParse(text, out int seqNo);
                                    itemGrpMst.SeqNo = seqNo;
                                    break;
                                case "F":
                                    int.TryParse(text, out int endDate);
                                    itemGrpMst.EndDate = endDate;
                                    break;
                                case "G":
                                    itemGrpMst.ItemCd = text;
                                    break;
                                default:
                                    break;
                            }
                        }
                        itemGrpMsts.Add(itemGrpMst);
                    }
                }
            }

            return itemGrpMsts;
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
