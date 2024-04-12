using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudUnitTest.SampleData.AccountingRepository
{
    public class AccountingRepositoryData
    {
        public static List<RaiinInf> ReadRaiinInf()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var raiinInfs = new List<RaiinInf>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "RaiinInf").WorksheetPart?.Worksheet.Elements<SheetData>().First();
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
                                    raiinInf.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int raiinNo);
                                    raiinInf.RaiinNo = raiinNo;
                                    break;
                                case "C":
                                    long.TryParse(text, out long ptId);
                                    raiinInf.PtId = ptId;
                                    break;
                                case "D":
                                    int.TryParse(text, out int sinDate);
                                    raiinInf.SinDate = sinDate;
                                    break;
                                case "E":
                                    long.TryParse(text, out long oyaRaiinNo);
                                    raiinInf.OyaRaiinNo = oyaRaiinNo;
                                    break;
                                case "F":
                                    int.TryParse(text, out int status);
                                    raiinInf.Status = status;
                                    break;
                                default:
                                    break;
                            }
                        }
                        raiinInfs.Add(raiinInf);
                    }
                }
            }

            return raiinInfs;
        }

        public static List<KaikeiInf> ReadKaikeiInf()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var kaikeiInfs = new List<KaikeiInf>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "KaikeiInf").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var kaikeiInf = new KaikeiInf();
                        kaikeiInf.CreateId = 1;
                        kaikeiInf.CreateDate = DateTime.UtcNow;
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
                                    kaikeiInf.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int ptId);
                                    kaikeiInf.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int sinDate);
                                    kaikeiInf.SinDate = sinDate;
                                    break;
                                case "D":
                                    int.TryParse(text, out int raiinNo);
                                    kaikeiInf.RaiinNo = raiinNo;
                                    break;
                                case "V":
                                    int.TryParse(text, out int kohi1Id);
                                    kaikeiInf.Kohi1Id = kohi1Id;
                                    break;
                                case "W":
                                    int.TryParse(text, out int kohi2Id);
                                    kaikeiInf.Kohi2Id = kohi2Id;
                                    break;
                                case "X":
                                    int.TryParse(text, out int kohi3Id);
                                    kaikeiInf.Kohi3Id = kohi3Id;
                                    break;
                                case "Y":
                                    int.TryParse(text, out int kohi4Id);
                                    kaikeiInf.Kohi4Id = kohi4Id;
                                    break;
                                default:
                                    break;
                            }
                        }
                        kaikeiInfs.Add(kaikeiInf);
                    }
                }
            }

            return kaikeiInfs;
        }

        public static List<PtInf> ReadPtInf()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var ptInfs = new List<PtInf>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PtInf").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptInf = new PtInf();
                        ptInf.CreateId = 1;
                        ptInf.CreateDate = DateTime.UtcNow;
                        ptInf.UpdateId = 1;
                        ptInf.UpdateDate = DateTime.UtcNow;
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
                                    ptInf.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int ptId);
                                    ptInf.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int seqNo);
                                    ptInf.SeqNo = seqNo;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptInfs.Add(ptInf);
                    }
                }
            }
            return ptInfs;
        }

        public static List<PtHokenInf> ReadPtHokenInf()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var ptHokenInfs = new List<PtHokenInf>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PtHokenInf").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptHokenInf = new PtHokenInf();
                        ptHokenInf.CreateId = 1;
                        ptHokenInf.CreateDate = DateTime.UtcNow;
                        ptHokenInf.UpdateId = 1;
                        ptHokenInf.UpdateDate = DateTime.UtcNow;
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
                                    ptHokenInf.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int ptId);
                                    ptHokenInf.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int hokenId);
                                    ptHokenInf.HokenId = hokenId;
                                    break;
                                case "D":
                                    int.TryParse(text, out int seqNo);
                                    ptHokenInf.SeqNo = seqNo;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptHokenInfs.Add(ptHokenInf);
                    }
                }
            }

            return ptHokenInfs;
        }

        public static List<PtHokenPattern> ReadPtHokenPattern()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var ptHokenPatterns = new List<PtHokenPattern>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PtHokenPattern").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptHokenPattern = new PtHokenPattern();
                        ptHokenPattern.CreateId = 1;
                        ptHokenPattern.CreateDate = DateTime.UtcNow;
                        ptHokenPattern.UpdateId = 1;
                        ptHokenPattern.UpdateDate = DateTime.UtcNow;
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
                                    ptHokenPattern.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int ptId);
                                    ptHokenPattern.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int hokenPId);
                                    ptHokenPattern.HokenPid = hokenPId;
                                    break;
                                case "D":
                                    int.TryParse(text, out int seqNo);
                                    ptHokenPattern.SeqNo = seqNo;
                                    break;
                                case "G":
                                    int.TryParse(text, out int hokenId);
                                    ptHokenPattern.HokenId = hokenId;
                                    break;
                                case "H":
                                    int.TryParse(text, out int kohi1Id);
                                    ptHokenPattern.Kohi1Id = kohi1Id;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptHokenPatterns.Add(ptHokenPattern);
                    }
                }
            }

            return ptHokenPatterns;
        }

        public static List<PtKohi> ReadPtKohi()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var ptKohis = new List<PtKohi>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PtKohi").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptKohi = new PtKohi();
                        ptKohi.CreateId = 1;
                        ptKohi.CreateDate = DateTime.UtcNow;
                        ptKohi.UpdateId = 1;
                        ptKohi.UpdateDate = DateTime.UtcNow;
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
                                    ptKohi.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int ptId);
                                    ptKohi.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int hokenId);
                                    ptKohi.HokenId = hokenId;
                                    break;
                                case "D":
                                    int.TryParse(text, out int seqNo);
                                    ptKohi.SeqNo = seqNo;
                                    break;

                                case "F":
                                    int.TryParse(text, out int hokenNo);
                                    ptKohi.HokenNo = hokenNo;
                                    break;
                                case "G":
                                    int.TryParse(text, out int hokenEdaNo);
                                    ptKohi.HokenEdaNo = hokenEdaNo;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptKohis.Add(ptKohi);
                    }
                }
            }
            return ptKohis;
        }

        public static List<PtHokenCheck> ReadPtHokenCheck()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var ptHokenChecks = new List<PtHokenCheck>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PtHokenCheck").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var ptHokenCheck = new PtHokenCheck();
                        ptHokenCheck.CreateId = 1;
                        ptHokenCheck.CreateDate = DateTime.UtcNow;
                        ptHokenCheck.UpdateId = 1;
                        ptHokenCheck.UpdateDate = DateTime.UtcNow;
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
                                    ptHokenCheck.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int ptId);
                                    ptHokenCheck.PtID = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int hokenGrp);
                                    ptHokenCheck.HokenGrp = hokenGrp;
                                    break;
                                case "D":
                                    int.TryParse(text, out int hokenId);
                                    ptHokenCheck.HokenId = hokenId;
                                    break;
                                default:
                                    break;
                            }
                        }
                        ptHokenChecks.Add(item: ptHokenCheck);
                    }
                }
            }
            return ptHokenChecks;
        }

        public static List<HokenMst> ReadHokenMst()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var hokenMsts = new List<HokenMst>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "HokenMst").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var hokenMst = new HokenMst();
                        hokenMst.CreateId = 1;
                        hokenMst.CreateDate = DateTime.UtcNow;
                        hokenMst.UpdateId = 1;
                        hokenMst.UpdateDate = DateTime.UtcNow;
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
                                    hokenMst.HpId = hpId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int hokenNo);
                                    hokenMst.HokenNo = hokenNo;
                                    break;
                                case "D":
                                    int.TryParse(text, out int hokenEdaNo);
                                    hokenMst.HokenEdaNo = hokenEdaNo;
                                    break;
                                case "AP":
                                    int.TryParse(text, out int monthLimitCount);
                                    hokenMst.MonthLimitCount = monthLimitCount;
                                    break;
                                default:
                                    break;
                            }
                        }
                        hokenMsts.Add(hokenMst);
                    }
                }
            }
            return hokenMsts;
        }

        public static List<SyunoSeikyu> ReadSyunoSeikyu()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var syunoSeikyus = new List<SyunoSeikyu>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "SyunoSeikyu").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var syunoSeikyu = new SyunoSeikyu();
                        syunoSeikyu.CreateId = 1;
                        syunoSeikyu.CreateDate = DateTime.UtcNow;
                        syunoSeikyu.UpdateId = 1;
                        syunoSeikyu.UpdateDate = DateTime.UtcNow;
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
                                    syunoSeikyu.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int ptId);
                                    syunoSeikyu.PtId = ptId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int sinDate);
                                    syunoSeikyu.SinDate = sinDate;
                                    break;
                                case "D":
                                    long.TryParse(text, out long raiiNo);
                                    syunoSeikyu.RaiinNo = raiiNo;
                                    break;
                                case "E":
                                    int.TryParse(text, out int nyukinKbn);
                                    syunoSeikyu.NyukinKbn = nyukinKbn;
                                    break;
                                default:
                                    break;
                            }
                        }
                        syunoSeikyus.Add(syunoSeikyu);
                    }
                }
            }
            return syunoSeikyus;
        }

        public static List<SyunoNyukin> ReadSyunoNyukin()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var syunoNyukins = new List<SyunoNyukin>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "SyunoNyukin").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var syunoNyukin = new SyunoNyukin();
                        syunoNyukin.CreateId = 1;
                        syunoNyukin.CreateDate = DateTime.UtcNow;
                        syunoNyukin.UpdateId = 1;
                        syunoNyukin.UpdateDate = DateTime.UtcNow;
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
                                    syunoNyukin.HpId = hpId;
                                    break;
                                case "B":
                                    long.TryParse(text, out long raiiNo);
                                    syunoNyukin.RaiinNo = raiiNo;
                                    break;
                                case "C":
                                    int.TryParse(text, out int ptId);
                                    syunoNyukin.PtId = ptId;
                                    break;
                                case "D":
                                    int.TryParse(text, out int sinDate);
                                    syunoNyukin.SinDate = sinDate;
                                    break;
                                default:
                                    break;
                            }
                        }
                        syunoNyukins.Add(syunoNyukin);
                    }
                }
            }
            return syunoNyukins;
        }

        public static List<HpInf> ReadHpInf()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var hpInfs = new List<HpInf>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "HpInf").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var hpInf = new HpInf();
                        hpInf.CreateId = 1;
                        hpInf.CreateDate = DateTime.UtcNow;
                        hpInf.UpdateId = 1;
                        hpInf.UpdateDate = DateTime.UtcNow;
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
                                    hpInf.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int startDate);
                                    hpInf.StartDate = startDate;
                                    break;
                                case "I":
                                    int.TryParse(text, out int prefNo);
                                    hpInf.PrefNo = prefNo;
                                    break;
                                default:
                                    break;
                            }
                        }
                        hpInfs.Add(hpInf);
                    }
                }
            }
            return hpInfs;
        }

        public static List<UserMst> ReadUserMst()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var userMsts = new List<UserMst>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "UserMst").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var userMst = new UserMst();
                        userMst.CreateId = 1;
                        userMst.CreateDate = DateTime.UtcNow;
                        userMst.UpdateId = 1;
                        userMst.UpdateDate = DateTime.UtcNow;
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
                                    userMst.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int userId);
                                    userMst.UserId = userId;
                                    break;
                                case "C":
                                    int.TryParse(text, out int jobCd);
                                    userMst.JobCd = jobCd;
                                    break;
                                case "D":
                                    int.TryParse(text, out int managerKbn);
                                    userMst.ManagerKbn = managerKbn;
                                    break;
                                case "E":
                                    int.TryParse(text, out int kaId);
                                    userMst.KaId = kaId;
                                    break;
                                case "L":
                                    int.TryParse(text, out int endDate);
                                    userMst.EndDate = endDate;
                                    break;
                                case "G":
                                    userMst.Name = text;
                                    break;
                                case "H":
                                    userMst.Sname = text;
                                    break;
                                case "I":
                                    userMst.LoginId = text;
                                    break;
                                case "M":
                                    int.TryParse(text, out int sortNo);
                                    userMst.SortNo = sortNo;
                                    break;
                                default:
                                    break;
                            }
                        }
                        userMsts.Add(userMst);
                    }
                }
            }
            return userMsts;
        }

        public static List<UserPermission> ReadUserPermission()
        {
            var rootPath = Environment.CurrentDirectory;
            rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

            string fileName = Path.Combine(rootPath, "SampleData", "AccountingRepository", "AccountingRepositorySample.xlsx");
            var userPermissions = new List<UserPermission>();
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var sheetData = GetworksheetBySheetName(spreadsheetDocument, "UserPermission").WorksheetPart?.Worksheet.Elements<SheetData>().First();
                string text;
                if (sheetData != null)
                {
                    foreach (var r in sheetData.Elements<Row>().Skip(1))
                    {
                        var userPermission = new UserPermission();
                        userPermission.CreateId = 1;
                        userPermission.CreateDate = DateTime.UtcNow;
                        userPermission.UpdateId = 1;
                        userPermission.UpdateDate = DateTime.UtcNow;
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
                                    userPermission.HpId = hpId;
                                    break;
                                case "B":
                                    int.TryParse(text, out int userId);
                                    userPermission.UserId = userId;
                                    break;
                                case "C":
                                    userPermission.FunctionCd = text;
                                    break;
                                case "D":
                                    int.TryParse(text, out int permission);
                                    userPermission.Permission = permission;
                                    break;                               
                                default:
                                    break;
                            }
                        }
                        userPermissions.Add(userPermission);
                    }
                }
            }
            return userPermissions;
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
