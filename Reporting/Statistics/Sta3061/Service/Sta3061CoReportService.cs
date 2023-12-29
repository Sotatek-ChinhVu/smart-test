using Entity.Tenant;
using Helper.Common;
using Helper.Extension;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3061.DB;
using Reporting.Statistics.Sta3061.Mapper;
using Reporting.Statistics.Sta3061.Models;
using System.ComponentModel;
using System.Globalization;

namespace Reporting.Statistics.Sta3061.Service;

public class Sta3061CoReportService : ISta3061CoReportService
{
    #region Constant
    private int maxRow = 4;
    private const int pageBreakRow = 39;

    private readonly List<PutColumn> putRows = new List<PutColumn>
        {
            new PutColumn("Tensu11",    "診察    初診"),
            new PutColumn("Tensu12",    "        再診"),
            new PutColumn("Tensu13",    "        医学管理"),
            new PutColumn("Tensu14x",   "        在宅"),
            new PutColumn("Tensu1450",  "        薬剤器材"),
            new PutColumn("Tensu1x",    "    （診察計）"),
            new PutColumn("Tensu2100",  "投薬    薬剤"),
            new PutColumn("Tensu2110",  "        調剤"),
            new PutColumn("Tensu2500",  "        処方"),
            new PutColumn("Tensu2600",  "        麻毒"),
            new PutColumn("Tensu2700",  "        調基"),
            new PutColumn("Tensu2x",    "    （投薬計）"),
            new PutColumn("Tensu31",    "注射    皮下筋肉内"),
            new PutColumn("Tensu32",    "        静脈内"),
            new PutColumn("Tensu33",    "        その他"),
            new PutColumn("Tensu39",    "        薬剤器材"),
            new PutColumn("Tensu3x",    "    （注射計）"),
            new PutColumn("Tensu4000",  "処置    手技"),
            new PutColumn("Tensu4010",  "        薬剤器材"),
            new PutColumn("Tensu4x",    "    （処置計）"),
            new PutColumn("Tensu5000",  "手術    手技"),
            new PutColumn("Tensu5010",  "        薬剤器材"),
            new PutColumn("Tensu5x",    "    （手術計）"),
            new PutColumn("Tensu6000",  "検査    手技"),
            new PutColumn("Tensu6010",  "        薬剤器材"),
            new PutColumn("Tensu6x",    "    （検査計）"),
            new PutColumn("Tensu7000",  "画像    手技"),
            new PutColumn("Tensu7f",    "        フィルム"),
            new PutColumn("Tensu7010",  "        薬剤器材"),
            new PutColumn("Tensu7x",    "    （画像計）"),
            new PutColumn("Tensu8000",  "その他  処方箋"),
            new PutColumn("Tensu8010",  "        その他"),
            new PutColumn("Tensu8020",  "        薬剤器材"),
            new PutColumn("Tensu8x",    "    （その他計）"),
            new PutColumn("Total1",     "≪合計≫          ①"),
            new PutColumn("Rate1",      "（構成比％）"),
            new PutColumn("DayAvg1",    "１日当り平均      ①/③"),
            new PutColumn("RaiinAvg1",  "１来院当り平均    ①/④"),
            new PutColumn("PtAvg1",     "１患者当り平均    ①/⑤"),
            new PutColumn("JihiMeisai", "保険外  <自費明細>"),
            new PutColumn("Total2",     "≪保険外計≫       ②"),
            new PutColumn("Rate2",      "（構成比％）"),
            new PutColumn("DayAvg2",    "１日当り平均       ②/③"),
            new PutColumn("RaiinAvg2",  "１来院当り平均     ②/④"),
            new PutColumn("PtAvg2",     "１患者当り平均     ②/⑤"),
            new PutColumn("DayCount",   "稼働実日数         ③"),
            new PutColumn("RaiinCount", "来院数             ④"),
            new PutColumn("PtCount",    "実人数             ⑤"),
            new PutColumn("DayRaiinAvg","１日平均来院       ④/③"),
            new PutColumn("DayPtAvg",   "１人平均来院       ④/⑤")
        };

    private readonly string[] subTotals = new string[]
    {
            "Tensu1x",
            "Tensu2x",
            "Tensu3x",
            "Tensu4x",
            "Tensu5x",
            "Tensu6x",
            "Tensu7x",
            "Tensu8x"
    };

