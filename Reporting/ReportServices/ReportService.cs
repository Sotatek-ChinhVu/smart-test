using Reporting.Accounting.DB;
using Reporting.Accounting.Model;
using Reporting.Accounting.Model.Output;
using Reporting.Accounting.Service;
using Reporting.AccountingCard.Service;
using Reporting.AccountingCardList.Model;
using Reporting.AccountingCardList.Service;
using Reporting.Byomei.Service;
using Reporting.CommonMasters.Enums;
using Reporting.DailyStatic.Service;
using Reporting.DrugInfo.Model;
using Reporting.DrugInfo.Service;
using Reporting.DrugNoteSeal.Service;
using Reporting.GrowthCurve.Model;
using Reporting.GrowthCurve.Service;
using Reporting.InDrug.Service;
using Reporting.Karte1.Service;
using Reporting.Karte3.Service;
using Reporting.KensaLabel.Model;
using Reporting.KensaLabel.Service;
using Reporting.Kensalrai.Service;
using Reporting.Mappers.Common;
using Reporting.MedicalRecordWebId.Service;
using Reporting.Memo.Service;
using Reporting.NameLabel.Service;
using Reporting.OrderLabel.Model;
using Reporting.OrderLabel.Service;
using Reporting.OutDrug.Model.Output;
using Reporting.OutDrug.Service;
using Reporting.PatientManagement.Service;
using Reporting.Receipt.Service;
using Reporting.ReceiptCheck.Service;
using Reporting.ReceiptList.Model;
using Reporting.ReceiptList.Service;
using Reporting.ReceiptPrint.Service;
using Reporting.ReceTarget.Service;
using Reporting.Sijisen.Service;
using Reporting.SyojyoSyoki.Service;
using Reporting.Yakutai.Service;

namespace Reporting.ReportServices;

public class ReportService : IReportService
{
    private readonly IOrderLabelCoReportService _orderLabelCoReportService;
    private readonly IDrugInfoCoReportService _drugInfoCoReportService;
    private readonly ISijisenReportService _sijisenReportService;
    private readonly IByomeiService _byomeiService;
    private readonly IKarte1Service _karte1Service;
    private readonly INameLabelService _nameLabelService;
    private readonly IMedicalRecordWebIdReportService _medicalRecordWebIdReportService;
    private readonly IReceiptCheckCoReportService _receiptCheckCoReportService;
    private readonly IReceiptListCoReportService _receiptListCoReportService;
    private readonly IOutDrugCoReportService _outDrugCoReportService;
    private readonly IAccountingCoReportService _accountingCoReportService;
    private readonly IStatisticService _statisticService;
    private readonly IReceiptCoReportService _receiptCoReportService;
    private readonly IPatientManagementService _patientManagementService;
    private readonly ISyojyoSyokiCoReportService _syojyoSyokiCoReportService;
    private readonly IKensaIraiCoReportService _kensaIraiCoReportService;
    private readonly IReceiptPrintService _receiptPrintService;
    private readonly IMemoMsgCoReportService _memoMsgCoReportService;
    private readonly IReceTargetCoReportService _receTargetCoReportService;
    private readonly IDrugNoteSealCoReportService _drugNoteSealCoReportService;
    private readonly IYakutaiCoReportService _yakutaiCoReportService;
    private readonly IAccountingCardCoReportService _accountingCardCoReportService;
    private readonly ICoAccountingFinder _coAccountingFinder;
    private readonly IKarte3CoReportService _karte3CoReportService;
    private readonly IAccountingCardListCoReportService _accountingCardListCoReportService;
    private readonly IInDrugCoReportService _inDrugCoReportService;
    private readonly IGrowthCurveA4CoReportService _growthCurveA4CoReportService;
    private readonly IGrowthCurveA5CoReportService _growthCurveA5CoReportService;
    private readonly IKensaLabelCoReportService _kensaLabelCoReportService;

