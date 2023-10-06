using Helper.Common;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3030.DB;
using Reporting.Statistics.Sta3030.Mapper;
using Reporting.Statistics.Sta3030.Models;

namespace Reporting.Statistics.Sta3030.Service;

public class Sta3030CoReportService : ISta3030CoReportService
{
    #region Constant
    private int maxRow = 40;

    private readonly List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("PtNum", "患者番号"),
            new PutColumn("PtName", "氏名"),
            new PutColumn("PtKanaName", "カナ氏名"),
            new PutColumn("BirthdayWS", "生年月日(和暦西暦)"),
            new PutColumn("Age", "年齢"),
            new PutColumn("Sex", "性別"),
            new PutColumn("SexCd", "性別コード"),
            new PutColumn("LastVisitDateWS", "最終来院日(和暦西暦)"),
            new PutColumn("Byomei", "病名"),
            new PutColumn("ByomeiCd", "病名コード"),
            new PutColumn("ByomeiComp", "病名（合成）"),
            new PutColumn("SyubyoKbn", "主"),
            new PutColumn("StartDateWS", "開始日(和暦西暦)"),
            new PutColumn("TenkiKbn", "転帰区分"),
            new PutColumn("Tenki", "転帰"),
            new PutColumn("TenkiDateWS", "転帰日(和暦西暦)"),
            new PutColumn("TogetuByomei", "当月病名"),
            new PutColumn("SikkanKbn", "慢性疾患区分"),
            new PutColumn("SikkanKbnCd", "慢性疾患区分コード"),
            new PutColumn("Nanbyo", "難病外来"),
            new PutColumn("NanbyoCd", "難病外来コード"),
            new PutColumn("HosokuCmt", "補足コメント"),
            new PutColumn("Hoken", "保険"),
            new PutColumn("HokenPid", "保険ID"),
            new PutColumn("SettogoCd", "接頭語コード"),
            new PutColumn("SetubigoCd", "接尾語コード"),
            new PutColumn("Birthday", "生年月日(西暦)", false, "BirthdayI"),
            new PutColumn("BirthdayW", "生年月日(和暦)"),
            new PutColumn("LastVisitDate", "最終来院日(西暦)", false, "LastVisitDateI"),
            new PutColumn("LastVisitDateW", "最終来院日(和暦)"),
            new PutColumn("StartDate", "開始日(西暦)", false, "StartDateI"),
            new PutColumn("StartDateW", "開始日(和暦)"),
            new PutColumn("TenkiDate", "転帰日(西暦)", false, "TenkiDateI"),
            new PutColumn("TenkiDateW", "転帰日(和暦)")
        };

    #endregion

    #region Private properties
    private CoHpInfModel hpInf;

    private List<CoPtByomeiModel> ptByomeiInfs;
    private List<CoSta3030PrintData> printDatas;
    #endregion
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly ICoSta3030Finder _finder;
    private readonly IReadRseReportFileService _readRseReportFileService;

    private int currentPage;
    private bool hasNextPage;
    private List<string> objectRseList;
    private string rowCountFieldName;
    private CoSta3030PrintConf printConf;
    private CoFileType outputFileType;
    private CoFileType? coFileType;

    public Sta3030CoReportService(ICoSta3030Finder finder, IReadRseReportFileService readRseReportFileService)
    {
        _finder = finder;
        _readRseReportFileService = readRseReportFileService;
        hpInf = new();
        _singleFieldData = new();
        _extralData = new();
        _tableFieldData = new();
        objectRseList = new();
        printConf = new();
        printDatas = new();
        rowCountFieldName = string.Empty;
    }

    public CommonReportingRequestModel GetSta3030ReportingData(CoSta3030PrintConf printConf, int hpId, CoFileType outputFileType)
    {
        this.printConf = printConf;
        this.outputFileType = outputFileType;
        string formFileName = printConf.FormFileName;

        // get data to print
        GetFieldNameList(formFileName);
        GetRowCount(formFileName);

        if (GetData(hpId))
        {
            hasNextPage = true;
            currentPage = 1;

            //印刷
            while (hasNextPage)
            {
                UpdateDrawForm();
                currentPage++;
            }
        }

        return new Sta3030Mapper(_singleFieldData, _tableFieldData, _extralData, rowCountFieldName, formFileName).GetData();
    }

    private void UpdateDrawForm()
    {
        #region Header
        void UpdateFormHeader()
        {
            //タイトル
            SetFieldData("Title", printConf.ReportName);

            //医療機関名
            _extralData.Add("HeaderR_0_0_" + currentPage, hpInf.HpName);

            //作成日時
            _extralData.Add("HeaderR_0_1_" + currentPage, CIUtil.SDateToShowSWDate(
                CIUtil.ShowSDateToSDate(CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd")), 0, 1
            ) + CIUtil.GetJapanDateTimeNow().ToString(" HH:mm") + "作成");

            //ページ数
            int totalPage = (int)Math.Ceiling((double)printDatas.Count / maxRow);
            _extralData.Add("HeaderR_0_2_" + currentPage, currentPage + " / " + totalPage);

            //開始日
            if (printConf.StartDateFrom > 0 || printConf.StartDateTo > 0)
            {
                SetFieldData("StartRange", string.Format("開始日: {0}～{1}　",
                    printConf.StartDateFrom > 0 ? CIUtil.SDateToShowSWDate(printConf.StartDateFrom, 0, 1) : string.Empty,
                    printConf.StartDateTo > 0 ? CIUtil.SDateToShowSWDate(printConf.StartDateTo, 0, 1) : string.Empty));
            }


            //転帰日
            if (printConf.TenkiDateFrom > 0 || printConf.TenkiDateTo > 0)
            {
                SetFieldData("TenkiRange", string.Format("転帰日: {0}～{1}　",
                    printConf.TenkiDateFrom > 0 ? CIUtil.SDateToShowSWDate(printConf.TenkiDateFrom, 0, 1) : string.Empty,
                    printConf.TenkiDateTo > 0 ? CIUtil.SDateToShowSWDate(printConf.TenkiDateTo, 0, 1) : string.Empty));
            }

            //有効期間
            if (printConf.EnableRangeFrom > 0 || printConf.EnableRangeTo > 0)
            {
                SetFieldData("EnableRange", string.Format("期間: {0}～{1}　",
                    printConf.EnableRangeFrom > 0 ? CIUtil.SDateToShowSWDate(printConf.EnableRangeFrom, 0, 1) : string.Empty,
                    printConf.EnableRangeTo > 0 ? CIUtil.SDateToShowSWDate(printConf.EnableRangeTo, 0, 1) : string.Empty));
            }
        }
        #endregion

        #region Body
        void UpdateFormBody()
        {
            if (printDatas == null || printDatas.Count == 0)
            {
                hasNextPage = false;
                return;
            }

            int ptIndex = (currentPage - 1) * maxRow;
            int lineCount = 0;
            int sptMaxRow = 5;

            //存在しているフィールドに絞り込み
            var existsCols = putColumns.Where(p => objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                Dictionary<string, CellModel> data = new();
                var printData = printDatas[ptIndex];
                string baseListName = string.Empty;

                //明細のデータ出力
                if (printData.RowType != RowType.Brank)
                {
                    foreach (var colName in existsCols)
                    {
                        var value = typeof(CoSta3030PrintData).GetProperty(colName).GetValue(printData);
                        AddListData(ref data, colName, value == null ? string.Empty : value.ToString() ?? string.Empty);

                        if (baseListName == string.Empty && objectRseList.Contains(colName))
                        {
                            baseListName = colName;
                        }
                    }
                }

                //5項目毎に区切り線を引く
                lineCount = printData.RowType != RowType.Brank && printData.RowType != RowType.Total ? lineCount + 1 : lineCount;

                if (lineCount == sptMaxRow && rowNo != maxRow - 1)
                {
                    lineCount = 0;

                    if (!_extralData.ContainsKey("headerLine"))
                    {
                        _extralData.Add("headerLine", "true");
                    }
                    string rowNoKey = rowNo + "_" + currentPage;
                    _extralData.Add("baseListName_" + rowNoKey, baseListName);
                    _extralData.Add("rowNo_" + rowNoKey, rowNo.ToString());
                }

                _tableFieldData.Add(data);
                ptIndex++;
                if (ptIndex >= printDatas.Count)
                {
                    hasNextPage = false;
                    break;
                }
            }
        }
        #endregion

        UpdateFormHeader();
        UpdateFormBody();
    }

    private bool GetData(int hpId)
    {
        /// <summary>
        /// 明細データ追加
        /// </summary>
        /// <param name="tgtData">病名情報</param>
        /// <param name="omitMode">1:患者情報を省略 2:病名を省略</param>
        /// <returns></returns>
        void AddMeisaiRecord(CoPtByomeiModel tgtData, int omitMode)
        {
            CoSta3030PrintData printData = new CoSta3030PrintData();
            if (omitMode != 1)
            {
                //患者情報
                printData.PtNum = tgtData.PtNum.ToString();
                printData.PtName = tgtData.PtName;
                printData.PtKanaName = tgtData.PtKanaName;
                printData.BirthdayI = tgtData.Birthday;
                printData.Age = CIUtil.SDateToAge(tgtData.Birthday, CIUtil.DateTimeToInt(CIUtil.GetJapanDateTimeNow()));
                printData.SexCd = tgtData.SexCd;
                printData.LastVisitDateI = tgtData.LastVisitDate;
            }
            if (omitMode != 2)
            {
                printData.Byomei = tgtData.Byomei;
            }
            printData.ByomeiCd = tgtData.ByomeiCd;
            printData.SyubyoKbn = tgtData.SyubyoKbn;
            printData.StartDateI = tgtData.StartDate;
            printData.TenkiKbn = tgtData.TenkiKbn;
            printData.TenkiDateI = tgtData.TenkiDate;
            printData.TogetuByomei = tgtData.TogetuByomei;
            printData.SikkanKbnCd = tgtData.SikkanKbnCd;
            printData.NanbyoCd = tgtData.NanbyoCd;
            printData.HosokuCmt = tgtData.HosokuCmt;
            printData.Hoken = tgtData.Hoken;
            printData.HokenPid = tgtData.HokenPid.ToString();
            printData.SettogoCd = tgtData.SettogoCd;
            printData.SetubigoCd = tgtData.SetubigoCd;
            //明細行追加
            printDatas.Add(printData);
        }

        /*void MakePrintData()
        {
            printDatas = new List<CoSta3030PrintData>();

            int rowCount = 0;
            int pgCount = 1;
            long prePtNum = -1;
            string preByomei = string.Empty;
            bool pgBreak = false;

            //改ページ条件
            bool ptNumPgBreak = new int[] { printConf.PageBreak1, printConf.PageBreak2, printConf.PageBreak3 }.Contains(1);

            #region ソート順
            ptByomeiInfs = ptByomeiInfs?
                .OrderBy(s =>
                   ptNumPgBreak ? s.PtNum.ToString().PadLeft(10, '0') : "0")
                .ThenBy(s =>
                   printConf.SortOpt1 == 1 ? "0" :
                   printConf.SortOrder1 == 1 ? s.PtNum.ToString().PadLeft(10, '0') :
                   printConf.SortOrder1 == 2 ? s.Byomei :
                   printConf.SortOrder1 == 3 ? s.StartDate.ToString() :
                   printConf.SortOrder1 == 4 ? s.PtKanaName :
                   printConf.SortOrder1 == 5 ? s.TenkiKbn.ToString() :
                   printConf.SortOrder1 == 6 ? (s.TenkiDate == 0 ? 99999999 : s.TenkiDate).ToString() : "0")
                .ThenByDescending(s =>
                   printConf.SortOpt1 == 0 ? "0" :
                   printConf.SortOrder1 == 1 ? s.PtNum.ToString().PadLeft(10, '0') :
                   printConf.SortOrder1 == 2 ? s.Byomei :
                   printConf.SortOrder1 == 3 ? s.StartDate.ToString() :
                   printConf.SortOrder1 == 4 ? s.PtKanaName :
                   printConf.SortOrder1 == 5 ? s.TenkiKbn.ToString() :
                   printConf.SortOrder1 == 6 ? (s.TenkiDate == 0 ? 99999999 : s.TenkiDate).ToString() : "0")
                .ThenBy(s =>
                   printConf.SortOpt2 == 1 ? "0" :
                   printConf.SortOrder2 == 1 ? s.PtNum.ToString().PadLeft(10, '0') :
                   printConf.SortOrder2 == 2 ? s.Byomei :
                   printConf.SortOrder2 == 3 ? s.StartDate.ToString() :
                   printConf.SortOrder2 == 4 ? s.PtKanaName :
                   printConf.SortOrder2 == 5 ? s.TenkiKbn.ToString() :
                   printConf.SortOrder2 == 6 ? (s.TenkiDate == 0 ? 99999999 : s.TenkiDate).ToString() : "0")
                .ThenByDescending(s =>
                   printConf.SortOpt2 == 0 ? "0" :
                   printConf.SortOrder2 == 1 ? s.PtNum.ToString().PadLeft(10, '0') :
                   printConf.SortOrder2 == 2 ? s.Byomei :
                   printConf.SortOrder2 == 3 ? s.StartDate.ToString() :
                   printConf.SortOrder2 == 4 ? s.PtKanaName :
                   printConf.SortOrder2 == 5 ? s.TenkiKbn.ToString() :
                   printConf.SortOrder2 == 6 ? (s.TenkiDate == 0 ? 99999999 : s.TenkiDate).ToString() : "0")
                .ThenBy(s =>
                   printConf.SortOpt3 == 1 ? "0" :
                   printConf.SortOrder3 == 1 ? s.PtNum.ToString().PadLeft(10, '0') :
                   printConf.SortOrder3 == 2 ? s.Byomei :
                   printConf.SortOrder3 == 3 ? s.StartDate.ToString() :
                   printConf.SortOrder3 == 4 ? s.PtKanaName :
                   printConf.SortOrder3 == 5 ? s.TenkiKbn.ToString() :
                   printConf.SortOrder3 == 6 ? (s.TenkiDate == 0 ? 99999999 : s.TenkiDate).ToString() : "0")
                .ThenByDescending(s =>
                   printConf.SortOpt3 == 0 ? "0" :
                   printConf.SortOrder3 == 1 ? s.PtNum.ToString().PadLeft(10, '0') :
                   printConf.SortOrder3 == 2 ? s.Byomei :
                   printConf.SortOrder3 == 3 ? s.StartDate.ToString() :
                   printConf.SortOrder3 == 4 ? s.PtKanaName :
                   printConf.SortOrder3 == 5 ? s.TenkiKbn.ToString() :
                   printConf.SortOrder3 == 6 ? (s.TenkiDate == 0 ? 99999999 : s.TenkiDate).ToString() : "0")
                .ToList() ?? new();
            #endregion

            foreach (var ptByomeiInf in ptByomeiInfs)
            {

                //改ページ条件
                pgBreak = false;
                if (ptNumPgBreak && ptByomeiInf.PtNum != prePtNum)
                {
                    pgBreak = rowCount > 0;
                }

                //改ページ
                if (rowCount == maxRow || pgBreak)
                {
                    for (int i = printDatas.Count; i < maxRow * pgCount; i++)
                    {
                        //空行を追加
                        printDatas.Add(new CoSta3030PrintData(RowType.Brank));
                    }
                    pgCount++;
                    rowCount = 0;
                }

                //前の行と同じ情報を省略
                int omit = 0;
                if ((outputFileType != CoFileType.Csv || coFileType != CoFileType.Csv) && (rowCount > 0))
                {
                    omit = (ptNumPgBreak || new int[] { 1, 4 }.Contains(printConf.SortOrder1)) && (ptByomeiInf.PtNum == prePtNum) ? 1
                        : (!ptNumPgBreak && printConf.SortOrder1 == 2) && (ptByomeiInf.Byomei == preByomei) ? 2
                        : 0;
                }

                AddMeisaiRecord(ptByomeiInf, omit);
                rowCount++;

                prePtNum = ptByomeiInf.PtNum;
                preByomei = ptByomeiInf.Byomei;
            }
        }*/

        void MakePrintData()
        {
            printDatas = new List<CoSta3030PrintData>();

            int rowCount = 0;
            int pgCount = 1;
            long prePtNum = -1;
            string preByomei = "";
            bool pgBreak = false;

            //改ページ条件
            bool ptNumPgBreak = new int[] { printConf.PageBreak1, printConf.PageBreak2, printConf.PageBreak3 }.Contains(1);

            #region ソート順
            ptByomeiInfs = ptByomeiInfs
                .OrderBy(s =>
                   ptNumPgBreak ? s.PtNum.ToString().PadLeft(10, '0') : "0")
                .ThenBy(s =>
                   printConf.SortOpt1 == 1 ? "0" :
                   printConf.SortOrder1 == 1 ? s.PtNum.ToString().PadLeft(10, '0') :
                   printConf.SortOrder1 == 2 ? s.Byomei :
                   printConf.SortOrder1 == 3 ? s.StartDate.ToString() :
                   printConf.SortOrder1 == 4 ? s.PtKanaName :
                   printConf.SortOrder1 == 5 ? s.TenkiKbn.ToString() :
                   printConf.SortOrder1 == 6 ? (s.TenkiDate == 0 ? 99999999 : s.TenkiDate).ToString() : "0")
                .ThenByDescending(s =>
                   printConf.SortOpt1 == 0 ? "0" :
                   printConf.SortOrder1 == 1 ? s.PtNum.ToString().PadLeft(10, '0') :
                   printConf.SortOrder1 == 2 ? s.Byomei :
                   printConf.SortOrder1 == 3 ? s.StartDate.ToString() :
                   printConf.SortOrder1 == 4 ? s.PtKanaName :
                   printConf.SortOrder1 == 5 ? s.TenkiKbn.ToString() :
                   printConf.SortOrder1 == 6 ? (s.TenkiDate == 0 ? 99999999 : s.TenkiDate).ToString() : "0")
                .ThenBy(s =>
                   printConf.SortOpt2 == 1 ? "0" :
                   printConf.SortOrder2 == 1 ? s.PtNum.ToString().PadLeft(10, '0') :
                   printConf.SortOrder2 == 2 ? s.Byomei :
                   printConf.SortOrder2 == 3 ? s.StartDate.ToString() :
                   printConf.SortOrder2 == 4 ? s.PtKanaName :
                   printConf.SortOrder2 == 5 ? s.TenkiKbn.ToString() :
                   printConf.SortOrder2 == 6 ? (s.TenkiDate == 0 ? 99999999 : s.TenkiDate).ToString() : "0")
                .ThenByDescending(s =>
                   printConf.SortOpt2 == 0 ? "0" :
                   printConf.SortOrder2 == 1 ? s.PtNum.ToString().PadLeft(10, '0') :
                   printConf.SortOrder2 == 2 ? s.Byomei :
                   printConf.SortOrder2 == 3 ? s.StartDate.ToString() :
                   printConf.SortOrder2 == 4 ? s.PtKanaName :
                   printConf.SortOrder2 == 5 ? s.TenkiKbn.ToString() :
                   printConf.SortOrder2 == 6 ? (s.TenkiDate == 0 ? 99999999 : s.TenkiDate).ToString() : "0")
                .ThenBy(s =>
                   printConf.SortOpt3 == 1 ? "0" :
                   printConf.SortOrder3 == 1 ? s.PtNum.ToString().PadLeft(10, '0') :
                   printConf.SortOrder3 == 2 ? s.Byomei :
                   printConf.SortOrder3 == 3 ? s.StartDate.ToString() :
                   printConf.SortOrder3 == 4 ? s.PtKanaName :
                   printConf.SortOrder3 == 5 ? s.TenkiKbn.ToString() :
                   printConf.SortOrder3 == 6 ? (s.TenkiDate == 0 ? 99999999 : s.TenkiDate).ToString() : "0")
                .ThenByDescending(s =>
                   printConf.SortOpt3 == 0 ? "0" :
                   printConf.SortOrder3 == 1 ? s.PtNum.ToString().PadLeft(10, '0') :
                   printConf.SortOrder3 == 2 ? s.Byomei :
                   printConf.SortOrder3 == 3 ? s.StartDate.ToString() :
                   printConf.SortOrder3 == 4 ? s.PtKanaName :
                   printConf.SortOrder3 == 5 ? s.TenkiKbn.ToString() :
                   printConf.SortOrder3 == 6 ? (s.TenkiDate == 0 ? 99999999 : s.TenkiDate).ToString() : "0")
                .ToList();
            #endregion

            foreach (var ptByomeiInf in ptByomeiInfs)
            {

                //改ページ条件
                pgBreak = false;
                if (ptNumPgBreak && ptByomeiInf.PtNum != prePtNum)
                {
                    pgBreak = rowCount > 0;
                }

                //改ページ
                if (rowCount == maxRow || pgBreak)
                {
                    for (int i = printDatas.Count; i < maxRow * pgCount; i++)
                    {
                        //空行を追加
                        printDatas.Add(new CoSta3030PrintData(RowType.Brank));
                    }
                    pgCount++;
                    rowCount = 0;
                }

                //前の行と同じ情報を省略
                int omit = 0;
                if ((outputFileType != CoFileType.Csv && coFileType != CoFileType.Csv) && (rowCount > 0))
                {
                    omit = (ptNumPgBreak || new int[] { 1, 4 }.Contains(printConf.SortOrder1)) && (ptByomeiInf.PtNum == prePtNum) ? 1
                        : (!ptNumPgBreak && printConf.SortOrder1 == 2) && (ptByomeiInf.Byomei == preByomei) ? 2
                        : 0;
                }

                AddMeisaiRecord(ptByomeiInf, omit);
                rowCount++;

                prePtNum = ptByomeiInf.PtNum;
                preByomei = ptByomeiInf.Byomei;
            }

        }

        hpInf = _finder.GetHpInf(hpId, CIUtil.DateTimeToInt(DateTime.Today));

        ptByomeiInfs = _finder.GetPtByomeiInfs(hpId, printConf);
        if ((ptByomeiInfs?.Count ?? 0) == 0)
        {
            return false;
        }

        //印刷用データの作成
        MakePrintData();

        return printDatas.Count > 0;
    }

    private void SetFieldData(string field, string value)
    {
        if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
        {
            _singleFieldData.Add(field, value);
        }
    }

    private void AddListData(ref Dictionary<string, CellModel> dictionary, string field, string value)
    {
        if (!string.IsNullOrEmpty(field) && !dictionary.ContainsKey(field))
        {
            dictionary.Add(field, new CellModel(value));
        }
    }

    private void GetFieldNameList(string fileName)
    {
        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3030, fileName, new());
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        objectRseList = javaOutputData.objectNames;
    }

    private void GetRowCount(string fileName)
    {
        rowCountFieldName = putColumns.Find(p => objectRseList.Contains(p.ColName)).ColName;
        List<ObjectCalculate> fieldInputList = new()
        {
            new ObjectCalculate(rowCountFieldName, (int)CalculateTypeEnum.GetListRowCount)
        };

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3030, fileName, fieldInputList);
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? maxRow;
    }

    public CommonExcelReportingModel ExportCsv(CoSta3030PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType)
    {
        this.printConf = printConf;
        this.coFileType = coFileType;
        string fileName = printConf.ReportName + "_" + printConf.EnableRangeTo;
        List<string> retDatas = new List<string>();

        if (!GetData(hpId)) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

        var csvDatas = printDatas.Where(p => p.RowType == RowType.Data).ToList();
        if (csvDatas.Count == 0) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

        //出力フィールド
        List<string> wrkTitles = putColumns.Select(p => p.JpName).ToList();
        List<string> wrkColumns = putColumns.Select(p => p.CsvColName).ToList();

        //タイトル行
        retDatas.Add("\"" + string.Join("\",\"", wrkTitles) + "\"");
        if (isPutColName)
        {
            retDatas.Add("\"" + string.Join("\",\"", wrkColumns) + "\"");
        }

        //データ
        int totalRow = csvDatas.Count;
        int rowOutputed = 0;
        foreach (var csvData in csvDatas)
        {
            retDatas.Add(RecordData(csvData));
            rowOutputed++;
        }

        string RecordData(CoSta3030PrintData csvData)
        {
            List<string> colDatas = new List<string>();

            foreach (var column in putColumns)
            {
                var value = typeof(CoSta3030PrintData).GetProperty(column.CsvColName).GetValue(csvData);
                colDatas.Add("\"" + (value == null ? "" : value.ToString()) + "\"");
            }

            return string.Join(",", colDatas);
        }

        return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);
    }
}
