using Helper.Common;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta2021.DB;
using Reporting.Statistics.Sta2021.Mapper;
using Reporting.Statistics.Sta2021.Models;
using System.ComponentModel;
using System.Globalization;

namespace Reporting.Statistics.Sta2021.Service
{
    public class Sta2021CoReportService : ISta2021CoReportService
    {
        #region Constant
        private int HpId;
        private int maxRow = 21;
        private int _currentPage;
        private bool _hasNextPage;
        private string _rowCountFieldName = string.Empty;
        private int _colCountSinYM;
        int prtIndex = 0;
        int colIndex = 0;
        private List<string> _objectRseList;
        private CoSta2021PrintConf _printConf;
        private CoFileType? coFileType;
        private readonly Dictionary<string, string> _singleFieldData = new Dictionary<string, string>();
        private readonly Dictionary<string, bool> _visibleFieldData = new Dictionary<string, bool>();
        private readonly Dictionary<string, string> _extralData = new Dictionary<string, string>();
        private readonly List<Dictionary<string, CellModel>> _tableFieldData = new List<Dictionary<string, CellModel>>();

        private readonly List<PutColumn> csvTotalColumns = new List<PutColumn>
        {
            new PutColumn("RowType", "明細区分"),
            new PutColumn("TotalCaption", "合計行")
        };

        private readonly List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("KaId", "診療科ID"),
            new PutColumn("KaSname", "診療科"),
            new PutColumn("TantoId", "担当医ID"),
            new PutColumn("TantoSname", "担当医"),
            new PutColumn("SinId", "レセプト識別"),
            new PutColumn("SinKouiKbn", "項目種別"),
            new PutColumn("SinKouiKbnName", "項目種別名称"),
            new PutColumn("ItemCd", "診療行為コード"),
            new PutColumn("ItemName", "診療行為名称"),
            new PutColumn("Ten", "単価"),
            new PutColumn("TenUnit", "単価(単位)"),
            new PutColumn("Suryo", "数量"),
            new PutColumn("UnitName", "単位"),
            new PutColumn("SinYmValues", "診療年月データ"),
            new PutColumn("InoutKbn", "院内院外区分", false),
            new PutColumn("MadokuKbn", "麻毒区分", false),
            new PutColumn("MadokuKbnSname", "麻毒区分略称", false),
            new PutColumn("MadokuKbnName", "麻毒区分名称", false),
            new PutColumn("KouseisinKbn", "向精神薬区分", false),
            new PutColumn("KouseisinKbnSname", "向精神薬区分略称", false),
            new PutColumn("KouseisinKbnName", "向精神薬区分名称", false),
            new PutColumn("KohatuKbn", "後発医薬品区分", false),
            new PutColumn("KohatuKbnName", "後発医薬品区分名称", false),
            new PutColumn("IsAdopted", "採用区分", false)
        };
        #endregion

        #region Private properties
        /// <summary>
        /// Finder
        /// </summary>
        private readonly ICoSta2021Finder _staFinder;
        private readonly IReadRseReportFileService _readRseReportFileService;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoSta2021PrintData> printDatas;
        private List<string> headerL1;
        private List<string> headerL2;
        private List<CoSinKouiModel> sinKouis;
        private CoHpInfModel hpInf;
        private readonly List<PutColumn> putCurColumns = new();

        #endregion


        public Sta2021CoReportService(ICoSta2021Finder staFinder, IReadRseReportFileService readRseReportFileService)
        {
            _staFinder = staFinder;
            _readRseReportFileService = readRseReportFileService;
            _objectRseList = new();
            _printConf = new();
            printDatas = new();
            headerL1 = new();
            headerL2 = new();
            sinKouis = new();
            hpInf = new();
        }

        public CommonReportingRequestModel GetSta2021ReportingData(CoSta2021PrintConf printConf, int hpId)
        {
            try
            {
                HpId = hpId;
                _printConf = printConf;
                // get data to print
                GetFieldNameList();
                GetColRowCount();
                if (GetData())
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

                return new Sta2021Mapper(_singleFieldData, _tableFieldData, _extralData, _rowCountFieldName, _visibleFieldData).GetData();
            }
            finally
            {
                _staFinder.ReleaseResource();
            }
        }

