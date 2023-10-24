using EmrCloudApi.Constants;
using EmrCloudApi.Requests.ExportPDF;
using EmrCloudApi.Services;
using Helper.Enum;
using Microsoft.AspNetCore.Mvc;
using Reporting.Accounting.Model;
using Reporting.ReportServices;
using System.Text.Json;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
[ApiController]
public class CheckOpenFormController : AuthorizeControllerBase
{
    private readonly ICheckOpenReportingService _checkOpenReportingService;

    public CheckOpenFormController(ICheckOpenReportingService checkOpenReportingService, IUserService userService) : base(userService)
    {
        _checkOpenReportingService = checkOpenReportingService;
    }

    [HttpPost(ApiPath.AccountingReport)]
    public IActionResult CheckOpenReportingForm([FromForm] AccountingReportRequest requestStringJson)
    {
        var stringJson = requestStringJson.JsonAccounting;
        var request = JsonSerializer.Deserialize<AccountingCoReportModelRequest>(stringJson) ?? new();
        var multiAccountDueListModels = request.MultiAccountDueListModels.Select(item => ConvertToCoAccountDueListModel(item)).ToList();

        var data = _checkOpenReportingService.CheckOpenAccountingForm(HpId, request.Mode, request.PtId, multiAccountDueListModels, request.IsPrintMonth, request.Ryoshusho, request.Meisai);
        return Ok(data);
    }

    [HttpGet(ApiPath.ReceiptReport)]
    public IActionResult GenerateReceiptReport([FromQuery] ReceiptExportRequest request)
    {
        var data = _checkOpenReportingService.CheckOpenAccountingForm(HpId, request.PtId, request.PrintType, request.RaiinNoList, request.RaiinNoPayList, request.IsCalculateProcess);
        return Ok(data);
    }

    [HttpGet(ApiPath.ReceiptCheck)]
    public IActionResult CheckOpenReceiptCheck([FromQuery] ReceiptCheckRequest request)
    {
        var data = _checkOpenReportingService.CheckOpenReceiptCheck(request.HpId, request.PtIds, request.SeikyuYm);
        return Ok(data);
    }

    [HttpPost(ApiPath.CheckExistTemplateAccounting)]
    public IActionResult CheckExistTemplateAccounting([FromBody] CheckExistTemplateAccountingRequest request)
    {
        var data = _checkOpenReportingService.CheckExistTemplate(request.TemplateName, request.PrintType);
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
