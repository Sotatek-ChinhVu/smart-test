using Helper.Common;
using Helper.Extension;
using Reporting.DailyStatic.DB;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta1001.DB;
using Reporting.Statistics.Sta1001.Mapper;
using Reporting.Statistics.Sta1001.Models;
using System.Globalization;

namespace Reporting.Statistics.Sta1001.Service
{
    public class Sta1001CoReportService : ISta1001CoReportService
    {
        private List<CoSta1001PrintData> printDatas;
        private List<string> headerL1;
        private List<string> headerL2;
        private List<CoSyunoInfModel> syunoInfs;
        private List<CoJihiSbtMstModel> jihiSbtMsts;
        private List<CoJihiSbtFutan> jihiSbtFutans;
        private CoHpInfModel hpInf;
        private CoSta1001PrintConf _printConf;
        private readonly List<PutColumn> putCurColumns = new();
        private CountData grandTotal = new CountData();
        private CountData total = new CountData();
        private CountData subTotal = new CountData();

        private int maxRow = 43;
        private int HpId;
        private List<string> _objectRseList;
        private int _currentPage;
        private string _rowCountFieldName = string.Empty;
        private bool _hasNextPage;

        private readonly Dictionary<string, string> _extralData = new();
        private readonly Dictionary<string, string> SingleData = new();
        private readonly List<Dictionary<string, CellModel>> CellData = new();

        private readonly ICoSta1001Finder _sta1001Finder;
        private readonly IReadRseReportFileService _readRseReportFileService;
        private readonly IDailyStatisticCommandFinder _dailyStatisticCommandFinder;

        #region Constant
        private readonly List<PutColumn> csvTotalColumns = new List<PutColumn>
        {
            new PutColumn("RowType", "明細区分"),
            new PutColumn("TotalCaption", "合計行"),
            new PutColumn("TotalCount", "合計件数"),
            new PutColumn("TotalPtCount", "合計実人数")
        };

        private readonly List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("NyukinDateFmt", "入金日", false, "NyukinDate"),
            new PutColumn("UketukeSbt", "受付種別ID", false),
            new PutColumn("UketukeSbtName", "受付種別", false),
            new PutColumn("KaId", "診療科ID", false),
            new PutColumn("KaSname", "診療科", false),
            new PutColumn("TantoId", "担当医ID", false),
            new PutColumn("TantoSname", "担当医", false),
            new PutColumn("PtNum", "患者番号", false),
            new PutColumn("PtKanaName", "カナ氏名", false),
            new PutColumn("PtName", "氏名", false),
            new PutColumn("HokenSbt", "保険種別", false),
            new PutColumn("Syosaisin", "初再診", false),
            new PutColumn("SinDateFmt", "診療日", false, "SinDate"),
            new PutColumn("UketukeId", "受付者ID", false),
            new PutColumn("UketukeSname", "受付者", false),
            new PutColumn("RaiinNo", "来院番号", false),
            new PutColumn("OyaRaiinNo", "親来院番号", false),
            new PutColumn("UketukeTime", "来院時間", false),
            new PutColumn("KaikeiTime", "精算時間", false),
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
            new PutColumn("PreNyukinGaku", "前回入金額"),
            new PutColumn("MisyuGaku", "未収額"),
            new PutColumn("PayCd", "支払区分コード", false),
            new PutColumn("PaySname", "支払区分", false),
            new PutColumn("NyukinUserId", "入金者ID", false),
            new PutColumn("NyukinUserSname", "入金者", false),
            new PutColumn("NyukinTime", "入金時間", false),
            new PutColumn("NyukinKbn", "入金区分", false),
            new PutColumn("RaiinCmt", "来院コメント", false),
            new PutColumn("NyukinCmt", "入金コメント", false),
            new PutColumn("Seq", "No.", false)
        };
        #endregion

        private struct CountData
        {
            public int Count;
            public List<string> PtCount;
            public int Tensu;
            public int NewTensu;
            public int PtFutan;
            public int PtFutanJihiRece;
            public int PtFutanElse;
            public int JihiFutan;
            public int JihiTax;
            public int AdjustFutan;
            public int MenjyoGaku;
            public int SeikyuGaku;
            public int NewSeikyuGaku;
            public int NyukinGaku;
            public int PreNyukinGaku;
            public int MisyuGaku;
            public int JihiFutanTaxFree;
            public int JihiFutanTaxNrSum;
            public int JihiFutanTaxGenSum;
            public int JihiTaxNrSum;
            public int JihiTaxGenSum;
            public int JihiFutanTaxNr;
            public int JihiFutanTaxGen;
            public int JihiTaxNr;
            public int JihiTaxGen;
            public int JihiFutanOuttaxNr;
            public int JihiFutanOuttaxGen;
            public int JihiOuttaxNr;
            public int JihiOuttaxGen;
            public List<int> JihiSbtFutans;

