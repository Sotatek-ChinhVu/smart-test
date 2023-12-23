using Domain.Constant;
using Helper.Common;
using Infrastructure.Interfaces;
using Reporting.AccountingCard.DB;
using Reporting.AccountingCard.Model;
using Reporting.Calculate.Interface;
using Reporting.Calculate.Receipt.Constants;
using Reporting.Calculate.Receipt.Models;
using Reporting.Calculate.Receipt.ViewModels;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.Receipt.Models;

namespace Reporting.AccountingCard.Service
{
    public class AccountingCardCoReportService : IAccountingCardCoReportService
    {
        #region Constant
        /// <summary>
        /// 病名欄の病番号の幅
        /// </summary>
        private const int CON_BYOMEI_NO_WIDTH = 3;
        /// <summary>
        /// 摘要欄の点数回数の幅
        /// </summary>
        private const int CON_TEKIYO_TENKAI_WIDTH = 13;
        /// <summary>
        /// 摘要欄の病名番号の幅
        /// </summary>
        private const int CON_TEKIYO_BYOMEI_NO_WIDTH = 4;
        /// <summary>
        /// 摘要欄の転帰日の幅
        /// </summary>
        private const int CON_TEKIYO_BYOMEI_TENKIDATE_WIDTH = 5;
        /// <summary>
        /// 摘要欄の病名開始日の幅（病名との間のスペース(2)を含む
        /// </summary>
        private const int CON_TEKIYO_BYOMEI_START_WIDTH = 18;
        /// <summary>
        /// 摘要欄の転帰の幅
        /// </summary>
        private const int CON_TEKIYO_BYOMEI_TENKI_WIDTH = 4;
        #endregion

        #region Private properties

        private readonly ICoAccountingCardFinder _finder;
        private readonly IEmrLogger _emrLogger;
        private readonly ITenantProvider _tenantProvider;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IReadRseReportFileService _readRseReportFileService;
        private CoAccountingCardModel coModel;
        private int _byomeiCharCount;
        private int _byomeiRowCount;
        private int _tekiyoCharCount;
        private int _tekiyoByoCharCount;
        private int _tekiyoRowCount;

        private int _currentPage;
        private bool hasNextPage;
        private DateTime _printoutDateTime;
        List<CoReceiptByomeiModel> byomeiModels;
        List<CoReceiptTekiyoModel> tekiyoModels;

        private readonly Dictionary<int, Dictionary<string, string>> _singleFieldDataM = new Dictionary<int, Dictionary<string, string>>();
        private readonly Dictionary<string, string> _singleFieldData = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _extralData = new Dictionary<string, string>();
        private readonly Dictionary<int, List<ListTextObject>> _listTextData = new Dictionary<int, List<ListTextObject>>();
        private readonly Dictionary<string, bool> _visibleFieldData = new Dictionary<string, bool>();
        #endregion

        #region Constructor and Init
        public AccountingCardCoReportService(ICoAccountingCardFinder finder, ISystemConfigProvider systemConfigProvider, ITenantProvider tenantProvider, IEmrLogger emrLogger, IReadRseReportFileService readRseReportFileService)
        {
            _finder = finder;
            _systemConfigProvider = systemConfigProvider;
            _tenantProvider = tenantProvider;
            _emrLogger = emrLogger;
            _readRseReportFileService = readRseReportFileService;
            coModel = new();
            byomeiModels = new();
            tekiyoModels = new();
        }

        public CommonReportingRequestModel GetAccountingCardReportingData(int hpId, long ptId, int sinYm, int hokenId, bool includeOutDrug)
        {
            try
            {
                _hpId = hpId;
                _ptId = ptId;
                _sinYm = sinYm;
                _hokenId = hokenId;
                _includeOutDrug = includeOutDrug;
                _printoutDateTime = CIUtil.GetJapanDateTimeNow();
                byomeiModels = new List<CoReceiptByomeiModel>();
                tekiyoModels = new List<CoReceiptTekiyoModel>();
                coModel = GetData();
                if (coModel != null && coModel.PtInfModel != new CoPtInfModel())
                {
                    _currentPage = 1;
                    hasNextPage = true;
                    GetRowCount("fmAccountingCard.rse");
                    MakeByoList();

                    MakeTekiyoList();

                    while (hasNextPage)
                    {
                        UpdateDrawForm();
                        _currentPage++;
                    }
                }

                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
                _extralData.Add("totalPage", pageIndex.ToString());
                return new CommonMapper(_singleFieldDataM, _listTextData, _extralData, "fmAccountingCard.rse", _singleFieldData, _visibleFieldData, (int)CoReportType.AccountingCard, "会計カード").GetData();
            }
            finally
            {
                _finder.ReleaseResource();
                _systemConfigProvider.ReleaseResource();
                _tenantProvider.DisposeDataContext();
            }

        }
        #endregion

