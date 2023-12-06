using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta2020.DB;
using Reporting.Statistics.Sta2020.Mapper;
using Reporting.Statistics.Sta2020.Models;
using System.ComponentModel;

namespace Reporting.Statistics.Sta2020.Service
{
    public class Sta2020CoReportService : ISta2020CoReportService
    {
        #region Constant
        private int _maxRow = 43;

        private readonly List<PutColumn> csvTotalColumns = new List<PutColumn>
        {
            new PutColumn("RowType", "明細区分"),
            new PutColumn("TotalCaption", "合計行")
        };

        private readonly List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("SinYmFmt", "診療年月", false, "SinYm"),
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
            new PutColumn("Money", "金額"),
            new PutColumn("Rate", "全体比率"),
            new PutColumn("GrpRate", "グループ比率"),
            new PutColumn("MadokuKbn", "麻毒区分"),
            new PutColumn("MadokuKbnSname", "麻毒区分略称"),
            new PutColumn("MadokuKbnName", "麻毒区分名称"),
            new PutColumn("KouseisinKbn", "向精神薬区分"),
            new PutColumn("KouseisinKbnSname", "向精神薬区分略称"),
            new PutColumn("KouseisinKbnName", "向精神薬区分名称"),
            new PutColumn("KazeiKbn", "課税区分"),
            new PutColumn("KazeiKbnName", "課税区分名称"),
            new PutColumn("RaiinCount", "来院数"),
            new PutColumn("PtCount", "実人数")
        };
        #endregion

        #region Private properties

        /// <summary>
        /// Finder
        /// </summary>
        private readonly ICoSta2020Finder _staFinder;
        private readonly IReadRseReportFileService _readRseReportFileService;
        private CoSta2020PrintConf _printConf;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoSta2020PrintData> printDatas;
        private List<string> headerL1;
        private List<string> headerL2;
        private List<CoSinKouiModel> sinKouis;
        private CoHpInfModel hpInf;
        private readonly BackgroundWorker? _backgroundWorker = null;
        private int HpId;
        private int _currentPage;
        private List<string> _objectRseList = new();
        private bool _hasNextPage;
        private string _rowCountFieldName = string.Empty;

        private List<PutColumn> putCurColumns = new List<PutColumn>();

        private readonly Dictionary<string, string> _singleFieldData = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _extralData = new Dictionary<string, string>();
        private readonly List<Dictionary<string, CellModel>> _tableFieldData = new List<Dictionary<string, CellModel>>();

        public Sta2020CoReportService(ICoSta2020Finder staFinder, IReadRseReportFileService readRseReportFileService)
        {
            _staFinder = staFinder;
            _readRseReportFileService = readRseReportFileService;
        }
        #endregion

        public CommonReportingRequestModel GetSta2020ReportingData(CoSta2020PrintConf printConf, int hpId)
        {
            try
            {
                HpId = hpId;
                _printConf = printConf;
                // get data to print
                GetFieldNameList();
                GetRowCount();

                var getData = GetData();

                if (getData)
                {
                    _hasNextPage = true;

                    _currentPage = 1;
                    while (_hasNextPage && getData)
                    {
                        UpdateDrawForm();
                        _currentPage++;
                    }
                }

                return new Sta2020Mapper(_singleFieldData, _tableFieldData, _extralData, _rowCountFieldName).GetData();
            }
            finally
            {
                _staFinder.ReleaseResource();
            }
        }

