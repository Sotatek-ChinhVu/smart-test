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

        //Create COM Objects. Create a COM object for everything that is referenced
        public static List<DensiSanteiKaisu> ReadDensiSanteiKaisu()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "DataSample.xlsx");
            var densiSanteiKaisus = new List<DensiSanteiKaisu>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "DENSI_SANTEI_KAISU").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var densiSanteiKaisu = new DensiSanteiKaisu();
                        densiSanteiKaisu.CreateId = 1;
                        densiSanteiKaisu.CreateDate = DateTime.UtcNow;
                        densiSanteiKaisu.UpdateId = 1;
                        densiSanteiKaisu.UpdateDate = DateTime.UtcNow;
                        var numberCell = 1;
                        foreach (var c in r.Elements<Cell>())
                        {
                            if (numberCell > 15)
                            {
                                break;
                            }
                            text = c.CellValue?.Text ?? string.Empty;
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText);
                                text = workbookPart?.SharedStringTablePart?.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText ?? string.Empty;
                            }

                            switch (numberCell)
                            {
                                case 1:
                                    int.TryParse(text, out int hpId);
                                    densiSanteiKaisu.HpId = hpId;
                                    break;
                                case 2:
                                    densiSanteiKaisu.ItemCd = text;
                                    break;
                                case 3:
                                    int.TryParse(text, out int unitCd);
                                    densiSanteiKaisu.UnitCd = unitCd;
                                    break;
                                case 4:
                                    int.TryParse(text, out int startDate);
                                    densiSanteiKaisu.StartDate = startDate;
                                    break;
                                case 5:
                                    int.TryParse(text, out int seqNo);
                                    densiSanteiKaisu.SeqNo = seqNo;
                                    break;
                                case 6:
                                    int.TryParse(text, out int userSetting);
                                    densiSanteiKaisu.UserSetting = userSetting;
                                    break;
                                case 7:
                                    int.TryParse(text, out int maxCount);
                                    densiSanteiKaisu.MaxCount = maxCount;
                                    break;
                                case 8:
                                    int.TryParse(text, out int spJyoken);
                                    densiSanteiKaisu.SpJyoken = spJyoken;
                                    break;
                                case 9:
                                    int.TryParse(text, out int endDate);
                                    densiSanteiKaisu.EndDate = endDate;
                                    break;
                                case 10:
                                    int.TryParse(text, out int targetKbn);
                                    densiSanteiKaisu.TargetKbn = targetKbn;
                                    break;
                                case 11:
                                    int.TryParse(text, out int termCount);
                                    densiSanteiKaisu.TermCount = termCount;
                                    break;
                                case 12:
                                    int.TryParse(text, out int termSbt);
                                    densiSanteiKaisu.TermSbt = termSbt;
                                    break;
                                case 13:
                                    int.TryParse(text, out int isValid);
                                    densiSanteiKaisu.IsInvalid = isValid;
                                    break;
                                case 14:
                                    int.TryParse(text, out int id);
                                    densiSanteiKaisu.Id = id;
                                    break;
                                case 15:
                                    int.TryParse(text, out int itemGrpCd);
                                    densiSanteiKaisu.ItemGrpCd = itemGrpCd;
                                    break;
                                default:
                                    break;
                            }
                            numberCell++;
                        }
                        densiSanteiKaisus.Add(densiSanteiKaisu);
                    }
                }
            }

            return densiSanteiKaisus;
        }

        //Create COM Objects. Create a COM object for everything that is referenced
        public static List<TenMst> ReadTenMst()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "DataSample.xlsx");
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
                                case "F":
                                    tenMst.Name = text;
                                    break;
                                case "G":
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

        public static List<SinRpInf> ReadSinRpInf()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "DataSample.xlsx");
            var sinRpInfs = new List<SinRpInf>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "SIN_RP_INF").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var sinRpInf = new SinRpInf();
                        sinRpInf.CreateId = 1;
                        sinRpInf.CreateDate = DateTime.UtcNow;
                        sinRpInf.UpdateId = 1;
                        sinRpInf.UpdateDate = DateTime.UtcNow;
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
                                    sinRpInf.HpId = hpId;
                                    break;
                                case "B":
                                    long.TryParse(text, out long ptId);
                                    sinRpInf.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int sinYm);
                                    sinRpInf.SinYm = sinYm;
                                    break;
                                case "D":
                                    int.TryParse(text, out int rpNo);
                                    sinRpInf.RpNo = rpNo;
                                    break;
                                case "E":
                                    int.TryParse(text, out int sinKouiKbn);
                                    sinRpInf.SinKouiKbn = sinKouiKbn;
                                    break;
                                case "F":
                                    int.TryParse(text, out int sinId);
                                    sinRpInf.SinId = sinId;
                                    break;
                                case "H":
                                    int.TryParse(text, out int santeiKbn);
                                    sinRpInf.SanteiKbn = santeiKbn;
                                    break;
                                case "P":
                                    int.TryParse(text, out int hokenKbn);
                                    sinRpInf.HokenKbn = hokenKbn;
                                    break;
                                default:
                                    break;
                            }
                        }
                        sinRpInfs.Add(sinRpInf);
                    }
                }
            }

            return sinRpInfs;
        }

        public static List<SinKouiDetail> ReadSinKouiDetail()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "DataSample.xlsx");
            var sinKouiDetails = new List<SinKouiDetail>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "SIN_KOUI_DETAIL").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var sinRpInf = new SinKouiDetail();
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
                                    sinRpInf.HpId = hpId;
                                    break;
                                case "B":
                                    long.TryParse(text, out long ptId);
                                    sinRpInf.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int sinYm);
                                    sinRpInf.SinYm = sinYm;
                                    break;
                                case "D":
                                    int.TryParse(text, out int rpNo);
                                    sinRpInf.RpNo = rpNo;
                                    break;
                                case "E":
                                    int.TryParse(text, out int seqNo);
                                    sinRpInf.SeqNo = seqNo;
                                    break;
                                case "H":
                                    sinRpInf.ItemCd = text;
                                    break;
                                case "I":
                                    sinRpInf.ItemName = text;
                                    break;
                                case "J":
                                    var check = double.TryParse(text, out double suryo);
                                    suryo = check ? double.Parse(text, System.Globalization.CultureInfo.InvariantCulture) : 0;
                                    sinRpInf.Suryo = suryo;
                                    break;
                                case "O":
                                    double.TryParse(text, out double ten);
                                    sinRpInf.Ten = ten;
                                    break;
                                default:
                                    break;
                            }
                        }
                        sinKouiDetails.Add(sinRpInf);
                    }
                }
            }

            return sinKouiDetails;
        }

        public static List<SinKouiCount> ReadSinKouiCount()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));
            //string path = Path.Combine(Environment.CurrentDirectory, @"Data\", fileName);

            string fileName = Path.Combine(rootPath, "SampleData", "DataSample.xlsx");
            var sinKouiCounts = new List<SinKouiCount>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "SIN_KOUI_COUNT").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var sinKouiCount = new SinKouiCount();
                        sinKouiCount.CreateId = 1;
                        sinKouiCount.CreateDate = DateTime.UtcNow;
                        sinKouiCount.UpdateId = 1;
                        sinKouiCount.UpdateDate = DateTime.UtcNow;
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
                                    sinKouiCount.HpId = hpId;
                                    break;
                                case "B":
                                    long.TryParse(text, out long ptId);
                                    sinKouiCount.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int sinYm);
                                    sinKouiCount.SinYm = sinYm;
                                    break;
                                case "D":
                                    int.TryParse(text, out int sinDay);
                                    sinKouiCount.SinDay = sinDay;
                                    break;
                                case "E":
                                    long.TryParse(text, out long raiinNo);
                                    sinKouiCount.RaiinNo = raiinNo;
                                    break;
                                case "F":
                                    int.TryParse(text, out int rpNo);
                                    sinKouiCount.RpNo = rpNo;
                                    break;
                                case "G":
                                    int.TryParse(text, out int seqNo);
                                    sinKouiCount.SeqNo = seqNo;
                                    break;
                                case "H":
                                    int.TryParse(text, out int count);
                                    sinKouiCount.Count = count;
                                    break;
                                case "O":
                                    int.TryParse(text, out int sinDate);
                                    sinKouiCount.SinDate = sinDate;
                                    break;
                                default:
                                    break;
                            }
                        }
                        sinKouiCounts.Add(sinKouiCount);
                    }
                }
            }

            return sinKouiCounts;
        }

        private static Worksheet GetworksheetBySheetName(SpreadsheetDocument spreadsheetDocument, string sheetName)
        {

            var workbookPart = spreadsheetDocument.WorkbookPart;
            StringValue relationshipId = workbookPart?.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name != null && s.Name.Equals(sheetName))?.Id ?? string.Empty;

            var worksheet = workbookPart != null ? ((WorksheetPart)workbookPart.GetPartById(relationshipId)).Worksheet : new();

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
