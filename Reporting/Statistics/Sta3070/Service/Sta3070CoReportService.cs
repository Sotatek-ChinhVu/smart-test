using Helper.Common;
using Helper.Extension;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3070.DB;
using Reporting.Statistics.Sta3070.Mapper;
using Reporting.Statistics.Sta3070.Models;

namespace Reporting.Statistics.Sta3070.Service;

public class Sta3070CoReportService : ISta3070CoReportService
{
    #region Constant
    private const int maxCol = 18;

    private readonly List<PutColumn> putRecords = new List<PutColumn>
        {
            new PutColumn("ColTitleA1", string.Empty),
            new PutColumn("ColTitleA2", string.Empty),
            new PutColumn("ColTitleB", string.Empty),
            new PutColumn("SyosinRaiinCnt", "初診来院回数①"),
            new PutColumn("SaisinRaiinCnt", "再診来院回数②"),
            new PutColumn("SonotaRaiinCnt", "その他来院回数③"),
            new PutColumn("RaiinCnt", "来院回数(①+②+③)④"),
            new PutColumn("PtCnt", "実人数⑤"),
            new PutColumn("SinkanCnt", "新患数⑥"),
            new PutColumn("SinDateCnt", "診療日数⑦"),
            new PutColumn("SyosinAvg", "１日平均初診数(①/⑦)"),
            new PutColumn("PtAvg", "１日平均患者数(④/⑦)"),
            new PutColumn("SinkanAvg", "１日平均新患数(⑥/⑦)"),
            new PutColumn("RaiinCntAvg", "患者平均来院回数(④/⑤)"),
            new PutColumn("RaiinRatio", "構成比　来院回数(④)"),
            new PutColumn("PtRatio", "構成比　実人数(⑤)"),
            new PutColumn("SinkanRatio", "構成比　新患数(⑥)"),
            new PutColumn("SyosinMaleCnt", "初診来院回数内訳　男性"),
            new PutColumn("SyosinFemaleCnt", "初診来院回数内訳　女性"),
            new PutColumn("SyosinJinaiCnt", "初診来院回数内訳　時間内"),
            new PutColumn("SyosinJigaiCnt", "初診来院回数内訳　時間外"),
            new PutColumn("SyosinKyujituCnt", "初診来院回数内訳　休日"),
            new PutColumn("SyosinSinyaCnt", "初診来院回数内訳　深夜"),
            new PutColumn("SyosinJigaiEtcCnt", "初診来院回数内訳　時間外等"),
            new PutColumn("SyosinYasouCnt", "初診来院回数内訳　夜間早朝"),
            new PutColumn("SaisinMaleCnt", "再診来院回数内訳　男性"),
            new PutColumn("SaisinFemaleCnt", "再診来院回数内訳　女性"),
            new PutColumn("SaisinJinaiCnt", "再診来院回数内訳　時間内"),
            new PutColumn("SaisinJigaiCnt", "再診来院回数内訳　時間外"),
            new PutColumn("SaisinKyujituCnt", "再診来院回数内訳　休日"),
            new PutColumn("SaisinSinyaCnt", "再診来院回数内訳　深夜"),
            new PutColumn("SaisinJigaiEtcCnt", "再診来院回数内訳　時間外等"),
            new PutColumn("SaisinYasouCnt", "再診来院回数内訳　夜間早朝"),
            new PutColumn("SonotaMaleCnt", "その他来院回数内訳　男性"),
            new PutColumn("SonotaFemaleCnt", "その他来院回数内訳　女性"),
            new PutColumn("SonotaJinaiCnt", "その他来院回数内訳　時間内"),
            new PutColumn("SonotaJigaiCnt", "その他来院回数内訳　時間外"),
            new PutColumn("SonotaKyujituCnt", "その他来院回数内訳　休日"),
            new PutColumn("SonotaSinyaCnt", "その他来院回数内訳　深夜"),
            new PutColumn("SonotaJigaiEtcCnt", "その他来院回数内訳　時間外等"),
            new PutColumn("SonotaYasouCnt", "その他来院回数内訳　夜間早朝"),
            new PutColumn("RaiinMaleCnt", "来院回数内訳　男性"),
            new PutColumn("RaiinFemaleCnt", "来院回数内訳　女性"),
            new PutColumn("RaiinJinaiCnt", "来院回数内訳　時間内"),
            new PutColumn("RaiinJigaiCnt", "来院回数内訳　時間外"),
            new PutColumn("RaiinKyujituCnt", "来院回数内訳　休日"),
            new PutColumn("RaiinSinyaCnt", "来院回数内訳　深夜"),
            new PutColumn("RaiinJigaiEtcCnt", "来院回数内訳　時間外等"),
            new PutColumn("RaiinYasouCnt", "来院回数内訳　夜間早朝"),
            new PutColumn("PtMaleCnt", "実人数内訳　男性"),
            new PutColumn("PtFemaleCnt", "実人数内訳　女性"),
            new PutColumn("PtJinaiCnt", "実人数内訳　時間内"),
            new PutColumn("PtJigaiCnt", "実人数内訳　時間外"),
            new PutColumn("PtKyujituCnt", "実人数内訳　休日"),
            new PutColumn("PtSinyaCnt", "実人数内訳　深夜"),
            new PutColumn("PtJigaiEtcCnt", "実人数内訳　時間外等"),
            new PutColumn("PtYasouCnt", "実人数内訳　夜間早朝"),
            new PutColumn("PtSyosinCnt", "実人数内訳　初診"),
            new PutColumn("PtSaisinCnt", "実人数内訳　再診"),
            new PutColumn("PtSonotaCnt", "実人数内訳　その他"),
            new PutColumn("SinkanMaleCnt", "新患数内訳　男性"),
            new PutColumn("SinkanFemaleCnt", "新患数内訳　女性")
        };

