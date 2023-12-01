using Helper.Common;
using Helper.Constants;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta2010.Models;
using Reporting.Statistics.Sta2011.DB;
using Reporting.Statistics.Sta2011.Mapper;
using Reporting.Statistics.Sta2011.Models;
using System.Linq;

namespace Reporting.Statistics.Sta2011.Service
{
    public class Sta2011CoReportService : ISta2011CoReportService
    {

        #region Constant
        private int _maxRow = 45;

        private readonly List<PutColumn> csvTotalColumns = new List<PutColumn>
        {
            new PutColumn("RowType", "明細区分"),
            new PutColumn("TotalCaption", "合計行")
        };

        private readonly List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("SeikyuYmFmt", "請求年月", true, "SeikyuYm"),
            new PutColumn("IsZaiiso", "在医総"),
            new PutColumn("KaId", "診療科ID"),
            new PutColumn("KaSname", "診療科"),
            new PutColumn("TantoId", "担当医ID"),
            new PutColumn("TantoSname", "担当医"),
            new PutColumn("HokenSbt1", "保険種別（階層１）", false),
            new PutColumn("HokenSbt2", "保険種別（階層２）", false),
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

        /// <summary>
        /// Finder
        /// </summary>
        private readonly ICoSta2011Finder _staFinder;
        private readonly IReadRseReportFileService _readRseReportFileService;
        private int HpId;
        private CoSta2011PrintConf _printConf;
        private int _currentPage;
        private string _rowCountFieldName = string.Empty;
        private List<string> _objectRseList = new();

        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoSta2011PrintData> printDatas;
        private List<string> headerL1;
        private List<string> headerL2;
        private List<CoZaiReceInfModel> receInfs;
        private CoHpInfModel hpInf;
        private bool _hasNextPage;
        private readonly List<PutColumn> putCurColumns = new();
        private readonly Dictionary<string, string> _singleFieldData = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _extralData = new Dictionary<string, string>();
        private readonly List<Dictionary<string, CellModel>> _tableFieldData = new List<Dictionary<string, CellModel>>();
        private CoFileType? coFileType;
        private bool isPutTotalRow;


        #endregion
        public Sta2011CoReportService(ICoSta2011Finder staFinder, IReadRseReportFileService readRseReportFileService)
        {
            _staFinder = staFinder;
            _readRseReportFileService = readRseReportFileService;
            _printConf = new();
            printDatas = new();
            headerL1 = new();
            headerL2 = new();
            receInfs = new();
            hpInf = new();
        }

