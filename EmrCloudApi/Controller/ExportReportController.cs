using EmrCloudApi.Constants;
using EmrCloudApi.Requests.ExportPDF;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using Reporting.Interface;
using Reporting.Model.ExportKarte1;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class ExportReportController : AuthorizeControllerBase
{
    private readonly IReportingService _reporting;
    public ExportReportController(IUserService userService, IReportingService reporting) : base(userService)
    {
        _reporting = reporting;
    }

    [HttpGet(ApiPath.ExportKarte1)]
    public ActionResult<Karte1ExportModel> ExportKarte1([FromQuery] Karte1ExportRequest request)
    {
        return _reporting.GetDataKarte1(HpId, request.PtId, request.SinDate, request.HokenPid, request.TenkiByomei);
    }
}