    private class ColTitle
    {
        public int TitleValue;
        public string TitleA1Name = string.Empty;
        public string TitleA2Name = string.Empty;
        public string TitleBName = string.Empty;
    }

    private readonly List<ColTitle> hokenTitles = new List<ColTitle>
        {
            new ColTitle() { TitleValue = 111, TitleA1Name = "社保単独", TitleA2Name = "（本人）" , TitleBName = "社保単独（本人）"},
            new ColTitle() { TitleValue = 112,                           TitleA2Name = "（６未）" , TitleBName = "社保単独（６未）"},
            new ColTitle() { TitleValue = 113,                           TitleA2Name = "（家族）" , TitleBName = "社保単独（家族）"},
            new ColTitle() { TitleValue = 114,                           TitleA2Name = "（高齢）" , TitleBName = "社保単独（高齢）"},
            new ColTitle() { TitleValue = 121, TitleA1Name = "社保併用", TitleA2Name = "（本人）" , TitleBName = "社保併用（本人）"},
            new ColTitle() { TitleValue = 122,                           TitleA2Name = "（６未）" , TitleBName = "社保併用（６未）"},
            new ColTitle() { TitleValue = 123,                           TitleA2Name = "（家族）" , TitleBName = "社保併用（家族）"},
            new ColTitle() { TitleValue = 124,                           TitleA2Name = "（高齢）" , TitleBName = "社保併用（高齢）"},
            new ColTitle() { TitleValue = 210, TitleA1Name = "公費" ,    TitleA2Name = "単独" ,     TitleBName = "公費単独"},
            new ColTitle() { TitleValue = 220, TitleA1Name = "公費 " ,   TitleA2Name = "併用 " ,    TitleBName = "公費併用"},
            new ColTitle() { TitleValue = 311, TitleA1Name = "国保単独", TitleA2Name = "（本人）" , TitleBName = "国保単独（本人）"},
            new ColTitle() { TitleValue = 312,                           TitleA2Name = "（６未）" , TitleBName = "国保単独（６未）"},
            new ColTitle() { TitleValue = 313,                           TitleA2Name = "（家族）" , TitleBName = "国保単独（家族）"},
            new ColTitle() { TitleValue = 314,                           TitleA2Name = "（高齢）" , TitleBName = "国保単独（高齢）"},
            new ColTitle() { TitleValue = 321, TitleA1Name = "国保併用", TitleA2Name = "（本人）" , TitleBName = "国保併用（本人）"},
            new ColTitle() { TitleValue = 322,                           TitleA2Name = "（６未）" , TitleBName = "国保併用（６未）"},
            new ColTitle() { TitleValue = 323,                           TitleA2Name = "（家族）" , TitleBName = "国保併用（家族）"},
            new ColTitle() { TitleValue = 324,                           TitleA2Name = "（高齢）" , TitleBName = "国保併用（高齢）"},
            new ColTitle() { TitleValue = 411, TitleA1Name = "退職単独", TitleA2Name = "（本人）" , TitleBName = "退職単独（本人）"},
            new ColTitle() { TitleValue = 412,                           TitleA2Name = "（６未）" , TitleBName = "退職単独（６未）"},
            new ColTitle() { TitleValue = 413,                           TitleA2Name = "（家族）" , TitleBName = "退職単独（家族）"},
            new ColTitle() { TitleValue = 421, TitleA1Name = "退職併用", TitleA2Name = "（本人）" , TitleBName = "退職併用（本人）"},
            new ColTitle() { TitleValue = 422,                           TitleA2Name = "（６未）" , TitleBName = "退職併用（６未）"},
            new ColTitle() { TitleValue = 423,                           TitleA2Name = "（家族）" , TitleBName = "退職併用（家族）"},
            new ColTitle() { TitleValue = 514, TitleA1Name = "後期" ,    TitleA2Name = "単独" ,     TitleBName = "後期単独"},
            new ColTitle() { TitleValue = 524, TitleA1Name = "後期" ,    TitleA2Name = "併用" ,     TitleBName = "後期併用"},
            new ColTitle() { TitleValue = 600, TitleA1Name = "自費" ,                               TitleBName = "自費" },
            new ColTitle() { TitleValue = 700, TitleA1Name = "自費" ,    TitleA2Name = "レセ" ,     TitleBName = "自費レセ" },
            new ColTitle() { TitleValue = 800, TitleA1Name = "労災" ,                               TitleBName = "労災" },
            new ColTitle() { TitleValue = 900, TitleA1Name = "自賠責" ,                             TitleBName = "自賠責" }
        };

