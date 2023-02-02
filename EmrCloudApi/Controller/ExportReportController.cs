using EmrCloudApi.Constants;
using EmrCloudApi.Requests.ExportPDF;
using EmrCloudApi.Services;
using Microsoft.AspNetCore.Mvc;
using Reporting.Interface;
using Reporting.Karte1.Model;
using Reporting.Model.ExportKarte1;
using Reporting.NameLabel.Models;

namespace EmrCloudApi.Controller;

[Route("api/[controller]")]
public class ExportReportController : AuthorizeControllerBase
{
    private readonly IReportService _report;
    public ExportReportController(IUserService userService, IReportService report) : base(userService)
    {
        _report = report;
    }

    [HttpGet("ExportNameLabel11")]
    public ActionResult<Karte1ExportModel> ExportKarte1([FromQuery] Karte1ExportRequest request)
    {
        return _report.GetDataKarte1(HpId, request.PtId, request.SinDate, request.HokenPid, request.TenkiByomei, request.SyuByomei);
    }

    [HttpGet(ApiPath.ExportKarte1)]
    public ActionResult<CoKarte1Model> ExportKarte1w([FromQuery] Karte1ExportRequest request)
    {
        return _report.GetKarte1ReportingData(HpId, request.PtId, request.SinDate, request.HokenPid, request.TenkiByomei, request.SyuByomei);
    }

    [HttpGet("ExportNameLabel")]
    public ActionResult<CoNameLabelModel> ExportNameLabel([FromQuery] NameLabelRequest request)
    {
        return _report.GetNameLabelReportingData(request.PtId, request.KanjiName, request.SinDate);
    }
}
