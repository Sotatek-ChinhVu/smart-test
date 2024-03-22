using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity.Tenant;

namespace CloudUnitTest.SampleData
{
    public static class ConvertConversionItemToOrderInfModelData
    {
        //Create COM Objects. Create a COM object for everything that is referenced
        public static List<IpnMinYakkaMst> ReadIpnMinYakkaMst()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            int count = 1;
            string fileName = Path.Combine(rootPath, "SampleData", "ConvertConversionItemToOrderInfModelSample.xlsx");
            var ipnMinYakkaMsts = new List<IpnMinYakkaMst>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {

                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "ipn_min_yakka_mst").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ipnMinYakkaMst = new IpnMinYakkaMst();
                        ipnMinYakkaMst.CreateId = 1;
                        ipnMinYakkaMst.CreateDate = DateTime.UtcNow;
                        ipnMinYakkaMst.UpdateId = 1;
                        ipnMinYakkaMst.UpdateDate = DateTime.UtcNow;
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
                                    ;
                                    ipnMinYakkaMst.IpnNameCd = text;
                                    break;
                                case "B":
                                    int.TryParse(text, out int startDate);
                                    ipnMinYakkaMst.StartDate = startDate;
                                    break;
                                case "C":
                                    int.TryParse(text, out int seqNo);
                                    ipnMinYakkaMst.SeqNo = seqNo;
                                    break;
                                case "D":
                                    int.TryParse(text, out int endDate);
                                    ipnMinYakkaMst.EndDate = endDate;
                                    break;
                                case "E":
                                    double.TryParse(text, out double yakka);
                                    ipnMinYakkaMst.Yakka = yakka;
                                    break;
                                case "F":
                                    int.TryParse(text, out int isDeleted);
                                    ipnMinYakkaMst.IsDeleted = isDeleted;
                                    break;

                                default:
                                    break;
                            }
                        }
                        ipnMinYakkaMsts.Add(ipnMinYakkaMst);
                        count++;
                    }
                }
            }

            return ipnMinYakkaMsts;
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