    private readonly List<ColTitle> ageTitles = new List<ColTitle>
        {
            new ColTitle() { TitleValue = 0,  TitleA1Name = "0～1歳",  TitleA2Name = "未満" },
            new ColTitle() { TitleValue = 1,  TitleA1Name = "1～2歳",  TitleA2Name = "未満" },
            new ColTitle() { TitleValue = 2,  TitleA1Name = "2～3歳",  TitleA2Name = "未満" },
            new ColTitle() { TitleValue = 3,  TitleA1Name = "3～6歳",  TitleA2Name = "未満" },
            new ColTitle() { TitleValue = 4,  TitleA1Name = "6～10歳",  TitleA2Name = "未満" },
            new ColTitle() { TitleValue = 5,  TitleA1Name = "10～15歳", TitleA2Name = "未満" },
            new ColTitle() { TitleValue = 6,  TitleA1Name = "15～20歳", TitleA2Name = "未満" },
            new ColTitle() { TitleValue = 7,  TitleA1Name = "20～30歳", TitleA2Name = "未満" },
            new ColTitle() { TitleValue = 8,  TitleA1Name = "30～40歳", TitleA2Name = "未満" },
            new ColTitle() { TitleValue = 9,  TitleA1Name = "40～50歳", TitleA2Name = "未満" },
            new ColTitle() { TitleValue = 10, TitleA1Name = "50～60歳", TitleA2Name = "未満" },
            new ColTitle() { TitleValue = 11, TitleA1Name = "60～70歳", TitleA2Name = "未満" },
            new ColTitle() { TitleValue = 12, TitleA1Name = "70～80歳", TitleA2Name = "未満" },
            new ColTitle() { TitleValue = 13, TitleA1Name = "80～85歳", TitleA2Name = "未満" },
            new ColTitle() { TitleValue = 14, TitleA1Name = "85歳以上" }
        };

    private readonly string[] reportKbnNames = { "診療年月別", "診療年別", "診療科別", "担当医別", "保険種別", "年齢区分別" };

    #endregion

    #region Private properties
    /// <summary>
    /// CoReport Model
    /// </summary>
    private List<CoSta3070PrintData> printDatas;
    private List<string> headerL1;
    private List<CoRaiinInfModel> raiinInfs;
    private CoHpInfModel hpInf;
    #endregion

    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly ICoSta3070Finder _finder;
    private readonly IReadRseReportFileService _readRseReportFileService;