            public void AddValue(CoSta1001PrintData printData, int decMisyuGaku)
            {
                Count++;
                if (PtCount == null)
                {
                    PtCount = new List<string>();
                }
                if (printData.PtNum.AsString() != "" && !PtCount.Contains(printData.PtNum))
                {
                    PtCount.Add(printData.PtNum);
                }
                Tensu += int.Parse(printData.Tensu ?? "0", NumberStyles.Any);
                NewTensu += int.Parse(printData.NewTensu ?? "0", NumberStyles.Any);
                PtFutan += int.Parse(printData.PtFutan ?? "0", NumberStyles.Any);
                PtFutanJihiRece += int.Parse(printData.PtFutanJihiRece ?? "0", NumberStyles.Any);
                PtFutanElse += int.Parse(printData.PtFutanElse ?? "0", NumberStyles.Any);
                JihiFutan += int.Parse(printData.JihiFutan ?? "0", NumberStyles.Any);
                JihiTax += int.Parse(printData.JihiTax ?? "0", NumberStyles.Any);
                AdjustFutan += int.Parse(printData.AdjustFutan ?? "0", NumberStyles.Any);
                MenjyoGaku += int.Parse(printData.MenjyoGaku ?? "0", NumberStyles.Any);
                SeikyuGaku += int.Parse(printData.SeikyuGaku ?? "0", NumberStyles.Any);
                NewSeikyuGaku += int.Parse(printData.NewSeikyuGaku ?? "0", NumberStyles.Any);
                NyukinGaku += int.Parse(printData.NyukinGaku ?? "0", NumberStyles.Any);
                PreNyukinGaku += int.Parse(printData.PreNyukinGaku ?? "0", NumberStyles.Any);
                MisyuGaku += int.Parse(printData.MisyuGaku ?? "0", NumberStyles.Any);
                MisyuGaku -= decMisyuGaku;
                JihiFutanTaxFree += int.Parse(printData.JihiFutanTaxFree ?? "0", NumberStyles.Any);
                JihiFutanTaxNrSum += int.Parse(printData.JihiFutanTaxNrSum ?? "0", NumberStyles.Any);
                JihiFutanTaxGenSum += int.Parse(printData.JihiFutanTaxGenSum ?? "0", NumberStyles.Any);
                JihiTaxNrSum += int.Parse(printData.JihiTaxNrSum ?? "0", NumberStyles.Any);
                JihiTaxGenSum += int.Parse(printData.JihiTaxGenSum ?? "0", NumberStyles.Any);
                JihiFutanTaxNr += int.Parse(printData.JihiFutanTaxNr ?? "0", NumberStyles.Any);
                JihiFutanTaxGen += int.Parse(printData.JihiFutanTaxGen ?? "0", NumberStyles.Any);
                JihiTaxNr += int.Parse(printData.JihiTaxNr ?? "0", NumberStyles.Any);
                JihiTaxGen += int.Parse(printData.JihiTaxGen ?? "0", NumberStyles.Any);
                JihiFutanOuttaxNr += int.Parse(printData.JihiFutanOuttaxNr ?? "0", NumberStyles.Any);
                JihiFutanOuttaxGen += int.Parse(printData.JihiFutanOuttaxGen ?? "0", NumberStyles.Any);
                JihiOuttaxNr += int.Parse(printData.JihiOuttaxNr ?? "0", NumberStyles.Any);
                JihiOuttaxGen += int.Parse(printData.JihiOuttaxGen ?? "0", NumberStyles.Any);

                if (printData.JihiSbtFutans != null)
                {
                    // check if JihiSbtFutans is null or empty, set default JihiSbtFutans data
                    if (JihiSbtFutans == null || !JihiSbtFutans.Any())
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
                Tensu = 0;
                NewTensu = 0;
                PtFutan = 0;
                PtFutanJihiRece = 0;
                PtFutanElse = 0;
                JihiFutan = 0;
                JihiTax = 0;
                AdjustFutan = 0;
                MenjyoGaku = 0;
                SeikyuGaku = 0;
                NewSeikyuGaku = 0;
                NyukinGaku = 0;
                PreNyukinGaku = 0;
                MisyuGaku = 0;
                JihiFutanTaxFree = 0;
                JihiFutanTaxNrSum = 0;
                JihiFutanTaxGenSum = 0;
                JihiTaxNrSum = 0;
                JihiTaxGenSum = 0;
                JihiFutanTaxNr = 0;
                JihiFutanTaxGen = 0;
                JihiTaxNr = 0;
                JihiTaxGen = 0;
                JihiFutanOuttaxNr = 0;
                JihiFutanOuttaxGen = 0;
                JihiOuttaxNr = 0;
                JihiOuttaxGen = 0;
                JihiSbtFutans = new();
            }
        }