    public ReportService(IOrderLabelCoReportService orderLabelCoReportService, IDrugInfoCoReportService drugInfoCoReportService, ISijisenReportService sijisenReportService, IByomeiService byomeiService, IKarte1Service karte1Service, INameLabelService nameLabelService, IMedicalRecordWebIdReportService medicalRecordWebIdReportService, IReceiptCheckCoReportService receiptCheckCoReportService, IReceiptListCoReportService receiptListCoReportService, IOutDrugCoReportService outDrugCoReportService, IAccountingCoReportService accountingCoReportService, IStatisticService statisticService, IReceiptCoReportService receiptCoReportService, IPatientManagementService patientManagementService, ISyojyoSyokiCoReportService syojyoSyokiCoReportService, IKensaIraiCoReportService kensaIraiCoReportService, IReceiptPrintService receiptPrintService, IMemoMsgCoReportService memoMsgCoReportService, IReceTargetCoReportService receTargetCoReportService, IDrugNoteSealCoReportService drugNoteSealCoReportService, IYakutaiCoReportService yakutaiCoReportService, IAccountingCardCoReportService accountingCardCoReportService, ICoAccountingFinder coAccountingFinder, IKarte3CoReportService karte3CoReportService, IAccountingCardListCoReportService accountingCardListCoReportService, IInDrugCoReportService inDrugCoReportService, IGrowthCurveA4CoReportService growthCurveA4CoReportService, IGrowthCurveA5CoReportService growthCurveA5CoReportService, IKensaLabelCoReportService kensaLabelCoReportService)
    {
        _orderLabelCoReportService = orderLabelCoReportService;
        _drugInfoCoReportService = drugInfoCoReportService;
        _sijisenReportService = sijisenReportService;
        _byomeiService = byomeiService;
        _karte1Service = karte1Service;
        _nameLabelService = nameLabelService;
        _medicalRecordWebIdReportService = medicalRecordWebIdReportService;
        _receiptCheckCoReportService = receiptCheckCoReportService;
        _receiptListCoReportService = receiptListCoReportService;
        _outDrugCoReportService = outDrugCoReportService;
        _accountingCoReportService = accountingCoReportService;
        _statisticService = statisticService;
        _receiptCoReportService = receiptCoReportService;
        _patientManagementService = patientManagementService;
        _syojyoSyokiCoReportService = syojyoSyokiCoReportService;
        _kensaIraiCoReportService = kensaIraiCoReportService;
        _receiptPrintService = receiptPrintService;
        _memoMsgCoReportService = memoMsgCoReportService;
        _receTargetCoReportService = receTargetCoReportService;
        _drugNoteSealCoReportService = drugNoteSealCoReportService;
        _yakutaiCoReportService = yakutaiCoReportService;
        _accountingCardCoReportService = accountingCardCoReportService;
        _coAccountingFinder = coAccountingFinder;
        _karte3CoReportService = karte3CoReportService;
        _accountingCardListCoReportService = accountingCardListCoReportService;
        _inDrugCoReportService = inDrugCoReportService;
        _growthCurveA4CoReportService = growthCurveA4CoReportService;
        _growthCurveA5CoReportService = growthCurveA5CoReportService;
        _kensaLabelCoReportService = kensaLabelCoReportService;
    }

    //Byomei
    public CommonReportingRequestModel GetByomeiReportingData(int hpId, long ptId, int fromDay, int toDay, bool tenkiIn, List<int> hokenIds)
    {
        return _byomeiService.GetByomeiReportingData(hpId, ptId, fromDay, toDay, tenkiIn, hokenIds);
    }

    //Karte1
    public CommonReportingRequestModel GetKarte1ReportingData(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei)
    {
        return _karte1Service.GetKarte1ReportingData(hpId, ptId, sinDate, hokenPid, tenkiByomei, syuByomei);
    }

    //NameLabel
    public CommonReportingRequestModel GetNameLabelReportingData(long ptId, string kanjiName, int sinDate)
    {
        return _nameLabelService.GetNameLabelReportingData(ptId, kanjiName, sinDate);
    }

    //Sijisen
    public CommonReportingRequestModel GetSijisenReportingData(int hpId, int formType, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, bool printNoOdr)
    {
        return _sijisenReportService.GetSijisenReportingData(hpId, formType, ptId, sinDate, raiinNo, odrKouiKbns, printNoOdr);
    }

