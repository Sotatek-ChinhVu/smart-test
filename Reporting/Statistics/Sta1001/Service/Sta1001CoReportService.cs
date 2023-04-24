using Helper.Common;
using Helper.Extension;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta1001.DB;
using Reporting.Statistics.Sta1001.Models;
using System.ComponentModel;
using System.Globalization;

namespace Reporting.Statistics.Sta1001.Service
{
    public class Sta1001CoReportService : ISta1001CoReportService
    {
        #region Private properties

        private ICoSta1001Finder _sta1001Finder;
        private readonly IReadRseReportFileService _readRseReportFileService;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private int HpId;
        private List<CoSta1001PrintData> printDatas;
        private List<string> headerL1;
        private List<string> headerL2;
        private List<CoSyunoInfModel> syunoInfs;
        private List<CoJihiSbtMstModel> jihiSbtMsts;
        private List<CoJihiSbtFutan> jihiSbtFutans;
        private CoHpInfModel hpInf;

        private List<PutColumn> putCurColumns = new List<PutColumn>();

        private countData grandTotal = new countData();
        private countData total = new countData();
        private countData subTotal = new countData();

        private struct countData
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
                PtCount = null;
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
                JihiSbtFutans = null;
            }
        }

        #endregion

        private CoSta1001PrintConf printConf;

        private int maxRow = 43;


        private void GetFormParam(string formfile)
        {
            List<ObjectCalculate> fieldInputList = new();

            fieldInputList.Add(new ObjectCalculate("lsData", (int)CalculateTypeEnum.GetFormatLength));
            fieldInputList.Add(new ObjectCalculate("lsSuryo", (int)CalculateTypeEnum.GetFormatLength));
            fieldInputList.Add(new ObjectCalculate("lsUnitName", (int)CalculateTypeEnum.GetFormatLength));
            fieldInputList.Add(new ObjectCalculate("lsKaisu", (int)CalculateTypeEnum.GetFormatLength));
            fieldInputList.Add(new ObjectCalculate("lsYohoUnitName", (int)CalculateTypeEnum.GetFormatLength));
            fieldInputList.Add(new ObjectCalculate("lsData", (int)CalculateTypeEnum.ListRowCount));
            fieldInputList.Add(new ObjectCalculate("lsBikoShort", (int)CalculateTypeEnum.GetFormatLength));
            fieldInputList.Add(new ObjectCalculate("lsBikoShort", (int)CalculateTypeEnum.ListRowCount));
            fieldInputList.Add(new ObjectCalculate("lsBikoLong", (int)CalculateTypeEnum.GetFormatLength));
            fieldInputList.Add(new ObjectCalculate("lsBikoLong", (int)CalculateTypeEnum.ListRowCount));

            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.OutDrug, formfile, fieldInputList);

            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            UpdateParamLocal(javaOutputData.responses ?? new());
        }

        private void UpdateParamLocal(List<ObjectCalculateResponse> result)
        {
            foreach (var item in result)
            {
                switch (item.typeInt)
                {
                    case (int)CalculateTypeEnum.ListRowCount:
                        switch (item.listName)
                        {
                            case "lsData":
                                _dataRowCount = item.result;
                                break;
                            case "lsBikoShort":
                                _bikoShortRowCount = item.result;
                                break;
                            case "lsBikoLong":
                                _bikoLongRowCount = item.result;
                                break;
                        }
                        break;
                    case (int)CalculateTypeEnum.GetFormatLength:
                        switch (item.listName)
                        {
                            case "lsData":
                                _dataCharCount = item.result;
                                break;
                            case "lsSuryo":
                                _suryoCharCount = item.result;
                                break;
                            case "lsUnitName":
                                _unitCharCount = item.result;
                                break;
                            case "lsKaisu":
                                _kaisuCharCount = item.result;
                                break;
                            case "lsYohoUnitName":
                                _yohoUnitCharCount = item.result;
                                break;
                            case "lsBikoShort":
                                _bikoShortCharCount = item.result;
                                break;
                            case "lsBikoLong":
                                _bikoLongCharCount = item.result;
                                break;
                        }
                        break;
                }
            }
        }

        private List<CoSta1001PrintData> GetData()
        {
            void MakePrintData()
            {
                printDatas = new List<CoSta1001PrintData>();
                headerL1 = new List<string>();
                headerL2 = new List<string>();
                int pageCount = 1;
                int recNo = 0;

                //改ページ条件
                bool pbUketukeSbt = new int[] { printConf.PageBreak1, printConf.PageBreak2, printConf.PageBreak3 }.Contains(1);
                bool pbKaId = new int[] { printConf.PageBreak1, printConf.PageBreak2, printConf.PageBreak3 }.Contains(2);
                bool pbTantoId = new int[] { printConf.PageBreak1, printConf.PageBreak2, printConf.PageBreak3 }.Contains(3);

                #region ソート順
                syunoInfs =
                    syunoInfs
                        .OrderBy(s => s.NyukinDate)
                        .ThenBy(s => pbUketukeSbt ? s.UketukeSbt : 0)
                        .ThenBy(s => pbKaId ? s.KaId : 0)
                        .ThenBy(s => pbTantoId ? s.TantoId : 0)
                        .ThenBy(s => s.NyukinDate == s.SinDate ? "0" : "1")
                        .ThenBy(s =>
                            printConf.SortOpt1 == 1 ? "0" :
                            printConf.SortOrder1 == 1 ? s.PtKanaName :
                            printConf.SortOrder1 == 2 ? s.PtNum.ToString().PadLeft(10, '0') :
                            printConf.SortOrder1 == 3 ? s.UketukeTime :
                            printConf.SortOrder1 == 4 ? s.KaikeiTime : "0")
                        .ThenByDescending(s =>
                            printConf.SortOpt1 == 0 ? "0" :
                            printConf.SortOrder1 == 1 ? s.PtKanaName :
                            printConf.SortOrder1 == 2 ? s.PtNum.ToString().PadLeft(10, '0') :
                            printConf.SortOrder1 == 3 ? s.UketukeTime :
                            printConf.SortOrder1 == 4 ? s.KaikeiTime : "0")
                        .ThenBy(s =>
                            printConf.SortOpt2 == 1 ? "0" :
                            printConf.SortOrder2 == 1 ? s.PtKanaName :
                            printConf.SortOrder2 == 2 ? s.PtNum.ToString().PadLeft(10, '0') :
                            printConf.SortOrder2 == 3 ? s.UketukeTime :
                            printConf.SortOrder2 == 4 ? s.KaikeiTime : "0")
                        .ThenByDescending(s =>
                            printConf.SortOpt2 == 0 ? "0" :
                            printConf.SortOrder2 == 1 ? s.PtKanaName :
                            printConf.SortOrder2 == 2 ? s.PtNum.ToString().PadLeft(10, '0') :
                            printConf.SortOrder2 == 3 ? s.UketukeTime :
                            printConf.SortOrder2 == 4 ? s.KaikeiTime : "0")
                        .ThenBy(s =>
                            printConf.SortOpt3 == 1 ? "0" :
                            printConf.SortOrder3 == 1 ? s.PtKanaName :
                            printConf.SortOrder3 == 2 ? s.PtNum.ToString().PadLeft(10, '0') :
                            printConf.SortOrder3 == 3 ? s.UketukeTime :
                            printConf.SortOrder3 == 4 ? s.KaikeiTime : "0")
                        .ThenByDescending(s =>
                            printConf.SortOpt3 == 0 ? "0" :
                            printConf.SortOrder3 == 1 ? s.PtKanaName :
                            printConf.SortOrder3 == 2 ? s.PtNum.ToString().PadLeft(10, '0') :
                            printConf.SortOrder3 == 3 ? s.UketukeTime :
                            printConf.SortOrder3 == 4 ? s.KaikeiTime : "0")
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
                        while ((int)Math.Ceiling((double)(printDatas.Count) / maxRow) > headerL1.Count && headerL1.Count >= 1 && headerL2.Count >= 1)
                        {
                            headerL1.Add(headerL1.Last());
                            headerL2.Add(headerL2.Last());
                        }
                    }

                    if (syunoInf.RaiinNo != prePrintData.RaiinNo)
                    {
                        //連番
                        recNo++;
                        printData.Seq = recNo.ToString();
                    }

                    if (syunoInf.RaiinNo != prePrintData.RaiinNo || pageBreak)
                    {
                        printData.PtNum = syunoInf.PtNum.ToString();
                        printData.PtName = syunoInf.PtName;
                        printData.PtKanaName = syunoInf.PtKanaName;
                        printData.Syosaisin = syunoInf.Syosaisin;
                        printData.HokenSbt = syunoInf.HokenSbt;

                        bool csvOmit = false;
                        printData.Tensu = csvOmit ? "0" : syunoInf.Tensu.ToString("#,0");
                        printData.NewTensu = csvOmit ? "0" : syunoInf.NewTensu.ToString("#,0");
                        printData.PtFutan = csvOmit ? "0" : syunoInf.PtFutan.ToString("#,0");
                        printData.PtFutanJihiRece = csvOmit ? "0" : (syunoInf.IsJihiRece ? syunoInf.PtFutan : 0).ToString("#,0");
                        printData.PtFutanElse = csvOmit ? "0" : (!syunoInf.IsJihiRece ? syunoInf.PtFutan : 0).ToString("#,0");
                        printData.JihiFutan = csvOmit ? "0" : syunoInf.JihiFutan.ToString("#,0");
                        printData.JihiTax = csvOmit ? "0" : syunoInf.JihiTax.ToString("#,0");
                        printData.AdjustFutan = csvOmit ? "0" : syunoInf.AdjustFutan.ToString("#,0");
                        printData.MenjyoGaku = csvOmit ? "0" : syunoInf.MenjyoGaku.ToString("#,0");
                        printData.SeikyuGaku = csvOmit ? "0" :
                            syunoInfs.Where(s =>
                                s.RaiinNo == syunoInf.RaiinNo &&
                                s.NyukinSortNo == syunoInfs.Where(x => x.RaiinNo == syunoInf.RaiinNo).Max(x => x.NyukinSortNo)
                            ).First().SeikyuGaku.ToString("#,0");
                        printData.NewSeikyuGaku = csvOmit ? "0" :
                            syunoInfs.Where(s =>
                                s.RaiinNo == syunoInf.RaiinNo &&
                                s.NyukinSortNo == syunoInfs.Where(x => x.RaiinNo == syunoInf.RaiinNo).Max(x => x.NyukinSortNo)
                            ).First().NewSeikyuGaku.ToString("#,0");
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
                        //decMisyuGaku =
                        //    int.Parse(prePrintData.MisyuGaku ?? "0", NumberStyles.Any) -
                        //    int.Parse(printData.MisyuGaku ?? "0", NumberStyles.Any);

                        //prePrintData.MisyuGaku = printData.MisyuGaku;
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
                        if (printConf.StartNyukinTime >= 0 || printConf.EndNyukinTime >= 0)
                        {
                            wrkNyukinDate +=
                                string.Format(
                                    " {0}～{1}",
                                    printConf.StartNyukinTime == -1 ? "" : CIUtil.TryCIToTimeZone(printConf.StartNyukinTime),
                                    printConf.EndNyukinTime == -1 ? "" : CIUtil.TryCIToTimeZone(printConf.EndNyukinTime)
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
                while ((int)Math.Ceiling((double)(printDatas.Count) / maxRow) > headerL1.Count && headerL1.Count >= 1 && headerL2.Count >= 1)
                {
                    headerL1.Add(headerL1.Last());
                    headerL2.Add(headerL2.Last());
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
            void AddTotalRecord(string title, ref countData totalData)
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
                        JihiSbtFutans = totalData.JihiSbtFutans?.Select(j => j.ToString("#,0")).ToList() ?? new()
                    }
                );
                totalData.Clear();
            }
            hpInf = _sta1001Finder.GetHpInf(HpId, printConf.StartNyukinDate);

            syunoInfs = _sta1001Finder.GetSyunoInfs(HpId, printConf, 0);
            if ((syunoInfs?.Count ?? 0) == 0) return new List<CoSta1001PrintData>();

            jihiSbtMsts = _sta1001Finder.GetJihiSbtMst(HpId);
            jihiSbtFutans = _sta1001Finder.GetJihiSbtFutan(HpId, printConf);

            //来院コメントの取得
            if (printDatas.Any(x => x.RaiinCmt != string.Empty))
            {
                foreach (var syunoInf in syunoInfs)
                {
                    syunoInf.RaiinCmt = _sta1001Finder.GetRaiinCmtInf(HpId, syunoInf.RaiinNo);
                }
            }

            //印刷用データの作成
            MakePrintData();

            return printDatas;
        }
    }
}
