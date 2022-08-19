using Domain.Models.Reception;
using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Presenters.Reception;
using EmrCloudApi.Tenant.Presenters.VisitingList;
using EmrCloudApi.Tenant.Requests.Reception;
using EmrCloudApi.Tenant.Requests.VisitingList;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Reception;
using EmrCloudApi.Tenant.Responses.VisitingList;
using Microsoft.AspNetCore.Mvc;
using UseCase.Core.Sync;
using UseCase.Reception.GetList;
using UseCase.Reception.GetSettings;
using UseCase.Reception.UpdateDynamicCell;
using UseCase.Reception.UpdateStaticCell;
using UseCase.VisitingList.SaveSettings;

namespace EmrCloudApi.Tenant.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VisitingController : ControllerBase
{
    private readonly UseCaseBus _bus;

    public VisitingController(UseCaseBus bus, ILogger<VisitingController> logger)
    {
        _bus = bus;
    }

    [HttpGet(ApiPath.GetList)]
    public ActionResult<Response<GetReceptionListResponse>> GetList([FromQuery] GetReceptionListRequest request)
    {
        var input = new GetReceptionListInputData(request.HpId, request.SinDate);
        var output = _bus.Handle(input);
        var presenter = new GetReceptionListPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpGet(ApiPath.Get + "Settings")]
    public ActionResult<Response<GetReceptionSettingsResponse>> GetSettings([FromQuery] GetReceptionSettingsRequest req)
    {
        var input = new GetReceptionSettingsInputData(req.UserId);
        var output = _bus.Handle(input);
        var presenter = new GetReceptionSettingsPresenter();
        presenter.Complete(output);
        return Ok(presenter.Result);
    }

    [HttpPost("SaveSettings")]
    public ActionResult<Response<SaveVisitingListSettingsResponse>> SaveSettings([FromBody] SaveVisitingListSettingsRequest req)
    {
        var input = new SaveVisitingListSettingsInputData(req.UserId, req.Settings);
        var output = _bus.Handle(input);
        var presenter = new SaveVisitingListSettingsPresenter();
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
