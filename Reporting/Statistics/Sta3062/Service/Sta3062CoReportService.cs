using Helper.Common;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3062.DB;
using Reporting.Statistics.Sta3062.Mapper;
using Reporting.Statistics.Sta3062.Models;
using System.Drawing.Printing;

namespace Reporting.Statistics.Sta3062.Service;

public class Sta3062CoReportService : ISta3062CoReportService
{
    #region Constant
    private const int maxCol = 31;
    private List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("ReportKbn", "診療日", false),
            new PutColumn("SinYmFmt", "診療年月", false, "SinYm"),
            new PutColumn("KaId", "診療科ID", false),
            new PutColumn("KaSname", "診療科", false),
            new PutColumn("TantoId", "担当医ID", false),
            new PutColumn("TantoSname", "担当医", false),
            new PutColumn("TotalTensu", "合計点数"),
            new PutColumn("TotalCount", "来院数"),  //来院数(合計件数)
            new PutColumn("PtCount", "実人数"),
            new PutColumn("AvgTensu", "来院単価(点)"),  //平均点数
            new PutColumn("AvgTanka", "来院単価(円)"),
            new PutColumn("TotalTanka", "合計単価"),
            new PutColumn("KouiTensu0", "初診(点)"),
            new PutColumn("KouiTensu1", "再診(点)"),
            new PutColumn("KouiTensu2", "医学管理(点)"),
            new PutColumn("KouiTensu3", "在宅(点)"),
            new PutColumn("KouiTensu4", "薬剤器材(点)"),
            new PutColumn("KouiTensu5", "投薬(点)"),
            new PutColumn("KouiTensu6", "注射(点)"),
            new PutColumn("KouiTensu7", "処置(点)"),
            new PutColumn("KouiTensu8", "手術(点)"),
            new PutColumn("KouiTensu9", "検査(点)"),
            new PutColumn("KouiTensu10", "画像(点)"),
            new PutColumn("KouiTensu11", "その他(点)"),
            new PutColumn("KouiTensu12", "自費(円)"),
            new PutColumn("KouiCount0", "初診(件数)"),
            new PutColumn("KouiCount1", "再診(件数)"),
            new PutColumn("KouiCount2", "医学管理(件数)"),
            new PutColumn("KouiCount3", "在宅(件数)"),
            new PutColumn("KouiCount4", "薬剤器材(件数)"),
            new PutColumn("KouiCount5", "投薬(件数)"),
            new PutColumn("KouiCount6", "注射(件数)"),
            new PutColumn("KouiCount7", "処置(件数)"),
            new PutColumn("KouiCount8", "手術(件数)"),
            new PutColumn("KouiCount9", "検査(件数)"),
            new PutColumn("KouiCount10", "画像(件数)"),
            new PutColumn("KouiCount11", "その他(件数)"),
            new PutColumn("KouiCount12", "自費(件数)"),
            new PutColumn("KouiTanka0", "初診(単価)"),
            new PutColumn("KouiTanka1", "再診(単価)"),
            new PutColumn("KouiTanka2", "医学管理(単価)"),
            new PutColumn("KouiTanka3", "在宅(単価)"),
            new PutColumn("KouiTanka4", "薬剤器材(単価)"),
            new PutColumn("KouiTanka5", "投薬(単価)"),
            new PutColumn("KouiTanka6", "注射(単価)"),
            new PutColumn("KouiTanka7", "処置(単価)"),
            new PutColumn("KouiTanka8", "手術(単価)"),
            new PutColumn("KouiTanka9", "検査(単価)"),
            new PutColumn("KouiTanka10", "画像(単価)"),
            new PutColumn("KouiTanka11", "その他(単価)"),
        };
    #endregion

    private List<CoSta3062PrintData> printDatas;
    private List<string> headerL1;
    private List<string> headerL2;
    private List<CoKouiTensuModel> kouiTensus;
    private CoHpInfModel hpInf;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly ICoSta3062Finder _finder;
    private readonly IReadRseReportFileService _readRseReportFileService;

    private CoSta3062PrintConf printConf;
    private CoFileType outputFileType;
    private CoFileType? coFileType;
    private int _currentPage;
    private List<string> _objectRseList;
    private bool _hasNextPage;

    public Sta3062CoReportService(ICoSta3062Finder finder, IReadRseReportFileService readRseReportFileService)
    {
        _finder = finder;
        _readRseReportFileService = readRseReportFileService;
        hpInf = new();
        _singleFieldData = new();
        _extralData = new();
        _listTextData = new();
        _objectRseList = new();
        printConf = new();
        printDatas = new();
        headerL1 = new();
        headerL2 = new();
        kouiTensus = new();
    }

    public CommonReportingRequestModel GetSta3062ReportingData(CoSta3062PrintConf printConf, int hpId)
    {
        try
        {
            this.printConf = printConf;
            string formFileName = printConf.FormFileName;

            // get data to print
            GetFieldNameList(formFileName);
            if (GetData(hpId))
            {
                _hasNextPage = true;
                _currentPage = 1;

                //印刷
                while (_hasNextPage)
                {
                    UpdateDrawForm();
                    _currentPage++;
                }
            }

            _extralData.Add("totalPage", (_currentPage - 1).ToString());
            var result = new Sta3062Mapper(_singleFieldData, _listTextData, _extralData, formFileName).GetData();
            return result;
        }
        finally
        {
            _finder.ReleaseResource();
        }
    }

    #region OutPut method
    //private CoPrintExitCode outPutFile(string outputDirectory, string outputFileName)
    //{
    //    #region SubMethod
    //    string RecordData(CoSta3062PrintData csvData)
    //    {
    //        List<string> colDatas = new List<string>();

    //        foreach (var column in putCurColumns)
    //        {
    //            var value = typeof(CoSta3062PrintData).GetProperty(column.CsvColName).GetValue(csvData);
    //            if (csvData.RowType == RowType.Total && !column.IsTotal)
    //            {
    //                value = string.Empty;
    //            }
    //            else if (value is RowType)
    //            {
    //                value = (int)value;
    //            }
    //            colDatas.Add("\"" + (value == null ? string.Empty : value.ToString()) + "\"");
    //        }

    //        return string.Join(",", colDatas);
    //    }
    //    #endregion

    //    try
    //    {
    //        if (!GetData(hpId)) return CoPrintExitCode.EndNoData;

    //        putCurColumns.AddRange(putColumns);

    //        var csvDatas = printDatas.Where(p => p.RowType == RowType.Data).ToList();
    //        if (csvDatas.Count == 0) return CoPrintExitCode.EndNoData;

    //        List<string> retDatas = new List<string>();

    //        //出力フィールド
    //        List<string> wrkTitles = putCurColumns.Select(p => p.JpName).ToList();
    //        List<string> wrkColumns = putCurColumns.Select(p => p.CsvColName).ToList();

    //        //タイトル行
    //        List<string> wrkCols = new List<string>();

    //        foreach (var wrkTitle in wrkTitles)
    //        {
    //            wrkCols.Add("\"" + wrkTitle + "\"");
    //        }
    //        retDatas.Add(string.Join(",", wrkCols));

    //        wrkCols.Clear();
    //        if (isPutColName)
    //        {
    //            foreach (var wrkColumn in wrkColumns)
    //            {
    //                wrkCols.Add("\"" + wrkColumn + "\"");
    //            }
    //            retDatas.Add(string.Join(",", wrkCols));
    //        }

    //        //データ
    //        int totalRow = csvDatas.Count;
    //        int rowOutputed = 0;
    //        foreach (var csvData in csvDatas)
    //        {
    //            retDatas.Add(RecordData(csvData));
    //            rowOutputed++;
    //            if (_backgroundWorker != null)
    //            {
    //                int pecentProcess = rowOutputed * 100 / totalRow;
    //                _backgroundWorker.ReportProgress(pecentProcess);
    //            }
    //        }

    //        var encoding = Encoding.UTF8;
    //        var path = Path.Combine(outputDirectory, outputFileName);
    //        path = Path.HasExtension(path) ? path : Path.ChangeExtension(path, "csv");

    //        //ファイル保存
    //        File.WriteAllLines(path, retDatas, encoding);

    //        return CoPrintExitCode.EndSuccess;
    //    }
    //    catch (Exception ex)
    //    {
    //        return CoPrintExitCode.EndError;
    //    }
    //}
    #endregion

    #region Private function

    private void UpdateDrawForm()
    {
        if (printDatas.Count == 0)
        {
            _hasNextPage = false;
            return;
        }

        #region SubMethod

        List<ListTextObject> listDataPerPage = new();
        #region Header
        void UpdateFormHeader()
        {
            //タイトル
            SetFieldData("Title", printConf.ReportName);
            //医療機関名
            listDataPerPage.Add(new("HeaderR", 0, 0, hpInf.HpName));
            //作成日時
            listDataPerPage.Add(new("HeaderR", 0, 1, CIUtil.SDateToShowSWDate(
                CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd")), 0, 1
            ) + DateTime.Now.ToString(" HH:mm") + "作成"));
            //ページ数
            int totalPage = (int)Math.Ceiling((double)printDatas.Count / maxCol);
            listDataPerPage.Add(new("HeaderR", 0, 2, _currentPage + " / " + totalPage));
            //請求年月
            listDataPerPage.Add(new("HeaderL", 0, 1, headerL1.Count >= _currentPage ? headerL1[_currentPage - 1] : string.Empty));
            //改ページ条件
            listDataPerPage.Add(new("HeaderL", 0, 2, headerL2.Count >= _currentPage ? headerL2[_currentPage - 1] : string.Empty));

            //期間
            SetFieldData("Range",
                string.Format(
                    "期間: {0} ～ {1}",
                    CIUtil.SDateToShowSWDate(printConf.StartSinYm * 100 + 1, 0, 1).Substring(0, 12),
                    CIUtil.SDateToShowSWDate(printConf.EndSinYm * 100 + 1, 0, 1).Substring(0, 12)
                )
            );
        }
        #endregion

        #region Body
        int UpdateFormBody()
        {
            int printIndex = (_currentPage - 1) * maxCol;

            int day = 0;
            for (short colNo = 0; colNo < maxCol; colNo++)
            {
                day++;
                int wrkYmd = printDatas[printIndex].SinYm * 100 + day;
                //診療日が日付として評価できる場合に印字する
                if (CIUtil.SDateToDateTime(wrkYmd) != null)
                {
                    listDataPerPage.Add(new("ColTitleA1", colNo, 0, day.ToString()));
                    listDataPerPage.Add(new("ColTitleB1", colNo, 0, day.ToString()));
                    listDataPerPage.Add(new("ColTitleC1", colNo, 0, day.ToString()));
                    listDataPerPage.Add(new("ColTitleA2", colNo, 0, CIUtil.GetYobi(wrkYmd)));
                    listDataPerPage.Add(new("ColTitleB2", colNo, 0, CIUtil.GetYobi(wrkYmd)));
                    listDataPerPage.Add(new("ColTitleC2", colNo, 0, CIUtil.GetYobi(wrkYmd)));
                }
            }

            //存在しているフィールドに絞り込み
            var existsCols = putColumns.Where(p => _objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

            for (short colNo = 0; colNo < maxCol; colNo++)
            {
                var printData = printDatas[printIndex];
                string baseListName = string.Empty;

                if (printData.ReportKbn != null)
                {
                    int wrkYmd = 0;
                    int.TryParse(printData.ReportKbn.Replace("/", string.Empty), out wrkYmd);
                    if (CIUtil.SDateToDateTime(wrkYmd) != null)
                    {
                        short colDay = (short)(int.Parse(printData.ReportKbn.Replace("/", string.Empty).Substring(6, 2)) - 1);
                        //明細データ出力
                        foreach (var colName in existsCols)
                        {
                            var value = typeof(CoSta3062PrintData).GetProperty(colName)?.GetValue(printData);
                            listDataPerPage.Add(new(colName, colDay, 0, value == null ? string.Empty : value.ToString() ?? string.Empty));

                            if (baseListName == string.Empty && _objectRseList.Contains(colName))
                            {
                                baseListName = colName;
                            }
                        }
                    }
                }

                printIndex++;
                if (printIndex >= printDatas.Count)
                {
                    _hasNextPage = false;
                    break;
                }
            }

            return printIndex;
        }
        #endregion

        _listTextData.Add(_currentPage, listDataPerPage);
        #endregion
        UpdateFormHeader();
        UpdateFormBody();
    }

    private void SetFieldData(string field, string value)
    {
        if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
        {
            _singleFieldData.Add(field, value);
        }
    }

    private void GetFieldNameList(string fileName)
    {
        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3062, fileName, new());
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        _objectRseList = javaOutputData.objectNames;
    }

    private bool GetData(int hpId)
    {
        void MakePrintData()
        {
            printDatas = new();
            headerL1 = new();
            headerL2 = new();

            //改ページ条件
            bool pbKaId = new int[] { printConf.PageBreak1, printConf.PageBreak2 }.Contains(1);
            bool pbTantoId = new int[] { printConf.PageBreak1, printConf.PageBreak2 }.Contains(2);

            CoSta3062PrintData setPrintData(List<CoKouiTensuModel> coKouiTensus)
            {
                CoSta3062PrintData printData = new CoSta3062PrintData();

                printData.ReportKbn = outputFileType == CoFileType.Csv ? coKouiTensus.First().SinDate.ToString() : CIUtil.SDateToShowSDate(coKouiTensus.First().SinDate);

                int raiinCount = coKouiTensus.GroupBy(s => s.RaiinNo).Count();

                printData.SinYm = coKouiTensus.Count() >= 1 ? coKouiTensus.First().SinYm : 0;
                printData.KaId = (pbKaId) && coKouiTensus.Count() >= 1 ? coKouiTensus.First().KaId : 0;
                printData.KaSname = (pbKaId) && coKouiTensus.Count() >= 1 ? coKouiTensus.First().KaSname : string.Empty;
                printData.TantoId = (pbTantoId) && coKouiTensus.Count() >= 1 ? coKouiTensus.First().TantoId : 0;
                printData.TantoSname = (pbTantoId) && coKouiTensus.Count() >= 1 ? coKouiTensus.First().TantoSname : string.Empty;

                printData.TotalTensu = coKouiTensus.Sum(s => s.TotalTensu).ToString("#,0");
                printData.TotalCount = raiinCount.ToString("#,0");
                printData.PtCount = coKouiTensus.GroupBy(s => s.PtId).Count().ToString("#,0");
                printData.AvgTensu = coKouiTensus.Count() == 0 ? "0" : CIUtil.RoundInt(coKouiTensus.Sum(s => s.TotalTensu) / raiinCount, 0).ToString("#,0");
                printData.AvgTanka = coKouiTensus.Count() == 0 ? "0" : CIUtil.RoundInt((coKouiTensus.Sum(s => s.TotalIryohi) + coKouiTensus.Sum(s => s.Jihi)) / raiinCount, 0).ToString("#,0");

                printData.KouiTensu0 = coKouiTensus.Sum(s => s.Tensu0).ToString("#,0");    //初診
                printData.KouiTensu1 = coKouiTensus.Sum(s => s.Tensu1).ToString("#,0");    //再診
                printData.KouiTensu2 = coKouiTensus.Sum(s => s.Tensu2).ToString("#,0");    //医学管理
                printData.KouiTensu3 = coKouiTensus.Sum(s => s.Tensu3).ToString("#,0");    //在宅
                printData.KouiTensu4 = coKouiTensus.Sum(s => s.Tensu4).ToString("#,0");    //薬剤器材
                printData.KouiTensu5 = coKouiTensus.Sum(s => s.Tensu5).ToString("#,0");    //投薬
                printData.KouiTensu6 = coKouiTensus.Sum(s => s.Tensu6).ToString("#,0");    //注射
                printData.KouiTensu7 = coKouiTensus.Sum(s => s.Tensu7).ToString("#,0");    //処置
                printData.KouiTensu8 = coKouiTensus.Sum(s => s.Tensu8).ToString("#,0");    //手術
                printData.KouiTensu9 = coKouiTensus.Sum(s => s.Tensu9).ToString("#,0");    //検査
                printData.KouiTensu10 = coKouiTensus.Sum(s => s.Tensu10).ToString("#,0");  //画像
                printData.KouiTensu11 = coKouiTensus.Sum(s => s.Tensu11).ToString("#,0");  //その他
                printData.KouiTensu12 = coKouiTensus.Sum(s => s.Jihi).ToString("#,0");     //自費

                printData.KouiCount0 = coKouiTensus.Where(s => s.Tensu0 > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");    //初診
                printData.KouiCount1 = coKouiTensus.Where(s => s.Tensu1 > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");    //再診
                printData.KouiCount2 = coKouiTensus.Where(s => s.Tensu2 > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");    //医学管理
                printData.KouiCount3 = coKouiTensus.Where(s => s.Tensu3 > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");    //在宅
                printData.KouiCount4 = coKouiTensus.Where(s => s.Tensu4 > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");    //薬剤器材
                printData.KouiCount5 = coKouiTensus.Where(s => s.Tensu5 > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");    //投薬
                printData.KouiCount6 = coKouiTensus.Where(s => s.Tensu6 > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");    //注射
                printData.KouiCount7 = coKouiTensus.Where(s => s.Tensu7 > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");    //処置
                printData.KouiCount8 = coKouiTensus.Where(s => s.Tensu8 > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");    //手術
                printData.KouiCount9 = coKouiTensus.Where(s => s.Tensu9 > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");    //検査
                printData.KouiCount10 = coKouiTensus.Where(s => s.Tensu10 > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");  //画像
                printData.KouiCount11 = coKouiTensus.Where(s => s.Tensu11 > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");  //その他
                printData.KouiCount12 = coKouiTensus.Where(s => s.Jihi > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");     //自費

                int kouiTanka0 = coKouiTensus.Where(s => s.Tensu0 > 0).GroupBy(s => s.RaiinNo).Count() == 0 ? 0 :
                    CIUtil.RoundInt(coKouiTensus.Sum(s => s.Tensu0) / coKouiTensus.Where(s => s.Tensu0 > 0).GroupBy(s => s.RaiinNo).Count(), 0);
                int kouiTanka1 = coKouiTensus.Where(s => s.Tensu1 > 0).GroupBy(s => s.RaiinNo).Count() == 0 ? 0 :
                    CIUtil.RoundInt(coKouiTensus.Sum(s => s.Tensu1) / coKouiTensus.Where(s => s.Tensu1 > 0).GroupBy(s => s.RaiinNo).Count(), 0);
                int kouiTanka2 = coKouiTensus.Where(s => s.Tensu2 > 0).GroupBy(s => s.RaiinNo).Count() == 0 ? 0 :
                    CIUtil.RoundInt(coKouiTensus.Sum(s => s.Tensu2) / coKouiTensus.Where(s => s.Tensu2 > 0).GroupBy(s => s.RaiinNo).Count(), 0);
                int kouiTanka3 = coKouiTensus.Where(s => s.Tensu3 > 0).GroupBy(s => s.RaiinNo).Count() == 0 ? 0 :
                    CIUtil.RoundInt(coKouiTensus.Sum(s => s.Tensu3) / coKouiTensus.Where(s => s.Tensu3 > 0).GroupBy(s => s.RaiinNo).Count(), 0);
                int kouiTanka4 = coKouiTensus.Where(s => s.Tensu4 > 0).GroupBy(s => s.RaiinNo).Count() == 0 ? 0 :
                    CIUtil.RoundInt(coKouiTensus.Sum(s => s.Tensu4) / coKouiTensus.Where(s => s.Tensu4 > 0).GroupBy(s => s.RaiinNo).Count(), 0);
                int kouiTanka5 = coKouiTensus.Where(s => s.Tensu5 > 0).GroupBy(s => s.RaiinNo).Count() == 0 ? 0 :
                    CIUtil.RoundInt(coKouiTensus.Sum(s => s.Tensu5) / coKouiTensus.Where(s => s.Tensu5 > 0).GroupBy(s => s.RaiinNo).Count(), 0);
                int kouiTanka6 = coKouiTensus.Where(s => s.Tensu6 > 0).GroupBy(s => s.RaiinNo).Count() == 0 ? 0 :
                    CIUtil.RoundInt(coKouiTensus.Sum(s => s.Tensu6) / coKouiTensus.Where(s => s.Tensu6 > 0).GroupBy(s => s.RaiinNo).Count(), 0);
                int kouiTanka7 = coKouiTensus.Where(s => s.Tensu7 > 0).GroupBy(s => s.RaiinNo).Count() == 0 ? 0 :
                    CIUtil.RoundInt(coKouiTensus.Sum(s => s.Tensu7) / coKouiTensus.Where(s => s.Tensu7 > 0).GroupBy(s => s.RaiinNo).Count(), 0);
                int kouiTanka8 = coKouiTensus.Where(s => s.Tensu8 > 0).GroupBy(s => s.RaiinNo).Count() == 0 ? 0 :
                    CIUtil.RoundInt(coKouiTensus.Sum(s => s.Tensu8) / coKouiTensus.Where(s => s.Tensu8 > 0).GroupBy(s => s.RaiinNo).Count(), 0);
                int kouiTanka9 = coKouiTensus.Where(s => s.Tensu9 > 0).GroupBy(s => s.RaiinNo).Count() == 0 ? 0 :
                    CIUtil.RoundInt(coKouiTensus.Sum(s => s.Tensu9) / coKouiTensus.Where(s => s.Tensu9 > 0).GroupBy(s => s.RaiinNo).Count(), 0);
                int kouiTanka10 = coKouiTensus.Where(s => s.Tensu10 > 0).GroupBy(s => s.RaiinNo).Count() == 0 ? 0 :
                    CIUtil.RoundInt(coKouiTensus.Sum(s => s.Tensu10) / coKouiTensus.Where(s => s.Tensu10 > 0).GroupBy(s => s.RaiinNo).Count(), 0);
                int kouiTanka11 = coKouiTensus.Where(s => s.Tensu11 > 0).GroupBy(s => s.RaiinNo).Count() == 0 ? 0 :
                    CIUtil.RoundInt(coKouiTensus.Sum(s => s.Tensu11) / coKouiTensus.Where(s => s.Tensu11 > 0).GroupBy(s => s.RaiinNo).Count(), 0);

                printData.KouiTanka0 = kouiTanka0.ToString("#,0");
                printData.KouiTanka1 = kouiTanka1.ToString("#,0");
                printData.KouiTanka2 = kouiTanka2.ToString("#,0");
                printData.KouiTanka3 = kouiTanka3.ToString("#,0");
                printData.KouiTanka4 = kouiTanka4.ToString("#,0");
                printData.KouiTanka5 = kouiTanka5.ToString("#,0");
                printData.KouiTanka6 = kouiTanka6.ToString("#,0");
                printData.KouiTanka7 = kouiTanka7.ToString("#,0");
                printData.KouiTanka8 = kouiTanka8.ToString("#,0");
                printData.KouiTanka9 = kouiTanka9.ToString("#,0");
                printData.KouiTanka10 = kouiTanka10.ToString("#,0");
                printData.KouiTanka11 = kouiTanka11.ToString("#,0");

                printData.TotalTanka =
                    (kouiTanka0 + kouiTanka1 + kouiTanka2 + kouiTanka3 + kouiTanka4 + kouiTanka5 + kouiTanka6 +
                     kouiTanka7 + kouiTanka8 + kouiTanka9 + kouiTanka10 + kouiTanka11).ToString("#,0");

                return printData;
            }

            var sinYms = kouiTensus.GroupBy(s => s.SinYm).OrderBy(s => s.Key).Select(s => s.Key).ToList();
            for (int ymCnt = 0; (ymCnt <= sinYms.Count - 1) || ymCnt == 0; ymCnt++)
            {
                var kaIds = kouiTensus.GroupBy(s => s.KaId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                for (int kaCnt = 0; (pbKaId && kaCnt <= kaIds.Count - 1) || kaCnt == 0; kaCnt++)
                {
                    var tantoIds = kouiTensus.GroupBy(s => s.TantoId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                    for (int taCnt = 0; (pbTantoId && taCnt <= tantoIds.Count - 1) || taCnt == 0; taCnt++)
                    {
                        var curDatas = kouiTensus.Where(s =>
                            (s.SinYm == sinYms[ymCnt]) &&
                            (pbKaId ? s.KaId == kaIds[kaCnt] : true) &&
                            (pbTantoId ? s.TantoId == tantoIds[taCnt] : true)
                        ).ToList();

                        if (curDatas.Count == 0) continue;

                        List<long> grpValues = curDatas
                            .GroupBy(k => k.ReportKbnValue)
                            .OrderBy(k => k.Key)
                            .Select(k => k.Key)
                            .ToList();

                        foreach (var grpValue in grpValues)
                        {
                            var wrkDatas = curDatas.Where(s => s.ReportKbnValue == grpValue).ToList();

                            printDatas.Add(setPrintData(wrkDatas));
                        }

                        //改ページ
                        if ((ymCnt + 1 <= sinYms.Count - 1) ||
                            (pbKaId && (kaCnt + 1 <= kaIds.Count - 1)) ||
                            (pbTantoId && (taCnt + 1 <= tantoIds.Count - 1)))
                        {
                            //改ページ
                            for (int i = printDatas.Count; i % maxCol != 0; i++)
                            {
                                //空行を追加
                                printDatas.Add(new CoSta3062PrintData(RowType.Brank));
                            }
                        }

                        //ヘッダー情報
                        if ((int)Math.Ceiling((double)(printDatas.Count) / maxCol) > headerL1.Count)
                        {
                            //診療年月
                            string wrkYm = CIUtil.Copy(CIUtil.SDateToShowSWDate(curDatas.First().SinYm * 100 + 1, 0, 1, 1), 1, 13);
                            headerL1.Add(wrkYm + "度");

                            //改ページ条件
                            List<string> wrkHeaders = new List<string>();
                            if (pbKaId) wrkHeaders.Add(curDatas.First().KaSname);
                            if (pbTantoId) wrkHeaders.Add(curDatas.First().TantoSname);

                            if (wrkHeaders.Count >= 1) headerL2.Add(string.Join("／", wrkHeaders));
                        }
                    }
                }
            }
        }

        //データ取得
        kouiTensus = _finder.GetKouiTensu(hpId, printConf);
        if (!kouiTensus.Any())
        {
            return false;
        }

        hpInf = _finder.GetHpInf(hpId, kouiTensus.First().SinDate);

        //印刷用データの作成
        MakePrintData();

        return printDatas.Count > 0;
    }

    public CommonExcelReportingModel ExportCsv(CoSta3062PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType)
    {
        throw new NotImplementedException();
    }
    #endregion
}