    //OrderLabel
    public CommonReportingRequestModel GetOrderLabelReportingData(int mode, int hpId, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, List<RsvkrtOdrInfModel> rsvKrtOdrInfModels)
    {
        return _orderLabelCoReportService.GetOrderLabelReportingData(mode, hpId, ptId, sinDate, raiinNo, odrKouiKbns, rsvKrtOdrInfModels);
    }

    //OrderInfo
    public DrugInfoData SetOrderInfo(int hpId, long ptId, int sinDate, long raiinNo)
    {
        return _drugInfoCoReportService.SetOrderInfo(hpId, ptId, sinDate, raiinNo);
    }

    public CommonReportingRequestModel GetInDrugPrintData(int hpId, long ptId, int sinDate, long raiinNo)
    {
        return _inDrugCoReportService.GetInDrugPrintData(hpId, ptId, sinDate, raiinNo);
    }

    public CommonReportingRequestModel GetAccountingCardListReportingData(int hpId, List<TargetItem> targets, bool includeOutDrug, string kaName, string tantoName, string uketukeSbt, string hoken)
    {
        return _accountingCardListCoReportService.GetAccountingCardListData(hpId, targets, includeOutDrug, kaName, tantoName, uketukeSbt, hoken);
    }

    //MedicalRecordWebId
    public CommonReportingRequestModel GetMedicalRecordWebIdReportingData(int hpId, long ptId, int sinDate)
    {
        return _medicalRecordWebIdReportService.GetMedicalRecordWebIdReportingData(hpId, ptId, sinDate);
    }

    //ReceiptCheckCoReport
    public CommonReportingRequestModel GetReceiptCheckCoReportService(int hpId, List<long> ptIds, int seikyuYm)
    {
        return _receiptCheckCoReportService.GetReceiptCheckCoReportingData(hpId, ptIds, seikyuYm);
    }

    //ReceiptListCoReport
    public CommonReportingRequestModel GetReceiptListReportingData(int hpId, int seikyuYm, List<ReceiptInputModel> receiptListModels)
    {
        return _receiptListCoReportService.GetReceiptListReportingData(hpId, seikyuYm, receiptListModels);
    }

    public CommonReportingRequestModel GetSyojyoSyokiReportingData(int hpId, long ptId, int seikyuYm, int hokenId)
    {
        return _syojyoSyokiCoReportService.GetSyojyoSyokiReportingData(hpId, ptId, seikyuYm, hokenId);
    }

    //KensaLabelCoReportService
    public CommonReportingRequestModel GetKensaLabelPrintData(int hpId, long ptId, long raiinNo, int sinDate, KensaPrinterModel printerModel)
    {
        return _kensaLabelCoReportService.GetKensaLabelPrintData(hpId, ptId, raiinNo, sinDate, printerModel);
    }

    /// <summary>
    /// OutDrug
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="ptId"></param>
    /// <param name="sinDate"></param>
    /// <param name="raiinNo"></param>
    /// <returns></returns>
    public CoOutDrugReportingOutputData GetOutDrugReportingData(int hpId, long ptId, int sinDate, long raiinNo)
    {
        return _outDrugCoReportService.GetOutDrugReportingData(hpId, ptId, sinDate, raiinNo);
    }

    /// <summary>
    /// GetAccountingReportingData
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="ptId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="raiinNos"></param>
    /// <param name="hokenId"></param>
    /// <param name="miseisanKbn"></param>
    /// <param name="saiKbn"></param>
    /// <param name="misyuKbn"></param>
    /// <param name="seikyuKbn"></param>
    /// <param name="hokenKbn"></param>
    /// <param name="hokenSeikyu"></param>
    /// <param name="jihiSeikyu"></param>
    /// <param name="nyukinBase"></param>
    /// <param name="hakkoDay"></param>
    /// <param name="memo"></param>
    /// <param name="printType"></param>
    /// <param name="formFileName"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public AccountingResponse GetAccountingReportingData(int hpId, long ptId, int startDate, int endDate, List<long> raiinNos, int hokenId = 0, int miseisanKbn = 0, int saiKbn = 0, int misyuKbn = 0, int seikyuKbn = 1, int hokenKbn = 0, bool hokenSeikyu = false, bool jihiSeikyu = false, bool nyukinBase = false, int hakkoDay = 0, string memo = "", int printType = 0, string formFileName = "")
    {
        return _accountingCoReportService.GetAccountingReportingData(hpId, ptId, startDate, endDate, raiinNos, hokenId, miseisanKbn, saiKbn, misyuKbn, seikyuKbn, hokenKbn, hokenSeikyu, jihiSeikyu, nyukinBase, hakkoDay, memo, printType, formFileName);
    }