        public Sta1001CoReportService(ICoSta1001Finder sta1001Finder, IReadRseReportFileService readRseReportFileService, IDailyStatisticCommandFinder dailyStatisticCommandFinder)
        {
            _sta1001Finder = sta1001Finder;
            _readRseReportFileService = readRseReportFileService;
            _dailyStatisticCommandFinder = dailyStatisticCommandFinder;
            _objectRseList = new();
            printDatas = new();
            headerL1 = new();
            headerL2 = new();
            syunoInfs = new();
            jihiSbtFutans = new();
            jihiSbtMsts = new();
            _printConf = new();
            hpInf = new();
        }

        public CommonReportingRequestModel GetSta1001ReportingData(CoSta1001PrintConf printConf, int hpId)
        {
            try
            {
                _printConf = printConf;

                HpId = hpId;
                GetFieldNameList();
                GetRowCount();
                putCurColumns.AddRange(putColumns);
                if (GetData())
                {
                    _hasNextPage = true;
                    _currentPage = 1;
                    while (_hasNextPage)
                    {
                        UpdateDrawForm();
                        _currentPage++;
                    }
                }
                return new Sta1001Mapper(_extralData, SingleData, CellData, _rowCountFieldName).GetData();
            }
            finally
            {
                _sta1001Finder.ReleaseResource();
                _dailyStatisticCommandFinder.ReleaseResource();
            }
        }

