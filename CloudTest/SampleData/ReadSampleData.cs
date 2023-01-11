using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity.Tenant;

namespace CloudUnitTest.SampleData
{
    public static class ReadSampleData
    {
        //Create COM Objects. Create a COM object for everything that is referenced
        public static List<RaiinInf> ReadRainInf()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "DataSample.xlsx");
            var raiinInfs = new List<RaiinInf>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {

                var workbookPart = spreadsheetDocument.WorkbookPart;
                var worksheetPart = workbookPart?.WorksheetParts.FirstOrDefault();
                var sheetData = worksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var raiinInf = new RaiinInf();
                        raiinInf.CreateId = 1;
                        raiinInf.CreateDate = DateTime.UtcNow;
                        raiinInf.UpdateId = 1;
                        raiinInf.UpdateDate = DateTime.UtcNow;
                        var numberCell = 1;
                        foreach (var c in r.Elements<Cell>())
                        {
                            if (numberCell > 22)
                            {
                                break;
                            }
                            text = c.CellValue?.Text ?? string.Empty;
                            
                            switch (numberCell)
                            {
                                case 1:
                                    int.TryParse(text, out int hpId);
                                    raiinInf.HpId = hpId;
                                    break;
                                case 2:
                                    long.TryParse(text, out long raiinNo);
                                    raiinInf.RaiinNo = raiinNo;
                                    break;
                                case 3:
                                    long.TryParse(text, out long ptId);
                                    raiinInf.PtId = ptId;
                                    break;
                                case 4:
                                    int.TryParse(text, out int sinDate);
                                    raiinInf.SinDate = sinDate;
                                    break;
                                case 5:
                                    long.TryParse(text, out long oyaRaiinNo);
                                    raiinInf.OyaRaiinNo = oyaRaiinNo;
                                    break;
                                case 6:
                                    int.TryParse(text, out int status);
                                    raiinInf.Status = status;
                                    break;
                                case 7:
                                    int.TryParse(text, out int isYokaku);
                                    raiinInf.IsYoyaku = isYokaku;
                                    break;
                                case 8:
                                    int.TryParse(text, out int isYokakuId);
                                    raiinInf.YoyakuId = isYokakuId;
                                    break;
                                case 9:
                                    int.TryParse(text, out int uketukeSBT);
                                    raiinInf.UketukeSbt = uketukeSBT;
                                    break;
                                case 10:
                                    raiinInf.UketukeTime = text;
                                    break;
                                case 11:
                                    int.TryParse(text, out int uketukeId);
                                    raiinInf.UketukeId = uketukeId;
                                    break;
                                case 12:
                                    int.TryParse(text, out int uketukeNo);
                                    raiinInf.UketukeNo = uketukeNo;
                                    break;
                                case 13:
                                    raiinInf.SinStartTime = text;
                                    break;
                                case 14:
                                    raiinInf.SinEndTime = text;
                                    break;
                                case 15:
                                    raiinInf.KaikeiTime = text;
                                    break;
                                case 16:
                                    int.TryParse(text, out int kaikeId);
                                    raiinInf.KaikeiId = kaikeId;
                                    break;
                                case 17:
                                    int.TryParse(text, out int kaId);
                                    raiinInf.KaId = kaId;
                                    break;
                                case 18:
                                    int.TryParse(text, out int tantoId);
                                    raiinInf.TantoId = tantoId;
                                    break;
                                case 19:
                                    int.TryParse(text, out int hokenPId);
                                    raiinInf.HokenPid = hokenPId;
                                    break;
                                case 20:
                                    int.TryParse(text, out int syosaisinKbn);
                                    raiinInf.SyosaisinKbn = syosaisinKbn;
                                    break;
                                case 21:
                                    int.TryParse(text, out int jikanKbn);
                                    raiinInf.SyosaisinKbn = jikanKbn;
                                    break;
                                case 22:
                                    int.TryParse(text, out int isDeleted);
                                    raiinInf.IsDeleted = isDeleted;
                                    break;
                                default:
                                    break;
                            }
                            numberCell++;
                        }
                        raiinInfs.Add(raiinInf);
                    }
                }
            }

            return raiinInfs;
        }
    }
}