        #region Get  Data
        private bool GetData()
        {
            void MakePrintData()
            {
                printDatas = new List<CoSta2020PrintData>();
                headerL1 = new List<string>();
                headerL2 = new List<string>();
                int totalRow = 0;

                //改ページ条件
                bool pbSinYm = new int[] { _printConf.PageBreak1, _printConf.PageBreak2, _printConf.PageBreak3 }.Contains(1);
                bool pbKaId = new int[] { _printConf.PageBreak1, _printConf.PageBreak2, _printConf.PageBreak3 }.Contains(2);
                bool pbTantoId = new int[] { _printConf.PageBreak1, _printConf.PageBreak2, _printConf.PageBreak3 }.Contains(3);

                var sinYms = sinKouis!.GroupBy(s => s.SinYm).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                for (int ymCnt = 0; (pbSinYm && ymCnt <= sinYms.Count - 1) || ymCnt == 0; ymCnt++)
                {
                    var kaIds = sinKouis!.GroupBy(s => s.KaId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                    for (int kaCnt = 0; (pbKaId && kaCnt <= kaIds.Count - 1) || kaCnt == 0; kaCnt++)
                    {
                        var tantoIds = sinKouis!.GroupBy(s => s.TantoId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                        for (int taCnt = 0; (pbTantoId && taCnt <= tantoIds.Count - 1) || taCnt == 0; taCnt++)
                        {
                            var curDatas = sinKouis!.Where(s =>
                                (!pbSinYm || s.SinYm == sinYms[ymCnt]) &&
                                (!pbKaId || s.KaId == kaIds[kaCnt]) &&
                                (!pbTantoId || s.TantoId == tantoIds[taCnt])
                            ).ToList();

                            if (curDatas.Count == 0) continue;

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

                                bool groupSinId = sortOrder1 == 2;

                                //項目単位のリスト
                                var itemDatas =
                                    grpDatas
                                        .GroupBy(s => new { SinId = groupSinId ? s.SinId : string.Empty, s.ItemCdCmt, s.ItemCd, s.Ten })
                                        .Select(s =>
                                            new
                                            {
                                                s.Key.SinId,
                                                s.Key.ItemCdCmt,
                                                s.Key.ItemCd,
                                                s.Key.Ten,
                                                ItemName = s.Max(x => x.ItemName),
                                                Suryo = s.Sum(x => x.Suryo),
                                                Money = s.Sum(x => x.Money)
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
                                        .ToList();

                                foreach (var itemData in itemDatas)
                                {
                                    var wrkDatas = grpDatas.Where(s => (!groupSinId || s.SinId == itemData.SinId) && s.ItemCdCmt == itemData.ItemCdCmt && s.ItemCd == itemData.ItemCd && s.Ten == itemData.Ten).ToList();

                                    CoSta2020PrintData printData = new();

                                    printData.SinYm = (pbSinYm || sinYms.Count == 1) ? wrkDatas.First().SinYm : 0;
                                    printData.KaId = (pbKaId || kaIds.Count == 1) ? wrkDatas.First()?.KaId.ToString() ?? string.Empty : string.Empty;
                                    printData.KaSname = (pbKaId || kaIds.Count == 1) ? wrkDatas.First()?.KaSname ?? string.Empty : string.Empty;
                                    printData.TantoId = (pbTantoId || tantoIds.Count == 1) ? wrkDatas.First()?.TantoId.ToString() ?? string.Empty : string.Empty;
                                    printData.TantoSname = (pbTantoId || tantoIds.Count == 1) ? wrkDatas.First()?.TantoSname ?? string.Empty : string.Empty;
                                    printData.SinId = wrkDatas.First().SinId;
                                    printData.SinKouiKbn = wrkDatas.First().SinKouiKbn;
                                    printData.ItemCd = itemData.ItemCd;
                                    printData.ItemName = wrkDatas.First().ItemName;
                                    printData.Ten = itemData.Ten.ToString("#,0.00");
                                    printData.TenUnit = wrkDatas.First().EntenKbn == 0 ? "点" : "円";
                                    printData.Suryo = itemData.Suryo.ToString("#,0.00");
                                    printData.Money = itemData.Money.ToString("#,0");
                                    //全体比率
                                    printData.Rate =
                                        (
                                            curDatas.Sum(s => s.Money) == 0 ? 0 :
                                                Math.Round(
                                                    (double)itemData.Money / curDatas.Sum(s => s.Money) * 100,
                                                    2, MidpointRounding.AwayFromZero
                                                )
                                        ).ToString("#,0.00");
                                    //グループ比率
                                    if (grpIds != null)
                                    {
                                        printData.GrpRate =
                                            (
                                                grpDatas.Sum(s => s.Money) == 0 ? 0 :
                                                    Math.Round(
                                                        (double)itemData.Money / grpDatas.Sum(s => s.Money) * 100,
                                                        2, MidpointRounding.AwayFromZero
                                                    )
                                            ).ToString("#,0.00");
                                    }
                                    printData.MadokuKbn = wrkDatas.First().MadokuKbn;
                                    printData.KouseisinKbn = wrkDatas.First().KouseisinKbn;
                                    printData.KazeiKbn = wrkDatas.First().KazeiKbn;
                                    printData.RaiinCount = wrkDatas.GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                                    printData.PtCount = wrkDatas.GroupBy(s => s.PtId).Count().ToString("#,0");

                                    printDatas.Add(printData);
                                }

                                //小計
                                if (grpIds != null)
                                {
                                    CoSta2020PrintData printData = new CoSta2020PrintData();

                                    printData.RowType = RowType.Total;
                                    printData.TotalCaption =
                                        sortOrder1 == 1 ? string.Format("◆{0} {1}計", grpDatas.First().SinKouiKbn, grpDatas.First().SinKouiKbnName) :
                                        sortOrder1 == 2 ? string.Format("◆{0} {1}計", grpDatas.First().SinId ?? string.Empty, grpDatas.First().SinIdName ?? string.Empty) : string.Empty;
                                    printData.Ten = "-";
                                    printData.Suryo = grpDatas.Sum(s => s.Suryo).ToString("#,0.00");
                                    printData.Money = grpDatas.Sum(s => s.Money).ToString("#,0");
                                    //全体比率
                                    printData.Rate =
                                        (
                                            curDatas.Sum(s => s.Money) == 0 ? 0 :
                                                Math.Round(
                                                    (double)grpDatas.Sum(s => s.Money) / curDatas.Sum(s => s.Money) * 100,
                                                    2, MidpointRounding.AwayFromZero
                                                )
                                        ).ToString("#,0.00");
                                    //グループ比率
                                    printData.GrpRate = grpDatas.Sum(s => s.Money) == 0 ? "0.00" : "100.00";

                                    printData.RaiinCount = grpDatas.GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                                    printData.PtCount = grpDatas.GroupBy(s => s.PtId).Count().ToString("#,0");

                                    printDatas.Add(printData);
                                    if (printDatas.Count % _maxRow != 0)
                                    {
                                        printDatas.Add(new CoSta2020PrintData(RowType.Brank));
                                    }
                                }
                            }

                            //合計
                            printDatas.Add(new CoSta2020PrintData(RowType.Brank));

                            CoSta2020PrintData totalData = new CoSta2020PrintData();

                            totalData.RowType = RowType.Total;
                            totalData.TotalCaption = "◆合計";
                            totalData.Ten = "-";
                            totalData.Suryo = curDatas.Sum(s => s.Suryo).ToString("#,0.00");
                            totalData.Money = curDatas.Sum(s => s.Money).ToString("#,0");
                            //全体比率
                            totalData.Rate = "100.00";
                            //グループ比率
                            totalData.GrpRate = "-";

                            totalData.RaiinCount = curDatas.GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                            totalData.PtCount = curDatas.GroupBy(s => s.PtId).Count().ToString("#,0");

                            printDatas.Add(totalData);

                            //改ページ
                            for (int i = printDatas.Count; i % _maxRow != 0; i++)
                            {
                                //空行を追加
                                printDatas.Add(new CoSta2020PrintData(RowType.Brank));
                            }

                            //ヘッダー情報
                            int rowCount = printDatas.Count - totalRow;
                            int pageCount = (int)Math.Ceiling((double)(rowCount) / _maxRow);
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
            }


            //データ取得
            sinKouis = _staFinder.GetSinKouis(HpId, _printConf);
            if ((sinKouis?.Count ?? 0) == 0) return false;

            hpInf = _staFinder.GetHpInf(HpId, sinKouis?.First().SinDate ?? 0);

            //印刷用データの作成
            MakePrintData();

            return printDatas.Count > 0;
        }

        private void UpdateDrawForm()
        {
            _hasNextPage = true;

            #region SubMethod

            #region Header
            //using void function because it not return data
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
                int totalPage = (int)Math.Ceiling((double)printDatas.Count / _maxRow);
                _extralData.Add("HeaderR_0_2_" + _currentPage, _currentPage + " / " + totalPage);
                //請求年月
                _extralData.Add("HeaderL_0_1_" + _currentPage, headerL1.Count >= _currentPage ? headerL1[_currentPage - 1] : "");
                //改ページ条件
                _extralData.Add("HeaderL_0_2_" + _currentPage, headerL2.Count >= _currentPage ? headerL2[_currentPage - 1] : "");


                //期間
                SetFieldData("Range",
                    string.Format(
                        "期間: {0} ～ {1}",
                       _printConf.StartSinYm >= 0 ? CIUtil.SDateToShowSWDate(_printConf.StartSinYm * 100 + 1, 0, 1).Substring(0, 12) : CIUtil.SDateToShowSWDate(_printConf.StartSinDate, 0, 1),
                       _printConf.EndSinYm >= 0 ? CIUtil.SDateToShowSWDate(_printConf.EndSinYm * 100 + 1, 0, 1).Substring(0, 12) : CIUtil.SDateToShowSWDate(_printConf.EndSinDate, 0, 1)
                    )
                );
            }
            #endregion

            #region Body
            //using void function because it not return data
            void UpdateFormBody()
            {
                int hokIndex = (_currentPage - 1) * _maxRow;

                //存在しているフィールドに絞り込み
                var existsCols = putColumns.Where(p => _objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

                for (short rowNo = 0; rowNo < _maxRow; rowNo++)
                {

                    Dictionary<string, CellModel> data = new();
                    var printData = printDatas[hokIndex];
                    string baseListName = "";

                    //明細データ出力
                    foreach (var colName in existsCols)
                    {
                        var value = typeof(CoSta2020PrintData).GetProperty(colName)?.GetValue(printData);
                        AddListData(ref data, colName, value == null ? string.Empty : value.ToString() ?? string.Empty);

                        if (baseListName == "" && _objectRseList.Contains(colName))
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
                        string rowNoKey = rowNo + "_" + _currentPage;
                        _extralData.Add("baseListName_" + rowNoKey, baseListName);
                        _extralData.Add("rowNo_" + rowNoKey, rowNo.ToString());
                    }

                    _tableFieldData.Add(data);
                    hokIndex++;
                    if (hokIndex >= printDatas.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }
            }
            #endregion

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

        private void AddListData(ref Dictionary<string, CellModel> dictionary, string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !dictionary.ContainsKey(field))
            {
                dictionary.Add(field, new CellModel(value));
            }
        }
        #endregion

        #region get field java

        private void GetFieldNameList()
        {
            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta2020, "sta2020a.rse", new());
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            _objectRseList = javaOutputData.objectNames;
        }

        private void GetRowCount()
        {
            _rowCountFieldName = putColumns.Find(p => _objectRseList.Contains(p.ColName)).ColName;
            List<ObjectCalculate> fieldInputList = new()
            {
                new ObjectCalculate(_rowCountFieldName, (int)CalculateTypeEnum.GetListRowCount)
            };

            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta2020, "sta2020a.rse", fieldInputList);
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            _maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == _rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? _maxRow;
        }

        public CommonExcelReportingModel ExportCsv(CoSta2020PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow)
        {
            _printConf = printConf;
            HpId = hpId;
            string fileName = menuName + "_" + monthFrom + "_" + monthTo;
            List<string> retDatas = new List<string>();
            if (!GetData()) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

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
            int totalRow = csvDatas.Count;
            int rowOutputed = 0;
            foreach (var csvData in csvDatas)
            {
                retDatas.Add(RecordData(csvData));
                rowOutputed++;
                if (_backgroundWorker != null)
                {
                    int pecentProcess = rowOutputed * 100 / totalRow;
                    _backgroundWorker.ReportProgress(pecentProcess);
                }
            }

            string RecordData(CoSta2020PrintData csvData)
            {
                List<string> colDatas = new();

                foreach (var column in putCurColumns)
                {
                    var value = typeof(CoSta2020PrintData).GetProperty(column.CsvColName)?.GetValue(csvData);
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
        #endregion
    }
}
