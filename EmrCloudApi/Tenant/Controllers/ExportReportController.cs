using DevExpress.Response.Karte1;
using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Requests.ExportPDF;
using EmrCloudApi.Tenant.Responses;
using Interactor.ExportPDF;
using Interactor.ExportPDF.Karte2;
using Microsoft.AspNetCore.Mvc;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExportReportController : ControllerBase
{
    private readonly IReporting _reporting;

    public ExportReportController(IReporting reporting)
    {
        _reporting = reporting;
    }

    [HttpGet(ApiPath.ExportKarte1)]
    public async Task<ActionResult<Response<string>>> GetList([FromQuery] Karte1ExportRequest request)
    {
        var output = await Task.Run(() => _reporting.PrintKarte1(request.HpId, request.PtId, request.SinDate, request.HokenPid, request.TenkiByomei));
        Response<string> response = new();
        response.Data = Convert.ToBase64String(output.DataStream.ToArray());
        response.Status = (byte)output.Status;
        response.Message = GetMessageKarte1(output.Status);

        return response;
    }

    [HttpPost(ApiPath.ExportKarte2)]
    public ActionResult<Response<string>> GetListKarte2([FromBody] Karte2ExportRequest request)
    {
        var output = _reporting.PrintKarte2(ConvertToKarte2ExportInput(request));

        Response<string> response = new();
        response.Data = Convert.ToBase64String(output.DataStream.ToArray());
        response.Status = (byte)output.Status;
        response.Message = GetMessageKarte2(output.Status);

        return response;
    }

    private string GetMessageKarte1(Karte1Status status) => status switch
    {
        Karte1Status.Success => ResponseMessage.Success,
        Karte1Status.PtInfNotFould => ResponseMessage.PtInfNotFould,
        Karte1Status.InvalidSindate => ResponseMessage.InvalidSinDate,
        Karte1Status.InvalidHpId => ResponseMessage.InvalidHpId,
        Karte1Status.HokenNotFould => ResponseMessage.HokenNotFould,
        Karte1Status.Failed => ResponseMessage.Failed,
        Karte1Status.CanNotExportPdf => ResponseMessage.CanNotExportPdf,
        _ => string.Empty
    };

    private string GetMessageKarte2(Karte2Status status) => status switch
    {
        Karte2Status.Success => ResponseMessage.Success,
        Karte2Status.Failed => ResponseMessage.Failed,
        Karte2Status.CanNotExportPdf => ResponseMessage.CanNotExportPdf,
        _ => string.Empty
    };

    private Karte2ExportInput ConvertToKarte2ExportInput(Karte2ExportRequest request)
    {
        return new Karte2ExportInput(
                request.PtId,
                request.HpId,
                request.UserId,
                request.SinDate,
                request.StartDate,
                request.EndDate,
                request.IsCheckedHoken,
                request.IsCheckedJihi,
                request.IsCheckedHokenJihi,
                request.IsCheckedJihiRece,
                request.IsCheckedHokenRousai,
                request.IsCheckedHokenJibai,
                request.IsCheckedDoctor,
                request.IsCheckedStartTime,
                request.IsCheckedVisitingTime,
                request.IsCheckedEndTime,
                request.IsUketsukeNameChecked,
                request.IsCheckedSyosai,
                request.IsIncludeTempSave,
                request.IsCheckedApproved,
                request.IsCheckedInputDate,
                request.IsCheckedSetName,
                request.DeletedOdrVisibilitySetting,
                request.IsIppanNameChecked,
                request.IsCheckedHideOrder
            );
    }
}
