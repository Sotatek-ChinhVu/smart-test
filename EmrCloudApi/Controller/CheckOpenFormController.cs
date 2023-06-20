using EmrCloudApi.Constants;
using EmrCloudApi.Requests.ExportPDF;
using Microsoft.AspNetCore.Mvc;
using Reporting.Accounting.Model;
using Reporting.ReportServices;
using System.Text.Json;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class CheckOpenFormController : ControllerBase
{
    private readonly ICheckOpenReportingService _checkOpenReportingService;

    public CheckOpenFormController(ICheckOpenReportingService checkOpenReportingService)
    {
        _checkOpenReportingService = checkOpenReportingService;
    }

    [HttpPost(ApiPath.AccountingReport)]
    public IActionResult CheckOpenReportingForm([FromForm] AccountingReportRequest requestStringJson)
    {
        var stringJson = requestStringJson.JsonAccounting;
        var request = JsonSerializer.Deserialize<AccountingCoReportModelRequest>(stringJson) ?? new();
        var multiAccountDueListModels = request.MultiAccountDueListModels.Select(item => ConvertToCoAccountDueListModel(item)).ToList();

        var data = _checkOpenReportingService.CheckOpenAccountingForm(request.HpId, request.Mode, request.PtId, multiAccountDueListModels, request.IsPrintMonth, request.Ryoshusho, request.Meisai);
        return Ok(data);
    }

    [HttpGet(ApiPath.ReceiptReport)]
    public IActionResult GenerateReceiptReport([FromQuery] ReceiptExportRequest request)
    {
        var data = _checkOpenReportingService.CheckOpenAccountingForm(request.HpId, request.PtId, request.PrintType, request.RaiinNoList, request.RaiinNoPayList, request.IsCalculateProcess);
        return Ok(data);
    }

    #region Private function
    private CoAccountDueListModel ConvertToCoAccountDueListModel(CoAccountDueListRequestModel request)
    {
        return new CoAccountDueListModel(
                   request.SinDate,
                   0,
                   request.RaiinNo,
                   request.OyaRaiinNo
               );
    }
    #endregion
}
