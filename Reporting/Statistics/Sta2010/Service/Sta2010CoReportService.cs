using Helper.Common;
using Helper.Constants;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta2010.DB;
using Reporting.Statistics.Sta2010.Mapper;
using Reporting.Statistics.Sta2010.Models;

namespace Reporting.Statistics.Sta2010.Service
{
    public class Sta2010CoReportService : ISta2010CoReportService
    {
        private readonly IReadRseReportFileService _readRseReportFileService;
        private readonly ICoSta2010Finder _staFinder;

        public Sta2010CoReportService(IReadRseReportFileService readRseReportFileService, ICoSta2010Finder staFinder)
        {
            _readRseReportFileService = readRseReportFileService;
            _staFinder = staFinder;
        }

        public CommonReportingRequestModel GetSta2010ReportingData(CoSta2010PrintConf printConf, int hpId)
        {
            HpId = hpId;
            _printConf = printConf;
            // get data to print
            GetFieldNameList();
            GetRowCount();

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

            return new Sta2010Mapper(_singleFieldData, _tableFieldData, _extralData, _rowCountFieldName).GetData();
        }

        #region Constant
        private int maxRow = 45;

        private List<PutColumn> csvTotalColumns = new List<PutColumn>
        {
            new PutColumn("RowType", "明細区分")
        };

        private List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("SeikyuYmFmt", "請求年月", false, "SeikyuYm"),
            new PutColumn("IsZaiiso", "在医総"),
            new PutColumn("KaId", "診療科ID"),
            new PutColumn("KaSname", "診療科"),
            new PutColumn("TantoId", "担当医ID"),
            new PutColumn("TantoSname", "担当医"),
            new PutColumn("HokenSbt1", "保険種別（階層１）"),
            new PutColumn("HokenSbt2", "保険種別（階層２）"),
            new PutColumn("HokenSbt3", "保険種別（階層３）"),
            new PutColumn("HokenSbt4", "保険種別（階層４）"),
            new PutColumn("Count", "件数"),
            new PutColumn("Nissu", "実日数"),
            new PutColumn("Tensu", "点数"),
            new PutColumn("Futan", "一部負担金"),
            new PutColumn("KohiCount", "件数(公費併用分)"),
            new PutColumn("KohiNissu", "実日数(公費併用分)"),
            new PutColumn("KohiTensu", "点数(公費併用分)"),
            new PutColumn("KohiFutan", "一部負担金(公費併用分)"),
            new PutColumn("PtFutan", "窓口負担"),
            new PutColumn("Furikomi", "振込予定額")
        };
        #endregion

        #region Private properties


        private List<CoSta2010PrintData> printDatas;
        private List<string> headerL1;
        private List<string> headerL2;
        private List<CoReceInfModel> receInfs;
        private List<CoKohiHoubetuMstModel> kohiHoubetuMsts;
        private List<CoHokensyaMstModel> hokensyaNames;
        private CoHpInfModel hpInf;
        private CoSta2010PrintConf _printConf = new(0);
        private int _currentPage;
        private string _rowCountFieldName = string.Empty;
        private List<string> _objectRseList = new();
        private bool _hasNextPage;
        private int _maxRow;
        private int HpId;
        private CoFileType? coFileType;
        private bool isPutTotalRow;

        private readonly Dictionary<string, string> _singleFieldData = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _extralData = new Dictionary<string, string>();
        private readonly List<Dictionary<string, CellModel>> _tableFieldData = new List<Dictionary<string, CellModel>>();

        private List<PutColumn> putCurColumns = new List<PutColumn>();
        #endregion

        #region Load Data
        private List<string> GetKohiHoubetu(List<CoReceInfModel> coReceInfs, List<string> excludeHoubetu)
        {
            excludeHoubetu = excludeHoubetu ?? new List<string>();

            //公１法別
            var retNums = coReceInfs.Where(r =>
                !excludeHoubetu.Contains(r.Kohi1Houbetu) && r.Kohi1ReceKisai
            ).GroupBy(r => r.Kohi1Houbetu).Select(r => r.Key).ToList();
            //公２法別
            var wrkNums = coReceInfs.Where(r =>
                !excludeHoubetu.Contains(r.Kohi2Houbetu) && r.Kohi2ReceKisai
            ).GroupBy(r => r.Kohi2Houbetu).Select(r => r.Key).ToList();
            //公１法別 + 公２法別
            retNums = retNums.Union(wrkNums).ToList();
            //公３法別
            wrkNums = coReceInfs.Where(r =>
                !excludeHoubetu.Contains(r.Kohi3Houbetu) && r.Kohi3ReceKisai
            ).GroupBy(r => r.Kohi3Houbetu).Select(r => r.Key).ToList();
            //公１法別 + 公２法別 + 公３法別
            retNums = retNums.Union(wrkNums).ToList();
            //公４法別
            wrkNums = coReceInfs.Where(r =>
                !excludeHoubetu.Contains(r.Kohi4Houbetu) && r.Kohi4ReceKisai
            ).GroupBy(r => r.Kohi4Houbetu).Select(r => r.Key).ToList();
            //公１法別 + 公２法別 + 公３法別 + 公４法別
            retNums = retNums.Union(wrkNums).ToList();

            //法別番号順にソート
            retNums.Sort();

            return retNums;
        }

        /// <summary>
        /// 公費名称の取得
        /// </summary>
        private string GetKohiName(string houbetu)
        {
            return
                kohiHoubetuMsts.Find(k => k.PrefNo == 0 && k.Houbetu == houbetu)?.HokenNameCd ??
                kohiHoubetuMsts.Find(k => k.PrefNo == hpInf.PrefNo && k.Houbetu == houbetu)?.HokenNameCd;
        }

        //公費の組合せを取得する
        private List<string> GetHoubetuPair(List<CoReceInfModel> coReceInfs)
        {
            var retNums = coReceInfs.GroupBy(r =>
                (r.Kohi1ReceKisai ? r.Kohi1Houbetu : "") +
                (r.Kohi2ReceKisai ? r.Kohi2Houbetu : "") +
                (r.Kohi3ReceKisai ? r.Kohi3Houbetu : "") +
                (r.Kohi4ReceKisai ? r.Kohi4Houbetu : "")
            ).Select(r => r.Key).ToList();

            //法別番号順にソート
            retNums.Sort();

            return retNums;
        }