    private int currentPage;
    private bool hasNextPage;
    private List<string> objectRseList;
    private CoSta3070PrintConf printConf;
    private CoFileType outputFileType;
    private CoFileType? coFileType;

    public Sta3070CoReportService(ICoSta3070Finder finder, IReadRseReportFileService readRseReportFileService)
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
        headerL1 = new();
        raiinInfs = new();
    }

    public CommonReportingRequestModel GetSta3070ReportingData(CoSta3070PrintConf printConf, int hpId, CoFileType outputFileType)
    {
        try
        {
            this.printConf = printConf;
            this.outputFileType = outputFileType;
            string formFileName = printConf.FormFileName;

            // get data to print
            GetFieldNameList(formFileName);

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

            return new Sta3070Mapper(_singleFieldData, _tableFieldData, _extralData, formFileName).GetData();
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
            int totalPage = (int)Math.Ceiling((double)printDatas.Count / maxCol);
            if (!_extralData.ContainsKey("totalPage"))
            {
                _extralData.Add("totalPage", totalPage.ToString());
                _extralData.Add("maxCol", maxCol.ToString());
            }
            _extralData.Add("HeaderR_0_2_" + currentPage, currentPage + " / " + totalPage);
            //集計区分
            _extralData.Add("ReportKbnTitle_" + currentPage, reportKbnNames[printConf.ReportKbn]);
            //改ページ条件
            _extralData.Add("HeaderL_0_0_" + currentPage, headerL1.Count >= currentPage ? headerL1[currentPage - 1] : string.Empty);

            //期間
            if (printConf.StartSinYmd > 0 || printConf.EndSinYmd > 0)
            {
                SetFieldData("Range",
                    string.Format(
                        "期間: {0} ～ {1}",
                        printConf.ReportKbn == 0 ? CIUtil.SDateToShowSWDate(printConf.StartSinYmd).Substring(0, 12) :
                        printConf.ReportKbn == 1 ? CIUtil.SDateToShowSWDate(printConf.StartSinYmd).Substring(0, 9) :
                        CIUtil.SDateToShowSWDate(printConf.StartSinYmd),
                        printConf.ReportKbn == 0 ? CIUtil.SDateToShowSWDate(printConf.EndSinYmd).Substring(0, 12) :
                        printConf.ReportKbn == 1 ? CIUtil.SDateToShowSWDate(printConf.EndSinYmd).Substring(0, 9) :
                        CIUtil.SDateToShowSWDate(printConf.EndSinYmd)
                        )
                );
            }
        }
        #endregion

        #region Body
        void UpdateFormBody()
        {
            int hokIndex = (currentPage - 1) * maxCol;

            //存在しているレコードに絞り込み
            var existsRecs = putRecords.Where(p => objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

            for (short colNo = 0; colNo < maxCol; colNo++)
            {
                Dictionary<string, CellModel> data = new();
                var printData = printDatas[hokIndex];
                string baseListName = string.Empty;

                //明細データ出力
                foreach (var recName in existsRecs)
                {
                    var value = typeof(CoSta3070PrintData).GetProperty(recName)?.GetValue(printData);
                    AddListData(ref data, recName, value == null ? string.Empty : value.ToString() ?? string.Empty);

                    if (baseListName == string.Empty && objectRseList.Contains(recName))
                    {
                        baseListName = recName;
                    }
                }
                _tableFieldData.Add(data);
                hokIndex++;
                if (hokIndex >= printDatas.Count)
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

    private struct TotalCnt
    {
        public int RaiinCnt { get; set; }
        public int PtCnt { get; set; }
        public int SinkanCnt { get; set; }

    }

    private bool GetData(int hpId)
    {
        CoSta3070PrintData SetPrintData(List<CoRaiinInfModel> syukeiData, TotalCnt totalCnt, string clA1, string clA2, string clB, bool isTotal = false)
        {
            CoSta3070PrintData printData = new CoSta3070PrintData
            {
                RowType = isTotal ? RowType.Total : RowType.Data
            };

            #region 列タイトル 
            printData.ColTitleA1 = clA1;
            printData.ColTitleA2 = clA2;
            printData.ColTitleB = clB;
            #endregion

            #region 集計
            int[] syosinKbns = { 1, 6 };
            int[] saisinKbns = { 3, 4, 7, 8 };
            int[] jikangaiKbns = { 1, 2, 3, 6, 7 };
            int[] yasouKbns = { 4, 5 };
            int[] kyujituKbns = { 2, 6 };
            int[] sinyaKbns = { 3, 7 };

            //来院回数
            printData.SyosinRaiinCnt = syukeiData.Where(s => syosinKbns.Contains(s.SyosaisinKbn))
                .GroupBy(s => s.RaiinNo).Count().ToString("#,0");
            printData.SaisinRaiinCnt = syukeiData.Where(s => saisinKbns.Contains(s.SyosaisinKbn))
                .GroupBy(s => s.RaiinNo).Count().ToString("#,0");
            printData.SonotaRaiinCnt = syukeiData.Where(s => !syosinKbns.Contains(s.SyosaisinKbn) && !saisinKbns.Contains(s.SyosaisinKbn))
                .GroupBy(s => s.RaiinNo).Count().ToString("#,0");

            int raiinCnt = syukeiData.GroupBy(s => s.RaiinNo).Count();
            printData.RaiinCnt = raiinCnt.ToString("#,0");

            //実人数
            printData.PtSyosinCnt = syukeiData.Where(s => syosinKbns.Contains(s.SyosaisinKbn))
                .GroupBy(s => s.PtId).Count().ToString("#,0");
            printData.PtSaisinCnt = syukeiData.Where(s => saisinKbns.Contains(s.SyosaisinKbn))
                .GroupBy(s => s.PtId).Count().ToString("#,0");
            printData.PtSonotaCnt = syukeiData.Where(s => !syosinKbns.Contains(s.SyosaisinKbn) && !saisinKbns.Contains(s.SyosaisinKbn))
                .GroupBy(s => s.PtId).Count().ToString("#,0");

            int ptCnt = syukeiData.GroupBy(s => s.PtId).Count();
            printData.PtCnt = ptCnt.ToString("#,0");

            //新患数
            int sinkanCnt = syukeiData.Where(s => s.IsSinkan).GroupBy(s => s.PtId).Count();
            printData.SinkanCnt = sinkanCnt.ToString("#,0");
            //診療日数
            int sinDateCnt = syukeiData.GroupBy(s => s.SinDate).Count();
            printData.SinDateCnt = sinDateCnt.ToString("#,0");

            //１日平均
            printData.SyosinAvg = (printData.SyosinRaiinCnt.StrToFloatDef(0) / (sinDateCnt == 0 ? 1 : sinDateCnt)).ToString("#,0.00");
            printData.PtAvg = (printData.RaiinCnt.StrToFloatDef(0) / (sinDateCnt == 0 ? 1 : sinDateCnt)).ToString("#,0.00");
            printData.SinkanAvg = (printData.SinkanCnt.StrToFloatDef(0) / (sinDateCnt == 0 ? 1 : sinDateCnt)).ToString("#,0.00");

            //患者平均来院回数
            printData.RaiinCntAvg = (printData.RaiinCnt.StrToFloatDef(0) / (ptCnt == 0 ? 1 : ptCnt)).ToString("#,0.00");

            //構成比
            if (isTotal)
            {
                printData.RaiinRatio = "-";
                printData.PtRatio = "-";
                printData.SinkanRatio = "-";
            }
            else
            {
                int totalRaiinCnt = totalCnt.RaiinCnt;
                int totalPtCnt = totalCnt.PtCnt;
                int totalSinkanCnt = totalCnt.SinkanCnt;
                printData.RaiinRatio = (100 * printData.RaiinCnt.StrToFloatDef(0) / (totalRaiinCnt == 0 ? 1 : totalRaiinCnt)).ToString("#,0.00");
                printData.PtRatio = (100 * printData.PtCnt.StrToFloatDef(0) / (totalPtCnt == 0 ? 1 : totalPtCnt)).ToString("#,0.00");
                printData.SinkanRatio = (100 * printData.SinkanCnt.StrToFloatDef(0) / (totalSinkanCnt == 0 ? 1 : totalSinkanCnt)).ToString("#,0.00");
            }

            #region 来院回数/実人数内訳
            string[] raiinTypes = { "Syosin", "Saisin", "Sonota", "Raiin", "Pt" };
            string[] raiinSubTypes = { "Male", "Female", "Jinai", "Jigai", "Kyujitu", "Sinya", "JigaiEtc", "Yasou", };
            for (int i = 0; i < raiinTypes.Length; i++)
            {
                for (int j = 0; j < raiinSubTypes.Length; j++)
                {
                    var wrksyukeiData = syukeiData;
                    switch (i)
                    {
                        case 0:
                            wrksyukeiData = wrksyukeiData.Where(s => syosinKbns.Contains(s.SyosaisinKbn)).ToList(); break;
                        case 1:
                            wrksyukeiData = wrksyukeiData.Where(s => saisinKbns.Contains(s.SyosaisinKbn)).ToList(); break;
                        case 2:
                            wrksyukeiData = wrksyukeiData.Where(s => !syosinKbns.Contains(s.SyosaisinKbn) && !saisinKbns.Contains(s.SyosaisinKbn)).ToList(); break;
                        default:
                            break;
                    }
                    switch (j)
                    {
                        case 0:
                            wrksyukeiData = wrksyukeiData.Where(s => s.Sex == 1).ToList(); break;
                        case 1:
                            wrksyukeiData = wrksyukeiData.Where(s => s.Sex != 1).ToList(); break;
                        case 2:
                            wrksyukeiData = wrksyukeiData.Where(s => s.JikanKbn == 0).ToList(); break;
                        case 3:
                            wrksyukeiData = wrksyukeiData.Where(s => s.JikanKbn == 1).ToList(); break;
                        case 4:
                            wrksyukeiData = wrksyukeiData.Where(s => kyujituKbns.Contains(s.JikanKbn)).ToList(); break;
                        case 5:
                            wrksyukeiData = wrksyukeiData.Where(s => sinyaKbns.Contains(s.JikanKbn)).ToList(); break;
                        case 6:
                            wrksyukeiData = wrksyukeiData.Where(s => jikangaiKbns.Contains(s.JikanKbn)).ToList(); break;
                        case 7:
                            wrksyukeiData = wrksyukeiData.Where(s => yasouKbns.Contains(s.JikanKbn)).ToList(); break;
                        default:
                            break;
                    }

                    string newVal = wrksyukeiData.GroupBy(s => raiinTypes[i] == "Pt" ? s.PtId : s.RaiinNo).Count().ToString("#,0");
                    printData.SetMemberValue(raiinTypes[i] + raiinSubTypes[j] + "Cnt", newVal);
                }
            }
            #endregion

            //新患数内訳
            printData.SinkanMaleCnt = syukeiData.Where(s => s.IsSinkan && s.Sex == 1).GroupBy(s => s.PtId).Count().ToString();
            printData.SinkanFemaleCnt = syukeiData.Where(s => s.IsSinkan && s.Sex != 1).GroupBy(s => s.PtId).Count().ToString();
            #endregion

            return printData;
        }

        void MakePrintData()
        {
            printDatas = new List<CoSta3070PrintData>();
            headerL1 = new List<string>();

            //改ページ条件
            bool pbSinYm = outputFileType != CoFileType.Csv && coFileType != CoFileType.Csv && new int[] { printConf.PgBreak1, printConf.PgBreak2, printConf.PgBreak3 }.Contains(1);
            bool pbKaId = outputFileType != CoFileType.Csv && coFileType != CoFileType.Csv && new int[] { printConf.PgBreak1, printConf.PgBreak2, printConf.PgBreak3 }.Contains(2);
            bool pbTantoId = outputFileType != CoFileType.Csv && coFileType != CoFileType.Csv && new int[] { printConf.PgBreak1, printConf.PgBreak2, printConf.PgBreak3 }.Contains(3);

            //ソート順
            raiinInfs = raiinInfs?.OrderBy(r => pbSinYm ? r.SinYm : 0)
                .ThenBy(r => pbKaId ? r.KaId : 0)
                .ThenBy(r => pbTantoId ? r.TantoId : 0)
                .ThenBy(r => r.ReportKbnValue)
                .ThenBy(r => r.SinDate)
                .ToList() ?? new();

            TotalCnt pgtotal = new TotalCnt();

            var sinYms = raiinInfs.GroupBy(s => s.SinYm).OrderBy(s => s.Key).Select(s => s.Key).ToList();
            for (int ymCnt = 0; (pbSinYm && ymCnt <= sinYms.Count - 1) || ymCnt == 0; ymCnt++)
            {
                var kaIds = raiinInfs.GroupBy(s => s.KaId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                for (int kaCnt = 0; (pbKaId && kaCnt <= kaIds.Count - 1) || kaCnt == 0; kaCnt++)
                {
                    var tantoIds = raiinInfs.GroupBy(s => s.TantoId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                    for (int taCnt = 0; (pbTantoId && taCnt <= tantoIds.Count - 1) || taCnt == 0; taCnt++)
                    {
                        var curDatas = raiinInfs.Where(s =>
                            (!pbSinYm || s.SinYm == sinYms[ymCnt]) &&
                            (!pbKaId || s.KaId == kaIds[kaCnt]) &&
                            (!pbTantoId || s.TantoId == tantoIds[taCnt])
                        ).ToList();

                        if (curDatas.Count == 0) continue;

                        int colNo = 0;
                        string clA1Name = string.Empty;
                        string clA2Name = string.Empty;
                        string clBName = string.Empty;

                        //ページごとの合計
                        pgtotal.RaiinCnt = curDatas.GroupBy(s => s.RaiinNo).Count();
                        pgtotal.PtCnt = curDatas.GroupBy(s => s.PtId).Count();
                        pgtotal.SinkanCnt = curDatas.Where(s => s.IsSinkan).GroupBy(s => s.PtId).Count();

                        switch (printConf.ReportKbn)
                        {
                            case 0:
                                //診療年月別
                                int syukeiYm = printConf.StartSinYmd / 100;
                                int endYm = printConf.EndSinYmd / 100;
                                while (syukeiYm <= endYm)
                                {
                                    string ym = CIUtil.SMonthToShowSWMonth(syukeiYm);
                                    clA1Name = string.Format("({0})", ym.Substring(0, 4));
                                    clA2Name = ym.Substring(5, 3) + ym.Substring(9, 3);
                                    //csvはExcelで年月日に変換されないように/を入れない
                                    clBName = outputFileType == CoFileType.Csv ? syukeiYm.ToString() : CIUtil.SMonthToShowSMonth(syukeiYm);

                                    var wrkDatas = curDatas.Where(r => r.SinYm == syukeiYm).ToList();
                                    printDatas.Add(SetPrintData(wrkDatas, pgtotal, clA1Name, clA2Name, clBName));
                                    colNo++;
                                    syukeiYm = CIUtil.MonthsAfter(syukeiYm * 100 + 1, 1) / 100;
                                }
                                break;
                            case 1:
                                //診療年別
                                int syukeiY = printConf.StartSinYmd / 10000;
                                int endY = printConf.EndSinYmd / 10000;
                                while (syukeiY <= endY)
                                {
                                    string y = CIUtil.SMonthToShowSWMonth(syukeiY * 100 + 1);
                                    clA1Name = string.Format("({0})", y.Substring(0, 4));
                                    clA2Name = y.Substring(5, 3);
                                    clBName = syukeiY.ToString();

                                    var wrkDatas = curDatas.Where(r => r.SinY == syukeiY).ToList();
                                    printDatas.Add(SetPrintData(wrkDatas, pgtotal, clA1Name, clA2Name, clBName));
                                    colNo++;
                                    syukeiY++;
                                }
                                break;
                            case 4:
                                //保険種別
                                foreach (var hokenTitle in hokenTitles)
                                {
                                    clA1Name = hokenTitle.TitleA1Name;
                                    clA2Name = hokenTitle.TitleA2Name;
                                    clBName = hokenTitle.TitleBName;

                                    var wrkDatas = curDatas.Where(s => s.ReportKbnValue == hokenTitle.TitleValue).ToList();
                                    printDatas.Add(SetPrintData(wrkDatas, pgtotal, clA1Name, clA2Name, clBName));
                                    colNo++;
                                }
                                break;
                            case 5:
                                //年齢区分別
                                foreach (var ageTitle in ageTitles)
                                {
                                    clA1Name = ageTitle.TitleA1Name;
                                    clA2Name = ageTitle.TitleA2Name;
                                    clBName = ageTitle.TitleA1Name + ageTitle.TitleA2Name;

                                    var wrkDatas = curDatas.Where(s => s.ReportKbnValue == ageTitle.TitleValue).ToList();
                                    printDatas.Add(SetPrintData(wrkDatas, pgtotal, clA1Name, clA2Name, clBName));
                                    colNo++;
                                }
                                break;
                            default:
                                List<int> grpValues = curDatas
                                .GroupBy(k => k.ReportKbnValue)
                                .OrderBy(k => k.Key)
                                .Select(k => k.Key)
                                .ToList();

                                foreach (var grpValue in grpValues)
                                {
                                    var wrkDatas = curDatas.Where(s => s.ReportKbnValue == grpValue).ToList();
                                    if (printConf.ReportKbn == 2)
                                    {
                                        //診療科別
                                        clA1Name = string.Format("({0})", wrkDatas.First().KaId);
                                        clA2Name = wrkDatas.First().KaSname ?? string.Empty;
                                        clBName = string.Format("{0}.{1}", wrkDatas.First().KaId, wrkDatas.First().KaSname);
                                    }
                                    else if (printConf.ReportKbn == 3)
                                    {
                                        //担当医別
                                        clA1Name = string.Format("({0})", wrkDatas.First().TantoId);
                                        clA2Name = wrkDatas.First().TantoSname ?? string.Empty;
                                        clBName = string.Format("{0}.{1}", wrkDatas.First().TantoId, wrkDatas.First().TantoSname);
                                    }
                                    printDatas.Add(SetPrintData(wrkDatas, pgtotal, clA1Name, clA2Name, clBName));
                                    colNo++;
                                }
                                break;
                        }

                        //ページごとの合計
                        printDatas.Add(SetPrintData(curDatas, pgtotal, "合計", "", "合計", true));

                        //ヘッダー情報を追加されたページ数分追加
                        int pageCount = colNo % maxCol == 0 ? colNo / maxCol : (colNo / maxCol) + 1;
                        for (int i = 0; i < pageCount; i++)
                        {
                            //改ページ条件
                            List<string> wrkHeaders = new List<string>();
                            if (pbSinYm) wrkHeaders.Add(CIUtil.SMonthToShowSWMonth(sinYms[ymCnt]));
                            if (pbKaId) wrkHeaders.Add(curDatas.First().KaSname ?? string.Empty);
                            if (pbTantoId) wrkHeaders.Add(curDatas.First().TantoSname ?? string.Empty);
                            headerL1.Add(string.Join("／", wrkHeaders));
                        }

                        //改ページ
                        for (int i = printDatas.Count; i % maxCol != 0; i++)
                        {
                            //空列を追加
                            printDatas.Add(new CoSta3070PrintData(RowType.Brank));
                        }

                    }
                }
            }
        }

        //データ取得
        raiinInfs = _finder.GetRaiinInfs(hpId, printConf);
        if ((raiinInfs?.Count ?? 0) == 0) return false;

        hpInf = _finder.GetHpInf(hpId, raiinInfs?.FirstOrDefault()?.SinDate ?? 0);

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
        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3070, fileName, new());
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        objectRseList = javaOutputData.objectNames;
    }

    public CommonExcelReportingModel ExportCsv(CoSta3070PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType)
    {
        this.printConf = printConf;
        string fileName = menuName + "_" + monthFrom + "_" + monthTo;
        this.coFileType = coFileType;
        List<string> retDatas = new List<string>();

        if (!GetData(hpId)) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

        var csvDatas = printDatas.Where(p => p.RowType == RowType.Data).ToList();
        if (csvDatas.Count == 0) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

        int rowOutputed = 0;
        foreach (var putRecord in putRecords)
        {
            //CSV出力時は２行タイプのタイトルを出力しない
            if (!putRecord.ColName.StartsWith("ColTitleA"))
            {
                //１行ごとにデータをセット
                retDatas.Add(RecordData(putRecord, csvDatas));
            }

            rowOutputed++;
        }

        string RecordData(PutColumn putRec, List<CoSta3070PrintData> putDatas)
        {
            List<string> colDatas = new List<string>();
            //行タイトル
            colDatas.Add("\"" + putRec.JpName + "\"");

            //データ
            foreach (var putData in putDatas)
            {
                var value = typeof(CoSta3070PrintData).GetProperty(putRec.ColName)?.GetValue(putData);
                colDatas.Add("\"" + (value == null ? "" : value.ToString()) + "\"");
            }

            return string.Join(",", colDatas);
        }

        return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);
    }
}