    /// <summary>
    /// GetAccountingReportingData
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="ptConditions"></param>
    /// <param name="grpConditions"></param>
    /// <param name="sort"></param>
    /// <param name="miseisanKbn"></param>
    /// <param name="saiKbn"></param>
    /// <param name="misyuKbn"></param>
    /// <param name="seikyuKbn"></param>
    /// <param name="hokenKbn"></param>
    /// <param name="hakkoDay"></param>
    /// <param name="memo"></param>
    /// <param name="formFileName"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public AccountingResponse GetAccountingReportingData(int hpId, int startDate, int endDate, List<(long ptId, int hokenId)> ptConditions, List<(int grpId, string grpCd)> grpConditions, int sort, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn, int hakkoDay, string memo, string formFileName)
    {
        return _accountingCoReportService.GetAccountingReportingData(hpId, startDate, endDate, ptConditions, grpConditions, sort, miseisanKbn, saiKbn, misyuKbn, seikyuKbn, hokenKbn, hakkoDay, memo, formFileName);
    }

    /// <summary>
    /// GetAccountingReportingData
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="coAccountingParamModels"></param>
    /// <returns></returns>
    public AccountingResponse GetAccountingReportingData(int hpId, List<CoAccountingParamModel> coAccountingParamModels)
    {
        return _accountingCoReportService.GetAccountingReportingData(hpId, coAccountingParamModels);
    }