        private bool GetData()
        {
            void MakePrintData()
            {
                printDatas = new List<CoSta2010PrintData>();
                headerL1 = new List<string>();
                headerL2 = new List<string>();
                int totalRow = 0;

                //改ページ条件
                bool pbZaiiso = new int[] { _printConf.PageBreak1, _printConf.PageBreak2, _printConf.PageBreak3 }.Contains(1);
                bool pbKaId = new int[] { _printConf.PageBreak1, _printConf.PageBreak2, _printConf.PageBreak3 }.Contains(2);
                bool pbTantoId = new int[] { _printConf.PageBreak1, _printConf.PageBreak2, _printConf.PageBreak3 }.Contains(3);

                var isZaiisoes = receInfs.GroupBy(s => s.IsZaiiso).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                for (int zaiCnt = 0; (pbZaiiso && zaiCnt < isZaiisoes.Count) || zaiCnt == 0; zaiCnt++)
                {
                    var kaIds = receInfs.GroupBy(s => s.KaId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                    for (int kaCnt = 0; (pbKaId && kaCnt <= kaIds.Count - 1) || kaCnt == 0; kaCnt++)
                    {
                        var tantoIds = receInfs.GroupBy(s => s.TantoId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                        for (int taCnt = 0; (pbTantoId && taCnt <= tantoIds.Count - 1) || taCnt == 0; taCnt++)
                        {
                            var curDatas = receInfs.Where(s =>
                                (pbZaiiso ? s.IsZaiiso == isZaiisoes[zaiCnt] : true) &&
                                (pbKaId ? s.KaId == kaIds[kaCnt] : true) &&
                                (pbTantoId ? s.TantoId == tantoIds[taCnt] : true)
                            ).ToList();

                            int curRecNo = Math.Max(printDatas.Count - 1, 0);

                            //社保
                            var syahoDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Syaho).ToList();
                            AddSyahoData(syahoDatas);

                            //国保
                            var kokhoDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && (s.IsNrAll || s.IsRetAll)).ToList();
                            AddKokhoData(kokhoDatas);

                            //後期
                            var koukiDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && s.IsKoukiAll).ToList();
                            AddKoukiData(koukiDatas);

                            //その他
                            var elseDatas = curDatas.Where(s => new int[] { HokenKbn.RousaiShort, HokenKbn.RousaiLong,
                                HokenKbn.AfterCare, HokenKbn.Jibai, HokenKbn.Jihi }.Contains(s.HokenKbn)).ToList();
                            AddElseData(elseDatas);

                            for (int i = curRecNo; i < printDatas.Count; i++)
                            {
                                printDatas[i].SeikyuYm = curDatas.FirstOrDefault().SeikyuYm;
                                printDatas[i].IsZaiiso = pbZaiiso ? curDatas.FirstOrDefault().IsZaiiso.ToString() : null;
                                printDatas[i].KaId = (pbKaId || kaIds.Count == 1) ? curDatas.FirstOrDefault().KaId.ToString() : null;
                                printDatas[i].KaSname = (pbKaId || kaIds.Count == 1) ? curDatas.FirstOrDefault().KaSname : null;
                                printDatas[i].TantoId = (pbTantoId || tantoIds.Count == 1) ? curDatas.FirstOrDefault().TantoId.ToString() : null;
                                printDatas[i].TantoSname = (pbTantoId || tantoIds.Count == 1) ? curDatas.FirstOrDefault().TantoSname : null;
                            }

                            //ヘッダー情報
                            int rowCount = printDatas.Count - totalRow;
                            int pageCount = (int)Math.Ceiling((double)(rowCount) / maxRow);
                            for (int i = 0; i < pageCount; i++)
                            {
                                //請求年月
                                string wrkYm = CIUtil.Copy(CIUtil.SDateToShowSWDate(_printConf.SeikyuYm * 100 + 1, 0, 1, 1), 1, 13);
                                headerL1.Add(wrkYm + "度");
                                //改ページ条件
                                List<string> wrkHeaders = new List<string>();
                                if (pbZaiiso) wrkHeaders.Add(curDatas.First().IsZaiiso == 1 ? "在医総" : "在医総以外");
                                if (pbKaId) wrkHeaders.Add(curDatas.First().KaSname);
                                if (pbTantoId) wrkHeaders.Add(curDatas.First().TantoSname);

                                if (wrkHeaders.Count >= 1) headerL2.Add(string.Join("／", wrkHeaders));
                            }
                            totalRow += rowCount;
                        }
                    }
                }

                if (coFileType == CoFileType.Csv)
                {
                    //CSV出力の場合は空白を埋める
                    printDatas = printDatas.Where(p => p.RowType == RowType.Data || (isPutTotalRow && p.RowType == RowType.Total)).ToList();
                    for (int i = 1; i < printDatas.Count; i++)
                    {
                        if (printDatas[i].HokenSbt1 == null)
                        {
                            printDatas[i].HokenSbt1 = printDatas[i - 1].HokenSbt1;
                        }
                        if (printDatas[i].HokenSbt2 == null && printDatas[i].HokenSbt1 == printDatas[i - 1].HokenSbt1)
                        {
                            printDatas[i].HokenSbt2 = printDatas[i - 1].HokenSbt2;
                        }
                        if (printDatas[i].HokenSbt3 == null && printDatas[i].HokenSbt2 == printDatas[i - 1].HokenSbt2)
                        {
                            printDatas[i].HokenSbt3 = printDatas[i - 1].HokenSbt3;
                        }
                    }
                }
            }

