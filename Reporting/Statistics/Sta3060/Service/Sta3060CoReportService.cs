using Helper.Common;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3060.DB;
using Reporting.Statistics.Sta3060.Mapper;
using Reporting.Statistics.Sta3060.Models;

namespace Reporting.Statistics.Sta3060.Service;

public class Sta3060CoReportService : ISta3060CoReportService
{
    #region Constant
    private int maxRow = 45;

    private List<PutColumn> csvTotalColumns = new List<PutColumn>
        {
            new PutColumn("RowType", "明細区分"),
            new PutColumn("TotalCaption", "合計行"),
        };

    private readonly List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("ReportKbn", "集計区分", false),
            new PutColumn("SinYmFmt", "診療年月", false, "SinYm"),
            new PutColumn("KaId", "診療科ID", false),
            new PutColumn("KaSname", "診療科", false),
            new PutColumn("TantoId", "担当医ID", false),
            new PutColumn("TantoSname", "担当医", false),
            new PutColumn("SyosinCount", "初診件数"),
            new PutColumn("SaisinCount", "再診件数"),
            new PutColumn("SyosinRate", "初診率"),
            new PutColumn("TotalCount", "合計件数"),
            new PutColumn("PtCount", "実人数"),
            new PutColumn("TotalTensu", "合計点数"),
            new PutColumn("AvgTensu", "平均点数"),
            new PutColumn("KouiTensu0", "診察(点)"),
            new PutColumn("KouiTensu1", "投薬(点)"),
            new PutColumn("KouiTensu2", "注射(点)"),
            new PutColumn("KouiTensu3", "処置(点)"),
            new PutColumn("KouiTensu4", "手術(点)"),
            new PutColumn("KouiTensu5", "検査(点)"),
            new PutColumn("KouiTensu6", "画像(点)"),
            new PutColumn("KouiTensu7", "その他(点)"),
            new PutColumn("KouiTensu8", "自費(円)"),
            new PutColumn("KouiCount0", "診察(件数)"),
            new PutColumn("KouiCount1", "投薬(件数)"),
            new PutColumn("KouiCount2", "注射(件数)"),
            new PutColumn("KouiCount3", "処置(件数)"),
            new PutColumn("KouiCount4", "手術(件数)"),
            new PutColumn("KouiCount5", "検査(件数)"),
            new PutColumn("KouiCount6", "画像(件数)"),
            new PutColumn("KouiCount7", "その他(件数)"),
            new PutColumn("KouiCount8", "自費(件数)"),
            new PutColumn("KouiTensuDetail0",  "D初再診(点)"),
            new PutColumn("KouiTensuDetail1",  "D医管(点)"),
            new PutColumn("KouiTensuDetail2",  "D在宅(点)"),
            new PutColumn("KouiTensuDetail3",  "D検査(点)"),
            new PutColumn("KouiTensuDetail4",  "D画像(点)"),
            new PutColumn("KouiTensuDetail5",  "D投薬(点)"),
            new PutColumn("KouiTensuDetail6",  "D注射(点)"),
            new PutColumn("KouiTensuDetail7",  "Dリハ(点)"),
            new PutColumn("KouiTensuDetail8",  "D精神(点)"),
            new PutColumn("KouiTensuDetail9",  "D処置(点)"),
            new PutColumn("KouiTensuDetail10", "D手術(点)"),
            new PutColumn("KouiTensuDetail11", "D麻酔(点)"),
            new PutColumn("KouiTensuDetail12", "D放射(点)"),
            new PutColumn("KouiTensuDetail13", "D病理(点)"),
            new PutColumn("KouiTensuDetail14", "Dその他(点)"),
            new PutColumn("KouiTensuDetail15", "D自費(円)"),
            new PutColumn("KouiCountDetail0",  "D初再診(件数)"),
            new PutColumn("KouiCountDetail1",  "D医管(件数)"),
            new PutColumn("KouiCountDetail2",  "D在宅(件数)"),
            new PutColumn("KouiCountDetail3",  "D検査(件数)"),
            new PutColumn("KouiCountDetail4",  "D画像(件数)"),
            new PutColumn("KouiCountDetail5",  "D投薬(件数)"),
            new PutColumn("KouiCountDetail6",  "D注射(件数)"),
            new PutColumn("KouiCountDetail7",  "Dリハ(件数)"),
            new PutColumn("KouiCountDetail8",  "D精神(件数)"),
            new PutColumn("KouiCountDetail9",  "D処置(件数)"),
            new PutColumn("KouiCountDetail10", "D手術(件数)"),
            new PutColumn("KouiCountDetail11", "D麻酔(件数)"),
            new PutColumn("KouiCountDetail12", "D放射(件数)"),
            new PutColumn("KouiCountDetail13", "D病理(件数)"),
            new PutColumn("KouiCountDetail14", "Dその他(件数)"),
            new PutColumn("KouiCountDetail15", "D自費(件数)"),
            new PutColumn("PtNum", "患者番号", false),
            new PutColumn("PtName", "患者氏名", false),
            new PutColumn("TotalPtFutan", "患者負担合計"),
            new PutColumn("KouiTensuSyosin", "初診(点)"),
            new PutColumn("KouiTensuSaisin", "再診(点)"),
            new PutColumn("KouiTensuSyosaiSonota", "初再診他(点)"),
            new PutColumn("KouiCountSyosin", "初診(件数)"),
            new PutColumn("KouiCountSaisin", "再診(件数)"),
            new PutColumn("KouiCountSyosaiSonota", "初再診他(件数)"),
        };

    private readonly string[] reportKbnTitles = new string[] { "診療日", "診療年月", "診療科", "担当医", "保険", "年齢区分", "患者" };

    private readonly string[] ageKbnTitles = new string[]
    {
            "0～1歳未満", "1～2歳未満", "2～3歳未満", "3～6歳未満", "7～10歳未満", "10～15歳未満", "15～20歳未満",
            "20～30歳未満", "30～40歳未満", "40～50歳未満", "50～60歳未満", "60～70歳未満", "70～80歳未満", "80～85歳未満", "85歳以上"
    };

    private class HokenKbn
    {
        public int TitleValue;
        public string TitleName;
    }

    private readonly List<HokenKbn> hokenTitles = new List<HokenKbn>
        {
            new HokenKbn() { TitleValue = 111, TitleName = "社保 単独(本人)" },
            new HokenKbn() { TitleValue = 112, TitleName = "　　 単独(６未)" },
            new HokenKbn() { TitleValue = 113, TitleName = "　　 単独(家族)" },
            new HokenKbn() { TitleValue = 114, TitleName = "　　 単独(高齢)" },
            new HokenKbn() { TitleValue = 121, TitleName = "　　 併用(本人)" },
            new HokenKbn() { TitleValue = 122, TitleName = "　　 併用(６未)" },
            new HokenKbn() { TitleValue = 123, TitleName = "　　 併用(家族)" },
            new HokenKbn() { TitleValue = 124, TitleName = "　　 併用(高齢)" },
            new HokenKbn() { TitleValue = 1,   TitleName = "　 ≪社保計≫" },
            new HokenKbn() { TitleValue = 210, TitleName = "　　 公費単独" },
            new HokenKbn() { TitleValue = 220, TitleName = "　　 公費併用" },
            new HokenKbn() { TitleValue = 2,   TitleName = "　 ≪公費計≫" },
            new HokenKbn() { TitleValue = 0,   TitleName = "　 ◆社保合計" },
            new HokenKbn() { TitleValue = 311, TitleName = "国保 単独(本人)" },
            new HokenKbn() { TitleValue = 312, TitleName = "　　 単独(６未)" },
            new HokenKbn() { TitleValue = 313, TitleName = "　　 単独(家族)" },
            new HokenKbn() { TitleValue = 314, TitleName = "　　 単独(高齢)" },
            new HokenKbn() { TitleValue = 321, TitleName = "　　 併用(本人)" },
            new HokenKbn() { TitleValue = 322, TitleName = "　　 併用(６未)" },
            new HokenKbn() { TitleValue = 323, TitleName = "　　 併用(家族)" },
            new HokenKbn() { TitleValue = 324, TitleName = "　　 併用(高齢)" },
            new HokenKbn() { TitleValue = 3,   TitleName = "　 ≪国保計≫" },
            new HokenKbn() { TitleValue = 411, TitleName = "退職 単独(本人)" },
            new HokenKbn() { TitleValue = 412, TitleName = "　　 単独(６未)" },
            new HokenKbn() { TitleValue = 413, TitleName = "　　 単独(家族)" },
            new HokenKbn() { TitleValue = 421, TitleName = "　　 併用(本人)" },
            new HokenKbn() { TitleValue = 422, TitleName = "　　 併用(６未)" },
            new HokenKbn() { TitleValue = 423, TitleName = "　　 併用(家族)" },
            new HokenKbn() { TitleValue = 4,   TitleName = "　 ≪退職計≫" },
            new HokenKbn() { TitleValue = 510, TitleName = "後期 単独" },
            new HokenKbn() { TitleValue = 520, TitleName = "　　 併用" },
            new HokenKbn() { TitleValue = 5,   TitleName = "　 ≪後期計≫" },
            new HokenKbn() { TitleValue = 0,   TitleName = "　 ◆国保合計" },
            new HokenKbn() { TitleValue = 600, TitleName = "保険外 自費" },
            new HokenKbn() { TitleValue = 700, TitleName = "　　　 自費レセ" },
            new HokenKbn() { TitleValue = 800, TitleName = "　　　 労災" },
            new HokenKbn() { TitleValue = 900, TitleName = "　　　 自賠責" },
            new HokenKbn() { TitleValue = 0,   TitleName = "　 ◆保険外計" },
            new HokenKbn() { TitleValue = 0,   TitleName = "　◆合計" }
        };
    #endregion

    #region Private properties

    /// <summary>
    /// CoReport Model
    /// </summary>
    private List<CoSta3060PrintData> printDatas;
    private List<string> headerL1;
    private List<string> headerL2;
    private List<CoKouiTensuModel> kouiTensus;
    private CoHpInfModel hpInf;
    #endregion

    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly ICoSta3060Finder _finder;
    private readonly IReadRseReportFileService _readRseReportFileService;

    private int currentPage;
    private bool hasNextPage;
    private List<string> objectRseList;
    private string rowCountFieldName;
    private CoSta3060PrintConf printConf;
    private CoFileType outputFileType;
    private CoFileType? coFileType;
    private List<PutColumn> putCurColumns = new List<PutColumn>();

    public Sta3060CoReportService(ICoSta3060Finder finder, IReadRseReportFileService readRseReportFileService)
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
        headerL1 = new();
        headerL2 = new();
        kouiTensus = new();
    }

    public CommonReportingRequestModel GetSta3060ReportingData(CoSta3060PrintConf printConf, int hpId, CoFileType outputFileType)
    {
        try
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

            return new Sta3060Mapper(_singleFieldData, _tableFieldData, _extralData, rowCountFieldName, formFileName).GetData();
        }
        finally
        {
            _finder.ReleaseResource();
        }
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
            //請求年月
            _extralData.Add("HeaderL_0_1_" + currentPage, headerL1.Count >= currentPage ? headerL1[currentPage - 1] : string.Empty);
            //改ページ条件
            _extralData.Add("HeaderL_0_2_" + currentPage, headerL2.Count >= currentPage ? headerL2[currentPage - 1] : string.Empty);

            //期間
            SetFieldData("Range",
                string.Format(
                    "期間: {0} ～ {1}",
                    CIUtil.SDateToShowSWDate(printConf.StartSinYm * 100 + 1, 0, 1).Substring(0, 12),
                    CIUtil.SDateToShowSWDate(printConf.EndSinYm * 100 + 1, 0, 1).Substring(0, 12)
                )
            );

            //集計区分
            SetFieldData("ReportKbnTitle", reportKbnTitles[printConf.ReportKbn]);
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

            int printIndex = (currentPage - 1) * maxRow;

            //存在しているフィールドに絞り込み
            var existsCols = putColumns.Where(p => objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                Dictionary<string, CellModel> data = new();
                var printData = printDatas[printIndex];
                string baseListName = string.Empty;

                //明細データ出力
                foreach (var colName in existsCols)
                {
                    var value = typeof(CoSta3060PrintData).GetProperty(colName).GetValue(printData);
                    AddListData(ref data, colName, value == null ? string.Empty : value.ToString() ?? string.Empty);

                    if (baseListName == string.Empty && objectRseList.Contains(colName))
                    {
                        baseListName = colName;
                    }
                }

                //合計行キャプションと件数
                AddListData(ref data, "TotalCaption", printData.TotalCaption);

                if (printConf.ReportKbn == 4)
                {
                    //保険種別毎に区切り線を引く
                    if (new int[] { 14, 37 }.Contains(rowNo))
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
                else
                {
                    //5行毎（2段表示[25行以下]の場合は1行ごと）に区切り線を引く
                    if ((rowNo + 1) % 5 == 0)
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

                _tableFieldData.Add(data);
                printIndex++;
                if (printIndex >= printDatas.Count)
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
        void MakePrintData()
        {
            printDatas = new();
            headerL1 = new();
            headerL2 = new();
            int totalRow = 0;

            //改ページ条件
            bool pbSinYm = new int[] { printConf.PageBreak1, printConf.PageBreak2, printConf.PageBreak3 }.Contains(1);
            bool pbKaId = new int[] { printConf.PageBreak1, printConf.PageBreak2, printConf.PageBreak3 }.Contains(2);
            bool pbTantoId = new int[] { printConf.PageBreak1, printConf.PageBreak2, printConf.PageBreak3 }.Contains(3);

            CoSta3060PrintData setPrintData(List<CoKouiTensuModel> coKouiTensus, int rowNo = 0)
            {
                CoSta3060PrintData printData = new CoSta3060PrintData();

                printData.ReportKbn =
                    printConf.ReportKbn == 0 ? outputFileType == CoFileType.Csv || coFileType == CoFileType.Csv ? coKouiTensus.First().SinDate.ToString() : CIUtil.SDateToShowSDate(coKouiTensus.First().SinDate) :
                    printConf.ReportKbn == 1 ? outputFileType == CoFileType.Csv || coFileType == CoFileType.Csv ? coKouiTensus.First().SinYm.ToString() : CIUtil.SMonthToShowSMonth(coKouiTensus.First().SinYm) :
                    printConf.ReportKbn == 2 ? coKouiTensus.First().KaSname :
                    printConf.ReportKbn == 3 ? coKouiTensus.First().TantoSname :
                    printConf.ReportKbn == 4 ? hokenTitles[rowNo].TitleName :
                    printConf.ReportKbn == 5 ? ageKbnTitles[rowNo] :
                    printConf.ReportKbn == 6 ? coKouiTensus.First().PtNum.ToString().PadLeft(kouiTensus.DefaultIfEmpty()?.Max(x => x?.PtNum)?.ToString().Length ?? 0) + " " + coKouiTensus.First().PtName :
                    string.Empty;

                printData.SinYm = (pbSinYm || printConf.ReportKbn == 1) && coKouiTensus.Count() >= 1 ? coKouiTensus.First().SinYm : 0;
                printData.KaId = (pbKaId || printConf.ReportKbn == 2) && coKouiTensus.Count() >= 1 ? coKouiTensus.First().KaId : 0;
                printData.KaSname = (pbKaId || printConf.ReportKbn == 2) && coKouiTensus.Count() >= 1 ? coKouiTensus.First().KaSname : string.Empty;
                printData.TantoId = (pbTantoId || printConf.ReportKbn == 3) && coKouiTensus.Count() >= 1 ? coKouiTensus.First().TantoId : 0;
                printData.TantoSname = (pbTantoId || printConf.ReportKbn == 3) && coKouiTensus.Count() >= 1 ? coKouiTensus.First().TantoSname : string.Empty;

                printData.SyosinCount = coKouiTensus.Where(s => new int[] { 1, 6 }.Contains(s.SyosaisinKbn)).GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                printData.SaisinCount = coKouiTensus.Where(s => new int[] { 3, 4, 7, 8 }.Contains(s.SyosaisinKbn)).GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                printData.SyosinRate =
                    (
                        (double)coKouiTensus.Where(s => new int[] { 1, 6 }.Contains(s.SyosaisinKbn)).GroupBy(s => s.RaiinNo).Count() == 0 ? 0 :
                            Math.Round(
                                (double)coKouiTensus.Where(s => new int[] { 1, 6 }.Contains(s.SyosaisinKbn)).GroupBy(s => s.RaiinNo).Count() / coKouiTensus.GroupBy(s => s.RaiinNo).Count(),
                                2, MidpointRounding.AwayFromZero
                            )
                    ).ToString("#,0.00");
                printData.TotalCount = coKouiTensus.GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                printData.PtCount = coKouiTensus.GroupBy(s => s.PtId).Count().ToString("#,0");
                printData.TotalTensu = coKouiTensus.Sum(s => s.TotalTensu).ToString("#,0");
                printData.AvgTensu = coKouiTensus.Count() == 0 ? "0" : CIUtil.RoundInt(coKouiTensus.Sum(s => s.TotalTensu) / coKouiTensus.GroupBy(s => s.RaiinNo).Count(), 0).ToString("#,0");

                printData.KouiTensu0 = coKouiTensus.Sum(s => s.Tensu1).ToString("#,0");  //診察
                printData.KouiTensu1 = coKouiTensus.Sum(s => s.Tensu2).ToString("#,0");  //投薬
                printData.KouiTensu2 = coKouiTensus.Sum(s => s.Tensu3).ToString("#,0");  //注射
                printData.KouiTensu3 = coKouiTensus.Sum(s => s.Tensu4).ToString("#,0");  //処置
                printData.KouiTensu4 = coKouiTensus.Sum(s => s.Tensu5).ToString("#,0");  //手術(+麻酔)
                printData.KouiTensu5 = coKouiTensus.Sum(s => s.Tensu6).ToString("#,0");  //検査(+病理)
                printData.KouiTensu6 = coKouiTensus.Sum(s => s.Tensu7).ToString("#,0");  //画像
                printData.KouiTensu7 = coKouiTensus.Sum(s => s.Tensu8).ToString("#,0");  //その他
                printData.KouiTensu8 = coKouiTensus.Sum(s => s.Jihi).ToString("#,0");    //自費

                printData.KouiCount0 = coKouiTensus.Where(s => s.Tensu1 > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");  //診察
                printData.KouiCount1 = coKouiTensus.Where(s => s.Tensu2 > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");  //投薬
                printData.KouiCount2 = coKouiTensus.Where(s => s.Tensu3 > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");  //注射
                printData.KouiCount3 = coKouiTensus.Where(s => s.Tensu4 > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");  //処置
                printData.KouiCount4 = coKouiTensus.Where(s => s.Tensu5 > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");  //手術(+麻酔)
                printData.KouiCount5 = coKouiTensus.Where(s => s.Tensu6 > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");  //検査(+病理)
                printData.KouiCount6 = coKouiTensus.Where(s => s.Tensu7 > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");  //画像
                printData.KouiCount7 = coKouiTensus.Where(s => s.Tensu8 > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");  //その他
                printData.KouiCount8 = coKouiTensus.Where(s => s.Jihi > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");    //自費

                printData.KouiTensuDetail0 = coKouiTensus.Sum(s => s.TensuA).ToString("#,0");
                printData.KouiTensuDetail1 = coKouiTensus.Sum(s => s.TensuB).ToString("#,0");
                printData.KouiTensuDetail2 = coKouiTensus.Sum(s => s.TensuC).ToString("#,0");
                printData.KouiTensuDetail3 = coKouiTensus.Sum(s => s.TensuD).ToString("#,0");
                printData.KouiTensuDetail4 = coKouiTensus.Sum(s => s.TensuE).ToString("#,0");
                printData.KouiTensuDetail5 = coKouiTensus.Sum(s => s.TensuF).ToString("#,0");
                printData.KouiTensuDetail6 = coKouiTensus.Sum(s => s.TensuG).ToString("#,0");
                printData.KouiTensuDetail7 = coKouiTensus.Sum(s => s.TensuH).ToString("#,0");
                printData.KouiTensuDetail8 = coKouiTensus.Sum(s => s.TensuI).ToString("#,0");
                printData.KouiTensuDetail9 = coKouiTensus.Sum(s => s.TensuJ).ToString("#,0");
                printData.KouiTensuDetail10 = coKouiTensus.Sum(s => s.TensuK).ToString("#,0");
                printData.KouiTensuDetail11 = coKouiTensus.Sum(s => s.TensuL).ToString("#,0");
                printData.KouiTensuDetail12 = coKouiTensus.Sum(s => s.TensuM).ToString("#,0");
                printData.KouiTensuDetail13 = coKouiTensus.Sum(s => s.TensuN).ToString("#,0");
                printData.KouiTensuDetail14 = coKouiTensus.Sum(s => s.TensuElse).ToString("#,0");
                printData.KouiTensuDetail15 = coKouiTensus.Sum(s => s.Jihi).ToString("#,0");

                printData.KouiCountDetail0 = coKouiTensus.Where(s => s.TensuA > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                printData.KouiCountDetail1 = coKouiTensus.Where(s => s.TensuB > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                printData.KouiCountDetail2 = coKouiTensus.Where(s => s.TensuC > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                printData.KouiCountDetail3 = coKouiTensus.Where(s => s.TensuD > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                printData.KouiCountDetail4 = coKouiTensus.Where(s => s.TensuE > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                printData.KouiCountDetail5 = coKouiTensus.Where(s => s.TensuF > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                printData.KouiCountDetail6 = coKouiTensus.Where(s => s.TensuG > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                printData.KouiCountDetail7 = coKouiTensus.Where(s => s.TensuH > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                printData.KouiCountDetail8 = coKouiTensus.Where(s => s.TensuI > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                printData.KouiCountDetail9 = coKouiTensus.Where(s => s.TensuJ > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                printData.KouiCountDetail10 = coKouiTensus.Where(s => s.TensuK > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                printData.KouiCountDetail11 = coKouiTensus.Where(s => s.TensuL > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                printData.KouiCountDetail12 = coKouiTensus.Where(s => s.TensuM > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                printData.KouiCountDetail13 = coKouiTensus.Where(s => s.TensuN > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                printData.KouiCountDetail14 = coKouiTensus.Where(s => s.TensuElse > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                printData.KouiCountDetail15 = coKouiTensus.Where(s => s.Jihi > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");

                printData.TotalPtFutan = printConf.ReportKbn == 6 && coKouiTensus.Count() >= 1 ? coKouiTensus.GroupBy(s => new { s.RaiinNo, s.TotalPtFutan }).Sum(s => s.Key.TotalPtFutan).ToString("#,0") : string.Empty;
                printData.PtNum = printConf.ReportKbn == 6 && coKouiTensus.Count() >= 1 ? coKouiTensus.First().PtNum.ToString() : string.Empty;
                printData.PtName = printConf.ReportKbn == 6 && coKouiTensus.Count() >= 1 ? coKouiTensus.First().PtName.ToString() : string.Empty;

                printData.KouiTensuSyosin = coKouiTensus.Sum(s => s.TensuSyosin).ToString("#,0");
                printData.KouiTensuSaisin = coKouiTensus.Sum(s => s.TensuSaisin).ToString("#,0");
                printData.KouiTensuSyosaiSonota = coKouiTensus.Sum(s => s.TensuSyosaiSonota).ToString("#,0");
                printData.KouiCountSyosin = coKouiTensus.Where(s => s.TensuSyosin > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                printData.KouiCountSaisin = coKouiTensus.Where(s => s.TensuSaisin > 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                printData.KouiCountSyosaiSonota = coKouiTensus.Where(s => s.TensuA > 0 && s.TensuSyosin <= 0 && s.TensuSaisin <= 0).GroupBy(s => s.RaiinNo).Count().ToString("#,0");

                return printData;
            }

            var sinYms = kouiTensus.GroupBy(s => s.SinYm).OrderBy(s => s.Key).Select(s => s.Key).ToList();
            for (int ymCnt = 0; (pbSinYm && ymCnt <= sinYms.Count - 1) || ymCnt == 0; ymCnt++)
            {
                var kaIds = kouiTensus.GroupBy(s => s.KaId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                for (int kaCnt = 0; (pbKaId && kaCnt <= kaIds.Count - 1) || kaCnt == 0; kaCnt++)
                {
                    var tantoIds = kouiTensus.GroupBy(s => s.TantoId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                    for (int taCnt = 0; (pbTantoId && taCnt <= tantoIds.Count - 1) || taCnt == 0; taCnt++)
                    {
                        var curDatas = kouiTensus.Where(s =>
                            (pbSinYm ? s.SinYm == sinYms[ymCnt] : true) &&
                            (pbKaId ? s.KaId == kaIds[kaCnt] : true) &&
                            (pbTantoId ? s.TantoId == tantoIds[taCnt] : true)
                        ).ToList();

                        if (curDatas.Count == 0) continue;

                        switch (printConf.ReportKbn)
                        {
                            case 4:
                                //保険種別
                                for (var i = 0; i <= hokenTitles.Count() - 1; i++)
                                {
                                    List<CoKouiTensuModel> wrkDatas;

                                    //小計
                                    if (new int[] { 8, 11, 21, 28, 31 }.Contains(i))
                                    {
                                        wrkDatas = curDatas.Where(s => s.ReportKbnValue / 100 == hokenTitles[i].TitleValue).ToList();
                                        printDatas.Add(setPrintData(wrkDatas, i));
                                        printDatas.Last().RowType = RowType.Total;
                                        //空行を追加
                                        printDatas.Add(new CoSta3060PrintData(RowType.Brank));
                                    }
                                    //社保合計
                                    else if (i == 12)
                                    {
                                        wrkDatas = curDatas.Where(s => new long[] { 1, 2 }.Contains(s.ReportKbnValue / 100)).ToList();
                                        printDatas.Add(setPrintData(wrkDatas, i));
                                        printDatas.Last().RowType = RowType.Total;
                                    }
                                    //国保合計
                                    else if (i == 32)
                                    {
                                        wrkDatas = curDatas.Where(s => new long[] { 3, 4, 5 }.Contains(s.ReportKbnValue / 100)).ToList();
                                        printDatas.Add(setPrintData(wrkDatas, i));
                                        printDatas.Last().RowType = RowType.Total;
                                    }
                                    //保険外計
                                    else if (i == 37)
                                    {
                                        wrkDatas = curDatas.Where(s => new long[] { 6, 7, 8, 9 }.Contains(s.ReportKbnValue / 100)).ToList();
                                        printDatas.Add(setPrintData(wrkDatas, i));
                                        printDatas.Last().RowType = RowType.Total;
                                    }
                                    //総合計
                                    else if (i == 38)
                                    {
                                        //空行を追加
                                        printDatas.Add(new CoSta3060PrintData(RowType.Brank));

                                        printDatas.Add(setPrintData(curDatas, i));
                                        printDatas.Last().RowType = RowType.Total;
                                    }
                                    //明細
                                    else
                                    {
                                        wrkDatas = curDatas.Where(s => s.ReportKbnValue == hokenTitles[i].TitleValue).ToList();
                                        printDatas.Add(setPrintData(wrkDatas, i));
                                    }
                                }
                                break;
                            case 5:
                                //年齢区分別
                                for (var i = 0; i <= ageKbnTitles.Count() - 1; i++)
                                {
                                    var wrkDatas = curDatas.Where(s => s.ReportKbnValue == i).ToList();

                                    printDatas.Add(setPrintData(wrkDatas, i));
                                }
                                break;
                            default:
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
                                break;
                        }

                        //小計
                        if ((pbSinYm || pbKaId || pbTantoId) && printConf.ReportKbn != 4)
                        {
                            //空行を追加
                            printDatas.Add(new CoSta3060PrintData(RowType.Brank));

                            printDatas.Add(setPrintData(curDatas));

                            printDatas.Last().RowType = RowType.Total;
                            printDatas.Last().TotalCaption = "◆合計";
                            printDatas.Last().ReportKbn = string.Empty;

                            //改ページ
                            if ((pbSinYm && (ymCnt + 1 <= sinYms.Count - 1)) ||
                                (pbKaId && (kaCnt + 1 <= kaIds.Count - 1)) ||
                                (pbTantoId && (taCnt + 1 <= tantoIds.Count - 1)))
                            {
                                //改ページ
                                for (int i = printDatas.Count; i % maxRow != 0; i++)
                                {
                                    //空行を追加
                                    printDatas.Add(new CoSta3060PrintData(RowType.Brank));
                                }
                            }
                        }

                        //ヘッダー情報
                        int rowCount = printDatas.Count - totalRow;
                        int pageCount = (int)Math.Ceiling((double)(rowCount) / maxRow);
                        for (int i = 0; i < pageCount; i++)
                        {
                            //診療年月
                            if (pbSinYm)
                            {
                                string wrkYm = CIUtil.Copy(CIUtil.SDateToShowSWDate(curDatas.First().SinYm * 100 + 1, 0, 1, 1), 1, 13);
                                headerL1.Add(wrkYm + "度");
                            }
                            //改ページ条件
                            List<string> wrkHeaders = new List<string>();
                            if (pbKaId) wrkHeaders.Add(curDatas.First().KaSname);
                            if (pbTantoId) wrkHeaders.Add(curDatas.First().TantoSname);

                            if (wrkHeaders.Count >= 1) headerL2.Add(string.Join("／", wrkHeaders));
                        }
                        totalRow += rowCount;
                    }
                }
            }

            if (((pbSinYm || pbKaId || pbTantoId) && printConf.ReportKbn == 4) || printConf.ReportKbn != 4)
            {
                //空行を追加
                printDatas.Add(new CoSta3060PrintData(RowType.Brank));
                printDatas.Add(new CoSta3060PrintData(RowType.Brank));

                //合計
                printDatas.Add(setPrintData(kouiTensus));

                printDatas.Last().RowType = RowType.Total;
                printDatas.Last().TotalCaption = "◆総合計";
                printDatas.Last().ReportKbn = string.Empty;
            }
        }

        //データ取得
        kouiTensus = _finder.GetKouiTensu(hpId, printConf);
        if ((kouiTensus?.Count ?? 0) == 0) return false;

        hpInf = _finder.GetHpInf(hpId, kouiTensus?.FirstOrDefault()?.SinDate ?? 0);

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
        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3060, fileName, new());
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

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3060, fileName, fieldInputList);
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? maxRow;
    }

    public CommonExcelReportingModel ExportCsv(CoSta3060PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType)
    {
        this.printConf = printConf;
        this.coFileType = coFileType;
        string fileName = printConf.ReportName + "_" + monthFrom + "_" + monthTo;
        List<string> retDatas = new List<string>();
        if (!GetData(hpId)) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

        if (isPutTotalRow)
        {
            putCurColumns.AddRange(csvTotalColumns);
        }
        putCurColumns.AddRange(putColumns);

        var csvDatas = printDatas.Where(p => p.RowType == RowType.Data || (isPutTotalRow && p.RowType == RowType.Total)).ToList();
        if (csvDatas.Count == 0) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

        //出力フィールド
        List<string> wrkTitles = putCurColumns.Select(p => p.JpName).ToList();
        List<string> wrkColumns = putCurColumns.Select(p => p.CsvColName).ToList();

        //タイトル行
        List<string> wrkCols = new List<string>();

        foreach (var wrkTitle in wrkTitles)
        {
            if (wrkTitle == "集計区分")
            {
                switch (printConf.ReportKbn)
                {
                    case 0: wrkCols.Add($"\"{wrkTitle}(診療日)\""); break;
                    case 1: wrkCols.Add($"\"{wrkTitle}(診療年月)\""); break;
                    case 2: wrkCols.Add($"\"{wrkTitle}(診療科)\""); break;
                    case 3: wrkCols.Add($"\"{wrkTitle}(担当医)\""); break;
                    case 4: wrkCols.Add($"\"{wrkTitle}(保険種別)\""); break;
                    case 5: wrkCols.Add($"\"{wrkTitle}(年齢区分)\""); break;
                    case 6: wrkCols.Add($"\"{wrkTitle}(患者)\""); break;
                    default:
                        wrkCols.Add("\"" + wrkTitle + "\"");
                        break;
                }
            }
            else
            {
                wrkCols.Add("\"" + wrkTitle + "\"");
            }
        }
        retDatas.Add(string.Join(",", wrkCols));

        wrkCols.Clear();
        if (isPutColName)
        {
            foreach (var wrkColumn in wrkColumns)
            {
                wrkCols.Add("\"" + wrkColumn + "\"");
            }
            retDatas.Add(string.Join(",", wrkCols));
        }

        //データ
        int totalRow = csvDatas.Count;
        int rowOutputed = 0;
        foreach (var csvData in csvDatas)
        {
            retDatas.Add(RecordData(csvData));
            rowOutputed++;
        }

        string RecordData(CoSta3060PrintData csvData)
        {
            List<string> colDatas = new List<string>();

            foreach (var column in putCurColumns)
            {
                var value = typeof(CoSta3060PrintData).GetProperty(column.CsvColName).GetValue(csvData);
                if (csvData.RowType == RowType.Total && !column.IsTotal)
                {
                    value = string.Empty;
                }
                else if (value is RowType)
                {
                    value = (int)value;
                }
                colDatas.Add("\"" + (value == null ? "" : value.ToString()) + "\"");
            }

            return string.Join(",", colDatas);
        }

        return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);
    }
}
