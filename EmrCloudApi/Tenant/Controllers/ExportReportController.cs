using DevExpress.Response.Karte1;
using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Requests.ExportPDF;
using EmrCloudApi.Tenant.Responses;
using Interactor.ExportPDF;
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
}
