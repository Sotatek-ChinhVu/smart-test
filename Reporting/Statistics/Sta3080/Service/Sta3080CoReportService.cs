using Helper.Common;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3080.DB;
using Reporting.Statistics.Sta3080.Mapper;
using Reporting.Statistics.Sta3080.Models;

namespace Reporting.Statistics.Sta3080.Service;

public class Sta3080CoReportService : ISta3080CoReportService
{
    #region Constant
    private int maxRow = 40;

    private readonly List<PutColumn> putColumns = new()
        {
            new PutColumn("SinYm", "診療年月"),
            new PutColumn("PtNum", "患者番号"),
            new PutColumn("PtKanaName", "カナ氏名"),
            new PutColumn("PtName", "氏名"),
            new PutColumn("ItemCd", "診療行為コード"),
            new PutColumn("ItemName", "名称"),
            new PutColumn("OdrCount", "回数"),
            new PutColumn("TotalOdrCount", "患者回数計"),
            new PutColumn("SyokaiYm", "初回算定月"),
            new PutColumn("MeisaiKbn", "明細区分"),
            new PutColumn("MeisaiKbnName", "計見出し"),
            new PutColumn("PtCount1", "1回以上実施患者数"),
            new PutColumn("PtCount14", "14回以上実施患者数"),
            new PutColumn("KeikaMon", "経過月数"),
            new PutColumn("TotalKeikaMon", "経過月数合計")
        };

    private readonly List<PutColumn> totalColumns = new()
        {
            new PutColumn("TotalKbn", "集計区分"),
            new PutColumn("TotalCaption", "集計名称"),

        };
    #endregion

    #region Private properties
    private CoHpInfModel hpInf;
    private List<string> headerR;
    private List<CoSeisinDayCareInf> seisinDayCareInfs;
    private List<CoSta3080PrintData> printDatas;
    private CoFileType? coFileType;
    #endregion

    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly Dictionary<string, bool> _visibleFieldData;
    private readonly ICoSta3080Finder _finder;
    private readonly IReadRseReportFileService _readRseReportFileService;

    private int currentPage;
    private bool hasNextPage;
    private List<string> objectRseList;
    private string rowCountFieldName;
    private CoSta3080PrintConf printConf;
    private CoFileType outputFileType;
    private CountData total = new();
    private CountData subTotal = new();

    public Sta3080CoReportService(ICoSta3080Finder finder, IReadRseReportFileService readRseReportFileService)
    {
        _finder = finder;
        _readRseReportFileService = readRseReportFileService;
        hpInf = new();
        _singleFieldData = new();
        _extralData = new();
        _tableFieldData = new();
        _visibleFieldData = new();
        objectRseList = new();
        printConf = new();
        seisinDayCareInfs = new();
        printDatas = new();
        headerR = new();
        rowCountFieldName = string.Empty;
    }

    public CommonReportingRequestModel GetSta3080ReportingData(CoSta3080PrintConf printConf, int hpId, CoFileType outputFileType)
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