        #region Init properties
        private int _hpId;
        private long _ptId;
        private int _sinYm;
        private int _hokenId;
        private bool _includeOutDrug;

        #endregion

        #region Printer method

        #endregion

        #region Private function
        /// <summary>
        /// 病名データを加工
        /// </summary>
        private void MakeByoList()
        {

            const string CON_BYOMEI_CONTINUE = "以下、診療内容欄";

            if (_byomeiCharCount <= 0 || _byomeiRowCount <= 0) return;

            #region sub function
            // 病名リストに追加
            void _addByomeiList(string addByomei, string addStartDate, string addByomeiTenki, string addByomeiTenkiDate, ref int byomeiIndex)
            {
                bool firstLine = true;
                string wkline = addByomei;

                if (wkline != "")
                {
                    while (wkline != "")
                    {
                        CoReceiptByomeiModel addByomeiModel = new CoReceiptByomeiModel();

                        string tmp = wkline;
                        if (CIUtil.LenB(tmp) > _byomeiCharCount)
                        {
                            tmp = CIUtil.CiCopyStrWidth(tmp, 1, _byomeiCharCount);
                        }

                        if (firstLine)
                        {
                            addByomeiModel.Byomei = $"({byomeiIndex})" + tmp;
                            firstLine = false;
                        }
                        else
                        {
                            addByomeiModel.Byomei = new string(' ', CON_BYOMEI_NO_WIDTH) + tmp;
                        }

                        byomeiModels.Add(addByomeiModel);

                        wkline = CIUtil.CiCopyStrWidth(wkline, CIUtil.LenB(tmp) + 1, CIUtil.LenB(wkline) - CIUtil.LenB(tmp));

                    }

                    byomeiModels.Last().StartDate = $"({byomeiIndex}){addStartDate}";
                    byomeiModels.Last().Tenki = $"({byomeiIndex}){addByomeiTenki} {addByomeiTenkiDate}";

                    byomeiIndex++;
                }
            }

            //摘要欄に追加
            void _addTekiyoList(string addByomei, string addStartDate, string addByomeiTenki, string addByomeiTenkiDate, ref int byomeiIndex)
            {
                bool firstLine = true;
                string wkline = addByomei;

                if (addByomei != "")
                {
                    while (wkline != "")
                    {
                        string tmp = wkline;
                        if (CIUtil.LenB(tmp) > _tekiyoByoCharCount)
                        {
                            tmp = CIUtil.CiCopyStrWidth(wkline, 1, _tekiyoByoCharCount);
                        }

                        CoReceiptTekiyoModel addTekiyo = new CoReceiptTekiyoModel();

                        if (firstLine)
                        {
                            addTekiyo.Tekiyo = $"({byomeiIndex})".PadRight(CON_TEKIYO_BYOMEI_NO_WIDTH, ' ') + tmp;
                            firstLine = false;
                        }
                        else
                        {
                            addTekiyo.Tekiyo = new string(' ', CON_TEKIYO_BYOMEI_NO_WIDTH) + tmp;
                        }

                        tekiyoModels.Add(addTekiyo);

                        wkline = CIUtil.CiCopyStrWidth(wkline, CIUtil.LenB(tmp) + 1, CIUtil.LenB(wkline) - CIUtil.LenB(tmp));

                    }

                    tekiyoModels.Last().Tekiyo =
                        CIUtil.PadRightB(tekiyoModels.Last().Tekiyo, _tekiyoByoCharCount + CON_TEKIYO_BYOMEI_NO_WIDTH) + "  " +
                        addStartDate +
                        addByomeiTenki +
                        CIUtil.Copy(addByomeiTenkiDate, 10, 3);

                    byomeiIndex++;
                }
            }

            //病名データ追加後の病名欄の行数を計算
            int _ifAddRowCount(string addLine)
            {
                int lineCount = CIUtil.LenB(addLine) / _byomeiCharCount;
                if (CIUtil.LenB(addLine) % _byomeiCharCount > 0)
                {
                    lineCount++;
                }

                return byomeiModels.Count + lineCount;
            }

            //病名転帰日、出力設定でない場合は空文字を返す
            string _getTenkiDate(CoPtByomeiModel syobyoData)
            {
                string ret = "";

                if (syobyoData.TenkiDate > 0 && syobyoData.TenkiKbn > TenkiKbnConst.Continued)
                {
                    ret = CIUtil.SDateToShowWDate3(syobyoData.TenkiDate).Ymd;
                }

                return ret;
            }
            #endregion


            // 1行1病名
            int byoIndex = 1;
            for (int j = 0; j < coModel.PtByomeiModels?.Count; j++)
            {
                if (byomeiModels.Count < _byomeiRowCount)
                {
                    string tmpLine = coModel.PtByomeiModels[j].ReceByomei;

                    int lineCount = _ifAddRowCount(tmpLine);

                    if (lineCount > _byomeiRowCount || (lineCount == _byomeiRowCount && (coModel.PtByomeiModels.Count - j - 1 > 0)))
                    {
                        for (int k = byomeiModels.Count; k < _byomeiRowCount - 1; k++)
                        {
                            byomeiModels.Add(new CoReceiptByomeiModel());
                        }

                        CoReceiptByomeiModel addByomeiModel = new CoReceiptByomeiModel
                        {
                            Byomei = CON_BYOMEI_CONTINUE
                        };
                        byomeiModels.Add(addByomeiModel);

                        // 摘要欄に追加
                        _addTekiyoList(
                            tmpLine,
                            CIUtil.SDateToShowWDate3(coModel.PtByomeiModels[j].StartDate).Ymd,
                            coModel.PtByomeiModels[j].Tenki,
                            _getTenkiDate(coModel.PtByomeiModels[j]), ref byoIndex);
                    }
                    else
                    {
                        _addByomeiList(
                            tmpLine,
                            CIUtil.SDateToShowWDate3(coModel.PtByomeiModels[j].StartDate).Ymd,
                            coModel.PtByomeiModels[j].Tenki,
                            _getTenkiDate(coModel.PtByomeiModels[j]), ref byoIndex);
                    }
                }
                else
                {
                    string tmpLine = coModel.PtByomeiModels[j].ReceByomei;
                    _addTekiyoList(
                        tmpLine,
                        CIUtil.SDateToShowWDate3(coModel.PtByomeiModels[j].StartDate).Ymd,
                        coModel.PtByomeiModels[j].Tenki,
                        _getTenkiDate(coModel.PtByomeiModels[j]), ref byoIndex);
                }
            }

            if (tekiyoModels.Any())
            {
                // 摘要欄に病名を追加する場合、区切り線を付けておく
                CoReceiptTekiyoModel addTekiyo = new CoReceiptTekiyoModel
                {
                    Tekiyo = new string('-', _tekiyoCharCount + CON_TEKIYO_TENKAI_WIDTH),
                    SinId = "---",
                    Mark = "--"
                };
                tekiyoModels.Add(addTekiyo);
            }
        }
        /// <summary>
        /// 摘要欄データ作成（健保用）
        /// </summary>
        private void MakeTekiyoList()
        {
            #region sub function
            //摘要欄に追加
            void _addTekiyoList(string addSinId, string addMark, string addTekiyo, string addTenCount, int maxCharCount)
            {
                AddTekiyoList(tekiyoModels, addSinId, addMark, addTekiyo, addTenCount, maxCharCount, CON_TEKIYO_TENKAI_WIDTH);
            }

            // 単純に摘要欄に文字列追加する
            void _addTekiyoLine(string sinId, string mark, string tekiyo, string tenCount)
            {
                AddTekiyoLine(tekiyoModels, sinId, mark, tekiyo, tenCount, _tekiyoCharCount, CON_TEKIYO_TENKAI_WIDTH);
            }
            #endregion


            // 明細
            int tmpSinId = 0;

            List<SinMeiDataModel> sinmeiDatas = coModel.SinMeiViewModel?.SinMei ?? new();

            foreach (SinMeiDataModel sinmeiData in sinmeiDatas)
            {
                string mark = "";
                string sinId = "";

                if (sinmeiData.RowNo == 1)
                {
                    mark = "*";
                }

                if (sinmeiData.SinId > 0 && sinmeiData.SinId != tmpSinId)
                {
                    if (tmpSinId > 0)
                    {
                        _addTekiyoLine("", "", new string('-', _tekiyoCharCount + CON_TEKIYO_TENKAI_WIDTH), "");
                        tekiyoModels.Last().SinId = "---";
                        tekiyoModels.Last().Mark = "--";
                    }

                    sinId = CIUtil.ToStringIgnoreZero(sinmeiData.SinId);
                    tmpSinId = sinmeiData.SinId;
                }

                //レセコメント以外の場合
                _addTekiyoList(sinId, mark, sinmeiData.ItemName, sinmeiData.TenKai, _tekiyoCharCount);


                if (sinmeiData.TenKai != "")
                {
                    for (int i = 0; i < 31; i++)
                    {
                        tekiyoModels.Last().DayCount[i] = sinmeiData.Day(i + 1);
                    }
                }
            }
        }
        /// <summary>
        /// 摘要欄に文字列を追加する
        /// </summary>
        /// <param name="targetTekiyols">追加する摘要欄モデル</param>
        /// <param name="addSinId">診療区分</param>
        /// <param name="addMark">マーク</param>
        /// <param name="addTekiyo">摘要欄の内容</param>
        /// <param name="addTenCount">点数回数</param>
        /// <param name="maxCharCount">摘要欄の幅（byte）</param>
        /// <param name="tenkaiCount">点数回数の幅（byte）</param>
        void AddTekiyoList(List<CoReceiptTekiyoModel> targetTekiyols, string addSinId, string addMark, string addTekiyo, string addTenCount, int maxCharCount, int tenkaiCount)
        {
            bool firstLine = true;

            if (addMark != "" || addTekiyo != "")
            {
                string[] tmpLines = addTekiyo.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                foreach (string tmpline in tmpLines)
                {
                    string wkline = tmpline;
                    while (wkline != "")
                    {
                        string tmp = wkline;
                        if (CIUtil.LenB(tmp) > maxCharCount)
                        {
                            tmp = CIUtil.CiCopyStrWidth(wkline, 1, maxCharCount);
                        }

                        CoReceiptTekiyoModel addTekiyoModel = new CoReceiptTekiyoModel();

                        if (firstLine)
                        {
                            addTekiyoModel.SinId = addSinId;
                            addTekiyoModel.Mark = addMark;

                            firstLine = false;
                        }

                        addTekiyoModel.Tekiyo = tmp;

                        targetTekiyols.Add(addTekiyoModel);

                        wkline = CIUtil.CiCopyStrWidth(wkline, CIUtil.LenB(tmp) + 1, CIUtil.LenB(wkline) - CIUtil.LenB(tmp));

                    }
                    targetTekiyols.Last().Tekiyo = CIUtil.PadRightB(targetTekiyols.Last().Tekiyo, maxCharCount) + CIUtil.PadLeftB(addTenCount, tenkaiCount);
                }
            }
        }

