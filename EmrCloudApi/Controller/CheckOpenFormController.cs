using EmrCloudApi.Constants;
using EmrCloudApi.Requests.ExportPDF;
using EmrCloudApi.Responses;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using Reporting.Accounting.Model;
using Reporting.CommonMasters.Enums;
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

    [HttpGet(ApiPath.ExportOrderLabel)]
    public ActionResult<Response<bool>> GenerateOrderLabelReport([FromQuery] OrderLabelExportRequest request)
    {
        List<(int from, int to)> odrKouiKbns = new();
        foreach (var item in request.OdrKouiKbns)
        {
            odrKouiKbns.Add(new(item.From, item.To));
        }
        var data = _checkOpenReportingService.CheckOpenOrderLabel(0, request.HpId, request.PtId, request.SinDate, request.RaiinNo, odrKouiKbns, new());
        return ConvertToResponse(data);
    }

    [HttpGet(ApiPath.SyojyoSyoki)]
    public ActionResult<Response<bool>> GenerateSyojyoSyokiReport([FromQuery] SyojyoSyokiRequest request)
    {
        try
        {
            var status = _checkOpenReportingService.CheckOpenSyojyoSyokiReportStatus(HpId, request.PtId, request.SeiKyuYm, 0, request.HokenId);

            return ConvertToResponse(status);
        }
        finally
        {
            _checkOpenReportingService.ReleaseResource();
        }
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

    private Response<bool> ConvertToResponse(CoPrintExitCode code)
    {
        Response<bool> result = new();
        result.Data = code == CoPrintExitCode.EndSuccess;
        result.Message = GetMessage(code);
        result.Status = (int)code;
        return result;
    }

    private string GetMessage(CoPrintExitCode code) => code switch
    {
        CoPrintExitCode.EndSuccess => ResponseMessage.Success,
        CoPrintExitCode.EndNoData => ResponseMessage.Failed,
        CoPrintExitCode.None => ResponseMessage.Failed,
        CoPrintExitCode.EndInvalidArg => ResponseMessage.Failed,
        CoPrintExitCode.EndDirectoryNotFound => ResponseMessage.Failed,
        CoPrintExitCode.EndError => ResponseMessage.Failed,
        CoPrintExitCode.EndFormFileNotFound => ResponseMessage.Failed,
        CoPrintExitCode.EndTemplateNotFound => ResponseMessage.Failed,
        _ => string.Empty
    };
    #endregion
}
