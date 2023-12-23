using Helper.Common;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta3050.DB;
using Reporting.Statistics.Sta3050.Mapper;
using Reporting.Statistics.Sta3050.Models;

namespace Reporting.Statistics.Sta3050.Service;

public class Sta3050CoReportService : ISta3050CoReportService
{
    #region Constant
    private int maxRow = 43;

    private readonly List<PutColumn> csvTotalColumns = new List<PutColumn>
        {
            new PutColumn("RowType", "明細区分"),
            new PutColumn("TotalCaption", "合計行"),
        };

    private readonly List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("SinDateS", "診療日", false, "SinDate"),
            new PutColumn("PtNum", "患者番号", false),
            new PutColumn("PtKanaName", "カナ氏名", false),
            new PutColumn("PtName", "氏名", false),
            new PutColumn("Sex", "性別", false),
            new PutColumn("Age", "年齢", false),
            new PutColumn("AgeYear", "年齢(年数)", false),
            new PutColumn("AgeMonth", "年齢(月数)", false),
            new PutColumn("KaId", "診療科ID", false),
            new PutColumn("KaSname", "診療科", false),
            new PutColumn("TantoId", "担当医ID", false),
            new PutColumn("TantoSname", "担当医", false),
            new PutColumn("SinId", "レセプト識別", false),
            new PutColumn("SinKouiKbn", "項目種別", false),
            new PutColumn("ItemCd", "診療行為コード", false),
            new PutColumn("ItemName", "診療行為名称", false),
            new PutColumn("Ten", "単価", false),
            new PutColumn("TenUnit", "単価(単位)", false),
            new PutColumn("Suryo", "数量", false),
            new PutColumn("UnitName", "単位", false),
            new PutColumn("Count", "回数", false),
            new PutColumn("TotalSuryo", "合計数量"),
            new PutColumn("Money", "金額"),
            new PutColumn("Syosaisin", "初再診", false),
            new PutColumn("HokenSbt", "保険種別", false),
            new PutColumn("InoutKbn", "院内院外区分", false),
            new PutColumn("MadokuKbn", "麻毒区分", false),
            new PutColumn("MadokuKbnSname", "麻毒区分略称", false),
            new PutColumn("MadokuKbnName", "麻毒区分名称", false),
            new PutColumn("KouseisinKbn", "向精神薬区分", false),
            new PutColumn("KouseisinKbnSname", "向精神薬区分略称", false),
            new PutColumn("KouseisinKbnName", "向精神薬区分名称", false),
            new PutColumn("KohatuKbn", "後発医薬品区分", false),
            new PutColumn("KohatuKbnName", "後発医薬品区分名称", false),
            new PutColumn("IsAdopted", "採用区分", false),
            new PutColumn("HokenPid", "保険組合せID", false),
            new PutColumn("Kohi1Houbetu", "公１法別", false),
            new PutColumn("Kohi2Houbetu", "公２法別", false),
            new PutColumn("Kohi3Houbetu", "公３法別", false),
            new PutColumn("Kohi4Houbetu", "公４法別", false)
        };
    #endregion

    #region Private properties
    /// <summary>
    /// CoReport Model
    /// </summary>
    private List<CoSta3050PrintData> printDatas;
    private List<string> headerL1;
    private List<string> headerL2;
    private List<CoSinKouiModel> sinKouis;
    private CoHpInfModel hpInf;

    private readonly List<PutColumn> putCurColumns = new();
    #endregion

    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly ICoSta3050Finder _finder;
    private readonly IReadRseReportFileService _readRseReportFileService;

    private int currentPage;
    private bool hasNextPage;
    private List<string> objectRseList;
    private string rowCountFieldName;
    private CoSta3050PrintConf printConf;
    private CoFileType outputFileType;
    private CoFileType? coFileType;

    public Sta3050CoReportService(ICoSta3050Finder finder, IReadRseReportFileService readRseReportFileService)
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
        sinKouis = new();
    }

    public CommonReportingRequestModel GetSta3050ReportingData(CoSta3050PrintConf printConf, int hpId, CoFileType outputFileType)
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

            return new Sta3050Mapper(_singleFieldData, _tableFieldData, _extralData, rowCountFieldName, formFileName).GetData();
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
                    printConf.StartSinYm >= 0 ? CIUtil.SDateToShowSWDate(printConf.StartSinYm * 100 + 1, 0, 1).Substring(0, 12) : CIUtil.SDateToShowSWDate(printConf.StartSinDate, 0, 1),
                    printConf.EndSinYm >= 0 ? CIUtil.SDateToShowSWDate(printConf.EndSinYm * 100 + 1, 0, 1).Substring(0, 12) : CIUtil.SDateToShowSWDate(printConf.EndSinDate, 0, 1)
                )
            );
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

            int hokIndex = (currentPage - 1) * maxRow;

            //存在しているフィールドに絞り込み
            var existsCols = putColumns.Where(p => objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                Dictionary<string, CellModel> data = new();
                var printData = printDatas[hokIndex];
                string baseListName = string.Empty;

                //明細データ出力
                foreach (var colName in existsCols)
                {
                    var value = typeof(CoSta3050PrintData).GetProperty(colName)?.GetValue(printData);
                    AddListData(ref data, colName, value == null ? string.Empty : value.ToString() ?? string.Empty);

                    if (baseListName == string.Empty && objectRseList.Contains(colName))
                    {
                        baseListName = colName;
                    }
                }

                //合計行キャプションと件数
                AddListData(ref data, "TotalCaption", printData.TotalCaption);

                //5行毎に区切り線を引く
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

            var sinYms = sinKouis?.GroupBy(s => s.SinYm).OrderBy(s => s.Key).Select(s => s.Key).ToList();
            for (int ymCnt = 0; (pbSinYm && ymCnt <= sinYms?.Count - 1) || ymCnt == 0; ymCnt++)
            {
                var kaIds = sinKouis?.GroupBy(s => s.KaId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                for (int kaCnt = 0; (pbKaId && kaCnt <= kaIds?.Count - 1) || kaCnt == 0; kaCnt++)
                {
                    var tantoIds = sinKouis?.GroupBy(s => s.TantoId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                    for (int taCnt = 0; (pbTantoId && taCnt <= tantoIds?.Count - 1) || taCnt == 0; taCnt++)
                    {
                        var curDatas = sinKouis?.Where(s =>
                            (!pbSinYm || (sinYms != null && s.SinYm == sinYms[ymCnt])) &&
                            (!pbKaId || (kaIds != null && s.KaId == kaIds[kaCnt])) &&
                            (!pbTantoId || (tantoIds != null && s.TantoId == tantoIds[taCnt]))
                        ).ToList();

                        if (curDatas?.Count == 0) continue;

                        #region ソート順
                        curDatas =
                            curDatas?
                                .OrderBy(s => pbSinYm ? s.SinDate : 0)
                                .ThenBy(s => pbKaId ? s.KaId : 0)
                                .ThenBy(s => pbTantoId ? s.TantoId : 0)
                                .ThenBy(s =>
                                    printConf.SortOpt1 == 1 ? "0" :
                                    printConf.SortOrder1 == 1 ? s.SinDate.ToString() :
                                    printConf.SortOrder1 == 2 ? s.PtNum.ToString().PadLeft(10, '0') + s.RaiinNo.ToString().PadLeft(10, '0') :
                                    printConf.SortOrder1 == 3 ? s.PtKanaName + s.RaiinNo.ToString().PadLeft(10, '0') :
                                    printConf.SortOrder1 == 4 ? s.SinKouiKbn + s.ItemCd : "0")
                                .ThenByDescending(s =>
                                    printConf.SortOpt1 == 0 ? "0" :
                                    printConf.SortOrder1 == 1 ? s.SinDate.ToString() :
                                    printConf.SortOrder1 == 2 ? s.PtNum.ToString().PadLeft(10, '0') :
                                    printConf.SortOrder1 == 3 ? s.PtKanaName + s.RaiinNo.ToString().PadLeft(10, '0') :
                                    printConf.SortOrder1 == 4 ? s.SinKouiKbn + s.ItemCd : "0")
                                .ThenBy(s =>
                                    printConf.SortOpt2 == 1 ? "0" :
                                    printConf.SortOrder2 == 1 ? s.SinDate.ToString() :
                                    printConf.SortOrder2 == 2 ? s.PtNum.ToString().PadLeft(10, '0') + s.RaiinNo.ToString().PadLeft(10, '0') :
                                    printConf.SortOrder2 == 3 ? s.PtKanaName + s.RaiinNo.ToString().PadLeft(10, '0') :
                                    printConf.SortOrder2 == 4 ? s.SinKouiKbn + s.ItemCd : "0")
                                .ThenByDescending(s =>
                                    printConf.SortOpt2 == 0 ? "0" :
                                    printConf.SortOrder2 == 1 ? s.SinDate.ToString() :
                                    printConf.SortOrder2 == 2 ? s.PtNum.ToString().PadLeft(10, '0') + s.RaiinNo.ToString().PadLeft(10, '0') :
                                    printConf.SortOrder2 == 3 ? s.PtKanaName + s.RaiinNo.ToString().PadLeft(10, '0') :
                                    printConf.SortOrder2 == 4 ? s.SinKouiKbn + s.ItemCd : "0")
                                .ThenBy(s =>
                                    printConf.SortOpt3 == 1 ? "0" :
                                    printConf.SortOrder3 == 1 ? s.SinDate.ToString() :
                                    printConf.SortOrder3 == 2 ? s.PtNum.ToString().PadLeft(10, '0') + s.RaiinNo.ToString().PadLeft(10, '0') :
                                    printConf.SortOrder3 == 3 ? s.PtKanaName + s.RaiinNo.ToString().PadLeft(10, '0') :
                                    printConf.SortOrder3 == 4 ? s.SinKouiKbn + s.ItemCd : "0")
                                .ThenByDescending(s =>
                                    printConf.SortOpt3 == 0 ? "0" :
                                    printConf.SortOrder3 == 1 ? s.SinDate.ToString() :
                                    printConf.SortOrder3 == 2 ? s.PtNum.ToString().PadLeft(10, '0') + s.RaiinNo.ToString().PadLeft(10, '0') :
                                    printConf.SortOrder3 == 3 ? s.PtKanaName + s.RaiinNo.ToString().PadLeft(10, '0') :
                                    printConf.SortOrder3 == 4 ? s.SinKouiKbn + s.ItemCd : "0")
                                .ThenBy(s => s.ItemName)
                                .ThenBy(s => s.ItemCd)
                                .ToList();
                        #endregion

                        int preSinDate = 0;
                        long prePtNum = 0;
                        long preRaiinNo = 0;
                        string preSinKouiKbn = string.Empty;
                        string preItemCd = string.Empty;
                        string preItemName = string.Empty;
                        foreach (var curData in (curDatas ?? new()))
                        {
                            CoSta3050PrintData printData = new CoSta3050PrintData();

                            printData.SinDate = curData.SinDate;
                            printData.SinDateS = CIUtil.SDateToShowSDate(curData.SinDate);
                            printData.PtNum = curData.PtNum;
                            printData.PtKanaName = curData.PtKanaName;
                            printData.PtName = curData.PtName;
                            printData.SexCd = curData.Sex;
                            printData.BirthDay = curData.BirthDay;
                            printData.SyosaisinCd = curData.SyosaisinKbn;
                            printData.KaId = curData.KaId;
                            printData.KaSname = curData.KaSname;
                            printData.TantoId = curData.TantoId;
                            printData.TantoSname = curData.TantoSname;
                            printData.SinId = curData.SinId;
                            printData.SinKouiKbn = curData.SinKouiKbn;
                            printData.ItemCd = curData.ItemCd;
                            printData.ItemName = curData.ItemName;
                            printData.Ten = curData.Ten.ToString("#,0.00");
                            printData.TenUnit = curData.Ten == 0 ? "-" : curData.EntenKbn == 0 ? "点" : "円";
                            printData.Suryo = curData.Suryo.ToString("#,0.00");
                            printData.UnitName = curData.UnitName;
                            printData.Count = curData.Count.ToString();
                            printData.TotalSuryo = curData.TotalSuryo.ToString("#,0.00");
                            printData.Money = curData.Money.ToString("#,0");
                            printData.HokenSbt = curData.HokenSbt;
                            printData.InoutKbn = curData.InoutKbn;
                            printData.MadokuKbn = curData.MadokuKbn;
                            printData.KouseisinKbn = curData.KouseisinKbn;
                            printData.KohatuKbn = curData.KohatuKbn;
                            printData.IsAdopted = curData.IsAdopted;
                            printData.HokenPid = curData.HokenPid;
                            printData.Kohi1Houbetu = curData.Kohi1Houbetu;
                            printData.Kohi2Houbetu = curData.Kohi2Houbetu;
                            printData.Kohi3Houbetu = curData.Kohi3Houbetu;
                            printData.Kohi4Houbetu = curData.Kohi4Houbetu;

                            #region 印刷の場合、前の行と同じ値のカラムは省略
                            if ((outputFileType != CoFileType.Csv || coFileType != CoFileType.Csv) && printDatas.Count % maxRow != 0)
                            {
                                int[] sortOrders = { printConf.SortOrder1, printConf.SortOrder2, printConf.SortOrder3 };
                                bool preBlank = true;

                                foreach (var sortOrder in sortOrders)
                                {
                                    bool curBlank = false;

                                    switch (sortOrder)
                                    {
                                        case 1:  //診療日
                                            if (curData.SinDate == preSinDate && preBlank)
                                            {
                                                printData.SinDateS = string.Empty;
                                                curBlank = true;
                                                if (curData.RaiinNo == preRaiinNo)
                                                {
                                                    printData.SyosaisinCd = null;
                                                }
                                            }
                                            break;
                                        case 2:  //患者番号
                                        case 3:  //氏名
                                            if (curData.PtNum == prePtNum && preBlank)
                                            {
                                                printData.PtNum = 0;
                                                printData.PtKanaName = string.Empty;
                                                printData.PtName = string.Empty;
                                                printData.SexCd = 0;
                                                printData.BirthDay = 0;
                                                curBlank = true;
                                            }
                                            break;
                                        case 4:  //項目種別
                                            if (curData.SinKouiKbn == preSinKouiKbn && preBlank)
                                            {
                                                printData.SinKouiKbn = string.Empty;
                                                curBlank = true;
                                            }
                                            if (curData.ItemCd == preItemCd && curData.ItemName == preItemName && preBlank)
                                            {
                                                printData.ItemCd = string.Empty;
                                                printData.ItemName = string.Empty;
                                            }
                                            break;
                                    }
                                    preBlank = curBlank;
                                }
                            }
                            #endregion

                            printDatas.Add(printData);

                            preSinDate = curData.SinDate;
                            prePtNum = curData.PtNum;
                            preRaiinNo = curData.RaiinNo;
                            preSinKouiKbn = curData.SinKouiKbn;
                            preItemCd = curData.ItemCd;
                            preItemName = curData.ItemName;
                        }

                        //小計
                        if (pbSinYm || pbKaId || pbTantoId)
                        {
                            //空行を追加
                            printDatas.Add(new CoSta3050PrintData(RowType.Brank));
                            printDatas.Add(new CoSta3050PrintData(RowType.Brank));

                            CoSta3050PrintData printData = new CoSta3050PrintData();

                            printData.RowType = RowType.Total;
                            printData.TotalCaption = string.Format(
                                "◆小計    実人数 ({0}人)    日数({1}日)",
                                curDatas?.GroupBy(s => s.PtNum).Count().ToString("#,0"),
                                curDatas?.GroupBy(s => s.SinDate).Count().ToString("#,0")
                            );
                            printData.TotalSuryo = (curDatas ?? new()).Sum(s => s.TotalSuryo).ToString("#,0.00");
                            printData.Money = (curDatas ?? new()).Sum(s => s.Money).ToString("#,0");

                            printDatas.Add(printData);

                            if (
                                (pbSinYm && (ymCnt + 1 <= sinYms?.Count - 1)) ||
                                (pbKaId && (kaCnt + 1 <= kaIds?.Count - 1)) ||
                                (pbTantoId && (taCnt + 1 <= tantoIds?.Count - 1))
                            )
                            {
                                //改ページ
                                for (int i = printDatas.Count; i % maxRow != 0; i++)
                                {
                                    //空行を追加
                                    printDatas.Add(new CoSta3050PrintData(RowType.Brank));
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
                                string wrkYm = CIUtil.Copy(CIUtil.SDateToShowSWDate((curDatas?.First().SinYm ?? 0) * 100 + 1, 0, 1, 1), 1, 13);
                                headerL1.Add(wrkYm + "度");
                            }
                            //改ページ条件
                            List<string> wrkHeaders = new List<string>();
                            if (pbKaId) wrkHeaders.Add(curDatas?.First().KaSname ?? string.Empty);
                            if (pbTantoId) wrkHeaders.Add(curDatas?.First().TantoSname ?? string.Empty);

                            if (wrkHeaders.Count >= 1) headerL2.Add(string.Join("／", wrkHeaders));
                        }
                        totalRow += rowCount;
                    }
                }
            }

            //空行を追加
            printDatas.Add(new CoSta3050PrintData(RowType.Brank));
            printDatas.Add(new CoSta3050PrintData(RowType.Brank));

            //合計
            CoSta3050PrintData totalData = new CoSta3050PrintData();

            totalData.RowType = RowType.Total;
            totalData.TotalCaption = string.Format(
                "◆合計    実人数 ({0}人)    日数({1}日)",
                sinKouis?.GroupBy(s => s.PtNum).Count().ToString("#,0"),
                sinKouis?.GroupBy(s => s.SinDate).Count().ToString("#,0")
            );
            totalData.TotalSuryo = (sinKouis ?? new()).Sum(s => s.TotalSuryo).ToString("#,0.00");
            totalData.Money = (sinKouis ?? new()).Sum(s => s.Money).ToString("#,0");
            printDatas.Add(totalData);
        }

        //データ取得
        sinKouis = _finder.GetSinKouis(hpId, printConf);
        if ((sinKouis?.Count ?? 0) == 0) return false;

        hpInf = _finder.GetHpInf(hpId, sinKouis?.FirstOrDefault()?.SinDate ?? 0);

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
        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3050, fileName, new());
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

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta3050, fileName, fieldInputList);
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? maxRow;
    }

    public CommonExcelReportingModel ExportCsv(CoSta3050PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType)
    {
        this.printConf = printConf;
        string fileName = menuName + "_" + monthFrom + "_" + monthTo;
        List<string> retDatas = new List<string>();
        this.coFileType = coFileType;

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
        retDatas.Add("\"" + string.Join("\",\"", wrkTitles) + "\"");
        if (isPutColName)
        {
            retDatas.Add("\"" + string.Join("\",\"", wrkColumns) + "\"");
        }

        //データ
        int rowOutputed = 0;
        foreach (var csvData in csvDatas)
        {
            retDatas.Add(RecordData(csvData));
            rowOutputed++;
        }

        string RecordData(CoSta3050PrintData csvData)
        {
            List<string> colDatas = new List<string>();

            foreach (var column in putCurColumns)
            {
                var value = typeof(CoSta3050PrintData).GetProperty(column.CsvColName)?.GetValue(csvData);
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
