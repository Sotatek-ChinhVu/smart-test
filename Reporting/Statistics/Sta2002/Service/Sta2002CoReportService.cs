using Helper.Common;
using Helper.Constants;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta1001.Models;
using Reporting.Statistics.Sta2002.DB;
using Reporting.Statistics.Sta2002.Mapper;
using Reporting.Statistics.Sta2002.Models;
using System.Globalization;

namespace Reporting.Statistics.Sta2002.Service
{
    public class Sta2002CoReportService : ISta2002CoReportService
    {
        #region Private properties
        private readonly ICoSta2002Finder _staFinder;
        private readonly IReadRseReportFileService _readRseReportFileService;

        /// <summary>
        /// CoReport Model
        /// </summary>
        private int hpId;
        private int _maxRow = 45;
        private List<CoSta2002PrintData> printDatas = new();
        private List<string> headerL1 = new();
        private List<string> headerL2 = new();
        private List<CoSyunoInfModel> syunoInfs = new();
        private List<CoJihiSbtMstModel> jihiSbtMsts = new();
        private List<CoJihiSbtFutan> jihiSbtFutans = new();
        private CoHpInfModel hpInf = new();

        private CoSta2002PrintConf _printConf = new(0);

        private List<PutColumn> putCurColumns = new List<PutColumn>();

        private int _currentPage;
        private string _rowCountFieldName = string.Empty;
        private List<string> _objectRseList = new();
        private bool _hasNextPage;

        private readonly Dictionary<string, string> _singleFieldData = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _extralData = new Dictionary<string, string>();
        private readonly List<Dictionary<string, CellModel>> _tableFieldData = new List<Dictionary<string, CellModel>>();
        #endregion

        private List<PutColumn> csvTotalColumns = new List<PutColumn>
        {
            new PutColumn("RowType", "明細区分")
        };

        private List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("NyukinYmFmt", "診療年月", false, "NyukinYm"),
            new PutColumn("KaId", "診療科ID"),
            new PutColumn("KaSname", "診療科"),
            new PutColumn("TantoId", "担当医ID"),
            new PutColumn("TantoSname", "担当医"),
            new PutColumn("HokenSbtName", "保険種別名"),
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
            new PutColumn("PostAdjustFutan", "期間外調整額")
        };

        public Sta2002CoReportService(IReadRseReportFileService readRseReportFileService, ICoSta2002Finder staFinder)
        {
            _readRseReportFileService = readRseReportFileService;
            _staFinder = staFinder;
        }

        public CommonReportingRequestModel GetSta2002ReportingData(CoSta2002PrintConf printConf, int hpId)
        {
            this.hpId = hpId;
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


            return new Sta2002Mapper(_singleFieldData, _tableFieldData, _extralData, _rowCountFieldName).GetData();
        }

        #region UpdateDrawForm

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
                int totalPage = (int)Math.Ceiling((double)printDatas.Count / _maxRow);
                _extralData.Add("HeaderR_0_2_" + _currentPage, _currentPage + " / " + totalPage);
                //入金日
                _extralData.Add("HeaderL_0_1_" + _currentPage, headerL1.Count >= _currentPage ? headerL1[_currentPage - 1] : "");
                //改ページ条件
                _extralData.Add("HeaderL_0_2_" + _currentPage, headerL2.Count >= _currentPage ? headerL2[_currentPage - 1] : "");

                //期間
                SetFieldData("Range",
                    string.Format(
                        "期間: {0} ～ {1}",
                        CIUtil.SDateToShowSWDate(_printConf.StartNyukinYm * 100 + 1, 0, 1).Substring(0, 12),
                        CIUtil.SDateToShowSWDate(_printConf.EndNyukinYm * 100 + 1, 0, 1).Substring(0, 12)
                    )
                );

