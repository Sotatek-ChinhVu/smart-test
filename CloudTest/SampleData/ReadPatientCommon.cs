﻿using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Entity.Tenant;

namespace CloudUnitTest.SampleData;

public static class ReadPatientCommon
{
    public static List<PtMemo> ReadPtMemo()
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        string fileName = Path.Combine(rootPath, "SampleData", "PatientCommonDataSample.xlsx");
        var ptMemos = new List<PtMemo>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
            var workbookPart = spreadsheetDocument.WorkbookPart;
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PT_MEMO").WorksheetPart?.Worksheet.Elements<SheetData>().First();
            string text;
            if (sheetData != null)
            {
                foreach (var r in sheetData.Elements<Row>().Skip(1))
                {
                    var ptMemo = new PtMemo();
                    ptMemo.CreateId = 1;
                    ptMemo.CreateDate = DateTime.UtcNow;
                    ptMemo.UpdateId = 1;
                    ptMemo.UpdateDate = DateTime.UtcNow;
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
                                ptMemo.HpId = hpId;
                                break;
                            case "B":
                                long.TryParse(text, out long ptId);
                                ptMemo.PtId = ptId;
                                break;
                            case "C":
                                int.TryParse(text, out int seqNo);
                                ptMemo.SeqNo = seqNo;
                                break;
                            case "D":
                                ptMemo.Memo = text;
                                break;
                            default:
                                break;
                        }
                        numberCell++;
                    }
                    ptMemos.Add(ptMemo);
                }
            }
        }
        return ptMemos;
    }

    public static List<PtKyusei> ReadPtKyusei()
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        string fileName = Path.Combine(rootPath, "SampleData", "PatientCommonDataSample.xlsx");
        var ptKyuseis = new List<PtKyusei>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
            var workbookPart = spreadsheetDocument.WorkbookPart;
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PT_KYUSEI").WorksheetPart?.Worksheet.Elements<SheetData>().First();
            string text;
            if (sheetData != null)
            {
                foreach (var r in sheetData.Elements<Row>().Skip(1))
                {
                    var ptKyusei = new PtKyusei();
                    ptKyusei.CreateId = 1;
                    ptKyusei.CreateDate = DateTime.UtcNow;
                    ptKyusei.UpdateId = 1;
                    ptKyusei.UpdateDate = DateTime.UtcNow;
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
                                ptKyusei.HpId = hpId;
                                break;
                            case "B":
                                long.TryParse(text, out long ptId);
                                ptKyusei.PtId = ptId;
                                break;
                            case "C":
                                int.TryParse(text, out int seqNo);
                                ptKyusei.SeqNo = seqNo;
                                break;
                            case "D":
                                ptKyusei.KanaName = text;
                                break;
                            case "E":
                                ptKyusei.Name = text;
                                break;
                            case "F":
                                int.TryParse(text, out int endDate);
                                ptKyusei.EndDate = endDate;
                                break;
                            default:
                                break;
                        }
                        numberCell++;
                    }
                    ptKyuseis.Add(ptKyusei);
                }
            }
        }
        return ptKyuseis;
    }

    public static List<RaiinInf> ReadRainInf(int randomKey)
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        int count = 1;
        string fileName = Path.Combine(rootPath, "SampleData", "PatientCommonDataSample.xlsx");
        var raiinInfs = new List<RaiinInf>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {

            var workbookPart = spreadsheetDocument.WorkbookPart;
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "RAIIN_INF").WorksheetPart?.Worksheet.Elements<SheetData>().First();
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
                                raiinInf.RaiinNo = long.MaxValue - randomKey - count;
                                break;
                            case "C":
                                var ptId = long.Parse(text, System.Globalization.CultureInfo.InvariantCulture);
                                raiinInf.PtId = ptId;
                                break;
                            case "D":
                                int.TryParse(text, out int sinDate);
                                raiinInf.SinDate = sinDate;
                                break;
                            case "E":
                                var oyaRaiinNo = long.Parse(text, System.Globalization.CultureInfo.InvariantCulture);
                                raiinInf.OyaRaiinNo = oyaRaiinNo;
                                break;
                            case "F":
                                int.TryParse(text, out int status);
                                raiinInf.Status = status;
                                break;
                            case "G":
                                int.TryParse(text, out int isYokaku);
                                raiinInf.IsYoyaku = isYokaku;
                                break;
                            case "H":
                                int.TryParse(text, out int isYokakuId);
                                raiinInf.YoyakuId = isYokakuId;
                                break;
                            case "I":
                                int.TryParse(text, out int uketukeSBT);
                                raiinInf.UketukeSbt = uketukeSBT;
                                break;
                            case "K":
                                raiinInf.UketukeTime = text;
                                break;
                            case "L":
                                int.TryParse(text, out int uketukeId);
                                raiinInf.UketukeId = uketukeId;
                                break;
                            case "M":
                                int.TryParse(text, out int uketukeNo);
                                raiinInf.UketukeNo = uketukeNo;
                                break;
                            case "N":
                                raiinInf.SinStartTime = text;
                                break;
                            case "O":
                                raiinInf.SinEndTime = text;
                                break;
                            case "P":
                                raiinInf.KaikeiTime = text;
                                break;
                            case "Q":
                                int.TryParse(text, out int kaikeId);
                                raiinInf.KaikeiId = kaikeId;
                                break;
                            case "R":
                                int.TryParse(text, out int kaId);
                                raiinInf.KaId = kaId;
                                break;
                            case "S":
                                int.TryParse(text, out int tantoId);
                                raiinInf.TantoId = tantoId;
                                break;
                            case "T":
                                int.TryParse(text, out int hokenPId);
                                raiinInf.HokenPid = hokenPId;
                                break;
                            case "U":
                                int.TryParse(text, out int syosaisinKbn);
                                raiinInf.SyosaisinKbn = syosaisinKbn;
                                break;
                            case "V":
                                int.TryParse(text, out int jikanKbn);
                                raiinInf.JikanKbn = jikanKbn;
                                break;
                            case "W":
                                int.TryParse(text, out int isDeleted);
                                raiinInf.IsDeleted = isDeleted;
                                break;
                            default:
                                break;
                        }
                    }
                    raiinInfs.Add(raiinInf);
                    count++;
                }
            }
        }

        return raiinInfs;
    }

    public static List<PtCmtInf> ReadPtCMT()
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        string fileName = Path.Combine(rootPath, "SampleData", "PatientCommonDataSample.xlsx");
        var ptCmts = new List<PtCmtInf>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
            var workbookPart = spreadsheetDocument.WorkbookPart;
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PT_CMT").WorksheetPart?.Worksheet.Elements<SheetData>().First();
            string text;
            if (sheetData != null)
            {
                foreach (var r in sheetData.Elements<Row>().Skip(1))
                {
                    var ptCmtInf = new PtCmtInf();
                    ptCmtInf.CreateId = 1;
                    ptCmtInf.CreateDate = DateTime.UtcNow;
                    ptCmtInf.UpdateId = 1;
                    ptCmtInf.UpdateDate = DateTime.UtcNow;
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
                                ptCmtInf.HpId = hpId;
                                break;
                            case "B":
                                long.TryParse(text, out long ptId);
                                ptCmtInf.PtId = ptId;
                                break;
                            case "C":
                                int.TryParse(text, out int seqNo);
                                ptCmtInf.SeqNo = seqNo;
                                break;
                            case "D":
                                ptCmtInf.Text = text;
                                break;
                            default:
                                break;
                        }
                        numberCell++;
                    }
                    ptCmts.Add(ptCmtInf);
                }
            }
        }
        return ptCmts;
    }

    public static List<PtInf> ReadPtInf()
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        string fileName = Path.Combine(rootPath, "SampleData", "PatientCommonDataSample.xlsx");
        var ptInfs = new List<PtInf>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
            var workbookPart = spreadsheetDocument.WorkbookPart;
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PT_INF").WorksheetPart?.Worksheet.Elements<SheetData>().First();
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
                            case "D":
                                int.TryParse(text, out int ptNum);
                                ptInf.PtNum = ptNum;
                                break;
                            case "E":
                                ptInf.KanaName = text;
                                break;
                            case "F":
                                ptInf.Name = text;
                                break;
                            case "G":
                                int.TryParse(text, out int sex);
                                ptInf.Sex = sex;
                                break;
                            case "H":
                                int.TryParse(text, out int birthDay);
                                ptInf.Birthday = birthDay;
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

    public static List<PtHokenCheck> ReadPtHokenCheck()
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        string fileName = Path.Combine(rootPath, "SampleData", "PatientCommonDataSample.xlsx");
        var ptHokenChecks = new List<PtHokenCheck>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
            var workbookPart = spreadsheetDocument.WorkbookPart;
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PT_HOKEN_CHECK").WorksheetPart?.Worksheet.Elements<SheetData>().First();
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
                    ptHokenCheck.CheckId = 1;
                    ptHokenCheck.CheckDate = DateTime.UtcNow;
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
                            case "E":
                                int.TryParse(text, out int seqNo);
                                ptHokenCheck.SeqNo = seqNo;
                                break;
                            case "I":
                                ptHokenCheck.CheckCmt = text;
                                break;
                            default:
                                break;
                        }
                    }
                    ptHokenChecks.Add(ptHokenCheck);
                }
            }
        }

        return ptHokenChecks;
    }

    public static List<PtByomei> ReadPtByomei(string byomeiCd = "")
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        string fileName = Path.Combine(rootPath, "SampleData", "PatientCommonDataSample.xlsx");
        var ptByomeis = new List<PtByomei>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {
            var workbookPart = spreadsheetDocument.WorkbookPart;
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "PT_BYOMEI").WorksheetPart?.Worksheet.Elements<SheetData>().First();
            string text;
            if (sheetData != null)
            {
                foreach (var r in sheetData.Elements<Row>().Skip(1))
                {
                    var ptByomei = new PtByomei();
                    ptByomei.CreateId = 1;
                    ptByomei.CreateDate = DateTime.UtcNow;
                    ptByomei.UpdateId = 1;
                    ptByomei.UpdateDate = DateTime.UtcNow;
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
                                ptByomei.HpId = hpId;
                                break;
                            case "B":
                                int.TryParse(text, out int ptId);
                                ptByomei.PtId = ptId;
                                break;
                            case "C":
                                int.TryParse(text, out int seqNo);
                                ptByomei.SeqNo = seqNo;
                                break;
                            case "D":
                                ptByomei.ByomeiCd = string.IsNullOrEmpty(byomeiCd) ? text : byomeiCd;
                                break;
                            case "AI":
                                int.TryParse(text, out int hokenPID);
                                ptByomei.HokenPid = hokenPID;
                                break;
                            default:
                                break;
                        }
                    }
                    ptByomeis.Add(ptByomei);
                }
            }
        }

        return ptByomeis;
    }

    public static List<ByomeiMst> ReadByomeiMst(string byomeiCd)
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        string fileName = Path.Combine(rootPath, "SampleData", "PatientCommonDataSample.xlsx");
        var byomeiMsts = new List<ByomeiMst>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {

            var workbookPart = spreadsheetDocument.WorkbookPart;
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "BYOMEI_MST").WorksheetPart?.Worksheet.Elements<SheetData>().First();
            string text;
            if (sheetData != null)
            {
                foreach (var r in sheetData.Elements<Row>().Skip(1))
                {
                    var byomeiMst = new ByomeiMst();
                    byomeiMst.CreateId = 1;
                    byomeiMst.CreateDate = DateTime.UtcNow;
                    byomeiMst.UpdateId = 1;
                    byomeiMst.UpdateDate = DateTime.UtcNow;
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
                                byomeiMst.HpId = hpId;
                                break;
                            case "B":
                                byomeiMst.ByomeiCd = byomeiCd;
                                break;
                            case "C":
                                byomeiMst.Byomei = text;
                                break;
                            case "D":
                                byomeiMst.Sbyomei = text;
                                break;
                            case "E":
                                byomeiMst.KanaName1 = text;
                                break;
                            case "F":
                                byomeiMst.KanaName2 = text;
                                break;
                            case "G":
                                byomeiMst.KanaName3 = text;
                                break;
                            case "H":
                                byomeiMst.KanaName4 = text;
                                break;
                            case "I":
                                byomeiMst.KanaName5 = text;
                                break;
                            case "J":
                                byomeiMst.KanaName6 = text;
                                break;
                            case "K":
                                byomeiMst.KanaName7 = text;
                                break;
                            case "L":
                                byomeiMst.IkoCd = text;
                                break;
                            case "M":
                                int.TryParse(text, out int sikkanCd);
                                byomeiMst.SikkanCd = sikkanCd;
                                break;
                            case "N":
                                int.TryParse(text, out int tandokuKinsi);
                                byomeiMst.TandokuKinsi = tandokuKinsi;
                                break;
                            case "O":
                                int.TryParse(text, out int hokenGai);
                                byomeiMst.HokenGai = hokenGai;
                                break;
                            case "P":
                                byomeiMst.ByomeiKanri = text;
                                break;
                            case "Q":
                                byomeiMst.SaitakuKbn = text;
                                break;
                            case "R":
                                byomeiMst.KoukanCd = text;
                                break;
                            case "S":
                                int.TryParse(text, out int syusaiDate);
                                byomeiMst.SyusaiDate = syusaiDate;
                                break;
                            case "T":
                                int.TryParse(text, out int updDate);
                                byomeiMst.UpdDate = updDate;
                                break;
                            case "U":
                                int.TryParse(text, out int delDate);
                                byomeiMst.DelDate = delDate;
                                break;
                            case "V":
                                int.TryParse(text, out int nanbyoCd);
                                byomeiMst.NanbyoCd = nanbyoCd;
                                break;
                            case "W":
                                byomeiMst.Icd101 = text;
                                break;
                            case "X":
                                byomeiMst.Icd102 = text;
                                break;
                            case "Y":
                                byomeiMst.Icd1012013 = text;
                                break;
                            case "Z":
                                byomeiMst.Icd1022013 = text;
                                break;
                            case "AA":
                                int.TryParse(text, out int isAdopted);
                                byomeiMst.IsAdopted = isAdopted;
                                break;
                            case "AB":
                                byomeiMst.SyusyokuKbn = text;
                                break;
                            default:
                                break;
                        }
                    }
                    byomeiMsts.Add(byomeiMst);
                }
            }
        }

        return byomeiMsts;
    }

    public static List<SystemConf> ReadSystemConf()
    {
        var rootPath = Environment.CurrentDirectory;
        rootPath = rootPath.Remove(rootPath.IndexOf("bin"));

        string fileName = Path.Combine(rootPath, "SampleData", "PatientCommonDataSample.xlsx");
        var systemConfs = new List<SystemConf>();
        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
        {

            var workbookPart = spreadsheetDocument.WorkbookPart;
            var sheetData = GetworksheetBySheetName(spreadsheetDocument, "SYSTEM_CONF").WorksheetPart?.Worksheet.Elements<SheetData>().First();
            string text;
            if (sheetData != null)
            {
                foreach (var r in sheetData.Elements<Row>().Skip(1))
                {
                    var systemConf = new SystemConf();
                    systemConf.CreateId = 1;
                    systemConf.CreateDate = DateTime.UtcNow;
                    systemConf.UpdateId = 1;
                    systemConf.UpdateDate = DateTime.UtcNow;
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
                                systemConf.HpId = hpId;
                                break;
                            case "B":
                                int.TryParse(text, out int grpCd);
                                systemConf.GrpCd = grpCd;
                                break;
                            case "C":
                                int.TryParse(text, out int grpEdaNo);
                                systemConf.GrpEdaNo = grpEdaNo;
                                break;
                            case "D":
                                int.TryParse(text, out int val);
                                systemConf.Val = val;
                                break;
                            default:
                                break;
                        }
                    }
                    systemConfs.Add(systemConf);
                }
            }
        }

        return systemConfs;
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
        StringValue relationshipId = workbookPart?.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name != null && s.Name.Equals(sheetName))?.Id ?? string.Empty;

        var worksheet = (workbookPart != null && !string.IsNullOrEmpty(relationshipId.ToString())) ? ((WorksheetPart)workbookPart.GetPartById(relationshipId?.Value ?? string.Empty)).Worksheet : new();

        return worksheet;
    }
}
