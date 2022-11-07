using DevExpress.Response.Karte1;
using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Requests.ExportPDF;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Services;
using Interactor.ExportPDF;
using Microsoft.AspNetCore.Mvc;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExportReportController : ControllerBase
{
    private readonly IReporting _reporting;
    private readonly IUserService _userService;

    public ExportReportController(IReporting reporting, IUserService userService)
    {
        _reporting = reporting;
        _userService = userService;
    }

    [HttpGet(ApiPath.ExportKarte1)]
    public async Task<ActionResult<Response<string>>> GetList([FromQuery] Karte1ExportRequest request)
    {
        var validateToken = int.TryParse(_userService.GetLoginUser().HpId, out int hpId);
        if (!validateToken)
        {
            return new ActionResult<Response<string>>(new Response<string> { Status = LoginUserConstant.InvalidStatus, Message = ResponseMessage.InvalidToken });
        }
        var output = await Task.Run(() => _reporting.PrintKarte1(hpId, request.PtId, request.SinDate, request.HokenPid, request.TenkiByomei));
        
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