    private readonly List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("KouiName", "行為名称"),
            new PutColumn("ReportKbn1", "集計タイトル"),
            new PutColumn("Count1", "回数"),
            new PutColumn("Tensu1", "点数/金額"),
            new PutColumn("RaiinTensu1", "1来院当り点数/金額"),
            new PutColumn("Rate1", "構成比％"),
            new PutColumn("ReportKbn2", "集計タイトル"),
            new PutColumn("Count2", "回数"),
            new PutColumn("Tensu2", "点数/金額"),
            new PutColumn("RaiinTensu2", "1来院当り点数/金額"),
            new PutColumn("Rate2", "構成比％"),
            new PutColumn("ReportKbn3", "集計タイトル"),
            new PutColumn("Count3", "回数"),
            new PutColumn("Tensu3", "点数/金額"),
            new PutColumn("RaiinTensu3", "1来院当り点数/金額"),
            new PutColumn("Rate3", "構成比％"),
            new PutColumn("ReportKbn4", "集計タイトル"),
            new PutColumn("Count4", "回数"),
            new PutColumn("Tensu4", "点数/金額"),
            new PutColumn("RaiinTensu4", "1来院当り点数/金額"),
            new PutColumn("Rate4", "構成比％")
        };

    private readonly string[] ageKbnTitles = new string[]
    {
            "0～1歳未満", "1～2歳未満", "2～3歳未満", "3～6歳未満", "7～10歳未満", "10～15歳未満", "15～20歳未満",
            "20～30歳未満", "30～40歳未満", "40～50歳未満", "50～60歳未満", "60～70歳未満", "70～80歳未満", "80～85歳未満", "85歳以上"
    };

    private class HokenKbn
    {
        public string TitleValue { get; set; }
        public string TitleName { get; set; }
    }

    private readonly List<HokenKbn> hokenTitles = new List<HokenKbn>
        {
            new HokenKbn() { TitleValue = "11", TitleName = "社保単独" },
            new HokenKbn() { TitleValue = "12", TitleName = "社保併用" },
            new HokenKbn() { TitleValue = "21", TitleName = "公費単独" },
            new HokenKbn() { TitleValue = "22", TitleName = "公費併用" },
            new HokenKbn() { TitleValue = "31", TitleName = "国保単独" },
            new HokenKbn() { TitleValue = "32", TitleName = "国保併用" },
            new HokenKbn() { TitleValue = "41", TitleName = "退職単独" },
            new HokenKbn() { TitleValue = "42", TitleName = "退職併用" },
            new HokenKbn() { TitleValue = "51", TitleName = "後期単独" },
            new HokenKbn() { TitleValue = "52", TitleName = "後期併用" },
            new HokenKbn() { TitleValue = "60", TitleName = "自費" },
            new HokenKbn() { TitleValue = "70", TitleName = "自費レセ" },
            new HokenKbn() { TitleValue = "80", TitleName = "労災" },
            new HokenKbn() { TitleValue = "90", TitleName = "自賠責" }
        };
    #endregion

    #region Private properties

    /// <summary>
    /// CoReport Model
    /// </summary>
    private List<CoSta3061PrintData> printDatas;
    private List<string> headerL1;
    private List<string> headerL2;
    private List<CoKouiTensuModel> kouiTensus;
    private CoHpInfModel hpInf;
    private List<CoJihiSbtMstModel> jihiSbtMsts;
    #endregion

    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly ICoSta3061Finder _finder;
    private readonly IReadRseReportFileService _readRseReportFileService;

    private int currentPage;
    private bool hasNextPage;
    private List<string> objectRseList;
    private string rowCountFieldName;
    private CoSta3061PrintConf printConf;
    private string ptGrpName;

    public Sta3061CoReportService(ICoSta3061Finder finder, IReadRseReportFileService readRseReportFileService)
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
        jihiSbtMsts = new();
    }

    public CommonReportingRequestModel GetSta3061ReportingData(CoSta3061PrintConf printConf, int hpId)
    {
        try
        {
            this.printConf = printConf;
            string formFileName = printConf.FormFileName;

            // get data to print
            GetFieldNameList(formFileName);
            GetRowCount();

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

            return new Sta3061Mapper(_singleFieldData, _tableFieldData, _extralData, rowCountFieldName, formFileName).GetData();
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
            int totalPage = (int)Math.Ceiling((double)printDatas.Count / maxRow) * 2;
            if (!_extralData.ContainsKey("totalPage"))
            {
                _extralData.Add("totalPage", totalPage.ToString());
            }
            _extralData.Add("HeaderR_0_2_" + currentPage, currentPage + " / " + totalPage);
            //請求年月
            _extralData.Add("HeaderL_0_1_" + currentPage, headerL1.Count >= currentPage ? headerL1[currentPage - 1] : "");
            //改ページ条件
            _extralData.Add("HeaderL_0_2_" + currentPage, headerL2.Count >= currentPage ? headerL2[currentPage - 1] : "");

            //期間
            if (printConf.StartSinDate > 0 || printConf.EndSinDate > 0)
            {
                SetFieldData("Range",
                    string.Format(
                        "期間: {0} ～ {1}",
                        CIUtil.SDateToShowSWDate(printConf.StartSinDate, 0, 1),
                        CIUtil.SDateToShowSWDate(printConf.EndSinDate, 0, 1)
                    )
                );
            }
            else
            {
                SetFieldData("Range",
                    string.Format(
                        "期間: {0} ～ {1}",
                        CIUtil.SDateToShowSWDate(printConf.StartSinYm * 100 + 1, 0, 1).Substring(0, 12),
                        CIUtil.SDateToShowSWDate(printConf.EndSinYm * 100 + 1, 0, 1).Substring(0, 12)
                    )
                );
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
            int printIndex = currentPage == 1 ? 0 : (CIUtil.RoundInt((double)currentPage / 2, 0) - 1) * maxRow;

            bool printJihi = currentPage % 2 == 0;

            var wrkRows = putRows.ToList();
            if (!_extralData.ContainsKey("wrkRowsCount"))
            {
                _extralData.Add("wrkRowsCount", wrkRows.Count.ToString());
            }

            //行為区分名
            if (printJihi)
            {
                //自費明細
                int jihiIndex = wrkRows.FindIndex(r => r.CsvColName == "JihiMeisai");
                if (jihiIndex >= 0)
                {
                    wrkRows.RemoveAt(jihiIndex);
                    for (int i = jihiSbtMsts.Count - 1; i >= 0; i--)
                    {
                        wrkRows.Insert
                        (
                            jihiIndex,
                            new PutColumn()
                            {
                                ColName = $"JihiMeisai{i}",
                                JpName = (i == 0 ? "保険外  " : "        ") + jihiSbtMsts[i].Name
                            }
                        );
                    }
                }

                for (short i = pageBreakRow; i < wrkRows.Count; i++)
                {
                    string rowNoKey = (i - pageBreakRow) + "_" + currentPage;
                    _extralData.Add("KouiName_" + rowNoKey, wrkRows[i].JpName);
                }
            }
            else
            {
                for (short i = 0; i < wrkRows.Count; i++)
                {
                    if (i == pageBreakRow) break;

                    string rowNoKey = i + "_" + currentPage;
                    _extralData.Add("KouiName_" + rowNoKey, wrkRows[i].JpName);
                }
            }

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                if (printIndex + rowNo >= printDatas.Count) break;

                var printData = printDatas[printIndex + rowNo];
                string baseListName = "KouiName";

                if (printData.RowType == RowType.Brank) break;

                _extralData.Add($"ReportKbn{rowNo}_{currentPage}", printData.ReportKbn);

                //明細データ出力
                for (short i = (short)(printJihi ? pageBreakRow : 0); i < wrkRows.Count; i++)
                {
                    CoSta3061PrintData.CountDetail value;

                    if (wrkRows[i].ColName.StartsWith("JihiMeisai"))
                    {
                        value = printData.JihiMeisais[wrkRows[i].ColName.Replace("JihiMeisai", "").AsInteger()];
                    }
                    else
                    {
                        value = (CoSta3061PrintData.CountDetail)typeof(CoSta3061PrintData).GetProperty(wrkRows[i].ColName).GetValue(printData);
                    }
                    short curRow = printJihi ? (short)(i - pageBreakRow) : i;
                    string rowNoKey = curRow + "_" + rowNo + "_" + currentPage;
                    _extralData.Add("Count_" + rowNoKey, value.Count);
                    _extralData.Add("CountRow_" + rowNoKey, curRow.ToString());

                    _extralData.Add("Tensu_" + rowNoKey, value.Tensu);
                    _extralData.Add("TensuRow_" + rowNoKey, curRow.ToString());

                    _extralData.Add("RaiinTensu_" + rowNoKey, value.RaiinTensu);
                    _extralData.Add("RaiinTensuRow_" + rowNoKey, curRow.ToString());

                    _extralData.Add("Rate_" + rowNoKey, value.Rate);
                    _extralData.Add("RateRow_" + rowNoKey, curRow.ToString());

                    if (rowNo >= 1) continue;

                    if (printJihi)
                    {
                        if (new int[] { jihiSbtMsts.Count + 1 }.Contains(i - pageBreakRow))
                        {
                            if (!_extralData.ContainsKey("headerLine"))
                            {
                                _extralData.Add("headerLine", "true");
                            }
                            rowNoKey = curRow + "_" + rowNo + "_" + currentPage;
                            _extralData.Add("baseListName_" + rowNoKey, baseListName);
                            _extralData.Add("rowNo_" + rowNoKey, (i - pageBreakRow).ToString());
                            _extralData.Add("ConLineStyle_" + rowNoKey, "Dash");
                        }
                        else if (new int[] { jihiSbtMsts.Count - 1, jihiSbtMsts.Count + 4, jihiSbtMsts.Count + 9 }.Contains(i - pageBreakRow))
                        {
                            if (!_extralData.ContainsKey("headerLine"))
                            {
                                _extralData.Add("headerLine", "true");
                            }
                            rowNoKey = curRow + "_" + rowNo + "_" + currentPage;
                            _extralData.Add("baseListName_" + rowNoKey, baseListName);
                            _extralData.Add("rowNo_" + rowNoKey, (i - pageBreakRow).ToString());
                            _extralData.Add("ConLineStyle_" + rowNoKey, "Solid");
                        }
                    }
                    else
                    {
                        if (new int[] { 4, 10, 15, 18, 21, 24, 28, 32, 35 }.Contains(i))
                        {
                            if (!_extralData.ContainsKey("headerLine"))
                            {
                                _extralData.Add("headerLine", "true");
                            }
                            rowNoKey = curRow + "_" + rowNo + "_" + currentPage;
                            _extralData.Add("baseListName_" + rowNoKey, baseListName);
                            _extralData.Add("rowNo_" + rowNoKey, i.ToString());
                            _extralData.Add("ConLineStyle_" + rowNoKey, "Dash");
                        }
                        else if (new int[] { 5, 11, 16, 19, 22, 25, 29, 33 }.Contains(i))
                        {
                            if (!_extralData.ContainsKey("headerLine"))
                            {
                                _extralData.Add("headerLine", "true");
                            }
                            rowNoKey = curRow + "_" + rowNo + "_" + currentPage;
                            _extralData.Add("baseListName_" + rowNoKey, baseListName);
                            _extralData.Add("rowNo_" + rowNoKey, i.ToString());
                            _extralData.Add("ConLineStyle_" + rowNoKey, "Solid");
                        }
                    }
                }
            }

            if (printJihi)
            {
                printIndex += maxRow;
                if (printIndex >= printDatas.Count)
                {
                    hasNextPage = false;
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
            printDatas = new List<CoSta3061PrintData>();
            headerL1 = new List<string>();
            headerL2 = new List<string>();
            int totalRow = 0;

            //改ページ条件
            bool pbSinYm = new int[] { printConf.PageBreak1, printConf.PageBreak2, printConf.PageBreak3 }.Contains(1);
            bool pbKaId = new int[] { printConf.PageBreak1, printConf.PageBreak2, printConf.PageBreak3 }.Contains(2);
            bool pbTantoId = new int[] { printConf.PageBreak1, printConf.PageBreak2, printConf.PageBreak3 }.Contains(3);

            double _divValue(double AValue, double BValue)
            {
                return BValue == 0 ? 0 : AValue / BValue;
            }
            string _titileFormat(string AValue)
            {
                string retVal = "<" + AValue + ">";
                while (retVal.Length < 34)
                {
                    retVal = retVal.Insert(1, "-");
                    retVal = retVal.Insert(retVal.Length - 1, "-");
                }
                return retVal;
            }

            CoSta3061PrintData setPrintData(List<CoKouiTensuModel> coKouiTensus, int rowNo = 0)
            {
                CoSta3061PrintData printData = new CoSta3061PrintData();

                printData.ReportKbn =
                    printConf.ReportKbn == 0 ? CIUtil.SDateToShowSDate(coKouiTensus.First().SinDate) :
                    printConf.ReportKbn == 1 ? CIUtil.SMonthToShowSWMonth(coKouiTensus.First().SinYm) :
                    printConf.ReportKbn == 2 ? coKouiTensus.First().KaSname :
                    printConf.ReportKbn == 3 ? coKouiTensus.First().TantoSname :
                    printConf.ReportKbn == 4 ? hokenTitles[rowNo].TitleName :
                    printConf.ReportKbn == 5 ? ageKbnTitles[rowNo] :
                    printConf.ReportKbn == 6 ? coKouiTensus.First().Sex == 1 ? "男" : "女" :
                    printConf.ReportKbn > 10 ? coKouiTensus.First().PtGrpCodeName :
                    string.Empty;

                printData.ReportKbn = _titileFormat(printData.ReportKbn);

                int subCount = 0;
                int totalCount = 0;  //①
                int dayCount = coKouiTensus.GroupBy(s => s.SinDate).Count();    //③
                int raiinCount = coKouiTensus.GroupBy(s => s.RaiinNo).Count();  //④
                int ptCount = coKouiTensus.GroupBy(s => s.PtId).Count();        //⑤

                //診察..その他
                foreach (var putRow in putRows)
                {
                    if (putRow.ColName == "Total1") break;

                    string countColName = "Count" + putRow.ColName.Substring(5, putRow.ColName.Length - 5);
                    int wrkCount = CIUtil.RoundInt(coKouiTensus.Sum(s => typeof(CoKouiTensuModel).GetProperty(countColName).GetValue(s).AsDouble()), 0);
                    int wrkTensu = CIUtil.RoundInt(coKouiTensus.Sum(s => typeof(CoKouiTensuModel).GetProperty(putRow.ColName).GetValue(s).AsDouble()), 0);

                    CoSta3061PrintData.CountDetail wrkDetail = new CoSta3061PrintData.CountDetail();

                    if (subTotals.Contains(putRow.ColName))
                    {
                        wrkDetail.Count = subCount.ToString("#,0");
                        subCount = 0;
                    }
                    else
                    {
                        wrkDetail.Count = wrkCount.ToString("#,0");
                        subCount += wrkCount;
                        totalCount += wrkCount;
                    }

                    wrkDetail.Tensu = wrkTensu.ToString("#,0");
                    wrkDetail.RaiinTensu = CIUtil.RoundInt(_divValue(wrkTensu, raiinCount), 0).ToString("#,0");
                    wrkDetail.Rate = CIUtil.RoundoffNum(_divValue(wrkTensu, coKouiTensus.Sum(s => s.TotalTensu)) * 100, 2).ToString("#,0.0");

                    typeof(CoSta3061PrintData).GetProperty(putRow.ColName).SetValue(printData, wrkDetail);
                }

                //合計①
                printData.Total1.Count = totalCount.ToString("#,0");
                printData.Total1.Tensu = coKouiTensus.Sum(s => s.TotalTensu).ToString("#,0");
                printData.Total1.RaiinTensu = CIUtil.RoundInt(_divValue(coKouiTensus.Sum(s => s.TotalTensu), raiinCount), 0).ToString("#,0");
                printData.Total1.Rate = printData.Total1.Count == "0" && printData.Total1.Tensu == "0" ? "0.0" : "100.0";
                //構成比％
                printData.Rate1.Count = null;  //後から計算
                printData.Rate1.Tensu = CIUtil.RoundoffNum(_divValue(coKouiTensus.Sum(s => s.TotalTensu), kouiTensus.Sum(s => s.TotalTensu)) * 100, 2).ToString("#,0.0");
                //１日当り平均　　①/③
                printData.DayAvg1.Count = CIUtil.RoundoffNum(_divValue(totalCount, dayCount), 2).ToString("#,0.0");
                printData.DayAvg1.Tensu = CIUtil.RoundoffNum(_divValue(coKouiTensus.Sum(s => s.TotalTensu), dayCount), 2).ToString("#,0.0");
                //１来院当り平均　②/④
                printData.RaiinAvg1.Count = CIUtil.RoundoffNum(_divValue(totalCount, raiinCount), 2).ToString("#,0.0");
                printData.RaiinAvg1.Tensu = CIUtil.RoundoffNum(_divValue(coKouiTensus.Sum(s => s.TotalTensu), raiinCount), 2).ToString("#,0.0");
                //１患者当り平均　③/⑤
                printData.PtAvg1.Count = CIUtil.RoundoffNum(_divValue(totalCount, ptCount), 2).ToString("#,0.0");
                printData.PtAvg1.Tensu = CIUtil.RoundoffNum(_divValue(coKouiTensus.Sum(s => s.TotalTensu), ptCount), 2).ToString("#,0.0");

                int jihiCount = 0;  //②

                //保険外明細
                for (int i = 0; i <= jihiSbtMsts.Count - 1; i++)
                {
                    CoSta3061PrintData.CountDetail wrkDetail = new CoSta3061PrintData.CountDetail();

                    int wrkCount = coKouiTensus.Where(s => s.CountJihiMeisais != null).Sum(s => s.CountJihiMeisais[i]);
                    int wrkTensu = CIUtil.RoundInt(coKouiTensus.Where(s => s.JihiMeisais != null).Sum(s => s.JihiMeisais[i]), 0);

                    jihiCount += wrkCount;

                    wrkDetail.Count = wrkCount.ToString("#,0");
                    wrkDetail.Tensu = wrkTensu.ToString("#,0");
                    wrkDetail.RaiinTensu = CIUtil.RoundInt(_divValue(wrkTensu, raiinCount), 0).ToString("#,0");
                    wrkDetail.Rate = CIUtil.RoundoffNum(_divValue(wrkTensu, coKouiTensus.Sum(s => s.Jihi)) * 100, 2).ToString("#,0.0");

                    printData.JihiMeisais.Add(wrkDetail);
                }

                //保険外計②
                printData.Total2.Count = jihiCount.ToString("#,0");
                printData.Total2.Tensu = coKouiTensus.Sum(s => s.Jihi).ToString("#,0");
                printData.Total2.RaiinTensu = CIUtil.RoundInt(_divValue(coKouiTensus.Sum(s => s.Jihi), raiinCount), 0).ToString("#,0");
                printData.Total2.Rate = printData.Total2.Count == "0" && printData.Total2.Tensu == "0" ? "0.0" : "100.0";
                //構成比％
                printData.Rate2.Count = null;  //後から計算
                printData.Rate2.Tensu = CIUtil.RoundoffNum(_divValue(coKouiTensus.Sum(s => s.Jihi), kouiTensus.Sum(s => s.Jihi)) * 100, 2).ToString("#,0.0");
                //１日当り平均　　②/③
                printData.DayAvg2.Count = CIUtil.RoundoffNum(_divValue(jihiCount, dayCount), 2).ToString("#,0.0");
                printData.DayAvg2.Tensu = CIUtil.RoundoffNum(_divValue(coKouiTensus.Sum(s => s.Jihi), dayCount), 2).ToString("#,0.0");
                //１来院当り平均　②/④
                printData.RaiinAvg2.Count = CIUtil.RoundoffNum(_divValue(jihiCount, raiinCount), 2).ToString("#,0.0");
                printData.RaiinAvg2.Tensu = CIUtil.RoundoffNum(_divValue(coKouiTensus.Sum(s => s.Jihi), raiinCount), 2).ToString("#,0.0");
                //１患者当り平均　③/⑤
                printData.PtAvg2.Count = CIUtil.RoundoffNum(_divValue(jihiCount, ptCount), 2).ToString("#,0.0");
                printData.PtAvg2.Tensu = CIUtil.RoundoffNum(_divValue(coKouiTensus.Sum(s => s.Jihi), ptCount), 2).ToString("#,0.0");

                //稼働実日数③
                printData.DayCount.Count = dayCount.ToString("#,0");
                //来院数④
                printData.RaiinCount.Count = raiinCount.ToString("#,0");
                //実人数⑤
                printData.PtCount.Count = ptCount.ToString("#,0");
                //１日平均来院　④/③
                printData.DayRaiinAvg.Count = CIUtil.RoundoffNum(_divValue(raiinCount, dayCount), 2).ToString("#,0.0");
                //１人平均来院　④/⑤
                printData.DayPtAvg.Count = CIUtil.RoundoffNum(_divValue(raiinCount, ptCount), 2).ToString("#,0.0");

                printData.KaSname = pbKaId && coKouiTensus.Count >= 1 ? coKouiTensus.First().KaSname : string.Empty;
                printData.TantoSname = pbTantoId && coKouiTensus.Count >= 1 ? coKouiTensus.First().TantoSname : string.Empty;

                return printData;
            }

            var sinYms = kouiTensus?.GroupBy(s => printConf.IsSinDate ? s.SinDate : s.SinYm).OrderBy(s => s.Key).Select(s => s.Key).ToList();
            for (int ymCnt = 0; (pbSinYm && ymCnt <= sinYms.Count - 1) || ymCnt == 0; ymCnt++)
            {
                var kaIds = kouiTensus?.GroupBy(s => s.KaId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                for (int kaCnt = 0; (pbKaId && kaCnt <= kaIds.Count - 1) || kaCnt == 0; kaCnt++)
                {
                    var tantoIds = kouiTensus.GroupBy(s => s.TantoId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                    for (int taCnt = 0; (pbTantoId && taCnt <= tantoIds.Count - 1) || taCnt == 0; taCnt++)
                    {
                        var curDatas = kouiTensus.Where(s =>
                            (!(pbSinYm && !printConf.IsSinDate) || s.SinYm == sinYms[ymCnt])
                            && (!(pbSinYm && printConf.IsSinDate) || s.SinDate == sinYms[ymCnt])
                            && (!pbKaId || s.KaId == kaIds[kaCnt])
                            && (!pbTantoId || s.TantoId == tantoIds[taCnt])
                        ).ToList();

                        if (curDatas.Count == 0) continue;

                        switch (printConf.ReportKbn)
                        {
                            case 4:
                                //保険種別
                                for (var i = 0; i <= hokenTitles.Count - 1; i++)
                                {
                                    var wrkDatas = curDatas.Where(s => s.ReportKbnValue == hokenTitles[i].TitleValue).ToList();

                                    printDatas.Add(setPrintData(wrkDatas, i));
                                }
                                break;
                            case 5:
                                //年齢区分別
                                for (var i = 0; i <= ageKbnTitles.Count() - 1; i++)
                                {
                                    var wrkDatas = curDatas.Where(s => s.ReportKbnValue == i.ToString()).ToList();

                                    printDatas.Add(setPrintData(wrkDatas, i));
                                }
                                break;
                            default:
                                List<string> grpValues = curDatas
                                    .GroupBy(k => k.ReportKbnValue)
                                    .OrderBy(k => string.IsNullOrEmpty(k.Key)) //分類なしを後ろにする
                                    .ThenBy(k => k.Key)
                                    .Select(k => k.Key)
                                    .ToList();

                                foreach (var grpValue in grpValues)
                                {
                                    var wrkDatas = curDatas.Where(s => s.ReportKbnValue == grpValue).ToList();

                                    printDatas.Add(setPrintData(wrkDatas));
                                }
                                break;
                        }

                        //構成比の計算
                        int total1Count = printDatas.Sum(p => int.Parse(!string.IsNullOrEmpty(p.Total1.Count) ? p.Total1.Count : "0", NumberStyles.Any));
                        int total2Count = printDatas.Sum(p => int.Parse(!string.IsNullOrEmpty(p.Total2.Count) ? p.Total2.Count : "0", NumberStyles.Any));
                        foreach (var printData in printDatas)
                        {
                            printData.Rate1.Count = (total1Count == 0 ? 0 : CIUtil.RoundoffNum((double)int.Parse(!string.IsNullOrEmpty(printData.Total1.Count) ? printData.Total1.Count : "0", NumberStyles.Any) / total1Count * 100, 2)).ToString("#,0.0");
                            printData.Rate2.Count = (total2Count == 0 ? 0 : CIUtil.RoundoffNum((double)int.Parse(!string.IsNullOrEmpty(printData.Total2.Count) ? printData.Total2.Count : "0", NumberStyles.Any) / total2Count * 100, 2)).ToString("#,0.0");
                        }

                        //小計
                        if (pbSinYm || pbKaId || pbTantoId)
                        {
                            printDatas.Add(setPrintData(curDatas));

                            printDatas.Last().RowType = RowType.Total;
                            printDatas.Last().ReportKbn = _titileFormat("合計");

                            //改ページ
                            if ((pbSinYm && (ymCnt + 1 <= sinYms.Count - 1)) ||
                                (pbKaId && (kaCnt + 1 <= kaIds.Count - 1)) ||
                                (pbTantoId && (taCnt + 1 <= tantoIds.Count - 1)))
                            {
                                //改ページ
                                for (int i = printDatas.Count; i % maxRow != 0; i++)
                                {
                                    //空行を追加
                                    printDatas.Add(new CoSta3061PrintData(RowType.Brank));
                                }
                            }
                        }

                        //ヘッダー情報
                        int rowCount = printDatas.Count - totalRow;
                        int pageCount = (int)Math.Ceiling((double)(rowCount) / maxRow) * 2;
                        for (int i = 0; i < pageCount; i++)
                        {
                            //診療年月
                            if (pbSinYm)
                            {
                                if (printConf.IsSinDate)
                                {
                                    string wrkYmd = CIUtil.SDateToShowSWDate(curDatas.First().SinDate, 0, 1, 1);
                                    headerL1.Add(wrkYmd + (printConf.PtGrpId > 0 ? $"＜{ptGrpName}＞" : string.Empty));
                                }
                                else
                                {
                                    string wrkYm = CIUtil.Copy(CIUtil.SDateToShowSWDate(curDatas.First().SinYm * 100 + 1, 0, 1, 1), 1, 13);
                                    headerL1.Add(wrkYm + "度" + (printConf.PtGrpId > 0 ? $"＜{ptGrpName}＞" : string.Empty));
                                }
                            }
                            else if (printConf.PtGrpId > 0)
                            {
                                headerL1.Add($"＜{ptGrpName}＞");
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

            if (!pbSinYm && !pbKaId && !pbTantoId)
            {
                //合計
                printDatas.Add(setPrintData(kouiTensus));

                printDatas.Last().RowType = RowType.Total;
                printDatas.Last().ReportKbn = _titileFormat("合計");
            }
        }

        //データ取得
        if (printConf.IsSinDate)
        {
            kouiTensus = _finder.GetKouiTensu(hpId, printConf);
        }
        else
        {
            if (printConf.EndSinYm - printConf.StartSinYm == 0)
            {
                kouiTensus = _finder.GetKouiTensu(hpId, printConf);
            }
            else
            {
                kouiTensus = _finder.GetKouiTensu2(hpId, printConf);
            }
        }
        if ((kouiTensus?.Count ?? 0) == 0) return false;

        hpInf = _finder.GetHpInf(hpId, kouiTensus?.FirstOrDefault()?.SinDate ?? 0);
        jihiSbtMsts = _finder.GetJihiSbtMst(hpId);
        jihiSbtMsts.Add
        (
            new CoJihiSbtMstModel
            (
                new JihiSbtMst()
                {
                    JihiSbt = 0,
                    Name = "自費算定"
                }
            )
        );
        //患者グループ名
        ptGrpName = _finder.GetPtGrpName(hpId, printConf.PtGrpId);

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

    private void GetFieldNameList(string fileName)
    {
        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3061, fileName, new());
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        objectRseList = javaOutputData.objectNames;
    }

    private void GetRowCount()
    {
        rowCountFieldName = putColumns.Find(p => objectRseList.Contains(p.ColName)).ColName;
    }

    public CommonExcelReportingModel ExportCsv(CoSta3061PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow)
    {
        this.printConf = printConf;
        string fileName = menuName + "_" + monthFrom + "_" + monthTo;
        if (!GetData(hpId)) return new();

        var csvDatas = printDatas.Where(p => p.RowType == RowType.Data || p.RowType == RowType.Total).ToList();
        if (csvDatas.Count == 0) return new();

        bool pbKaId = new int[] { printConf.PageBreak1, printConf.PageBreak2, printConf.PageBreak3 }.Contains(2);
        bool pbTantoId = new int[] { printConf.PageBreak1, printConf.PageBreak2, printConf.PageBreak3 }.Contains(3);

        //自費明細
        List<PutColumn> wrkRows = new List<PutColumn>();
        wrkRows = putRows.ToList();

        int jihiIndex = wrkRows.FindIndex(r => r.CsvColName == "JihiMeisai");
        wrkRows.RemoveAt(jihiIndex);
        for (int i = jihiSbtMsts.Count - 1; i >= 0; i--)
        {
            wrkRows.Insert(jihiIndex, new PutColumn() { ColName = "JihiMeisai" + jihiSbtMsts[i].JihiSbt.ToString(), JpName = jihiSbtMsts[i].Name });
        }

        //集計(縦)
        List<string> retDatas = wrkRows.Select(p => "\"" + p.JpName + "\"").ToList();
        //集計区分
        retDatas.Insert(0, "\"\"");
        //改ページ条件
        retDatas.Insert(0, "\"\"");
        //タイトル
        retDatas.Insert(0, "\"(単位：点/円)\"");

        //データ
        int totalRow = csvDatas.Count;
        int rowOutputed = 0;
        bool pageBreak = true;
        foreach (var csvData in csvDatas)
        {
            if (pageBreak)
            {
                retDatas[0] += ",\"" + (pbKaId ? $"<{csvData.KaSname}>" : "") + (pbTantoId ? $"<{csvData.TantoSname}>" : "") + "\",\"\",\"\",\"\"";
                pageBreak = false;
            }
            else
            {
                retDatas[0] += ",\"\",\"\",\"\",\"\"";
            }
            if (csvData.RowType == RowType.Total)
            {
                pageBreak = true;
            }

            retDatas[1] += ",\"" + csvData.ReportKbn + "\",\"\",\"\",\"\"";
            retDatas[2] += ",\"回数\",\"点数/金額\",\"1来院当り点数/金額\",\"構成比％\"";

            for (int i = 0; i < wrkRows.Count; i++)
            {
                CoSta3061PrintData.CountDetail value;

                if (wrkRows[i].ColName.StartsWith("JihiMeisai"))
                {
                    value = csvData.JihiMeisais[i - jihiIndex];
                }
                else
                {
                    value = (CoSta3061PrintData.CountDetail)typeof(CoSta3061PrintData).GetProperty(wrkRows[i].ColName).GetValue(csvData);
                }
                retDatas[i + 3] += ",\"" + value.Count + "\"";
                retDatas[i + 3] += ",\"" + value.Tensu + "\"";
                retDatas[i + 3] += ",\"" + value.RaiinTensu + "\"";
                retDatas[i + 3] += ",\"" + value.Rate + "\"";
            }

            rowOutputed++;
        }

        return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);
    }
}
