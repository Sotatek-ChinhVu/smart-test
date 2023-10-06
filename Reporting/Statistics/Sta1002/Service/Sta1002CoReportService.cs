using Helper.Common;
using Helper.Constants;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta1001.Models;
using Reporting.Statistics.Sta1002.DB;
using Reporting.Statistics.Sta1002.Mapper;
using Reporting.Statistics.Sta1002.Models;
using System.Globalization;

namespace Reporting.Statistics.Sta1002.Service;

public class Sta1002CoReportService : ISta1002CoReportService
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly ICoSta1002Finder _finder;
    private readonly IReadRseReportFileService _readRseReportFileService;
    private CoSta1002PrintConf _printConf;
    private List<CoSta1002PrintData> _printDatas;
    private List<string> _headerL1;
    private List<string> _headerL2;
    private List<CoSyunoInfModel>? _syunoInfs;
    private List<CoJihiSbtMstModel> _jihiSbtMsts;
    private List<CoJihiSbtFutan> _jihiSbtFutans;
    private CoHpInfModel _hpInf;
    private List<PutColumn> putCurColumns = new List<PutColumn>();
    private List<PutColumn> csvTotalColumns = new List<PutColumn> { new PutColumn("RowType", "明細区分") };

    public Sta1002CoReportService(ICoSta1002Finder finder, IReadRseReportFileService readRseReportFileService)
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
    private int maxRow = 46;

    private readonly List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("NyukinDateFmt", "入金日", false, "NyukinDate"),
            new PutColumn("UketukeSbt", "受付種別ID"),
            new PutColumn("UketukeSbtName", "受付種別"),
            new PutColumn("KaId", "診療科ID"),
            new PutColumn("KaSname", "診療科"),
            new PutColumn("TantoId", "担当医ID"),
            new PutColumn("TantoSname", "担当医"),
            new PutColumn("HokenSbtName", "保険種別名"),
            new PutColumn("SyosinCount", "初診件数"),
            new PutColumn("SaisinCount", "再診件数"),
            new PutColumn("Count", "合計件数"),
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
            new PutColumn("PostAdjustFutan", "期間外調整額")
        };
    #endregion

    private int _currentPage;
    private string _rowCountFieldName = string.Empty;
    private List<string> _objectRseList;
    private bool _hasNextPage;


    public CommonReportingRequestModel GetSta1002ReportingData(CoSta1002PrintConf printConf, int hpId)
    {
        _printConf = printConf;
        // get data to print
        GetFieldNameList();
        GetRowCount();
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
        return new Sta1002Mapper(_singleFieldData, _tableFieldData, _extralData, _rowCountFieldName).GetData();
    }

    private bool GetData(int hpId)
    {
        void MakePrintData()
        {
            _printDatas = new List<CoSta1002PrintData>();
            _headerL1 = new List<string>();
            _headerL2 = new List<string>();

            //改ページ条件
            bool pbUketukeSbt = new int[] { _printConf.PageBreak1, _printConf.PageBreak2, _printConf.PageBreak3 }.Contains(1);
            bool pbKaId = new int[] { _printConf.PageBreak1, _printConf.PageBreak2, _printConf.PageBreak3 }.Contains(2);
            bool pbTantoId = new int[] { _printConf.PageBreak1, _printConf.PageBreak2, _printConf.PageBreak3 }.Contains(3);

            var nyukinDays = _syunoInfs.GroupBy(s => s.NyukinDate).OrderBy(s => s.Key).Select(s => s.Key).ToList();
            foreach (var nyukinDate in nyukinDays)
            {
                var uketukeSbts = _syunoInfs.GroupBy(s => s.UketukeSbt).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                for (int i = 0; (pbUketukeSbt && i <= uketukeSbts.Count - 1) || i == 0; i++)
                {
                    var kaIds = _syunoInfs.GroupBy(s => s.KaId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                    for (int j = 0; (pbKaId && j <= kaIds.Count - 1) || j == 0; j++)
                    {
                        var tantoIds = _syunoInfs.GroupBy(s => s.TantoId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                        for (int k = 0; (pbTantoId && k <= tantoIds.Count - 1) || k == 0; k++)
                        {
                            var curDatas = _syunoInfs.Where(s =>
                                s.NyukinDate == nyukinDate &&
                                (pbUketukeSbt ? s.UketukeSbt == uketukeSbts[i] : true) &&
                                (pbKaId ? s.KaId == kaIds[j] : true) &&
                                (pbTantoId ? s.TantoId == tantoIds[k] : true)
                            ).ToList();

                            if (curDatas.Count == 0) continue;

                            //明細
                            for (int rowNo = 0; rowNo <= maxRow - 1; rowNo++)
                            {
                                CoSta1002PrintData printData = new CoSta1002PrintData();

                                List<CoSyunoInfModel> wrkDatas = null;
                                switch (rowNo)
                                {
                                    //社保単独
                                    case 0:
                                        printData.HokenSbtName = "社保単独(本人)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Syaho && !s.IsHeiyo && s.IsNrMine).ToList();
                                        break;
                                    case 1:
                                        printData.HokenSbtName = "社保単独(６未)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Syaho && !s.IsHeiyo && s.IsNrPreSchool).ToList();
                                        break;
                                    case 2:
                                        printData.HokenSbtName = "社保単独(家族)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Syaho && !s.IsHeiyo && s.IsNrFamily).ToList();
                                        break;
                                    case 3:
                                        printData.HokenSbtName = "社保単独(高齢)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Syaho && !s.IsHeiyo && s.IsNrElder).ToList();
                                        break;
                                    //社保併用
                                    case 4:
                                        printData.HokenSbtName = "社保併用(本人)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Syaho && s.IsHeiyo && s.IsNrMine).ToList();
                                        break;
                                    case 5:
                                        printData.HokenSbtName = "社保併用(６未)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Syaho && s.IsHeiyo && s.IsNrPreSchool).ToList();
                                        break;
                                    case 6:
                                        printData.HokenSbtName = "社保併用(家族)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Syaho && s.IsHeiyo && s.IsNrFamily).ToList();
                                        break;
                                    case 7:
                                        printData.HokenSbtName = "社保併用(高齢)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Syaho && s.IsHeiyo && s.IsNrElder).ToList();
                                        break;
                                    //社保計
                                    case 8:
                                        printData.HokenSbtName = "≪社保計≫";
                                        printData.RowType = RowType.Total;
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Syaho && s.IsNrAll).ToList();
                                        break;

                                    //公費
                                    case 10:
                                        printData.HokenSbtName = "公費単独";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Syaho && !s.IsHeiyo && s.IsKohiOnly).ToList();
                                        break;
                                    case 11:
                                        printData.HokenSbtName = "公費併用";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Syaho && s.IsHeiyo && s.IsKohiOnly).ToList();
                                        break;
                                    //公費計
                                    case 12:
                                        printData.HokenSbtName = "≪公費計≫";
                                        printData.RowType = RowType.Total;
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Syaho && s.IsKohiOnly).ToList();
                                        break;

                                    //社保合計
                                    case 14:
                                        printData.HokenSbtName = "◆社保合計";
                                        printData.RowType = RowType.Total;
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Syaho).ToList();
                                        break;

                                    //国保単独
                                    case 15:
                                        printData.HokenSbtName = "国保単独(本人)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && !s.IsHeiyo && s.IsNrMine).ToList();
                                        break;
                                    case 16:
                                        printData.HokenSbtName = "国保単独(６未)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && !s.IsHeiyo && s.IsNrPreSchool).ToList();
                                        break;
                                    case 17:
                                        printData.HokenSbtName = "国保単独(家族)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && !s.IsHeiyo && s.IsNrFamily).ToList();
                                        break;
                                    case 18:
                                        printData.HokenSbtName = "国保単独(高齢)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && !s.IsHeiyo && s.IsNrElder).ToList();
                                        break;
                                    //国保併用
                                    case 19:
                                        printData.HokenSbtName = "国保併用(本人)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && s.IsHeiyo && s.IsNrMine).ToList();
                                        break;
                                    case 20:
                                        printData.HokenSbtName = "国保併用(６未)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && s.IsHeiyo && s.IsNrPreSchool).ToList();
                                        break;
                                    case 21:
                                        printData.HokenSbtName = "国保併用(家族)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && s.IsHeiyo && s.IsNrFamily).ToList();
                                        break;
                                    case 22:
                                        printData.HokenSbtName = "国保併用(高齢)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && s.IsHeiyo && s.IsNrElder).ToList();
                                        break;
                                    //国保計
                                    case 23:
                                        printData.HokenSbtName = "≪国保計≫";
                                        printData.RowType = RowType.Total;
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && s.IsNrAll).ToList();
                                        break;

                                    //退職単独
                                    case 25:
                                        printData.HokenSbtName = "退職単独(本人)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && !s.IsHeiyo && s.IsRetMine).ToList();
                                        break;
                                    case 26:
                                        printData.HokenSbtName = "退職単独(６未)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && !s.IsHeiyo && s.IsRetPreSchool).ToList();
                                        break;
                                    case 27:
                                        printData.HokenSbtName = "退職単独(家族)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && !s.IsHeiyo && s.IsRetFamily).ToList();
                                        break;
                                    //退職併用
                                    case 28:
                                        printData.HokenSbtName = "退職併用(本人)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && s.IsHeiyo && s.IsRetMine).ToList();
                                        break;
                                    case 29:
                                        printData.HokenSbtName = "退職併用(６未)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && s.IsHeiyo && s.IsRetPreSchool).ToList();
                                        break;
                                    case 30:
                                        printData.HokenSbtName = "退職併用(家族)";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && s.IsHeiyo && s.IsRetFamily).ToList(); break;
                                    //退職計
                                    case 31:
                                        printData.HokenSbtName = "≪退職計≫";
                                        printData.RowType = RowType.Total;
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && s.IsRetAll).ToList(); break;

                                    //後期
                                    case 33:
                                        printData.HokenSbtName = "後期単独";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && !s.IsHeiyo && s.IsKouki).ToList();
                                        break;
                                    case 34:
                                        printData.HokenSbtName = "後期併用";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && s.IsHeiyo && s.IsKouki).ToList();
                                        break;
                                    //後期計
                                    case 35:
                                        printData.HokenSbtName = "≪後期計≫";
                                        printData.RowType = RowType.Total;
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && s.IsKouki).ToList();
                                        break;

                                    //国保合計
                                    case 37:
                                        printData.HokenSbtName = "◆国保合計";
                                        printData.RowType = RowType.Total;
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho).ToList();
                                        break;

                                    //保険計
                                    case 38:
                                        printData.HokenSbtName = "◆保険合計";
                                        printData.RowType = RowType.Total;
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Syaho || s.HokenKbn == HokenKbn.Kokho).ToList();
                                        break;

                                    //保険外
                                    case 39:
                                        printData.HokenSbtName = "自費";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Jihi && s.IsJihiNoRece).ToList();
                                        break;
                                    case 40:
                                        printData.HokenSbtName = "自費レセ";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Jihi && s.IsJihiRece).ToList();
                                        break;
                                    case 41:
                                        printData.HokenSbtName = "労災";
                                        wrkDatas = curDatas.Where(s => s.IsRousai).ToList();
                                        break;
                                    case 42:
                                        printData.HokenSbtName = "自賠責";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Jibai).ToList();
                                        break;
                                    //保険外計
                                    case 43:
                                        printData.HokenSbtName = "◆保険外計";
                                        printData.RowType = RowType.Total;
                                        wrkDatas = curDatas.Where(s => s.HokenKbn != HokenKbn.Syaho && s.HokenKbn != HokenKbn.Kokho).ToList();
                                        break;

                                    //総合計
                                    case 45:
                                        printData.HokenSbtName = "◆総合計";
                                        printData.RowType = RowType.Total;
                                        wrkDatas = curDatas.ToList();
                                        break;
                                }

                                if (wrkDatas == null)
                                {
                                    //空行を追加
                                    _printDatas.Add(new CoSta1002PrintData(RowType.Brank));
                                    continue;
                                }

                                printData.NyukinDate = nyukinDate;
                                printData.UketukeSbt = pbUketukeSbt ? curDatas.FirstOrDefault().UketukeSbt.ToString() : null;
                                printData.UketukeSbtName = pbUketukeSbt ? curDatas.FirstOrDefault().UketukeSbtName : null;
                                printData.KaId = (pbKaId || kaIds.Count == 1) ? curDatas.FirstOrDefault().KaId.ToString() : null;
                                printData.KaSname = (pbKaId || kaIds.Count == 1) ? curDatas.FirstOrDefault().KaSname : null;
                                printData.TantoId = (pbTantoId || tantoIds.Count == 1) ? curDatas.FirstOrDefault().TantoId.ToString() : null;
                                printData.TantoSname = (pbTantoId || tantoIds.Count == 1) ? curDatas.FirstOrDefault().TantoSname : null;
                                printData.SyosinCount = wrkDatas.Where(s => s.IsSinDate && s.Syosaisin == "初診")
                                    .GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                                printData.SaisinCount = wrkDatas.Where(s => s.IsSinDate && s.Syosaisin == "再診")
                                    .GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                                printData.Count = wrkDatas.Where(s => s.IsSinDate)
                                    .GroupBy(s => s.RaiinNo).Count().ToString("#,0");
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
                                int wrkMisyuSeikyu = 0;
                                foreach (var wrkNyukin in wrkNyukins)
                                {
                                    wrkSeikyu += wrkDatas.Find(s => s.RaiinNo == wrkNyukin.RaiinNo && s.NyukinSortNo == wrkNyukin.NyukinSortNo).SeikyuGaku;
                                    wrkNewSeikyu += wrkDatas.Find(s => s.RaiinNo == wrkNyukin.RaiinNo && s.NyukinSortNo == wrkNyukin.NyukinSortNo).NewSeikyuGaku;
                                    wrkMisyuSeikyu += wrkDatas.Find(s => s.RaiinNo == wrkNyukin.RaiinNo && s.NyukinSortNo == wrkNyukin.NyukinSortNo && s.NyukinKbn != 0)?.SeikyuGaku ?? 0;
                                }
                                printData.SeikyuGaku = wrkSeikyu.ToString("#,0");
                                printData.NewSeikyuGaku = wrkNewSeikyu.ToString("#,0");
                                printData.MenjyoGaku = wrkDatas.Where(s => s.IsSinDate)
                                    .GroupBy(s => s.RaiinNo)
                                    .Select(s => new { RaiinNo = s.Key, MenjyoGaku = s.Max(x => x.MenjyoGaku) })
                                    .Sum(s => s.MenjyoGaku).ToString("#,0");
                                printData.NyukinGaku = wrkDatas.Sum(s => s.IsSinDate ? s.NyukinGaku : 0).ToString("#,0");
                                printData.PostNyukinGaku = wrkDatas.Sum(s => !s.IsSinDate ? s.NyukinGaku : 0).ToString("#,0");
                                printData.PostAdjustFutan = wrkDatas.Sum(s => !s.IsSinDate ? s.NyukinAdjustFutan : 0).ToString("#,0");
                                printData.MisyuGaku =
                                    (
                                        wrkMisyuSeikyu -
                                        int.Parse(printData.NyukinGaku ?? "0", NumberStyles.Any)
                                    ).ToString("#,0");

                                //自費種別ごとの金額
                                List<string> wrkPrintFutans = new List<string>();

                                foreach (var jihiSbtMst in _jihiSbtMsts)
                                {
                                    int wrkFutan = 0;
                                    foreach (var wrkData in wrkDatas)
                                    {
                                        wrkFutan += _jihiSbtFutans.Find(f => f.RaiinNo == wrkData.RaiinNo && f.JihiSbt == jihiSbtMst.JihiSbt)?.JihiFutan ?? 0;
                                    }
                                    wrkPrintFutans.Add(wrkFutan.ToString("#,0"));
                                }
                                printData.JihiSbtFutans = wrkPrintFutans;

                                //行追加
                                _printDatas.Add(printData);

                                //ヘッダー情報
                                if ((int)Math.Ceiling((double)(_printDatas.Count) / maxRow) > _headerL1.Count)
                                {
                                    //入金日
                                    string wrkNyukinDate = CIUtil.SDateToShowSWDate(curDatas.First().NyukinDate, 0, 1);
                                    if (_printConf.StartNyukinTime >= 0 || _printConf.EndNyukinTime >= 0)
                                    {
                                        wrkNyukinDate +=
                                            string.Format(
                                                " {0}～{1}",
                                                _printConf.StartNyukinTime == -1 ? "" : CIUtil.TryCIToTimeZone(_printConf.StartNyukinTime),
                                                _printConf.EndNyukinTime == -1 ? "" : CIUtil.TryCIToTimeZone(_printConf.EndNyukinTime)
                                            );
                                    }
                                    _headerL1.Add(wrkNyukinDate);
                                    //改ページ条件
                                    List<string> wrkHeaders = new List<string>();
                                    if (pbUketukeSbt) wrkHeaders.Add(curDatas.First().UketukeSbtName);
                                    if (pbKaId) wrkHeaders.Add(curDatas.First().KaSname);
                                    if (pbTantoId) wrkHeaders.Add(curDatas.First().TantoSname);

                                    if (wrkHeaders.Count >= 1) _headerL2.Add(string.Join("／", wrkHeaders));
                                }
                            }
                        }
                    }
                }
            }
        }

        _hpInf = _finder.GetHpInf(hpId, _printConf.StartNyukinDate);

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
            _extralData.Add("HeaderL_0_1_" + _currentPage, _headerL1.Count >= _currentPage ? _headerL1[_currentPage - 1] : string.Empty);

            //改ページ条件
            _extralData.Add("HeaderL_0_2_" + _currentPage, _headerL2.Count >= _currentPage ? _headerL2[_currentPage - 1] : string.Empty);

            //期間
            SetFieldData("Range",
                string.Format(
                    "期間: {0} ～ {1}",
                    CIUtil.SDateToShowSWDate(_printConf.StartNyukinDate, 0, 1),
                    CIUtil.SDateToShowSWDate(_printConf.EndNyukinDate, 0, 1)
                )
            );
        }
        #endregion

        #region Body
        void UpdateFormBody()
        {
            int hokIndex = (_currentPage - 1) * maxRow;
            //存在しているフィールドに絞り込み
            var existsCols = putColumns.Where(p => _objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                Dictionary<string, CellModel> data = new();
                var printData = _printDatas[hokIndex];
                string baseListName = string.Empty;

                //保険外金額（内訳）タイトル
                foreach (var jihiSbtMst in _jihiSbtMsts)
                {
                    SetFieldData(string.Format("tJihiFutanSbt{0}", jihiSbtMst.JihiSbt), jihiSbtMst.Name);
                }

                //明細データ出力
                foreach (var colName in existsCols)
                {
                    if (colName == null)
                    {
                        continue;
                    }
                    var value = typeof(CoSta1002PrintData).GetProperty(colName).GetValue(printData);
                    string valueInput = value?.ToString() ?? string.Empty;

                    if (!data.ContainsKey(colName))
                    {
                        AddListData(ref data, colName, valueInput);
                    }

                    if (baseListName == string.Empty && _objectRseList.Contains(colName))
                    {
                        baseListName = colName;
                    }
                }
                //自費種別毎の金額
                for (int i = 0; i <= _jihiSbtMsts.Count - 1; i++)
                {
                    if (printData.JihiSbtFutans == null)
                    {
                        break;
                    }

                    var jihiSbtMst = _jihiSbtMsts[i];
                    AddListData(ref data, string.Format("JihiFutanSbt{0}", jihiSbtMst.JihiSbt), printData.JihiSbtFutans[i]);
                }

                //区切り線を引く
                if (new int[] { 14, 37, 38, 43 }.Contains(rowNo))
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
                if (hokIndex >= _printDatas.Count)
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

    private void GetFieldNameList()
    {
        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta1002, string.Empty, new());
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

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta1002, string.Empty, fieldInputList);
        var javaOutputData = _readRseReportFileService.ReadFileRse(data);
        maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == _rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? maxRow;
    }

    public CommonExcelReportingModel ExportCsv(CoSta1002PrintConf printConf, int dateFrom, int dateTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow)
    {
        string fileName = menuName + "_" + dateFrom + "_" + dateTo;
        List<string> retDatas = new List<string>();
        _printConf = printConf;
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
        int totalRow = csvDatas.Count;
        int rowOutputed = 0;
        foreach (var csvData in csvDatas)
        {
            retDatas.Add(RecordData(csvData));
            rowOutputed++;
        }
        string RecordData(CoSta1002PrintData csvData)
        {
            List<string> colDatas = new List<string>();

            foreach (var column in putCurColumns)
            {
                var value = typeof(CoSta1002PrintData).GetProperty(column.CsvColName)?.GetValue(csvData);
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