        private void GetFieldNameList()
        {
            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta1001, "sta1001a.rse", new());
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

            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta1001, "sta1001a.rse", fieldInputList);
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == _rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? maxRow;
        }

        private bool GetData()
        {
            void MakePrintData()
            {
                printDatas = new List<CoSta1001PrintData>();
                headerL1 = new List<string>();
                headerL2 = new List<string>();
                int pageCount = 1;
                int recNo = 0;

                //改ページ条件
                bool pbUketukeSbt = new int[] { _printConf.PageBreak1, _printConf.PageBreak2, _printConf.PageBreak3 }.Contains(1);
                bool pbKaId = new int[] { _printConf.PageBreak1, _printConf.PageBreak2, _printConf.PageBreak3 }.Contains(2);
                bool pbTantoId = new int[] { _printConf.PageBreak1, _printConf.PageBreak2, _printConf.PageBreak3 }.Contains(3);

                #region ソート順
                syunoInfs =
                    syunoInfs!
                        .OrderBy(s => s.NyukinDate)
                        .ThenBy(s => pbUketukeSbt ? s.UketukeSbt : 0)
                        .ThenBy(s => pbKaId ? s.KaId : 0)
                        .ThenBy(s => pbTantoId ? s.TantoId : 0)
                        .ThenBy(s => s.NyukinDate == s.SinDate ? "0" : "1")
                        .ThenBy(s =>
                            _printConf.SortOpt1 == 1 ? "0" :
                            _printConf.SortOrder1 == 1 ? s.PtKanaName :
                            _printConf.SortOrder1 == 2 ? s.PtNum.ToString().PadLeft(10, '0') :
                            _printConf.SortOrder1 == 3 ? s.UketukeTime :
                            _printConf.SortOrder1 == 4 ? s.KaikeiTime : "0")
                        .ThenByDescending(s =>
                           _printConf.SortOpt1 == 0 ? "0" :
                           _printConf.SortOrder1 == 1 ? s.PtKanaName :
                           _printConf.SortOrder1 == 2 ? s.PtNum.ToString().PadLeft(10, '0') :
                           _printConf.SortOrder1 == 3 ? s.UketukeTime :
                           _printConf.SortOrder1 == 4 ? s.KaikeiTime : "0")
                        .ThenBy(s =>
                            _printConf.SortOpt2 == 1 ? "0" :
                            _printConf.SortOrder2 == 1 ? s.PtKanaName :
                            _printConf.SortOrder2 == 2 ? s.PtNum.ToString().PadLeft(10, '0') :
                            _printConf.SortOrder2 == 3 ? s.UketukeTime :
                            _printConf.SortOrder2 == 4 ? s.KaikeiTime : "0")
                        .ThenByDescending(s =>
                            _printConf.SortOpt2 == 0 ? "0" :
                            _printConf.SortOrder2 == 1 ? s.PtKanaName :
                            _printConf.SortOrder2 == 2 ? s.PtNum.ToString().PadLeft(10, '0') :
                            _printConf.SortOrder2 == 3 ? s.UketukeTime :
                            _printConf.SortOrder2 == 4 ? s.KaikeiTime : "0")
                        .ThenBy(s =>
                            _printConf.SortOpt3 == 1 ? "0" :
                            _printConf.SortOrder3 == 1 ? s.PtKanaName :
                            _printConf.SortOrder3 == 2 ? s.PtNum.ToString().PadLeft(10, '0') :
                            _printConf.SortOrder3 == 3 ? s.UketukeTime :
                            _printConf.SortOrder3 == 4 ? s.KaikeiTime : "0")
                        .ThenByDescending(s =>
                            _printConf.SortOpt3 == 0 ? "0" :
                            _printConf.SortOrder3 == 1 ? s.PtKanaName :
                            _printConf.SortOrder3 == 2 ? s.PtNum.ToString().PadLeft(10, '0') :
                            _printConf.SortOrder3 == 3 ? s.UketukeTime :
                            _printConf.SortOrder3 == 4 ? s.KaikeiTime : "0")
                        .ThenBy(s => s.PtNum)
                        .ThenBy(s => s.RaiinNo)
                        .ThenBy(s => s.NyukinSortNo)
                        .ToList();
                #endregion

                foreach (var syunoInf in syunoInfs)
                {
                    CoSta1001PrintData printData = new CoSta1001PrintData();
                    CoSta1001PrintData prePrintData = printDatas.Count >= 1 ? printDatas.Last() : new CoSta1001PrintData() { UketukeSbt = -1 };

                    //改ページ条件
                    bool pageBreak =
                        (printDatas.Count >= 1 && syunoInf.NyukinDate != prePrintData.NyukinDate) ||
                        (pbUketukeSbt && syunoInf.UketukeSbt != prePrintData.UketukeSbt && prePrintData.UketukeSbt >= 0) ||
                        (pbKaId && syunoInf.KaId != prePrintData.KaId && prePrintData.KaId > 0) ||
                        (pbTantoId && syunoInf.TantoId != prePrintData.TantoId && prePrintData.TantoId > 0);

                    if (printDatas.Count >= 1 &&
                        prePrintData.NyukinDate == prePrintData.SinDate &&
                        (pageBreak || syunoInf.SinDate != prePrintData.SinDate))
                    {
                        //当日分合計
                        AddTotalRecord("◆当日分合計", ref subTotal);
                        //空行を追加
                        printDatas.Add(new CoSta1001PrintData(RowType.Brank));
                    }

                    if (printDatas.Count >= 1 &&
                        prePrintData.NyukinDate != prePrintData.SinDate &&
                        pageBreak)
                    {
                        //期間外合計
                        AddTotalRecord("◆期間外合計", ref subTotal);
                        //空行を追加
                        printDatas.Add(new CoSta1001PrintData(RowType.Brank));
                    }

                    if (pageBreak)
                    {
                        recNo = 0;

                        //合計
                        AddTotalRecord("◆合計", ref total);

                        //改ページ
                        while (printDatas.Count % maxRow != 0)
                        {
                            //空行を追加
                            printDatas.Add(new CoSta1001PrintData(RowType.Brank));
                        }
                        pageCount++;

                        //ヘッダー情報
                        while ((int)Math.Ceiling((double)(printDatas.Count) / maxRow) > headerL1.Count && headerL1.Count >= 1)
                        {
                            headerL1.Add(headerL1.Last());
                            if (headerL2.Count >= 1)
                            {
                                headerL2.Add(headerL2.Last());
                            }
                        }
                    }

                    if (syunoInf.RaiinNo != prePrintData.RaiinNo)
                    {
                        //連番
                        recNo++;
                        printData.Seq = recNo.ToString();
                    }

                    /// todo quan.to
                    ///if (syunoInf.RaiinNo != prePrintData.RaiinNo || pageBreak || outputFileType == CoFileType.Csv)
                    if (syunoInf.RaiinNo != prePrintData.RaiinNo || pageBreak)
                    {
                        printData.PtNum = syunoInf.PtNum.ToString();
                        printData.PtName = syunoInf.PtName;
                        printData.PtKanaName = syunoInf.PtKanaName;
                        printData.Syosaisin = syunoInf.Syosaisin;
                        printData.HokenSbt = syunoInf.HokenSbt;

                        /// todo quan.to
                        ///bool csvOmit = syunoInf.RaiinNo == prePrintData.RaiinNo && outputFileType == CoFileType.Csv;
                        bool csvOmit = syunoInf.RaiinNo == prePrintData.RaiinNo;
                        printData.Tensu = csvOmit ? "0" : syunoInf.Tensu.ToString("#,0");
                        printData.NewTensu = csvOmit ? "0" : syunoInf.NewTensu.ToString("#,0");
                        printData.PtFutan = csvOmit ? "0" : syunoInf.PtFutan.ToString("#,0");
                        var ptFutanJihiReceDisplay = syunoInf.IsJihiRece ? syunoInf.PtFutan : 0;
                        var ptFutanElseDisplay = !syunoInf.IsJihiRece ? syunoInf.PtFutan : 0;
                        printData.PtFutanJihiRece = csvOmit ? "0" : ptFutanJihiReceDisplay.ToString("#,0");
                        printData.PtFutanElse = csvOmit ? "0" : ptFutanElseDisplay.ToString("#,0");
                        printData.JihiFutan = csvOmit ? "0" : syunoInf.JihiFutan.ToString("#,0");
                        printData.JihiTax = csvOmit ? "0" : syunoInf.JihiTax.ToString("#,0");
                        printData.JihiFutanTaxFree = csvOmit ? "0" : syunoInf.JihiFutanTaxFree.ToString("#,0");
                        printData.JihiFutanTaxNrSum = csvOmit ? "0" : syunoInf.JihiFutanTaxNrSum.ToString("#,0");
                        printData.JihiFutanTaxGenSum = csvOmit ? "0" : syunoInf.JihiFutanTaxGenSum.ToString("#,0");
                        printData.JihiTaxNrSum = csvOmit ? "0" : syunoInf.JihiTaxNrSum.ToString("#,0");
                        printData.JihiTaxGenSum = csvOmit ? "0" : syunoInf.JihiTaxGenSum.ToString("#,0");
                        printData.JihiFutanTaxNr = csvOmit ? "0" : syunoInf.JihiFutanTaxNr.ToString("#,0");
                        printData.JihiFutanTaxGen = csvOmit ? "0" : syunoInf.JihiFutanTaxGen.ToString("#,0");
                        printData.JihiTaxNr = csvOmit ? "0" : syunoInf.JihiTaxNr.ToString("#,0");
                        printData.JihiTaxGen = csvOmit ? "0" : syunoInf.JihiTaxGen.ToString("#,0");
                        printData.JihiFutanOuttaxNr = csvOmit ? "0" : syunoInf.JihiFutanOuttaxNr.ToString("#,0");
                        printData.JihiFutanOuttaxGen = csvOmit ? "0" : syunoInf.JihiFutanOuttaxGen.ToString("#,0");
                        printData.JihiOuttaxNr = csvOmit ? "0" : syunoInf.JihiOuttaxNr.ToString("#,0");
                        printData.JihiOuttaxGen = csvOmit ? "0" : syunoInf.JihiOuttaxGen.ToString("#,0");
                        printData.AdjustFutan = csvOmit ? "0" : syunoInf.AdjustFutan.ToString("#,0");
                        printData.MenjyoGaku = csvOmit ? "0" : syunoInf.MenjyoGaku.ToString("#,0");
                        printData.SeikyuGaku = csvOmit ? "0" :
                            syunoInfs.First(s =>
                                s.RaiinNo == syunoInf.RaiinNo &&
                                s.NyukinSortNo == syunoInfs.Where(x => x.RaiinNo == syunoInf.RaiinNo).Max(x => x.NyukinSortNo)
                            ).SeikyuGaku.ToString("#,0");
                        printData.NewSeikyuGaku = csvOmit ? "0" :
                            syunoInfs.First(s =>
                                s.RaiinNo == syunoInf.RaiinNo &&
                                s.NyukinSortNo == syunoInfs.Where(x => x.RaiinNo == syunoInf.RaiinNo).Max(x => x.NyukinSortNo)
                            ).NewSeikyuGaku.ToString("#,0");
                        if (!csvOmit)
                        {
                            List<string> wrkPrintFutans = new List<string>();

                            foreach (var jihiSbtMst in jihiSbtMsts)
                            {
                                var wrkFutan = jihiSbtFutans.Find(j => j.RaiinNo == syunoInf.RaiinNo && j.JihiSbt == jihiSbtMst.JihiSbt);
                                wrkPrintFutans.Add(wrkFutan == null ? "0" : wrkFutan.JihiFutan.ToString("#,0"));
                            }
                            printData.JihiSbtFutans = wrkPrintFutans;
                        }

                        printData.KaSname = syunoInf.KaSname;
                        printData.TantoSname = syunoInf.TantoSname;
                        printData.UketukeId = syunoInf.UketukeId;
                        printData.UketukeSname = syunoInf.UketukeSname;

                        string uketukeTime = syunoInf.UketukeTime;
                        if (uketukeTime.AsString() != string.Empty)
                        {
                            uketukeTime = uketukeTime.PadLeft(4, '0');
                            printData.UketukeTime = uketukeTime.Substring(0, 2) + ":" + uketukeTime.Substring(2, 2);
                        }
                        string kaikeiTime = syunoInf.KaikeiTime;
                        if (kaikeiTime.AsString() != string.Empty)
                        {
                            kaikeiTime = kaikeiTime.PadLeft(4, '0');
                            printData.KaikeiTime = kaikeiTime.Substring(0, 2) + ":" + kaikeiTime.Substring(2, 2);
                        }

                        printData.NyukinKbn = syunoInf.NyukinKbnName;
                        printData.RaiinCmt = syunoInf.RaiinCmt;
                    }
                    //キー情報
                    printData.RaiinNo = syunoInf.RaiinNo;
                    printData.OyaRaiinNo = syunoInf.OyaRaiinNo;
                    printData.SinDate = syunoInf.SinDate;
                    printData.NyukinDate = syunoInf.NyukinDate;
                    printData.UketukeSbt = syunoInf.UketukeSbt;
                    printData.KaId = syunoInf.KaId;
                    printData.TantoId = syunoInf.TantoId;
                    //入金情報
                    printData.PaySname = syunoInf.PaySName;
                    printData.PayCd = syunoInf.PayCd;
                    printData.NyukinGaku = syunoInf.NyukinGaku.ToString("#,0");
                    printData.PreNyukinGaku = syunoInf.NyukinDate != syunoInf.SinDate ? syunoInf.PreNyukinGaku.ToString("#,0") : "0";
                    printData.MisyuGaku = syunoInf.MisyuGaku.ToString("#,0");
                    printData.UketukeSbtName = syunoInf.UketukeSbtName;
                    printData.NyukinUserId = syunoInf.NyukinUserId;
                    printData.NyukinUserSname = syunoInf.NyukinUserSname;
                    printData.NyukinTime = CIUtil.TryCIToTimeZone(syunoInf.NyukinTime);
                    printData.NyukinCmt = syunoInf.NyukinCmt;

                    //同来院に同一日複数回入金されている場合は日単位で未収を計算する
                    int decMisyuGaku = 0;
                    if (syunoInf.RaiinNo == prePrintData.RaiinNo)
                    {
                        decMisyuGaku =
                            int.Parse(prePrintData.MisyuGaku ?? "0", NumberStyles.Any);

                        prePrintData.MisyuGaku = "0";
                    }

                    //合計
                    grandTotal.AddValue(printData, decMisyuGaku);
                    total.AddValue(printData, decMisyuGaku);
                    subTotal.AddValue(printData, decMisyuGaku);

                    //行追加
                    printDatas.Add(printData);

                    //ヘッダー情報
                    if ((int)Math.Ceiling((double)(printDatas.Count) / maxRow) > headerL1.Count)
                    {
                        //入金日
                        string wrkNyukinDate = CIUtil.SDateToShowSWDate(printData.NyukinDate, 0, 1);
                        if (_printConf.StartNyukinTime >= 0 || _printConf.EndNyukinTime >= 0)
                        {
                            wrkNyukinDate +=
                                string.Format(
                                    " {0}～{1}",
                                    _printConf.StartNyukinTime == -1 ? "" : CIUtil.TryCIToTimeZone(_printConf.StartNyukinTime),
                                    _printConf.EndNyukinTime == -1 ? "" : CIUtil.TryCIToTimeZone(_printConf.EndNyukinTime)
                                );
                        }
                        headerL1.Add(wrkNyukinDate);
                        //改ページ条件
                        List<string> wrkHeaders = new List<string>();
                        if (pbUketukeSbt) wrkHeaders.Add(printData.UketukeSbtName);
                        if (pbKaId) wrkHeaders.Add(printData.KaSname);
                        if (pbTantoId) wrkHeaders.Add(printData.TantoSname);

                        if (wrkHeaders.Count >= 1) headerL2.Add(string.Join("／", wrkHeaders));
                    }
                }

                //合計行
                if (printDatas.Last().NyukinDate == printDatas.Last().SinDate)
                {
                    AddTotalRecord("◆当日分合計", ref subTotal);
                }
                else
                {
                    AddTotalRecord("◆期間外合計", ref subTotal);
                }

                //空行を追加
                printDatas.Add(new CoSta1001PrintData(RowType.Brank));
                //合計
                AddTotalRecord("◆合計", ref total);

                //ヘッダー情報
                while ((int)Math.Ceiling((double)(printDatas.Count) / maxRow) > headerL1.Count && headerL1.Count >= 1)
                {
                    headerL1.Add(headerL1.Last());
                    if (headerL2.Count >= 1)
                    {
                        headerL2.Add(headerL2.Last());
                    }
                }

                if (pageCount >= 2)
                {
                    //空行を追加
                    printDatas.Add(new CoSta1001PrintData(RowType.Brank));
                    printDatas.Add(new CoSta1001PrintData(RowType.Brank));

                    //総合計
                    AddTotalRecord("◆総合計", ref grandTotal);
                }
            }

            //合計レコードの追加
            void AddTotalRecord(string title, ref CountData totalData)
            {
                printDatas.Add(
                    new CoSta1001PrintData(RowType.Total)
                    {
                        TotalCaption = title,
                        TotalCount = totalData.Count.ToString("#,0件"),
                        TotalPtCount = totalData.PtCount.Count.ToString("(#,0人)"),
                        Tensu = totalData.Tensu.ToString("#,0"),
                        NewTensu = totalData.NewTensu.ToString("#,0"),
                        PtFutan = totalData.PtFutan.ToString("#,0"),
                        PtFutanJihiRece = totalData.PtFutanJihiRece.ToString("#,0"),
                        PtFutanElse = totalData.PtFutanElse.ToString("#,0"),
                        JihiFutan = totalData.JihiFutan.ToString("#,0"),
                        JihiTax = totalData.JihiTax.ToString("#,0"),
                        AdjustFutan = totalData.AdjustFutan.ToString("#,0"),
                        MenjyoGaku = totalData.MenjyoGaku.ToString("#,0"),
                        SeikyuGaku = totalData.SeikyuGaku.ToString("#,0"),
                        NewSeikyuGaku = totalData.NewSeikyuGaku.ToString("#,0"),
                        NyukinGaku = totalData.NyukinGaku.ToString("#,0"),
                        PreNyukinGaku = totalData.PreNyukinGaku.ToString("#,0"),
                        MisyuGaku = totalData.MisyuGaku.ToString("#,0"),
                        JihiFutanTaxFree = totalData.JihiFutanTaxFree.ToString("#,0"),
                        JihiFutanTaxNrSum = totalData.JihiFutanTaxNrSum.ToString("#,0"),
                        JihiFutanTaxGenSum = totalData.JihiFutanTaxGenSum.ToString("#,0"),
                        JihiTaxNrSum = totalData.JihiTaxNrSum.ToString("#,0"),
                        JihiTaxGenSum = totalData.JihiTaxGenSum.ToString("#,0"),
                        JihiFutanTaxNr = totalData.JihiFutanTaxNr.ToString("#,0"),
                        JihiFutanTaxGen = totalData.JihiFutanTaxGen.ToString("#,0"),
                        JihiTaxNr = totalData.JihiTaxNr.ToString("#,0"),
                        JihiTaxGen = totalData.JihiTaxGen.ToString("#,0"),
                        JihiFutanOuttaxNr = totalData.JihiFutanOuttaxNr.ToString("#,0"),
                        JihiFutanOuttaxGen = totalData.JihiFutanOuttaxGen.ToString("#,0"),
                        JihiOuttaxNr = totalData.JihiOuttaxNr.ToString("#,0"),
                        JihiOuttaxGen = totalData.JihiOuttaxGen.ToString("#,0"),
                        JihiSbtFutans = totalData.JihiSbtFutans?.Select(j => j.ToString("#,0")).ToList() ?? new()
                    }
                );
                totalData.Clear();
            }

            hpInf = _sta1001Finder.GetHpInf(HpId, _printConf.StartNyukinDate);

            syunoInfs = _sta1001Finder.GetSyunoInfs(HpId, _printConf, 0);
            if ((syunoInfs?.Count ?? 0) == 0) return false;

            jihiSbtMsts = _sta1001Finder.GetJihiSbtMst(HpId);
            jihiSbtFutans = _sta1001Finder.GetJihiSbtFutan(HpId, _printConf);

            //来院コメントの取得
            if (_objectRseList.Contains("RaiinCmt"))
            {
                foreach (var syunoInf in syunoInfs!)
                {
                    syunoInf.RaiinCmt = _sta1001Finder.GetRaiinCmtInf(HpId, syunoInf.RaiinNo);
                }
            }

            //印刷用データの作成
            MakePrintData();

            return printDatas.Count > 0;
        }

        private void SetFieldData(string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !SingleData.ContainsKey(field))
            {
                SingleData.Add(field, value);
            }
        }

        #region Update Draw Form
        public void UpdateDrawForm()
        {
            _hasNextPage = true;

            #region SubMethod

            #region Header

            //タイトル
            SetFieldData("Title", _printConf.ReportName);
            //医療機関名
            _extralData.Add("HeaderR_0_0_" + _currentPage, hpInf.HpName);
            //作成日時
            _extralData.Add("HeaderR_0_1_" + _currentPage, CIUtil.SDateToShowSWDate(
                CIUtil.ShowSDateToSDate(CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd")), 0, 1
            ) + CIUtil.GetJapanDateTimeNow().ToString(" HH:mm") + "作成");
            //ページ数
            int totalPage = (int)Math.Ceiling((double)printDatas.Count / maxRow);
            _extralData.Add("HeaderR_0_2_" + _currentPage, _currentPage + " / " + totalPage);
            //入金日
            _extralData.Add("HeaderL_0_1_" + _currentPage, headerL1.Count >= _currentPage ? headerL1[_currentPage - 1] : "");
            //改ページ条件
            _extralData.Add("HeaderL_0_2_" + _currentPage, headerL2.Count >= _currentPage ? headerL2[_currentPage - 1] : "");

            //期間
            SetFieldData("Range",
                string.Format(
                    "期間: {0} ～ {1}",
                    CIUtil.SDateToShowSWDate(_printConf.StartNyukinDate, 0, 1),
                    CIUtil.SDateToShowSWDate(_printConf.EndNyukinDate, 0, 1)
                )
            );


            #endregion

            #region Body

            int ptIndex = (_currentPage - 1) * maxRow;
            int lineCount = 0;

            //存在しているフィールドに絞り込み
            var existsCols = putCurColumns.Where(p => _objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                var celldata = new Dictionary<string, CellModel>();

                var printData = printDatas[ptIndex];
                string baseListName = "";

                //保険外金額（内訳）タイトル
                foreach (var jihiSbtMst in jihiSbtMsts)
                {
                    SetFieldData(string.Format("tJihiFutanSbt{0}", jihiSbtMst.JihiSbt), jihiSbtMst.Name);
                }

                //明細データ出力
                foreach (var colName in existsCols)
                {
                    var value = typeof(CoSta1001PrintData).GetProperty(colName)?.GetValue(printData);
                    string valueInput = value?.ToString() ?? string.Empty;
                    celldata.Add(colName, new CellModel(valueInput));
                    if (baseListName == "" && _objectRseList.Contains(colName))
                    {
                        baseListName = colName;
                    }
                }
                //自費種別毎の金額
                for (int i = 0; i <= jihiSbtMsts.Count - 1; i++)
                {
                    if (printData.JihiSbtFutans == null) break;
                    var jihiSbtMst = jihiSbtMsts[i];
                    celldata.Add(string.Format("JihiFutanSbt{0}", jihiSbtMst.JihiSbt), new CellModel(printData.JihiSbtFutans[i]));
                }

                //合計行キャプションと件数
                celldata.Add("TotalCaption", new CellModel(printData.TotalCaption));
                celldata.Add("TotalCount", new CellModel(printData.TotalCount));
                celldata.Add("TotalPtCount", new CellModel(printData.TotalPtCount));

                //5行毎に区切り線を引く
                lineCount = printData.RowType != RowType.Brank ? lineCount + 1 : lineCount;
                string rowNoKey = rowNo + "_" + _currentPage;
                _extralData.Add("lineCount" + rowNoKey, lineCount.ToString());

                if (lineCount == 5)
                {
                    lineCount = 0;
                    if (!_extralData.ContainsKey("headerLine"))
                    {
                        _extralData.Add("headerLine", "true");
                    }
                    _extralData.Add("baseListName" + rowNoKey, baseListName);
                    _extralData.Add("rowNo" + rowNoKey, rowNo.ToString());
                }

                CellData.Add(celldata);

                ptIndex++;
                if (ptIndex >= printDatas.Count)
                {
                    _hasNextPage = false;
                    break;
                }
            }

            #endregion
            #endregion
        }

        public CommonExcelReportingModel ExportCsv(CoSta1001PrintConf printConf, int dateFrom, int dateTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow)
        {
            HpId = hpId;
            _printConf = printConf;

            if (isPutTotalRow)
            {
                putCurColumns.AddRange(csvTotalColumns);
            }

            putCurColumns.AddRange(putColumns);
            string fileName = menuName + "_" + dateFrom + "_" + dateTo;
            List<string> retDatas = new List<string>();

            if (!GetData()) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

            var csvDatas = printDatas.Where(p => p.RowType == RowType.Data || (isPutTotalRow && p.RowType == RowType.Total)).ToList();

            if (csvDatas.Count == 0) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

            //出力フィールド
            List<string> wrkTitles = putCurColumns.Select(p => p.JpName).ToList();
            List<string> wrkColumns = putCurColumns.Select(p => p.CsvColName).ToList();

            //タイトル行
            retDatas.Add("\"" + string.Join("\",\"", wrkTitles.Union(jihiSbtMsts.Select(j => string.Format("保険外金額({0})", j.Name)))) + "\"");

            if (isPutColName)
            {
                retDatas.Add("\"" + string.Join("\",\"", wrkColumns.Union(jihiSbtMsts.Select(j => string.Format("JihiFutanSbt{0}", j.JihiSbt)))) + "\"");
            }

            string RecordData(CoSta1001PrintData csvData)
            {
                List<string> colDatas = new List<string>();

                foreach (var column in putCurColumns)
                {
                    var value = typeof(CoSta1001PrintData).GetProperty(column.CsvColName)?.GetValue(csvData);
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
                    foreach (var jihiSbtMst in jihiSbtMsts)
                    {
                        colDatas.Add("\"0\"");
                    }
                }

                return string.Join(",", colDatas);
            }
            //データ
            int rowOutputed = 0;
            foreach (var csvData in csvDatas)
            {
                retDatas.Add(RecordData(csvData));
                rowOutputed++;
            }

            return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);
        }
        #endregion
    }
}