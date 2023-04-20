using EmrCalculateApi.Interface;
using EmrCalculateApi.Receipt.Constants;
using EmrCalculateApi.Receipt.Models;
using EmrCalculateApi.Receipt.ViewModels;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Interfaces;
using Reporting.Accounting.Constants;
using Reporting.Accounting.DB;
using Reporting.Accounting.Model;
using Reporting.Accounting.Model.Output;
using Reporting.CommonMasters.Config;
using Reporting.Mappers.Common;
using System.Text;
using System.Linq;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;

namespace Reporting.Accounting.Service;

public class AccountingCoReportService : IAccountingCoReportService
{
    private readonly ISystemConfig _systemConfig;
    private readonly ICoAccountingFinder _finder;
    private readonly ITenantProvider _tenantProvider;
    private readonly ISystemConfigProvider _systemConfigProvider;
    private readonly IEmrLogger _emrLogger;
    private readonly IReadRseReportFileService _readRseReportFileService;


    List<(string Title, List<string> CdKbn)> totalCdKbnls =
            new List<(string, List<string>)>
            {
                ("初・再診療", new List<string>{ "A" }),
                ("入院料等", new List<string>{ "XXX" }),
                ("医学管理等", new List<string>{ "B" }),
                ("在宅医療", new List<string>{ "C" }),
                ("検査", new List<string>{ "D" }),
                ("画像診断", new List<string>{ "E" }),
                ("投薬", new List<string>{ "F" }),
                ("注射", new List<string>{ "G" }),
                ("リハビリテーション", new List<string>{ "H" }),
                ("精神科専門療法", new List<string>{ "I" }),
                ("処置", new List<string>{ "J" }),
                ("手術", new List<string>{ "K" }),
                ("麻酔", new List<string>{ "L" }),
                ("放射線治療", new List<string>{ "M" }),
                ("その他", new List<string>{ "JS" }),
                ("病理診断", new List<string>{ "N" }),
                ("労災（その他）", new List<string>{ "R" })
            };

    #region Private properties
    /// <summary>
    /// CoReport Model
    /// </summary>
    private CoAccountingModel coModel;
    /// <summary>
    /// CoReport Model リスト用
    /// </summary>
    private CoAccountingListModel coModelList;
    /// <summary>
    /// 自費種別マスタ
    /// </summary>
    private List<CoJihiSbtMstModel> jihiSbtMstModels;
    /// <summary>
    /// グループ名称マスタ
    /// </summary>
    private List<CoPtGrpNameMstModel> ptGrpNameMstModels;
    /// <summary>
    /// グループ項目マスタ
    /// </summary>
    private List<CoPtGrpItemModel> ptGrpItemModels;
    /// <summary>
    /// システム世代管理（消費税率別　診療明細記号）
    /// </summary>
    private List<CoSystemGenerationConfModel> sysGeneHanreis;
    /// <summary>
    /// 診療明細印字用データ
    /// </summary>
    private List<CoSinmeiPrintDataModel> sinmeiPrintDataModels;

    List<(string field, int charCount, int rowCount)> _sinmeiListPropertysPage1;
    List<(string field, int charCount, int rowCount)> _sinmeiListPropertysPage2;

    /// <summary>
    /// リスト用フィールドの行数
    /// </summary>
    int ListGridRowCount;

    /// <summary>
    /// 合計患者数
    /// </summary>
    int TotalPtCount;
    /// <summary>
    /// 合計点数
    /// </summary>
    int TotalTensu;
    /// <summary>
    /// 合計請求額
    /// </summary>
    int TotalSeikyuGaku;
    /// <summary>
    /// 合計入金額
    /// </summary>
    int TotalNyukinGaku;
    /// <summary>
    /// 合計未収額
    /// </summary>
    int TotalMisyu;
    /// <summary>
    /// 合計自費負担
    /// </summary>
    int TotalJihiFutan;
    /// <summary>
    /// 合計自費項目金額
    /// </summary>
    double TotalJihiKoumoku;
    /// <summary>
    /// 合計自費診療額
    /// </summary>
    int TotalJihiSinryo;
    /// <summary>
    /// 合計自費種別別金額
    /// </summary>
    List<(int jihiSbt, double kingaku)> TotalJihiKoumokuDtl;
    /// <summary>
    /// 合計医療費
    /// </summary>
    int TotalIryohi;
    /// <summary>
    /// 合計非課税対象金額
    /// </summary>
    int TotalJihiFutanFree;
    /// <summary>
    /// 合計通常税率外税対象金額
    /// </summary>
    int TotalJihiFutanOuttaxNr;
    /// <summary>
    /// 合計軽減税率外税対象金額
    /// </summary>
    int TotalJihiFutanOuttaxGen;
    /// <summary>
    /// 合計通常税率内税対象金額
    /// </summary>
    int TotalJihiFutanTaxNr;
    /// <summary>
    /// 合計軽減税率内税対象金額
    /// </summary>
    int TotalJihiFutanTaxGen;
    /// <summary>
    /// 合計外税
    /// </summary>
    int TotalSotoZei;
    /// <summary>
    /// 合計外税（通常税率分）
    /// </summary>
    int TotalSotoZeiNr;
    /// <summary>
    /// 合計外税（軽減税率分）
    /// </summary>
    int TotalSotoZeiGen;
    /// <summary>
    /// 合計内税
    /// </summary>
    int TotalUchiZei;
    /// <summary>
    /// 合計内税（通常税率分）
    /// </summary>
    int TotalUchiZeiNr;
    /// <summary>
    /// 合計内税（軽減税率分）
    /// </summary>
    int TotalUchiZeiGen;
    /// <summary>
    /// 合計消費税
    /// </summary>
    int TotalZei;
    /// <summary>
    /// 合計消費税（通常税率分）
    /// </summary>
    int TotalZeiNr;
    /// <summary>
    /// 合計消費税（軽減税率分）
    /// </summary>
    int TotalZeiGen;
    /// <summary>
    /// 合計患者負担
    /// </summary>
    int TotalPtFutan;
    /// <summary>
    /// 自費種別ごとの合計
    /// </summary>
    List<CoJihiSbtKingakuModel> TotalJihiSbtKingakus;

    DateTime _printoutDateTime;
    #endregion

    #region Init properties
    int HpId;

    /// <summary>
    /// 領収証タイプ
    ///     0:１患者１帳票タイプ
    ///     1:リストタイプ
    /// </summary>
    private int _mode;
    /// <summary>
    /// 患者ID
    /// </summary>
    private long PtId;
    /// <summary>
    /// 開始日
    /// </summary>
    private int StartDate;
    /// <summary>
    ///  終了日
    /// </summary>
    private int EndDate;
    /// <summary>
    /// 診療開始日
    /// </summary>
    private int SinStartDate;
    /// <summary>
    /// 診療終了日
    /// </summary>
    private int SinEndDate;
    /// <summary>
    /// 実日数
    /// </summary>
    private int JituNissu;
    /// <summary>
    /// 来院番号のリスト
    /// </summary>
    private List<long> RaiinNos;
    /// <summary>
    /// 保険ID
    /// </summary>
    private int HokenId;
    /// <summary>
    /// 未精算区分
    ///     0: 未精算分(RAIIN_INF.STATUS<9)は印刷しない
    ///     1: 未精算分(RAIIN_INF.STATUS<9)も印刷する
    /// </summary>
    private int MiseisanKbn;
    /// <summary>
    /// 差異区分
    ///     0: SYUNO_SEIKYU.SEIKYU_GAKU<>SYUNO_SEIKYU.NEW_SEIKYU_GAKUの分は印刷しない
    ///     1: SYUNO_SEIKYU.SEIKYU_GAKU<>SYUNO_SEIKYU.NEW_SEIKYU_GAKUの分も印刷する
    /// </summary>
    private int SaiKbn;
    /// <summary>
    /// 未収区分
    ///     0: 未収関係なく印刷
    ///     1: 未収分(SYUNO_SEIKYU.NYUKIN_KBN= 1)のみ印刷
    /// </summary>
    private int MisyuKbn;
    /// <summary>
    /// 請求区分
    ///     0: SYUNO_SEIKYU.SEIKYU_GAKU=0は印刷しない
    ///     1: SYUNO_SEIKYU.SEIKYU_GAKUに関係なく印刷
    /// </summary>
    private int SeikyuKbn;
    /// <summary>
    /// 保険指定
    ///     0: 保険の指定があればその保険のみ印刷、指定がなければどんな保険でも対象
    ///     1: PT_HOKEN_INF.HOKEN_SBT_KBN = 1 を対象とする
    /// </summary>
    private int HokenKbn;
    /// <summary>
    /// true: 保険請求ありのみ
    /// </summary>
    private bool HokenSeikyu;
    /// <summary>
    /// true: 自費請求アリのみ
    /// </summary>
    private bool JihiSeikyu;
    /// <summary>
    /// ture: 入金日ベースで取得する
    /// </summary>
    private bool NyukinBase;
    /// <summary>
    /// 発行日
    /// </summary>
    private int HakkoDay;
    /// <summary>
    /// メモ
    /// </summary>
    private string Memo;
    /// <summary>
    /// 印刷タイプ（フォームファイル名未指定時に使用）
    ///     0:領収証
    ///     1:明細
    ///     2:月間領収証
    ///     3:月間明細書
    /// </summary>
    private int PrintType;
    private int Sort;

    /// <summary>
    /// 患者条件
    /// </summary>
    private List<(long ptId, int hokenId)> PtConditions;
    /// <summary>
    /// グループ条件
    /// </summary>
    private List<(int grpId, string grpCd)> GrpConditions;
    /// <summary>
    /// Initパラメータ
    /// </summary>
    private List<CoAccountingParamModel> Params;
    /// <summary>
    /// フォームファイル名
    /// </summary>
    private string _formFileName;

    public Dictionary<string, string> _singleFieldDataResult { get; set; }
    public List<ListTextModel> _listTextModelResult { get; set; }
    public Dictionary<string, string> _systemConfigList { get; set; }
    public JavaOutputData _javaOutputData { get; set; }
    private int CurrentPage = 1;
    private bool _hasNextPage = true;

    public AccountingCoReportService(ISystemConfig systemConfig, ICoAccountingFinder finder, ITenantProvider tenantProvider, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger, IReadRseReportFileService readRseReportFileService)
    {
        _systemConfig = systemConfig;
        _finder = finder;
        _tenantProvider = tenantProvider;
        _systemConfigProvider = systemConfigProvider;
        _emrLogger = emrLogger;
        _readRseReportFileService = readRseReportFileService;
        _singleFieldDataResult = new();
        _listTextModelResult = new();
        _systemConfigList = new();
        _javaOutputData = new();
        _systemConfigList.Add("accountingUseBackwardFields", _systemConfig.AccountingUseBackwardFields().ToString());
    }
    #endregion

    public AccountingOutputModel GetAccountingReportingData(
            int hpId, long ptId, int startDate, int endDate, List<long> raiinNos, int hokenId = 0,
            int miseisanKbn = 0, int saiKbn = 0, int misyuKbn = 0, int seikyuKbn = 1, int hokenKbn = 0,
            bool hokenSeikyu = false, bool jihiSeikyu = false, bool nyukinBase = false,
            int hakkoDay = 0, string memo = "", int printType = 0, string formFileName = "")
    {
        HpId = hpId;
        _mode = PrintMode.SinglePrint;
        PtId = ptId;
        StartDate = startDate;
        EndDate = endDate;
        RaiinNos = raiinNos;
        if (RaiinNos == null)
        {
            RaiinNos = new();
        }
        HokenId = hokenId;
        MiseisanKbn = miseisanKbn;
        SaiKbn = saiKbn;
        MisyuKbn = misyuKbn;
        SeikyuKbn = seikyuKbn;
        HokenKbn = hokenKbn;
        HokenSeikyu = hokenSeikyu;
        JihiSeikyu = jihiSeikyu;
        NyukinBase = nyukinBase;
        HakkoDay = hakkoDay;
        Memo = memo;
        PrintType = printType;
        _formFileName = formFileName;
        PrintOut();
        return new AccountingOutputModel(
                   _formFileName,
                   _mode,
                   _singleFieldDataResult,
                   _listTextModelResult,
                   sinmeiPrintDataModels,
                   _systemConfigList);
    }

    public AccountingOutputModel GetAccountingReportingData(int hpId, int startDate, int endDate, List<(long ptId, int hokenId)> ptConditions, List<(int grpId, string grpCd)> grpConditions,
            int sort, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn,
            int hakkoDay, string memo, string formFileName)
    {
        HpId = hpId;
        _mode = PrintMode.ListPrint;
        StartDate = startDate;
        EndDate = endDate;
        PtConditions = ptConditions;
        GrpConditions = grpConditions;
        if (GrpConditions.Any())
        {
            PtConditions.AddRange(_finder.FindPtInf(hpId, grpConditions));
        }
        Sort = sort;
        MiseisanKbn = miseisanKbn;
        SaiKbn = saiKbn;
        MisyuKbn = misyuKbn;
        SeikyuKbn = seikyuKbn;
        HokenKbn = hokenKbn;
        HakkoDay = hakkoDay;
        Memo = memo;
        PrintType = 0;
        _formFileName = formFileName;
        PrintOut();
        return new AccountingOutputModel(
                   _formFileName,
                   _mode,
                   _singleFieldDataResult,
                   _listTextModelResult,
                   sinmeiPrintDataModels,
                   _systemConfigList);
    }

    public AccountingOutputModel GetAccountingReportingData(int hpId, List<CoAccountingParamModel> coAccountingParamModels)
    {
        HpId = hpId;
        _mode = PrintMode.MultiPrint;
        Params = coAccountingParamModels;
        PrintOut();
        return new AccountingOutputModel(
                   _formFileName,
                   _mode,
                   _singleFieldDataResult,
                   _listTextModelResult,
                   sinmeiPrintDataModels,
                   _systemConfigList);
    }

    private void PrintOut()
    {
        GetParamFromRseFile();
        if (_mode == PrintMode.SinglePrint)
        {
            PrintOutSingle();
        }
        else if (_mode == PrintMode.ListPrint)
        {
            PrintOutList();
        }
        else if (_mode == PrintMode.MultiPrint)
        {
            PrintOutMulti();
        }
    }