        return new Sta3080Mapper(_singleFieldData, _tableFieldData, _extralData, _visibleFieldData, rowCountFieldName, formFileName).GetData();
    }

    private struct CountData
    {
        public int Count;
        public int PtCount1;
        public int PtCount14;
        public int TotalKeikaMon;

        public void AddValue(int jissi1, int jissi14, int keikaMon)
        {
            Count++;
            PtCount1 += jissi1;
            PtCount14 += jissi14;
            TotalKeikaMon += keikaMon;
        }

        public void Clear()
        {
            Count = 0;
            PtCount1 = 0;
            PtCount14 = 0;
            TotalKeikaMon = 0;
        }

    }

    private void UpdateDrawForm()
    {
        int sinYm = 0;
        #region Header
        void UpdateFormHeader()
        {
            //タイトル
            SetFieldData("Title", printConf.ReportName);

            //医療機関名
            _extralData.Add("HeaderR_0_0_" + currentPage, hpInf.HpName);

            //作成日時
            _extralData.Add("HeaderR_0_1_" + currentPage, CIUtil.SDateToShowSWDate(
                CIUtil.ShowSDateToSDate(CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd")), 0, 1, 1
            ) + CIUtil.GetJapanDateTimeNow().ToString(" HH:mm") + "作成");

            //ページ数
            int totalPage = (int)Math.Ceiling((double)printDatas.Count / maxRow);
            _extralData.Add("HeaderR_0_2_" + currentPage, currentPage + " / " + totalPage);

            //期間
            SetFieldData("Range", string.Format("期間: {0}～{1}　",
                     printConf.FromYm > 0 ? CIUtil.SMonthToShowSWMonth(printConf.FromYm, 0, 0, 1) : string.Empty,
                     printConf.ToYm > 0 ? CIUtil.SMonthToShowSWMonth(printConf.ToYm, 0, 0, 1) : string.Empty));
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

            //存在しているフィールドに絞り込み
            var existsCols = putColumns.Where(p => objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();
            var totalexistsCols = totalColumns.Where(p => objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();
            foreach (var totalexistsCol in totalexistsCols)
            {
                existsCols.Add(totalexistsCol);
            }

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                Dictionary<string, CellModel> data = new();
                var printData = printDatas[ptIndex];
                string baseListName = string.Empty;

                if (printData.SinYm != null)
                {
                    sinYm = CIUtil.ShowSMonthToSMonth(printData.SinYm);
                }

                //終了年月の時のみ初回算定月と経過月数を表示
                if (printConf.ToYm == sinYm)
                {
                    SetVisibleFieldData("lblSyokaiYm", true);
                    SetVisibleFieldData("lblKeikaMon", true);
                }
                else
                {
                    SetVisibleFieldData("lblSyokaiYm", false);
                    SetVisibleFieldData("lblKeikaMon", false);
                }

                //明細と合計のデータ出力
                foreach (var colName in existsCols)
                {

                    var value = typeof(CoSta3080PrintData).GetProperty(colName).GetValue(printData);
                    AddListData(ref data, colName, value == null ? string.Empty : value.ToString() ?? string.Empty);

                    if (baseListName == string.Empty && objectRseList.Contains(colName))
                    {
                        baseListName = colName;
                    }
                }

                _tableFieldData.Add(data);
                ptIndex++;
                if (ptIndex >= printDatas.Count)
                {
                    hasNextPage = false;
                    break;
                }

                var prePrintData = printDatas[ptIndex];

                //患者毎に区切り線を引く
                if (!string.IsNullOrEmpty(prePrintData.PtNum) && printData.PtNum != prePrintData.PtNum && (rowNo + 1) % maxRow != 0)
                {
                    if (!_extralData.ContainsKey("headerLine"))
                    {
                        _extralData.Add("headerLine", "true");
                    }
                    string rowNoKey = rowNo + "_" + currentPage;
                    _extralData.Add("baseListName_" + rowNoKey, baseListName);
                    _extralData.Add("rowNo_" + rowNoKey, rowNo.ToString());
                }
            }
        }
        #endregion

        UpdateFormHeader();
        UpdateFormBody();
    }

    private bool GetData(int hpId)
    {
        void MakePrintData()
        {
            printDatas = new List<CoSta3080PrintData>();
            CoSeisinDayCareInf? preSeisinDayCareInf = null;

            headerR = new List<string>();
            int rowCount = 0;
            int pageCount = 1;
            int ptTotalOdrCnt = 0;
            int jissiCntt1 = 0;
            int jissiCnt14 = 0;
            int totalKeikaMon = 0;
            bool pageBreak = false;
            const int subTotalRowsCnt = 3;
            const int totalRowsCnt = 14;

            #region ソート順
            seisinDayCareInfs = seisinDayCareInfs?
                .OrderBy(s => s.SinYm)
                .ThenBy(s => s.PtNum)
                .ThenBy(s => s.Name)
                .ThenBy(s => s.ItemCd)
                .ToList() ?? new();
            #endregion

            #region SubMethod

            void AddBrankRecord(int rowCnt, ref int pgCnt, int addedRowCnt = 0, bool isBreak = false)
            {
                if (isBreak || (addedRowCnt > 0 && maxRow * pgCnt - rowCnt < addedRowCnt))
                {
                    //改ページ
                    for (int i = printDatas.Count; i < maxRow * pageCount; i++)
                    {
                        printDatas.Add(new CoSta3080PrintData(RowType.Brank));
                    }
                    pgCnt++;
                }
                else
                {
                    //1行空白を追加
                    printDatas.Add(new CoSta3080PrintData(RowType.Brank));
                }
            }

            void AddMeisaiRecord(CoSeisinDayCareInf tgtData, CoSeisinDayCareInf? preData, bool is1stRow, bool isPageBreak)
            {
                CoSta3080PrintData printData = new CoSta3080PrintData();

                //前の行と重複する値を省略
                if (outputFileType == CoFileType.Csv || coFileType == CoFileType.Csv || is1stRow || tgtData.PtNum != preData?.PtNum || isPageBreak)
                {
                    if (outputFileType == CoFileType.Csv || coFileType == CoFileType.Csv)
                    {
                        printData.SinYm = tgtData.SinYm.ToString();
                    }
                    else
                    {
                        printData.SinYm = CIUtil.SMonthToShowSMonth(tgtData.SinYm);
                    }
                    printData.PtNum = tgtData.PtNum.ToString();
                    printData.PtName = tgtData.Name.ToString();
                    printData.PtKanaName = tgtData.KanaName.ToString();
                }

                printData.ItemCd = tgtData.ItemCd.ToString();
                printData.ItemName = tgtData.ItemName.ToString();
                printData.OdrCount = tgtData.OdrCount;
                ptTotalOdrCnt += int.Parse(tgtData.OdrCount);

                //明細行追加
                printDatas.Add(printData);
            }

            void AddSubTotalRecord(CountData totalData, int sinYm = 0)
            {
                CoSta3080PrintData printData = new CoSta3080PrintData();

                if (outputFileType == CoFileType.Csv || coFileType == CoFileType.Csv)
                {
                    printData.SinYm = sinYm.ToString();
                    printData.MeisaiKbn = 1;
                    printData.PtCount1 = totalData.PtCount1.ToString();
                    printData.PtCount14 = totalData.PtCount14.ToString();

                    if (sinYm == printConf.ToYm)
                    {
                        printData.TotalKeikaMon = totalData.TotalKeikaMon.ToString();
                    }

                    printDatas.Add(printData);
                }
                else
                {
                    printDatas.Add(
                        new CoSta3080PrintData(RowType.Total)
                        {
                            SinYm = CIUtil.SMonthToShowSMonth(sinYm),
                            TotalKbn = "◆小計",
                            TotalCaption = string.Format("精神科デイ・ケア等を１回以上実施した患者数 : {0,5}　人",
                                totalData.PtCount1.ToString()
                                )
                        }
                    );
                    printDatas.Add(
                        new CoSta3080PrintData(RowType.Total)
                        {
                            TotalCaption = string.Format("精神科デイ・ケア等を14回以上実施した患者数 : {0,5}　人",
                                totalData.PtCount14.ToString()
                                )
                        }
                    );
                }
            }

            void AddTotalRecord(CountData totalData, CountData subTotalData)
            {
                if (outputFileType == CoFileType.Csv || coFileType == CoFileType.Csv)
                {
                    printDatas.Add(
                        new CoSta3080PrintData(RowType.Total)
                        {
                            MeisaiKbn = 2,
                            PtCount1 = totalData.PtCount1.ToString(),
                            PtCount14 = totalData.PtCount14.ToString(),
                            TotalKeikaMon = totalData.TotalKeikaMon.ToString()
                        }
                    );
                }
                else
                {
                    printDatas.Add(
                        new CoSta3080PrintData(RowType.Total)
                        {
                            TotalKbn = "◆合計",
                            TotalCaption = string.Format("月14回以上精神科デイ・ケア等を実施する患者の割合")
                        }
                    );

                    //空行を追加
                    printDatas.Add(new CoSta3080PrintData(RowType.Brank));

                    int monthdiff = MonthDifference(printConf.FromYm, printConf.ToYm);
                    double avrgPtCnt1 = 0;
                    double avrgPtCnt14 = 0;
                    double rate = 0;

                    if (totalData.PtCount1 > 0)
                    {
                        avrgPtCnt1 = 1.0 * totalData.PtCount1 / monthdiff;
                    }

                    if (totalData.PtCount14 > 0)
                    {
                        avrgPtCnt14 = 1.0 * totalData.PtCount14 / monthdiff;
                    }

                    if (avrgPtCnt1 > 0)
                    {
                        rate = 1.0 * avrgPtCnt14 / avrgPtCnt1;
                    }

                    printDatas.Add(
                        new CoSta3080PrintData(RowType.Total)
                        {
                            TotalCaption = string.Format("①　精神科デイ・ケア等を１回以上実施した患者数：　{0,5}　人　②　1月当たりの平均：　{1,5:f2}　人",
                                totalData.PtCount1,
                                avrgPtCnt1
                                )
                        }
                    );
                    printDatas.Add(
                        new CoSta3080PrintData(RowType.Total)
                        {
                            TotalCaption = string.Format("③　精神科デイ・ケア等を14回以上実施した患者数：　{0,5}　人　④　1月当たりの平均：　{1,5:f2}　人",
                                totalData.PtCount14,
                                avrgPtCnt14
                                )
                        }
                    );

                    //空行を追加
                    printDatas.Add(new CoSta3080PrintData(RowType.Brank));

                    printDatas.Add(
                        new CoSta3080PrintData(RowType.Total)
                        {
                            TotalCaption = string.Format("④／②　＝　{0:f2}", rate)
                        }
                    );

                    //空行を追加
                    printDatas.Add(new CoSta3080PrintData(RowType.Brank));

                    printDatas.Add(
                        new CoSta3080PrintData(RowType.Total)
                        {
                            TotalCaption = string.Format("精神科デイ・ケア等の平均実施期間")
                        }
                    );

                    //空行を追加
                    printDatas.Add(new CoSta3080PrintData(RowType.Brank));

                    printDatas.Add(
                       new CoSta3080PrintData(RowType.Total)
                       {
                           TotalCaption = string.Format("⑤　{0}に精神科デイ・ケア等を1回以上実施した患者数：　{1,5}　人",
                               CIUtil.SMonthToShowSMonth(printConf.ToYm),
                               //最終年月の小計から1回以上実施患者数を取得
                               subTotalData.PtCount1
                               )
                       }
                    );

                    printDatas.Add(
                       new CoSta3080PrintData(RowType.Total)
                       {
                           TotalCaption = string.Format("⑥　⑤の患者の精神科デイ・ケア等初回算定月から{0}末までの月数合計：　{1,5}　月",
                               CIUtil.SMonthToShowSMonth(printConf.ToYm),
                               totalData.TotalKeikaMon
                               )
                       }
                    );

                    rate = 0;
                    if (subTotalData.PtCount1 > 0)
                    {
                        //最終年月の小計から1回以上実施患者数を取得
                        rate = 1.0 * totalData.TotalKeikaMon / subTotalData.PtCount1;
                    }

                    printDatas.Add(
                        new CoSta3080PrintData(RowType.Total)
                        {
                            TotalCaption = string.Format("月数の平均（⑥／⑤）＝　{0:f2}", rate)
                        }
                    );

                }
            }

            void SetPtLastRow(CoSeisinDayCareInf? tgtData)
            {
                int lastIdx = printDatas.Count - 1;
                if (lastIdx >= 0)
                {

                    //終了年月の場合初回算定月と経過月数を記載
                    if (tgtData?.SinYm == printConf.ToYm)
                    {
                        if (outputFileType == CoFileType.Csv || coFileType == CoFileType.Csv)
                        {
                            printDatas[lastIdx].SyokaiYm = tgtData.SyokaiYm;
                        }
                        else
                        {
                            printDatas[lastIdx].SyokaiYm = CIUtil.SMonthToShowSMonth(int.Parse(tgtData.SyokaiYm));
                        }

                        printDatas[lastIdx].KeikaMon = MonthDifference(int.Parse(tgtData.SyokaiYm), printConf.ToYm).ToString();
                        totalKeikaMon += int.Parse(printDatas[lastIdx].KeikaMon);
                    }

                    if (outputFileType == CoFileType.Csv || coFileType == CoFileType.Csv)
                    {
                        //CSVの場合、カウントフラグをたてる
                        if (ptTotalOdrCnt >= 1)
                        {
                            printDatas[lastIdx].PtCount1 = "1";
                        }
                        else
                        {
                            printDatas[lastIdx].PtCount1 = "0";
                        }

                        if (ptTotalOdrCnt >= 14)
                        {
                            printDatas[lastIdx].PtCount14 = "1";
                        }
                        else
                        {
                            printDatas[lastIdx].PtCount14 = "0";
                        }

                        printDatas[lastIdx].TotalOdrCount = ptTotalOdrCnt.ToString();

                    }
                    else
                    {
                        printDatas[lastIdx].TotalOdrCount = "(" + ptTotalOdrCnt.ToString() + ")";
                    }

                }
            }

            /// <summary>
            /// 経過月数
            /// </summary>
            int MonthDifference(int FromYm, int ToYm)
            {
                FromYm = (FromYm) * 100 + 01;
                ToYm = (ToYm) * 100 + 01;

                DateTime dt1 = CIUtil.IntToDate(FromYm);
                DateTime dt2 = CIUtil.IntToDate(ToYm);

                int ts = dt2.Month - dt1.Month + (12 * (dt2.Year - dt1.Year)) + 1;
                return ts;
            }


            #endregion

            foreach (var seisinDayCareInf in seisinDayCareInfs)
            {

                if (preSeisinDayCareInf != null && seisinDayCareInf.PtNum != preSeisinDayCareInf.PtNum)
                {

                    SetPtLastRow(preSeisinDayCareInf);

                    //1回以上実施患者数
                    if (ptTotalOdrCnt >= 1)
                    {
                        jissiCntt1++;
                    }

                    //14回以上実施患者数
                    if (ptTotalOdrCnt >= 14)
                    {
                        jissiCnt14++;
                    }

                    subTotal.AddValue(jissiCntt1, jissiCnt14, totalKeikaMon);
                    total.AddValue(jissiCntt1, jissiCnt14, totalKeikaMon);

                    ptTotalOdrCnt = 0;
                    jissiCntt1 = 0;
                    jissiCnt14 = 0;
                    totalKeikaMon = 0;
                }

                //改ページ条件
                pageBreak = false;
                if (preSeisinDayCareInf != null)
                {
                    pageBreak = seisinDayCareInf.SinYm != preSeisinDayCareInf.SinYm;
                }

                //改ページ
                if (rowCount == maxRow || pageBreak)
                {
                    if (pageBreak)
                    {
                        //診療年月毎に小計を出力
                        AddBrankRecord(printDatas.Count, ref pageCount, subTotalRowsCnt);
                        AddSubTotalRecord(subTotal, preSeisinDayCareInf?.SinYm ?? 0);
                        subTotal.Clear();
                    }
                    AddBrankRecord(printDatas.Count, ref pageCount, 0, true);
                    rowCount = 0;
                }

                AddMeisaiRecord(seisinDayCareInf, preSeisinDayCareInf, rowCount == 0, pageBreak);
                rowCount++;

                preSeisinDayCareInf = seisinDayCareInf;

            }

            SetPtLastRow(preSeisinDayCareInf);

            //1回以上実施患者数
            if (ptTotalOdrCnt >= 1)
            {
                jissiCntt1++;
            }

            //14回以上実施患者数
            if (ptTotalOdrCnt >= 14)
            {
                jissiCnt14++;
            }

            //最後の患者をカウント
            subTotal.AddValue(jissiCntt1, jissiCnt14, totalKeikaMon);
            total.AddValue(jissiCntt1, jissiCnt14, totalKeikaMon);

            //小計
            AddBrankRecord(printDatas.Count, ref pageCount, subTotalRowsCnt);
            AddSubTotalRecord(subTotal, preSeisinDayCareInf?.SinYm ?? 0);

            if (preSeisinDayCareInf?.SinYm != printConf.ToYm)
            {
                subTotal.Clear();
            }

            //合計
            AddBrankRecord(printDatas.Count, ref pageCount, totalRowsCnt);
            AddTotalRecord(total, subTotal);

        }

        hpInf = _finder.GetHpInf(hpId, CIUtil.DateTimeToInt(DateTime.Today));

        seisinDayCareInfs = _finder.GetSeisinDayCareInfs(hpId, printConf);
        if ((seisinDayCareInfs?.Count ?? 0) == 0) { return false; }

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
        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3080, fileName, new());
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        objectRseList = javaOutputData.objectNames;
    }

    private void SetVisibleFieldData(string fieldName, bool status)
    {
        if (!_visibleFieldData.ContainsKey(fieldName))
        {
            _visibleFieldData.Add(fieldName, status);
        }
        else if (_visibleFieldData.ContainsKey(fieldName))
        {
            _visibleFieldData[fieldName] = status;
        }
    }

    private void GetRowCount(string fileName)
    {
        rowCountFieldName = putColumns.Find(p => objectRseList.Contains(p.ColName)).ColName;
        List<ObjectCalculate> fieldInputList = new()
        {
            new ObjectCalculate(rowCountFieldName, (int)CalculateTypeEnum.GetListRowCount)
        };

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3080, fileName, fieldInputList);
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? maxRow;
    }

    public CommonExcelReportingModel ExportCsv(CoSta3080PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType)
    {
        this.printConf = printConf;
        string fileName = menuName + "_" + monthFrom + "_" + monthTo;
        this.coFileType = coFileType;
        List<string> retDatas = new List<string>();
        if (!GetData(hpId)) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

        var csvDatas = printDatas.Where(p => p.RowType != RowType.Brank).ToList();
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

        string RecordData(CoSta3080PrintData csvData)
        {
            List<string> colDatas = new List<string>();

            foreach (var column in putColumns)
            {

                var value = typeof(CoSta3080PrintData).GetProperty(column.CsvColName).GetValue(csvData);
                colDatas.Add("\"" + (value == null ? "" : value.ToString()) + "\"");
            }

            return string.Join(",", colDatas);
        }

        return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);
    }
}