        public CommonReportingRequestModel GetSta2011ReportingData(CoSta2011PrintConf printConf, int hpId)
        {
            try
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

                return new Sta2011Mapper(_singleFieldData, _tableFieldData, _extralData, _rowCountFieldName).GetData();
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
                printDatas = new List<CoSta2011PrintData>();
                headerL1 = new List<string>();
                headerL2 = new List<string>();
                int totalRow = 0;

                //改ページ条件
                bool pbZaiiso = new int[] { _printConf.PageBreak1, _printConf.PageBreak2, _printConf.PageBreak3 }.Contains(1);
                bool pbKaId = new int[] { _printConf.PageBreak1, _printConf.PageBreak2, _printConf.PageBreak3 }.Contains(2);
                bool pbTantoId = new int[] { _printConf.PageBreak1, _printConf.PageBreak2, _printConf.PageBreak3 }.Contains(3);

                var isZaiisoes = receInfs.GroupBy(s => s.IsZaiiso).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                for (int zaiCnt = 0; (pbZaiiso && zaiCnt <= 1) || zaiCnt == 0; zaiCnt++)
                {
                    var kaIds = receInfs.GroupBy(s => s.KaId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                    for (int kaCnt = 0; (pbKaId && kaCnt <= kaIds.Count - 1) || kaCnt == 0; kaCnt++)
                    {
                        var tantoIds = receInfs.GroupBy(s => s.TantoId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                        for (int taCnt = 0; (pbTantoId && taCnt <= tantoIds.Count - 1) || taCnt == 0; taCnt++)
                        {
                            var curDatas = receInfs.Where(s =>
                                (!pbZaiiso || s.IsZaiiso == isZaiisoes[zaiCnt]) &&
                                (!pbKaId || s.KaId == kaIds[kaCnt]) &&
                                (!pbTantoId || s.TantoId == tantoIds[taCnt])
                            ).ToList();

                            for (int rowNo = 0; rowNo <= _maxRow - 1; rowNo++)
                            {
                                CoSta2011PrintData printData = new CoSta2011PrintData();
                                List<CoZaiReceInfModel>? wrkReces = null;

                                printData.SeikyuYm = curDatas.FirstOrDefault()?.SeikyuYm ?? 0;
                                printData.IsZaiiso = pbZaiiso ? curDatas.FirstOrDefault()?.IsZaiiso.ToString() ?? string.Empty : string.Empty;
                                printData.KaId = (pbKaId || kaIds.Count == 1) ? curDatas.FirstOrDefault()?.KaId.ToString() ?? string.Empty : string.Empty;
                                printData.KaSname = (pbKaId || kaIds.Count == 1) ? curDatas.FirstOrDefault()?.KaSname ?? string.Empty : string.Empty;
                                printData.TantoId = (pbTantoId || tantoIds.Count == 1) ? curDatas.FirstOrDefault()?.TantoId.ToString() ?? string.Empty : string.Empty;
                                printData.TantoSname = (pbTantoId || tantoIds.Count == 1) ? curDatas.FirstOrDefault()?.TantoSname ?? string.Empty : string.Empty;

                                if (_printConf.IsZaitaku)
                                {
                                    //在宅患者を別枠に集計する
                                    switch (rowNo)
                                    {
                                        case 0:
                                            //空行
                                            printDatas.Add(new CoSta2011PrintData(RowType.Brank));

                                            printData.HokenSbt1 = "社保";
                                            printData.HokenSbt2 = "外来";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && r.IsNrAll && !r.IsZaitaku).ToList();
                                            break;
                                        case 1:
                                            printData.HokenSbt2 = "在宅";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && r.IsNrAll && r.IsZaitaku).ToList();
                                            break;
                                        case 2:
                                            printData.HokenSbt2 = "◆小計";
                                            printData.RowType = RowType.Total;
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && r.IsNrAll).ToList();
                                            break;
                                        case 3:
                                            //空行
                                            printDatas.Add(new CoSta2011PrintData(RowType.Brank));

                                            printData.HokenSbt1 = "公費";
                                            printData.HokenSbt2 = "外来";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && r.IsKohiOnly && !r.IsZaitaku).ToList();
                                            break;
                                        case 4:
                                            printData.HokenSbt2 = "在宅";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && r.IsKohiOnly && r.IsZaitaku).ToList();
                                            break;
                                        case 5:
                                            printData.HokenSbt2 = "◆小計";
                                            printData.RowType = RowType.Total;
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && r.IsKohiOnly).ToList();
                                            break;
                                        case 6:
                                            //空行
                                            printDatas.Add(new CoSta2011PrintData(RowType.Brank));

                                            printData.HokenSbt1 = "国保";
                                            printData.HokenSbt2 = "外来";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && (r.IsNrAll || r.IsRetAll) && !r.IsZaitaku).ToList();
                                            break;
                                        case 7:
                                            printData.HokenSbt2 = "在宅";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && (r.IsNrAll || r.IsRetAll) && r.IsZaitaku).ToList();
                                            break;
                                        case 8:
                                            printData.HokenSbt2 = "◆小計";
                                            printData.RowType = RowType.Total;
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && (r.IsNrAll || r.IsRetAll)).ToList();
                                            break;
                                        case 9:
                                            //空行
                                            printDatas.Add(new CoSta2011PrintData(RowType.Brank));