    public AccountingResponse GetAccountingData(int hpId, ConfirmationMode mode, long ptId, List<CoAccountDueListModel> multiAccountDueListModels, bool isPrintMonth, bool ryoshusho, bool meisai)
    {
        List<CoAccountingParamModel> requestAccountting = new();

        List<CoAccountDueListModel> nyukinModels = _coAccountingFinder.GetAccountDueList(hpId, ptId);
        List<int> months = new();
        foreach (var model in multiAccountDueListModels)
        {
            var selectedAccountDueListModel = model;
            var accountDueListModels = nyukinModels.FindAll(p => p.SinDate / 100 == model.SinDate / 100);
            if (isPrintMonth)
            {
                if (!months.Contains(model.SinDate / 100))
                {
                    var printItem = PrintWithoutThread(ryoshusho, meisai, mode, ptId, accountDueListModels, selectedAccountDueListModel, isPrintMonth, model.SinDate, model.OyaRaiinNo, accountDueListModels);
                    requestAccountting.AddRange(printItem);
                    months.Add(model.SinDate / 100);
                }
            }
            else
            {
                var printItem = PrintWithoutThread(ryoshusho, meisai, mode, ptId, accountDueListModels, selectedAccountDueListModel, isPrintMonth, model.SinDate, model.OyaRaiinNo);
                requestAccountting.AddRange(printItem);
            }
        }

        AccountingResponse result = null;
        try
        {
            result = _accountingCoReportService.GetAccountingReportingData(hpId, requestAccountting);
            Console.WriteLine("result AccountingResponse: " + result);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex);
        }
        return result;
    }

    public List<CoAccountingParamModel> PrintWithoutThread(bool ryoshusho, bool meisai, ConfirmationMode mode, long ptId, List<CoAccountDueListModel> accountDueListModels, CoAccountDueListModel selectedAccountDueListModel, bool isPrintMonth, int sinDate, long oyaRaiinNo, List<CoAccountDueListModel>? nyukinModels = null)
    {
        int GetMaxDateInMonth(int month)
        {
            var models = accountDueListModels.Where(x => x.Month == month).OrderBy(x => x.SinDate);
            return models.Last().SinDate;
        }

        int GetMinDateInMonth(int month)
        {
            var models = accountDueListModels.Where(x => x.Month == month).OrderBy(x => x.SinDate);
            return models.First().SinDate;
        }

        List<CoAccountingParamModel> result = new();
        List<(int startDate, int endDate)> dates = new();
        if (accountDueListModels.Count >= 1)
        {
            if (isPrintMonth)
            {
                var groups = accountDueListModels.GroupBy(x => x.Month);
                List<int> months = groups.Select(x => x.Key).ToList();
                foreach (var month in months)
                {
                    dates.Add((GetMinDateInMonth(month), GetMaxDateInMonth(month)));
                }
            }
            else
            {
                if (mode == ConfirmationMode.FromPrintBtn || mode == ConfirmationMode.FromMenu)
                {
                    dates.Add((selectedAccountDueListModel.SinDate, selectedAccountDueListModel.SinDate));
                }
                else
                {
                    dates.AddRange(from item in accountDueListModels
                                   select (item.SinDate, item.SinDate));
                    dates = dates.Distinct().ToList();
                }
            }
        }

        List<int> printTypes = new();
        if (isPrintMonth)
        {
            if (ryoshusho)
            {
                ///2:月間領収証
                ///3:月間明細
                printTypes.Add(2);
            }

            if (meisai)
            {
                printTypes.Add(3);
            }
        }
        else
        {
            if (ryoshusho)
            {
                ///0:領収証
                ///1:明細
                printTypes.Add(0);
            }

            if (meisai)
            {
                printTypes.Add(1);
            }
        }

        if (!isPrintMonth && mode == ConfirmationMode.FromPrintBtn || mode == ConfirmationMode.FromMenu)
        {
            foreach (var printType in printTypes)
            {
                var raiinNos = accountDueListModels.Where(x => x.OyaRaiinNo == (oyaRaiinNo > 0 ? oyaRaiinNo : selectedAccountDueListModel.OyaRaiinNo)).Select(x => x.RaiinNo).ToList();
                result.Add(new(
                              ptId,
                              sinDate > 0 ? sinDate : selectedAccountDueListModel.SinDate,
                              sinDate > 0 ? sinDate : selectedAccountDueListModel.SinDate,
                              raiinNos,
                              printType: printType));
            }
        }
        else
        {
            foreach (var (startDate, endDate) in dates)
            {
                if (isPrintMonth)
                {
                    List<long> raiinNos = new();
                    if (nyukinModels != null)
                    {
                        raiinNos = nyukinModels.Where(x => x.SinDate >= startDate && x.SinDate <= endDate)
                                                                         .Select(x => x.RaiinNo).ToList();
                    }
                    else
                    {
                        accountDueListModels.Where(x => x.SinDate >= startDate && x.SinDate <= endDate)
                                                                         .Select(x => x.RaiinNo).ToList();
                    }
                    foreach (var printType in printTypes)
                    {
                        result.Add(new(ptId, startDate, endDate, raiinNos, printType: printType));
                    }
                }
                else
                {
                    List<(long, List<long>)> raiinNos;
                    if (nyukinModels != null)
                    {
                        raiinNos = nyukinModels.Where(x => x.SinDate >= startDate && x.SinDate <= endDate)
                                               .GroupBy(x => x.OyaRaiinNo)
                                               .Select(x => (x.Key, x.Select(item => item.RaiinNo).ToList())).ToList();
                    }
                    else
                    {
                        raiinNos = accountDueListModels.Where(x => x.SinDate >= startDate && x.SinDate <= endDate)
                                                       .GroupBy(x => x.OyaRaiinNo)
                                                       .Select(x => (x.Key, x.Select(item => item.RaiinNo).ToList())).ToList();
                    }
                    foreach (var printType in printTypes)
                    {
                        foreach (var oya in raiinNos)
                        {
                            result.Add(new(ptId, startDate, endDate, oya.Item2, printType: printType));
                        }
                    }
                }
            }
        }
        return result;
    }

    /// <summary>
    /// GetAccountingReportingData
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="ptId"></param>
    /// <param name="printTypeInput"></param>
    /// <param name="raiinNoList"></param>
    /// <param name="raiinNoPayList"></param>
    /// <param name="isCalculateProcess"></param>
    /// <returns></returns>
    public AccountingResponse GetAccountingReportingData(int hpId, long ptId, int printTypeInput, List<long> raiinNoList, List<long> raiinNoPayList, bool isCalculateProcess)
    {
        return _accountingCoReportService.GetAccountingReportingData(hpId, ptId, printTypeInput, raiinNoList, raiinNoPayList, isCalculateProcess);
    }

    public CommonReportingRequestModel GetStatisticReportingData(int hpId, string formName, int menuId, int monthFrom, int monthTo, int dateFrom, int dateTo, int timeFrom, int timeTo, CoFileType? coFileType = null, bool? isPutTotalRow = false, int? tenkiDateFrom = -1, int? tenkiDateTo = -1, int? enableRangeFrom = -1, int? enableRangeTo = -1, long? ptNumFrom = 0, long? ptNumTo = 0)
    {
        return _statisticService.PrintExecute(hpId, formName, menuId, monthFrom, monthTo, dateFrom, dateTo, timeFrom, timeTo, coFileType, isPutTotalRow, tenkiDateFrom, tenkiDateTo, enableRangeFrom, enableRangeTo, ptNumFrom, ptNumTo);
    }

    public CommonReportingRequestModel GetPatientManagement(int hpId, int menuId)
    {
        return _patientManagementService.PrintData(hpId, menuId);
    }

    //Receipt Preview
    public CommonReportingRequestModel GetReceiptData(int hpId, long ptId, int sinYm, int hokenId)
    {
        return _receiptCoReportService.GetReceiptData(hpId, ptId, sinYm, hokenId);
    }

    public CommonReportingRequestModel GetKensalraiData(int hpId, int systemDate, int fromDate, int toDate, string centerCd)
    {
        return _kensaIraiCoReportService.GetKensalraiData(hpId, systemDate, fromDate, toDate, centerCd);
    }

    public CommonReportingRequestModel GetReceiptPrint(int hpId, string formName, int prefNo, int reportId, int reportEdaNo, int dataKbn, int ptId, int seikyuYm, int sinYm, int hokenId, int diskKind, int diskCnt, int welfareType, List<string> printHokensyaNos)
    {
        return _receiptPrintService.GetReceiptPrint(hpId, formName, prefNo, reportId, reportEdaNo, dataKbn, ptId, seikyuYm, sinYm, hokenId, diskKind, diskCnt, welfareType, printHokensyaNos);
    }

    public CommonReportingRequestModel GetMemoMsgReportingData(string reportName, string title, List<string> listMessage)
    {
        return _memoMsgCoReportService.GetMemoMsgReportingData(reportName, title, listMessage);
    }

    public CommonReportingRequestModel GetReceTargetPrint(int hpId, int seikyuYm)
    {
        return _receTargetCoReportService.GetReceTargetPrintData(hpId, seikyuYm);
    }

    public CommonReportingRequestModel GetDrugNoteSealPrintData(int hpId, long ptId, int sinDate, long raiinNo)
    {
        return _drugNoteSealCoReportService.GetDrugNoteSealPrintData(hpId, ptId, sinDate, raiinNo);
    }

    public CommonReportingRequestModel GetYakutaiReportingData(int hpId, long ptId, int sinDate, int raiinNo)
    {
        return _yakutaiCoReportService.GetYakutaiReportingData(hpId, ptId, sinDate, raiinNo);
    }

    public CommonReportingRequestModel GetAccountingCardReportingData(int hpId, long ptId, int sinYm, int hokenId, bool includeOutDrug)
    {
        return _accountingCardCoReportService.GetAccountingCardReportingData(hpId, ptId, sinYm, hokenId, includeOutDrug);
    }

    // Karte 3
    public CommonReportingRequestModel GetKarte3ReportingData(int hpId, long ptId, int startSinYm, int endSinYm, bool includeHoken, bool includeJihi)
    {
        return _karte3CoReportService.GetKarte3PrintData(hpId, ptId, startSinYm, endSinYm, includeHoken, includeJihi);
    }

    public CommonReportingRequestModel GetGrowthCurveA4PrintData(int hpId, GrowthCurveConfig growthCurveConfig)
    {
        return _growthCurveA4CoReportService.GetGrowthCurveA4PrintData(hpId, growthCurveConfig);
    }

    public CommonReportingRequestModel GetGrowthCurveA5PrintData(int hpId, GrowthCurveConfig growthCurveConfig)
    {
        return _growthCurveA5CoReportService.GetGrowthCurveA5PrintData(hpId, growthCurveConfig);
    }
}