        #region Get Data
        private bool GetData()
        {
            void MakePrintData()
            {
                printDatas = new List<CoSta2021PrintData>();
                headerL1 = new List<string>();
                headerL2 = new List<string>();
                int totalRow = 0;

                //改ページ条件
                bool pbKaId = new int[] { _printConf.PageBreak1, _printConf.PageBreak2 }.Contains(1);
                bool pbTantoId = new int[] { _printConf.PageBreak1, _printConf.PageBreak2 }.Contains(2);

                #region 診療年月のリストを作成
                int startYm = _printConf.StartSinYm <= 0 ? sinKouis!.Min(s => s.SinYm) : _printConf.StartSinYm;
                int endYm = _printConf.EndSinYm <= 0 ? sinKouis!.Max(s => s.SinYm) : _printConf.EndSinYm;

                List<int> sinYms = new List<int>();
                while (startYm <= endYm)
                {
                    sinYms.Add(startYm);

                    var wrkDt = CIUtil.SDateToDateTime(startYm * 100 + 1);

                    if (wrkDt != null)
                    {
                        var wrkDt2 = (DateTime)wrkDt;
                        startYm = CIUtil.DateTimeToInt(wrkDt2.AddMonths(1), "yyyyMM");
                    }
                    else
                    {
                        startYm++;
                    }
                }
                #endregion

                var kaIds = sinKouis!.GroupBy(s => s.KaId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                for (int kaCnt = 0; (pbKaId && kaCnt <= kaIds.Count - 1) || kaCnt == 0; kaCnt++)
                {
                    var tantoIds = sinKouis!.GroupBy(s => s.TantoId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                    for (int taCnt = 0; (pbTantoId && taCnt <= tantoIds.Count - 1) || taCnt == 0; taCnt++)
                    {
                        var curDatas = sinKouis!.Where(s =>
                            (!pbKaId || s.KaId == kaIds[kaCnt]) &&
                            (!pbTantoId || s.TantoId == tantoIds[taCnt])
                        ).ToList();

                        if (curDatas.Count == 0) continue;

                        double totalSuryo = 0;
                        //第１ソート
                        int sortOrder1 =
                            _printConf.SortOrder1 >= 1 ? _printConf.SortOrder1 :
                            _printConf.SortOrder2 >= 1 ? _printConf.SortOrder2 :
                            _printConf.SortOrder3 >= 1 ? _printConf.SortOrder3 : 0;
                        int sortOpt1 =
                            _printConf.SortOrder1 >= 1 ? _printConf.SortOpt1 :
                            _printConf.SortOrder2 >= 1 ? _printConf.SortOpt2 :
                            _printConf.SortOrder3 >= 1 ? _printConf.SortOpt3 : 0;

                        //小計毎のリスト
                        List<string>? grpIds = null;
                        switch (sortOrder1)
                        {
                            case 1:
                                grpIds = curDatas
                                    .GroupBy(s => s.SinKouiKbn)
                                    .OrderBy(s => sortOpt1 == 1 ? "0" : s.Key)
                                    .OrderByDescending(s => sortOpt1 == 0 ? "0" : s.Key)
                                    .Select(s => s.Key)
                                    .ToList();
                                break;
                            case 2:
                                grpIds = curDatas
                                    .GroupBy(s => s.SinId)
                                    .OrderBy(s => sortOpt1 == 1 ? "0" : (s.Key.Equals("1x") ? "10" : s.Key).PadLeft(3, '0'))
                                    .OrderByDescending(s => sortOpt1 == 0 ? "0" : (s.Key.Equals("1x") ? "10" : s.Key).PadLeft(3, '0'))
                                    .Select(s => s.Key)
                                    .ToList();
                                break;
                        }

                        //小計毎の集計
                        for (int grpCnt = 0; (grpIds != null && grpCnt <= grpIds.Count - 1) || grpCnt == 0; grpCnt++)
                        {
                            var grpDatas = curDatas.Where(s =>
                                (grpIds == null || (sortOrder1 == 1 ? s.SinKouiKbn == grpIds[grpCnt] : s.SinId == grpIds[grpCnt]))
                            ).ToList();

                            if (grpDatas.Count == 0) continue;

                            bool groupSinId = sortOrder1 == 2 || coFileType == CoFileType.Csv;

                            //項目単位のリスト
                            var itemDatas =
                                grpDatas
                                    .GroupBy(s => new { SinId = groupSinId ? s.SinId : string.Empty, s.ItemCdCmt, s.ItemCd, s.Ten, s.Suryo })
                                    .Select(s =>
                                        new
                                        {
                                            s.Key.SinId,
                                            s.Key.ItemCdCmt,
                                            s.Key.ItemCd,
                                            s.Key.Ten,
                                            s.Key.Suryo,
                                            ItemName = s.Max(x => x.ItemName),
                                            //Suryo = s.Sum(x => x.Suryo),
                                            Money = CIUtil.RoundInt(s.Sum(x => x.Money), 0)
                                        }
                                    )
                                    .OrderBy(s => _printConf.SortOpt1 == 0 && _printConf.SortOrder1 == 3 ? s.ItemName : string.Empty)
                                    .ThenBy(s => _printConf.SortOpt1 == 0 && _printConf.SortOrder1 == 4 ? s.Money : 0)
                                    .ThenBy(s => _printConf.SortOpt1 == 0 && _printConf.SortOrder1 == 5 ? s.Suryo : 0)
                                    .ThenByDescending(s => _printConf.SortOpt1 == 1 && _printConf.SortOrder1 == 3 ? s.ItemName : string.Empty)
                                    .ThenByDescending(s => _printConf.SortOpt1 == 1 && _printConf.SortOrder1 == 4 ? s.Money : 0)
                                    .ThenByDescending(s => _printConf.SortOpt1 == 1 && _printConf.SortOrder1 == 5 ? s.Suryo : 0)
                                    .ThenBy(s => _printConf.SortOpt2 == 0 && _printConf.SortOrder2 == 3 ? s.ItemName : string.Empty)
                                    .ThenBy(s => _printConf.SortOpt2 == 0 && _printConf.SortOrder2 == 4 ? s.Money : 0)
                                    .ThenBy(s => _printConf.SortOpt2 == 0 && _printConf.SortOrder2 == 5 ? s.Suryo : 0)
                                    .ThenByDescending(s => _printConf.SortOpt2 == 1 && _printConf.SortOrder2 == 3 ? s.ItemName : string.Empty)
                                    .ThenByDescending(s => _printConf.SortOpt2 == 1 && _printConf.SortOrder2 == 4 ? s.Money : 0)
                                    .ThenByDescending(s => _printConf.SortOpt2 == 1 && _printConf.SortOrder2 == 5 ? s.Suryo : 0)
                                    .ThenBy(s => _printConf.SortOpt3 == 0 && _printConf.SortOrder3 == 3 ? s.ItemName : string.Empty)
                                    .ThenBy(s => _printConf.SortOpt3 == 0 && _printConf.SortOrder3 == 4 ? s.Money : 0)
                                    .ThenBy(s => _printConf.SortOpt3 == 0 && _printConf.SortOrder3 == 5 ? s.Suryo : 0)
                                    .ThenByDescending(s => _printConf.SortOpt3 == 1 && _printConf.SortOrder3 == 3 ? s.ItemName : string.Empty)
                                    .ThenByDescending(s => _printConf.SortOpt3 == 1 && _printConf.SortOrder3 == 4 ? s.Money : 0)
                                    .ThenByDescending(s => _printConf.SortOpt3 == 1 && _printConf.SortOrder3 == 5 ? s.Suryo : 0)
                                    .ThenBy(s => s.ItemName)
                                    .ThenBy(s => s.ItemCd)
                                    .ThenBy(s => s.Suryo.ToString("#,0.00").PadLeft(12, '0'))
                                    .ToList();

                            double grpTotalSuryo = 0;

                            foreach (var itemData in itemDatas)
                            {
                                var wrkDatas = grpDatas.Where(s =>
                                    (!groupSinId || s.SinId == itemData.SinId) &&
                                    s.ItemCdCmt == itemData.ItemCdCmt &&
                                    s.ItemCd == itemData.ItemCd &&
                                    s.Ten == itemData.Ten &&
                                    s.Suryo == itemData.Suryo
                                ).ToList();

                                CoSta2021PrintData printData = new CoSta2021PrintData();

                                printData.KaId = (pbKaId || kaIds.Count == 1) ? wrkDatas.First().KaId.ToString() : string.Empty;
                                printData.KaSname = (pbKaId || kaIds.Count == 1) ? wrkDatas.First().KaSname ?? string.Empty : string.Empty;
                                printData.TantoId = (pbTantoId || tantoIds.Count == 1) ? wrkDatas.First().TantoId.ToString() ?? string.Empty : string.Empty;
                                printData.TantoSname = (pbTantoId || tantoIds.Count == 1) ? wrkDatas.First().TantoSname ?? string.Empty : string.Empty;
                                printData.SinId = wrkDatas.First().SinId;
                                printData.SinKouiKbn = wrkDatas.First().SinKouiKbn;
                                printData.ItemCd = itemData.ItemCd;
                                printData.ItemName = wrkDatas.First().ItemName;
                                printData.Ten = itemData.Ten.ToString("#,0.00");
                                printData.TenUnit = wrkDatas.First().EntenKbn == 0 ? "点" : "円";
                                printData.Suryo = itemData.Suryo.ToString("#,0.00");
                                printData.UnitName = wrkDatas.First().UnitName;

                                printData.MadokuKbn = wrkDatas.First().MadokuKbn;
                                printData.KouseisinKbn = wrkDatas.First().KouseisinKbn;
                                printData.KohatuKbn = wrkDatas.First().KohatuKbn;
                                printData.IsAdopted = wrkDatas.First().IsAdopted;

                                foreach (var sinYm in sinYms)
                                {
                                    printData.SinYm.Add(sinYm);
                                    printData.SinYmS.Add(CIUtil.SMonthToShowSMonth(sinYm));
                                    printData.Counts.Add(wrkDatas.Where(s => s.SinYm == sinYm).Sum(s => s.Count).ToString("#,0"));
                                    printData.Moneys.Add(CIUtil.RoundInt(wrkDatas.Where(s => s.SinYm == sinYm).Sum(s => s.Money), 0).ToString("#,0"));
                                }

                                printDatas.Add(printData);

                                grpTotalSuryo += itemData.Suryo;
                                totalSuryo += itemData.Suryo;
                            }

                            //小計
                            if (grpIds != null)
                            {
                                CoSta2021PrintData printData = new CoSta2021PrintData();

                                printData.RowType = RowType.Total;
                                string sortOrder1_2 = sortOrder1 == 2 ? string.Format("◆{0} {1}計", grpDatas.First().SinId, grpDatas.First().SinIdName) ?? string.Empty : string.Empty;
                                printData.TotalCaption =
                                    sortOrder1 == 1 ? string.Format("◆{0} {1}計", grpDatas.First().SinKouiKbn, grpDatas.First().SinKouiKbnName) :
                                    sortOrder1_2;
                                printData.Suryo = grpTotalSuryo.ToString("#,0.00");

                                foreach (var sinYm in sinYms)
                                {
                                    printData.SinYm.Add(sinYm);
                                    printData.SinYmS.Add(CIUtil.SMonthToShowSMonth(sinYm));
                                    printData.Counts.Add(grpDatas.Where(s => s.SinYm == sinYm).Sum(s => s.Count).ToString("#,0"));
                                    printData.Moneys.Add(CIUtil.RoundInt(grpDatas.Where(s => s.SinYm == sinYm).Sum(s => s.Money), 0).ToString("#,0"));
                                }

                                printDatas.Add(printData);
                                if (printDatas.Count % maxRow != 0)
                                {
                                    printDatas.Add(new CoSta2021PrintData(RowType.Brank));
                                    foreach (var sinYm in sinYms)
                                    {
                                        printDatas.Last().SinYm.Add(sinYm);
                                        printDatas.Last().SinYmS.Add(CIUtil.SMonthToShowSMonth(sinYm));
                                        printDatas.Last().Counts.Add("0");
                                        printDatas.Last().Moneys.Add("0");
                                    }
                                }
                            }
                        }

                        //合計
                        CoSta2021PrintData totalData = new CoSta2021PrintData();

                        totalData.RowType = RowType.Total;
                        totalData.TotalCaption = "◆合計";
                        totalData.Suryo = totalSuryo.ToString("#,0.00");

                        foreach (var sinYm in sinYms)
                        {
                            totalData.SinYm.Add(sinYm);
                            totalData.SinYmS.Add(CIUtil.SMonthToShowSMonth(sinYm));
                            totalData.Counts.Add(curDatas.Where(s => s.SinYm == sinYm).Sum(s => s.Count).ToString("#,0"));
                            totalData.Moneys.Add(CIUtil.RoundInt(curDatas.Where(s => s.SinYm == sinYm).Sum(s => s.Money), 0).ToString("#,0"));
                        }

                        printDatas.Add(totalData);

                        //改ページ
                        for (int i = printDatas.Count; i % maxRow != 0; i++)
                        {
                            //空行を追加
                            printDatas.Add(new CoSta2021PrintData(RowType.Brank));
                            foreach (var sinYm in sinYms)
                            {
                                printDatas.Last().SinYm.Add(sinYm);
                                printDatas.Last().SinYmS.Add(CIUtil.SMonthToShowSMonth(sinYm));
                                printDatas.Last().Counts.Add("0");
                                printDatas.Last().Moneys.Add("0");
                            }
                        }

                        //ヘッダー情報
                        int rowCount = printDatas.Count - totalRow;
                        int pageCount = (int)Math.Ceiling((double)(rowCount) / maxRow);
                        for (int i = 0; i < pageCount; i++)
                        {
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

            //データ取得
            sinKouis = _staFinder.GetSinKouis(HpId, _printConf);
            if ((sinKouis?.Count ?? 0) == 0) return false;

            hpInf = _staFinder.GetHpInf(HpId, sinKouis!.First().SinYm);

            //印刷用データの作成
            MakePrintData();

            return printDatas.Count > 0;
        }
        #endregion

        #region Update Draw Form
        private void UpdateDrawForm()
        {
            _hasNextPage = true;

            #region SubMethod

            #region Header
            void UpdateFormHeader()
            {
                //タイトル
                SetFieldData("Title", _printConf.ReportName);
                //医療機関名
                _extralData.Add("HeaderR_0_0_" + _currentPage, hpInf.HpName);
                //作成日時
                _extralData.Add("HeaderR_0_1_" + _currentPage, CIUtil.SDateToShowSWDate(
                    CIUtil.ShowSDateToSDate(CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd")), 0, 1
                ) + CIUtil.GetJapanDateTimeNow().ToString(" HH:mm") + "作成");
                //ページ数
                int colCount = (int)Math.Ceiling((double)printDatas.First().SinYm.Count / _colCountSinYM);
                int totalPage = (int)Math.Ceiling((double)printDatas.Count / maxRow) * colCount;
                _extralData.Add("HeaderR_0_2_" + _currentPage, _currentPage + " / " + totalPage);

                int wrkIndex = (_currentPage - 1) / colCount;
                //請求年月
                _extralData.Add("HeaderL_0_1_" + _currentPage, headerL1.Count - 1 >= wrkIndex ? headerL1[wrkIndex] : "");
                //改ページ条件
                _extralData.Add("HeaderL_0_2_" + _currentPage, headerL2.Count - 1 >= wrkIndex ? headerL2[wrkIndex] : "");

                //期間
                SetFieldData("Range",
                    string.Format(
                        "期間: {0} ～ {1}",
                        CIUtil.SDateToShowSWDate(_printConf.StartSinYm * 100 + 1, 0, 1).Substring(0, 12),
                        CIUtil.SDateToShowSWDate(_printConf.EndSinYm * 100 + 1, 0, 1).Substring(0, 12)
                    )
                );
            }
            #endregion

            #region Body
            void UpdateFormBody()
            {
                int wrkIndex = prtIndex;
                int wrkColIndex = colIndex;
                int lineCount = 0;

                //存在しているフィールドに絞り込み
                var existsCols = putColumns.Where(p => _objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    Dictionary<string, CellModel> data = new();
                    var printData = printDatas[wrkIndex];
                    string baseListName = "";

                    //明細データ出力
                    foreach (var colName in existsCols)
                    {
                        if (baseListName == "" && _objectRseList.Contains(colName))
                        {
                            baseListName = colName;
                        }

                        var value = typeof(CoSta2021PrintData).GetProperty(colName)?.GetValue(printData);
                        if (wrkIndex >= 1 && rowNo >= 1)
                        {
                            bool sameItem = true;
                            if (new string[] { "ItemName", "Ten", "TenUnit" }.Contains(colName))
                            {
                                var itemCd = printData.ItemCd ?? string.Empty;
                                sameItem = itemCd.CompareTo(printDatas[wrkIndex - 1].ItemCd) == 0;
                            }

                            //前の行と同じ場合は記載を省略する
                            if (new string[] { "SinKouiKbn", "ItemCd", "ItemName", "Ten" }.Contains(colName))
                            {
                                var preValue = typeof(CoSta2021PrintData).GetProperty(colName)?.GetValue(printDatas[wrkIndex - 1]);
                                if ((value == null ? "" : value.ToString() ?? string.Empty).CompareTo(preValue == null ? "" : preValue.ToString()) == 0 && sameItem) continue;
                            }
                            else if (colName == "TenUnit")
                            {
                                var tenValue = typeof(CoSta2021PrintData).GetProperty("Ten")?.GetValue(printDatas[wrkIndex]);
                                var preValue = typeof(CoSta2021PrintData).GetProperty("Ten")?.GetValue(printDatas[wrkIndex - 1]);
                                if ((tenValue == null ? "" : tenValue.ToString() ?? string.Empty).CompareTo(preValue == null ? "" : preValue.ToString()) == 0 && sameItem) continue;
                            }
                        }

                        AddListData(ref data, colName, value == null ? "" : value.ToString() ?? string.Empty);
                    }

                    //診療年月別データ
                    int subTotalCount = 0;
                    int subTotalMoney = 0;
                    wrkColIndex = colIndex;
                    for (short i = 0; i <= _colCountSinYM - 1 && wrkColIndex <= printData.SinYmS.Count - 1; i++)
                    {
                        if (rowNo == 0)
                        {
                            AddListData(ref data, "SinYm", printData.SinYmS[wrkColIndex]);
                            AddListData(ref data, "SinYm", "回数／金額");
                        }
                        AddListData(ref data, $"Count{i}", printData.Counts[wrkColIndex]);
                        AddListData(ref data, $"Money{i}", printData.Moneys[wrkColIndex]);

                        subTotalCount += int.Parse(printData.Counts[wrkColIndex] ?? "0", NumberStyles.Any);
                        subTotalMoney += int.Parse(printData.Moneys[wrkColIndex] ?? "0", NumberStyles.Any);

                        wrkColIndex++;
                    }

                    int totalCount = 0;
                    int totalMoney = 0;
                    for (short i = 0; i <= printData.SinYmS.Count - 1; i++)
                    {
                        totalCount += int.Parse(printData.Counts[i] ?? "0", NumberStyles.Any);
                        totalMoney += int.Parse(printData.Moneys[i] ?? "0", NumberStyles.Any);
                    }

                    //小計・合計
                    if (printData.RowType != RowType.Brank)
                    {
                        //小計
                        AddListData(ref data, "CountSubTotal", subTotalCount.ToString("#,0"));
                        AddListData(ref data, "MoneySubTotal", subTotalMoney.ToString("#,0"));

                        SetVisibleFieldData("TotalTitle1", false);
                        SetVisibleFieldData("TotalTitle2", false);
                        if (wrkColIndex == printDatas.First().SinYm.Count)
                        {
                            //合計
                            AddListData(ref data, "CountTotal", totalCount.ToString("#,0"));
                            AddListData(ref data, "MoneyTotal", totalMoney.ToString("#,0"));
                            SetVisibleFieldData("TotalTitle1", true);
                            SetVisibleFieldData("TotalTitle2", true);
                        }
                    }

                    //合計行キャプション
                    AddListData(ref data, "TotalCaption", printData.TotalCaption);

                    //区切り線を引く
                    lineCount = printData.RowType != RowType.Brank ? lineCount + 1 : lineCount;
                    string rowNoKey = rowNo + "_" + _currentPage;
                    _extralData.Add("lineCount_" + rowNoKey, lineCount.ToString());

                    if (!_extralData.ContainsKey("headerLine"))
                    {
                        _extralData.Add("headerLine", "true");
                    }
                    _extralData.Add("baseListName_" + rowNoKey, baseListName);
                    _extralData.Add("rowNo_" + rowNoKey, rowNo.ToString());

                    _tableFieldData.Add(data);
                    wrkIndex++;
                    if (wrkIndex >= printDatas.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }

                colIndex = wrkColIndex;
                if (colIndex >= printDatas.First().SinYm.Count || colIndex == 0)
                {
                    prtIndex = wrkIndex;
                    colIndex = 0;
                }
                else if (!_hasNextPage)
                {
                    _hasNextPage = true;
                }

            }
            #endregion

            #endregion
            UpdateFormHeader();
            UpdateFormBody();
        }
        #endregion

        #region get data java
        private void SetFieldData(string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
            {
                _singleFieldData.Add(field, value);
            }
        }

        private void SetVisibleFieldData(string field, bool value)
        {
            if (!string.IsNullOrEmpty(field) && !_visibleFieldData.ContainsKey(field))
            {
                _visibleFieldData.Add(field, value);
            }
        }

        private void AddListData(ref Dictionary<string, CellModel> dictionary, string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !dictionary.ContainsKey(field))
            {
                dictionary.Add(field, new CellModel(value));
            }
        }

        private void GetFieldNameList()
        {
            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta2021, "sta2021a.rse", new());
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            _objectRseList = javaOutputData.objectNames;
        }

        private void GetColRowCount()
        {
            _rowCountFieldName = putColumns.Find(p => _objectRseList.Contains(p.ColName)).ColName;
            List<ObjectCalculate> fieldInputList = new()
        {
            new ObjectCalculate(_rowCountFieldName, (int)CalculateTypeEnum.GetListRowCount),
            new ObjectCalculate("SinYm", (int)CalculateTypeEnum.GetListColCount)
        };

            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta2021, "sta2021a.rse", fieldInputList);
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == _rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? maxRow;
            _colCountSinYM = javaOutputData.responses?.FirstOrDefault(item => item.typeInt == (int)CalculateTypeEnum.GetListColCount)?.result ?? 0;
        }

        public CommonExcelReportingModel ExportCsv(CoSta2021PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType)
        {
            _printConf = printConf;
            HpId = hpId;
            this.coFileType = coFileType;
            string fileName = menuName + "_" + monthFrom + "_" + monthTo;
            List<string> retDatas = new List<string>();
            if (!GetData()) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

            //合計行
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
                if (wrkTitle == "診療年月データ")
                {
                    for (int i = 0; i <= csvDatas.First().SinYm.Count - 1; i++)
                    {
                        wrkCols.Add($"\"回数_{csvDatas.First().SinYm[i]}\"");
                        wrkCols.Add($"\"金額_{csvDatas.First().SinYm[i]}\"");
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
                    if (wrkColumn == "SinYmValues")
                    {
                        for (int i = 0; i <= csvDatas.First().SinYm.Count - 1; i++)
                        {
                            wrkCols.Add($"\"Counts{i}\"");
                            wrkCols.Add($"\"Moneys{i}\"");
                        }
                    }
                    else
                    {
                        wrkCols.Add("\"" + wrkColumn + "\"");
                    }
                }
                retDatas.Add(string.Join(",", wrkCols));
            }

            //データ
            int rowOutputed = 0;
            foreach (var csvData in csvDatas)
            {
                retDatas.Add(RecordData(csvData));
                rowOutputed++;
            }

            string RecordData(CoSta2021PrintData csvData)
            {
                List<string> colDatas = new List<string>();

                foreach (var column in putCurColumns)
                {
                    if (column.ColName == "SinYmValues")
                    {
                        for (int i = 0; i <= csvData.SinYm.Count - 1; i++)
                        {
                            colDatas.Add("\"" + csvData.Counts[i] + "\"");
                            colDatas.Add("\"" + csvData.Moneys[i] + "\"");
                        }
                    }
                    else
                    {
                        var value = typeof(CoSta2021PrintData).GetProperty(column.CsvColName)?.GetValue(csvData);
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
                }

                return string.Join(",", colDatas);
            }

            return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);
        }
        #endregion
    }
}