    private void GetParamFromRseFile()
    {
        SetFormFilePath();
        List<ObjectCalculate> fieldInputList = new();
        for (int i = 1; i <= 4; i++)
        {
            fieldInputList.Add(new ObjectCalculate($"lsSinMei_{CIUtil.ToStringIgnoreZero(i)}", (int)CalculateTypeEnum.GetFormatLendB));
            fieldInputList.Add(new ObjectCalculate($"lsSinMei_{CIUtil.ToStringIgnoreZero(i)}", (int)CalculateTypeEnum.GetListRowCount));
        }
        fieldInputList.Add(new ObjectCalculate("KanNoList", (int)CalculateTypeEnum.GetListRowCount));
        fieldInputList.Add(new ObjectCalculate("lsPtNum_1", (int)CalculateTypeEnum.GetListRowCount));
        fieldInputList.Add(new ObjectCalculate("MessageList", (int)CalculateTypeEnum.GetListRowCount));
        fieldInputList.Add(new ObjectCalculate("MessageListF", (int)CalculateTypeEnum.GetListRowCount));
        fieldInputList.Add(new ObjectCalculate("MessageList", (int)CalculateTypeEnum.GetListFormatLendB));
        fieldInputList.Add(new ObjectCalculate("MessageListF", (int)CalculateTypeEnum.GetListFormatLendB));

        CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Accounting, _formFileName, fieldInputList);
        _javaOutputData = _readRseReportFileService.ReadFileRse(data);
    }

    #region Accounting Form
    public const string ACCOUNTING_FORM_FILE_NAME = "fmAccounting_{0}.rse";
    public const string ACCOUNTINGTERM_FORM_FILE_NAME = "fmTermAccounting_{0}.rse";
    public const string ACCOUNTING_PAGE2_FORM_FILE_NAME = "fmAccounting_{0}_2.rse";
    public const string ACCOUNTINGTERM_PAGE2_FORM_FILE_NAME = "fmTermAccounting_{0}.rse";
    #endregion
    private void SetFormFilePath()
    {
        if (string.IsNullOrEmpty(_formFileName))
        {
            if (PrintType == 0)
            {
                // 領収証
                _formFileName = string.Format(ACCOUNTING_FORM_FILE_NAME, _systemConfig.AccountingFormType());
            }
            else if (PrintType == 1)
            {
                // 明細
                _formFileName = string.Format(ACCOUNTING_FORM_FILE_NAME, _systemConfig.AccountingDetailFormType());
            }
            if (PrintType == 2)
            {
                // 月間領収証
                _formFileName = string.Format(ACCOUNTINGTERM_FORM_FILE_NAME, _systemConfig.AccountingMonthFormType());
            }
            else if (PrintType == 3)
            {
                // 月間明細
                _formFileName = string.Format(ACCOUNTINGTERM_FORM_FILE_NAME, _systemConfig.AccountingDetailMonthFormType());
            }
        }
    }

    private void _addListProperty(ref List<(string field, int charCount, int rowCount)> listPropertys, int page)
    {
        var objectList = _javaOutputData.objectNames;
        var responses = _javaOutputData.responses;
        for (int i = 1; i <= 4; i++)
        {
            var getFormatLendBPage1 = responses.FirstOrDefault(item => item.typeInt == (int)CalculateTypeEnum.GetFormatLendB && item.listName == $"lsSinMei_{CIUtil.ToStringIgnoreZero(i)}")!.result;
            var getListRowCountPage1 = responses.FirstOrDefault(item => item.typeInt == (int)CalculateTypeEnum.GetListRowCount && item.listName == $"lsSinMei_{CIUtil.ToStringIgnoreZero(i)}")!.result;
            var getFormatLendBPage2 = responses.FirstOrDefault(item => item.typeInt == (int)CalculateTypeEnum.GetFormatLendB && item.listName == $"lsSinMei_{CIUtil.ToStringIgnoreZero(i)}" + "_page2")?.result ?? getFormatLendBPage1;
            var getListRowCountPage2 = responses.FirstOrDefault(item => item.typeInt == (int)CalculateTypeEnum.GetListRowCount && item.listName == $"lsSinMei_{CIUtil.ToStringIgnoreZero(i)}" + "_page2")?.result ?? getListRowCountPage1;

            if (objectList.Contains($"lsSinMei_{CIUtil.ToStringIgnoreZero(i)}") && page == 1)
            {
                listPropertys.Add(
                    (
                        CIUtil.ToStringIgnoreZero(i),
                        getFormatLendBPage1,
                        getListRowCountPage1
                    )
                );
            }
            else if (objectList.Contains($"lsSinMei_{CIUtil.ToStringIgnoreZero(i)}") && page > 1)
            {
                listPropertys.Add(
                    (
                        CIUtil.ToStringIgnoreZero(i),
                        getFormatLendBPage2,
                        getListRowCountPage2
                    )
                );
            }
        }
    }

    private void PrintOutSingle()
    {
        coModel = GetData(HpId, PtId, StartDate, EndDate);

        _sinmeiListPropertysPage1 = new List<(string field, int charCount, int rowCount)>();
        _sinmeiListPropertysPage2 = new List<(string field, int charCount, int rowCount)>();

        _addListProperty(ref _sinmeiListPropertysPage1, 1);

        if (_javaOutputData.responses.FirstOrDefault(item => item.listName == "havePage2")!.result == 1)
        {
            _addListProperty(ref _sinmeiListPropertysPage2, 2);
        }
        else
        {
            // 2ページ目用のフォームファイルが存在しない場合、
            // 2ページ目以降も1ページ目の設定を使用する
            _sinmeiListPropertysPage2.AddRange(_sinmeiListPropertysPage1);
        }

        _printoutDateTime = DateTime.Now;

        // 診療明細データを作成する
        MakeSinMeiPrintData();
        UpdateDrawForm();
    }

    private void PrintOutList()
    {
        coModelList = GetDataList(HpId, StartDate, EndDate, PtConditions, GrpConditions, Sort, MiseisanKbn, SaiKbn, MisyuKbn, SeikyuKbn, HokenKbn);
        var objectList = _javaOutputData.objectNames;
        if (objectList.Contains("lsPtNum_1"))
        {
            ListGridRowCount = GetListRowCount("lsPtNum_1");
        }
        else if (objectList.Contains("KanNoList"))
        {
            ListGridRowCount = GetListRowCount("KanNoList");
        }

        TotalPtCount = 0;
        TotalTensu = 0;
        TotalSeikyuGaku = 0;
        TotalNyukinGaku = 0;
        TotalMisyu = 0;
        TotalJihiFutan = 0;
        TotalJihiKoumoku = 0;
        TotalJihiSinryo = 0;
        TotalJihiKoumokuDtl = new();
        TotalJihiFutanFree = 0;
        TotalJihiFutanOuttaxNr = 0;
        TotalJihiFutanOuttaxGen = 0;
        TotalJihiFutanTaxNr = 0;
        TotalJihiFutanTaxGen = 0;
        TotalSotoZei = 0;
        TotalSotoZeiNr = 0;
        TotalSotoZeiGen = 0;
        TotalUchiZei = 0;
        TotalUchiZeiNr = 0;
        TotalUchiZeiGen = 0;
        TotalZei = 0;
        TotalZeiNr = 0;
        TotalZeiGen = 0;
        TotalIryohi = 0;
        TotalPtFutan = 0;
        TotalJihiSbtKingakus = new();

        _printoutDateTime = DateTime.Now;
        UpdateDrawForm();
    }

    private void PrintOutMulti()
    {
        if (Params != null && Params.Any())
        {
            foreach (CoAccountingParamModel param in Params)
            {
                _mode = PrintMode.SinglePrint;
                PtId = param.PtId;
                StartDate = param.StartDate;
                EndDate = param.EndDate;
                RaiinNos = param.RaiinNos;
                if (RaiinNos == null)
                {
                    RaiinNos = new List<long>();
                }
                HokenId = param.HokenId;
                MiseisanKbn = param.MiseisanKbn;
                SaiKbn = param.SaiKbn;
                MisyuKbn = param.MisyuKbn;
                SeikyuKbn = param.SeikyuKbn;
                HokenKbn = param.HokenKbn;
                HokenSeikyu = param.HokenSeikyu;
                JihiSeikyu = param.JihiSeikyu;
                NyukinBase = param.NyukinBase;
                HakkoDay = param.HakkoDate;
                Memo = param.Memo;
                PrintType = param.PrintType;
                _formFileName = param.FormFileName;
                coModel = GetData(HpId, PtId, StartDate, EndDate);

                if (coModel == null)
                {
                    continue;
                }

                _sinmeiListPropertysPage1 = new List<(string field, int charCount, int rowCount)>();
                _sinmeiListPropertysPage2 = new List<(string field, int charCount, int rowCount)>();

                _addListProperty(ref _sinmeiListPropertysPage1, 1);

                if (_javaOutputData.responses.FirstOrDefault(item => item.listName == "havePage2")!.result == 1)
                {
                    _addListProperty(ref _sinmeiListPropertysPage2, 2);
                }
                else
                {
                    // 2ページ目用のフォームファイルが存在しない場合、
                    // 2ページ目以降も1ページ目の設定を使用する
                    _sinmeiListPropertysPage2.AddRange(_sinmeiListPropertysPage1);
                }

                _printoutDateTime = DateTime.Now;

                // 診療明細データを作成する
                MakeSinMeiPrintData();
                UpdateDrawForm();
            }
        }
    }

    private void UpdateDrawForm()
    {
        if (_mode == PrintMode.SinglePrint)
        {
            UpdateDrawFormSingle();
        }
        else
        {
            UpdateDrawFormList();
        }
    }

    private void MakeSinMeiPrintData()
    {
        int page = 1;
        int sinmeiListIndex = 0;
        int sinmeiListRow = 0;
        string field = string.Empty;
        int sinmeiCharCount = 0;

        if (_sinmeiListPropertysPage1 != null && _sinmeiListPropertysPage1.Any())
        {
            // 1ページ目のフォームに診療明細リストが存在する場合
            sinmeiCharCount = _sinmeiListPropertysPage1[sinmeiListIndex].charCount;
        }
        else if (_sinmeiListPropertysPage2 != null && _sinmeiListPropertysPage2.Any())
        {
            // 2ページ目のフォームに診療明細リストが存在する場合
            page = 2;
            sinmeiCharCount = _sinmeiListPropertysPage2[sinmeiListIndex].charCount;
        }
        else
        {
            return;
        }

        #region sub method
        // リストに追加（先頭行に行為名）
        List<CoSinmeiPrintDataModel> _addList(string str, string AkouiName)
        {
            List<CoSinmeiPrintDataModel> addPrintOutData = new List<CoSinmeiPrintDataModel>();

            bool firstAdd = true;
            string line = str;
            int maxLength = sinmeiCharCount;

            while (line != string.Empty)
            {
                string tmp = line;
                if (CIUtil.LenB(line) > maxLength)
                {
                    // 文字列が最大幅より広い場合、カット
                    tmp = CIUtil.CiCopyStrWidth(line, 1, maxLength);
                }

                CoSinmeiPrintDataModel printOutData = new CoSinmeiPrintDataModel();
                printOutData.MeiData = tmp;
                addPrintOutData.Add(printOutData);

                if (firstAdd)
                {
                    // 初回追加時は、行為名をセット
                    firstAdd = false;
                    addPrintOutData.Last().KouiNm = AkouiName;
                }

                // 今回出力分の文字列を削除
                line = CIUtil.CiCopyStrWidth(line, CIUtil.LenB(tmp) + 1, CIUtil.LenB(line) - CIUtil.LenB(tmp));

                sinmeiListRow++;

                if (page == 1 && _sinmeiListPropertysPage1 != null)
                {
                    if (_sinmeiListPropertysPage1[sinmeiListIndex].rowCount <= sinmeiListRow)
                    {
                        sinmeiListRow = 0;
                        sinmeiListIndex++;

                        if (_sinmeiListPropertysPage1.Count <= sinmeiListIndex)
                        {
                            page = 2;
                            sinmeiListIndex = 0;

                            if (_sinmeiListPropertysPage2 != null && _sinmeiListPropertysPage2.Any())
                            {
                                field = _sinmeiListPropertysPage2[sinmeiListIndex].field;
                                sinmeiCharCount = _sinmeiListPropertysPage2[sinmeiListIndex].charCount;
                                maxLength = sinmeiCharCount;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            field = _sinmeiListPropertysPage1[sinmeiListIndex].field;
                            sinmeiCharCount = _sinmeiListPropertysPage1[sinmeiListIndex].charCount;
                            maxLength = sinmeiCharCount;
                        }
                    }
                }
                else
                {
                    if (_sinmeiListPropertysPage2[sinmeiListIndex].rowCount <= sinmeiListRow)
                    {
                        sinmeiListRow = 0;
                        sinmeiListIndex++;

                        if (_sinmeiListPropertysPage2.Count <= sinmeiListIndex)
                        {
                            sinmeiListIndex = 0;
                        }

                        field = _sinmeiListPropertysPage2[sinmeiListIndex].field;
                        sinmeiCharCount = _sinmeiListPropertysPage2[sinmeiListIndex].charCount;
                        maxLength = sinmeiCharCount;
                    }
                }
            }

            return addPrintOutData;
        }

        // コード区分チェック
        bool _diffCdKbn(string newCdKbn, string oldCdKbn)
        {
            bool ret = false;
            if (newCdKbn != null && (newCdKbn != oldCdKbn && !(newCdKbn == "N" && oldCdKbn == "D")))
            {
                ret = true;
            }
            return ret;
        }

        // 行為名
        string _getKouiName(string cdKbn)
        {
            string ret = string.Empty;

            switch (cdKbn)
            {
                case "A":
                    ret = "初再診";
                    break;
                case "B":
                    ret = "医学管理";
                    break;
                case "C":
                    ret = "在宅";
                    break;
                case "D":
                case "N":
                    ret = "検査";
                    break;
                case "E":
                    ret = "画像";
                    break;
                case "F":
                    ret = "投薬";
                    break;
                case "G":
                    ret = "注射";
                    break;
                case "H":
                    ret = "リハビリ";
                    break;
                case "I":
                    ret = "精神";
                    break;
                case "J":
                    ret = "処置";
                    break;
                case "K":
                    ret = "手術";
                    break;
                case "L":
                    ret = "麻酔";
                    break;
                case "M":
                    ret = "放射";
                    break;
                case "SO":
                    ret = "その他";
                    break;
                case "JS":
                    ret = "自費";
                    break;
            }
            return ret;
        }
        #endregion

        sinmeiPrintDataModels = new List<CoSinmeiPrintDataModel>();

        foreach (SinMeiViewModel sinmeiView in coModel.SinMeiViewModels)
        {
            string preCdKbn = string.Empty;
            bool enRp = false;
            bool dayRp = false;
            int curSinId = 0;
            long preRpNo = 0;

            if (coModel.SinMeiViewModels.Count > 1 || NyukinBase || (!NyukinBase && StartDate / 100 != EndDate / 100))
            {
                // 下記いずれかの条件に当てはまる場合は、診療月のタイトルを印字
                // 診療情報が2ヶ月分以上ある場合
                // 入金ベース
                // 請求ベースで指定期間が月をまたぐ
                if (sinmeiPrintDataModels.Count > 0)
                {
                    sinmeiPrintDataModels.Add(new CoSinmeiPrintDataModel());
                }

                sinmeiPrintDataModels.Add(new CoSinmeiPrintDataModel());
                sinmeiPrintDataModels.Last().MeiData = $"【{sinmeiView.SinKoui.First().SinYm / 100}年{sinmeiView.SinKoui.First().SinYm % 100}月 診療分】";

            }

            foreach (SinMeiDataModel sinmeiData in sinmeiView.SinMei)
            {
                if (preRpNo > 0 && sinmeiData.RpNo != preRpNo)
                {
                    enRp = false;
                    dayRp = false;
                    curSinId = 0;
                    sinmeiPrintDataModels.Add(new CoSinmeiPrintDataModel());
                }

                if (curSinId == 0)
                {
                    curSinId = sinmeiData.SinId;
                }

                string kouiName = string.Empty;
                if (_diffCdKbn(sinmeiData.CdKbn, preCdKbn))
                {
                    kouiName = _getKouiName(sinmeiData.CdKbn);
                    preCdKbn = sinmeiData.CdKbn;
                }

                StringBuilder itemName = new();
                itemName.Append(sinmeiData.ItemName);

                if (sinmeiData.SanteiKbn == 2 && !sinmeiData.IsComment && sysGeneHanreis.Any(p =>
                         p.StartDate <= EndDate && EndDate <= p.EndDate && p.Val == sinmeiData.TaxRate && p.GrpEdaNo == sinmeiData.ZeiKigoEdaNo && p.Param != null))
                {
                    itemName.Append(sysGeneHanreis.FirstOrDefault(p => p.StartDate <= EndDate && EndDate <= p.EndDate && p.Val == sinmeiData.TaxRate && p.GrpEdaNo == sinmeiData.ZeiKigoEdaNo)?.GetParam(sinmeiData.ZeiInOut));
                }

                sinmeiPrintDataModels.AddRange(_addList(itemName.ToString(), kouiName));

                if (sinmeiData.EnTenKbn == 0)
                {
                    // 点数
                    sinmeiPrintDataModels.Last().Tensu = CIUtil.ToStringIgnoreZero(sinmeiData.Ten);
                    sinmeiPrintDataModels.Last().TotalTensu = CIUtil.ToStringIgnoreZero(sinmeiData.TotalTen);
                    sinmeiPrintDataModels.Last().EnTen = "点";
                }
                else
                {
                    // 金額
                    if (sinmeiData.Kingaku != 0)
                    {
                        sinmeiPrintDataModels.Last().Tensu = @"\" + CIUtil.ToStringIgnoreZero(sinmeiData.Kingaku);
                        sinmeiPrintDataModels.Last().TotalTensu = @"\" + CIUtil.ToStringIgnoreZero(sinmeiData.TotalKingaku);
                        sinmeiPrintDataModels.Last().EnTen = "円";
                    }
                    enRp = true;
                }

                if (sinmeiData.UnitName != string.Empty)
                {
                    // 単位
                    sinmeiPrintDataModels.Last().Suuryo = CIUtil.ToStringIgnoreZero(sinmeiData.Suryo);
                    sinmeiPrintDataModels.Last().Tani = sinmeiData.UnitName;
                }

                if (curSinId == 21 && sinmeiData.ItemCd.StartsWith("6"))
                {
                    dayRp = true;
                }

                if (sinmeiData.TenKai != string.Empty)
                {
                    // 回数
                    sinmeiPrintDataModels.Last().Kaisu = CIUtil.ToStringIgnoreZero(sinmeiData.Count);
                    if (sinmeiPrintDataModels.Last().Kaisu != string.Empty)
                    {
                        if (dayRp)
                        {
                            sinmeiPrintDataModels.Last().KaisuTani = "日";
                        }
                        else
                        {
                            sinmeiPrintDataModels.Last().KaisuTani = "回";
                        }
                    }

                    if (!enRp)
                    {
                        sinmeiPrintDataModels.Last().TenKai = sinmeiData.TenKai;
                    }
                    else
                    {
                        sinmeiPrintDataModels.Last().TenKai = @"\" + sinmeiData.TenKai;
                    }
                }

                preRpNo = sinmeiData.RpNo;
            }

            // 院外処方
            if (_systemConfig.AccountingDetailIncludeOutDrug() == 1 && coModel.OdrInfModels.Any())
            {
                sinmeiPrintDataModels.Add(new CoSinmeiPrintDataModel());
                sinmeiPrintDataModels.Add(new CoSinmeiPrintDataModel());
                sinmeiPrintDataModels.Last().MeiData = "《以下、院外処方》";

                bool addBlank = false;

                foreach (CoOdrInfModel odrInf in coModel.OdrInfModels.FindAll(p =>
                    p.SinDate >= sinmeiView.SinKoui.First().SinYm * 100 + 1 &&
                    p.SinDate <= sinmeiView.SinKoui.First().SinYm * 100 + 31))
                {
                    if (addBlank)
                    {
                        sinmeiPrintDataModels.Add(new CoSinmeiPrintDataModel());
                        sinmeiPrintDataModels.Last().MeiData = new string('.', sinmeiCharCount);
                    }

                    foreach (CoOdrInfDetailModel odrDtl in coModel.OdrInfDetailModels.FindAll(p => p.RaiinNo == odrInf.RaiinNo && p.RpNo == odrInf.RpNo && p.RpEdaNo == odrInf.RpEdaNo))
                    {
                        if (_systemConfig.AccountingDetailIncludeComment() == 0 && odrDtl.IsComment)
                        {
                            // コメントを出さない
                            continue;
                        }

                        StringBuilder itemName = new();
                        itemName.Append(odrDtl.ItemName);

                        if (odrDtl.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu)
                        {
                            // 分割調剤
                            itemName.Append(CIUtil.GetBunkatuStr(odrDtl.Bunkatu, odrInf.OdrKouiKbn));
                        }

                        sinmeiPrintDataModels.AddRange(_addList(odrDtl.ItemName, string.Empty));


                        if (odrDtl.UnitName != string.Empty)
                        {
                            sinmeiPrintDataModels.Last().Suuryo = odrDtl.Suryo.ToString();
                            sinmeiPrintDataModels.Last().Tani = odrDtl.UnitName;
                        }
                    }
                    addBlank = true;
                }
            }
        }
    }

    private void UpdateDrawFormSingle()
    {
        // 下位互換
        bool backword = _systemConfig.AccountingUseBackwardFields() == 1;

        UpdateFormHeader();
        UpdateFormBody();

        #region SubMethod
        // ヘッダー 
        void UpdateFormHeader()
        {
            #region sub method
            // 記号番号
            string _getKigoBango()
            {
                string ret = string.Empty;
                if (new int[] { 1, 2 }.Contains(coModel.HokenKbn))
                {
                    // 健保
                    ret = coModel.KigoBango;
                }
                else if (new int[] { 11, 12, 13 }.Contains(coModel.HokenKbn))
                {
                    // 労災
                    ret = coModel.RousaiKofuNo;
                }
                else if (new int[] { 14 }.Contains(coModel.HokenKbn))
                {
                    // 自賠
                    ret = coModel.JibaiHokenName;
                }
                return ret;
            }


            // メッセージ（P/F）
            void _PrintMessageList(string field, int karteKbn)
            {
                if (_javaOutputData.objectNames.Contains(field))
                {
                    int charCount = GetListFormatLenB(field);
                    int rowCount = GetListRowCount(field);

                    List<string> karteInfTexts = coModel.KarteInfStringList(karteKbn);

                    List<string> messageTexts = new List<string>();

                    foreach (string karteInfText in karteInfTexts)
                    {
                        string line = karteInfText;

                        while (line != string.Empty)
                        {
                            string tmp = line;
                            if (CIUtil.LenB(line) > charCount)
                            {
                                // 文字列が最大幅より広い場合、カット
                                tmp = CIUtil.CiCopyStrWidth(line, 1, charCount);
                            }

                            messageTexts.Add(tmp);

                            // 今回出力分の文字列を削除
                            line = CIUtil.CiCopyStrWidth(line, CIUtil.LenB(tmp) + 1, CIUtil.LenB(line) - CIUtil.LenB(tmp));
                        }
                    }

                    short row = 0;
                    foreach (string messageText in messageTexts)
                    {
                        ListText(field, 0, row, messageText);
                        row++;

                        if (row >= rowCount)
                        {
                            break;
                        }
                    }
                }
            }
            #endregion

            #region sub print out

            // 医療機関関連
            #region print hpinf
            // 病院名
            void _printHpName()
            {
                string value = coModel.HpName;
                SetFieldDataRep("dfHpName_", 1, 2, value);
                if (backword)
                {
                    // 下位互換
                    SetFieldDataRep("HpName", 0, 3, value);
                }
            }
            // 病院住所
            void _printHpAddress()
            {
                // 正式
                string address = coModel.HpAddress;
                string address1 = coModel.HpAddress1;
                string address2 = coModel.HpAddress2;

                // 合体版
                SetFieldDataRep("dfHpAddress_", 1, 2, address);
                // 分割版
                SetFieldDataRep("dfHpAddress1_", 1, 2, address1);
                SetFieldDataRep("dfHpAddress2_", 1, 2, address2);

                // 下位互換
                if (backword)
                {
                    SetFieldsData("Address1", address1);
                    SetFieldsData("Address2", address2);
                    SetFieldDataRep("Address1_", 1, 3, address1);
                    SetFieldDataRep("Address2_", 1, 3, address2);
                }

            }
            // 病院郵便番号
            void _printHpPostCd()
            {
                string value = coModel.HpPostCdDsp;
                SetFieldDataRep("dfHpPostCd_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("Zip", 0, 3, value);
                }
            }
            // 開設者
            void _printKaisetuName()
            {
                string value = coModel.KaisetuName;
                SetFieldDataRep("dfKaisetuName_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("Establisher", 0, 3, value);
                }
            }
            // 病院電話番号
            void _printHpTel()
            {
                string value = coModel.HpTel;
                SetFieldDataRep("dfHpTel_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldsData("Address3", value);
                    SetFieldDataRep("Address3_", 1, 3, value);
                }
            }

            // 医療機関FAX番号
            void _printHpFaxNo()
            {
                string value = coModel.HpFaxNo;
                SetFieldDataRep("dfHpFaxNo_", 1, 2, value);
            }

            // 医療機関その他連絡先
            void _printHpOtherContacts()
            {
                string value = coModel.HpOtherContacts;
                SetFieldDataRep("dfHpOtherContacts_", 1, 2, value);
            }
            #endregion

            // 日付関連
            #region print date
            // システム日付
            void _printSysDate(int AsysDate)
            {
                SetFieldDataRepSW("dfSysDate", 1, 2, AsysDate);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("StdPrnYmd", 1, 2, CIUtil.SDateToShowWDate3(AsysDate).Ymd);
                }
            }
            // 発行日
            void _printPrintDate(int Adate)
            {
                SetFieldDataRepSW("dfPrintDate", 1, 2, Adate);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("PrnYmd", 1, 2, "発行日 " + CIUtil.SDateToShowWDate3(Adate).Ymd);
                    SetFieldDataRep("PrnYmdOnly", 1, 2, CIUtil.SDateToShowWDate3(Adate).Ymd);
                }
            }
            // 発行日（指定）
            void _printHakkoDate()
            {
                SetFieldDataRepSW("dfHakkoDate", 1, 2, HakkoDay);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("HakkoYmd", 1, 2, CIUtil.SDateToShowSDate(HakkoDay));
                    SetFieldDataRep("PrintDay", 1, 2, CIUtil.SDateToShowWDate3(HakkoDay).Ymd);
                }
            }
            // 診療日
            void _printSinDate(int Adate)
            {
                SetFieldDataRepSW("dfSinDate", 1, 2, Adate);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("SinYmd", 1, 2, "診察日 " + CIUtil.SDateToShowWDate3(Adate).Ymd);
                    SetFieldDataRep("SinYmdOnly", 1, 2, CIUtil.SDateToShowWDate3(Adate).Ymd);
                }
            }
            // 集計日
            void _printSyukeiDate()
            {
                SetFieldDataRepSW("dfSyukeiFrom", 1, 2, StartDate);
                SetFieldDataRep("dfSyukeiFromYearS_", 1, 2, StartDate / 10000);
                SetFieldDataRep("dfSyukeiFromYearW_", 1, 2, CIUtil.SDateToShowWDate3(StartDate).Gengo + " " + CIUtil.SDateToShowWDate3(StartDate).Year);
                SetFieldDataRep("dfSyukeiFromMonth_", 1, 2, StartDate % 10000 / 100);

                SetFieldDataRepSW("dfSyukeiTo", 1, 2, EndDate);
                SetFieldDataRep("dfSyukeiToYearS_", 1, 2, EndDate / 10000);
                SetFieldDataRep("dfSyukeiToYearW_", 1, 2, CIUtil.SDateToShowWDate3(EndDate).Gengo + " " + CIUtil.SDateToShowWDate3(EndDate).Year);
                SetFieldDataRep("dfSyukeiToMonth_", 1, 2, EndDate % 10000 / 100);

                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("SyukeiFrom", 1, 2, CIUtil.SDateToShowWDate3(StartDate).Ymd);
                    SetFieldDataRep("SyukeiFromYear", 1, 2, StartDate / 10000);
                    SetFieldDataRep("SyukeiFromMonth", 1, 2, StartDate % 10000 / 100);
                    SetFieldDataRep("SyukeiTo", 1, 2, CIUtil.SDateToShowWDate3(EndDate).Ymd);
                    SetFieldDataRep("SyukeiToYear", 1, 2, EndDate / 10000);
                    SetFieldDataRep("SyukeiToMonth", 1, 2, EndDate % 10000 / 100);
                }
            }
            // 診療期間
            void _printSinDateTerm()
            {
                SetFieldDataRepSW("dfSinDateFrom", 1, 2, SinStartDate);
                SetFieldDataRep("dfSinDateFromYearS_", 1, 2, SinStartDate / 10000);
                SetFieldDataRep("dfSinDateFromYearW_", 1, 2, CIUtil.SDateToShowWDate3(SinStartDate).Gengo + " " + CIUtil.SDateToShowWDate3(SinStartDate).Year);
                SetFieldDataRep("dfSinDateFromMonth_", 1, 2, SinStartDate % 10000 / 100);

                SetFieldDataRepSW("dfSinDateTo", 1, 2, SinEndDate);
                SetFieldDataRep("dfSinDateToYearS_", 1, 2, SinEndDate / 10000);
                SetFieldDataRep("dfSinDateToYearW_", 1, 2, CIUtil.SDateToShowWDate3(SinEndDate).Gengo + " " + CIUtil.SDateToShowWDate3(SinEndDate).Year);
                SetFieldDataRep("dfSinDateToMonth_", 1, 2, SinEndDate % 10000 / 100);
            }
            // 実日数
            void _printJituNissu()
            {
                SetFieldDataRep("dfJituNissu_", 1, 2, JituNissu);
            }
            // 請求月
            void _printSeikyuYm()
            {
                if (StartDate / 100 == EndDate / 100)
                {
                    SetFieldDataRep("dfSeikyuYmS_", 1, 2, $"{StartDate / 10000}年{StartDate % 10000 / 100}月");
                    CIUtil.WarekiYmd wareki = CIUtil.SDateToShowWDate3(StartDate);
                    SetFieldDataRep("dfSeikyuYmW_", 1, 2, $"{wareki.Gengo} {wareki.Year}年{wareki.Month}月");
                    // 下位互換
                    if (backword)
                    {
                        SetFieldDataRep("SeikyuYm", 1, 2, $"{StartDate / 10000}年{StartDate % 10000 / 100}月");
                    }
                }
            }
            #endregion

            // 患者情報関連
            #region print ptinf
            // 患者番号
            void _printPtNum()
            {
                long value = coModel.PtNum;
                SetFieldDataRep("dfPtNum_", 1, 2, value);
                SetFieldDataRep("bcPtNum_", 1, 2, value);
                SetFieldsData("bcPtNum1_z9", $"{coModel.PtNum:D9}");
                SetFieldsData("bcPtNum2_z9", $"{coModel.PtNum:D9}");

                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("KanID", 1, 2, coModel.PtNum);
                    SetFieldDataRep("bcKanID", 1, 2, coModel.PtNum);
                    SetFieldsData("bcKanID1_z9", $"{coModel.PtNum:D9}");
                    SetFieldsData("bcKanID2_z9", $"{coModel.PtNum:D9}");
                }
            }
            // 患者氏名
            void _printPtName()
            {
                string value = coModel.PtName;
                SetFieldDataRep("dfPtName_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("KjNM", 1, 2, value);
                }
            }
            // 患者カナ氏名
            void _printPtKanaName()
            {
                string value = coModel.PtKanaName;
                SetFieldDataRep("dfPtKanaName_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("KanaNM", 1, 2, value);
                }
            }
            // 患者性別
            void _printPtSex()
            {
                string value = coModel.PtSex;
                SetFieldDataRep("dfPtSex_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("Sex", 1, 2, value);
                }
            }
            // 患者生年月日
            void _printPtBirthday(int seikyudate)
            {
                int value = coModel.BirthDay;
                SetFieldDataRepSW("dfBirthday", 1, 2, value);
                // 年齢
                SetFieldDataRep("dfAge_", 1, 2, CIUtil.SDateToAge(value, seikyudate));
            }
            // 患者住所
            void _printPtAddress()
            {
                string address = coModel.PtAddress;
                string address1 = coModel.PtAddress1;
                string address2 = coModel.PtAddress2;

                SetFieldDataRep("dfPtAddress_", 1, 2, address);
                SetFieldDataRep("dfPtAddress1_", 1, 2, address1);
                SetFieldDataRep("dfPtAddress2_", 1, 2, address2);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("KanAddress_", 1, 2, address);
                    SetFieldDataRep("KanAddress1_", 1, 2, address1);
                    SetFieldDataRep("KanAddress2_", 1, 2, address2);
                }
            }
            // 患者郵便番号
            void _printPtPostCd()
            {
                string value = coModel.PtPostCdDsp;
                SetFieldDataRep("dfPtPostCd_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("KanZip_", 1, 2, value);
                }
            }
            #endregion

            // 保険関連
            #region print hoken
            // 保険種別
            void _printHokenSbt()
            {
                string value = coModel.HokenSbt;
                SetFieldDataRep("dfHokenSbt_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("HoKind", 1, 2, value);
                }
            }
            void _printHokenSbtAll()
            {
                string value = coModel.HokenSbtAll;
                SetFieldDataRep("dfHokenSbtAll_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("HoKindSum", 1, 2, value);
                }
            }
            // 負担率
            void _printFutanRate()
            {
                int? futanRate = coModel.FutanRate;
                if (futanRate != null)
                {
                    string value = $"{futanRate / 10}割";
                    SetFieldDataRep("dfFutanRate_", 1, 2, value);
                    // 下位互換
                    if (backword)
                    {
                        SetFieldDataRep("HoRitu", 1, 2, value);
                    }
                }
            }
            void _printFutanRateAll()
            {
                int? futanRate = coModel.FutanRateAll;
                if (futanRate != null)
                {
                    string value = $"{futanRate / 10}割";
                    SetFieldDataRep("dfFutanRateAll_", 1, 2, value);
                }
            }
            // 主保険負担率
            void _printHokenRate()
            {
                int? hokenRate = coModel.HokenRate;
                if (hokenRate != null)
                {
                    string value = $"{hokenRate / 10}割";
                    SetFieldDataRep("dfHokenRate_", 1, 2, value);
                    // 下位互換
                    if (backword)
                    {
                        SetFieldDataRep("HoFutanRitu", 1, 2, value);
                    }
                }
            }
            void _printHokenRateAll()
            {
                int? hokenRate = coModel.HokenRateAll;
                if (hokenRate != null)
                {
                    string value = $"{hokenRate / 10}割";
                    SetFieldDataRep("dfHokenRateAll_", 1, 2, value);
                    // 下位互換
                    if (backword)
                    {
                        SetFieldDataRep("HoFutanRituSum", 1, 2, value);
                    }
                }
            }

            // 保険者番号
            void _printHokensyaNo()
            {
                string value = coModel.HokensyaNo;
                SetFieldDataRep("dfHokensyaNo_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("Ho_HknNo", 0, 2, value);
                }
            }
            // 記号番号
            void _printKigoBango()
            {
                string value = _getKigoBango();
                SetFieldDataRep("dfKigoBango_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("Ho_KigouBan", 0, 2, value);
                }
            }
            // 本人家族
            void _printHonKe()
            {
                string value = coModel.HonKe;
                SetFieldDataRep("dfHonKe_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("Ho_HonKa", 0, 2, value);
                }
            }
            // 公費負担者番号・受給者番号
            void _printKohiFutansyaNo_JyukyusyaNo()
            {
                for (int i = 0; i < 4; i++)
                {
                    string futansyaNo = coModel.KohiFutansyaNo(i);
                    string jyukyusyaNo = coModel.KohiJyukyusyaNo(i);

                    // 公費負担者番号
                    SetFieldDataRep($"dfFutansyaNo_K{i + 1}_", 1, 2, futansyaNo);
                    // 公費受給者番号
                    SetFieldDataRep($"dfJyukyusyaNo_K{i + 1}_", 1, 2, jyukyusyaNo);
                    // 下位互換
                    if (backword)
                    {
                        SetFieldDataRep($"K{i + 1}_FtnNo", 0, 2, futansyaNo);
                        SetFieldDataRep($"K{i + 1}_JyuNo", 0, 2, jyukyusyaNo);
                    }
                }
            }
            #endregion

            // 自費関連
            #region print jihi
            // 自費負担額（自費診療＋自費項目）
            void _printJihiFutan()
            {
                int value = coModel.JihiFutan + coModel.JihiOuttax;
                SetFieldDataRep("dfJihiFutanGaku_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("HoGai", 1, 2, value);
                }
            }
            // 自費診療の合計金額
            void _printJihiSinryo()
            {
                double value = coModel.JihiFutan - coModel.TotalJihiKingakuAll();
                // 自費分患者負担額
                SetFieldDataRep("dfJihiSinryoGaku_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("JiKanFutan", 1, 2, value);
                }
            }
            // 自費項目の合計金額
            void _printJihiKoumoku()
            {
                SetFieldDataRep("dfJihiKoumokuGaku_", 1, 2, coModel.TotalJihiKingakuAll());
            }
            // 非課税対象金額
            void _printJihiFutanFree()
            {
                double value = coModel.JihiFutanTaxFree;
                SetFieldDataRep("dfJihiFutanFree_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("HikazeiFutan", 1, 2, value);
                }
            }
            // 通常税率対象金額
            void _printJihiFutanOutTaxNr()
            {
                double value = coModel.JihiFutanOutTaxNr;
                SetFieldDataRep("dfJihiFutanZeiNr_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("KazeiFutan", 1, 2, value);
                }
            }
            // 軽減税率対象金額
            void _printJihiFutanOutTaxGen()
            {
                double value = coModel.JihiFutanOutTaxGen;
                SetFieldDataRep("dfJihiFutanZeiGen_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("KeigenFutan", 1, 2, value);
                }
            }
            // 通常税率内税対象金額
            void _printJihiFutanTaxNr()
            {
                double value = coModel.JihiFutanTaxNr;
                SetFieldDataRep("dfJihiFutanUchiNr_", 1, 2, value);
            }
            // 軽減税率内税対象金額
            void _printJihiFutanTaxGen()
            {
                double value = coModel.JihiFutanTaxGen;
                SetFieldDataRep("dfJihiFutanUchiGen_", 1, 2, value);
            }
            // 外税
            void _printOutTax()
            {
                int value = coModel.JihiOuttax;
                SetFieldDataRep("dfSotoZei_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("Zei", 1, 2, value);
                }
            }
            // 外税（通常税率分）
            void _printOutTaxNr()
            {
                int value = coModel.JihiOuttaxNr;
                SetFieldDataRep("dfSotoZeiNr_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("KazeiZei", 1, 2, value);
                }
            }
            // 外税（軽減税率分）
            void _printOutTaxGen()
            {
                int value = coModel.JihiOuttaxGen;
                SetFieldDataRep("dfSotoZeiGen_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("KeigenZei", 1, 2, value);
                }
            }
            // 内税
            void _printInTax()
            {
                SetFieldDataRep("dfUchiZei_", 1, 2, coModel.JihiTax);
            }

            // 内税（通常税率分）                
            void _printInTaxNr()
            {
                SetFieldDataRep("dfUchiZeiNr_", 1, 2, coModel.JihiTaxNr);
            }
            // 内税（軽減税率分）
            void _printInTaxGen()
            {
                SetFieldDataRep("dfUchiZeiGen_", 1, 2, coModel.JihiTaxGen);
            }
            // 消費税
            void _printTax()
            {
                SetFieldDataRep("dfZei_", 1, 2, coModel.JihiOuttax + coModel.JihiTax);
            }
            // 消費税（通常税率分）
            void _printTaxNr()
            {
                SetFieldDataRep("dfZeiNr_", 1, 2, coModel.JihiOuttaxNr + coModel.JihiTaxNr);
            }
            // 消費税（軽減税率分）
            void _printTaxGen()
            {
                SetFieldDataRep("dfZeiGen_", 1, 2, coModel.JihiOuttaxGen + coModel.JihiTaxGen);
            }
            // 税率別の消費税関連情報印字
            void _printTaxByRate()
            {
                foreach (TaxSum taxsum in coModel.TaxSums)
                {
                    SetFieldDataRep($"dfJihiFutanZei{taxsum.Rate}_", 1, 2, taxsum.OuttaxFutan);
                    SetFieldDataRep($"dfJihiFutanUchi{taxsum.Rate}_", 1, 2, taxsum.TaxFutan);
                    SetFieldDataRep($"dfSotoZei{taxsum.Rate}_", 1, 2, taxsum.OuttaxZei);
                    SetFieldDataRep($"dfUchiZei{taxsum.Rate}_", 1, 2, taxsum.TaxZei);
                    SetFieldDataRep($"dfZei{taxsum.Rate}_", 1, 2, taxsum.OuttaxZei + taxsum.TaxZei);
                }
            }
            // 凡例
            void _printTaxHanrei()
            {
                List<CoSystemGenerationConfModel> hanreis =
                    sysGeneHanreis.FindAll(p => p.StartDate <= EndDate && EndDate <= p.EndDate && p.Param != null).OrderBy(p => p.Val).ThenBy(p => p.GrpEdaNo).ToList();

                StringBuilder hanreiString = new();

                foreach (CoSystemGenerationConfModel hanrei in hanreis)
                {
                    if (!string.IsNullOrEmpty(hanrei.Param))
                    {
                        for (int i = 0; i <= 1; i++)
                        {
                            string param = hanrei.GetParam(i);

                            if (!string.IsNullOrEmpty(param))
                            {

                                if (hanreiString.Length > 0)
                                {
                                    hanreiString.Append("、");
                                }
                                if (hanrei.GrpEdaNo == 2)
                                {
                                    hanreiString.Append($"{param} は 非課税対象");
                                }
                                else
                                {
                                    string rate = $"{hanrei.Val}%" + (hanrei.GrpEdaNo == 1 ? "(減)" : string.Empty) + (i == 1 ? "(内)" : string.Empty);
                                    hanreiString.Append($"{param} は {rate} 対象");
                                }
                            }
                        }
                    }
                }
                SetFieldDataRep("dfJihiHanrei_", 1, 2, hanreiString);
            }
            #endregion

            // 点数・金額関連
            #region print tensu kingaku
            // 患者負担額
            void _printPtFutan()
            {
                int value = coModel.PtFutan + coModel.AdjustRound;
                SetFieldDataRep("dfPtFutan_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("KanFutan", 1, 2, value);
                    SetFieldDataRep("KanRoFutan", 1, 2, value);
                }
            }
            // 調整額
            void _printAdjust()
            {
                int value = coModel.AdjustFutan + coModel.TotalNyukinAjust;
                SetFieldDataRep("dfAdjust_", 1, 2, value);
            }
            // 請求額（調整前）
            void _printSeikyuGaku()
            {
                int value = coModel.TotalPtFutan - coModel.AdjustFutan;
                SetFieldDataRep("dfSeikyuGaku_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("Seikyu", 1, 2, value);
                }
            }
            // 請求額（調整前／免除を含む）
            void _printSeikyuGakuIncludeMenjyo()
            {
                int value = coModel.TotalPtFutanIncludeMenjyo - coModel.AdjustFutan;
                SetFieldDataRep("dfSeikyuGakuIncludeMenjyo_", 1, 2, value);
            }
            // 請求額（調整額を引いたもの）
            void _printSeikyuGakuAdjust()
            {
                int value = coModel.TotalPtFutan - coModel.TotalNyukinAjust;
                SetFieldDataRep("dfSeikyuGakuAdjust_", 1, 2, value);
            }
            // 前回入金額
            void _printNyukinGakuZenkai()
            {
                int value = coModel.ExceptLastNyukin;
                SetFieldDataRep("dfNyukinGaku_Zenkai_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("ZenRyoSyu", 1, 2, value);
                }
            }
            // 今回入金額
            void _printNyukinGakuKonkai()
            {
                int value = coModel.LastNyukin;
                SetFieldDataRep("dfNyukinGaku_Konkai_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("KonRyoSyu", 1, 2, value);
                }
            }
            // 合計入金額
            void _printNyukinGakuTotal()
            {
                int value = coModel.TotalNyukin;
                SetFieldDataRep("dfNyukinGaku_Total_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("GouRyoSyu", 1, 2, value);
                }
            }
            void _printSiharaiHouhou()
            {
                SetFieldDataRep("dfSiharaiHouhou_", 1, 2, coModel.PayMethod);
            }
            // 請求バーコード
            void _printSeikyuBarCode()
            {
                string buf;
                if (coModel.ExceptLastNyukin == 0)
                {
                    // 前回入金額なし
                    buf = "0001";
                    int nbuf = coModel.PtFutan + coModel.AdjustRound;

                    if (nbuf != 0)
                    {
                        // 患者負担あり
                        if (nbuf < 0)
                        {
                            buf = "1001";
                            SetFieldDataRep("dfHoSeikyu", 1, 2, "返金");
                        }

                        buf = $"02{buf}{Math.Abs(nbuf):D6}";
                        buf += CIUtil.CalcChkDgtJAN13(buf);
                        SetFieldDataRep("bcHoSeikyu", 1, 2, buf);
                        SetFieldDataRep("bcHoSeikyuZ", 1, 2, buf);
                    }
                    else if (coModel.JihiFutan + coModel.JihiOuttax == 0)
                    {
                        buf = $"02{buf}{Math.Abs(nbuf):D6}";
                        buf += CIUtil.CalcChkDgtJAN13(buf);
                        SetFieldDataRep("bcHoSeikyuZ", 1, 2, buf);
                    }

                    buf = "0002";
                    nbuf = coModel.JihiFutan + coModel.JihiOuttax;
                    if (nbuf != 0)
                    {
                        if (nbuf < 0)
                        {
                            buf = "1002";
                            SetFieldDataRep("dfHoGaiSeikyu", 1, 2, "返金");
                        }
                        buf = $"02{buf}{Math.Abs(nbuf):D6}";
                        buf += CIUtil.CalcChkDgtJAN13(buf);
                        SetFieldDataRep("bcHoGaiSeikyu", 1, 2, buf);
                    }
                }
                else
                {
                    //前回領収額がある場合は差額
                    buf = "0003";
                    int nbuf = coModel.TotalPtFutan - coModel.AdjustFutan - coModel.ExceptLastNyukin;
                    if (nbuf != 0)
                    {
                        if (nbuf < 0)
                        {
                            buf = "1003";
                            SetFieldDataRep("dfMiSeikyu", 1, 2, "返金");
                        }
                        buf = $"02{buf}{Math.Abs(nbuf):D6}";
                        buf += CIUtil.CalcChkDgtJAN13(buf);
                        SetFieldDataRep("bcMiSeikyu", 1, 2, buf);
                    }
                }

                //今回領収額
                buf = "0004";
                if (coModel.LastNyukin < 0)
                {
                    buf = "1004";
                    SetFieldDataRep("dfKonRyosyu", 1, 2, "返金");
                }
                buf = $"02{buf}{Math.Abs(coModel.LastNyukin):D6}";
                buf += CIUtil.CalcChkDgtJAN13(buf);
                SetFieldDataRep("bcKonRyosyu", 1, 2, buf);
            }
            // 未収額
            void _printMisyu()
            {
                int value = coModel.Misyu;
                SetFieldDataRep("dfMisyu_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("Misyu", 1, 2, value);
                }
            }
            // 患者未収額
            void _printPtMisyu()
            {
                int value = coModel.PtMisyu;
                SetFieldDataRep("dfPtMisyu_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("KanMisyu", 1, 2, value);
                }
            }
            // 返金額
            void _printHenkin()
            {
                int value = coModel.Henkin;
                SetFieldDataRep("dfHenkin_", 1, 2, value);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("Henkin", 1, 2, value);
                }
            }
            // 行為別点数内訳と総点数
            void _printKouiTensu()
            {
                List<string> totalTens = new List<string>();

                // 総点数
                double allTatalTen = 0;

                for (int j = 0; j < totalCdKbnls.Count; j++)
                {
                    double totalTensu = coModel.TotalTen(totalCdKbnls[j].CdKbn);
                    string totalTen = CIUtil.ToStringIgnoreZero(totalTensu);
                    totalTens.Add(totalTen);

                    SetFieldDataRep($"dfTitle{j + 1:D2}_", 1, 2, totalCdKbnls[j].Title);
                    SetFieldDataRep($"dfData{j + 1:D2}_", 1, 2, totalTen);

                    if (!totalCdKbnls[j].CdKbn.Contains("JS"))
                    {
                        allTatalTen += coModel.TotalSinTen(totalCdKbnls[j].CdKbn);
                    }
                }

                // 検査+病理の点数
                SetFieldDataRep("dfTitle052_", 1, 2, "検査");
                SetFieldDataRep("dfData052_", 1, 2, CIUtil.ToStringIgnoreZero(coModel.TotalTen(new List<string> { "D", "N" })));

                // 下位互換
                if (backword)
                {
                    for (int i = 1; i <= 2; i++)
                    {
                        for (int j = 0; j < totalCdKbnls.Count; j++)
                        {
                            if (j == 4)
                            {
                                SetFieldsData($"Title14_{i}{j + 1:D2}", "検査");
                                SetFieldsData($"Data14_{i}{j + 1:D2}", CIUtil.ToStringIgnoreZero(coModel.TotalTen(new List<string> { "D", "N" })));
                            }
                            else
                            {
                                SetFieldsData($"Title14_{i}{j + 1:D2}", totalCdKbnls[j].Title);
                                SetFieldsData($"Data14_{i}{j + 1:D2}", totalTens[j]);
                            }
                        }

                        SetFieldsData($"Title14_{i}05_2", "検査");
                        SetFieldsData($"Data14_{i}05_2", CIUtil.ToStringIgnoreZero(coModel.TotalTen(new List<string> { "D" })));
                    }
                }

                // 総点数
                SetFieldDataRep("dfSeikyuTensu_", 1, 2, allTatalTen);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("TenGokei", 1, 2, allTatalTen);
                }
            }
            //自費種別別金額内訳
            void _printJihiSbtKingaku()
            {
                // 自費種別別金額
                List<(string Title, int JihiSbt, string totalJihiKingakuDsp, double kingaku)> jihiSbtls = new List<(string, int, string, double)>();

                foreach (CoJihiSbtMstModel jihiSbtMst in jihiSbtMstModels)
                {
                    double kingaku = coModel.TotalJihiKingaku(jihiSbtMst.JihiSbt);
                    jihiSbtls.Add((jihiSbtMst.Name, jihiSbtMst.JihiSbt, CIUtil.ToStringIgnoreZero(kingaku), kingaku));
                }

                for (int j = 0; j < jihiSbtls.Count; j++)
                {
                    SetFieldDataRep($"dfJihiTitle{jihiSbtls[j].JihiSbt:D2}_", 1, 2, jihiSbtls[j].Title);
                    SetFieldDataRep($"dfJihiData{jihiSbtls[j].JihiSbt:D2}_", 1, 2, jihiSbtls[j].totalJihiKingakuDsp);
                }

                // 下位互換
                if (backword)
                {
                    for (int i = 1; i <= 2; i++)
                    {
                        for (int j = 0; j < jihiSbtls.Count; j++)
                        {
                            SetFieldsData($"JihiTitle{i}{j + 1}", jihiSbtls[j].Title);
                            SetFieldsData($"JihiData{i}{j + 1}", jihiSbtls[j].totalJihiKingakuDsp);
                        }
                    }
                }

                foreach (string ObjectName in _javaOutputData.objectNames.FindAll(p => p.StartsWith("dfJihiSbt_")))
                {
                    double kingaku = 0;

                    // 分割
                    string[] jihiSbtCds = ObjectName.Split('_');

                    foreach (string jihiSbtCd in jihiSbtCds)
                    {
                        if (CIUtil.StrToIntDef(jihiSbtCd, 0) > 0 && jihiSbtls.Any(p => p.JihiSbt == CIUtil.StrToIntDef(jihiSbtCd, 0)))
                        {
                            kingaku += jihiSbtls.Where(p => p.JihiSbt == CIUtil.StrToIntDef(jihiSbtCd, 0)).Sum(p => p.kingaku);
                        }
                    }

                    SetFieldsData(ObjectName, kingaku.ToString());
                }
            }
            #endregion

            // メッセージ関連
            #region print message
            // メッセージP
            void _printMessageP()
            {
                // メッセージリストP
                string message = coModel.KarteInfText(4);
                SetFieldDataRep("txMessageP_", 1, 2, message);

                // 下位互換
                if (backword)
                {
                    _PrintMessageList("MessageList", 4);
                    SetFieldsData("MessageList", message);
                }

            }
            //メッセージF
            void _printMessageF()
            {
                // メッセージリストF
                string message = coModel.KarteInfText(5);
                SetFieldDataRep("txMessageF_", 1, 2, message);

                // 下位互換
                if (backword)
                {
                    _PrintMessageList("MessageListF", 5);
                    SetFieldsData("MessageListF", message);
                }

            }
            #endregion

            // 病名関連
            #region print byomei
            // 病名リスト
            void _printPtByomei()
            {
                short listRow = 0;
                foreach (CoPtByomeiModel ptByomei in coModel.PtByomeiModels)
                {
                    #region sub method
                    string _getUpdateMark()
                    {
                        string mark = string.Empty;
                        if (betweenStartEnd(ptByomei.UpdateDate))
                        {
                            mark = "(*)";
                        }
                        return mark;
                    }
                    #endregion

                    string byomei = ptByomei.Byomei + _getUpdateMark();
                    int startDate = ptByomei.StartDate;
                    string tenki = ptByomei.Tenki;
                    int tenkiDate = ptByomei.TenkiDate;

                    // 病名
                    SetListDataRep("lsByomei_", 1, 2, 0, listRow, byomei);
                    // 病名開始日
                    SetListDataRepSW("lsByomeiStartDate", 1, 2, 0, listRow, startDate);
                    // 病名転帰区分
                    SetListDataRep("lsByomeiTenkiKbn_", 1, 2, 0, listRow, tenki);
                    // 病名転帰日
                    SetListDataRepSW("lsByomeiTenkiDate", 1, 2, 0, listRow, tenkiDate);

                    // 下位互換
                    if (backword)
                    {
                        // 病名
                        SetListDataRep("ByoMeiList", 0, 2, 0, listRow, byomei);
                        // 病名開始日
                        SetListDataRep("ByoYmdList", 0, 2, 0, listRow, CIUtil.SDateToShowWDate3(startDate).Ymd);
                        // 病名転帰区分
                        SetListDataRep("ByoTenKubunList", 0, 2, 0, listRow, tenki);
                        // 病名転帰日
                        SetListDataRep("ByoTenYmdList", 0, 2, 0, listRow, CIUtil.SDateToShowWDate3(tenkiDate).Ymd);
                    }
                    listRow++;
                }
            }
            // 病名リスト（当日更新分）
            void _printPtByomeiToday()
            {
                short listRow = 0;
                foreach (CoPtByomeiModel ptByomei in coModel.PtByomeiModels.FindAll(p => betweenStartEnd(p.CreateDate) || betweenStartEnd(p.UpdateDate)))
                {
                    string _getUpdateMark()
                    {
                        string mark = string.Empty;
                        if (betweenStartEnd(ptByomei.UpdateDate))
                        {
                            mark = "(*)";
                        }
                        return mark;
                    }

                    string byomei = ptByomei.Byomei + _getUpdateMark();
                    int startDate = ptByomei.StartDate;
                    string tenki = ptByomei.Tenki;
                    int tenkiDate = ptByomei.TenkiDate;

                    // 病名
                    SetListDataRep("lsByomeiToday_", 1, 2, 0, listRow, byomei);
                    // 病名開始日
                    SetListDataRepSW("lsByomeiTodayStartDate", 1, 2, 0, listRow, startDate);
                    // 病名転帰区分
                    SetListDataRep("lsByomeiTodayTenkiKbn_", 1, 2, 0, listRow, tenki);
                    // 病名転帰日
                    SetListDataRepSW("lsByomeiTodayTenkiDate_", 1, 2, 0, listRow, tenkiDate);

                    // 下位互換
                    if (backword)
                    {
                        // 病名
                        SetListDataRep("ByoMeiTodayList", 1, 2, 0, listRow, byomei);
                        // 病名開始日
                        SetListDataRep("ByoYmdTodayList", 1, 2, 0, listRow, CIUtil.SDateToShowWDate3(startDate).Ymd);
                        // 病名転帰区分
                        SetListDataRep("ByoTenKubunTodayList", 1, 2, 0, listRow, tenki);
                        // 病名転帰日
                        SetListDataRep("ByoTenYmdTodayList", 1, 2, 0, listRow, CIUtil.SDateToShowWDate3(tenkiDate).Ymd);
                    }
                    listRow++;
                }
            }
            #endregion

            // リスト
            #region print list
            // 請求日リスト
            void _printSeikyuDateList(short listRow, CoKaikeiInfDailyModel kaikeidaily)
            {
                int value = kaikeidaily.SinDate;
                SetListDataRepSW("lsSeikyuDate", 1, 2, 0, listRow, value);
                // 下位互換
                if (backword)
                {
                    SetListDataRep("SinYmdList", 1, 2, 0, listRow, CIUtil.SDateToShowWDate3(value).Ymd);
                }
            }
            // 請求額リスト（調整前）
            void _printSeikyuGakuList(short listRow, CoKaikeiInfDailyModel kaikeidaily)
            {
                int value = kaikeidaily.SeikyuGaku - kaikeidaily.AdjustFutan;
                SetListDataRep("lsSeikyuGaku_", 1, 2, 0, listRow, value);
                // 下位互換
                if (backword)
                {
                    SetListDataRep("SeikyuList", 1, 2, 0, listRow, value);
                }
            }
            // 請求額リスト（調整後）
            void _printSeikyuGakuAdjustList(short listRow, CoKaikeiInfDailyModel kaikeidaily)
            {
                int value = kaikeidaily.SeikyuGaku - kaikeidaily.TotalNyukinAdjust;
                SetListDataRep("lsSeikyuGakuAdjust_", 1, 2, 0, listRow, value);
            }
            // 今回入金額リスト
            void _printKonkaiNyukinGakuList(short listRow, CoKaikeiInfDailyModel kaikeidaily)
            {
                int value = kaikeidaily.NyukinKonkai;
                SetListDataRep("lsNyukinGaku_Konkai_", 1, 2, 0, listRow, value);
                // 下位互換
                if (backword)
                {
                    SetListDataRep("KonRyoSyuList", 1, 2, 0, listRow, value);
                }
            }
            // 前回入金額
            void _printZenkaiNyukinGakuList(short listRow, CoKaikeiInfDailyModel kaikeidaily)
            {
                int value = kaikeidaily.NyukinZenkai;
                SetListDataRep("lsNyukinGaku_Zenkai_", 1, 2, 0, listRow, value);
                // 下位互換
                if (backword)
                {
                    SetListDataRep("ZenRyoSyuList", 1, 2, 0, listRow, value);
                }
            }
            // 合計入金額
            void _printTotalNyukinGakuList(short listRow, CoKaikeiInfDailyModel kaikeidaily)
            {
                int value = kaikeidaily.NyukinZenkai + kaikeidaily.NyukinKonkai;
                SetListDataRep("lsNyukinGaku_Total_", 1, 2, 0, listRow, value);
            }
            // 患者負担額リスト
            void _printPtFutanList(short listRow, CoKaikeiInfDailyModel kaikeidaily)
            {
                int value = kaikeidaily.PtFutan + kaikeidaily.AdjustRound;
                SetListDataRep("lsPtFutan_", 1, 2, 0, listRow, value);
                // 下位互換
                if (backword)
                {
                    SetListDataRep("KanFutanList", 1, 2, 0, listRow, value);
                }
            }
            // 調整額リスト
            void _printAdjustList(short listRow, CoKaikeiInfDailyModel kaikeidaily)
            {
                int value = kaikeidaily.AdjustFutan + kaikeidaily.TotalNyukinAdjust;
                SetListDataRep("lsAdjust_", 1, 2, 0, listRow, value);
            }
            // 自費負担リスト
            void _printJihiFutanList(short listRow, CoKaikeiInfDailyModel kaikeidaily)
            {
                int value = kaikeidaily.JihiFutan + kaikeidaily.JihiOuttax;
                SetListDataRep("lsJihiFutan_", 1, 2, 0, listRow, value);
                // 下位互換
                if (backword)
                {
                    SetListDataRep("JihiSeikyuList", 1, 2, 0, listRow, value);
                }
            }
            // 自費外税リスト
            void _printJihiOuttaxList(short listRow, CoKaikeiInfDailyModel kaikeidaily)
            {
                SetListDataRep("lsSotoZei_", 1, 2, 0, listRow, kaikeidaily.JihiOuttax);
            }
            // 自費内税リスト
            void _printJihiIntaxList(short listRow, CoKaikeiInfDailyModel kaikeidaily)
            {
                SetListDataRep("lsUchiZei_", 1, 2, 0, listRow, kaikeidaily.JihiTax);
            }
            // 自費消費税リスト
            void _printJihiTaxList(short listRow, CoKaikeiInfDailyModel kaikeidaily)
            {
                SetListDataRep("lsZei_", 1, 2, 0, listRow, kaikeidaily.JihiOuttax + kaikeidaily.JihiTax);
            }
            // 自費税率別リスト
            void _printJihiListByRate(short listRow, CoKaikeiInfDailyModel kaikeidaily)
            {
                SetListDataRep("lsJihiFutanZeiFree_", 1, 2, 0, listRow, kaikeidaily.JihiFutanTaxFree);

                foreach (TaxSum taxSum in kaikeidaily.TaxSums)
                {
                    SetListDataRep($"lsJihiFutanZei{taxSum.Rate}_", 1, 2, 0, listRow, taxSum.OuttaxFutan);
                    SetListDataRep($"lsJihiFutanUchi{taxSum.Rate}_", 1, 2, 0, listRow, taxSum.TaxFutan);
                    SetListDataRep($"lsJihiSotoZei{taxSum.Rate}_", 1, 2, 0, listRow, taxSum.OuttaxZei);
                    SetListDataRep($"lsJihiUchiZei{taxSum.Rate}_", 1, 2, 0, listRow, taxSum.TaxZei);
                    SetListDataRep($"lsJihiZei{taxSum.Rate}_", 1, 2, 0, listRow, taxSum.OuttaxZei + taxSum.TaxZei);
                }
            }
            // 診療点数リスト
            void _printTensuList(short listRow, CoKaikeiInfDailyModel kaikeidaily)
            {
                int value = kaikeidaily.Tensu;
                SetListDataRep("lsTensu_", 1, 2, 0, listRow, value);
                // 下位互換
                if (backword)
                {
                    SetListDataRep("TenGokeiList", 1, 2, 0, listRow, value);
                }
            }
            // 未収金リスト
            void _printMisyuList(short listRow, CoKaikeiInfDailyModel kaikeidaily)
            {
                int value = kaikeidaily.Misyu;
                SetListDataRep("lsMisyu_", 1, 2, 0, listRow, value);
                // 下位互換
                if (backword)
                {
                    SetListDataRep("MisyuList", 1, 2, 0, listRow, value);
                }
            }
            void _printPtMisyuList(short listRow, CoKaikeiInfDailyModel kaikeidaily)
            {
                int value = kaikeidaily.PtMisyu;
                SetListDataRep("lsPtMisyu_", 1, 2, 0, listRow, value);
            }
            //リスト
            void _printListData()
            {
                short listRow = 0;
                foreach (CoKaikeiInfDailyModel kaikeidaily in coModel.KaikeiInfDailyModels)
                {
                    // 請求日リスト
                    _printSeikyuDateList(listRow, kaikeidaily);
                    // 請求額リスト
                    _printSeikyuGakuList(listRow, kaikeidaily);
                    _printSeikyuGakuAdjustList(listRow, kaikeidaily);
                    // 今回入金額リスト
                    _printKonkaiNyukinGakuList(listRow, kaikeidaily);
                    // 前回入金額
                    _printZenkaiNyukinGakuList(listRow, kaikeidaily);
                    // 合計入金額リスト
                    _printTotalNyukinGakuList(listRow, kaikeidaily);
                    // 患者負担額リスト
                    _printPtFutanList(listRow, kaikeidaily);
                    // 調整額リスト
                    _printAdjustList(listRow, kaikeidaily);
                    // 自費負担リスト
                    _printJihiFutanList(listRow, kaikeidaily);
                    _printJihiOuttaxList(listRow, kaikeidaily);
                    _printJihiIntaxList(listRow, kaikeidaily);
                    _printJihiTaxList(listRow, kaikeidaily);
                    _printJihiListByRate(listRow, kaikeidaily);
                    // 診療点数リスト
                    _printTensuList(listRow, kaikeidaily);
                    // 未収金リスト
                    _printMisyuList(listRow, kaikeidaily);
                    // 患者未収金リスト
                    _printPtMisyuList(listRow, kaikeidaily);

                    listRow++;
                }
            }
            #endregion

            // 月別請求点数・患者負担・入金額
            void _printMonthly()
            {
                foreach (CoKaikeiInfMonthlyModel monthly in coModel.KaikeiInfMonthlyModels)
                {
                    SetFieldsData($"dfMonthTen{monthly.SinYm % 100:D2}", monthly.Tensu);
                    SetFieldsData($"dfMonthEn{monthly.SinYm % 100:D2}", monthly.PtFutan + monthly.AdjustRound);
                    SetFieldsData($"dfMonthAdjust{monthly.SinYm % 100:D2}", monthly.AdjustFutan);
                    SetFieldsData($"dfMonthJihi{monthly.SinYm % 100:D2}", monthly.JihiFutan);
                    SetFieldsData($"dfMonthTotalSeikyu{monthly.SinYm % 100:D2}", monthly.SeikyuGaku);
                }

                foreach (CoNyukinInfMonthlyModel monthly in coModel.NyukinInfMonthlyModels)
                {
                    SetFieldsData($"dfMonth{monthly.NyukinYm % 100:D2}", monthly.NyukinGaku);
                    SetFieldsData($"dfMonthNyukinAdjust{monthly.NyukinYm % 100:D2}", monthly.TotalAdjust);
                }
            }
            // メモ
            void _printMemo()
            {
                SetFieldDataRep("txMemo_", 1, 2, Memo);
                // 下位互換
                if (backword)
                {
                    SetFieldDataRep("Memo_", 1, 2, Memo);
                }
            }
            // 患者メモ
            void _printPtMemo()
            {
                SetFieldDataRep("txPtMemo_", 1, 2, coModel.PtMemoModel.Memo);

                string[] del = { "\r\n", "\r", "\n" };
                string[] splitPtMemos = coModel.PtMemoModel.Memo.Split(del, StringSplitOptions.None);

                for (int i = 0; i < 5; i++)
                {
                    if (i >= splitPtMemos.Count())
                    {
                        break;
                    }

                    SetFieldDataRep($"txPtMemo{i + 1}_", 1, 2, splitPtMemos[i]);
                    // 下位互換
                    if (backword)
                    {
                        SetFieldDataRep($"KanMemo{i + 1}_", 1, 2, splitPtMemos[i]);
                    }
                }
            }
            // 集計期間タイトル
            void _printSyukeiTitle()
            {
                string title = "期間";

                if (CIUtil.MonthsAfter(StartDate, 12) > EndDate && CIUtil.MonthsAfter(StartDate, 11) < EndDate)
                {
                    title = "年間";
                }
                else if (CIUtil.MonthsAfter(StartDate, 1) > EndDate)
                {
                    title = "月間";
                }

                SetFieldsData("dfSyukeiTitle", title);
            }
            // 予約
            void _printYoyaku()
            {
                short row = 0;
                foreach (CoRaiinInfModel raiinInf in coModel.RaiinInfModels)
                {
                    string yoyakuDateTimeS = raiinInf.YoyakuDateTimeS;
                    string yoyakuDateTimeW = raiinInf.YoyakuDateTimeW;
                    string yoyakuDateTimeSW = raiinInf.YoyakuDateTimeSW;
                    string yoyakuDateTimeWW = raiinInf.YoyakuDateTimeWW;
                    string yoyakuComment = raiinInf.YoyakuComment;
                    string yoyakusyaName = raiinInf.YoyakusyaName;
                    string uketukeKbn = raiinInf.UketukeKbnName;

                    SetListDataRep("lsYoyakuDateTimeS_", 1, 2, 0, row, yoyakuDateTimeS);
                    SetListDataRep("lsYoyakuDateTimeW_", 1, 2, 0, row, yoyakuDateTimeW);
                    SetListDataRep("lsYoyakuDateTimeSW_", 1, 2, 0, row, yoyakuDateTimeSW);
                    SetListDataRep("lsYoyakuDateTimeWW_", 1, 2, 0, row, yoyakuDateTimeWW);

                    SetListDataRep("lsYoyakuComment_", 1, 2, 0, row, yoyakuComment);
                    SetListDataRep("lsYoyakusyaName_", 1, 2, 0, row, yoyakusyaName);
                    SetListDataRep("lsUketukeKbn_", 1, 2, 0, row, uketukeKbn);

                    if (backword)
                    {
                        // 下位互換
                        SetFieldsData($"Rsv{row + 1}", yoyakuDateTimeW);
                        SetFieldsData($"RevCmt{row + 1}", yoyakuComment);
                        SetFieldsData($"RevDoc{row + 1}", yoyakusyaName);
                        SetFieldsData($"RevUke{row + 1}", uketukeKbn);
                        SetListDataRep("RevList", 0, 3, 0, row, yoyakuDateTimeW);
                        SetListDataRep("RevListS", 0, 3, 0, row, yoyakuDateTimeS);
                        SetListDataRep("RevCmtList", 0, 3, 0, row, yoyakuComment);
                        SetListDataRep("RevDocList", 0, 3, 0, row, yoyakusyaName);
                        SetListDataRep("RevUkeList", 0, 3, 0, row, uketukeKbn);
                        SetListsData("RevList_w", 0, row, yoyakuDateTimeWW);
                        SetListsData("RevListS_w", 0, row, yoyakuDateTimeSW);
                    }

                    row++;
                }
            }
            // 領収証定型文
            void _printTeikeibun()
            {
                if (_systemConfig.AccountingTeikeibunPrint() == 1)
                {
                    SetFieldDataRep("dfTeikeibun1_", 1, 2, _systemConfig.AccountingTeikeibun1());
                    SetFieldDataRep("dfTeikeibun2_", 1, 2, _systemConfig.AccountingTeikeibun2());
                    SetFieldDataRep("dfTeikeibun3_", 1, 2, _systemConfig.AccountingTeikeibun3());
                    // 下位互換
                    if (backword)
                    {
                        SetFieldDataRep("Teikeibun1_", 1, 2, _systemConfig.AccountingTeikeibun1());
                        SetFieldDataRep("Teikeibun2_", 1, 2, _systemConfig.AccountingTeikeibun2());
                        SetFieldDataRep("Teikeibun3_", 1, 2, _systemConfig.AccountingTeikeibun3());
                    }
                }
            }
            #endregion

            // 医療機関関連
            #region HpInf
            // 病院名
            _printHpName();

            // 病院住所
            _printHpAddress();

            // 病院郵便番号
            _printHpPostCd();

            // 開設者氏名
            _printKaisetuName();

            // 病院電話番号
            _printHpTel();

            // 医療機関FAX番号
            _printHpFaxNo();

            // 医療機関その他連絡先
            _printHpOtherContacts();

            #endregion
            // 日付関連
            #region date
            // システム日付
            int SysDate = CIUtil.StrToIntDef(_printoutDateTime.ToString("yyyyMMdd"), 0);
            _printSysDate(SysDate);

            // 発行日                
            int SeikyuDate = Math.Max(coModel.LastSinDate, SysDate);
            _printPrintDate(SeikyuDate);

            // 発行日（指定）
            _printHakkoDate();

            // 診察日
            _printSinDate(coModel.LastSinDate);

            // 集計日
            _printSyukeiDate();

            // 診療期間
            _printSinDateTerm();

            // 実日数
            _printJituNissu();

            // 請求年月
            _printSeikyuYm();
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
            _printPtBirthday(SeikyuDate);

            // 患者住所
            _printPtAddress();

            // 患者郵便番号
            _printPtPostCd();
            #endregion
            // 保険関連
            #region HokenInf
            // 保険種別
            _printHokenSbt();
            _printHokenSbtAll();

            // 負担率
            _printFutanRate();
            _printFutanRateAll();

            // 保険分負担率
            _printHokenRate();
            _printHokenRateAll();

            // 保険者番号
            _printHokensyaNo();

            // 記号番号
            _printKigoBango();

            // 本人家族
            _printHonKe();

            // 公費負担者番号・受給者番号
            _printKohiFutansyaNo_JyukyusyaNo();
            #endregion
            // 自費関連
            #region Jihi
            // 自費負担額（自費診療＋自費項目）
            _printJihiFutan();
            // 自費分患者負担額
            _printJihiSinryo();
            // 自費項目の合計金額
            _printJihiKoumoku();
            // 自費非課税対象額
            _printJihiFutanFree();
            // 自費通常税率外税対象額
            _printJihiFutanOutTaxNr();
            // 自費軽減税率外税対象額
            _printJihiFutanOutTaxGen();
            // 自費通常税率内税対象額
            _printJihiFutanTaxNr();
            // 自費軽減税率内税対象額
            _printJihiFutanTaxGen();
            // 自費凡例
            _printTaxHanrei();

            // 外税
            _printOutTax();
            // 外税（通常税率分）
            _printOutTaxNr();
            // 外税（軽減税率分）
            _printOutTaxGen();

            // 内税
            _printInTax();
            // 内税（通常税率分）
            _printInTaxNr();
            // 内税（軽減税率分）
            _printInTaxGen();

            // 消費税
            _printTax();
            // 消費税（通常税率分）
            _printTaxNr();
            // 消費税（軽減税率分）
            _printTaxGen();

            _printTaxByRate();
            #endregion
            // 点数・金額関連
            #region tensu kingaku
            // 患者負担額
            _printPtFutan();

            // 請求額
            _printSeikyuGaku();
            _printSeikyuGakuAdjust();
            _printSeikyuGakuIncludeMenjyo();
            // 調整額
            _printAdjust();

            // 前回入金額
            _printNyukinGakuZenkai();
            // 今回入金額
            _printNyukinGakuKonkai();
            // 合計領収額
            _printNyukinGakuTotal();
            // 支払方法
            _printSiharaiHouhou();
            // バーコード
            _printSeikyuBarCode();

            // 未収額
            _printMisyu();
            // 患者未収額
            _printPtMisyu();
            // 返金額
            _printHenkin();

            // 行為別点数
            _printKouiTensu();

            // 自費種別別金額
            _printJihiSbtKingaku();
            #endregion

            // メッセージ関連
            #region message
            // メッセージリストP
            _printMessageP();

            // メッセージリストF
            _printMessageF();
            #endregion
            // 病名関連
            #region byomei
            // 病名リスト
            _printPtByomei();

            // 病名リスト（期間内更新分）
            _printPtByomeiToday();
            #endregion

            // リスト
            _printListData();

            // 月別請求点数・患者負担・入金額
            _printMonthly();
            // メモ
            _printMemo();
            // 患者メモ
            _printPtMemo();
            // 集計期間タイトル
            _printSyukeiTitle();
            // 予約
            _printYoyaku();
            // 領収証定型文
            _printTeikeibun();
        }

        // 本体
        void UpdateFormBody()
        {
            List<(string field, int charCount, int rowCount)> sinmeiListPropertys =
                _sinmeiListPropertysPage1;

            int sinmeiListIndex = 0;

            if (CurrentPage > 1)
            {
                // 2ページ目以降の場合
                sinmeiListPropertys = _sinmeiListPropertysPage2;
            }

            if (!sinmeiListPropertys.Any() || sinmeiListPropertys[0].rowCount <= 0)
            {
                return;
            }

            if (CurrentPage > 1)
            {
                // 2ページ目以降の場合、1ページ目に出力した行数と、2ページ名以降に出力した行数を求める
                sinmeiListIndex += _sinmeiListPropertysPage1.Sum(p => p.rowCount);
                sinmeiListIndex += (CurrentPage - 2) * sinmeiListPropertys.Sum(p => p.rowCount);
            }

            for (int listIndex = 0; listIndex < sinmeiListPropertys.Count; listIndex++)
            {

                int sinmeiRowCount = sinmeiListPropertys[listIndex].rowCount;
                string field = sinmeiListPropertys[listIndex].field;

                for (short i = 0; i < sinmeiRowCount; i++)
                {
                    ListText("lsSinKoui_" + field, 0, i, sinmeiPrintDataModels[sinmeiListIndex].KouiNm);
                    ListText("lsSinMei_" + field, 0, i, sinmeiPrintDataModels[sinmeiListIndex].MeiData);
                    ListText("lsSinSuuryo_" + field, 0, i, sinmeiPrintDataModels[sinmeiListIndex].Suuryo);
                    ListText("lsSinTani_" + field, 0, i, sinmeiPrintDataModels[sinmeiListIndex].Tani);
                    ListText("lsSinTensu_" + field, 0, i, sinmeiPrintDataModels[sinmeiListIndex].Tensu);
                    ListText("lsSinTotalTen_" + field, 0, i, sinmeiPrintDataModels[sinmeiListIndex].TotalTensu);
                    ListText("lsSinKaisu_" + field, 0, i, sinmeiPrintDataModels[sinmeiListIndex].Kaisu);
                    ListText("lsSinKaisuTani_" + field, 0, i, sinmeiPrintDataModels[sinmeiListIndex].KaisuTani);
                    ListText("lsSinTensuKaisu_" + field, 0, i, sinmeiPrintDataModels[sinmeiListIndex].TenKai);

                    sinmeiListIndex++;
                    if (sinmeiListIndex >= sinmeiPrintDataModels.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }

                if (_hasNextPage == false)
                {
                    break;
                }
            }
        }
        #endregion
    }

    private void UpdateDrawFormList()
    {
        UpdateFormHeader();
        UpdateFormBody();
        #region SubMethod
        // ヘッダー 
        void UpdateFormHeader()
        {
            #region print method
            // 医療機関関連
            #region print hpinf
            // 病院名
            void _printHpName()
            {
                SetFieldDataRep("dfHpName_", 1, 2, coModelList.HpName);
                // 下位互換
                SetFieldDataRep("HpName", 0, 3, coModelList.HpName);
            }
            // 病院住所
            void _printHpAddress()
            {
                // 正式
                // 合体版
                SetFieldDataRep("dfHpAddress", 1, 2, coModelList.HpAddress);
                // 分割版
                SetFieldDataRep("dfHpAddress1_", 1, 2, coModelList.HpAddress1);
                SetFieldDataRep("dfHpAddress2_", 1, 2, coModelList.HpAddress2);

                // 下位互換
                SetFieldsData("Address1", coModelList.HpAddress1);
                SetFieldsData("Address2", coModelList.HpAddress2);
                SetFieldDataRep("Address1_", 1, 3, coModelList.HpAddress1);
                SetFieldDataRep("Address2_", 1, 3, coModelList.HpAddress2);

            }
            // 病院郵便番号
            void _printHpPostCd()
            {
                SetFieldDataRep("dfHpPostCd_", 1, 2, coModelList.HpPostCdDsp);
                // 下位互換
                SetFieldDataRep("Zip", 0, 3, coModelList.HpPostCdDsp);
            }
            // 開設者
            void _printKaisetuName()
            {
                SetFieldDataRep("dfKaisetuName_", 1, 2, coModelList.KaisetuName);
                // 下位互換
                SetFieldDataRep("Establisher", 0, 3, coModelList.KaisetuName);
            }
            // 病院電話番号
            void _printHpTel()
            {
                SetFieldDataRep("dfHpTel_", 1, 2, coModelList.HpTel);
                // 下位互換
                SetFieldsData("Address3", coModelList.HpTel);
                SetFieldDataRep("Address3_", 1, 3, coModelList.HpTel);
            }
            #endregion

            // 日付関連
            #region print date
            // システム日付
            void _printSysDate(int AsysDate)
            {
                SetFieldDataRepSW("dfSysDate", 1, 2, AsysDate);
                // 下位互換
                SetFieldDataRep("StdPrnYmd", 1, 2, CIUtil.SDateToShowWDate3(AsysDate).Ymd);
                SetFieldDataRep("StdPrnSYmd", 1, 2, CIUtil.SDateToShowSDate(AsysDate));
                SetFieldDataRep("StdPrnWYmd", 1, 2, CIUtil.SDateToShowWDate3(AsysDate).Ymd);
            }
            // 発行日
            void _printPrintDate(int Adate)
            {
                SetFieldDataRepSW("dfPrintDate", 1, 2, Adate);
                // 下位互換
                SetFieldDataRep("PrnYmd", 1, 2, "発行日 " + CIUtil.SDateToShowWDate3(Adate).Ymd);
                SetFieldDataRep("PrnYmdOnly", 1, 2, CIUtil.SDateToShowWDate3(Adate).Ymd);
                SetFieldDataRep("PrnSYmd", 1, 2, CIUtil.SDateToShowSDate(Adate));
                SetFieldDataRep("PrnWYmd", 1, 2, CIUtil.SDateToShowWDate3(Adate).Ymd);
            }
            // 発行日（指定）
            void _printHakkoDate()
            {
                SetFieldDataRepSW("dfHakkoDate", 1, 2, HakkoDay);
                // 下位互換
                SetFieldDataRep("HakkoYmd", 1, 2, CIUtil.SDateToShowSDate(HakkoDay));
                SetFieldDataRep("PrintDay", 1, 2, CIUtil.SDateToShowWDate3(HakkoDay).Ymd);
            }

            // 集計日
            void _printSyukeiDate()
            {
                SetFieldDataRepSW("dfSyukeiFrom", 1, 2, StartDate);
                SetFieldDataRep("dfSyukeiFromYearS_", 1, 2, StartDate / 10000);
                SetFieldDataRep("dfSyukeiFromYearW_", 1, 2, CIUtil.SDateToShowWDate3(StartDate).Gengo + " " + CIUtil.SDateToShowWDate3(StartDate).Year);
                SetFieldDataRep("dfSyukeiFromMonth_", 1, 2, StartDate % 10000 / 100);

                SetFieldDataRepSW("dfSyukeiTo", 1, 2, EndDate);
                SetFieldDataRep("dfSyukeiToYearS_", 1, 2, EndDate / 10000);
                SetFieldDataRep("dfSyukeiToYearW_", 1, 2, CIUtil.SDateToShowWDate3(EndDate).Gengo + " " + CIUtil.SDateToShowWDate3(EndDate).Year);
                SetFieldDataRep("dfSyukeiToMonth_", 1, 2, EndDate % 10000 / 100);

                // 下位互換
                SetFieldDataRep("SyukeiFrom", 1, 2, CIUtil.SDateToShowWDate3(StartDate).Ymd);
                SetFieldDataRep("SyukeiFromYear", 1, 2, StartDate / 10000);
                SetFieldDataRep("SyukeiFromMonth", 1, 2, StartDate % 10000 / 100);
                SetFieldDataRep("SyukeiTo", 1, 2, CIUtil.SDateToShowWDate3(EndDate).Ymd);
                SetFieldDataRep("SyukeiToYear", 1, 2, EndDate / 10000);
                SetFieldDataRep("SyukeiToMonth", 1, 2, EndDate % 10000 / 100);
            }
            // 請求月
            void _printSeikyuYm()
            {
                if (StartDate / 100 == EndDate / 100)
                {
                    SetFieldDataRep("dfSeikyuYmS_", 1, 2, $"{StartDate / 10000}年{StartDate % 10000 / 100}月");
                    CIUtil.WarekiYmd wareki = CIUtil.SDateToShowWDate3(StartDate);
                    SetFieldDataRep("dfSeikyuYmW_", 1, 2, $"{wareki.Gengo} {wareki.Year}年{wareki.Month}月");
                    // 下位互換
                    SetFieldDataRep("SeikyuYm", 1, 2, $"{StartDate / 10000}年{StartDate % 10000 / 100}月");
                }
            }
            // グループ名
            void _printGrpName()
            {
                if (ptGrpNameMstModels != null)
                {
                    foreach (CoPtGrpNameMstModel ptGrpNameMstModel in ptGrpNameMstModels)
                    {
                        SetFieldDataRep($"dfGrpName{ptGrpNameMstModel.GrpId}_", 1, 2, ptGrpNameMstModel.GrpName);

                        string grpCd = coModelList.GetGroupCd(ptGrpNameMstModel.GrpId);
                        SetFieldDataRep($"dfGrpCd{ptGrpNameMstModel.GrpId}_", 1, 2, grpCd);

                        // 下位互換
                        if (ptGrpNameMstModel.GrpId >= 1 && ptGrpNameMstModel.GrpId <= 6)
                        {
                            SetFieldsData($"ClassNm{ptGrpNameMstModel.GrpId}", ptGrpNameMstModel.GrpName);
                            SetFieldsData($"ClassCd{ptGrpNameMstModel.GrpId}", grpCd);
                        }
                    }
                }
            }
            // 検索条件グループ名
            void _printGrpCondition()
            {
                if (GrpConditions != null && GrpConditions.Any())
                {
                    foreach ((int grpId, string grpCd) in GrpConditions)
                    {
                        if (ptGrpNameMstModels.Any(p => p.GrpId == grpId))
                        {
                            SetFieldDataRep($"dfGrpCondition{grpId}_", 1, 2, ptGrpNameMstModels.Find(p => p.GrpId == grpId)?.GrpName ?? string.Empty);
                        }
                    }
                }
            }
            // 検索条件グループ項目
            void _printGrpItemCondition()
            {
                if (GrpConditions != null && GrpConditions.Any())
                {
                    foreach ((int grpId, string grpCd) in GrpConditions)
                    {
                        if (ptGrpItemModels.Any(p => p.GrpId == grpId && p.GrpCode == grpCd) && GrpConditions.Count(p => p.grpId == grpId) == 1)
                        {
                            // 同一グループで複数の値が指定されている場合は印字なし
                            SetFieldDataRep($"dfGrpItemCondition{grpId}_", 1, 2, ptGrpItemModels.Find(p => p.GrpId == grpId && p.GrpCode == grpCd)?.GrpCodeName ?? string.Empty);
                        }
                    }
                }
            }
            #endregion
            #endregion

            // 医療機関関連
            #region HpInf
            // 病院名
            _printHpName();

            // 病院住所
            _printHpAddress();

            // 病院郵便番号
            _printHpPostCd();

            // 開設者氏名
            _printKaisetuName();

            // 病院電話番号
            _printHpTel();
            #endregion

            // 日付関連
            #region date
            // システム日付
            int SysDate = CIUtil.StrToIntDef(_printoutDateTime.ToString("yyyyMMdd"), 0);
            _printSysDate(SysDate);

            // 発行日                
            _printPrintDate(SysDate);

            // 発行日（指定）
            _printHakkoDate();

            // 集計日
            _printSyukeiDate();

            // 請求年月
            _printSeikyuYm();

            // グループ名称／コード
            _printGrpName();

            #endregion

            // 検索条件

            // グループ名
            _printGrpCondition();
            // グループ項目名
            _printGrpItemCondition();
        }

        // 本体
        void UpdateFormBody()
        {
            List<int> jihiSbts = new();

            #region　print method
            // 患者番号
            void _printPtNum(short row, int index)
            {
                SetListDataRep("lsPtNum_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].PtNum);
                // 下位互換
                ListText("KanNoList", 0, row, coModelList.KaikeiInfListModels[index].PtNum);
            }
            // 患者氏名
            void _printPtName(short row, int index)
            {
                SetListDataRep("lsPtName_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].PtName);
                // 下位互換
                ListText("KanNameList", 0, row, coModelList.KaikeiInfListModels[index].PtName);
            }
            // 性別
            void _printSex(short row, int index)
            {
                SetListDataRep("lsSex_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].PtSex);
                // 下位互換
                ListText("KanSexList", 0, row, coModelList.KaikeiInfListModels[index].PtSex);
            }
            // 生年月日
            void _printBirthDay(short row, int index)
            {
                SetListDataRepSW("lsBirthDay", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].BirthDay);
                // 年齢
                SetListDataRep("lsAge_", 1, 2, 0, row, CIUtil.SDateToAge(coModelList.KaikeiInfListModels[index].BirthDay, HakkoDay));

                // 下位互換
                ListText("KanBirthdayList", 0, row, CIUtil.SDateToShowWDate3(coModelList.KaikeiInfListModels[index].BirthDay).Ymd +
                    "(" + CIUtil.SDateToAge(coModelList.KaikeiInfListModels[index].BirthDay, HakkoDay).ToString() + "歳)");
            }
            // 郵便番号
            void _printPtPostCd(short row, int index)
            {
                SetListDataRep("lsPtPostCd_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].PostCdDsp);
                // 下位互換
                ListText("KanZipList", 0, row, coModelList.KaikeiInfListModels[index].PostCdDsp);
            }
            // 住所
            void _printPtAddress(short row, int index)
            {
                SetListDataRep("lsPtAddress_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].Address);
                // 下位互換
                ListText("KanAddressList", 0, row, coModelList.KaikeiInfListModels[index].Address);
            }
            // 電話番号
            void _printPtTel(short row, int index)
            {
                SetListDataRep("lsPtTel_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].Tel);
                // 下位互換
                ListText("KanTelList", 0, row, coModelList.KaikeiInfListModels[index].Tel);
            }
            // 電話番号１
            void _printPtTel1(short row, int index)
            {
                SetListDataRep("lsPtTel1_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].Tel1);
                // 下位互換
                ListText("KanTelList1", 0, row, coModelList.KaikeiInfListModels[index].Tel1);
            }
            // 電話番号２
            void _printPtTel2(short row, int index)
            {
                SetListDataRep("lsPtTel2_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].Tel2);
                // 下位互換
                ListText("KanTelList2", 0, row, coModelList.KaikeiInfListModels[index].Tel2);
            }
            // 患者メモ
            void _printPtMemo(short row, int index)
            {
                SetListDataRep("lsPtMemo_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].Memo);

                string[] del = { "\r\n", "\r", "\n" };
                string[] splitPtMemos = coModelList.KaikeiInfListModels[index].Memo.Split(del, StringSplitOptions.None);

                for (int i = 0; i < 5; i++)
                {
                    if (i >= splitPtMemos.Count())
                    {
                        break;
                    }

                    SetFieldDataRep($"lsPtMemo{i + 1}_", 1, 2, splitPtMemos[i]);
                    // 下位互換
                    SetFieldsData($"UkeMemoList{i + 1}", splitPtMemos[i]);
                }
            }

            // 請求額
            void _printSeikyuGaku(short row, int index)
            {
                SetListDataRep("lsSeikyuGaku_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].SeikyuGaku);
                // 下位互換
                ListText("SeikyuList", 0, row, coModelList.KaikeiInfListModels[index].SeikyuGaku);
            }
            // 入金額リスト
            void _printNyukinGaku(short row, int index)
            {
                SetListDataRep("lsNyukinGaku_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].NyukinGaku);
                // 下位互換
                ListText("KanNyuList", 0, row, coModelList.KaikeiInfListModels[index].NyukinGaku);
            }
            // 未収金リスト
            void _printMisyu(short row, int index)
            {
                SetListDataRep("lsMisyu_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].Misyu);
                // 下位互換
                ListText("MisyuList", 0, row, coModelList.KaikeiInfListModels[index].Misyu);
            }
            // 総医療費
            void _printTotalIryohi(short row, int index)
            {
                SetListDataRep("lsTotalIryohi_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].TotalIryohi);
                // 下位互換
                ListText("DataGokeiList", 0, row, coModelList.KaikeiInfListModels[index].TotalIryohi);
            }
            // 患者負担額
            void _printPtFutan(short row, int index)
            {
                SetListDataRep("lsPtFutan_", 1, 2, 0, row,
                    coModelList.KaikeiInfListModels[index].PtFutan +
                    coModelList.KaikeiInfListModels[index].AdjustRound);
                // 下位互換
                ListText("HoSeikyuList", 0, row,
                    coModelList.KaikeiInfListModels[index].PtFutan +
                    coModelList.KaikeiInfListModels[index].AdjustRound);
            }
            // 自費負担額
            void _printJihiFutan(short row, int index)
            {
                SetListDataRep("lsJihiFutan_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].JihiFutan + coModelList.KaikeiInfListModels[index].JihiOuttax);
                // 下位互換
                ListText("JihiSeikyuList", 0, row, coModelList.KaikeiInfListModels[index].JihiFutan + coModelList.KaikeiInfListModels[index].JihiOuttax);
            }
            // 自費項目金額リスト
            void _printJihiKoumoku(short row, int index)
            {
                SetListDataRep("lsJihiKoumoku_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].JihiKoumoku);
                // 下位互換
                ListText("HoGaiList", 0, row, coModelList.KaikeiInfListModels[index].JihiKoumoku);
            }
            // 自費診療リスト
            void _printJihiSinryo(short row, int index)
            {
                SetListDataRep("lsJihiSinryo_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].JihiSinryo);
                // 下位互換
                ListText("JihiList", 0, row, coModelList.KaikeiInfListModels[index].JihiSinryo);
            }
            // 自費項目明細リスト
            void _printJihiKoumokuDtl(int jihiSbt, short row, int index)
            {
                SetListDataRep($"lsJihi{jihiSbt}_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].JihiKoumokuDtlKingaku(jihiSbt));
                // 下位互換
                if (jihiSbt >= 1 && jihiSbt <= 5)
                {
                    ListText($"JihiList{jihiSbt}", 0, row, coModelList.KaikeiInfListModels[index].JihiKoumokuDtlKingaku(jihiSbt));
                }
            }
            // 自費非課税対象額
            void _printJihiFutanFree(short row, int index)
            {
                SetListDataRep("lsJihiFutanZeiFree_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].JihiFutanFree);
            }
            // 自費通常税率外税対象額
            void _printJihiFutanOuttaxNr(short row, int index)
            {
                SetListDataRep("lsJihiFutanZeiNr_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].JihiFutanOuttaxNr);
            }
            // 自費軽減税率外税対象額
            void _printJihiFutanOuttaxGen(short row, int index)
            {
                SetListDataRep("lsJihiFutanZeiGen_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].JihiFutanOuttaxGen);
            }
            // 自費通常税率内税対象額
            void _printJihiFutanTaxNr(short row, int index)
            {
                SetListDataRep("lsJihiFutanUchiNr_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].JihiFutanTaxNr);
            }
            // 自費軽減税率内税対象額
            void _printJihiFutanTaxGen(short row, int index)
            {
                SetListDataRep("lsJihiFutanUchiGen_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].JihiFutanTaxGen);
            }
            // 自費外税
            void _printJihiSotoZei(short row, int index)
            {
                SetListDataRep("lsJihiSotoZei_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].JihiOuttax);
            }
            // 自費外税（通常税率分）
            void _printJihiSotoZeiNr(short row, int index)
            {
                SetListDataRep("lsJihiSotoZeiNr_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].JihiOuttaxNr);
            }
            // 自費外税（軽減税率分）
            void _printJihiSotoZeiGen(short row, int index)
            {
                SetListDataRep("lsJihiSotoZeiGen_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].JihiOuttaxGen);
            }
            // 自費内税
            void _printJihiUchiZei(short row, int index)
            {
                SetListDataRep("lsJihiUchiZei_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].JihiTax);
            }
            // 自費内税（通常税率）
            void _printJihiUchiZeiNr(short row, int index)
            {
                SetListDataRep("lsJihiUchiZeiNr_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].JihiTaxNr);
            }
            // 自費内税（軽減税率）
            void _printJihiUchiZeiGen(short row, int index)
            {
                SetListDataRep("lsJihiUchiZeiGen_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].JihiTaxGen);
            }
            // 消費税
            void _printJihiZei(short row, int index)
            {
                SetListDataRep("lsJihiZei_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].JihiOuttax + coModelList.KaikeiInfListModels[index].JihiTax);
            }
            // 消費税（通常税率）
            void _printJihiZeiNr(short row, int index)
            {
                SetListDataRep("lsJihiZeiNr_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].JihiOuttaxNr + coModelList.KaikeiInfListModels[index].JihiTaxNr);
            }
            // 消費税（軽減税率）
            void _printJihiZeiGen(short row, int index)
            {
                SetListDataRep("lsJihiZeiGen_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].JihiOuttaxGen + coModelList.KaikeiInfListModels[index].JihiTaxGen);
            }
            // 自費税率別
            void _printJihiByRate(short row, int index)
            {
                foreach (TaxSum taxSum in coModelList.KaikeiInfListModels[index].TaxSums)
                {
                    SetListDataRep($"lsJihiFutanZei{taxSum.Rate}_", 1, 2, 0, row, taxSum.OuttaxFutan);
                    SetListDataRep($"lsJihiFutanUchi{taxSum.Rate}_", 1, 2, 0, row, taxSum.TaxFutan);
                    SetListDataRep($"lsJihiSotoZei{taxSum.Rate}_", 1, 2, 0, row, taxSum.OuttaxZei);
                    SetListDataRep($"lsJihiUchiZei{taxSum.Rate}_", 1, 2, 0, row, taxSum.TaxZei);
                    SetListDataRep($"lsJihiZei{taxSum.Rate}_", 1, 2, 0, row, taxSum.OuttaxZei + taxSum.TaxZei);
                }
            }
            // 点数
            void _printTensu(short row, int index)
            {
                SetListDataRep("lsTensu_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].Tensu);
                // 下位互換
                ListText("JihiList", 0, row, coModelList.KaikeiInfListModels[index].Tensu);
            }
            // 総医療費
            void _printTotalSeikyu(short row, int index)
            {
                SetListDataRep("lsTotalSeikyu_", 1, 2, 0, row, coModelList.KaikeiInfListModels[index].TotalIryohi + coModelList.KaikeiInfListModels[index].JihiFutan + coModelList.KaikeiInfListModels[index].JihiOuttax);
                // 下位互換
                ListText("ALLDataGokeiList", 0, row, coModelList.KaikeiInfListModels[index].TotalIryohi + coModelList.KaikeiInfListModels[index].JihiFutan + coModelList.KaikeiInfListModels[index].JihiOuttax);

            }
            //自費種別別金額内訳
            void _printJihiSbtKingaku(short row, int index)
            {
                // 自費種別別金額
                foreach (string ListObjectName in _javaOutputData.objectNames.FindAll(p => p.StartsWith("lsJihiSbt_")))
                {
                    double kingaku = 0;

                    // 分割
                    string[] jihiSbtCds = ListObjectName.Split('_');

                    foreach (string jihiSbtCd in jihiSbtCds)
                    {
                        if (CIUtil.StrToIntDef(jihiSbtCd, 0) > 0 && coModelList.KaikeiInfListModels[index].JihiSbtKingakus.Any(p => p.JihiSbt == CIUtil.StrToIntDef(jihiSbtCd, 0)))
                        {
                            kingaku += coModelList.KaikeiInfListModels[index].JihiSbtKingakus.Where(p => p.JihiSbt == CIUtil.StrToIntDef(jihiSbtCd, 0)).Sum(p => p.Kingaku);
                        }
                    }

                    ListText(ListObjectName, 0, row, kingaku.ToString());
                }
            }
            // 合計
            void _printGokei(short row)
            {
                SetListDataRep("lsPtNum_", 1, 2, 0, row, "合　計");
                SetListDataRep("lsPtName_", 1, 2, 0, row, $"{TotalPtCount} 件");
                SetListDataRep("lsSeikyuGaku_", 1, 2, 0, row, TotalSeikyuGaku);
                SetListDataRep("lsNyukinGaku_", 1, 2, 0, row, TotalNyukinGaku);
                SetListDataRep("lsMisyu_", 1, 2, 0, row, TotalMisyu);
                SetListDataRep("lsTotalIryohi_", 1, 2, 0, row, TotalIryohi);
                SetListDataRep("lsPtFutan_", 1, 2, 0, row, TotalPtFutan);
                SetListDataRep("lsJihiFutan_", 1, 2, 0, row, TotalJihiFutan);
                SetListDataRep("lsJihiKoumoku_", 1, 2, 0, row, TotalJihiKoumoku);
                SetListDataRep("lsJihiSinryo_", 1, 2, 0, row, TotalJihiSinryo);

                // 自費負担額
                SetListDataRep("lsJihiFutanZeiFree_", 1, 2, 0, row, TotalJihiFutanFree);
                SetListDataRep("lsJihiFutanZeiNr_", 1, 2, 0, row, TotalJihiFutanOuttaxNr);
                SetListDataRep("lsJihiFutanZeiGen_", 1, 2, 0, row, TotalJihiFutanOuttaxGen);
                SetListDataRep("lsJihiFutanUchiNr_", 1, 2, 0, row, TotalJihiFutanTaxNr);
                SetListDataRep("lsJihiFutanUchiGen_", 1, 2, 0, row, TotalJihiFutanTaxGen);

                foreach (var jihiSbt in jihiSbts.Where(jihiSbt => TotalJihiKoumokuDtl.Any(p => p.jihiSbt == jihiSbt)).ToList())
                {
                    (int jihiSbt, double kingaku) totalJihiKoumokuDtl = TotalJihiKoumokuDtl.First(p => p.jihiSbt == jihiSbt);
                    SetListDataRep($"lsJihi{jihiSbt}_", 1, 2, 0, row, totalJihiKoumokuDtl.kingaku);
                }

                // 外税
                SetListDataRep("lsJihiSotoZei_", 1, 2, 0, row, TotalSotoZei);
                SetListDataRep("lsJihiSotoZeiNr_", 1, 2, 0, row, TotalSotoZeiNr);
                SetListDataRep("lsJihiSotoZeiGen_", 1, 2, 0, row, TotalSotoZeiGen);

                // 内税
                SetListDataRep("lsJihiUchiZei_", 1, 2, 0, row, TotalUchiZei);
                SetListDataRep("lsJihiUchiZeiNr_", 1, 2, 0, row, TotalUchiZeiNr);
                SetListDataRep("lsJihiUchiZeiGen_", 1, 2, 0, row, TotalUchiZeiGen);

                // 消費税
                SetListDataRep("lsJihiZei_", 1, 2, 0, row, TotalZei);
                SetListDataRep("lsJihiZeiNr_", 1, 2, 0, row, TotalZeiNr);
                SetListDataRep("lsJihiZeiGen_", 1, 2, 0, row, TotalZeiGen);

                SetFieldDataRep("dfPtCount_", 1, 2, TotalPtCount);
                SetFieldDataRep("dfTotalTensu_", 1, 2, TotalTensu);
                SetFieldDataRep("dfAveTensu_", 1, 2, Math.Round((double)(TotalTensu / TotalPtCount), MidpointRounding.AwayFromZero));

                // 請求額合計
                SetFieldDataRep("dfTotalSeikyuGaku_", 1, 2, TotalSeikyuGaku);
                // 入金額の合計
                SetFieldDataRep("dfNyukinGaku_", 1, 2, TotalNyukinGaku);
                // 未収額の合計
                SetFieldDataRep("dfMisyu_", 1, 2, TotalMisyu);
                // 医療費の合計
                SetFieldDataRep("dfTotalIryohi_", 1, 2, TotalIryohi);
                // 患者負担の合計
                SetFieldDataRep("dfPtFutan_", 1, 2, TotalPtFutan);
                // 自費負担の合計
                SetFieldDataRep("dfJihiFutan_", 1, 2, TotalJihiFutan);
                // 自費項目の合計
                SetFieldDataRep("dfJihiKoumoku_", 1, 2, TotalJihiKoumoku);
                // 自費診療の合計
                SetFieldDataRep("dfJihiSinryo_", 1, 2, TotalJihiSinryo);

                // 自費負担額
                SetFieldDataRep("dfJihiFutanZeiFree_", 1, 2, TotalJihiFutanFree);
                SetFieldDataRep("dfJihiFutanZeiNr_", 1, 2, TotalJihiFutanOuttaxNr);
                SetFieldDataRep("dfJihiFutanZeiGen_", 1, 2, TotalJihiFutanOuttaxGen);
                SetFieldDataRep("dfJihiFutanUchiNr_", 1, 2, TotalJihiFutanTaxNr);
                SetFieldDataRep("dfJihiFutanUchiGen_", 1, 2, TotalJihiFutanTaxGen);

                // 下位互換
                ListText("KanNoList", 0, row, "合　計");
                ListText("KanNameList", 0, row, $"{TotalPtCount} 件");
                ListText("SeikyuList", 0, row, TotalSeikyuGaku);
                ListText("KanNyuList", 0, row, TotalNyukinGaku);
                ListText("MisyuList", 0, row, TotalMisyu);
                ListText("DataGokeiList", 0, row, TotalIryohi);
                ListText("HoSeikyuList", 0, row, TotalPtFutan);
                ListText("JihiSeikyuList", 0, row, TotalJihiFutan);
                ListText("HoGaiList", 0, row, TotalJihiKoumoku);
                ListText("JihiList", 0, row, TotalJihiSinryo);
                foreach (var jihiSbt in jihiSbts.Where(jihiSbt => TotalJihiKoumokuDtl.Any(p => p.jihiSbt == jihiSbt)).ToList())
                {
                    (int jihiSbt, double kingaku) totalJihiKoumokuDtl = TotalJihiKoumokuDtl.First(p => p.jihiSbt == jihiSbt);
                    ListText($"JihiList{jihiSbt}", 0, row, totalJihiKoumokuDtl.kingaku);
                }

                SetFieldsData("KanKei", TotalPtCount);
                SetFieldsData("SinTenKei", TotalTensu);
                SetFieldsData("SinTenAve", Math.Round((double)(TotalTensu / TotalPtCount), MidpointRounding.AwayFromZero));
                foreach (var ListObjectName in _javaOutputData.objectNames.Where(ListObjectName => ListObjectName.StartsWith("lsJihiSbt_")))
                {
                    double kingaku = 0;
                    // 分割
                    string[] jihiSbtCds = ListObjectName.Split('_');
                    foreach (string jihiSbtCd in jihiSbtCds)
                    {
                        if (CIUtil.StrToIntDef(jihiSbtCd, 0) > 0 && TotalJihiSbtKingakus.Any(p => p.JihiSbt == CIUtil.StrToIntDef(jihiSbtCd, 0)))
                        {
                            kingaku += TotalJihiSbtKingakus.Where(p => p.JihiSbt == CIUtil.StrToIntDef(jihiSbtCd, 0)).Sum(p => p.Kingaku);
                        }
                    }

                    ListText(ListObjectName, 0, row, kingaku.ToString());
                }
            }
            #endregion

            #region local method
            void _addTotal(int idx)
            {
                #region gokei
                // 合計の変数に足す
                TotalPtCount++;
                TotalTensu += coModelList.KaikeiInfListModels[idx].Tensu;
                TotalSeikyuGaku += coModelList.KaikeiInfListModels[idx].SeikyuGaku;
                TotalNyukinGaku += coModelList.KaikeiInfListModels[idx].NyukinGaku;
                TotalMisyu += coModelList.KaikeiInfListModels[idx].Misyu;
                TotalIryohi += coModelList.KaikeiInfListModels[idx].TotalIryohi;
                TotalPtFutan += coModelList.KaikeiInfListModels[idx].PtFutan +
                    coModelList.KaikeiInfListModels[idx].AdjustRound;

                TotalJihiFutan += coModelList.KaikeiInfListModels[idx].JihiFutan + coModelList.KaikeiInfListModels[idx].JihiOuttax;
                TotalJihiKoumoku += coModelList.KaikeiInfListModels[idx].JihiKoumoku;
                TotalJihiSinryo += coModelList.KaikeiInfListModels[idx].JihiSinryo;

                // 自費分
                TotalJihiFutanFree += coModelList.KaikeiInfListModels[idx].JihiFutanFree;
                TotalJihiFutanOuttaxNr += coModelList.KaikeiInfListModels[idx].JihiFutanOuttaxNr;
                TotalJihiFutanOuttaxGen += coModelList.KaikeiInfListModels[idx].JihiFutanOuttaxGen;
                TotalJihiFutanTaxNr += coModelList.KaikeiInfListModels[idx].JihiFutanTaxNr;
                TotalJihiFutanTaxGen += coModelList.KaikeiInfListModels[idx].JihiFutanTaxGen;

                TotalJihiKoumokuDtl.Clear();
                foreach (int jihiSbt in jihiSbts)
                {
                    if (TotalJihiKoumokuDtl.Any(p => p.jihiSbt == jihiSbt))
                    {
                        (int jihiSbt, double kingaku) totalJihiKoumokuDtl = TotalJihiKoumokuDtl.First(p => p.jihiSbt == jihiSbt);
                        totalJihiKoumokuDtl.kingaku += coModelList.KaikeiInfListModels[idx].JihiKoumokuDtlKingaku(jihiSbt);
                    }
                    else
                    {
                        TotalJihiKoumokuDtl.Add((jihiSbt, coModelList.KaikeiInfListModels[idx].JihiKoumokuDtlKingaku(jihiSbt)));
                    }
                }
                // 外税
                TotalSotoZei = coModelList.KaikeiInfListModels[idx].JihiOuttax;
                TotalSotoZeiNr = coModelList.KaikeiInfListModels[idx].JihiOuttaxNr;
                TotalSotoZeiGen = coModelList.KaikeiInfListModels[idx].JihiOuttaxGen;

                // 内税
                TotalUchiZei = coModelList.KaikeiInfListModels[idx].JihiTax;
                TotalUchiZeiNr = coModelList.KaikeiInfListModels[idx].JihiTaxNr;
                TotalUchiZeiGen = coModelList.KaikeiInfListModels[idx].JihiTaxGen;

                // 消費税
                TotalZei = coModelList.KaikeiInfListModels[idx].JihiOuttax + coModelList.KaikeiInfListModels[idx].JihiTax;
                TotalZeiNr = coModelList.KaikeiInfListModels[idx].JihiOuttaxNr + coModelList.KaikeiInfListModels[idx].JihiTaxNr;
                TotalZeiGen = coModelList.KaikeiInfListModels[idx].JihiOuttaxGen + coModelList.KaikeiInfListModels[idx].JihiTaxGen;

                foreach (CoJihiSbtKingakuModel jihiSbtKingaku in coModelList.KaikeiInfListModels[idx].JihiSbtKingakus)
                {
                    if (TotalJihiSbtKingakus.Any(p => p.JihiSbt == jihiSbtKingaku.JihiSbt))
                    {
                        TotalJihiSbtKingakus.Find(p => p.JihiSbt == jihiSbtKingaku.JihiSbt).Kingaku += jihiSbtKingaku.Kingaku;
                    }
                    else
                    {
                        TotalJihiSbtKingakus.Add(new CoJihiSbtKingakuModel(jihiSbtKingaku.JihiSbt, jihiSbtKingaku.Kingaku));
                    }
                }
                #endregion
            }
            #endregion

            if (coModelList.KaikeiInfListModels == null || !coModelList.KaikeiInfListModels.Any())
            {
                return;
            }

            int listIndex = 0;

            if (listIndex >= coModelList.KaikeiInfListModels.Count)
            {
                _printGokei(0);
            }
            else if (ListGridRowCount <= 0)
            {
                // リストがない場合、合計だけ出す
                for (int i = 0; i < coModelList.KaikeiInfListModels.Count; i++)
                {
                    _addTotal(i);
                }

                // 合計出力
                _printGokei(0);
            }
            else
            {
                for (short i = 0; i < ListGridRowCount; i++)
                {
                    // 患者番号
                    _printPtNum(i, listIndex);

                    // 患者氏名
                    _printPtName(i, listIndex);

                    // 性別
                    _printSex(i, listIndex);

                    // 生年月日と年齢
                    _printBirthDay(i, listIndex);

                    // 郵便番号
                    _printPtPostCd(i, listIndex);

                    // 住所
                    _printPtAddress(i, listIndex);

                    // 電話
                    _printPtTel(i, listIndex);

                    // 電話１
                    _printPtTel1(i, listIndex);

                    // 電話２
                    _printPtTel2(i, listIndex);

                    // 患者メモ
                    _printPtMemo(i, listIndex);

                    // 請求金額リスト
                    _printSeikyuGaku(i, listIndex);

                    // 入金額リスト
                    _printNyukinGaku(i, listIndex);

                    // 未収金リスト
                    _printMisyu(i, listIndex);

                    // 総医療費リスト
                    _printTotalIryohi(i, listIndex);

                    // 患者負担額リスト
                    _printPtFutan(i, listIndex);

                    // 自費負担リスト
                    _printJihiFutan(i, listIndex);

                    // 自費項目金額リスト
                    _printJihiKoumoku(i, listIndex);

                    // 自費診療リスト
                    _printJihiSinryo(i, listIndex);

                    // 自費項目明細金額
                    jihiSbts = coModelList.GetJihiSbt();

                    foreach (int jihiSbt in jihiSbts)
                    {
                        _printJihiKoumokuDtl(jihiSbt, i, listIndex);
                    }

                    // 自費非課税対象額
                    _printJihiFutanFree(i, listIndex);

                    // 自費通常税率外税対象額
                    _printJihiFutanOuttaxNr(i, listIndex);

                    // 自費軽減税率外税対象額
                    _printJihiFutanOuttaxGen(i, listIndex);

                    // 自費通常税率内税対象額
                    _printJihiFutanTaxNr(i, listIndex);

                    // 自費軽減税率内税対象額
                    _printJihiFutanTaxGen(i, listIndex);

                    // 自費外税
                    _printJihiSotoZei(i, listIndex);
                    // 自費外税（通常税率）
                    _printJihiSotoZeiNr(i, listIndex);
                    // 自費外税（軽減税率）
                    _printJihiSotoZeiGen(i, listIndex);

                    // 自費内税
                    _printJihiUchiZei(i, listIndex);
                    // 自費内税（通常税率）
                    _printJihiUchiZeiNr(i, listIndex);
                    // 自費内税（軽減税率）
                    _printJihiUchiZeiGen(i, listIndex);

                    // 自費消費税
                    _printJihiZei(i, listIndex);
                    // 自費消費税（通常税率）
                    _printJihiZeiNr(i, listIndex);
                    // 自費消費税（軽減税率）
                    _printJihiZeiGen(i, listIndex);
                    // 自費税率別
                    _printJihiByRate(i, listIndex);
                    // 点数
                    _printTensu(i, listIndex);

                    //自費種別別金額内訳
                    _printJihiSbtKingaku(i, listIndex);

                    // 総医療費＋総自費
                    _printTotalSeikyu(i, listIndex);

                    // 合計の変数に足す
                    _addTotal(listIndex);

                    listIndex++;
                    if (listIndex >= coModelList.KaikeiInfListModels.Count)
                    {
                        if (i < ListGridRowCount - 1)
                        {
                            // 合計出力
                            _printGokei((short)(i + 1));
                        }
                        break;
                    }
                }
            }
        }
        #endregion
    }

    private CoAccountingModel GetData(int hpId, long ptId, int startDate, int endDate)
    {
        // 診療日
        int sinDate = endDate;

        // 医療機関情報
        CoHpInfModel hpInfModel = _finder.FindHpInf(hpId, sinDate);
        List<CoWarningMessage> warningMessages = new();

        List<CoKaikeiInfModel> kaikeiInfModels;
        if (!NyukinBase)
        {
            kaikeiInfModels = _finder.FindKaikeiInf(hpId, ptId, startDate, endDate, RaiinNos, HokenId, MiseisanKbn, SaiKbn, MisyuKbn, SeikyuKbn, HokenKbn, HokenSeikyu, JihiSeikyu, ref warningMessages);
        }
        else
        {
            kaikeiInfModels = _finder.FindKaikeiInfNyukinBase(hpId, ptId, startDate, endDate, HokenId, MiseisanKbn, SaiKbn, MisyuKbn, SeikyuKbn, HokenKbn, HokenSeikyu, JihiSeikyu, ref warningMessages);

            if (kaikeiInfModels != null && kaikeiInfModels.Any())
            {
                List<int> sinDates = kaikeiInfModels.GroupBy(p => p.SinDate).Select(p => p.Key).OrderBy(p => p).ToList();
                startDate = sinDates.First();
                endDate = sinDates.Last();
            }
        }

        // 診療期間
        SinStartDate = startDate;
        SinEndDate = endDate;

        if (kaikeiInfModels == null || !kaikeiInfModels.Any())
        {
            return new();
        }

        // 実日数
        JituNissu = kaikeiInfModels
                    .GroupBy(p => p.SinDate)
                    .Select(p => new { JituNissu = p.Max(q => q.JituNissu) })
                    .Sum(p => p.JituNissu);

        List<int> sinYMs = kaikeiInfModels.GroupBy(p => p.SinDate / 100).Select(p => p.Key).OrderBy(p => p).ToList();

        // 来院番号リストを改める
        RaiinNos = kaikeiInfModels.GroupBy(p => p.RaiinNo).Select(p => p.Key).ToList();

        // メモ
        CoPtMemoModel ptMemoModel = _finder.FindPtMemo(hpId, ptId);

        // 患者情報            
        CoPtInfModel ptInfModel = _finder.FindPtInf(hpId, ptId);

        // 診療情報
        List<SinMeiViewModel> sinMeiViewModels = new();

        foreach (int sinYM in sinYMs)
        {
            sinMeiViewModels.Add(new SinMeiViewModel(
                                     SinMeiMode.Ryosyu,
                                     false,
                                     hpId,
                                     ptId,
                                     sinYM * 100 + 1,
                                     RaiinNos,
                                     _tenantProvider,
                                     _systemConfigProvider,
                                     _emrLogger,
                                     _systemConfig.AccountingDetailIncludeComment() == 0
                                     ));
        }

        // 所見
        List<CoKarteInfModel> karteInfModels = _finder.FindKarteInf(hpId, PtId, startDate, endDate, RaiinNos);

        // オーダー
        List<CoOdrInfModel> odrInfModels = _finder.FindOdrInfData(hpId, PtId, startDate, endDate, RaiinNos);
        List<CoOdrInfDetailModel> odrInfDetailModels = _finder.FindOdrInfDetailData(hpId, ptId, startDate, endDate, RaiinNos);

        // 病名
        List<CoPtByomeiModel> ptByomeiModels = _finder.FindPtByomei(hpId, ptId, startDate, endDate);

        // 来院

        // 予約
        // 予約情報
        List<CoRaiinInfModel> raiinInfModels = _finder.FindYoyakuRaiinInf(hpId, PtId, endDate);

        // システム世代マスタ
        List<CoSystemGenerationConfModel> systemGenerationConfModels = _finder.FindSystemGenerationConf(hpId, 3001);

        CoAccountingModel result =
            new CoAccountingModel(
                hpInfModel, kaikeiInfModels, ptInfModel, sinMeiViewModels, karteInfModels, odrInfModels, odrInfDetailModels, ptByomeiModels, ptMemoModel, raiinInfModels, systemGenerationConfModels);

        jihiSbtMstModels = _finder.FindJihiSbtMst(hpId);
        ptGrpNameMstModels = _finder.FindPtGrpNameMst(hpId);
        sysGeneHanreis = _finder.FindSystemGenerationConf(hpId, 3001);

        return result;
    }

    private CoAccountingListModel GetDataList(
        int hpId, int startDate, int endDate,
        List<(long ptId, int hokenId)> ptConditions, List<(int grpId, string grpCd)> grpConditions,
        int sort, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn)
    {

        // 診療日
        int sinDate = endDate;

        // 医療機関情報
        CoHpInfModel hpInfModel = _finder.FindHpInf(hpId, sinDate);

        // 会計情報
        List<CoWarningMessage> warningMessages = new();
        List<CoKaikeiInfListModel> kaikeiInfListModels = _finder.FindKaikeiInfList(hpId, startDate, endDate, ptConditions, grpConditions, sort, miseisanKbn, saiKbn, misyuKbn, seikyuKbn, hokenKbn, ref warningMessages);

        CoAccountingListModel result =
            new CoAccountingListModel(
                hpInfModel, kaikeiInfListModels);

        jihiSbtMstModels = _finder.FindJihiSbtMst(hpId);
        ptGrpNameMstModels = _finder.FindPtGrpNameMst(hpId);
        ptGrpItemModels = _finder.FindPtGrpItemMst(hpId);

        sysGeneHanreis = _finder.FindSystemGenerationConf(hpId, 3001);

        return result;
    }

    private void SetFieldsData(List<string> fieldList, object value)
    {
        foreach (string field in fieldList)
        {
            SetFieldsData(field, value);
        }
    }

    private void SetFieldsData(string field, object value)
    {
        if (!_singleFieldDataResult.ContainsKey(field))
        {
            _singleFieldDataResult.Add(field, value.AsString());
        }
    }

    private void ListText(string listName, short col, short row, object data)
    {
        var item = new ListTextModel(listName, col, row, data.AsString());
        _listTextModelResult.Add(item);
    }

    private void SetFieldDataRep(string baseStr, int from, int to, object Value)
    {
        SetFieldsData(MakeFieldNames(baseStr, from, to), Value);
    }

    private void SetFieldDataRepSW(string baseStr, int from, int to, int date)
    {
        // 西暦
        SetFieldsData(MakeFieldNames(baseStr + "S_", from, to), CIUtil.SDateToShowSDate(date));
        // 和暦
        SetFieldsData(MakeFieldNames(baseStr + "W_", from, to), CIUtil.SDateToShowWDate3(date).Ymd);
    }

    private List<string> MakeFieldNames(string baseStr, int from, int to)
    {
        List<string> results = new();

        for (int i = from; i <= to; i++)
        {
            if (i == 0)
            {
                results.Add($"{baseStr}");
            }
            else
            {
                results.Add($"{baseStr}{i}");
            }
        }

        return results;
    }

    private void SetListDataRep(string baseStr, int from, int to, short col, short row, object Value)
    {
        SetListsData(MakeFieldNames(baseStr, from, to), col, row, Value);
    }

    private void SetListDataRepSW(string baseStr, int from, int to, short col, short row, int date)
    {
        // 西暦
        SetListsData(MakeFieldNames(baseStr + "S_", from, to), col, row, CIUtil.SDateToShowSDate(date));
        // 和暦
        SetListsData(MakeFieldNames(baseStr + "W_", from, to), col, row, CIUtil.SDateToShowWDate3(date).Ymd);
    }

    private void SetListsData(List<string> fieldList, short col, short row, object value)
    {
        foreach (string field in fieldList)
        {
            ListText(field, col, row, value);
        }
    }
    private void SetListsData(string field, short col, short row, object value)
    {
        ListText(field, col, row, value);
    }

    private bool betweenStartEnd(DateTime date)
    {
        return (CIUtil.DateTimeToInt(date) >= StartDate &&
                CIUtil.DateTimeToInt(date) <= EndDate);
    }

    private int GetListRowCount(string fileName)
    {
        return _javaOutputData.responses.FirstOrDefault(item => item.typeInt == (int)CalculateTypeEnum.GetListRowCount && item.listName == fileName)?.result ?? 0;
    }

    private int GetListFormatLenB(string fileName)
    {
        return _javaOutputData.responses.FirstOrDefault(item => item.typeInt == (int)CalculateTypeEnum.GetListFormatLendB && item.listName == fileName)?.result ?? 0;
    }
}
