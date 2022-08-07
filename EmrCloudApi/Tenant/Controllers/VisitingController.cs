using Domain.Models.Reception;
using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.Reception;
using EmrCloudApi.Tenant.Requests.Reception;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Reception;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Reception.GetList;
using UseCase.Reception.UpdateDynamicCell;
using UseCase.Reception.UpdateStaticCell;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VisitingController : ControllerBase
{
    private readonly UseCaseBus _bus;
    private readonly ILogger<VisitingController> _logger;
    public VisitingController(UseCaseBus bus, ILogger<VisitingController> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<List<ReceptionRowModel>>> GetList([FromQuery] GetReceptionListRequest request)
    {
        var input = new GetReceptionListInputData(request.HpId, request.SinDate);
        var output = _bus.Handle(input);
        var presenter = new GetReceptionListPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpPut(ApiPath.Update + "StaticCell")]
    public ActionResult<Response<UpdateReceptionStaticCellResponse>> UpdateStaticCell([FromBody] UpdateReceptionStaticCellRequest req)
    {
        var input = new UpdateReceptionStaticCellInputData(
            req.HpId, req.SinDate, req.RaiinNo, req.PtId, req.CellName, req.CellValue);
        var output = _bus.Handle(input);
        var presenter = new UpdateReceptionStaticCellPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpPut(ApiPath.Update + "DynamicCell")]
    public ActionResult<Response<UpdateReceptionDynamicCellResponse>> UpdateDynamicCell([FromBody] UpdateReceptionDynamicCellRequest req)
    {
        var input = new UpdateReceptionDynamicCellInputData(
            req.HpId, req.SinDate, req.RaiinNo, req.PtId, req.GrpId, req.KbnCd);
        var output = _bus.Handle(input);
        var presenter = new UpdateReceptionDynamicCellPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }
}