                                            printData.HokenSbt1 = "後期";
                                            printData.HokenSbt2 = "外来";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && r.IsKoukiAll && !r.IsZaitaku).ToList();
                                            break;
                                        case 10:
                                            printData.HokenSbt2 = "在宅";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && r.IsKoukiAll && r.IsZaitaku).ToList();
                                            break;
                                        case 11:
                                            printData.HokenSbt2 = "◆小計";
                                            printData.RowType = RowType.Total;
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && r.IsKoukiAll).ToList();
                                            break;
                                        case 12:
                                            //空行
                                            printDatas.Add(new CoSta2011PrintData(RowType.Brank));

                                            printData.HokenSbt1 = "労災";
                                            printData.HokenSbt2 = "外来";
                                            wrkReces = curDatas.Where(r => new int[] { HokenKbn.RousaiShort, HokenKbn.RousaiLong, HokenKbn.AfterCare }.Contains(r.HokenKbn) && !r.IsZaitaku).ToList();
                                            break;
                                        case 13:
                                            printData.HokenSbt2 = "在宅";
                                            wrkReces = curDatas.Where(r => new int[] { HokenKbn.RousaiShort, HokenKbn.RousaiLong, HokenKbn.AfterCare }.Contains(r.HokenKbn) && r.IsZaitaku).ToList();
                                            break;
                                        case 14:
                                            printData.HokenSbt2 = "◆小計";
                                            printData.RowType = RowType.Total;
                                            wrkReces = curDatas.Where(r => new int[] { HokenKbn.RousaiShort, HokenKbn.RousaiLong, HokenKbn.AfterCare }.Contains(r.HokenKbn)).ToList();
                                            break;
                                        case 15:
                                            //空行
                                            printDatas.Add(new CoSta2011PrintData(RowType.Brank));

                                            printData.HokenSbt1 = "自賠責";
                                            printData.HokenSbt2 = "外来";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Jibai && !r.IsZaitaku).ToList();
                                            break;
                                        case 16:
                                            printData.HokenSbt2 = "在宅";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Jibai && r.IsZaitaku).ToList();
                                            break;
                                        case 17:
                                            printData.HokenSbt2 = "◆小計";
                                            printData.RowType = RowType.Total;
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Jibai).ToList();
                                            break;
                                        case 18:
                                            //空行
                                            printDatas.Add(new CoSta2011PrintData(RowType.Brank));

                                            printData.HokenSbt1 = "自費レセ";
                                            printData.HokenSbt2 = "外来";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Jihi && !r.IsZaitaku).ToList();
                                            break;
                                        case 19:
                                            printData.HokenSbt2 = "在宅";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Jihi && r.IsZaitaku).ToList();
                                            break;
                                        case 20:
                                            printData.HokenSbt2 = "◆小計";
                                            printData.RowType = RowType.Total;
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Jihi).ToList();
                                            break;
                                        case 21:
                                            //空行
                                            printDatas.Add(new CoSta2011PrintData(RowType.Brank));
                                            printDatas.Add(new CoSta2011PrintData(RowType.Brank));

                                            printData.TotalCaption = "◆外来合計";
                                            printData.RowType = RowType.Total;
                                            wrkReces = curDatas.Where(r => !r.IsZaitaku).ToList();
                                            break;
                                        case 22:
                                            printData.TotalCaption = "◆在宅合計";
                                            printData.RowType = RowType.Total;
                                            wrkReces = curDatas.Where(r => r.IsZaitaku).ToList();
                                            break;
                                        case 23:
                                            //空行
                                            printDatas.Add(new CoSta2011PrintData(RowType.Brank));

                                            printData.TotalCaption = "◆合計";
                                            printData.RowType = RowType.Total;
                                            wrkReces = curDatas.ToList();
                                            break;
                                    }
                                }
                                else if (_printConf.IsUchiwake)
                                {
                                    //内訳を表示する
                                    switch (rowNo)
                                    {
                                        case 0:
                                            printData.HokenSbt1 = "社保";
                                            printData.HokenSbt2 = "　単独(本人)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && !r.IsHeiyo && r.IsNrMine).ToList();
                                            break;
                                        case 1:
                                            printData.HokenSbt2 = "　単独(６未)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && !r.IsHeiyo && r.IsNrPreSchool).ToList();
                                            break;
                                        case 2:
                                            printData.HokenSbt2 = "　単独(家族)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && !r.IsHeiyo && r.IsNrFamily).ToList();
                                            break;
                                        case 3:
                                            printData.HokenSbt2 = "　単独(高齢一般・低所)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && !r.IsHeiyo && r.IsNrElderIppan).ToList();
                                            break;
                                        case 4:
                                            printData.HokenSbt2 = "　単独(高齢上位)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && !r.IsHeiyo && r.IsNrElderUpper).ToList();
                                            break;
                                        case 5:
                                            printData.HokenSbt2 = "　併用(本人)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && r.IsHeiyo && r.IsNrMine).ToList();
                                            break;
                                        case 6:
                                            printData.HokenSbt2 = "　併用(６未)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && r.IsHeiyo && r.IsNrPreSchool).ToList();
                                            break;
                                        case 7:
                                            printData.HokenSbt2 = "　併用(家族)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && r.IsHeiyo && r.IsNrFamily).ToList();
                                            break;
                                        case 8:
                                            printData.HokenSbt2 = "　併用(高齢一般・低所)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && r.IsHeiyo && r.IsNrElderIppan).ToList();
                                            break;
                                        case 9:
                                            printData.HokenSbt2 = "　併用(高齢上位)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && r.IsHeiyo && r.IsNrElderUpper).ToList();
                                            break;
                                        //社保計
                                        case 10:
                                            printData.HokenSbt2 = "≪社保計≫";
                                            printData.RowType = RowType.Total;
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && r.IsNrAll).ToList();
                                            break;

                                        case 11:
                                            printData.HokenSbt2 = "　公費単独";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && !r.IsHeiyo && r.IsKohiOnly).ToList();
                                            break;
                                        case 12:
                                            printData.HokenSbt2 = "　公費併用";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && r.IsHeiyo && r.IsKohiOnly).ToList();
                                            break;
                                        //公費計
                                        case 13:
                                            printData.HokenSbt2 = "≪公費計≫";
                                            printData.RowType = RowType.Total;
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && r.IsKohiOnly).ToList();
                                            break;

                                        //社保合計
                                        case 14:
                                            printData.HokenSbt2 = "◆社保合計";
                                            printData.RowType = RowType.Total;
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho).ToList();
                                            break;

                                        case 15:
                                            printData.HokenSbt1 = "国保";
                                            printData.HokenSbt2 = "　単独(本人)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && !r.IsHeiyo && r.IsNrMine).ToList();
                                            break;
                                        case 16:
                                            printData.HokenSbt2 = "　単独(６未)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && !r.IsHeiyo && r.IsNrPreSchool).ToList();
                                            break;
                                        case 17:
                                            printData.HokenSbt2 = "　単独(家族)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && !r.IsHeiyo && r.IsNrFamily).ToList();
                                            break;
                                        case 18:
                                            printData.HokenSbt2 = "　単独(高齢一般・低所)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && !r.IsHeiyo && r.IsNrElderIppan).ToList();
                                            break;
                                        case 19:
                                            printData.HokenSbt2 = "　単独(高齢上位)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && !r.IsHeiyo && r.IsNrElderUpper).ToList();
                                            break;
                                        case 20:
                                            printData.HokenSbt2 = "　併用(本人)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && r.IsHeiyo && r.IsNrMine).ToList();
                                            break;
                                        case 21:
                                            printData.HokenSbt2 = "　併用(６未)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && r.IsHeiyo && r.IsNrPreSchool).ToList();
                                            break;
                                        case 22:
                                            printData.HokenSbt2 = "　併用(家族)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && r.IsHeiyo && r.IsNrFamily).ToList();
                                            break;
                                        case 23:
                                            printData.HokenSbt2 = "　併用(高齢一般・低所)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && r.IsHeiyo && r.IsNrElderIppan).ToList();
                                            break;
                                        case 24:
                                            printData.HokenSbt2 = "　併用(高齢上位)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && r.IsHeiyo && r.IsNrElderUpper).ToList();
                                            break;
                                        //国保計
                                        case 25:
                                            printData.HokenSbt2 = "≪国保計≫";
                                            printData.RowType = RowType.Total;
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && r.IsNrAll).ToList();
                                            break;

                                        case 26:
                                            printData.HokenSbt1 = "　　退職";
                                            printData.HokenSbt2 = "　単独(本人)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && !r.IsHeiyo && r.IsRetMine).ToList();
                                            break;
                                        case 27:
                                            printData.HokenSbt2 = "　単独(６未)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && !r.IsHeiyo && r.IsRetPreSchool).ToList();
                                            break;
                                        case 28:
                                            printData.HokenSbt2 = "　単独(家族)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && !r.IsHeiyo && r.IsRetFamily).ToList();
                                            break;
                                        case 29:
                                            printData.HokenSbt2 = "　併用(本人)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && r.IsHeiyo && r.IsRetMine).ToList();
                                            break;
                                        case 30:
                                            printData.HokenSbt2 = "　併用(６未)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && r.IsHeiyo && r.IsRetPreSchool).ToList();
                                            break;
                                        case 31:
                                            printData.HokenSbt2 = "　併用(家族)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && r.IsHeiyo && r.IsRetFamily).ToList();
                                            break;
                                        //退職計
                                        case 32:
                                            printData.HokenSbt2 = "≪退職計≫";
                                            printData.RowType = RowType.Total;
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && r.IsRetAll).ToList();
                                            break;

                                        case 33:
                                            printData.HokenSbt1 = "　　後期";
                                            printData.HokenSbt2 = "　単独(後期一般・低所)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && !r.IsHeiyo && r.IsKoukiIppan).ToList();
                                            break;
                                        case 34:
                                            printData.HokenSbt2 = "　単独(後期上位)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && !r.IsHeiyo && r.IsKoukiUpper).ToList();
                                            break;
                                        case 35:
                                            printData.HokenSbt2 = "　併用(後期一般・低所)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && r.IsHeiyo && r.IsKoukiIppan).ToList();
                                            break;
                                        case 36:
                                            printData.HokenSbt2 = "　併用(後期上位)";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && r.IsHeiyo && r.IsKoukiUpper).ToList();
                                            break;
                                        //後期計
                                        case 37:
                                            printData.HokenSbt2 = "≪後期計≫";
                                            printData.RowType = RowType.Total;
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho && r.IsKoukiAll).ToList();
                                            break;

                                        //国保合計
                                        case 38:
                                            printData.HokenSbt2 = "◆国保合計";
                                            printData.RowType = RowType.Total;
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Kokho).ToList();
                                            break;

                                        //保険合計
                                        case 39:
                                            printData.TotalCaption = "◆保険合計";
                                            printData.RowType = RowType.Total;
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho || r.HokenKbn == HokenKbn.Kokho).ToList();
                                            break;

                                        //保険外
                                        case 40:
                                            //空行
                                            printData.HokenSbt1 = "保険外";
                                            printData.HokenSbt2 = "労災";
                                            wrkReces = curDatas.Where(r => new int[] { HokenKbn.RousaiShort, HokenKbn.RousaiLong, HokenKbn.AfterCare }.Contains(r.HokenKbn)).ToList();
                                            break;
                                        case 41:
                                            printData.HokenSbt2 = "自賠責";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Jibai).ToList();
                                            break;
                                        case 42:
                                            printData.HokenSbt2 = "自費レセ";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Jihi).ToList();
                                            break;

                                        //総合計
                                        case 43:
                                            //空行
                                            printData.TotalCaption = "◆総合計";
                                            printData.RowType = RowType.Total;
                                            wrkReces = curDatas.ToList();
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (rowNo)
                                    {
                                        case 0:
                                            //空行
                                            printDatas.Add(new CoSta2011PrintData(RowType.Brank));

                                            printData.HokenSbt1 = "社保";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && r.IsNrAll).ToList();
                                            break;
                                        case 1:
                                            //空行
                                            printDatas.Add(new CoSta2011PrintData(RowType.Brank));

                                            printData.HokenSbt1 = "公費";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Syaho && r.IsKohiOnly).ToList();
                                            break;
                                        case 2:
                                            //空行
                                            printDatas.Add(new CoSta2011PrintData(RowType.Brank));

                                            printData.HokenSbt1 = "国保";
                                            wrkReces = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && (s.IsNrAll || s.IsRetAll)).ToList();
                                            break;
                                        case 3:
                                            //空行
                                            printDatas.Add(new CoSta2011PrintData(RowType.Brank));

                                            printData.HokenSbt1 = "後期";
                                            wrkReces = curDatas.Where(s => s.HokenKbn == HokenKbn.Kokho && s.IsKoukiAll).ToList();
                                            break;
                                        case 4:
                                            //空行
                                            printDatas.Add(new CoSta2011PrintData(RowType.Brank));

                                            printData.HokenSbt1 = "労災";
                                            wrkReces = curDatas.Where(s => new int[] { HokenKbn.RousaiShort, HokenKbn.RousaiLong, HokenKbn.AfterCare }.Contains(s.HokenKbn)).ToList();
                                            break;
                                        case 5:
                                            //空行
                                            printDatas.Add(new CoSta2011PrintData(RowType.Brank));

                                            printData.HokenSbt1 = "自賠責";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Jibai).ToList();
                                            break;
                                        case 6:
                                            //空行
                                            printDatas.Add(new CoSta2011PrintData(RowType.Brank));

                                            printData.HokenSbt1 = "自費レセ";
                                            wrkReces = curDatas.Where(r => r.HokenKbn == HokenKbn.Jihi).ToList();
                                            break;
                                        case 7:
                                            //空行
                                            printDatas.Add(new CoSta2011PrintData(RowType.Brank));
                                            printDatas.Add(new CoSta2011PrintData(RowType.Brank));

                                            printData.TotalCaption = "◆合計";
                                            printData.RowType = RowType.Total;
                                            wrkReces = curDatas.ToList();
                                            break;
                                    }
                                }
                                if (wrkReces == null) continue;

                                int rowKohi = 0;
                                if (_printConf.IsZaitaku)
                                {
                                    if (new int[] { 3, 4, 5 }.Contains(rowNo)) rowKohi = 1;
                                }
                                else if (_printConf.IsUchiwake)
                                {
                                    if (new int[] { 11, 12, 13 }.Contains(rowNo)) rowKohi = 1;
                                }
                                else
                                {
                                    if (rowNo == 1) rowKohi = 1;
                                }

                                //集計
                                printData.Count = wrkReces.Count.ToString("#,0");
                                printData.Nissu = rowKohi == 0 ? wrkReces.Sum(r => r.HokenNissu).ToString("#,0") : "-";
                                printData.Tensu = wrkReces.Sum(r => r.Tensu).ToString("#,0");
                                printData.Futan = rowKohi == 0 ? wrkReces.Sum(r => r.HokenReceFutan).ToString("#,0") : "-";
                                printData.PtFutan = wrkReces.Sum(r => r.PtFutan).ToString("#,0");
                                printData.Furikomi = wrkReces.Sum(r => r.Furikomi).ToString("#,0");
                                //公費併用分
                                var wrkHeiyo = wrkReces.Where(r => r.IsHeiyo || r.IsKohiOnly).ToList();
                                printData.KohiCount = wrkHeiyo.Sum(r => r.KohiCnt).ToString("#,0");
                                printData.KohiNissu = wrkHeiyo.Sum(r => r.KohiReceNissu()).ToString("#,0");
                                printData.KohiTensu = wrkHeiyo.Sum(r => r.KohiReceTensu()).ToString("#,0");
                                printData.KohiFutan = wrkHeiyo.Sum(r => r.KohiReceFutan()).ToString("#,0");

                                printDatas.Add(printData);
                            }

                            //改ページ
                            for (int i = printDatas.Count; i % _maxRow != 0; i++)
                            {
                                //空行を追加
                                printDatas.Add(new CoSta2011PrintData(RowType.Brank));
                            }

                            //ヘッダー情報
                            int rowCount = printDatas.Count - totalRow;
                            int pageCount = (int)Math.Ceiling((double)(rowCount) / _maxRow);
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
                    }
                }
            }

            hpInf = _staFinder.GetHpInf(HpId, _printConf.SeikyuYm * 100 + 1);

            //データ取得
            List<CoReceInfModel> wrkReceInfs = _staFinder.GetReceInfs(HpId, _printConf, hpInf.PrefNo);
            if ((wrkReceInfs?.Count ?? 0) == 0) return false;

            //在宅患者のリスト
            var zaitakuReces = _staFinder.GetZaitakuReces(HpId, _printConf);

            receInfs = wrkReceInfs!.Select(wrkReceInf => new CoZaiReceInfModel(
                        receInf: wrkReceInf.ReceInf,
                        ptHokenInf: wrkReceInf.PtHokenInf,
                        ptKohi1: wrkReceInf.PtKohi1,
                        ptKohi2: wrkReceInf.PtKohi2,
                        ptKohi3: wrkReceInf.PtKohi3,
                        ptKohi4: wrkReceInf.PtKohi4,
                        mainHokensyaNo: false,
                        prefNo: hpInf.PrefNo,
                        kaMst: wrkReceInf.KaMst,
                        tantoMst: wrkReceInf.TantoMst,
                        isZaitaku:
                            zaitakuReces?.FindIndex(r =>
                                r.SeikyuYm == wrkReceInf.SeikyuYm &&
                                r.PtId == wrkReceInf.PtId &&
                                r.SinYm == wrkReceInf.SinYm &&
                                r.HokenId == wrkReceInf.HokenId
                            ) >= 0
                    )).ToList();

            //印刷用データの作成
            MakePrintData();

            return printDatas.Count > 0;
        }
        #endregion

        #region Update DrawForm
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
                int totalPage = (int)Math.Ceiling((double)printDatas.Count / _maxRow);
                _extralData.Add("HeaderR_0_2_" + _currentPage, _currentPage + " / " + totalPage);
                //請求年月
                _extralData.Add("HeaderL_0_1_" + _currentPage, headerL1.Count >= _currentPage ? headerL1[_currentPage - 1] : string.Empty);
                //改ページ条件
                _extralData.Add("HeaderL_0_2_" + _currentPage, headerL2.Count >= _currentPage ? headerL2[_currentPage - 1] : string.Empty);
            }
            #endregion

            #region Body
            void UpdateFormBody()
            {
                int hokIndex = (_currentPage - 1) * _maxRow;

                //存在しているフィールドに絞り込み
                var existsCols = putColumns.Where(p => _objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

                for (short rowNo = 0; rowNo < _maxRow; rowNo++)
                {
                    var printData = printDatas[hokIndex];
                    string baseListName = string.Empty;

                    Dictionary<string, CellModel> data = new();
                    //明細データ出力
                    foreach (var colName in existsCols)
                    {
                        var value = typeof(CoSta2011PrintData).GetProperty(colName)?.GetValue(printData);
                        AddListData(ref data, colName, value == null ? string.Empty : value.ToString() ?? string.Empty);

                        if (baseListName == "" && _objectRseList.Contains(colName))
                        {
                            baseListName = colName;
                        }
                    }

                    //合計行キャプション
                    AddListData(ref data, "TotalCaption", printData.TotalCaption);

                    //区切り線を引く（内訳を表示する）
                    if ((_printConf.IsUchiwake) && (new int[] { 14, 38, 39, 42 }.Contains(rowNo)))
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
            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta2011, "sta2011a.rse", new());
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

            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta2011, "sta2011a.rse", fieldInputList);
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            _maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == _rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? _maxRow;
        }

        public CommonExcelReportingModel ExportCsv(CoSta2011PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow, CoFileType? coFileType)
        {
            _printConf = printConf;
            HpId = hpId;
            this.isPutTotalRow = isPutTotalRow;
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
            int rowOutputed = 0;
            foreach (var csvData in csvDatas)
            {
                retDatas.Add(RecordData(csvData));
                rowOutputed++;
            }

            string RecordData(CoSta2011PrintData csvData)
            {
                List<string> colDatas = new();

                foreach (var column in putCurColumns)
                {
                    var value = typeof(CoSta2011PrintData).GetProperty(column.CsvColName)?.GetValue(csvData);
                    if (csvData.RowType == RowType.Total && !column.IsTotal)
                    {
                        value = string.Empty;
                    }
                    else if (value is RowType)
                    {
                        value = (int)value;
                    }
                    colDatas.Add("\"" + (value == null ? string.Empty : value.ToString()) + "\"");
                }

                return string.Join(",", colDatas);
            }

            return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);
        }
        #endregion
    }
}
