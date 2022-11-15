using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Requests.ExportPDF;
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
    public IActionResult ReturnStream([FromQuery] Karte1ExportRequest request)
    {
        var output = _reporting.PrintKarte1(request.HpId, request.PtId, request.SinDate, request.HokenPid, request.TenkiByomei);
        return File(output.DataStream.ToArray(), "application/pdf");
    }
}