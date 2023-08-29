using Helper.Common;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3071.DB;
using Reporting.Statistics.Sta3071.Mapper;
using Reporting.Statistics.Sta3071.Models;

namespace Reporting.Statistics.Sta3071.Service;

public class Sta3071CoReportService : ISta3071CoReportService
{
    #region Constant
    private readonly List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("RowTitle", string.Empty),
            new PutColumn("SyosaiMidasi", string.Empty),
            new PutColumn("MainVals", string.Empty)
        };

    private struct MidasiTitle
    {
        public string TitleValue;
        public string TitleA1Name;
        public string TitleA2Name;
        public string TitleBName;
    }

    private readonly List<MidasiTitle> hokenTitles = new List<MidasiTitle>
        {
            new MidasiTitle() { TitleValue = "11", TitleA2Name = "社保単独", TitleBName = "社保単独"},
            new MidasiTitle() { TitleValue = "12", TitleA2Name = "社保併用", TitleBName = "社保併用"},
            new MidasiTitle() { TitleValue = "21", TitleA2Name = "公費単独", TitleBName = "公費単独"},
            new MidasiTitle() { TitleValue = "22", TitleA2Name = "公費併用", TitleBName = "公費併用"},
            new MidasiTitle() { TitleValue = "31", TitleA2Name = "国保単独", TitleBName = "国保単独"},
            new MidasiTitle() { TitleValue = "32", TitleA2Name = "国保併用", TitleBName = "国保併用"},
            new MidasiTitle() { TitleValue = "41", TitleA2Name = "退職単独", TitleBName = "退職単独"},
            new MidasiTitle() { TitleValue = "42", TitleA2Name = "退職併用", TitleBName = "退職併用"},
            new MidasiTitle() { TitleValue = "51", TitleA2Name = "後期単独", TitleBName = "後期単独"},
            new MidasiTitle() { TitleValue = "52", TitleA2Name = "後期併用", TitleBName = "後期併用"},
            new MidasiTitle() { TitleValue = "60", TitleA2Name = "自費",     TitleBName = "自費"},
            new MidasiTitle() { TitleValue = "70", TitleA2Name = "自費レセ", TitleBName = "自費レセ" },
            new MidasiTitle() { TitleValue = "80", TitleA2Name = "労災",     TitleBName = "労災"},
            new MidasiTitle() { TitleValue = "90", TitleA2Name = "自賠責",   TitleBName = "自賠責"}
        };

    private readonly List<MidasiTitle> timeTitles = new List<MidasiTitle>
        {
            new MidasiTitle() { TitleValue = "0",  TitleA2Name = "時間内"   ,TitleBName = "時間内"  },
            new MidasiTitle() { TitleValue = "1",  TitleA2Name = "時間外"   ,TitleBName = "時間外" },
            new MidasiTitle() { TitleValue = "2",  TitleA2Name = "休日"     ,TitleBName = "休日" },
            new MidasiTitle() { TitleValue = "3",  TitleA2Name = "深夜"     ,TitleBName = "深夜" },
            new MidasiTitle() { TitleValue = "4",  TitleA2Name = "夜間早朝" ,TitleBName = "夜間早朝" }
        };

    private readonly List<MidasiTitle> ageTitles = new List<MidasiTitle>
        {
            new MidasiTitle() { TitleValue = "00", TitleA1Name = "0～1歳"   ,TitleA2Name = "未満" ,TitleBName = "0歳以上～1歳未満" },
            new MidasiTitle() { TitleValue = "01", TitleA1Name = "1～2歳"   ,TitleA2Name = "未満" ,TitleBName = "1歳以上～2歳未満" },
            new MidasiTitle() { TitleValue = "02", TitleA1Name = "2～3歳"   ,TitleA2Name = "未満" ,TitleBName = "2歳以上～3歳未満" },
            new MidasiTitle() { TitleValue = "03", TitleA1Name = "3～6歳"   ,TitleA2Name = "未満" ,TitleBName = "3歳以上～6歳未満" },
            new MidasiTitle() { TitleValue = "04", TitleA1Name = "6～10歳"  ,TitleA2Name = "未満" ,TitleBName = "6歳以上～10歳未満" },
            new MidasiTitle() { TitleValue = "05", TitleA1Name = "10～15歳" ,TitleA2Name = "未満" ,TitleBName = "10歳以上～15歳未満" },
            new MidasiTitle() { TitleValue = "06", TitleA1Name = "15～20歳" ,TitleA2Name = "未満" ,TitleBName = "15歳以上～20歳未満" },
            new MidasiTitle() { TitleValue = "07", TitleA1Name = "20～30歳" ,TitleA2Name = "未満" ,TitleBName = "20歳以上～30歳未満" },
            new MidasiTitle() { TitleValue = "08", TitleA1Name = "30～40歳" ,TitleA2Name = "未満" ,TitleBName = "30歳以上～40歳未満" },
            new MidasiTitle() { TitleValue = "09", TitleA1Name = "40～50歳" ,TitleA2Name = "未満" ,TitleBName = "40歳以上～50歳未満" },
            new MidasiTitle() { TitleValue = "10", TitleA1Name = "50～60歳" ,TitleA2Name = "未満" ,TitleBName = "50歳以上～60歳未満" },
            new MidasiTitle() { TitleValue = "11", TitleA1Name = "60～70歳" ,TitleA2Name = "未満" ,TitleBName = "60歳以上～70歳未満" },
            new MidasiTitle() { TitleValue = "12", TitleA1Name = "70～80歳" ,TitleA2Name = "未満" ,TitleBName = "70歳以上～80歳未満" },
            new MidasiTitle() { TitleValue = "13", TitleA1Name = "80～85歳" ,TitleA2Name = "未満" ,TitleBName = "80歳以上～85歳未満" },
            new MidasiTitle() { TitleValue = "14", TitleA1Name = "85歳以上" ,TitleBName = "85歳以上" }
        };

    private readonly List<MidasiTitle> sexTitles = new List<MidasiTitle>
        {
            new MidasiTitle() { TitleValue = "0",  TitleA2Name = "不明",     TitleBName = "不明" },
            new MidasiTitle() { TitleValue = "1",  TitleA2Name = "男性",     TitleBName = "男性" },
            new MidasiTitle() { TitleValue = "2",  TitleA2Name = "女性",     TitleBName = "女性" }
        };

    private readonly string[] reportKbnNames = { string.Empty, "診療科別", "担当医別", "保険種別", "日別", "月別", "時間外等別", "年齢区分別", "性別" };

    private readonly string[] syosaiMidasi = { "なし", "初診", "再診", "自費", "合計", "実人数", "新患数" };
    #endregion

    #region Private properties

    /// <summary>
    /// CoReport Model
    /// </summary>
    private List<CoSta3071PrintData> printDatas;
    private List<CoRaiinInfModel>? raiinInfs;
    private CoHpInfModel hpInf;

    private List<MidasiTitle> colTitles;
    private int printBlockCnt = 0;
    private int printColIndex = 0;

    private int maxRow = 28;
    private int maxCol = 16;
    #endregion

    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly ICoSta3071Finder _finder;
    private readonly IReadRseReportFileService _readRseReportFileService;

    private int currentPage;
    private bool hasNextPage;
    private List<string> objectRseList;
    private string rowCountFieldName;
    private CoSta3071PrintConf printConf;
    private CoFileType outputFileType;
    private bool isPutTotalRow;
    private CoFileType? coFileType;

    public Sta3071CoReportService(ICoSta3071Finder finder, IReadRseReportFileService readRseReportFileService)
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
        colTitles = new();
    }

    public CommonReportingRequestModel GetSta3071ReportingData(CoSta3071PrintConf printConf, int hpId, CoFileType outputFileType, bool isPutTotalRow)
    {
        this.printConf = printConf;
        this.outputFileType = outputFileType;
        this.isPutTotalRow = isPutTotalRow;
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
                UpdateDrawForm(hpId);
                currentPage++;
            }
        }

        return new Sta3071Mapper(_singleFieldData, _tableFieldData, _extralData, rowCountFieldName, formFileName).GetData();
    }

    private void UpdateDrawForm(int hpId)
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
            int totalPage = (int)Math.Ceiling((double)printDatas.Count / maxRow) * (int)Math.Ceiling((double)colTitles.Count / maxCol);
            _extralData.Add("HeaderR_0_2_" + currentPage, currentPage + " / " + totalPage);

            //集計区分
            SetFieldData("ReportKbnTitle", string.Format("{0}×{1}", GetReportKbnName(hpId, printConf.ReportKbnV), GetReportKbnName(hpId, printConf.ReportKbnH)));

            //期間
            if (printConf.RangeFrom > 0 || printConf.RangeTo > 0)
            {
                SetFieldData("Range",
                string.Format("期間: {0} ～ {1}",
                    printConf.RangeFrom.ToString().Length == 6 ? CIUtil.SMonthToShowSWMonth(printConf.RangeFrom) : CIUtil.SDateToShowSWDate(printConf.RangeFrom),
                    printConf.RangeTo.ToString().Length == 6 ? CIUtil.SMonthToShowSWMonth(printConf.RangeTo) : CIUtil.SDateToShowSWDate(printConf.RangeTo))
                );
            }
        }
        #endregion

        #region Body
        /// <remarks>改ページの順番…残りの列を印字→残りの行を印字</remarks>
        void UpdateFormBody()
        {
            if (printDatas == null || printDatas.Count == 0)
            {
                hasNextPage = false;
                return;
            }
            int printRowIndex = printBlockCnt * maxRow;

            //列タイトル
            int colLmt = Math.Min(maxCol, colTitles.Count - printColIndex) - 1;
            for (short j = 0; j <= colLmt; j++)
            {
                var value = colTitles[j + printColIndex].TitleA1Name;
                _extralData.Add(string.Format("ColTitleA1_{0}_{1}", j, currentPage), value == null ? string.Empty : value.ToString());
                value = colTitles[j + printColIndex].TitleA2Name;
                _extralData.Add(string.Format("ColTitleA2_{0}_{1}", j, currentPage), value == null ? string.Empty : value.ToString());
                value = colTitles[j + printColIndex].TitleBName;
                _extralData.Add(string.Format("ColTitleB_{0}_{1}", j, currentPage), value == null ? string.Empty : value.ToString());
            }

            //存在しているフィールドに絞り込み
            var existsCols = putColumns.Where(p => objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                Dictionary<string, CellModel> data = new();
                var printData = printDatas[printRowIndex];
                string baseListName = string.Empty;

                //明細データ出力
                foreach (var colName in existsCols)
                {
                    if (colName == "MainVals")
                    {
                        for (short i = 0; i <= colLmt; i++)
                        {
                            var value = printData.MainVals[i + printColIndex];
                            _extralData.Add(string.Format("MainVals_{0}_{1}_{2}", i, rowNo, currentPage), value == null ? string.Empty : value.ToString());
                        }
                    }
                    else
                    {
                        var value = typeof(CoSta3071PrintData).GetProperty(colName).GetValue(printData);
                        AddListData(ref data, colName, value == null ? string.Empty : value.ToString() ?? string.Empty);
                    }

                    if (baseListName == string.Empty && objectRseList.Contains(colName))
                    {
                        baseListName = colName;
                    }
                }

                //集計区分ごとに区切り線を引く
                if (((rowNo + 1) % 7 == 0) && ((rowNo + 1) < maxRow))
                {
                    if (!_extralData.ContainsKey("headerLine"))
                    {
                        _extralData.Add("headerLine", "true");
                    }
                    string rowNoKey = rowNo + "_" + currentPage;
                    _extralData.Add("baseListName_" + rowNoKey, baseListName);
                    _extralData.Add("rowNo_" + rowNoKey, rowNo.ToString());
                }

                //合計の前後に区切り線を引く
                if ((rowNo + 1) % 7 == 4 || (rowNo + 1) % 7 == 5)
                {
                    if (!_extralData.ContainsKey("SyosaiMidasi"))
                    {
                        _extralData.Add("SyosaiMidasi", "true");
                    }
                    if (!_extralData.ContainsKey("MainVals"))
                    {
                        _extralData.Add("MainVals", "true");
                    }
                    string rowNoKey = rowNo + "_" + currentPage + "_2";
                    _extralData.Add("baseListName_" + rowNoKey, baseListName);
                    _extralData.Add("rowNo_" + rowNoKey, rowNo.ToString());
                }

                _tableFieldData.Add(data);
                printRowIndex++;
                if (printRowIndex >= printDatas.Count)
                {
                    break;
                }

            }
            printColIndex += maxCol;

            if (printRowIndex >= printDatas.Count && printColIndex >= colTitles.Count)
            {
                //出力するデータが無くなった
                hasNextPage = false;
            }

            if (printColIndex >= colTitles.Count)
            {
                //全列印字し終わったので、残りの行へ進む
                printColIndex = 0;
                printBlockCnt++;
            }
        }
        #endregion

        UpdateFormHeader();
        UpdateFormBody();
    }

    private string GetReportKbnName(int hpId, int repKbn)
    {
        if (repKbn > CoRaiinInfModel.PtGrtStartIdx)
        {
            //患者グループ
            return _finder.GetPtGrpName(hpId, repKbn - CoRaiinInfModel.PtGrtStartIdx);
        }
        else
        {
            return reportKbnNames[repKbn];
        }
    }

    private bool GetData(int hpId)
    {

        /// <summary>
        /// 各行/列のタイトル取得
        /// </summary>
        /// <param name="direct">集計方向　0:縦 1:横</param>
        /// <param name="type">0:2行タイプ1行目, 1:2行タイプ2行目, 2:1行タイプ</param>
        /// <returns>列/行タイトル</returns>
        string GetTitle(CoRaiinInfModel wrkData, int direct, int type)
        {
            string retA1Name = string.Empty;
            string retA2Name = string.Empty;
            string retBName = string.Empty;

            int repKbn = direct == 0 ? wrkData.ReportKbnV : wrkData.ReportKbnH;
            string kbnVal = direct == 0 ? wrkData.ReportKbnVValue : wrkData.ReportKbnHValue;

            if (repKbn > CoRaiinInfModel.PtGrtStartIdx)
            {
                //患者グループ
                string ptGrpCdName = wrkData.PtGrps?.Find(p => p.GrpId == (repKbn - CoRaiinInfModel.PtGrtStartIdx) && p.GrpCode == kbnVal).GrpCodeName;
                retA1Name = kbnVal;
                retA2Name = ptGrpCdName;
                retBName = string.Format("{0}.{1}", kbnVal, ptGrpCdName);
                retBName = retBName == "." ? "-" : retBName;
            }
            else
            {
                switch (repKbn)
                {
                    case 1:
                        //診療科別
                        retA1Name = string.Format("{0}", wrkData.KaId);
                        retA2Name = wrkData.KaSname;
                        retBName = string.Format("{0}.{1}", wrkData.KaId, wrkData.KaSname);
                        retBName = retBName == "." ? "-" : retBName;
                        break;
                    case 2:
                        //担当医別
                        retA1Name = string.Format("{0}", wrkData.TantoId);
                        retA2Name = wrkData.TantoSname;
                        retBName = string.Format("{0}.{1}", wrkData.TantoId, wrkData.TantoSname);
                        retBName = retBName == "." ? "-" : retBName;
                        break;
                    case 3:
                        //保険種別
                        var hokenTitle = hokenTitles.Find(t => t.TitleValue == kbnVal);
                        retA1Name = hokenTitle.TitleA1Name;
                        retA2Name = hokenTitle.TitleA2Name;
                        retBName = hokenTitle.TitleBName;
                        break;
                    case 4:
                        //日別
                        string ymd = CIUtil.SDateToShowSWDate(CIUtil.StrToIntDef(kbnVal, 0), 0, 1); //yyyy(gee)/MM/dd (ddd)
                        retA1Name = "";
                        retA2Name = ymd.Substring(10, 5) + " " + ymd.Substring(16, 1); //MM/dd ddd
                        retBName = ymd; //yyyy(gee)/MM/dd (ddd)
                        break;
                    case 5:
                        //月別
                        string ym = CIUtil.SMonthToShowSWMonth(CIUtil.StrToIntDef(kbnVal, 0)); //yyyy(gee)/MM
                        retA1Name = ym.Substring(5, 3) + ym.Substring(9, 3); //gee/MM
                        retA2Name = ym.Substring(0, 4) + ym.Substring(9, 3); //yyyy/MM
                        retBName = ym; //yyyy(gee)/MM
                        break;
                    case 6:
                        //時間外等別
                        var timeTitle = timeTitles.Find(t => t.TitleValue == kbnVal);
                        retA1Name = timeTitle.TitleA1Name;
                        retA2Name = timeTitle.TitleA2Name;
                        retBName = timeTitle.TitleBName;
                        break;
                    case 7:
                        //年齢区分別
                        var ageTitle = ageTitles.Find(t => t.TitleValue == kbnVal);
                        retA1Name = ageTitle.TitleA1Name;
                        retA2Name = ageTitle.TitleA2Name;
                        retBName = ageTitle.TitleBName;
                        break;
                    case 8:
                        //性別
                        var sexTitle = sexTitles.Find(t => t.TitleValue == kbnVal);
                        retA1Name = sexTitle.TitleA1Name;
                        retA2Name = sexTitle.TitleA2Name;
                        retBName = sexTitle.TitleBName;
                        break;
                }
            }

            return type switch
            {
                0 => retA1Name,
                1 => retA2Name,
                2 => retBName,
                _ => string.Empty,
            };
        }

        /// <summary>
        /// 全列のタイトル取得
        /// </summary>
        /// <returns>全列のタイトル</returns>
        List<MidasiTitle> GetRecTitles()
        {
            List<MidasiTitle> titles = new List<MidasiTitle>();
            var hVals = raiinInfs.GroupBy(r => r.ReportKbnHValue).OrderBy(r => r.Key).Select(r => r.Key).ToList();
            foreach (var hVal in hVals)
            {
                var hData = raiinInfs.Where(s => s.ReportKbnHValue == hVal).FirstOrDefault();
                titles.Add(new MidasiTitle
                {
                    TitleValue = hVal,
                    TitleA1Name = GetTitle(hData, 1, 0),
                    TitleA2Name = GetTitle(hData, 1, 1),
                    TitleBName = GetTitle(hData, 1, 2)
                });
            };

            if (outputFileType != CoFileType.Csv || coFileType == CoFileType.Csv || isPutTotalRow)
            {
                titles.Add(new MidasiTitle { TitleA2Name = "合計", TitleBName = "合計" });
            }

            return titles;
        }

        void SetPrintData(List<CoRaiinInfModel> syukeiData, bool isTotal = false)
        {
            int[] totalPts = new int[colTitles.Count];

            for (int i = 0; i < syosaiMidasi.Count(); i++)
            {
                //初再診の見出しごとに１行追加
                CoSta3071PrintData printData = new CoSta3071PrintData
                {
                    RowType = isTotal ? RowType.Total : RowType.Data
                };

                #region 見出し             
                if (i == 0 || outputFileType == CoFileType.Csv || coFileType == CoFileType.Csv)
                {
                    //行タイトル
                    printData.RowTitle = isTotal ? "合計" : GetTitle(syukeiData.FirstOrDefault(), 0, 2);
                }
                printData.SyosaiMidasi = syosaiMidasi[i];
                #endregion

                #region 集計
                int[] NasiKbns = { 0, 2 };
                int[] SyosinKbns = { 1, 6 };
                int[] SaisinKbns = { 3, 4, 7, 8 };

                printData.MainVals = new List<string> { };

                for (int j = 0; j < colTitles.Count; j++)
                {

                    var wrkData = (j < colTitles.Count - 1) || (outputFileType == CoFileType.Csv && coFileType == CoFileType.Csv && !(isPutTotalRow && j == colTitles.Count - 1)) ?
                        syukeiData.Where(s => s.ReportKbnHValue == colTitles[j].TitleValue) :
                        syukeiData; //合計列

                    int ptCnt = 0;
                    switch (i)
                    {
                        case 0: //なし
                            ptCnt = wrkData.Where(s => NasiKbns.Contains(s.SyosaisinKbn)).GroupBy(s => s.RaiinNo).Count();
                            totalPts[j] += ptCnt;
                            break;
                        case 1: //初診
                            ptCnt = wrkData.Where(s => SyosinKbns.Contains(s.SyosaisinKbn)).GroupBy(s => s.RaiinNo).Count();
                            totalPts[j] += ptCnt;
                            break;
                        case 2: //再診
                            ptCnt = wrkData.Where(s => SaisinKbns.Contains(s.SyosaisinKbn)).GroupBy(s => s.RaiinNo).Count();
                            totalPts[j] += ptCnt;
                            break;
                        case 3: //自費
                            ptCnt = wrkData.GroupBy(s => s.RaiinNo).Count() - totalPts[j];
                            totalPts[j] += ptCnt;
                            break;
                        case 4: //合計
                            ptCnt = totalPts[j];
                            break;
                        case 5: //実人数
                            ptCnt = wrkData.GroupBy(s => s.PtId).Count();
                            break;
                        case 6: //新患数
                            ptCnt = wrkData.Where(s => s.IsSinkan).GroupBy(s => s.PtId).Count();
                            break;
                        default: break;
                    }

                    printData.MainVals.Add(ptCnt.ToString("#,0"));
                }
                #endregion
                printDatas.Add(printData);
            }
        }

        void MakePrintData()
        {
            printDatas = new List<CoSta3071PrintData>();
            colTitles = new List<MidasiTitle>();

            //列タイトルを取得
            colTitles = GetRecTitles();

            //縦に集計
            var vVals = raiinInfs.GroupBy(r => r.ReportKbnVValue).OrderBy(r => r.Key).Select(r => r.Key).ToList();
            foreach (var vVal in vVals)
            {
                var vData = raiinInfs.Where(s => s.ReportKbnVValue == vVal).ToList();
                SetPrintData(vData);
            }

            //縦の合計 
            SetPrintData(raiinInfs, true);
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
        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3071, fileName, new());
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        objectRseList = javaOutputData.objectNames;
    }

    private void GetRowCount(string fileName)
    {
        rowCountFieldName = putColumns.Find(p => objectRseList.Contains(p.ColName)).ColName;
        List<ObjectCalculate> fieldInputList = new()
        {
            new ObjectCalculate(rowCountFieldName, (int)CalculateTypeEnum.GetListRowCount),
            new ObjectCalculate("MainVals", (int)CalculateTypeEnum.GetListColCount)
        };

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3071, fileName, fieldInputList);
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);

        int i = 0;
        i = javaOutputData.responses?.FirstOrDefault(item => item.listName == rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result / syosaiMidasi.Count() ?? 1;
        maxRow = i * syosaiMidasi.Count();
        maxCol = javaOutputData.responses?.FirstOrDefault(item => item.listName == "MainVals" && item.typeInt == (int)CalculateTypeEnum.GetListColCount)?.result ?? maxCol;
        _extralData.Add("maxCol", maxCol.ToString());
    }

    public CommonExcelReportingModel ExportCsv(CoSta3071PrintConf printConf, int hpId, bool isPutTotalRow, bool isPutColName, int monthFrom, int monthTo, string menuName, CoFileType? coFileType)
    {
        this.printConf = printConf;
        this.coFileType = coFileType;
        string fileName = menuName + "_" + monthFrom + "_" + monthTo;
        List<string> retDatas = new List<string>();
        if (!GetData(hpId)) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

        var csvDatas = printDatas.Where(p => p.RowType == RowType.Data || (isPutTotalRow && p.RowType == RowType.Total)).ToList();
        if (csvDatas.Count == 0) new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

        //出力フィールド
        List<string> wrkTitles = new List<string> { "見出し", "初再診見出し" }; //列タイトルは、見出し分を2列ずらす
        wrkTitles.AddRange(colTitles.Select(c => c.TitleBName).ToList());
        List<string> wrkColumns = putColumns.Select(p => p.ColName).ToList();

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

        string RecordData(CoSta3071PrintData csvData)
        {
            List<string> colDatas = new List<string>();

            foreach (var column in putColumns)
            {
                if (column.ColName == "MainVals")
                {
                    foreach (var val in csvData.MainVals)
                    {
                        colDatas.Add("\"" + (val == null ? "" : val.ToString()) + "\"");
                    }
                }
                else
                {
                    var value = typeof(CoSta3071PrintData).GetProperty(column.ColName).GetValue(csvData);
                    colDatas.Add("\"" + (value == null ? "" : value.ToString()) + "\"");
                }

            }

            return string.Join(",", colDatas);
        }

        return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);
    }
}
