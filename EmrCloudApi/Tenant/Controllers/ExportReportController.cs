using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.ExportPDF;
using EmrCloudApi.Tenant.Requests.ExportPDF;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.ExportPDF;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.ExportPDF.ExportKarte1;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExportReportController : ControllerBase
{
    private readonly UseCaseBus _bus;
    public ExportReportController(UseCaseBus bus)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.ExportKarte1)]
    public ActionResult<Response<Karte1ExportResponse>> GetList([FromQuery] Karte1ExportRequest request)
    {
        var input = new ExportKarte1InputData(request.HpId, request.PtId, request.SinDate, request.HokenPid, request.TenkiByomei);
        var output = _bus.Handle(input);

        var presenter = new Karte1ExportPresenter();
        presenter.Complete(output);

        return new ActionResult<Response<Karte1ExportResponse>>(presenter.Result);
    }
}