            #region 社保データ集計
            void AddSyahoData(List<CoReceInfModel> receDatas)
            {
                if (receDatas.Count == 0) return;

                int totalCount1 = 0;

                #region 医療保険
                for (int rowNo = 0; rowNo <= 56; rowNo++)
                {
                    CoSta2010PrintData printData = new CoSta2010PrintData();
                    List<CoReceInfModel> wrkReces = null;

                    switch (rowNo)
                    {
                        case 0:
                            printData.HokenSbt1 = "社保";
                            printDatas.Add(printData);
                            break;
                        //70歳以上一般・低所得 --------------------
                        case 1:
                            printData.HokenSbt2 = "医保(70歳以上一般・低所得)と公費の併用";
                            wrkReces = receDatas.Where(r => r.IsNrElderIppan && r.IsHeiyo).ToList();
                            break;
                        case 2:
                            printData.HokenSbt2 = "医保(70歳以上一般・低所得)単独";
                            if (receDatas.Where(r => r.IsNrElderIppan && !r.IsHeiyo).Count() >= 1)
                            {
                                printDatas.Add(printData);
                            }
                            continue;
                        case 3:
                            printData.HokenSbt3 = "01       (協)";
                            wrkReces = receDatas.Where(r => r.IsNrElderIppan && !r.IsHeiyo && r.Houbetu == "01").ToList();
                            break;
                        case 4:
                            printData.HokenSbt3 = "02       (船)(職務上)";
                            wrkReces = receDatas.Where(r => r.IsNrElderIppan && !r.IsHeiyo && r.Houbetu == "02" && new int[] { 1, 2 }.Contains(r.SyokumuKbn)).ToList();
                            break;
                        case 5:
                            printData.HokenSbt3 = "02       (船)(職務外)";
                            wrkReces = receDatas.Where(r => r.IsNrElderIppan && !r.IsHeiyo && r.Houbetu == "02" && r.SyokumuKbn == 3).ToList();
                            break;
                        case 6:
                            printData.HokenSbt3 = "03       (日)";
                            wrkReces = receDatas.Where(r => r.IsNrElderIppan && !r.IsHeiyo && r.Houbetu == "03").ToList();
                            break;
                        case 7:
                            printData.HokenSbt3 = "04       (日特)";
                            wrkReces = receDatas.Where(r => r.IsNrElderIppan && !r.IsHeiyo && r.Houbetu == "04").ToList();
                            break;
                        case 8:
                            printData.HokenSbt3 = "31-34    (共)(下船3月)";
                            wrkReces = receDatas.Where(r => r.IsNrElderIppan && !r.IsHeiyo && new string[] { "31", "32", "33", "34" }.Contains(r.Houbetu) && r.SyokumuKbn == 2).ToList();
                            break;
                        case 9:
                            printData.HokenSbt3 = "31-34    (共)(一般)";
                            wrkReces = receDatas.Where(r => r.IsNrElderIppan && !r.IsHeiyo && new string[] { "31", "32", "33", "34" }.Contains(r.Houbetu) && r.SyokumuKbn != 2).ToList();
                            break;
                        case 10:
                            printData.HokenSbt3 = "06       (組)";
                            wrkReces = receDatas.Where(r => r.IsNrElderIppan && !r.IsHeiyo && r.Houbetu == "06").ToList();
                            break;
                        case 11:
                            printData.HokenSbt3 = "63,72-75 (退)";
                            wrkReces = receDatas.Where(r => r.IsNrElderIppan && !r.IsHeiyo && new string[] { "63", "72", "73", "74", "75" }.Contains(r.Houbetu)).ToList();
                            break;
                        case 12:
                            printData.RowType = RowType.Total;
                            printData.HokenSbt3 = "◆小計";
                            wrkReces = receDatas.Where(r => r.IsNrElderIppan && !r.IsHeiyo).ToList();
                            break;
                        //70歳以上7割 --------------------
                        case 13:
                            printData.HokenSbt2 = "医保(70歳以上7割)と公費の併用";
                            wrkReces = receDatas.Where(r => r.IsNrElderUpper && r.IsHeiyo).ToList();
                            break;
                        case 14:
                            printData.HokenSbt2 = "医保(70歳以上7割)単独";
                            if (receDatas.Where(r => r.IsNrElderUpper && !r.IsHeiyo).Count() >= 1)
                            {
                                printDatas.Add(printData);
                            }
                            continue;
                        case 15:
                            printData.HokenSbt3 = "01       (協)";
                            wrkReces = receDatas.Where(r => r.IsNrElderUpper && !r.IsHeiyo && r.Houbetu == "01").ToList();
                            break;
                        case 16:
                            printData.HokenSbt3 = "02       (船)(職務上)";
                            wrkReces = receDatas.Where(r => r.IsNrElderUpper && !r.IsHeiyo && r.Houbetu == "02" && new int[] { 1, 2 }.Contains(r.SyokumuKbn)).ToList();
                            break;
                        case 17:
                            printData.HokenSbt3 = "02       (船)(職務外)";
                            wrkReces = receDatas.Where(r => r.IsNrElderUpper && !r.IsHeiyo && r.Houbetu == "02" && r.SyokumuKbn == 3).ToList();
                            break;
                        case 18:
                            printData.HokenSbt3 = "31-34    (共)(下船3月)";
                            wrkReces = receDatas.Where(r => r.IsNrElderUpper && !r.IsHeiyo && new string[] { "31", "32", "33", "34" }.Contains(r.Houbetu) && r.SyokumuKbn == 2).ToList();
                            break;
                        case 19:
                            printData.HokenSbt3 = "31-34    (共)(一般)";
                            wrkReces = receDatas.Where(r => r.IsNrElderUpper && !r.IsHeiyo && new string[] { "31", "32", "33", "34" }.Contains(r.Houbetu) && r.SyokumuKbn != 2).ToList();
                            break;
                        case 20:
                            printData.HokenSbt3 = "06       (組)";
                            wrkReces = receDatas.Where(r => r.IsNrElderUpper && !r.IsHeiyo && r.Houbetu == "06").ToList();
                            break;
                        case 21:
                            printData.HokenSbt3 = "63,72-75 (退)";
                            wrkReces = receDatas.Where(r => r.IsNrElderUpper && !r.IsHeiyo && new string[] { "63", "72", "73", "74", "75" }.Contains(r.Houbetu)).ToList();
                            break;
                        case 22:
                            printData.RowType = RowType.Total;
                            printData.HokenSbt3 = "◆小計";
                            printData.DrawLine = true;
                            wrkReces = receDatas.Where(r => r.IsNrElderUpper && !r.IsHeiyo).ToList();
                            break;
                        //本人 --------------------
                        case 23:
                            printData.HokenSbt2 = "医保(本人)と公費の併用";
                            wrkReces = receDatas.Where(r => r.IsNrMine && r.IsHeiyo).ToList();
                            break;
                        case 24:
                            printData.HokenSbt2 = "医保(本人)単独";
                            if (receDatas.Where(r => r.IsNrMine && !r.IsHeiyo).Count() >= 1)
                            {
                                printDatas.Add(printData);
                            }
                            continue;
                        case 25:
                            printData.HokenSbt3 = "01       (協)";
                            wrkReces = receDatas.Where(r => r.IsNrMine && !r.IsHeiyo && r.Houbetu == "01").ToList();
                            break;
                        case 26:
                            printData.HokenSbt3 = "02       (船)(職務上)";
                            wrkReces = receDatas.Where(r => r.IsNrMine && !r.IsHeiyo && r.Houbetu == "02" && new int[] { 1, 2 }.Contains(r.SyokumuKbn)).ToList();
                            break;
                        case 27:
                            printData.HokenSbt3 = "02       (船)(職務外)";
                            wrkReces = receDatas.Where(r => r.IsNrMine && !r.IsHeiyo && r.Houbetu == "02" && r.SyokumuKbn == 3).ToList();
                            break;
                        case 28:
                            printData.HokenSbt3 = "03       (日)";
                            wrkReces = receDatas.Where(r => r.IsNrMine && !r.IsHeiyo && r.Houbetu == "03").ToList();
                            break;
                        case 29:
                            printData.HokenSbt3 = "04       (日特)";
                            wrkReces = receDatas.Where(r => r.IsNrMine && !r.IsHeiyo && r.Houbetu == "04").ToList();
                            break;
                        case 30:
                            printData.HokenSbt3 = "31-34    (共)(下船3月)";
                            wrkReces = receDatas.Where(r => r.IsNrMine && !r.IsHeiyo && new string[] { "31", "32", "33", "34" }.Contains(r.Houbetu) && r.SyokumuKbn == 2).ToList();
                            break;
                        case 31:
                            printData.HokenSbt3 = "31-34    (共)(一般)";
                            wrkReces = receDatas.Where(r => r.IsNrMine && !r.IsHeiyo && new string[] { "31", "32", "33", "34" }.Contains(r.Houbetu) && r.SyokumuKbn != 2).ToList();
                            break;
                        case 32:
                            printData.HokenSbt3 = "06       (組)";
                            wrkReces = receDatas.Where(r => r.IsNrMine && !r.IsHeiyo && r.Houbetu == "06").ToList();
                            break;
                        case 33:
                            printData.HokenSbt3 = "07       (自)";
                            wrkReces = receDatas.Where(r => r.IsNrMine && !r.IsHeiyo && r.Houbetu == "07").ToList();
                            break;
                        case 34:
                            printData.HokenSbt3 = "63,72-75 (退)";
                            wrkReces = receDatas.Where(r => r.IsNrMine && !r.IsHeiyo && new string[] { "63", "72", "73", "74", "75" }.Contains(r.Houbetu)).ToList();
                            break;
                        case 35:
                            printData.RowType = RowType.Total;
                            printData.HokenSbt3 = "◆小計";
                            printData.DrawLine = true;
                            wrkReces = receDatas.Where(r => r.IsNrMine && !r.IsHeiyo).ToList();
                            break;
                        //家族 --------------------
                        case 36:
                            printData.HokenSbt2 = "医保(家族)と公費の併用";
                            wrkReces = receDatas.Where(r => r.IsNrFamily && r.IsHeiyo).ToList();
                            break;
                        case 37:
                            printData.HokenSbt2 = "医保(家族)単独";
                            if (receDatas.Where(r => r.IsNrFamily && !r.IsHeiyo).Count() >= 1)
                            {
                                printDatas.Add(printData);
                            }
                            continue;
                        case 38:
                            printData.HokenSbt3 = "01       (協)";
                            wrkReces = receDatas.Where(r => r.IsNrFamily && !r.IsHeiyo && r.Houbetu == "01").ToList();
                            break;
                        case 39:
                            printData.HokenSbt3 = "02       (船)";
                            wrkReces = receDatas.Where(r => r.IsNrFamily && !r.IsHeiyo && r.Houbetu == "02").ToList();
                            break;
                        case 40:
                            printData.HokenSbt3 = "03       (日)";
                            wrkReces = receDatas.Where(r => r.IsNrFamily && !r.IsHeiyo && r.Houbetu == "03").ToList();
                            break;
                        case 41:
                            printData.HokenSbt3 = "04       (日特)";
                            wrkReces = receDatas.Where(r => r.IsNrFamily && !r.IsHeiyo && r.Houbetu == "04").ToList();
                            break;
                        case 42:
                            printData.HokenSbt3 = "31-34    (共)";
                            wrkReces = receDatas.Where(r => r.IsNrFamily && !r.IsHeiyo && new string[] { "31", "32", "33", "34" }.Contains(r.Houbetu)).ToList();
                            break;
                        case 43:
                            printData.HokenSbt3 = "06       (組)";
                            wrkReces = receDatas.Where(r => r.IsNrFamily && !r.IsHeiyo && r.Houbetu == "06").ToList();
                            break;
                        case 44:
                            printData.HokenSbt3 = "63,72-75 (退)";
                            wrkReces = receDatas.Where(r => r.IsNrFamily && !r.IsHeiyo && new string[] { "63", "72", "73", "74", "75" }.Contains(r.Houbetu)).ToList();
                            break;
                        case 45:
                            printData.RowType = RowType.Total;
                            printData.HokenSbt3 = "◆小計";
                            printData.DrawLine = true;
                            wrkReces = receDatas.Where(r => r.IsNrFamily && !r.IsHeiyo).ToList();
                            break;
                        //6歳未満 --------------------
                        case 46:
                            printData.HokenSbt2 = "医保(6歳)と公費の併用";
                            wrkReces = receDatas.Where(r => r.IsNrPreSchool && r.IsHeiyo).ToList();
                            break;
                        case 47:
                            printData.HokenSbt2 = "医保(6歳)単独";
                            if (receDatas.Where(r => r.IsNrPreSchool && !r.IsHeiyo).Count() >= 1)
                            {
                                printDatas.Add(printData);
                            }
                            continue;
                        case 48:
                            printData.HokenSbt3 = "01       (協)";
                            wrkReces = receDatas.Where(r => r.IsNrPreSchool && !r.IsHeiyo && r.Houbetu == "01").ToList();
                            break;
                        case 49:
                            printData.HokenSbt3 = "02       (船)";
                            wrkReces = receDatas.Where(r => r.IsNrPreSchool && !r.IsHeiyo && r.Houbetu == "02").ToList();
                            break;
                        case 50:
                            printData.HokenSbt3 = "03       (日)";
                            wrkReces = receDatas.Where(r => r.IsNrPreSchool && !r.IsHeiyo && r.Houbetu == "03").ToList();
                            break;
                        case 51:
                            printData.HokenSbt3 = "04       (日特)";
                            wrkReces = receDatas.Where(r => r.IsNrPreSchool && !r.IsHeiyo && r.Houbetu == "04").ToList();
                            break;
                        case 52:
                            printData.HokenSbt3 = "31-34    (共)";
                            wrkReces = receDatas.Where(r => r.IsNrPreSchool && !r.IsHeiyo && new string[] { "31", "32", "33", "34" }.Contains(r.Houbetu)).ToList();
                            break;
                        case 53:
                            printData.HokenSbt3 = "06       (組)";
                            wrkReces = receDatas.Where(r => r.IsNrPreSchool && !r.IsHeiyo && r.Houbetu == "06").ToList();
                            break;
                        case 54:
                            printData.HokenSbt3 = "63,72-75 (退)";
                            wrkReces = receDatas.Where(r => r.IsNrPreSchool && !r.IsHeiyo && new string[] { "63", "72", "73", "74", "75" }.Contains(r.Houbetu)).ToList();
                            break;
                        case 55:
                            printData.RowType = RowType.Total;
                            printData.HokenSbt3 = "◆小計";
                            printData.DrawLine = true;
                            wrkReces = receDatas.Where(r => r.IsNrPreSchool && !r.IsHeiyo).ToList();
                            break;
                        case 56:
                            printData.RowType = RowType.Total;
                            printData.HokenSbt2 = "◆合計             ①";
                            wrkReces = receDatas.Where(r => r.IsNrAll).ToList();
                            totalCount1 = wrkReces.Count;
                            break;
                    }
                    if ((wrkReces?.Count ?? 0) == 0) continue;

                    //集計
                    printData.Count = wrkReces.Count.ToString("#,0");
                    printData.Nissu = wrkReces.Sum(r => r.HokenNissu).ToString("#,0");
                    printData.Tensu = wrkReces.Sum(r => r.Tensu).ToString("#,0");
                    printData.Futan = wrkReces.Sum(r => r.HokenReceFutan).ToString("#,0");
                    printData.PtFutan = wrkReces.Sum(r => r.PtFutan).ToString("#,0");
                    printData.Furikomi = wrkReces.Sum(r => r.Furikomi).ToString("#,0");
                    //公費併用分
                    var wrkHeiyo = wrkReces.Where(r => r.IsHeiyo).ToList();
                    printData.KohiCount = wrkHeiyo.Sum(r => r.ReceCnt - 1).ToString("#,0");
                    printData.KohiNissu = wrkHeiyo.Sum(r => r.KohiReceNissu()).ToString("#,0");
                    printData.KohiTensu = wrkHeiyo.Sum(r => r.KohiReceTensu()).ToString("#,0");
                    printData.KohiFutan = wrkHeiyo.Sum(r => r.KohiReceFutan()).ToString("#,0");

                    printDatas.Add(printData);
                }
                #endregion

                int totalCount2 = 0;

                #region 公費と医保の併用
                List<string> kohiHoubetus = GetKohiHoubetu(receDatas.Where(r => r.IsNrAll && r.IsHeiyo).ToList(), null);
                if (kohiHoubetus.Count >= 1)
                {
                    printDatas.Add(new CoSta2010PrintData(RowType.Brank));
                    printDatas.Add(new CoSta2010PrintData() { HokenSbt1 = "公費負担" });
                    printDatas.Add(new CoSta2010PrintData() { HokenSbt2 = "公費と医保の併用" });
                }

                //集計
                for (short rowNo = 0; rowNo < kohiHoubetus.Count; rowNo++)
                {
                    List<CoReceInfModel> wrkReces = receDatas.Where(r => r.IsNrAll && r.IsHeiyo && r.IsKohi(kohiHoubetus[rowNo])).ToList();

                    printDatas.Add(
                        new CoSta2010PrintData()
                        {
                            //名称
                            HokenSbt3 = string.Format("{0} ({1})", kohiHoubetus[rowNo], GetKohiName(kohiHoubetus[rowNo])),
                            //集計
                            Count = wrkReces.Count.ToString("#,0"),
                            Nissu = wrkReces.Sum(r => r.KohiReceNissu(kohiHoubetus[rowNo])).ToString("#,0"),
                            Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus[rowNo])).ToString("#,0"),
                            Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus[rowNo])).ToString("#,0"),
                            PtFutan = "-",
                            Furikomi = "-",
                            KohiCount = "-",
                            KohiNissu = "-",
                            KohiTensu = "-",
                            KohiFutan = "-"
                        }
                    );

                    //合計
                    totalCount2 += wrkReces.Count;
                }

                //小計
                if (kohiHoubetus.Count >= 2)
                {
                    List<CoReceInfModel> wrkReces = receDatas.Where(r =>
                        r.IsNrAll &&
                        r.IsHeiyo
                    ).ToList();

                    printDatas.Add(
                        new CoSta2010PrintData()
                        {
                            RowType = RowType.Total,
                            HokenSbt3 = "◆小計",
                            Count = wrkReces.Sum(r => r.ReceCnt - 1).ToString("#,0"),
                            Nissu = wrkReces.Sum(r => r.KohiReceNissu()).ToString("#,0"),
                            Tensu = wrkReces.Sum(r => r.KohiReceTensu()).ToString("#,0"),
                            Futan = wrkReces.Sum(r => r.KohiReceFutan()).ToString("#,0"),
                            PtFutan = "-",
                            Furikomi = "-",
                            KohiCount = "-",
                            KohiNissu = "-",
                            KohiTensu = "-",
                            KohiFutan = "-",
                            DrawLine = true
                        }
                    );
                }
                #endregion

                #region 公費と公費の併用
                kohiHoubetus = GetHoubetuPair(receDatas.Where(r => r.IsKohiOnly && r.IsHeiyo).ToList());
                if (kohiHoubetus.Count >= 1)
                {
                    if (totalCount2 == 0)
                    {
                        printDatas.Add(new CoSta2010PrintData(RowType.Brank));
                        printDatas.Add(new CoSta2010PrintData() { HokenSbt1 = "公費負担" });
                    }
                    printDatas.Add(new CoSta2010PrintData() { HokenSbt2 = "公費と公費の併用" });
                }

                //集計
                for (short pariNo = 0; pariNo < kohiHoubetus.Count; pariNo++)
                {
                    for (int i = 0; i <= 1; i++)
                    {
                        string wrkHoubetu = kohiHoubetus[pariNo].Substring(i * 2, 2);
                        short rowNo = (short)(pariNo * 2 + i);

                        List<CoReceInfModel> wrkReces = receDatas.Where(r =>
                            r.IsKohiOnly &&
                            r.IsHeiyo &&
                            r.IsKohi(wrkHoubetu)
                        ).ToList();

                        printDatas.Add(
                            new CoSta2010PrintData()
                            {
                                //名称
                                HokenSbt3 = string.Format("{0} ({1})", wrkHoubetu, GetKohiName(wrkHoubetu)),
                                //集計
                                Count = wrkReces.Count.ToString("#,0"),
                                Nissu = wrkReces.Sum(r => r.KohiReceNissu(wrkHoubetu)).ToString("#,0"),
                                Tensu = wrkReces.Sum(r => r.KohiReceTensu(wrkHoubetu)).ToString("#,0"),
                                Futan = wrkReces.Sum(r => r.KohiReceFutan(wrkHoubetu)).ToString("#,0"),
                                PtFutan = i == 0 ? wrkReces.Sum(r => r.PtFutan).ToString("#,0") : "-",
                                Furikomi = i == 0 ? wrkReces.Sum(r => r.Furikomi).ToString("#,0") : "-",
                                KohiCount = "-",
                                KohiNissu = "-",
                                KohiTensu = "-",
                                KohiFutan = "-"
                            }
                        );

                        //合計
                        totalCount2 += wrkReces.Count;
                    }
                }

                //小計
                if (kohiHoubetus.Count >= 2)
                {
                    List<CoReceInfModel> wrkReces = receDatas.Where(r =>
                        r.IsKohiOnly &&
                        r.IsHeiyo
                    ).ToList();

                    printDatas.Add(
                        new CoSta2010PrintData()
                        {
                            RowType = RowType.Total,
                            HokenSbt3 = "◆小計",
                            Count = wrkReces.Sum(r => r.ReceCnt).ToString("#,0"),
                            Nissu = wrkReces.Sum(r => r.KohiReceNissu()).ToString("#,0"),
                            Tensu = wrkReces.Sum(r => r.KohiReceTensu()).ToString("#,0"),
                            Futan = wrkReces.Sum(r => r.KohiReceFutan()).ToString("#,0"),
                            PtFutan = wrkReces.Sum(r => r.PtFutan).ToString("#,0"),
                            Furikomi = wrkReces.Sum(r => r.Furikomi).ToString("#,0"),
                            KohiCount = "-",
                            KohiNissu = "-",
                            KohiTensu = "-",
                            KohiFutan = "-",
                            DrawLine = true
                        }
                    );
                }
                #endregion

                #region 公費単独
                kohiHoubetus = GetKohiHoubetu(receDatas.Where(r => r.IsKohiOnly && !r.IsHeiyo).ToList(), null);
                if (kohiHoubetus.Count >= 1)
                {
                    if (totalCount2 == 0)
                    {
                        printDatas.Add(new CoSta2010PrintData(RowType.Brank));
                        printDatas.Add(new CoSta2010PrintData() { HokenSbt1 = "公費負担" });
                    }
                    printDatas.Add(new CoSta2010PrintData() { HokenSbt2 = "公費単独" });
                }

                //集計
                for (short rowNo = 0; rowNo < kohiHoubetus.Count; rowNo++)
                {
                    List<CoReceInfModel> wrkReces = receDatas.Where(r => r.IsKohiOnly && !r.IsHeiyo && r.IsKohi(kohiHoubetus[rowNo])).ToList();

                    printDatas.Add(
                        new CoSta2010PrintData()
                        {
                            //名称
                            HokenSbt3 = string.Format("{0} ({1})", kohiHoubetus[rowNo], GetKohiName(kohiHoubetus[rowNo])),
                            //集計
                            Count = wrkReces.Count.ToString("#,0"),
                            Nissu = wrkReces.Sum(r => r.KohiReceNissu(kohiHoubetus[rowNo])).ToString("#,0"),
                            Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus[rowNo])).ToString("#,0"),
                            Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus[rowNo])).ToString("#,0"),
                            PtFutan = wrkReces.Sum(r => r.PtFutan).ToString("#,0"),
                            Furikomi = wrkReces.Sum(r => r.Furikomi).ToString("#,0"),
                            KohiCount = "-",
                            KohiNissu = "-",
                            KohiTensu = "-",
                            KohiFutan = "-"
                        }
                    );

                    //合計
                    totalCount2 += wrkReces.Count;
                }

                //小計
                if (kohiHoubetus.Count >= 2)
                {
                    List<CoReceInfModel> wrkReces = receDatas.Where(r =>
                        r.IsKohiOnly &&
                        !r.IsHeiyo
                    ).ToList();

                    printDatas.Add(
                        new CoSta2010PrintData()
                        {
                            RowType = RowType.Total,
                            HokenSbt3 = "◆小計",
                            Count = wrkReces.Sum(r => r.ReceCnt).ToString("#,0"),
                            Nissu = wrkReces.Sum(r => r.KohiReceNissu()).ToString("#,0"),
                            Tensu = wrkReces.Sum(r => r.KohiReceTensu()).ToString("#,0"),
                            Futan = wrkReces.Sum(r => r.KohiReceFutan()).ToString("#,0"),
                            PtFutan = wrkReces.Sum(r => r.PtFutan).ToString("#,0"),
                            Furikomi = wrkReces.Sum(r => r.Furikomi).ToString("#,0"),
                            KohiCount = "-",
                            KohiNissu = "-",
                            KohiTensu = "-",
                            KohiFutan = "-",
                            DrawLine = true
                        }
                    );
                }
                #endregion

                //公費負担合計
                printDatas.Add(
                    new CoSta2010PrintData()
                    {
                        RowType = RowType.Total,
                        HokenSbt2 = "◆合計             ②",
                        Count = totalCount2.ToString("#,0"),
                        Nissu = "-",
                        Tensu = "-",
                        Futan = "-",
                        PtFutan = "-",
                        Furikomi = "-",
                        KohiCount = "-",
                        KohiNissu = "-",
                        KohiTensu = "-",
                        KohiFutan = "-"
                    }
                );

                //社保総件数
                printDatas.Add(new CoSta2010PrintData(RowType.Brank));
                printDatas.Add(
                    new CoSta2010PrintData()
                    {
                        RowType = RowType.Total,
                        HokenSbt1 = "",
                        HokenSbt2 = "",
                        HokenSbt3 = "◆社保総件数       ① + ②",
                        Count = (totalCount1 + totalCount2).ToString("#,0"),
                        Nissu = "-",
                        Tensu = "-",
                        Futan = "-",
                        PtFutan = "-",
                        Furikomi = "-",
                        KohiCount = "-",
                        KohiNissu = "-",
                        KohiTensu = "-",
                        KohiFutan = "-"
                    }
                );

                //改ページ
                for (int i = printDatas.Count; i % maxRow != 0; i++)
                {
                    //空行を追加
                    printDatas.Add(new CoSta2010PrintData(RowType.Brank));
                }
            }
            #endregion

            #region 国保データ集計
            void AddKokhoData(List<CoReceInfModel> receDatas)
            {
                if (receDatas.Count == 0) return;

                printDatas.Add(
                    new CoSta2010PrintData() { HokenSbt1 = "国保" }
                );

                //保険者番号リストを取得
                var hokensyaNos = receDatas.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();

                for (int prefFlg = 0; prefFlg <= 1; prefFlg++)
                {
                    //県内→県外
                    bool printPrefIn = prefFlg == 0;
                    var prefReces = receDatas.Where(r => r.IsPrefIn == printPrefIn).ToList();
                    if (prefReces.Count >= 1)
                    {
                        if (!printPrefIn) printDatas.Add(new CoSta2010PrintData(RowType.Brank));
                        printDatas.Add(
                            new CoSta2010PrintData() { HokenSbt2 = printPrefIn ? "県内" : "県外" }
                        );
                    }

                    //保険者単位で出力
                    foreach (var currentNo in hokensyaNos)
                    {
                        var curReceInfs = receDatas.Where(r => r.IsPrefIn == printPrefIn && r.HokensyaNo == currentNo).ToList();
                        if (curReceInfs.Count == 0) continue;

                        #region 医療保険
                        for (int rowNo = 0; rowNo <= 11; rowNo++)
                        {
                            CoSta2010PrintData printData = new CoSta2010PrintData();
                            List<CoReceInfModel> wrkReces = null;

                            switch (rowNo)
                            {
                                case 0:
                                    string hokensyaName = hokensyaNames.Find(h => h.HokensyaNo == currentNo)?.Name ?? "";
                                    printData.HokenSbt3 = hokensyaName != "" ? string.Format("{0} ({1})", currentNo, hokensyaName) : currentNo;
                                    printDatas.Add(printData);
                                    break;
                                //国保
                                case 1:
                                    printData.HokenSbt4 = "一般(70歳以上一般・低所得)";
                                    wrkReces = curReceInfs.Where(r => r.IsNrElderIppan).ToList();
                                    break;
                                case 2:
                                    printData.HokenSbt4 = "一般(70歳以上7割)";
                                    wrkReces = curReceInfs.Where(r => r.IsNrElderUpper).ToList();
                                    break;
                                case 3:
                                    wrkReces = curReceInfs.Where(r => (r.IsNrMine || r.IsNrFamily) && r.HokenRate != 30).ToList();
                                    if (wrkReces.Count >= 1)
                                    {
                                        int wrkRate = (100 - wrkReces[0].HokenRate) / 10;
                                        printData.HokenSbt4 = string.Format("一般(被保険者{0}割)", wrkRate);
                                    }
                                    break;
                                case 4:
                                    printData.HokenSbt4 = "一般(被保険者7割)";
                                    wrkReces = curReceInfs.Where(r => (r.IsNrMine || r.IsNrFamily) && r.HokenRate == 30).ToList();
                                    break;
                                case 5:
                                    printData.HokenSbt4 = "一般(6歳)";
                                    wrkReces = curReceInfs.Where(r => r.IsNrPreSchool).ToList();
                                    break;
                                //退職
                                case 6:
                                    printData.HokenSbt4 = "退職(70歳以上9割)";
                                    wrkReces = curReceInfs.Where(r => r.IsRetElderIppan).ToList();
                                    break;
                                case 7:
                                    printData.HokenSbt4 = "退職(70歳以上7割)";
                                    wrkReces = curReceInfs.Where(r => r.IsRetElderUpper).ToList();
                                    break;
                                case 8:
                                    printData.HokenSbt4 = "退職(本人)";
                                    wrkReces = curReceInfs.Where(r => r.IsRetMine).ToList();
                                    break;
                                case 9:
                                    printData.HokenSbt4 = "退職(被扶養者)";
                                    wrkReces = curReceInfs.Where(r => r.IsRetFamily).ToList();
                                    break;
                                case 10:
                                    printData.HokenSbt4 = "退職(6歳)";
                                    wrkReces = curReceInfs.Where(r => r.IsRetPreSchool).ToList();
                                    break;
                                case 11:
                                    printData.RowType = RowType.Total;
                                    printData.HokenSbt4 = "◆小計";
                                    wrkReces = curReceInfs.ToList();
                                    break;
                            }
                            if ((wrkReces?.Count ?? 0) == 0) continue;

                            //集計
                            printData.Count = wrkReces.Count.ToString("#,0");
                            printData.Nissu = wrkReces.Sum(r => r.HokenNissu).ToString("#,0");
                            printData.Tensu = wrkReces.Sum(r => r.Tensu).ToString("#,0");
                            printData.Futan = wrkReces.Sum(r => r.HokenReceFutan).ToString("#,0");
                            printData.PtFutan = wrkReces.Sum(r => r.PtFutan).ToString("#,0");
                            printData.Furikomi = wrkReces.Sum(r => r.Furikomi).ToString("#,0");
                            //公費併用分
                            var wrkHeiyo = wrkReces.Where(r => r.IsHeiyo).ToList();
                            printData.KohiCount = wrkHeiyo.Sum(r => r.ReceCnt - 1).ToString("#,0");
                            printData.KohiNissu = wrkHeiyo.Sum(r => r.KohiReceNissu()).ToString("#,0");
                            printData.KohiTensu = wrkHeiyo.Sum(r => r.KohiReceTensu()).ToString("#,0");
                            printData.KohiFutan = wrkHeiyo.Sum(r => r.KohiReceFutan()).ToString("#,0");

                            printDatas.Add(printData);
                        }
                        #endregion

                        #region 公費負担医療
                        var kohiHoubetus = GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), null);
                        if (kohiHoubetus.Count >= 1) printDatas.Add(new CoSta2010PrintData(RowType.Brank));

                        //集計
                        for (short rowNo = 0; rowNo < kohiHoubetus.Count; rowNo++)
                        {
                            var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus[rowNo])).ToList();

                            printDatas.Add(
                                new CoSta2010PrintData()
                                {
                                    //名称
                                    HokenSbt4 = (rowNo == 0 ? "公費  " : "      ") +
                                        string.Format("{0} ({1})", kohiHoubetus[rowNo], GetKohiName(kohiHoubetus[rowNo])),
                                    //集計
                                    Count = wrkReces.Count.ToString("#,0"),
                                    Nissu = wrkReces.Sum(r => r.KohiReceNissu(kohiHoubetus[rowNo])).ToString("#,0"),
                                    Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus[rowNo])).ToString("#,0"),
                                    Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus[rowNo])).ToString("#,0"),
                                    PtFutan = "-",
                                    Furikomi = "-",
                                    KohiCount = "-",
                                    KohiNissu = "-",
                                    KohiTensu = "-",
                                    KohiFutan = "-"
                                }
                            );
                        }
                        printDatas.Last().DrawLine = true;
                        #endregion
                    }

                    //県内県外合計                    
                    if (prefReces.Count >= 1)
                    {
                        printDatas.Add(new CoSta2010PrintData(RowType.Brank));
                        printDatas.Add(
                            new CoSta2010PrintData()
                            {
                                RowType = RowType.Total,
                                HokenSbt2 = printPrefIn ? "◆県内合計" : "◆県外合計",
                                Count = prefReces.Count.ToString("#,0"),
                                Nissu = prefReces.Sum(r => r.HokenNissu).ToString("#,0"),
                                Tensu = prefReces.Sum(r => r.Tensu).ToString("#,0"),
                                Futan = prefReces.Sum(r => r.HokenReceFutan).ToString("#,0"),
                                PtFutan = prefReces.Sum(r => r.PtFutan).ToString("#,0"),
                                Furikomi = prefReces.Sum(r => r.Furikomi).ToString("#,0"),
                                KohiCount = prefReces.Sum(r => r.ReceCnt - 1).ToString("#,0"),
                                KohiNissu = prefReces.Sum(r => r.KohiReceNissu()).ToString("#,0"),
                                KohiTensu = prefReces.Sum(r => r.KohiReceTensu()).ToString("#,0"),
                                KohiFutan = prefReces.Sum(r => r.KohiReceFutan()).ToString("#,0")
                            }
                        );
                        printDatas.Add(new CoSta2010PrintData(RowType.Brank) { DrawLine = true });
                    }
                }

                //国保合計
                printDatas.Add(new CoSta2010PrintData(RowType.Brank));
                printDatas.Add(
                    new CoSta2010PrintData()
                    {
                        RowType = RowType.Total,
                        HokenSbt2 = "◆国保合計",
                        Count = receDatas.Count.ToString("#,0"),
                        Nissu = receDatas.Sum(r => r.HokenNissu).ToString("#,0"),
                        Tensu = receDatas.Sum(r => r.Tensu).ToString("#,0"),
                        Futan = receDatas.Sum(r => r.HokenReceFutan).ToString("#,0"),
                        PtFutan = receDatas.Sum(r => r.PtFutan).ToString("#,0"),
                        Furikomi = receDatas.Sum(r => r.Furikomi).ToString("#,0"),
                        KohiCount = receDatas.Sum(r => r.ReceCnt - 1).ToString("#,0"),
                        KohiNissu = receDatas.Sum(r => r.KohiReceNissu()).ToString("#,0"),
                        KohiTensu = receDatas.Sum(r => r.KohiReceTensu()).ToString("#,0"),
                        KohiFutan = receDatas.Sum(r => r.KohiReceFutan()).ToString("#,0")
                    }
                );

                //改ページ
                for (int i = printDatas.Count; i % maxRow != 0; i++)
                {
                    //空行を追加
                    printDatas.Add(new CoSta2010PrintData(RowType.Brank));
                }
            }
            #endregion

            #region 後期データ集計
            void AddKoukiData(List<CoReceInfModel> receDatas)
            {
                if (receDatas.Count == 0) return;

                printDatas.Add(
                    new CoSta2010PrintData() { HokenSbt1 = "後期" }
                );

                //保険者番号リストを取得
                var hokensyaNos = receDatas.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();

                for (int prefFlg = 0; prefFlg <= 1; prefFlg++)
                {
                    //県内→県外
                    bool printPrefIn = prefFlg == 0;

                    //保険者単位で出力
                    foreach (var currentNo in hokensyaNos)
                    {
                        var curReceInfs = receDatas.Where(r => r.IsPrefIn == printPrefIn && r.HokensyaNo == currentNo).ToList();
                        if (curReceInfs.Count == 0) continue;

                        #region 医療保険
                        for (int rowNo = 0; rowNo <= 3; rowNo++)
                        {
                            CoSta2010PrintData printData = new CoSta2010PrintData();
                            List<CoReceInfModel> wrkReces = null;

                            switch (rowNo)
                            {
                                case 0:
                                    string hokensyaName = hokensyaNames.Find(h => h.HokensyaNo == currentNo)?.Name ?? "";
                                    printData.HokenSbt2 = hokensyaName != "" ? string.Format("{0} ({1})", currentNo, hokensyaName) : currentNo;
                                    printDatas.Add(printData);
                                    break;
                                case 1:
                                    printData.HokenSbt3 = (_printConf.SeikyuYm >= KaiseiDate.m202210 ? "一般・低所得" : "9割");
                                    wrkReces = curReceInfs.Where(r => r.IsKoukiIppan).ToList();
                                    break;
                                case 2:
                                    printData.HokenSbt3 = "7割";
                                    wrkReces = curReceInfs.Where(r => r.IsKoukiUpper).ToList();
                                    break; ;
                                case 3:
                                    printData.RowType = RowType.Total;
                                    printData.HokenSbt3 = "◆小計";
                                    wrkReces = curReceInfs.ToList();
                                    break;
                            }
                            if ((wrkReces?.Count ?? 0) == 0) continue;

                            //集計
                            printData.Count = wrkReces.Count.ToString("#,0");
                            printData.Nissu = wrkReces.Sum(r => r.HokenNissu).ToString("#,0");
                            printData.Tensu = wrkReces.Sum(r => r.Tensu).ToString("#,0");
                            printData.Futan = wrkReces.Sum(r => r.HokenReceFutan).ToString("#,0");
                            printData.PtFutan = wrkReces.Sum(r => r.PtFutan).ToString("#,0");
                            printData.Furikomi = wrkReces.Sum(r => r.Furikomi).ToString("#,0");
                            //公費併用分
                            var wrkHeiyo = wrkReces.Where(r => r.IsHeiyo).ToList();
                            printData.KohiCount = wrkHeiyo.Sum(r => r.ReceCnt - 1).ToString("#,0");
                            printData.KohiNissu = wrkHeiyo.Sum(r => r.KohiReceNissu()).ToString("#,0");
                            printData.KohiTensu = wrkHeiyo.Sum(r => r.KohiReceTensu()).ToString("#,0");
                            printData.KohiFutan = wrkHeiyo.Sum(r => r.KohiReceFutan()).ToString("#,0");

                            printDatas.Add(printData);
                        }
                        #endregion

                        #region 公費負担医療
                        var kohiHoubetus = GetKohiHoubetu(curReceInfs.Where(r => r.IsHeiyo).ToList(), null);
                        if (kohiHoubetus.Count >= 1) printDatas.Add(new CoSta2010PrintData(RowType.Brank));

                        //集計
                        for (short rowNo = 0; rowNo < kohiHoubetus.Count; rowNo++)
                        {
                            var wrkReces = curReceInfs.Where(r => r.IsHeiyo && r.IsKohi(kohiHoubetus[rowNo])).ToList();

                            printDatas.Add(
                                new CoSta2010PrintData()
                                {
                                    //名称
                                    HokenSbt3 = (rowNo == 0 ? "公費  " : "      ") +
                                        string.Format("{0} ({1})", kohiHoubetus[rowNo], GetKohiName(kohiHoubetus[rowNo])),
                                    //集計
                                    Count = wrkReces.Count.ToString("#,0"),
                                    Nissu = wrkReces.Sum(r => r.KohiReceNissu(kohiHoubetus[rowNo])).ToString("#,0"),
                                    Tensu = wrkReces.Sum(r => r.KohiReceTensu(kohiHoubetus[rowNo])).ToString("#,0"),
                                    Futan = wrkReces.Sum(r => r.KohiReceFutan(kohiHoubetus[rowNo])).ToString("#,0"),
                                    PtFutan = "-",
                                    Furikomi = "-",
                                    KohiCount = "-",
                                    KohiNissu = "-",
                                    KohiTensu = "-",
                                    KohiFutan = "-"
                                }
                            );
                        }
                        printDatas.Last().DrawLine = true;
                        #endregion
                    }
                }

                //後期合計
                printDatas.Add(new CoSta2010PrintData(RowType.Brank));
                printDatas.Add(
                    new CoSta2010PrintData()
                    {
                        RowType = RowType.Total,
                        HokenSbt2 = "◆後期合計",
                        Count = receDatas.Count.ToString("#,0"),
                        Nissu = receDatas.Sum(r => r.HokenNissu).ToString("#,0"),
                        Tensu = receDatas.Sum(r => r.Tensu).ToString("#,0"),
                        Futan = receDatas.Sum(r => r.HokenReceFutan).ToString("#,0"),
                        PtFutan = receDatas.Sum(r => r.PtFutan).ToString("#,0"),
                        Furikomi = receDatas.Sum(r => r.Furikomi).ToString("#,0"),
                        KohiCount = receDatas.Sum(r => r.ReceCnt - 1).ToString("#,0"),
                        KohiNissu = receDatas.Sum(r => r.KohiReceNissu()).ToString("#,0"),
                        KohiTensu = receDatas.Sum(r => r.KohiReceTensu()).ToString("#,0"),
                        KohiFutan = receDatas.Sum(r => r.KohiReceFutan()).ToString("#,0")
                    }
                );

                //改ページ
                for (int i = printDatas.Count; i % maxRow != 0; i++)
                {
                    //空行を追加
                    printDatas.Add(new CoSta2010PrintData(RowType.Brank));
                }
            }
            #endregion

            #region その他データ集計
            void AddElseData(List<CoReceInfModel> receDatas)
            {
                if (receDatas.Count == 0) return;

                var rousaiReces = receDatas.Where(s => new int[] { HokenKbn.RousaiShort, HokenKbn.RousaiLong, HokenKbn.AfterCare }.Contains(s.HokenKbn)).ToList();
                if (rousaiReces.Count >= 1)
                {
                    printDatas.Add(
                        new CoSta2010PrintData() { HokenSbt1 = "労災" }
                    );
                }

                for (int rowNo = 0; rowNo <= 5; rowNo++)
                {
                    CoSta2010PrintData printData = new CoSta2010PrintData();
                    List<CoReceInfModel> wrkReces = null;

                    switch (rowNo)
                    {
                        case 0:
                            printData.HokenSbt2 = "短期給付";
                            wrkReces = receDatas.Where(r => r.HokenKbn == HokenKbn.RousaiShort).ToList();
                            break;
                        case 1:
                            printData.HokenSbt2 = "傷病年金";
                            wrkReces = receDatas.Where(r => r.HokenKbn == HokenKbn.RousaiLong).ToList();
                            break;
                        case 2:
                            printData.HokenSbt2 = "アフターケア";
                            wrkReces = receDatas.Where(r => r.HokenKbn == HokenKbn.AfterCare).ToList();
                            break; ;
                        case 3:
                            printData.RowType = RowType.Total;
                            printData.HokenSbt2 = "◆労災計";
                            wrkReces = rousaiReces.ToList();
                            break;
                        case 4:
                            printData.HokenSbt1 = "自賠責";
                            wrkReces = receDatas.Where(r => r.HokenKbn == HokenKbn.Jibai).ToList();
                            break;
                        case 5:
                            printData.HokenSbt1 = "自費レセ";
                            wrkReces = receDatas.Where(r => r.HokenKbn == HokenKbn.Jihi).ToList();
                            break;
                    }
                    if ((wrkReces?.Count ?? 0) == 0) continue;

                    //集計
                    printData.Count = wrkReces.Count.ToString("#,0");
                    printData.Nissu = wrkReces.Sum(r => r.HokenNissu).ToString("#,0");
                    printData.Tensu = wrkReces.Sum(r => r.Tensu).ToString("#,0");
                    printData.Futan = "-";
                    printData.PtFutan = wrkReces.Sum(r => r.PtFutan).ToString("#,0");
                    printData.Furikomi = wrkReces.Sum(r => r.Furikomi).ToString("#,0");
                    printData.KohiCount = "-";
                    printData.KohiNissu = "-";
                    printData.KohiTensu = "-";
                    printData.KohiFutan = "-";

                    printDatas.Add(printData);

                    //労災計の後ろに空行を追加
                    if (printData.RowType == RowType.Total && rousaiReces.Count != receDatas.Count)
                    {
                        printDatas.Add(new CoSta2010PrintData(RowType.Brank));
                    }
                }

                //改ページ
                for (int i = printDatas.Count; i % maxRow != 0; i++)
                {
                    //空行を追加
                    printDatas.Add(new CoSta2010PrintData(RowType.Brank));
                }
            }
            #endregion

            hpInf = _staFinder.GetHpInf(HpId, _printConf.SeikyuYm * 100 + 1);

            //データ取得
            receInfs = _staFinder.GetReceInfs(HpId, _printConf, hpInf.PrefNo);
            if ((receInfs?.Count ?? 0) == 0) return false;

            //公費法別番号リストを取得
            kohiHoubetuMsts = _staFinder.GetKohiHoubetuMst(HpId, _printConf.SeikyuYm);
            //保険者名を取得
            hokensyaNames = _staFinder.GetHokensyaName(HpId,
                receInfs.Where(r => r.HokenKbn == HokenKbn.Kokho).GroupBy(r => r.HokensyaNo).Select(r => r.Key).ToList()
            );

            //印刷用データの作成
            MakePrintData();

            return printDatas.Count > 0;
        }
        #endregion

        #region Update Draw Form
        private bool UpdateDrawForm()
        {
            _hasNextPage = true;

            #region SubMethod

            #region Header
            int UpdateFormHeader()
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
                int totalPage = (int)Math.Ceiling((double)printDatas.Count / maxRow);
                _extralData.Add("HeaderR_0_2_" + _currentPage, _currentPage + " / " + totalPage);
                //請求年月
                _extralData.Add("HeaderL_0_1_" + _currentPage, headerL1.Count >= _currentPage ? headerL1[_currentPage - 1] : "");
                //改ページ条件
                _extralData.Add("HeaderL_0_2_" + _currentPage, headerL2.Count >= _currentPage ? headerL2[_currentPage - 1] : "");

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                int hokIndex = (_currentPage - 1) * maxRow;

                //存在しているフィールドに絞り込み
                var existsCols = putColumns.Where(p => _objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

                for (short rowNo = 0; rowNo < maxRow; rowNo++)
                {
                    var printData = printDatas[hokIndex];
                    string baseListName = "";

                    Dictionary<string, CellModel> data = new();

                    //明細データ出力
                    foreach (var colName in existsCols)
                    {
                        var value = typeof(CoSta2010PrintData).GetProperty(colName).GetValue(printData);
                        AddListData(ref data, colName, value == null ? "" : value.ToString());

                        if (baseListName == "" && _objectRseList.Contains(colName))
                        {
                            baseListName = colName;
                        }
                    }

                    //区切り線を引く
                    if (printData.DrawLine && rowNo < maxRow - 1)
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

                return hokIndex;
            }
            #endregion

            #endregion

            try
            {
                if (UpdateFormHeader() < 0 || UpdateFormBody() < 0)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
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
            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta2010, "sta2010a.rse", new());
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

            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta2010, "sta2010a.rse", fieldInputList);
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            _maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == _rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? _maxRow;
        }

        public CommonExcelReportingModel ExportCsv(CoSta2010PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType)
        {
            this.coFileType = coFileType;
            this.isPutTotalRow = isPutTotalRow;
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

            var csvDatas = printDatas.Where(p => (p.RowType == RowType.Data || (isPutTotalRow && p.RowType == RowType.Total)) && p.Count != null).ToList();
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
            }

            string RecordData(CoSta2010PrintData csvData)
            {
                List<string> colDatas = new List<string>();

                foreach (var column in putCurColumns)
                {
                    var value = typeof(CoSta2010PrintData).GetProperty(column.CsvColName).GetValue(csvData);
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