                return 1;
            }
            #endregion

            #region Body
            int UpdateFormBody()
            {
                int hokIndex = (_currentPage - 1) * _maxRow;

                //存在しているフィールドに絞り込み
                var existsCols = putColumns.Where(p => _objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

                for (short rowNo = 0; rowNo < _maxRow; rowNo++)
                {
                    var printData = printDatas[hokIndex];
                    string baseListName = "";

                    Dictionary<string, CellModel> data = new();

                    //保険外金額（内訳）タイトル
                    foreach (var jihiSbtMst in jihiSbtMsts)
                    {
                        SetFieldData(string.Format("tJihiFutanSbt{0}", jihiSbtMst.JihiSbt), jihiSbtMst.Name);
                    }

                    //明細データ出力
                    foreach (var colName in existsCols)
                    {
                        var value = typeof(CoSta2002PrintData).GetProperty(colName).GetValue(printData);
                        AddListData(ref data, colName, value == null ? "" : value.ToString());

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
                        AddListData(ref data, string.Format("JihiFutanSbt{0}", jihiSbtMst.JihiSbt), printData.JihiSbtFutans[i]);
                    }

                    //区切り線を引く
                    if (new int[] { 14, 37 }.Contains(rowNo))
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
        #region GetData
        private bool GetData(int hpId)
        {
            void MakePrintData()
            {
                printDatas = new List<CoSta2002PrintData>();
                headerL1 = new List<string>();
                headerL2 = new List<string>();

                //改ページ条件
                bool pbKaId = new int[] { _printConf.PageBreak1, _printConf.PageBreak2 }.Contains(1);
                bool pbTantoId = new int[] { _printConf.PageBreak1, _printConf.PageBreak2 }.Contains(2);

                var nyukinYms = syunoInfs.GroupBy(s => s.NyukinYm).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                foreach (var nyukinYm in nyukinYms)
                {
                    var kaIds = syunoInfs.GroupBy(s => s.KaId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                    for (int j = 0; (pbKaId && j <= kaIds.Count - 1) || j == 0; j++)
                    {
                        var tantoIds = syunoInfs.GroupBy(s => s.TantoId).OrderBy(s => s.Key).Select(s => s.Key).ToList();
                        for (int k = 0; (pbTantoId && k <= tantoIds.Count - 1) || k == 0; k++)
                        {
                            var curDatas = syunoInfs.Where(s =>
                                s.NyukinYm == nyukinYm &&
                                (pbKaId ? s.KaId == kaIds[j] : true) &&
                                (pbTantoId ? s.TantoId == tantoIds[k] : true)
                            ).ToList();

                            if (curDatas.Count == 0) continue;

                            //明細
                            for (int rowNo = 0; rowNo <= _maxRow - 1; rowNo++)
                            {
                                CoSta2002PrintData printData = new CoSta2002PrintData();

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

                                    //保険外
                                    case 38:
                                        printData.HokenSbtName = "自費";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Jihi && s.IsJihiNoRece).ToList();
                                        break;
                                    case 39:
                                        printData.HokenSbtName = "自費レセ";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Jihi && s.IsJihiRece).ToList();
                                        break;
                                    case 40:
                                        printData.HokenSbtName = "労災";
                                        wrkDatas = curDatas.Where(s => s.IsRousai).ToList();
                                        break;
                                    case 41:
                                        printData.HokenSbtName = "自賠責";
                                        wrkDatas = curDatas.Where(s => s.HokenKbn == HokenKbn.Jibai).ToList();
                                        break;
                                    //保険外計
                                    case 42:
                                        printData.HokenSbtName = "◆保険外計";
                                        printData.RowType = RowType.Total;
                                        wrkDatas = curDatas.Where(s => s.HokenKbn != HokenKbn.Syaho && s.HokenKbn != HokenKbn.Kokho).ToList();
                                        break;

                                    //総合計
                                    case 44:
                                        printData.HokenSbtName = "◆総合計";
                                        printData.RowType = RowType.Total;
                                        wrkDatas = curDatas.ToList();
                                        break;
                                }

                                if (wrkDatas == null)
                                {
                                    //空行を追加
                                    printDatas.Add(new CoSta2002PrintData(RowType.Brank));
                                    continue;
                                }

                                printData.NyukinYm = nyukinYm;
                                printData.KaId = (pbKaId || kaIds.Count == 1) ? curDatas.FirstOrDefault().KaId.ToString() : null;
                                printData.KaSname = (pbKaId || kaIds.Count == 1) ? curDatas.FirstOrDefault().KaSname : null;
                                printData.TantoId = (pbTantoId || tantoIds.Count == 1) ? curDatas.FirstOrDefault().TantoId.ToString() : null;
                                printData.TantoSname = (pbTantoId || tantoIds.Count == 1) ? curDatas.FirstOrDefault().TantoSname : null;
                                printData.SyosinCount = wrkDatas.Where(s => s.IsSinMonth && s.Syosaisin == "初診")
                                    .GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                                printData.SaisinCount = wrkDatas.Where(s => s.IsSinMonth && s.Syosaisin == "再診")
                                    .GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                                printData.Count = wrkDatas.Where(s => s.IsSinMonth)
                                    .GroupBy(s => s.RaiinNo).Count().ToString("#,0");
                                printData.NewPtCount = wrkDatas.Where(s => s.IsFirstRaiin)
                                    .GroupBy(s => s.PtNum).Count().ToString("#,0");
                                printData.PtCount = wrkDatas.Where(s => s.IsSinMonth)
                                    .GroupBy(s => s.PtNum).Count().ToString("#,0");
                                printData.Tensu = wrkDatas.Where(s => s.IsSinMonth)
                                    .GroupBy(s => s.RaiinNo)
                                    .Select(s => new { RaiinNo = s.Key, Tensu = s.Max(x => x.Tensu) })
                                    .Sum(s => s.Tensu).ToString("#,0");
                                printData.NewTensu = wrkDatas.Where(s => s.IsSinMonth)
                                    .GroupBy(s => s.RaiinNo)
                                    .Select(s => new { RaiinNo = s.Key, NewTensu = s.Max(x => x.NewTensu) })
                                    .Sum(s => s.NewTensu).ToString("#,0");
                                printData.PtFutan = wrkDatas.Where(s => s.IsSinMonth)
                                    .GroupBy(s => s.RaiinNo)
                                    .Select(s => new { RaiinNo = s.Key, PtFutan = s.Max(x => x.PtFutan) })
                                    .Sum(s => s.PtFutan).ToString("#,0");
                                printData.JihiFutan = wrkDatas.Where(s => s.IsSinMonth)
                                    .GroupBy(s => s.RaiinNo)
                                    .Select(s => new { RaiinNo = s.Key, JihiFutan = s.Max(x => x.JihiFutan) })
                                    .Sum(s => s.JihiFutan).ToString("#,0");
                                printData.JihiTax = wrkDatas.Where(s => s.IsSinMonth)
                                    .GroupBy(s => s.RaiinNo)
                                    .Select(s => new { RaiinNo = s.Key, JihiTax = s.Max(x => x.JihiTax) })
                                    .Sum(s => s.JihiTax).ToString("#,0");
                                printData.AdjustFutan = wrkDatas.Where(s => s.IsSinMonth)
                                    .GroupBy(s => s.RaiinNo)
                                    .Select(s => new { RaiinNo = s.Key, AdjustFutan = s.Max(x => x.AdjustFutan) })
                                    .Sum(s => s.AdjustFutan).ToString("#,0");

                                var wrkNyukins = wrkDatas.Where(s => s.IsSinMonth)
                                    .GroupBy(s => s.RaiinNo)
                                    .Select(s => new { RaiinNo = s.Key, NyukinSortNo = s.Max(x => x.NyukinSortNo) })
                                    .ToList();
                                int wrkSeikyu = 0;
                                int wrkNewSeikyu = 0;
                                foreach (var wrkNyukin in wrkNyukins)
                                {
                                    wrkSeikyu += wrkDatas.Find(s => s.RaiinNo == wrkNyukin.RaiinNo && s.NyukinSortNo == wrkNyukin.NyukinSortNo).SeikyuGaku;
                                    wrkNewSeikyu += wrkDatas.Find(s => s.RaiinNo == wrkNyukin.RaiinNo && s.NyukinSortNo == wrkNyukin.NyukinSortNo).NewSeikyuGaku;
                                }
                                printData.SeikyuGaku = wrkSeikyu.ToString("#,0");
                                printData.NewSeikyuGaku = wrkNewSeikyu.ToString("#,0");

                                //printData.SeikyuGaku = wrkDatas.GroupBy(s => s.RaiinNo)
                                //    .Select(s => new { RaiinNo = s.Key, SeikyuGaku = s.Min(x => x.SeikyuGaku) })
                                //    .Sum(s => s.SeikyuGaku).ToString("#,0");
                                //printData.NewSeikyuGaku = wrkDatas.GroupBy(s => s.RaiinNo)
                                //    .Select(s => new { RaiinNo = s.Key, NewSeikyuGaku = s.Min(x => x.NewSeikyuGaku) })
                                //    .Sum(s => s.NewSeikyuGaku).ToString("#,0");
                                printData.MenjyoGaku = wrkDatas.Where(s => s.IsSinMonth)
                                    .GroupBy(s => s.RaiinNo)
                                    .Select(s => new { RaiinNo = s.Key, MenjyoGaku = s.Max(x => x.MenjyoGaku) })
                                    .Sum(s => s.MenjyoGaku).ToString("#,0");
                                printData.NyukinGaku = wrkDatas.Sum(s => s.IsSinMonth ? s.NyukinGaku : 0).ToString("#,0");
                                printData.PostNyukinGaku = wrkDatas.Sum(s => !s.IsSinMonth ? s.NyukinGaku : 0).ToString("#,0");
                                printData.PostAdjustFutan = wrkDatas.Sum(s => !s.IsSinMonth ? s.NyukinAdjustFutan : 0).ToString("#,0");
                                printData.MisyuGaku =
                                    (
                                        int.Parse(printData.SeikyuGaku ?? "0", NumberStyles.Any) -
                                        int.Parse(printData.NyukinGaku ?? "0", NumberStyles.Any) -
                                        wrkDatas.Where(s => s.IsSinMonth && s.NyukinKbn == 0)
                                            .GroupBy(s => s.RaiinNo)
                                            .Select(s => new { RaiinNo = s.Key, SeikyuGaku = s.Max(x => x.SeikyuGaku) })
                                            .Sum(s => s.SeikyuGaku)
                                    ).ToString("#,0");

                                //自費種別ごとの金額
                                List<string> wrkPrintFutans = new List<string>();

                                var raiinNos = wrkDatas.Where(s => s.IsSinMonth)
                                    .GroupBy(s => s.RaiinNo)
                                    .Select(s => s.Key);
                                foreach (var jihiSbtMst in jihiSbtMsts)
                                {
                                    int wrkFutan = 0;
                                    foreach (var raiinNo in raiinNos)
                                    {
                                        wrkFutan += jihiSbtFutans.Find(f => f.RaiinNo == raiinNo && f.JihiSbt == jihiSbtMst.JihiSbt)?.JihiFutan ?? 0;
                                    }
                                    wrkPrintFutans.Add(wrkFutan.ToString("#,0"));
                                }
                                printData.JihiSbtFutans = wrkPrintFutans;

                                //行追加
                                printDatas.Add(printData);

                                //ヘッダー情報
                                if ((int)Math.Ceiling((double)(printDatas.Count) / _maxRow) > headerL1.Count)
                                {
                                    //入金月
                                    string wrkYm = CIUtil.Copy(CIUtil.SDateToShowSWDate(nyukinYm * 100 + 1, 0, 1, 1), 1, 13);
                                    headerL1.Add(wrkYm + "度");
                                    //改ページ条件
                                    List<string> wrkHeaders = new List<string>();
                                    if (pbKaId) wrkHeaders.Add(curDatas.First().KaSname);
                                    if (pbTantoId) wrkHeaders.Add(curDatas.First().TantoSname);

                                    if (wrkHeaders.Count >= 1) headerL2.Add(string.Join("／", wrkHeaders));
                                }
                            }
                        }
                    }
                }
            }

            hpInf = _staFinder.GetHpInf(hpId, _printConf.StartNyukinYm * 100 + 1);

            //データ取得
            syunoInfs = _staFinder.GetSyunoInfs(hpId, _printConf);
            if ((syunoInfs?.Count ?? 0) == 0) return false;

            jihiSbtMsts = _staFinder.GetJihiSbtMst(hpId);
            jihiSbtFutans = _staFinder.GetJihiSbtFutan(hpId, _printConf);

            //印刷用データの作成
            MakePrintData();

            return printDatas.Count > 0;
        }
        #endregion

        #region get field java

        private void GetFieldNameList()
        {
            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta2002, "sta2002a.rse", new());
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

            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta2002, "sta2002a.rse", fieldInputList);
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            _maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == _rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? _maxRow;
        }

        public CommonExcelReportingModel ExportCsv(CoSta2002PrintConf printConf, int monthFrom, int monthTo, string menuName, int hpId, bool isPutColName, bool isPutTotalRow)
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

            var csvDatas = printDatas.Where(p => p.RowType == RowType.Data || (isPutTotalRow && p.RowType == RowType.Total)).ToList();

            if (csvDatas.Count == 0) return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);

            List<string> wrkTitles = putCurColumns.Select(p => p.JpName).ToList();
            List<string> wrkColumns = putCurColumns.Select(p => p.CsvColName).ToList();

            //タイトル行
            retDatas.Add("\"" + string.Join("\",\"", wrkTitles.Union(jihiSbtMsts.Select(j => string.Format("保険外金額({0})", j.Name)))) + "\"");
            if (isPutColName)
            {
                retDatas.Add("\"" + string.Join("\",\"", wrkColumns.Union(jihiSbtMsts.Select(j => string.Format("JihiFutanSbt{0}", j.JihiSbt)))) + "\"");
            }

            //データ
            int totalRow = csvDatas.Count;
            int rowOutputed = 0;
            foreach (var csvData in csvDatas)
            {
                retDatas.Add(RecordData(csvData));
                rowOutputed++;
            }

            string RecordData(CoSta2002PrintData csvData)
            {
                List<string> colDatas = new List<string>();

                foreach (var column in putCurColumns)
                {
                    var value = typeof(CoSta2002PrintData).GetProperty(column.CsvColName).GetValue(csvData);
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

            return new CommonExcelReportingModel(fileName + ".csv", fileName, retDatas);
        }
        #endregion
    }
}
