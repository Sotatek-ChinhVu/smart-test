using Domain.Models.Receipt.ReceiptListAdvancedSearch;
using Domain.Models.SpecialNote.PatientInfo;
using Helper.Common;
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
using Reporting.KensaHistory.Service;
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
using Reporting.PatientManagement.Models;
using Reporting.PatientManagement.Service;
using Reporting.Receipt.Service;
using Reporting.ReceiptCheck.Service;
using Reporting.ReceiptList.Model;
using Reporting.ReceiptList.Service;
using Reporting.ReceiptPrint.Service;
using Reporting.ReceTarget.Service;
using Reporting.Sijisen.Service;
using Reporting.Statistics.Sta9000.Models;
using Reporting.Statistics.Sta9000.Service;
using Reporting.SyojyoSyoki.Service;
using Reporting.Yakutai.Service;
using static System.Net.Mime.MediaTypeNames;

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
    private readonly IReceiptPrintExcelService _receiptPrintExcelService;
    private readonly IImportCSVCoReportService _importCSVCoReportService;
    private readonly IStaticsticExportCsvService _staticsticExportCsvService;
    private readonly ISta9000CoReportService _sta9000CoReportService;
    private readonly IKensaHistoryCoReportService _kensaHistoryCoReportService;
    private readonly IKensaResultMultiCoReportService _kensaResultMultiCoReportService;

    public ReportService(IOrderLabelCoReportService orderLabelCoReportService, IDrugInfoCoReportService drugInfoCoReportService, ISijisenReportService sijisenReportService, IByomeiService byomeiService, IKarte1Service karte1Service, INameLabelService nameLabelService, IMedicalRecordWebIdReportService medicalRecordWebIdReportService, IReceiptCheckCoReportService receiptCheckCoReportService, IReceiptListCoReportService receiptListCoReportService, IOutDrugCoReportService outDrugCoReportService, IAccountingCoReportService accountingCoReportService, IStatisticService statisticService, IReceiptCoReportService receiptCoReportService, IPatientManagementService patientManagementService, ISyojyoSyokiCoReportService syojyoSyokiCoReportService, IKensaIraiCoReportService kensaIraiCoReportService, IReceiptPrintService receiptPrintService, IMemoMsgCoReportService memoMsgCoReportService, IReceTargetCoReportService receTargetCoReportService, IDrugNoteSealCoReportService drugNoteSealCoReportService, IYakutaiCoReportService yakutaiCoReportService, IAccountingCardCoReportService accountingCardCoReportService, ICoAccountingFinder coAccountingFinder, IKarte3CoReportService karte3CoReportService, IAccountingCardListCoReportService accountingCardListCoReportService, IInDrugCoReportService inDrugCoReportService, IGrowthCurveA4CoReportService growthCurveA4CoReportService, IGrowthCurveA5CoReportService growthCurveA5CoReportService, IKensaLabelCoReportService kensaLabelCoReportService, IReceiptPrintExcelService receiptPrintExcelService, IImportCSVCoReportService importCSVCoReportService, IStaticsticExportCsvService staticsticExportCsvService, ISta9000CoReportService sta9000CoReportService, IKensaHistoryCoReportService kensaHistoryCoReportService, IKensaResultMultiCoReportService kensaResultMultiCoReportService)
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
        _receiptPrintExcelService = receiptPrintExcelService;
        _importCSVCoReportService = importCSVCoReportService;
        _staticsticExportCsvService = staticsticExportCsvService;
        _sta9000CoReportService = sta9000CoReportService;
        _kensaHistoryCoReportService = kensaHistoryCoReportService;
        _kensaResultMultiCoReportService = kensaResultMultiCoReportService;
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

        List<CoAccountDueListModel> accountDueListUnique = new();
        foreach (var model in multiAccountDueListModels)
        {
            if (accountDueListUnique.Any(item => item.SinDate == model.SinDate
                                                 && item.NyukinKbn == model.NyukinKbn
                                                 && item.RaiinNo == model.RaiinNo
                                                 && item.OyaRaiinNo == model.OyaRaiinNo))
            {
                continue;
            }
            accountDueListUnique.Add(model);
        }

        foreach (var model in accountDueListUnique)
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

    public CommonReportingRequestModel GetPatientManagement(int hpId, PatientManagementModel patientManagementModel)
    {
        return _patientManagementService.PrintData(hpId, patientManagementModel);
    }

    //Receipt Preview
    public CommonReportingRequestModel GetReceiptData(int hpId, long ptId, int sinYm, int hokenId, int seikyuYm, int hokenKbn, bool isIncludeOutDrug, bool isModePrint, bool isOpenedFromAccounting)
    {
        if (isOpenedFromAccounting)
        {
            return _receiptCoReportService.GetReceiptDataFromAccounting(hpId, ptId, sinYm, hokenId, isIncludeOutDrug, isModePrint);
        }
        else
        {
            return _receiptCoReportService.GetReceiptDataFromReceCheck(hpId, ptId, sinYm, seikyuYm, hokenId, hokenKbn, isIncludeOutDrug, isModePrint);
        }
    }

    public CommonReportingRequestModel GetKensalraiData(int hpId, int systemDate, int fromDate, int toDate, string centerCd)
    {
        return _kensaIraiCoReportService.GetKensalraiData(hpId, systemDate, fromDate, toDate, centerCd);
    }

    //public CommonReportingRequestModel GetReceiptPrint(int hpId, string formName, int prefNo, int reportId, int reportEdaNo, int dataKbn, int ptId, int seikyuYm, int sinYm, int hokenId, int diskKind, int diskCnt, int welfareType, List<string> printHokensyaNos, List<long> printPtIds)
    //{
    //    return _receiptPrintService.GetReceiptPrint(hpId, formName, prefNo, reportId, reportEdaNo, dataKbn, ptId, seikyuYm, sinYm, hokenId, diskKind, diskCnt, welfareType, printHokensyaNos, printPtIds);
    //}

    public CommonReportingRequestModel GetReceiptPrint(int hpId, string formName, int prefNo, int reportId, int reportEdaNo, int dataKbn, long ptId, int seikyuYm, int sinYm, int hokenId, int diskKind, int diskCnt, int welfareType, List<string> printHokensyaNos, int hokenKbn, ReseputoShubetsuModel selectedReseputoShubeusu, int departmentId, int doctorId, int printNoFrom, int printNoTo, bool includeTester, bool includeOutDrug, int sort, List<long> listPtId)
    {
        return _receiptPrintService.GetReceiptPrint(hpId, formName, prefNo, reportId, reportEdaNo, dataKbn, ptId, seikyuYm, sinYm, hokenId, diskKind, diskCnt, welfareType, printHokensyaNos, hokenKbn, selectedReseputoShubeusu, departmentId, doctorId, printNoFrom, printNoTo, includeTester, includeOutDrug, sort, listPtId);
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

    public CommonExcelReportingModel GetReceiptPrintExcel(int hpId, int prefNo, int reportId, int reportEdaNo, int dataKbn, int seikyuYm)
    {
        return _receiptPrintExcelService.GetReceiptPrintExcel(hpId, prefNo, reportId, reportEdaNo, dataKbn, seikyuYm);
    }

    public CommonExcelReportingModel GetReceiptListExcel(int hpId, int seikyuYm, ReceiptListAdvancedSearchInput receiptListModel, bool isIsExportTitle)
    {
        return _importCSVCoReportService.GetImportCSVCoReportServiceReportingData(hpId, seikyuYm, receiptListModel, isIsExportTitle);
    }

    public List<string> OutputExcelForPeriodReceipt(int hpId, int startDate, int endDate, List<Tuple<long, int>> ptConditions, List<Tuple<int, string>> grpConditions, int sort, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn)
    {
        return _accountingCoReportService.ExportCsv(hpId, startDate, endDate, ptConditions, grpConditions, sort, miseisanKbn, saiKbn, misyuKbn, seikyuKbn, hokenKbn);
    }

    #region Period Report
    public AccountingResponse GetPeriodPrintData(int hpId, int startDate, int endDate, List<PtInfInputItem> sourcePt, List<(int grpId, string grpCd)> grpConditions, int printSort, bool isPrintList, bool printByMonth, bool printByGroup, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn, int hakkoDay, string memo, string formFileName, bool nyukinBase)
    {
        DateTime startDateOrigin = CIUtil.IntToDate(startDate);
        DateTime endDateOrigin = CIUtil.IntToDate(endDate);
        List<CoAccountingParamListModel> requestModelList = new();
        List<CoAccountingParamModel> coAccountingParamModels = new();

        List<(int startDate, int endDate)> dates = GetDates(startDateOrigin, endDateOrigin, startDate, endDate, printByMonth);
        if (isPrintList)
        {
            List<(long ptId, int hokenId)> ptCoditions = GetPtCondition(sourcePt, printSort);

            if (printByGroup)
            {
                foreach (var group in grpConditions)
                {
                    foreach (var date in dates)
                    {
                        requestModelList.Add(new(date.startDate,
                                                 date.endDate,
                                                 ptCoditions,
                                                 new List<(int grpId, string grpCd)>() { group }));
                    }
                }
                return _accountingCoReportService.GetAccountingReportingData(hpId, requestModelList, printSort, miseisanKbn, saiKbn, misyuKbn, seikyuKbn, hokenKbn, hakkoDay, memo, formFileName);
            }
            else
            {
                foreach (var date in dates)
                {
                    requestModelList.Add(new(date.startDate,
                                             date.endDate,
                                             ptCoditions,
                                             grpConditions));
                }
                return _accountingCoReportService.GetAccountingReportingData(hpId, requestModelList, printSort, miseisanKbn, saiKbn, misyuKbn, seikyuKbn, hokenKbn, hakkoDay, memo, formFileName);
            }
        }
        else
        {
            foreach (var date in dates)
            {
                foreach (var ptInf in sourcePt)
                {
                    coAccountingParamModels.Add(new CoAccountingParamModel(
                                                    ptInf.PtId,
                                                    date.startDate,
                                                    date.endDate,
                                                    raiinNos: new List<long>(),
                                                    hokenId: ptInf.HokenId,
                                                    miseisanKbn,
                                                    saiKbn,
                                                    misyuKbn,
                                                    seikyuKbn,
                                                    hokenKbn,
                                                    nyukinBase: nyukinBase,
                                                    hakkoDay: hakkoDay,
                                                    memo: memo,
                                                    formFileName: formFileName));
                }
            }
        }
        return _accountingCoReportService.GetAccountingReportingData(hpId, coAccountingParamModels);
    }

    #region private function
    private List<(int startDate, int endDate)> GetDates(DateTime startDateOrigin, DateTime endDateOrigin, int startDate, int endDate, bool printByMonth)
    {
        List<(int startDate, int endDate)> dates = new();
        if (printByMonth)
        {
            int differenceMonth = CountMonth(startDateOrigin, endDateOrigin);
            int lastDayOfMonth = DateTime.DaysInMonth(startDateOrigin.Year, startDateOrigin.Month);
            int lastDateInMonth = startDateOrigin.Year * 10000 + startDateOrigin.Month * 100 + lastDayOfMonth;
            dates.Add((startDate, lastDateInMonth));
            for (int i = 1; i <= differenceMonth; i++)
            {
                if (i == differenceMonth)
                {
                    int firstDateInMonth = endDateOrigin.Year * 10000 + endDateOrigin.Month * 100 + 1;
                    dates.Add((firstDateInMonth, endDate));
                }
                else
                {
                    DateTime nexttimeInMonth = startDateOrigin.AddMonths(i);
                    int nextStartDateInMonth = nexttimeInMonth.Year * 10000 + nexttimeInMonth.Month * 100 + 1;
                    int nextEndDateInMonth = nexttimeInMonth.Year * 10000 + nexttimeInMonth.Month * 100 + DateTime.DaysInMonth(nexttimeInMonth.Year, nexttimeInMonth.Month);
                    dates.Add((nextStartDateInMonth, nextEndDateInMonth));
                }
            }
        }
        else
        {
            dates.Add((startDate, endDate));
        }
        return dates;
    }

    private int CountMonth(DateTime startD, DateTime endD)
    {
        return 12 * (endD.Year - startD.Year) + endD.Month - startD.Month;
    }

    private List<(long ptId, int hokenId)> GetPtCondition(List<PtInfInputItem> sourcePt, int printSort, bool exportCSV = false)
    {
        List<(long ptId, int hokenId)> ptCoditions = new();
        var listPt = GetPtListBySort(sourcePt, printSort, exportCSV);
        foreach (var item in listPt)
        {
            ptCoditions.Add((item.PtId, 0));
        }
        return ptCoditions;
    }

    private List<PtInfInputItem> GetPtListBySort(List<PtInfInputItem> sourcePt, int printSort, bool exportCSV = false)
    {
        if (printSort == 0)
        {
            sourcePt = sourcePt.OrderBy(item => item.PtNum).ToList();
        }
        else if (printSort == 1)
        {
            sourcePt = sourcePt.OrderBy(item => item.KanaName).ToList();
        }
        if (exportCSV) return sourcePt;
        return sourcePt.GroupBy(p => new { p.PtNum, p.HokenId }).Select(g => g.First()).ToList();
    }
    #endregion
    #endregion

    public CommonExcelReportingModel ExportCsv(int hpId, string menuName, int menuId, int timeFrom, int timeTo, int? monthFrom = 0, int? monthTo = 0, int? dateFrom = 0, int? dateTo = 0, bool? isPutTotalRow = false, int? tenkiDateFrom = -1, int? tenkiDateTo = -1, int? enableRangeFrom = -1, int? enableRangeTo = -1, long? ptNumFrom = 0, long? ptNumTo = 0, bool? isPutColName = false, CoFileType? coFileType = null)
    {
        return _staticsticExportCsvService.ExportCsv(hpId, menuName, menuId, timeFrom, timeTo, monthFrom, monthTo, dateFrom, dateTo, isPutTotalRow, tenkiDateFrom, tenkiDateTo, enableRangeFrom, enableRangeTo, ptNumFrom, ptNumTo, isPutColName, coFileType);
    }

    public (string message, CoPrintExitCode code, List<string> data) OutPutFileSta900(int hpId, List<string> outputColumns, bool isPutColName, CoSta9000PtConf? ptConf, CoSta9000HokenConf? hokenConf, CoSta9000ByomeiConf? byomeiConf, CoSta9000RaiinConf? raiinConf, CoSta9000SinConf? sinConf, CoSta9000KarteConf? karteConf, CoSta9000KensaConf? kensaConf, List<long> ptIds, int sortOrder, int sortOrder2, int sortOrder3)
    {
        return _sta9000CoReportService.OutPutFile(hpId, outputColumns, isPutColName, ptConf, hokenConf, byomeiConf, raiinConf, sinConf, karteConf, kensaConf, ptIds, sortOrder, sortOrder2, sortOrder3);
    }

    public CommonReportingRequestModel GetKensaHistoryPrint(int hpId, int userId, long ptId, int setId, int iraiDate, int startDate, int endDate, bool showAbnormalKbn, int sinDate)
    {
        return _kensaHistoryCoReportService.GetKensaHistoryPrintData(hpId, userId, ptId, setId, iraiDate, startDate, endDate, showAbnormalKbn, sinDate);
    }

    public CommonReportingRequestModel GetKensaResultMultiPrint(int hpId, int userId, long ptId, int setId, int startDate, int endDate, bool showAbnormalKbn, int sinDate)
    {
        return _kensaResultMultiCoReportService.GetKensaResultMultiPrintData(hpId, userId, ptId, setId, startDate, endDate, showAbnormalKbn, sinDate);
    }
}