        /// <summary>
        /// 単純に摘要欄に文字列追加する
        /// </summary>
        void AddTekiyoLine(List<CoReceiptTekiyoModel> targetTekiyols, string sinId, string mark, string tekiyo, string tenCount, int maxCharCount, int tenkaiCount)
        {
            CoReceiptTekiyoModel addTekiyoModel = new CoReceiptTekiyoModel();

            addTekiyoModel.SinId = sinId;
            addTekiyoModel.Mark = mark;
            addTekiyoModel.Tekiyo = CIUtil.PadRightB(tekiyo, maxCharCount) + CIUtil.PadLeftB(tenCount, tenkaiCount);

            targetTekiyols.Add(addTekiyoModel);
        }
        /// <summary>
        /// 印刷処理
        /// </summary>
        /// <param name="hasNextPage"></param>
        /// <returns></returns>
        private void UpdateDrawForm()
        {
            bool _hasNextPage = true;
            #region SubMethod
            List<ListTextObject> listDataPerPage = new();
            // ヘッダー 
            int UpdateFormHeader()
            {
                #region sub print out
                // ページ
                void _printPage()
                {
                    SetFieldData("dfPage", _currentPage.ToString());
                }
                // 日付関連
                #region print date
                // 発行日時
                void _printPrintDateTime()
                {
                    SetFieldData("dfPrintDateTime", _printoutDateTime.ToString("yyyy/MM/dd HH:mm"));
                    SetFieldData("dfPrintDateTimeW", CIUtil.SDateToShowWDate3(CIUtil.StrToIntDef(_printoutDateTime.ToString("yyyyMMdd"), 0)).Ymd + _printoutDateTime.ToString(" HH:mm"));
                }
                // 診療年月
                void _printSinYm()
                {
                    CIUtil.WarekiYmd wareki = CIUtil.SDateToShowWDate3(_sinYm * 100 + 1);
                    SetFieldData("dfSinYm", $"{wareki.Gengo} {wareki.Year}年{wareki.Month}月分");
                }
                #endregion

                // 患者情報関連
                #region print ptinf
                // 患者番号
                void _printPtNum()
                {
                    SetFieldData("dfPtNum", coModel.PtNum.ToString());
                }
                // 患者氏名
                void _printPtName()
                {
                    SetFieldData("dfPtName", coModel.PtName);
                }
                // 患者カナ氏名
                void _printPtKanaName()
                {
                    SetFieldData("dfPtKanaName", coModel.PtKanaName);
                }
                // 患者性別
                void _printPtSex()
                {
                    SetFieldData("dfPtSex", coModel.PtSex);
                }
                // 生年月日
                void _printBirthday()
                {
                    SetFieldData("dfBirthDay", CIUtil.SDateToShowWDate3(coModel.BirthDay).Ymd);
                }
                // 年齢
                void _printAge()
                {
                    SetFieldData("dfAge", coModel.Age.ToString());
                }
                #endregion

                // 保険関連
                #region print hoken
                // 保険種別
                void _printHokenSbt()
                {
                    if (new int[] { 1, 2 }.Contains(coModel.HokenKbn))
                    {
                        //健保
                        SetFieldData("dfHokenSbt", $"{coModel.HokenSbt}　{coModel.HokenRate}%");
                    }
                    else if (new int[] { 0 }.Contains(coModel.HokenKbn))
                    {
                        //自費
                        SetFieldData("dfHokenSbt", $"自費　{coModel.HokenRate}％");
                    }
                    else if (new int[] { 11, 12, 13 }.Contains(coModel.HokenKbn))
                    {
                        //労災
                        SetFieldData("dfHokenSbt", $"労災　{coModel.HokenRate}％");
                    }
                    else if (new int[] { 14 }.Contains(coModel.HokenKbn))
                    {
                        //自賠
                        SetFieldData("dfHokenSbt", $"自賠　{coModel.HokenRate}％");
                    }

                }
                // 保険者番号
                void _printHokensyaNo()
                {
                    if (new int[] { 1, 2 }.Contains(coModel.HokenKbn))
                    {
                        // 健保
                        SetFieldData("dfHokensyaNo", coModel.HokensyaNo);
                    }
                    else if (new int[] { 0 }.Contains(coModel.HokenKbn))
                    {
                        // 自費
                        SetFieldData("自費", coModel.HokensyaNo);
                    }
                    else if (new int[] { 11, 12, 13 }.Contains(coModel.HokenKbn))
                    {
                        //労災
                        SetFieldData("労災", coModel.HokensyaNo);
                    }
                    else if (new int[] { 14 }.Contains(coModel.HokenKbn))
                    {
                        //自賠
                        SetFieldData("自賠", coModel.HokensyaNo);
                    }
                }
                // 記号番号
                void _printKigoBango()
                {
                    if (new int[] { 1, 2 }.Contains(coModel.HokenKbn))
                    {
                        //健保
                        SetFieldData("dfKigo", coModel.Kigo);
                        SetFieldData("dfBango", coModel.Bango);
                        SetFieldData("dfEdaNo", coModel.EdaNo);
                    }
                    else if (new int[] { 0 }.Contains(coModel.HokenKbn))
                    {
                        //自費
                    }
                    else if (new int[] { 11, 12, 13 }.Contains(coModel.HokenKbn))
                    {
                        //労災
                        SetFieldData("dfKigo", coModel.RousaiKofuNo);
                        SetFieldData("dfBango", coModel.RousaiJigyosyoName);
                    }
                    else if (new int[] { 14 }.Contains(coModel.HokenKbn))
                    {
                        //自賠
                        SetFieldData("dfKigo", coModel.JibaiHokenName);
                        SetFieldData("dfBango", coModel.JibaiTanto);
                    }

                }
                // 公費負担者番号・受給者番号
                void _printKohiFutansyaNo_JyukyusyaNo()
                {
                    for (int i = 0; i < 4; i++)
                    {
                        // 公費負担者番号
                        SetFieldData($"dfFutansyaNo_K{i + 1}", coModel.KohiFutansyaNo(i));
                        // 公費受給者番号
                        SetFieldData($"dfJyukyusyaNo_K{i + 1}", coModel.KohiJyukyusyaNo(i));
                    }
                }
                #endregion

                // 日数・点数・金額関連
                #region print tensu kingaku
                // 実日数
                void _printNissu()
                {
                    SetFieldData("dfNissu", coModel.Nissu.ToString());
                }
                // 請求点数
                void _printSeikyuTensu()
                {
                    SetFieldData("dfTensu", coModel.SinryoTensu.ToString());
                }
                // 患者負担額
                void _printPtFutan()
                {
                    SetFieldData("dfPtFutan", coModel.PtFutan.ToString());
                }
                #endregion

                // 病名関連
                #region print byomei
                // 病名リスト
                void _printPtByomei()
                {
                    short i = 0;
                    foreach (CoReceiptByomeiModel byomei in byomeiModels)
                    {
                        listDataPerPage.Add(new("lsByomei", 0, i, byomei.Byomei));
                        listDataPerPage.Add(new("lsByomeiStart", 0, i, byomei.StartDate));
                        listDataPerPage.Add(new("lsByomeiTenki", 0, i, byomei.Tenki));
                        i++;
                    }
                }
                #endregion

                // 日付
                void _printDays()
                {
                    for (short i = 0; i < 31; i++)
                    {
                        listDataPerPage.Add(new("lsDay", i, 0, (i + 1).ToString()));
                    }

                    for (short i = 0; i < 31; i++)
                    {
                        for (int j = 0; j < tekiyoModels.Count; j++)
                        {
                            // 日付欄の処理
                            if (tekiyoModels[j].DayCount[i] > 0)
                            {
                                listDataPerPage.Add(new($"lsDayMaru", i, 0, "〇"));
                                break;
                            }
                        }
                    }

                }
                #endregion

                // ページ
                _printPage();

                // 日付関連
                #region date                
                // 発行日時
                _printPrintDateTime();

                // 診療年月
                _printSinYm();
                #endregion

                // 患者情報関連
                #region PtInf
                // 患者番号
                _printPtNum();

                // 漢字患者名
                _printPtName();

                // カナ患者名
                _printPtKanaName();

                // 性別
                _printPtSex();
                // 生年月日
                _printBirthday();
                // 年齢
                _printAge();
                #endregion
                // 保険関連
                #region HokenInf
                // 保険種別
                _printHokenSbt();
                // 保険者番号
                _printHokensyaNo();
                // 記号番号
                _printKigoBango();
                // 公費負担者番号・受給者番号
                _printKohiFutansyaNo_JyukyusyaNo();
                #endregion

                // 点数・金額関連
                #region tensu kingaku
                // 実日数
                _printNissu();
                // 診療点数
                _printSeikyuTensu();
                // 患者負担額
                _printPtFutan();

                #endregion

                // 病名関連
                #region byomei
                // 病名リスト
                if (_currentPage == 1)
                {
                    _printPtByomei();
                }

                #endregion

                // 日付
                _printDays();

                return 1;
            }

            // 本体
            int UpdateFormBody()
            {

                if (tekiyoModels == null || tekiyoModels.Count == 0 || _tekiyoRowCount <= 0)
                {
                    _hasNextPage = false;
                    return -1;
                }

                int dataIndex = (_currentPage - 1) * _tekiyoRowCount;

                for (short i = 0; i < _tekiyoRowCount; i++)
                {
                    listDataPerPage.Add(new($"lsSinId", 0, i, tekiyoModels[dataIndex].SinId));
                    listDataPerPage.Add(new($"lsTekiyoMark", 0, i, tekiyoModels[dataIndex].Mark));
                    listDataPerPage.Add(new($"lsTekiyo", 0, i, tekiyoModels[dataIndex].Tekiyo));

                    // 日付欄の処理
                    for (short j = 0; j < 31; j++)
                    {
                        if (tekiyoModels[dataIndex].DayCount[j] <= 0)
                        {
                            listDataPerPage.Add(new($"lsDayCount", j, i, "・"));
                        }
                        else
                        {
                            listDataPerPage.Add(new($"lsDayCount", j, i, tekiyoModels[dataIndex].DayCount[j].ToString()));
                        }
                    }

                    dataIndex++;
                    if (dataIndex >= tekiyoModels.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }

                // 次ページあり
                SetVisibleFieldData("lbNextPage", _hasNextPage);

                var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count() + 1;
                _listTextData.Add(pageIndex, listDataPerPage);

                return dataIndex;

            }
            #endregion

            if (UpdateFormHeader() < 0 || UpdateFormBody() < 0)
            {
                hasNextPage = _hasNextPage;
                return;
            }
            hasNextPage = _hasNextPage;
        }

        /// <summary>
        /// 印刷するデータを取得する
        /// </summary>
        /// <returns></returns>
        private CoAccountingCardModel GetData()
        {
            // 会計情報
            List<CoKaikeiInfModel> kaikeiInfModels = _finder.FindKaikeiInf(_hpId, _ptId, _sinYm, _hokenId);

            // 患者情報            
            CoPtInfModel ptInfModel = _finder.FindPtInf(_hpId, _ptId, CIUtil.GetLastDateOfMonth(_sinYm * 100 + 1));

            // 診療情報
            SinMeiViewModel sinMeiViewModel = new SinMeiViewModel(SinMeiMode.AccountingCard, _includeOutDrug, _hpId, _ptId, _sinYm, _hokenId, _tenantProvider, _systemConfigProvider, _emrLogger);

            // 病名
            List<CoPtByomeiModel> ptByomeiModels = _finder.FindPtByomei(_hpId, _ptId, _sinYm * 100 + 1, _sinYm * 100 + 31, _hokenId);

            return new CoAccountingCardModel(kaikeiInfModels, ptInfModel, sinMeiViewModel, ptByomeiModels);
        }

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

        private void GetRowCount(string formFileName)
        {
            List<ObjectCalculate> fieldInputList = new();

            fieldInputList.Add(new ObjectCalculate("lsByomei", (int)CalculateTypeEnum.GetListRowCount));
            fieldInputList.Add(new ObjectCalculate("lsByomei", (int)CalculateTypeEnum.GetFormatLength));
            fieldInputList.Add(new ObjectCalculate("lsTekiyo", (int)CalculateTypeEnum.GetFormatLength));
            fieldInputList.Add(new ObjectCalculate("lsTekiyo", (int)CalculateTypeEnum.GetListRowCount));

            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.AccountingCard, formFileName, fieldInputList);
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            _byomeiRowCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsByomei" && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? 0;
            _byomeiCharCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsByomei" && item.typeInt == (int)CalculateTypeEnum.GetFormatLength)?.result - CON_BYOMEI_NO_WIDTH ?? 0;
            _tekiyoCharCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsTekiyo" && item.typeInt == (int)CalculateTypeEnum.GetFormatLength)?.result - CON_TEKIYO_TENKAI_WIDTH ?? 0;
            _tekiyoByoCharCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsTekiyo" && item.typeInt == (int)CalculateTypeEnum.GetFormatLength)?.result - CON_TEKIYO_BYOMEI_NO_WIDTH - CON_TEKIYO_BYOMEI_START_WIDTH - CON_TEKIYO_BYOMEI_TENKI_WIDTH - CON_TEKIYO_BYOMEI_TENKIDATE_WIDTH ?? 0;
            _tekiyoRowCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsTekiyo" && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? 0;
        }
        #endregion
    }
}
