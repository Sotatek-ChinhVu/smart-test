using Helper.Common;
using Helper.Constants;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta1001.Models;
using Reporting.Statistics.Sta2001.DB;
using Reporting.Statistics.Sta2001.Mapper;
using Reporting.Statistics.Sta2001.Models;
using System.Globalization;

namespace Reporting.Statistics.Sta2001.Service;

public class Sta2001CoReportService : ISta2001CoReportService
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly ICoSta2001Finder _finder;
    private readonly IReadRseReportFileService _readRseReportFileService;
    private CoSta2001PrintConf _printConf;
    private List<CoSta2001PrintData> _printDatas;
    private List<string> _headerL1;
    private List<string> _headerL2;
    private List<CoSyunoInfModel> _syunoInfs;
    private List<CoJihiSbtMstModel> _jihiSbtMsts;
    private List<CoJihiSbtFutan> _jihiSbtFutans;
    private CoHpInfModel _hpInf;
    private List<PutColumn> putCurColumns = new List<PutColumn>();

    private List<PutColumn> csvTotalColumns = new List<PutColumn>
        {
            new PutColumn("RowType", "明細区分"),
            new PutColumn("TotalCaption", "合計行")
        };

    public Sta2001CoReportService(ICoSta2001Finder finder, IReadRseReportFileService readRseReportFileService)
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
    private int maxRow = 37;

    private readonly List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("NyukinDateFmt", "入金日", false, "NyukinDate"),
            new PutColumn("NyukinDateFmt2", "入金日2", false),
            new PutColumn("KaId", "診療科ID"),
            new PutColumn("KaSname", "診療科"),
            new PutColumn("TantoId", "担当医ID"),
            new PutColumn("TantoSname", "担当医"),
            new PutColumn("SyosinCount", "初診件数"),
            new PutColumn("SaisinCount", "再診件数"),
            new PutColumn("Count", "合計件数"),
            new PutColumn("NewPtCount", "新患件数"),
            new PutColumn("PtCount", "実人数"),
            new PutColumn("Tensu", "合計点数"),
            new PutColumn("NewTensu", "合計点数(新)"),
            new PutColumn("PtFutan", "負担金額"),
            new PutColumn("JihiFutan", "保険外金額"),
            new PutColumn("JihiTax", "内消費税"),
            new PutColumn("AdjustFutan", "調整額"),
            new PutColumn("MenjyoGaku", "免除額"),
            new PutColumn("SeikyuGaku", "合計請求額"),
            new PutColumn("NewSeikyuGaku", "合計請求額(新)"),
            new PutColumn("NyukinGaku", "入金額"),
            new PutColumn("MisyuGaku", "未収額"),
            new PutColumn("PostNyukinGaku", "期間外入金額"),
            new PutColumn("PostAdjustFutan", "期間外調整額"),
            new PutColumn("NyukinGakuSyaho", "入金額(社保)"),
            new PutColumn("NyukinGakuKokho", "入金額(国保)"),
            new PutColumn("NyukinGakuKohi", "入金額(公費)"),
            new PutColumn("NyukinGakuJihi", "入金額(自費)"),
            new PutColumn("NyukinGakuJihiRece", "入金額(自費レセ)"),
            new PutColumn("NyukinGakuRousai", "入金額(労災)"),
            new PutColumn("NyukinGakuJibai", "入金額(自賠)"),
            new PutColumn("PostNyukinGakuSyaho", "期間外入金額(社保)"),
            new PutColumn("PostNyukinGakuKokho", "期間外入金額(国保)"),
            new PutColumn("PostNyukinGakuKohi", "期間外入金額(公費)"),
            new PutColumn("PostNyukinGakuJihi", "期間外入金額(自費)"),
            new PutColumn("PostNyukinGakuJihiRece", "期間外入金額(自費レセ)"),
            new PutColumn("PostNyukinGakuRousai", "期間外入金額(労災)"),
            new PutColumn("PostNyukinGakuJibai", "期間外入金額(自賠)")
        };
    #endregion

    private int _currentPage;
    private string _rowCountFieldName = string.Empty;
    private List<string> _objectRseList;
    private bool _hasNextPage;

    public CommonReportingRequestModel GetSta2001ReportingData(CoSta2001PrintConf printConf, int hpId)
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

            return new Sta2001Mapper(_singleFieldData, _tableFieldData, _extralData, _rowCountFieldName, formFileName).GetData();
        }
        finally
        {
            _finder.ReleaseResource();
        }
    }

    #region private function
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
            //診療月
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
            int dayIndex = (_currentPage - 1) * maxRow;

            //存在しているフィールドに絞り込み
            var existsCols = putColumns.Where(p => _objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                Dictionary<string, CellModel> data = new();
                var printData = _printDatas[dayIndex];
                string baseListName = "";

                //明細データ出力
                foreach (var colName in existsCols)
                {
                    var value = typeof(CoSta2001PrintData).GetProperty(colName)?.GetValue(printData);
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
                    if (printData.JihiSbtFutans == null) break;

                    var jihiSbtMst = _jihiSbtMsts[i];
                    AddListData(ref data, string.Format("JihiFutanSbt{0}", jihiSbtMst.JihiSbt), printData.JihiSbtFutans[i]);
                }

                //合計行キャプションと件数
                AddListData(ref data, "TotalCaption", printData.TotalCaption);

                //区切り線を引く
                if (new int[] { 4, 9, 14, 19, 24, 29, 32 }.Contains(rowNo))
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
                dayIndex++;
                if (dayIndex >= _printDatas.Count)
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
            _printDatas = new List<CoSta2001PrintData>();
            _headerL1 = new List<string>();
            _headerL2 = new List<string>();

            //改ページ条件
            bool pbKaId = new int[] { _printConf.PageBreak1, _printConf.PageBreak2 }.Contains(1);
            bool pbTantoId = new int[] { _printConf.PageBreak1, _printConf.PageBreak2 }.Contains(2);

            var nyukinYms = _syunoInfs?.GroupBy(s => s.NyukinYm).OrderBy(s => s.Key).Select(s => s.Key).ToList();
            foreach (var nyukinYm in nyukinYms ?? new())
            {
                var kaIds = _syunoInfs?.GroupBy(s => s.KaId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                for (int j = 0; (pbKaId && j <= kaIds?.Count - 1) || j == 0; j++)
                {
                    var tantoIds = _syunoInfs?.GroupBy(s => s.TantoId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                    for (int k = 0; (pbTantoId && k <= tantoIds?.Count - 1) || k == 0; k++)
                    {
                        var curDatas = _syunoInfs?.Where(s =>
                            s.NyukinYm == nyukinYm &&
                            (pbKaId ? s.KaId == kaIds?[j] : true) &&
                            (pbTantoId ? s.TantoId == tantoIds?[k] : true)
                        ).ToList();

                        if (curDatas?.Count == 0) continue;

                        //明細
                        for (int rowNo = 0; rowNo <= maxRow - 1; rowNo++)
                        {
                            CoSta2001PrintData printData = new CoSta2001PrintData();

                            int nyukinDate = nyukinYm * 100 + rowNo + 1;
                            string nyukinDateFmt2 = CIUtil.Copy(CIUtil.SDateToShowSDate2(nyukinDate), 6, 8);
                            int totalCount = 0;

                            //日付毎の明細
                            List<CoSyunoInfModel>? wrkDatas = null;

                            switch (rowNo)
                            {
                                case 32:
                                    printData.RowType = RowType.Total;
                                    printData.TotalCaption = "◆合計";
                                    wrkDatas = curDatas?.Where(s => s.NyukinYm == nyukinYm).ToList();
                                    break;
                                case 34:
                                    printData.RowType = RowType.Total;
                                    printData.TotalCaption = "◆平均(日)";
                                    wrkDatas = curDatas?.Where(s => s.NyukinYm == nyukinYm).ToList();
                                    totalCount = wrkDatas?.Where(s => s.IsSinDate).GroupBy(s => s.NyukinDate).Count() ?? 0;
                                    break;
                                case 35:
                                    printData.RowType = RowType.Total;
                                    printData.TotalCaption = "◆平均(患者)";
                                    wrkDatas = curDatas?.Where(s => s.NyukinYm == nyukinYm).ToList();
                                    totalCount = wrkDatas?.Where(s => s.IsSinDate).GroupBy(s => s.PtNum).Count() ?? 0;
                                    break;
                                case 36:
                                    printData.RowType = RowType.Total;
                                    printData.TotalCaption = "◆平均(来院)";
                                    wrkDatas = curDatas?.Where(s => s.NyukinYm == nyukinYm).ToList();
                                    totalCount = wrkDatas?.Where(s => s.IsSinDate).GroupBy(s => s.RaiinNo).Count() ?? 0;
                                    break;
                                default:
                                    wrkDatas = curDatas?.Where(s => s.NyukinDate == nyukinDate).ToList();
                                    break;
                            }

                            if (wrkDatas == null || (printData.RowType == RowType.Data && nyukinDateFmt2 == string.Empty))
                            {
                                //空行を追加
                                _printDatas.Add(new CoSta2001PrintData(RowType.Brank));
                                continue;
                            }

                            printData.NyukinDate = nyukinDate;
                            printData.NyukinDateFmt2 = nyukinDateFmt2;
                            printData.KaId = (pbKaId || kaIds?.Count == 1) ? curDatas?.FirstOrDefault()?.KaId.ToString() ?? string.Empty : string.Empty;
                            printData.KaSname = (pbKaId || kaIds?.Count == 1) ? curDatas?.FirstOrDefault()?.KaSname ?? string.Empty : string.Empty;
                            printData.TantoId = (pbTantoId || tantoIds?.Count == 1) ? curDatas?.FirstOrDefault()?.TantoId.ToString() ?? string.Empty : string.Empty;
                            printData.TantoSname = (pbTantoId || tantoIds?.Count == 1) ? curDatas?.FirstOrDefault()?.TantoSname ?? string.Empty : string.Empty;
                            printData.SyosinCount = wrkDatas.Where(s => s.IsSinDate && s.Syosaisin == "初診")
                                .GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                            printData.SaisinCount = wrkDatas.Where(s => s.IsSinDate && s.Syosaisin == "再診")
                                .GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                            printData.Count = wrkDatas.Where(s => s.IsSinDate)
                                .GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                            printData.NewPtCount = wrkDatas.Where(s => s.IsFirstRaiin)
                                .GroupBy(s => s.PtNum).Count().ToString("#,0");
                            printData.PtCount = wrkDatas.Where(s => s.IsSinDate)
                                .GroupBy(s => s.PtNum).Count().ToString("#,0");
                            printData.Tensu = wrkDatas.Where(s => s.IsSinDate)
                                .GroupBy(s => s.RaiinNo)
                                .Select(s => new { RaiinNo = s.Key, Tensu = s.Max(x => x.Tensu) })
                                .Sum(s => s.Tensu).ToString("#,0");
                            printData.NewTensu = wrkDatas.Where(s => s.IsSinDate)
                                .GroupBy(s => s.RaiinNo)
                                .Select(s => new { RaiinNo = s.Key, NewTensu = s.Max(x => x.NewTensu) })
                                .Sum(s => s.NewTensu).ToString("#,0");
                            printData.PtFutan = wrkDatas.Where(s => s.IsSinDate)
                                .GroupBy(s => s.RaiinNo)
                                .Select(s => new { RaiinNo = s.Key, PtFutan = s.Max(x => x.PtFutan) })
                                .Sum(s => s.PtFutan).ToString("#,0");
                            printData.JihiFutan = wrkDatas.Where(s => s.IsSinDate)
                                .GroupBy(s => s.RaiinNo)
                                .Select(s => new { RaiinNo = s.Key, JihiFutan = s.Max(x => x.JihiFutan) })
                                .Sum(s => s.JihiFutan).ToString("#,0");
                            printData.JihiTax = wrkDatas.Where(s => s.IsSinDate)
                                .GroupBy(s => s.RaiinNo)
                                .Select(s => new { RaiinNo = s.Key, JihiTax = s.Max(x => x.JihiTax) })
                                .Sum(s => s.JihiTax).ToString("#,0");
                            printData.AdjustFutan = wrkDatas.Where(s => s.IsSinDate)
                                .GroupBy(s => s.RaiinNo)
                                .Select(s => new { RaiinNo = s.Key, AdjustFutan = s.Max(x => x.AdjustFutan) })
                                .Sum(s => s.AdjustFutan).ToString("#,0");

                            var wrkNyukins = wrkDatas.Where(s => s.IsSinDate)
                                .GroupBy(s => s.RaiinNo)
                                .Select(s => new { RaiinNo = s.Key, NyukinSortNo = s.Max(x => x.NyukinSortNo) })
                                .ToList();
                            int wrkSeikyu = 0;
                            int wrkNewSeikyu = 0;
                            foreach (var wrkNyukin in wrkNyukins)
                            {
                                wrkSeikyu += wrkDatas?.Find(s => s.RaiinNo == wrkNyukin.RaiinNo && s.NyukinSortNo == wrkNyukin.NyukinSortNo)?.SeikyuGaku ?? 0;
                                wrkNewSeikyu += wrkDatas?.Find(s => s.RaiinNo == wrkNyukin.RaiinNo && s.NyukinSortNo == wrkNyukin.NyukinSortNo)?.NewSeikyuGaku ?? 0;
                            }
                            printData.SeikyuGaku = wrkSeikyu.ToString("#,0");
                            printData.NewSeikyuGaku = wrkNewSeikyu.ToString("#,0");

                            printData.MenjyoGaku = wrkDatas.Where(s => s.IsSinDate)
                                .GroupBy(s => s.RaiinNo)
                                .Select(s => new { RaiinNo = s.Key, MenjyoGaku = s.Max(x => x.MenjyoGaku) })
                                .Sum(s => s.MenjyoGaku).ToString("#,0");
                            printData.NyukinGaku = wrkDatas.Sum(s => s.IsSinDate ? s.NyukinGaku : 0).ToString("#,0");
                            #region 'NyukinGaku(保険別)'
                            printData.NyukinGakuSyaho = wrkDatas.Sum(s => s.IsSinDate && s.HokenKbn == HokenKbn.Syaho && !s.IsKohiOnly ? s.NyukinGaku : 0).ToString("#,0");
                            printData.NyukinGakuKokho = wrkDatas.Sum(s => s.IsSinDate && s.HokenKbn == HokenKbn.Kokho ? s.NyukinGaku : 0).ToString("#,0");
                            printData.NyukinGakuKohi = wrkDatas.Sum(s => s.IsSinDate && s.IsKohiOnly ? s.NyukinGaku : 0).ToString("#,0");
                            printData.NyukinGakuJihi = wrkDatas.Sum(s => s.IsSinDate && s.IsJihiNoRece ? s.NyukinGaku : 0).ToString("#,0");
                            printData.NyukinGakuJihiRece = wrkDatas.Sum(s => s.IsSinDate && s.IsJihiRece ? s.NyukinGaku : 0).ToString("#,0");
                            printData.NyukinGakuRousai = wrkDatas.Sum(s => s.IsSinDate && s.IsRousai ? s.NyukinGaku : 0).ToString("#,0");
                            printData.NyukinGakuJibai = wrkDatas.Sum(s => s.IsSinDate && s.HokenKbn == HokenKbn.Jibai ? s.NyukinGaku : 0).ToString("#,0");
                            #endregion
                            printData.PostNyukinGaku = wrkDatas.Sum(s => !s.IsSinDate ? s.NyukinGaku : 0).ToString("#,0");
                            #region 'PostNyukinGaku(保険別)'
                            printData.PostNyukinGakuSyaho = wrkDatas.Sum(s => !s.IsSinDate && s.HokenKbn == HokenKbn.Syaho && !s.IsKohiOnly ? s.NyukinGaku : 0).ToString("#,0");
                            printData.PostNyukinGakuKokho = wrkDatas.Sum(s => !s.IsSinDate && s.HokenKbn == HokenKbn.Kokho ? s.NyukinGaku : 0).ToString("#,0");
                            printData.PostNyukinGakuKohi = wrkDatas.Sum(s => !s.IsSinDate && s.IsKohiOnly ? s.NyukinGaku : 0).ToString("#,0");
                            printData.PostNyukinGakuJihi = wrkDatas.Sum(s => !s.IsSinDate && s.IsJihiNoRece ? s.NyukinGaku : 0).ToString("#,0");
                            printData.PostNyukinGakuJihiRece = wrkDatas.Sum(s => !s.IsSinDate && s.IsJihiRece ? s.NyukinGaku : 0).ToString("#,0");
                            printData.PostNyukinGakuRousai = wrkDatas.Sum(s => !s.IsSinDate && s.IsRousai ? s.NyukinGaku : 0).ToString("#,0");
                            printData.PostNyukinGakuJibai = wrkDatas.Sum(s => !s.IsSinDate && s.HokenKbn == HokenKbn.Jibai ? s.NyukinGaku : 0).ToString("#,0");
                            #endregion
                            printData.PostAdjustFutan = wrkDatas.Sum(s => !s.IsSinDate ? s.NyukinAdjustFutan : 0).ToString("#,0");
                            printData.MisyuGaku =
                                (
                                    int.Parse(printData.SeikyuGaku ?? "0", NumberStyles.Any) -
                                    int.Parse(printData.NyukinGaku ?? "0", NumberStyles.Any) -
                                    wrkDatas.Where(s => s.IsSinDate && s.NyukinKbn == 0 && !s.IsNyukin)
                                        .GroupBy(s => s.RaiinNo)
                                        .Select(s => new { RaiinNo = s.Key, SeikyuGaku = s.Max(x => x.SeikyuGaku) })
                                        .Sum(s => s.SeikyuGaku)
                                ).ToString("#,0");

                            //自費種別ごとの金額
                            List<string> wrkPrintFutans = new List<string>();

                            foreach (var jihiSbtMst in _jihiSbtMsts)
                            {
                                int wrkFutan = 0;
                                foreach (var wrkData in wrkDatas)
                                {
                                    if (wrkData.IsSinDate)
                                    {
                                        wrkFutan += _jihiSbtFutans.Find(f => f.RaiinNo == wrkData.RaiinNo && f.JihiSbt == jihiSbtMst.JihiSbt)?.JihiFutan ?? 0;
                                    }
                                }
                                wrkPrintFutans.Add(wrkFutan.ToString("#,0"));
                            }
                            printData.JihiSbtFutans = wrkPrintFutans;

                            //平均
                            if (totalCount >= 1)
                            {
                                printData.SyosinCount = (double.Parse(printData.SyosinCount ?? "0", NumberStyles.Any) / totalCount).ToString("#,0.0");
                                printData.SaisinCount = (double.Parse(printData.SaisinCount ?? "0", NumberStyles.Any) / totalCount).ToString("#,0.0");
                                printData.Count = (double.Parse(printData.Count ?? "0", NumberStyles.Any) / totalCount).ToString("#,0.0");
                                printData.NewPtCount = (double.Parse(printData.NewPtCount ?? "0", NumberStyles.Any) / totalCount).ToString("#,0.0");
                                printData.PtCount = (double.Parse(printData.PtCount ?? "0", NumberStyles.Any) / totalCount).ToString("#,0.0");
                                printData.Tensu = (double.Parse(printData.Tensu ?? "0", NumberStyles.Any) / totalCount).ToString("#,0.0");
                                printData.NewTensu = (double.Parse(printData.NewTensu ?? "0", NumberStyles.Any) / totalCount).ToString("#,0.0");
                                printData.PtFutan = (double.Parse(printData.PtFutan ?? "0", NumberStyles.Any) / totalCount).ToString("#,0.0");
                                printData.JihiFutan = (double.Parse(printData.JihiFutan ?? "0", NumberStyles.Any) / totalCount).ToString("#,0.0");
                                printData.JihiTax = (double.Parse(printData.JihiTax ?? "0", NumberStyles.Any) / totalCount).ToString("#,0.0");
                                printData.AdjustFutan = (double.Parse(printData.AdjustFutan ?? "0", NumberStyles.Any) / totalCount).ToString("#,0.0");
                                printData.SeikyuGaku = (double.Parse(printData.SeikyuGaku ?? "0", NumberStyles.Any) / totalCount).ToString("#,0.0");
                                printData.NewSeikyuGaku = (double.Parse(printData.NewSeikyuGaku ?? "0", NumberStyles.Any) / totalCount).ToString("#,0.0");
                                printData.MenjyoGaku = (double.Parse(printData.MenjyoGaku ?? "0", NumberStyles.Any) / totalCount).ToString("#,0.0");
                                printData.NyukinGaku = (double.Parse(printData.NyukinGaku ?? "0", NumberStyles.Any) / totalCount).ToString("#,0.0");
                                printData.PostNyukinGaku = (double.Parse(printData.PostNyukinGaku ?? "0", NumberStyles.Any) / totalCount).ToString("#,0.0");
                                printData.PostAdjustFutan = (double.Parse(printData.PostAdjustFutan ?? "0", NumberStyles.Any) / totalCount).ToString("#,0.0");
                                printData.MisyuGaku = (double.Parse(printData.MisyuGaku ?? "0", NumberStyles.Any) / totalCount).ToString("#,0.0");
                                for (int i = 0; i < printData.JihiSbtFutans.Count; i++)
                                {
                                    printData.JihiSbtFutans[i] = (double.Parse(printData.JihiSbtFutans[i] ?? "0", NumberStyles.Any) / totalCount).ToString("#,0.0");
                                }
                            }

                            //行追加
                            _printDatas.Add(printData);

                            //ヘッダー情報
                            if ((int)Math.Ceiling((double)(_printDatas.Count) / maxRow) > _headerL1.Count)
                            {
                                //入金日
                                string wrkYm = CIUtil.Copy(CIUtil.SDateToShowSWDate(nyukinYm * 100 + 1, 0, 1, 1), 1, 13);
                                _headerL1.Add(wrkYm + "度");
                                //改ページ条件
                                List<string> wrkHeaders = new List<string>();
                                if (pbKaId) wrkHeaders.Add(curDatas?.First().KaSname ?? string.Empty);
                                if (pbTantoId) wrkHeaders.Add(curDatas?.First().TantoSname ?? string.Empty);

                                if (wrkHeaders.Count >= 1) _headerL2.Add(string.Join("／", wrkHeaders));
                            }
                        }
                    }
                }
            }
        }

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
        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta2001, fileName, new());
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

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta2001, fileName, fieldInputList);
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == _rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? maxRow;
    }

    public CommonExcelReportingModel ExportCsv(CoSta2001PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow)
    {
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

        //出力フィールド
        List<string> wrkTitles = putCurColumns.Select(p => p.JpName).ToList();
        List<string> wrkColumns = putCurColumns.Select(p => p.CsvColName).ToList();

        //タイトル行
        retDatas.Add("\"" + string.Join("\",\"", wrkTitles.Union(_jihiSbtMsts.Select(j => string.Format("保険外金額({0})", j.Name)))) + "\"");
        if (isPutColName)
        {
            retDatas.Add("\"" + string.Join("\",\"", wrkColumns.Union(_jihiSbtMsts.Select(j => string.Format("JihiFutanSbt{0}", j.JihiSbt)))) + "\"");
        }

        //データ
        int totalRow = csvDatas.Count;
        int rowOutputed = 0;
        foreach (var csvData in csvDatas)
        {
            retDatas.Add(RecordData(csvData));
            rowOutputed++;
        }

        string RecordData(CoSta2001PrintData csvData)
        {
            List<string> colDatas = new List<string>();

            foreach (var column in putCurColumns)
            {
                var value = typeof(CoSta2001PrintData).GetProperty(column.CsvColName)?.GetValue(csvData);
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
    #endregion
}
