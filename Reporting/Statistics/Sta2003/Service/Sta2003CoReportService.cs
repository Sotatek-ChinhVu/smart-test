using Helper.Common;
using Helper.Extension;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta1001.Models;
using Reporting.Statistics.Sta2003.DB;
using Reporting.Statistics.Sta2003.Mapper;
using Reporting.Statistics.Sta2003.Models;
using System.Globalization;

namespace Reporting.Statistics.Sta2003.Service;

public class Sta2003CoReportService : ISta2003CoReportService
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly ICoSta2003Finder _finder;
    private readonly IReadRseReportFileService _readRseReportFileService;
    private CoSta2003PrintConf _printConf;
    private List<CoSta2003PrintData> _printDatas;
    private List<string> _headerL1;
    private List<string> _headerL2;
    private List<CoSyunoInfModel> _syunoInfs;
    private List<CoJihiSbtMstModel> _jihiSbtMsts;
    private List<CoJihiSbtFutan> _jihiSbtFutans;
    private CoHpInfModel _hpInf;
    private readonly List<PutColumn> putCurColumns = new();

    private int _currentPage;
    private string _rowCountFieldName = string.Empty;
    private List<string> _objectRseList;
    private bool _hasNextPage;
    private CoFileType? coFileType;

    private readonly List<PutColumn> csvTotalColumns = new()
        {
            new PutColumn("RowType", "明細区分"),
            new PutColumn("TotalCaption", "合計行"),
            new PutColumn("TotalCount", "合計件数"),
            new PutColumn("TotalPtCount", "合計実人数")
        };

    public Sta2003CoReportService(ICoSta2003Finder finder, IReadRseReportFileService readRseReportFileService)
    {
        _singleFieldData = new();
        _tableFieldData = new();
        _finder = finder;
        _readRseReportFileService = readRseReportFileService;
        _printDatas = new();
        _headerL1 = new();
        _headerL2 = new();
        _syunoInfs = new();
        _jihiSbtMsts = new();
        _jihiSbtFutans = new();
        _hpInf = new();
        _extralData = new();
        _objectRseList = new();
        _printConf = new();
    }

    #region Constant
    private int maxRow = 43;

    private readonly List<PutColumn> putColumns = new()
        {
            new PutColumn("NyukinYmFmt", "診療年月", false, "NyukinYm"),
            new PutColumn("KaId", "診療科ID", false),
            new PutColumn("KaSname", "診療科", false),
            new PutColumn("TantoId", "担当医ID", false),
            new PutColumn("TantoSname", "担当医", false),
            new PutColumn("PtNum", "患者番号", false),
            new PutColumn("PtKanaName", "カナ氏名", false),
            new PutColumn("PtName", "氏名", false),
            new PutColumn("HokenSbt", "保険種別", false),
            new PutColumn("RaiinCount", "来院回数"),
            new PutColumn("RaiinDayCount", "来院日数"),
            new PutColumn("NewTensu", "合計点数(新)"),
            new PutColumn("Tensu", "合計点数"),
            new PutColumn("PtFutan", "負担金額"),
            new PutColumn("JihiFutan", "保険外金額"),
            new PutColumn("JihiTax", "内消費税"),
            new PutColumn("AdjustFutan", "調整額"),
            new PutColumn("MenjyoGaku", "免除額"),
            new PutColumn("NewSeikyuGaku", "合計請求額(新)"),
            new PutColumn("SeikyuGaku", "合計請求額"),
            new PutColumn("NyukinGaku", "入金額"),
            new PutColumn("MisyuGaku", "未収額"),
            new PutColumn("PostNyukinGaku", "期間外入金額"),
            new PutColumn("PostAdjustFutan", "期間外調整額")
        };
    #endregion

    #region Printer method
    private struct CountData
    {
        public int Count;
        public List<string> PtCount;
        public int RaiinCount;
        public int RaiinDayCount;
        public int NewTensu;
        public int Tensu;
        public int PtFutan;
        public int JihiFutan;
        public int JihiTax;
        public int AdjustFutan;
        public int MenjyoGaku;
        public int NewSeikyuGaku;
        public int SeikyuGaku;
        public int NyukinGaku;
        public int MisyuGaku;
        public int PostNyukinGaku;
        public int PostAdjustFutan;
        public List<int> JihiSbtFutans;

        public void AddValue(CoSta2003PrintData printData)
        {
            Count++;
            if (PtCount == null) PtCount = new();
            if (printData.PtNum.AsString() != "" && !PtCount.Contains(printData.PtNum))
            {
                PtCount.Add(printData.PtNum);
            }
            RaiinCount += int.Parse(printData.RaiinCount ?? "0", NumberStyles.Any);
            RaiinDayCount += int.Parse(printData.RaiinDayCount ?? "0", NumberStyles.Any);
            NewTensu += int.Parse(printData.NewTensu ?? "0", NumberStyles.Any);
            Tensu += int.Parse(printData.Tensu ?? "0", NumberStyles.Any);
            PtFutan += int.Parse(printData.PtFutan ?? "0", NumberStyles.Any);
            JihiFutan += int.Parse(printData.JihiFutan ?? "0", NumberStyles.Any);
            JihiTax += int.Parse(printData.JihiTax ?? "0", NumberStyles.Any);
            AdjustFutan += int.Parse(printData.AdjustFutan ?? "0", NumberStyles.Any);
            MenjyoGaku += int.Parse(printData.MenjyoGaku ?? "0", NumberStyles.Any);
            NewSeikyuGaku += int.Parse(printData.NewSeikyuGaku ?? "0", NumberStyles.Any);
            SeikyuGaku += int.Parse(printData.SeikyuGaku ?? "0", NumberStyles.Any);
            NyukinGaku += int.Parse(printData.NyukinGaku ?? "0", NumberStyles.Any);
            MisyuGaku += int.Parse(printData.MisyuGaku ?? "0", NumberStyles.Any);
            PostNyukinGaku += int.Parse(printData.PostNyukinGaku ?? "0", NumberStyles.Any);
            PostAdjustFutan += int.Parse(printData.PostAdjustFutan ?? "0", NumberStyles.Any);

            if (printData.JihiSbtFutans != null)
            {
                if (JihiSbtFutans == null)
                {
                    JihiSbtFutans = new List<int>();
                    for (int i = 0; i <= printData.JihiSbtFutans.Count - 1; i++)
                    {
                        JihiSbtFutans.Add(0);
                    }
                }

                for (int i = 0; i <= printData.JihiSbtFutans.Count - 1; i++)
                {
                    JihiSbtFutans[i] += int.Parse(printData.JihiSbtFutans[i] ?? "0", NumberStyles.Any);
                }
            }
        }

        public void Clear()
        {
            Count = 0;
            PtCount = new();
            RaiinCount = 0;
            RaiinDayCount = 0;
            NewTensu = 0;
            Tensu = 0;
            PtFutan = 0;
            JihiFutan = 0;
            JihiTax = 0;
            AdjustFutan = 0;
            MenjyoGaku = 0;
            NewSeikyuGaku = 0;
            SeikyuGaku = 0;
            NyukinGaku = 0;
            MisyuGaku = 0;
            PostNyukinGaku = 0;
            PostAdjustFutan = 0;
            JihiSbtFutans = new();
        }
    }

    private CountData grandTotal = new CountData();
    private CountData total = new CountData();

    #endregion

    public CommonReportingRequestModel GetSta2003ReportingData(CoSta2003PrintConf printConf, int hpId)
    {
        try
        {
            _printConf = printConf;
            string formFileName = _printConf.FormFileName;

            // get data to print
            GetFieldNameList(formFileName);
            GetRowCount(formFileName);
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

            return new Sta2003Mapper(_singleFieldData, _tableFieldData, _extralData, _rowCountFieldName, formFileName).GetData();
        }
        finally
        {
            _finder.ReleaseResource();
        }
    }

    private void UpdateDrawForm()
    {
        if (_printDatas.Count == 0)
        {
            _hasNextPage = false;
            return;
        }

        #region Header
        void UpdateFormHeader()
        {
            //タイトル
            SetFieldData("Title", _printConf.ReportName);

            //医療機関名
            _extralData.Add("HeaderR_0_0_" + _currentPage, _hpInf.HpName);

            //作成日時
            _extralData.Add("HeaderR_0_1_" + _currentPage, CIUtil.SDateToShowSWDate(
                CIUtil.ShowSDateToSDate(CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd")), 0, 1
            ) + CIUtil.GetJapanDateTimeNow().ToString(" HH:mm") + "作成");

            //ページ数
            int totalPage = (int)Math.Ceiling((double)_printDatas.Count / maxRow);
            _extralData.Add("HeaderR_0_2_" + _currentPage, _currentPage + " / " + totalPage);

            //入金日
            _extralData.Add("HeaderL_0_1_" + _currentPage, _headerL1.Count >= _currentPage ? _headerL1[_currentPage - 1] : "");

            //改ページ条件
            _extralData.Add("HeaderL_0_2_" + _currentPage, _headerL2.Count >= _currentPage ? _headerL2[_currentPage - 1] : "");

            //期間
            SetFieldData("Range",
                string.Format(
                    "期間: {0} ～ {1}",
                    CIUtil.SDateToShowSWDate(_printConf.StartNyukinYm * 100 + 1, 0, 1).Substring(0, 12),
                    CIUtil.SDateToShowSWDate(_printConf.EndNyukinYm * 100 + 1, 0, 1).Substring(0, 12)
                )
            );
        }
        #endregion

        #region Body
        void UpdateFormBody()
        {
            int ptIndex = (_currentPage - 1) * maxRow;
            int lineCount = 0;

            //存在しているフィールドに絞り込み
            var existsCols = putColumns.Where(p => _objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                Dictionary<string, CellModel> data = new();

                var printData = _printDatas[ptIndex];
                string baseListName = "";

                //保険外金額（内訳）タイトル
                foreach (var jihiSbtMst in _jihiSbtMsts)
                {
                    SetFieldData(string.Format("tJihiFutanSbt{0}", jihiSbtMst.JihiSbt), jihiSbtMst.Name);
                }

                //明細データ出力
                foreach (var colName in existsCols)
                {
                    var value = typeof(CoSta2003PrintData).GetProperty(colName)?.GetValue(printData);
                    string valueInput = value?.ToString() ?? string.Empty;
                    AddListData(ref data, colName, valueInput);

                    if (baseListName == "" && _objectRseList.Contains(colName))
                    {
                        baseListName = colName;
                    }
                }
                //自費種別毎の金額
                for (int i = 0; i <= _jihiSbtMsts.Count - 1; i++)
                {
                    // check null printData.JihiSbtFutans
                    if (printData.JihiSbtFutans == null || !printData.JihiSbtFutans.Any()) break;

                    var jihiSbtMst = _jihiSbtMsts[i];
                    AddListData(ref data, string.Format("JihiFutanSbt{0}", jihiSbtMst.JihiSbt), printData.JihiSbtFutans[i]);
                }

                //合計行キャプションと件数
                AddListData(ref data, "TotalCaption", printData.TotalCaption);
                AddListData(ref data, "TotalCount", printData.TotalCount);
                AddListData(ref data, "TotalPtCount", printData.TotalPtCount);

                //5行毎に区切り線を引く
                lineCount = printData.RowType != RowType.Brank ? lineCount + 1 : lineCount;
                string rowNoKey = rowNo + "_" + _currentPage;
                _extralData.Add("lineCount_" + rowNoKey, lineCount.ToString());

                if (lineCount == 5)
                {
                    lineCount = 0;
                    if (!_extralData.ContainsKey("headerLine"))
                    {
                        _extralData.Add("headerLine", "true");
                    }
                    _extralData.Add("baseListName_" + rowNoKey, baseListName);
                    _extralData.Add("rowNo_" + rowNoKey, rowNo.ToString());
                }

                _tableFieldData.Add(data);
                ptIndex++;
                if (ptIndex >= _printDatas.Count)
                {
                    _hasNextPage = false;
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
            _printDatas = new();
            _headerL1 = new();
            _headerL2 = new();
            int totalRow = 0;
            int breakCount = 0;

            //改ページ条件
            bool pbKaId = new int[] { _printConf.PageBreak1, _printConf.PageBreak2 }.Contains(1);
            bool pbTantoId = new int[] { _printConf.PageBreak1, _printConf.PageBreak2 }.Contains(2);

            #region ソート順
            _syunoInfs =
                    _syunoInfs?
                        .OrderBy(s => s.NyukinYm)
                        .ThenBy(s => pbKaId ? s.KaId : 0)
                        .ThenBy(s => pbTantoId ? s.TantoId : 0)
                        .ThenBy(s =>
                            _printConf.SortOpt1 == 1 ? "0" :
                            _printConf.SortOrder1 == 1 ? s.PtKanaName :
                            _printConf.SortOrder1 == 2 ? s.PtNum.ToString().PadLeft(10, '0') : "0")
                        .ThenByDescending(s =>
                            _printConf.SortOpt1 == 0 ? "0" :
                            _printConf.SortOrder1 == 1 ? s.PtKanaName :
                            _printConf.SortOrder1 == 2 ? s.PtNum.ToString().PadLeft(10, '0') : "0")
                        .ThenBy(s => s.NyukinDate == s.SinDate ? "0" : "1" + s.SinDate.ToString())
                        .ThenBy(s => s.PtNum)
                        .ToList() ?? new();
            #endregion

            var nyukinYms = _syunoInfs?.GroupBy(s => s.NyukinYm).OrderBy(s => s.Key).Select(s => s.Key).ToList();
            foreach (var nyukinYm in nyukinYms ?? new())
            {
                var kaIds = _syunoInfs?.GroupBy(s => s.KaId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                for (int kaCnt = 0; (pbKaId && kaCnt <= kaIds?.Count - 1) || kaCnt == 0; kaCnt++)
                {
                    var tantoIds = _syunoInfs?.GroupBy(s => s.TantoId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                    for (int taCnt = 0; (pbTantoId && taCnt <= tantoIds?.Count - 1) || taCnt == 0; taCnt++)
                    {
                        var curDatas = _syunoInfs?.Where(s =>
                            s.NyukinYm == nyukinYm &&
                            (!pbKaId || (kaIds != null && kaIds.Contains(kaCnt) && s.KaId == kaIds[kaCnt])) &&
                            (!pbTantoId || (tantoIds != null && tantoIds.Contains(taCnt) && s.TantoId == tantoIds[taCnt]))
                        ).ToList();

                        var ptDatas = curDatas?
                            .GroupBy(s => new { s.PtNum, s.HokenSbt })
                            .Select(s => s.Key)
                            .ToList();

                        if (ptDatas?.Count == 0) { continue; }

                        //改ページ
                        for (int i = _printDatas.Count; i % maxRow != 0; i++)
                        {
                            //空行を追加
                            _printDatas.Add(new CoSta2003PrintData(RowType.Brank));
                        }

                        long prePtNum = 0;

                        //明細
                        foreach (var ptData in ptDatas ?? new())
                        {
                            var wrkDatas = curDatas?.Where(s => s.PtNum == ptData.PtNum && s.HokenSbt == ptData.HokenSbt).ToList();
                            //集計
                            var recData = RecordData(wrkDatas ?? new(), nyukinYm, prePtNum, RowType.Data);

                            recData.KaId = (pbKaId || kaIds?.Count == 1) ? wrkDatas?.FirstOrDefault()?.KaId.ToString() ?? string.Empty : string.Empty;
                            recData.KaSname = (pbKaId || kaIds?.Count == 1) ? wrkDatas?.FirstOrDefault()?.KaSname ?? string.Empty : string.Empty;
                            recData.TantoId = (pbTantoId || tantoIds?.Count == 1) ? wrkDatas?.FirstOrDefault()?.TantoId.ToString() ?? string.Empty : string.Empty;
                            recData.TantoSname = (pbTantoId || tantoIds?.Count == 1) ? wrkDatas?.FirstOrDefault()?.TantoSname ?? string.Empty : string.Empty;

                            //条件(診療点数)
                            if (_printConf.IsTensu == 1 && recData.Tensu != "0") continue;
                            if (_printConf.IsTensu == 2 && recData.Tensu == "0") continue;
                            //条件(保険外金額)
                            if (_printConf.IsJihiFutan == 1 && recData.JihiFutan != "0") continue;
                            if (_printConf.IsJihiFutan == 2 && recData.JihiFutan == "0") continue;
                            //条件(保険種別)
                            if (_printConf.HokenSbts?.Count >= 1 && !_printConf.HokenSbts.Contains(recData.HokenSbtCd)) continue;

                            //行追加
                            _printDatas.Add(recData);
                            //1行前の患者番号
                            prePtNum = ptData.PtNum;

                            //合計
                            grandTotal.AddValue(recData);
                            total.AddValue(recData);
                        }

                        //行追加
                        _printDatas.Add(new CoSta2003PrintData(RowType.Brank));
                        AddTotalRecord("◆合計", ref total);

                        if (
                            (nyukinYm != nyukinYms?.Last()) ||
                            (pbKaId && (kaCnt + 1 <= kaIds?.Count - 1)) ||
                            (pbTantoId && (taCnt + 1 <= tantoIds?.Count - 1))
                        )
                        {
                            //改ページ
                            for (int i = _printDatas.Count; i % maxRow != 0; i++)
                            {
                                //空行を追加
                                _printDatas.Add(new CoSta2003PrintData(RowType.Brank));
                            }
                        }
                        breakCount++;

                        //ヘッダー情報
                        int rowCount = _printDatas.Count - totalRow;
                        int pageCount = (int)Math.Ceiling((double)(rowCount) / maxRow);
                        for (int i = 0; i < pageCount; i++)
                        {
                            //診療年月
                            string wrkYm = CIUtil.Copy(CIUtil.SDateToShowSWDate(nyukinYm * 100 + 1, 0, 1, 1), 1, 13);
                            _headerL1.Add(wrkYm + "度");
                            //改ページ条件
                            List<string> wrkHeaders = new List<string>();
                            if (pbKaId) wrkHeaders.Add(curDatas?.FirstOrDefault()?.KaSname ?? string.Empty);
                            if (pbTantoId) wrkHeaders.Add(curDatas?.FirstOrDefault()?.TantoSname ?? string.Empty);

                            if (wrkHeaders.Count >= 1) _headerL2.Add(string.Join("／", wrkHeaders));
                        }
                        totalRow += rowCount;
                    }
                }
            }

            if (breakCount >= 2)
            {
                //行追加
                _printDatas.Add(new CoSta2003PrintData(RowType.Brank));
                _printDatas.Add(new CoSta2003PrintData(RowType.Brank));
                AddTotalRecord("◆総合計", ref grandTotal);
            }
        }

        #region データ集計
        CoSta2003PrintData RecordData(List<CoSyunoInfModel> rowDatas, int nyukinYm, long prePtNum, RowType rowType)
        {
            bool isTotalRow = rowType == RowType.Total;
            CoSta2003PrintData printData = new CoSta2003PrintData();

            printData.NyukinYm = nyukinYm;

            if (prePtNum != rowDatas.First().PtNum || coFileType == CoFileType.Csv)
            {
                printData.PtNum = isTotalRow ? "" : rowDatas.First().PtNum.ToString();
                printData.PtName = isTotalRow ? "" : rowDatas.First().PtName;
                printData.PtKanaName = isTotalRow ? "" : rowDatas.First().PtKanaName;
            }
            printData.HokenSbt = isTotalRow ? "" : rowDatas.First().HokenSbt;
            printData.RaiinCount = rowDatas.Where(s => s.IsSinMonth).GroupBy(s => s.RaiinNo).Count().ToString("#,0");
            printData.RaiinDayCount = rowDatas.Where(s => s.IsSinMonth).GroupBy(s => new { s.PtNum, s.SinDate }).Count().ToString("#,0");
            printData.Tensu = rowDatas.Where(s => s.IsSinMonth)
                .GroupBy(s => s.RaiinNo)
                .Select(s => new { RaiinNo = s.Key, Tensu = s.Max(x => x.Tensu) })
                .Sum(s => s.Tensu).ToString("#,0");
            printData.NewTensu = rowDatas.Where(s => s.IsSinMonth)
                .GroupBy(s => s.RaiinNo)
                .Select(s => new { RaiinNo = s.Key, NewTensu = s.Max(x => x.NewTensu) })
                .Sum(s => s.NewTensu).ToString("#,0");
            printData.PtFutan = rowDatas.Where(s => s.IsSinMonth)
                .GroupBy(s => s.RaiinNo)
                .Select(s => new { RaiinNo = s.Key, PtFutan = s.Max(x => x.PtFutan) })
                .Sum(s => s.PtFutan).ToString("#,0");
            printData.JihiFutan = rowDatas.Where(s => s.IsSinMonth)
                .GroupBy(s => s.RaiinNo)
                .Select(s => new { RaiinNo = s.Key, JihiFutan = s.Max(x => x.JihiFutan) })
                .Sum(s => s.JihiFutan).ToString("#,0");
            printData.JihiTax = rowDatas.Where(s => s.IsSinMonth)
                .GroupBy(s => s.RaiinNo)
                .Select(s => new { RaiinNo = s.Key, JihiTax = s.Max(x => x.JihiTax) })
                .Sum(s => s.JihiTax).ToString("#,0");
            printData.AdjustFutan = rowDatas.Where(s => s.IsSinMonth)
                .GroupBy(s => s.RaiinNo)
                .Select(s => new { RaiinNo = s.Key, AdjustFutan = s.Max(x => x.AdjustFutan) })
                .Sum(s => s.AdjustFutan).ToString("#,0");

            var wrkNyukins = rowDatas.Where(s => s.IsSinMonth)
                .GroupBy(s => s.RaiinNo)
                .Select(s => new { RaiinNo = s.Key, NyukinSortNo = s.Max(x => x.NyukinSortNo) })
                .ToList();
            int wrkSeikyu = 0;
            int wrkNewSeikyu = 0;
            foreach (var wrkNyukin in wrkNyukins)
            {
                wrkSeikyu += rowDatas?.FirstOrDefault(s => s.RaiinNo == wrkNyukin.RaiinNo && s.NyukinSortNo == wrkNyukin.NyukinSortNo)?.SeikyuGaku ?? 0;
                wrkNewSeikyu += rowDatas?.FirstOrDefault(s => s.RaiinNo == wrkNyukin.RaiinNo && s.NyukinSortNo == wrkNyukin.NyukinSortNo)?.NewSeikyuGaku ?? 0;
            }
            printData.SeikyuGaku = wrkSeikyu.ToString("#,0");
            printData.NewSeikyuGaku = wrkNewSeikyu.ToString("#,0");

            printData.MenjyoGaku = rowDatas.Where(s => s.IsSinMonth)
                .GroupBy(s => s.RaiinNo)
                .Select(s => new { RaiinNo = s.Key, MenjyoGaku = s.Max(x => x.MenjyoGaku) })
                .Sum(s => s.MenjyoGaku).ToString("#,0");
            printData.NyukinGaku = rowDatas.Sum(s => s.IsSinMonth ? s.NyukinGaku : 0).ToString("#,0");
            printData.PostNyukinGaku = rowDatas.Sum(s => !s.IsSinMonth ? s.NyukinGaku : 0).ToString("#,0");
            printData.PostAdjustFutan = rowDatas.Sum(s => !s.IsSinMonth ? s.NyukinAdjustFutan : 0).ToString("#,0");
            printData.MisyuGaku =
                (
                    int.Parse(printData.SeikyuGaku ?? "0", NumberStyles.Any) -
                    int.Parse(printData.NyukinGaku ?? "0", NumberStyles.Any) -
                    rowDatas.Where(s => s.IsSinMonth && s.NyukinKbn == 0)
                        .GroupBy(s => s.RaiinNo)
                        .Select(s => new { RaiinNo = s.Key, SeikyuGaku = s.Max(x => x.SeikyuGaku) })
                        .Sum(s => s.SeikyuGaku)
                ).ToString("#,0");

            //自費種別ごとの金額
            List<string> wrkPrintFutans = new List<string>();

            var raiinNos = rowDatas.Where(s => s.IsSinMonth)
                .GroupBy(s => s.RaiinNo)
                .Select(s => s.Key);
            foreach (var jihiSbtMst in _jihiSbtMsts)
            {
                int wrkFutan = 0;
                foreach (var raiinNo in raiinNos)
                {
                    wrkFutan += _jihiSbtFutans.Find(f => f.RaiinNo == raiinNo && f.JihiSbt == jihiSbtMst.JihiSbt)?.JihiFutan ?? 0;
                }
                wrkPrintFutans.Add(wrkFutan.ToString("#,0"));
            }
            printData.JihiSbtFutans = wrkPrintFutans;

            return printData;
        }
        #endregion

        #region 合計レコードの追加
        void AddTotalRecord(string title, ref CountData totalData)
        {
            _printDatas.Add(
                new CoSta2003PrintData(RowType.Total)
                {
                    TotalCaption = title,
                    TotalCount = totalData.Count.ToString("#,0件"),
                    TotalPtCount = totalData.PtCount.Count.ToString("(#,0人)"),
                    RaiinCount = totalData.RaiinCount.ToString("#,0"),
                    RaiinDayCount = totalData.RaiinDayCount.ToString("#,0"),
                    NewTensu = totalData.NewTensu.ToString("#,0"),
                    Tensu = totalData.Tensu.ToString("#,0"),
                    PtFutan = totalData.PtFutan.ToString("#,0"),
                    JihiFutan = totalData.JihiFutan.ToString("#,0"),
                    JihiTax = totalData.JihiTax.ToString("#,0"),
                    AdjustFutan = totalData.AdjustFutan.ToString("#,0"),
                    MenjyoGaku = totalData.MenjyoGaku.ToString("#,0"),
                    NewSeikyuGaku = totalData.NewSeikyuGaku.ToString("#,0"),
                    SeikyuGaku = totalData.SeikyuGaku.ToString("#,0"),
                    NyukinGaku = totalData.NyukinGaku.ToString("#,0"),
                    MisyuGaku = totalData.MisyuGaku.ToString("#,0"),
                    PostNyukinGaku = totalData.PostNyukinGaku.ToString("#,0"),
                    PostAdjustFutan = totalData.PostAdjustFutan.ToString("#,0"),
                    JihiSbtFutans = totalData.JihiSbtFutans.Select(j => j.ToString("#,0")).ToList()
                }
            );
            totalData.Clear();
        }
        #endregion

        _hpInf = _finder.GetHpInf(hpId, _printConf.StartNyukinYm * 100 + 1);

        //データ取得
        _syunoInfs = _finder.GetSyunoInfs(hpId, _printConf);
        if ((_syunoInfs?.Count ?? 0) == 0)
        {
            return false;
        }

        _jihiSbtMsts = _finder.GetJihiSbtMst(hpId);
        _jihiSbtFutans = _finder.GetJihiSbtFutan(hpId, _printConf);

        //印刷用データの作成
        MakePrintData();

        return _printDatas.Count > 0;
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
        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta2003, fileName, new());
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        _objectRseList = javaOutputData.objectNames;
    }

    private void GetRowCount(string fileName)
    {
        _rowCountFieldName = putColumns.Find(p => _objectRseList.Contains(p.ColName)).ColName;
        List<ObjectCalculate> fieldInputList = new()
        {
            new ObjectCalculate(_rowCountFieldName, (int)CalculateTypeEnum.GetListRowCount)
        };

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta2003, fileName, fieldInputList);
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == _rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? maxRow;
    }

    public CommonExcelReportingModel ExportCsv(CoSta2003PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType)
    {
        this.coFileType = coFileType;
        _printConf = printConf;
        string fileName = menuName + "_" + monthFrom + "_" + monthTo;
        List<string> retDatas = new List<string>();

        if (!GetData(hpId)) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

        if (isPutTotalRow)
        {
            putCurColumns.AddRange(csvTotalColumns);
        }
        putCurColumns.AddRange(putColumns);

        var csvDatas = _printDatas.Where(p => p.RowType == RowType.Data || (isPutTotalRow && p.RowType == RowType.Total)).ToList();

        if (csvDatas.Count == 0) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

        List<string> wrkTitles = putCurColumns.Select(p => p.JpName).ToList();
        List<string> wrkColumns = putCurColumns.Select(p => p.CsvColName).ToList();

        //タイトル行
        retDatas.Add("\"" + string.Join("\",\"", wrkTitles.Union(_jihiSbtMsts.Select(j => string.Format("保険外金額({0})", j.Name)))) + "\"");
        if (isPutColName)
        {
            retDatas.Add("\"" + string.Join("\",\"", wrkColumns.Union(_jihiSbtMsts.Select(j => string.Format("JihiFutanSbt{0}", j.JihiSbt)))) + "\"");
        }

        //データ
        int rowOutputed = 0;
        foreach (var csvData in csvDatas)
        {
            retDatas.Add(RecordData(csvData));
            rowOutputed++;
        }

        string RecordData(CoSta2003PrintData csvData)
        {
            List<string> colDatas = new List<string>();

            foreach (var column in putCurColumns)
            {
                var value = typeof(CoSta2003PrintData).GetProperty(column.CsvColName)?.GetValue(csvData);
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
            //自費種別毎の金額
            if (csvData.JihiSbtFutans != null)
            {
                foreach (var jihiSbtFutan in csvData.JihiSbtFutans)
                {
                    colDatas.Add("\"" + jihiSbtFutan + "\"");
                }
            }
            else
            {
                foreach (var jihiSbtMst in _jihiSbtMsts)
                {
                    colDatas.Add("\"0\"");
                }
            }

            return string.Join(",", colDatas);
        }

        return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);
    }
}
