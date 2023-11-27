using Reporting.Accounting.DB;
using Reporting.Accounting.Model;
using Reporting.Accounting.Service;
using Reporting.CommonMasters.Enums;
using Reporting.ReceiptCheck.Service;

namespace Reporting.ReportServices;

public class CheckOpenReportingService : ICheckOpenReportingService
{
    private readonly ICoAccountingFinder _coAccountingFinder;
    private readonly IAccountingCoReportService _accountingCoReportService;
    private readonly IReportService _reportService;
    private readonly IReceiptCheckCoReportService _receiptCheckCoReportService;

    public CheckOpenReportingService(ICoAccountingFinder coAccountingFinder, IAccountingCoReportService accountingCoReportService, IReportService reportService, IReceiptCheckCoReportService receiptCheckCoReportService)
    {
        _coAccountingFinder = coAccountingFinder;
        _accountingCoReportService = accountingCoReportService;
        _reportService = reportService;
        _receiptCheckCoReportService = receiptCheckCoReportService;
    }

    public bool CheckOpenAccountingForm(int hpId, long ptId, int printTypeInput, List<long> raiinNoList, List<long> raiinNoPayList, bool isCalculateProcess = false)
    {
        try
        {
            return _accountingCoReportService.CheckOpenReportingForm(hpId, ptId, printTypeInput, raiinNoList, raiinNoPayList, isCalculateProcess);
        }
        finally
        {
            _coAccountingFinder.ReleaseResource();
        }
    }

    public bool CheckExistTemplate(string templateName, int printType)
    {
        try
        {
            return _accountingCoReportService.CheckExistTemplate(templateName, printType);
        }
        finally
        {
            _coAccountingFinder.ReleaseResource();
        }
    }

    public bool CheckOpenAccountingForm(int hpId, ConfirmationMode mode, long ptId, List<CoAccountDueListModel> multiAccountDueListModels, bool isPrintMonth, bool ryoshusho, bool meisai)
    {
        try
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
                        var printItem = _reportService.PrintWithoutThread(ryoshusho, meisai, mode, ptId, accountDueListModels, selectedAccountDueListModel, isPrintMonth, model.SinDate, model.OyaRaiinNo, accountDueListModels);
                        requestAccountting.AddRange(printItem);
                        months.Add(model.SinDate / 100);
                    }
                }
                else
                {
                    var printItem = _reportService.PrintWithoutThread(ryoshusho, meisai, mode, ptId, accountDueListModels, selectedAccountDueListModel, isPrintMonth, model.SinDate, model.OyaRaiinNo);
                    requestAccountting.AddRange(printItem);
                }
            }
            return _accountingCoReportService.CheckOpenReportingForm(hpId, requestAccountting);
        }
        finally
        {
            _coAccountingFinder.ReleaseResource();
        }
    }

    public bool CheckOpenReceiptCheck(int hpId, List<long> ptIds, int seikyuYm)
    {
        try
        {
            return _receiptCheckCoReportService.CheckOpenReceiptCheck(hpId, ptIds, seikyuYm);
        }
        finally
        {
            _coAccountingFinder.ReleaseResource();
        }
    }
}
